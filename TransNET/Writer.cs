using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TransNET.Types;

namespace TransNET
{
    public class Writer
    {
        public string Write(object obj)
        {
            var o = WriteTopLevel(obj);
            var json = JsonConvert.SerializeObject(o);
            return json;
        }

        private object WriteTopLevel(object o)
        {
            if (!(o is string) && o is IEnumerable) return WriteToken(o);
            else return WriteSingleWrapper(o);
        }

        private object WriteToken(object o)
        {
            if (o == null || o is bool || o is string || o is int) return o;
            else if (o is Array) return WriteArray((Array)o);
            else if (o is Keyword) return WriteKeyword((Keyword)o);
            else if (o is Symbol) return WriteSymbol((Symbol)o);
            else if (o is DateTime) return WriteDateTime((DateTime)o);
            Debugger.Break();
            throw new Exception();
        }

        private IEnumerable WriteArray(Array array)
        {
            var l = new List<object>();
            foreach (var i in array)
            {
                var t = WriteToken(i);
                l.Add(t); 
            }
            return l;
        }

        private string WriteDateTime(DateTime dt)
        {
            var offset = dt.ToUniversalTime().Subtract(Globals.UnixEpoch).TotalMilliseconds;
            return $"~m{offset}";
        }

        private string WriteSymbol(Symbol symbol)
        {
            return $"~${symbol.name}";
        }

        private string WriteKeyword(Keyword keyword)
        {
            return $"~:{keyword.name}";
        }

        private object[] WriteSingleWrapper(object o)
        {
            var token = WriteToken(o);
            return new[] { "~#'", token};
        }
    }
}
