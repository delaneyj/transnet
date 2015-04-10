using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TransNET
{
    internal class Cache
    {
        private HashSet<string> cacheSet = new HashSet<string>();
        private Dictionary<int, string> cache = new Dictionary<int, string>();
        private Dictionary<string, int> reverseCache = new Dictionary<string, int>();
        private int nextCacheIndex = 0;
        private const byte CACHE_CODE_DIGITS = 44;
        private const byte BASE_CHAR_INDEX = 48;
        private const char SUB_STR = '^';
        private const int MAX_ENTRIES = CACHE_CODE_DIGITS * CACHE_CODE_DIGITS;

        internal void Clear()
        {
            nextCacheIndex = 0;
            cache.Clear();
            reverseCache.Clear();
            cacheSet.Clear();            
        }

        internal string Upsert(string keyCache)
        {
            dynamic key;
            if (keyCache.StartsWith("^"))
            {
                var rest = keyCache.Substring(1);
                var index = CodeToIndex(rest);
                key = cache[index];
            }
            else
            {
                if (keyCache.StartsWith("~~")) keyCache = keyCache.Substring(1);
                
                if (!cacheSet.Contains(keyCache) && keyCache.Length > 3)
                {
                    if (nextCacheIndex >= MAX_ENTRIES) Clear();
                    cache.Add(nextCacheIndex++, keyCache);
                    cacheSet.Add(keyCache);    
                }
                key = keyCache;
            }
            return key;
        }

        readonly string[] mustNotEscape = "#_s?idb:$fnmturc'zX".Select(c => $"~{c}").ToList().ToArray();
        readonly string[] mustEscape = new [] { "~","^","`"};

        internal string Shorten(string s)
        {
            if (!mustNotEscape.Any(c => s.StartsWith(c)))
            {
                if (mustEscape.Any(c => s.StartsWith(c)))
                {
                    s = $"~{s}";
                }
            }

            if (s.Length <= 3) return s;

            if (!cacheSet.Contains(s))
            {
                if (nextCacheIndex >= MAX_ENTRIES) Clear();
                cacheSet.Add(s);
                reverseCache.Add(s, nextCacheIndex++);
                return s;
            }
            else
            {
                var index = reverseCache[s];
                var code = IndexToCode(index);
                return code;
            }
        }

        private string IndexToCode(int index)
        {
            var hi = index / CACHE_CODE_DIGITS;
            var lo = index % CACHE_CODE_DIGITS;
            
            if (hi == 0)
            {
                return $"{SUB_STR}{Convert.ToChar(lo + BASE_CHAR_INDEX)}";
            }
            else
            {
                return $"{SUB_STR}{Convert.ToChar(hi + BASE_CHAR_INDEX)}{Convert.ToChar(lo + BASE_CHAR_INDEX)}";
            }
        }

        private int CodeToIndex(string s)
        {
            if (s.Length == 1)
            {
                return (Convert.ToInt16(s[0]) - BASE_CHAR_INDEX);
            }
            else
            {
                var a = Convert.ToInt16(s[0]);
                var b = Convert.ToInt16(s[1]);
                var x = a - BASE_CHAR_INDEX;
                var y = x * CACHE_CODE_DIGITS;
                var z = b - BASE_CHAR_INDEX;
                return y + z;
            }
        }
    }
}
