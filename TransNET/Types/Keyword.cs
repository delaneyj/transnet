using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransNET.Types
{
    public struct Keyword
    {
        internal readonly string name;
        public Keyword(string keyword)
        {
            this.name = keyword.StartsWith(":") ? keyword.Substring(1) : keyword; 
        }
        public override string ToString()
        {
            return $":{name}";
        }
    }
}
