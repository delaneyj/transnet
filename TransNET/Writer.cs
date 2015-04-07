using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TransNET.Types;

namespace TransNET
{
    public class Writer
    {
        public string Write(dynamic obj)
        {
            var o = WriteTopLevel(obj);
            var json = JsonConvert.SerializeObject(o);
            return json;
        }

        private object WriteTopLevel(dynamic o)
        {
            if (!(o is string) && o is IEnumerable) return Write(o);
            else return WriteSingleWrapper(o);
        }

        private object WriteSingleWrapper(dynamic o)
        {
            var token = Write(o);
            return new[] { "~#'", token };
        }

        private int Write(int i)
        {
            return i;
        }

        private long Write(long l)
        {
            return l;
        }

        private object Write(BigInteger bi)
        {
            var s = bi.ToString();

            var bigN = bi;
            var bitLength = 0;
            do
            {
                bitLength++;
                bigN /= 2;
            } while (bigN != BigInteger.Zero);

            if (bitLength < 32) return (int)bi;
            else if (bitLength < 53) return (long)bi;
            else if (bitLength <= 64) return $"~i{s}";
            else return $"~n{s}";
        }

        private object Write(double d)
        {
            if (double.IsPositiveInfinity(d)) return "~zINF";
            else if (double.IsNegativeInfinity(d)) return "~z-INF";
            else if (double.IsNaN(d)) return "~zNaN";
            return d;
        }

        

        private bool Write(bool b)
        {
            return b;
        }

        private string Write(string s)
        {
            return s;
        }

        private string Write(Keyword k)
        {
            return $"~:{k.name}";
        }

        private string Write(Symbol s) {
            return $"~${s.name}";
        }

        private string Write(DateTime dt)
        {
            var offset = dt.ToUniversalTime().Subtract(Globals.UnixEpoch).TotalMilliseconds;
            return $"~m{offset}";
        }

        private string Write(Uri uri)
        {
            return $"~r{uri.OriginalString}";
        }

        private string Write(Guid guid)
        {
            return $"~u{guid}";
        }

        private IEnumerable Write(IEnumerable enumerable) {
            var l = new List<object>();
            foreach (dynamic i in enumerable)
            {
                var t = Write(i);
                l.Add(t);
            }
            return l;
        }
    }
}
