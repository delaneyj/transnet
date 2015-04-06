using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransNET
{
    internal class Cache
    {
        private HashSet<string> cacheSet = new HashSet<string>();
        private Dictionary<int, string> cache = new Dictionary<int, string>();
        private int nextCacheIndex = 0;
        private const int CACHE_CODE_DIGITS = 44;
        private const int BASE_CHAR_INDEX = 48;
        private const char SUB_STR = '^';
        private const int MAX_ENTRIES = CACHE_CODE_DIGITS * CACHE_CODE_DIGITS;

        internal void Clear()
        {
            nextCacheIndex = 0;
            cache.Clear();
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
