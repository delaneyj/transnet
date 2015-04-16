using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TransNET.Types;

namespace TransNET
{
    public class Writer
    {
        private Cache cache = new Cache();
        private static readonly long MAX_53_BIT = (long)Math.Pow(2, 53);

        public string WriteJSON(dynamic obj, bool verbose = false)
        {
            cache.Clear();
            var o = WriteTopLevel(obj, verbose);
            var json = JsonConvert.SerializeObject(o);
            return json;
        }

        private object WriteTopLevel(dynamic o, bool verbose)
        {
            if((!(o is string) && o is IEnumerable) || Utils.IsAnonymousType(o)) return WriteToken(o, verbose, false);
            else return WriteSingleWrapper(o, verbose);
        }

        private object WriteSingleWrapper(dynamic o, bool verbose)
        {
            var token = WriteToken(o, verbose, false);
            return new[] { "~#'", token };
        }

        private object WriteToken(dynamic o, bool verbose, bool isMapKey)
        {
            if ((object)o == null) return isMapKey ? "~_" : null;

            if (o is bool) return isMapKey ? "~?" + (((bool)o) ? 't' : 'f') : o;
            else if (o is byte[]) return $"~b{Convert.ToBase64String(o)}";
            else if (o is Uri) return cache.Shorten($"~r{((Uri)o).OriginalString}");
            else if (o is Guid) return cache.Shorten($"~u{((Guid)o)}");
            else if (o is Keyword) return cache.Shorten($"~:{((Keyword)o).name}");
            else if (o is Symbol) return cache.Shorten($"~${((Symbol)o).name}");
            else if (o is DateTime)
            {
                var offset = ((DateTime)o).ToUniversalTime().Subtract(Globals.UnixEpoch).TotalMilliseconds;
                return cache.Shorten($"~m{offset}");
            }
            else if (o is float || o is double)
            {
                return WriteFloatingPoint(o, verbose, isMapKey);
            }
            else if(Utils.IsAnonymousType(o))
            {
                var map = new Dictionary<string, object>();
                foreach(var propertyInfo in o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    var name = propertyInfo.Name;
                    var value = propertyInfo.GetValue(o, null);
                    map.Add(name,value);
                }
                return WriteToken(map, verbose, false);
            }
            else
            {
                BigInteger bi;
                try
                {
                    bi = (BigInteger)o;
                }
                catch(Exception)
                {
                    bi = new BigInteger((long)o);
                }
                return WriteBigInteger(bi, verbose, isMapKey);

            }
            throw new NotImplementedException();
        }

        private object WriteToken<T>(IEnumerable<T> o, bool verbose, bool isMapKey)
        {
            if (o is string)
            {
                return cache.Shorten(o as string);
            }
            else if (o is Array)
            {
                var l = new List<object>();
                var array = o as Array;
                foreach (dynamic i in array)
                {
                    var t = WriteToken(i, verbose, false);
                    l.Add(t);
                }
                return l.ToArray();
            }
            else if (o is ISet<T>)
            {
                var prefix = cache.Shorten("~#set");
                var set = (ISet<T>)o;
                var array = WriteToken(set.ToArray(), verbose, false);
                return new object[] { prefix, array };
            }
            else if (o is IList<T>)
            {
                var prefix = cache.Shorten("~#list");
                var list = (IList<T>)o;
                var array = WriteToken(list.ToArray(), verbose, false);
                return new object[] { prefix, array };
            }
            else if (o is IDictionary)
            {
                var map = (IDictionary)o;

                var keyType = map.GetType().GetGenericArguments()[0];
                var isComposite = !(keyType == typeof(string)) && keyType.GetInterfaces().Contains(typeof(IEnumerable));

                var list = new List<object>();
                foreach (dynamic kvp in o)
                {
                    var k = WriteToken(kvp.Key, verbose, true);
                    var v = WriteToken(kvp.Value, verbose, false);

                    list.Add(k);
                    list.Add(v);
                }

                if (isComposite)
                {
                    return new object[] { "~#cmap", list.ToArray() };
                }
                else
                {
                    list.Insert(0,"^ ");
                    return list.ToArray();
                }
            }
            else
            {
                Debugger.Break();
            }

            throw new NotImplementedException();
        }

        private object WriteFloatingPoint(double d, bool versbose, bool isMapKey)
        {
            if (double.IsPositiveInfinity(d)) return "~zINF";
            else if (double.IsNegativeInfinity(d)) return "~z-INF";
            else if (double.IsNaN(d)) return "~zNaN";
            else if (isMapKey) return $"~d{d}";
            else return d;
        }

        private object WriteBigInteger(BigInteger bi, bool verbose, bool isMapKey)
        {
            var shouldAddPrefix = isMapKey;

            var abs = BigInteger.Abs(bi);
            if (!isMapKey)
            {
                if (abs <= int.MaxValue && abs >= int.MinValue) return (int)bi;
                else if (abs < MAX_53_BIT) return (long)bi;
            }

            if (bi <= long.MaxValue && bi >= long.MinValue) return $"~i{bi}";
            else return $"~n{bi}";
        }
    }
}
