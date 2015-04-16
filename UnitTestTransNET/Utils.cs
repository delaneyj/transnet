using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransNET;

namespace UnitTestTransNET
{
    public static class Utils
    {
        public static string LoadExample(string name)
        {
            var path = $"examples/{name}.json";
            var contents = File.ReadAllText(path);
            return contents;
        }

        internal static void RunWrite(string jsonName, object actual)
        {
            var writer = new Writer();
            var actualJson = writer.WriteJSON(actual);
            var expectedJson = LoadExample(jsonName);
            Assert.AreEqual(expectedJson, actualJson);
        }

        public static void RunRead(string jsonName, Action<dynamic> withResult)
        {
            withResult(Read(Utils.LoadExample(jsonName)));
            withResult(Read(Utils.LoadExample($"{jsonName}.verbose")));
        }

        private static dynamic Read(string contents)
        {
            var reader = new Reader();
            return reader.ReadJSON(contents);
        }
    }
}
