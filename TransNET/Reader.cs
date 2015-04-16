using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Numerics;
using System.Reflection;

namespace TransNET
{
    public class Reader
    {
        Cache cache = new Cache();

        public Reader()
        {
            //JsonConvert.DeserializeObject<TestParse>()
        }

        public dynamic ReadJSON(string json)
        {
            cache.Clear();
            var o = (JToken)JsonConvert.DeserializeObject(json);
            var transitParsed = ParseToken(o);
            return transitParsed;
        }

        public T Parse<T>(string transitJson) where T : new()
        {
            //TODO This is a dirty hack until I get a chance to really dig into how JSON.NET is automagically parse to a class.
            var parsed = ReadJSON(transitJson);
            var actualJson = JsonConvert.SerializeObject(parsed);
            var o = JsonConvert.DeserializeObject<T>(actualJson);
            return o;
        }

        private dynamic ParseToken(JToken jtoken)
        {
            switch (jtoken.Type)
            {
            case JTokenType.Array: return ParseArray(jtoken);
            case JTokenType.Integer: return ParseNumber(jtoken);
            case JTokenType.Float: return jtoken.Value<double>();
            case JTokenType.Boolean: return jtoken.Value<bool>();
            case JTokenType.Date: return jtoken.Value<DateTime>();
            case JTokenType.String: return ParseString(jtoken);
            case JTokenType.Object: return ParseObject(jtoken);
            case JTokenType.Null: return null;
            default: throw new Exception();
            }
        }

        private dynamic ParseObject(JToken jtoken)
        {
            var result = new Dictionary<dynamic, dynamic>();
            var o = jtoken as JObject;
            foreach (var kvp in o)
            {
                var possibleKey = ParseKey(kvp);
                if (kvp.Key.StartsWith("~#")) return possibleKey;

                var key = possibleKey;
                var value = ParseToken(kvp.Value);
                result.Add(possibleKey, value);
            }
            return result;
        }
        
        private dynamic ParseKey(KeyValuePair<string, JToken> kvp)
        {
            if (kvp.Key.StartsWith("~#"))
            {
                var prefix = kvp.Key.Substring(2);
                switch (prefix)
                {
                case "'":
                    return ParseToken(kvp.Value);
                case "set":
                    var set = new HashSet<dynamic>();
                    cache.Upsert(kvp.Key);
                    foreach (var i in kvp.Value)
                    {
                        set.Add(ParseToken(i));
                    }
                    return (ISet<dynamic>)set;
                case "list":
                    var l = new List<dynamic>();
                    cache.Upsert(kvp.Key);
                    foreach (var i in kvp.Value)
                    {
                        l.Add(ParseToken(i));
                    }
                    return l.AsEnumerable();
                case "cmap":
                    var d = new Dictionary<dynamic, dynamic>();
                    var pairs = kvp.Value.Select((v, i) => new { Index = i, Value = v })
                                    .GroupBy(x => x.Index / 2);
                    foreach (var p in pairs)
                    {
                        var k = p.ElementAt(0).Value;
                        var v = p.ElementAt(1).Value;

                        var key = ParseToken(k);
                        var value = ParseToken(v);

                        d.Add(key,value);
                    }
                    return d;
                }
            }

            return ParseToken(kvp.Key);
        }

        private dynamic ParseString(JToken jtoken)
        {
            var str = jtoken.Value<string>();
            cache.Upsert(str);

            if (str.Length == 0) return str;

            if (str[0] == '~')
            {
                if (str[1] == '_') return null;
                var rest = str.Substring(2);
                switch (str[1])
                {
                case '?':
                    switch (rest[0])
                    {
                    case 't': return true;
                    case 'f': return false;
                    default: throw new InvalidOperationException();
                    }
                case 'i':
                    return ParseNumber(new JValue(rest));
                case 'd':
                    return float.Parse(rest);
                case 'b':
                    return Convert.FromBase64String(rest);
                case ':':
                    return new Types.Keyword(rest);
                case '$':
                    return new Types.Symbol(rest);
                case 'm':
                    var ms = ParseNumber(new JValue(rest));
                    return Globals.UnixEpoch.AddMilliseconds(ms);
                case 't':
                    return DateTime.Parse(rest).ToUniversalTime();
                case 'u':
                    return Guid.Parse(rest);
                case 'r':
                    return new Uri(rest);
                case 'c':
                    return char.Parse(rest);
                case 'n':
                    return BigInteger.Parse(rest);
                case 'z':
                    switch (rest)
                    {
                    case "NaN": return double.NaN;
                    case "INF": return double.PositiveInfinity;
                    case "-INF": return double.NegativeInfinity;
                    default: throw new Exception();
                    }
                case '#':
                    return str;
                case '~':
                case '^':
                    return str.Substring(1);
                default:
                    throw new Exception();
                }
            }

            var cached = cache.Upsert(str);
            if (cached != str) return ParseToken(cached);
            return cached;
        }

        private dynamic ParseNumber(JToken jvalue)
        {
            var l = jvalue.Value<long>();
            if (l < int.MinValue || l > int.MaxValue) return l;
            else return (int)l;
        }

        private dynamic ParseArray(JToken jtoken)
        {
            var jarray = jtoken as JArray;
            var list = new List<dynamic>();

            if (jarray.Count > 0)
            {
                var f = jarray[0];
                if (f is JValue && f.Type == JTokenType.String)
                {
                    var firstValue = f.Value<string>();
                    if (firstValue == "^ ")
                    {
                        var i = 0;
                        Dictionary<dynamic, dynamic> entry = null;


                        while (i < jarray.Count)
                        {
                            var first = jarray[i++].Value<string>();

                            if (first == null) break;

                            if (first == "^ ") entry = new Dictionary<dynamic, dynamic>();
                            else i--;

                            var k = jarray[i++].Value<string>();
                            if (k.StartsWith("^"))
                            {
                                k = cache.Upsert(k);
                            }
                            var key = ParseToken(k);

                            dynamic value;
                            var valueToken = jarray[i++];
                            var isString = valueToken.Type == JTokenType.String;
                            value = isString ? cache.Upsert(valueToken.Value<string>()) : ParseToken(valueToken);
                            entry.Add(key, value);
                        }

                        return entry;
                    }
                    else
                    {
                        firstValue = cache.Upsert(firstValue);

                        if (firstValue.StartsWith("~#"))
                        { 
                            var prefix = firstValue.Substring(2);
                            if (prefix[0] == '\'') return ParseToken(jarray[1]);
                            else return ParseKey(new KeyValuePair<string, JToken>(firstValue, jarray[1]));
                        }
                    }
                }

                foreach (var t in jarray) list.Add(ParseToken(t));
            }
            return list.ToArray();
        }
    }
}