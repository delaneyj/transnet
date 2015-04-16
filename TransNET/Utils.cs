using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransNET
{
    public static class Utils
    {
        public static bool IsAnonymousType(object o)
        {
            if (o == null) return false;
            var isAnon = o.GetType().Namespace == null;
            return isAnon;
        }
    }
}
