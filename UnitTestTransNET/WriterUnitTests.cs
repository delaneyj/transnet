﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransNET;
using TransNET.Types;

namespace UnitTestTransNET
{
    [TestClass]
    public class WriterUnitTests
    {
        [TestMethod]
        public void Writer_Anonymous_Type()
        {
            var writer = new Writer();
            var json = writer.WriteJSON(new
            {
                Foo = "foo",
                Bar = 11,
                Baz = new[] {1,3,4,5},
                Monkey = new
                {
                    Foo = 12323,
                    Bar = 1232134
                }
            });
            
            Assert.AreEqual(json, "[\"^ \",\"Foo\",\"foo\",\"Bar\",11,\"Baz\",[1,3,4,5],\"Monkey\",[\"^ \",\"Foo\",12323,\"Bar\",1232134]]");
        }

        [TestMethod]
        public void Writer_Null()
        {
            Utils.RunWrite("nil", null);
        }

        [TestMethod]
        public void Writer_Bool_True()
        {
            Utils.RunWrite("true", true);
        }

        [TestMethod]
        public void Writer_Bool_False()
        {
            Utils.RunWrite("false", false);
        }

        [TestMethod]
        public void Writer_Int_Zero()
        {
            Utils.RunWrite("zero", 0);
        }

        [TestMethod]
        public void Writer_Int_One()
        {
            Utils.RunWrite("one", 1);
        }

        [TestMethod]
        public void Writer_String_One()
        {
            Utils.RunWrite("one_string", "hello");
        }

        [TestMethod]
        public void Writer_Keyword_One()
        {
            Utils.RunWrite("one_keyword", new Keyword("hello"));
        }

        [TestMethod]
        public void Writer_Symbol_One()
        {
            Utils.RunWrite("one_symbol", new Symbol("hello"));
        }

        [TestMethod]
        public void Writer_Date_One()
        {
            Utils.RunWrite("one_date", new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc));
        }

        [TestMethod]
        public void Writer_Vectors_Simple()
        {
            Utils.RunWrite("vector_simple", new[] { 1, 2, 3 });
        }

        [TestMethod]
        public void Writer_Vectors_Empty()
        {
            Utils.RunWrite("vector_empty", new object[] { });
        }

        [TestMethod]
        public void Writer_Vectors_Mixed()
        {
            Utils.RunWrite("vector_mixed", new object[] {
                0,1,2.0,true,false,"five",new Keyword("six"),new Symbol("seven"),"~eight",null 
            });
        }

        [TestMethod]
        public void Writer_Vectors_Nested()
        {
            Utils.RunWrite("vector_nested", new object[] {
                new object[] { 1, 2, 3 },
                new object[] { 0,1,2.0,true,false,"five",new Keyword("six"),new Symbol("seven"),"~eight",null }
            });
        }

        [TestMethod]
        public void Writer_String_Small_Vector()
        {
            Utils.RunWrite("small_strings", new[] {
                "", "a", "ab", "abc", "abcd", "abcde", "abcdef"
            });
        }

        [TestMethod]
        public void Writer_String_Tilde_Vector()
        {
            Utils.RunWrite("strings_tilde", new[]
            {
                "~",
                "~a",
                "~ab",
                "~abc",
                "~abcd",
                "~abcde",
                "~abcdef"
            });
        }

        [TestMethod]
        public void Writer_String_Hash_Vector()
        {
            Utils.RunWrite("strings_hash", new[]
            {
                "#",
                "#a",
                "#ab",
                "#abc",
                "#abcd",
                "#abcde",
                "#abcdef"
            });
        }

        [TestMethod]
        public void Writer_String_Hat_Vector()
        {
            Utils.RunWrite("strings_hat", new[]
            {
                "^",
                "^a",
                "^ab",
                "^abc",
                "^abcd",
                "^abcde",
                "^abcdef"
            });
        }

        [TestMethod]
        public void Writer_Int_Small_Vector()
        {
            Utils.RunWrite("vector_simple", new[]
            {
                1, 2, 3
            });
        }

        [TestMethod]
        public void Writer_Int_List()
        {
            Utils.RunWrite("ints", new[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
                16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31,
                32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47,
                48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63,
                64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79,
                80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95,
                96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111,
                112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127
            });
        }

        [TestMethod]
        public void Writer_Int_Interesting()
        {
            var o = new string[] { "-1", "0", "1", "2", "3", "0", "1", "2", "3", "4", "2", "3", "4", "5", "6", "6", "7", "8", "9", "10", "14", "15", "16", "17", "18", "30", "31", "32", "33", "34", "62", "63", "64", "65", "66", "126", "127", "128", "129", "130", "254", "255", "256", "257", "258", "510", "511", "512", "513", "514", "1022", "1023", "1024", "1025", "1026", "2046", "2047", "2048", "2049", "2050", "4094", "4095", "4096", "4097", "4098", "8190", "8191", "8192", "8193", "8194", "16382", "16383", "16384", "16385", "16386", "32766", "32767", "32768", "32769", "32770", "65534", "65535", "65536", "65537", "65538", "131070", "131071", "131072", "131073", "131074", "262142", "262143", "262144", "262145", "262146", "524286", "524287", "524288", "524289", "524290", "1048574", "1048575", "1048576", "1048577", "1048578", "2097150", "2097151", "2097152", "2097153", "2097154", "4194302", "4194303", "4194304", "4194305", "4194306", "8388606", "8388607", "8388608", "8388609", "8388610", "16777214", "16777215", "16777216", "16777217", "16777218", "33554430", "33554431", "33554432", "33554433", "33554434", "67108862", "67108863", "67108864", "67108865", "67108866", "134217726", "134217727", "134217728", "134217729", "134217730", "268435454", "268435455", "268435456", "268435457", "268435458", "536870910", "536870911", "536870912", "536870913", "536870914", "1073741822", "1073741823", "1073741824", "1073741825", "1073741826", "2147483646", "2147483647", "2147483648", "2147483649", "2147483650", "4294967294", "4294967295", "4294967296", "4294967297", "4294967298", "8589934590", "8589934591", "8589934592", "8589934593", "8589934594", "17179869182", "17179869183", "17179869184", "17179869185", "17179869186", "34359738366", "34359738367", "34359738368", "34359738369", "34359738370", "68719476734", "68719476735", "68719476736", "68719476737", "68719476738", "137438953470", "137438953471", "137438953472", "137438953473", "137438953474", "274877906942", "274877906943", "274877906944", "274877906945", "274877906946", "549755813886", "549755813887", "549755813888", "549755813889", "549755813890", "1099511627774", "1099511627775", "1099511627776", "1099511627777", "1099511627778", "2199023255550", "2199023255551", "2199023255552", "2199023255553", "2199023255554", "4398046511102", "4398046511103", "4398046511104", "4398046511105", "4398046511106", "8796093022206", "8796093022207", "8796093022208", "8796093022209", "8796093022210", "17592186044414", "17592186044415", "17592186044416", "17592186044417", "17592186044418", "35184372088830", "35184372088831", "35184372088832", "35184372088833", "35184372088834", "70368744177662", "70368744177663", "70368744177664", "70368744177665", "70368744177666", "140737488355326", "140737488355327", "140737488355328", "140737488355329", "140737488355330", "281474976710654", "281474976710655", "281474976710656", "281474976710657", "281474976710658", "562949953421310", "562949953421311", "562949953421312", "562949953421313", "562949953421314", "1125899906842622", "1125899906842623", "1125899906842624", "1125899906842625", "1125899906842626", "2251799813685246", "2251799813685247", "2251799813685248", "2251799813685249", "2251799813685250", "4503599627370494", "4503599627370495", "4503599627370496", "4503599627370497", "4503599627370498", "9007199254740990", "9007199254740991", "9007199254740992", "9007199254740993", "9007199254740994", "18014398509481982", "18014398509481983", "18014398509481984", "18014398509481985", "18014398509481986", "36028797018963966", "36028797018963967", "36028797018963968", "36028797018963969", "36028797018963970", "72057594037927934", "72057594037927935", "72057594037927936", "72057594037927937", "72057594037927938", "144115188075855870", "144115188075855871", "144115188075855872", "144115188075855873", "144115188075855874", "288230376151711742", "288230376151711743", "288230376151711744", "288230376151711745", "288230376151711746", "576460752303423486", "576460752303423487", "576460752303423488", "576460752303423489", "576460752303423490", "1152921504606846974", "1152921504606846975", "1152921504606846976", "1152921504606846977", "1152921504606846978", "2305843009213693950", "2305843009213693951", "2305843009213693952", "2305843009213693953", "2305843009213693954", "4611686018427387902", "4611686018427387903", "4611686018427387904", "4611686018427387905", "4611686018427387906", "9223372036854775806", "9223372036854775807", "9223372036854775808", "9223372036854775809", "9223372036854775810", "18446744073709551614", "18446744073709551615", "18446744073709551616", "18446744073709551617", "18446744073709551618", "36893488147419103230", "36893488147419103231", "36893488147419103232", "36893488147419103233", "36893488147419103234" };
            var ints = o.Select(i => BigInteger.Parse(i)).ToArray();
            Utils.RunWrite("ints_interesting", ints);
        }

        [TestMethod]
        public void Writer_Int_InterestingNegative()
        {
            var o = new string[] { "-1", "0", "1", "2", "3", "0", "1", "2", "3", "4", "2", "3", "4", "5", "6", "6", "7", "8", "9", "10", "14", "15", "16", "17", "18", "30", "31", "32", "33", "34", "62", "63", "64", "65", "66", "126", "127", "128", "129", "130", "254", "255", "256", "257", "258", "510", "511", "512", "513", "514", "1022", "1023", "1024", "1025", "1026", "2046", "2047", "2048", "2049", "2050", "4094", "4095", "4096", "4097", "4098", "8190", "8191", "8192", "8193", "8194", "16382", "16383", "16384", "16385", "16386", "32766", "32767", "32768", "32769", "32770", "65534", "65535", "65536", "65537", "65538", "131070", "131071", "131072", "131073", "131074", "262142", "262143", "262144", "262145", "262146", "524286", "524287", "524288", "524289", "524290", "1048574", "1048575", "1048576", "1048577", "1048578", "2097150", "2097151", "2097152", "2097153", "2097154", "4194302", "4194303", "4194304", "4194305", "4194306", "8388606", "8388607", "8388608", "8388609", "8388610", "16777214", "16777215", "16777216", "16777217", "16777218", "33554430", "33554431", "33554432", "33554433", "33554434", "67108862", "67108863", "67108864", "67108865", "67108866", "134217726", "134217727", "134217728", "134217729", "134217730", "268435454", "268435455", "268435456", "268435457", "268435458", "536870910", "536870911", "536870912", "536870913", "536870914", "1073741822", "1073741823", "1073741824", "1073741825", "1073741826", "2147483646", "2147483647", "2147483648", "2147483649", "2147483650", "4294967294", "4294967295", "4294967296", "4294967297", "4294967298", "8589934590", "8589934591", "8589934592", "8589934593", "8589934594", "17179869182", "17179869183", "17179869184", "17179869185", "17179869186", "34359738366", "34359738367", "34359738368", "34359738369", "34359738370", "68719476734", "68719476735", "68719476736", "68719476737", "68719476738", "137438953470", "137438953471", "137438953472", "137438953473", "137438953474", "274877906942", "274877906943", "274877906944", "274877906945", "274877906946", "549755813886", "549755813887", "549755813888", "549755813889", "549755813890", "1099511627774", "1099511627775", "1099511627776", "1099511627777", "1099511627778", "2199023255550", "2199023255551", "2199023255552", "2199023255553", "2199023255554", "4398046511102", "4398046511103", "4398046511104", "4398046511105", "4398046511106", "8796093022206", "8796093022207", "8796093022208", "8796093022209", "8796093022210", "17592186044414", "17592186044415", "17592186044416", "17592186044417", "17592186044418", "35184372088830", "35184372088831", "35184372088832", "35184372088833", "35184372088834", "70368744177662", "70368744177663", "70368744177664", "70368744177665", "70368744177666", "140737488355326", "140737488355327", "140737488355328", "140737488355329", "140737488355330", "281474976710654", "281474976710655", "281474976710656", "281474976710657", "281474976710658", "562949953421310", "562949953421311", "562949953421312", "562949953421313", "562949953421314", "1125899906842622", "1125899906842623", "1125899906842624", "1125899906842625", "1125899906842626", "2251799813685246", "2251799813685247", "2251799813685248", "2251799813685249", "2251799813685250", "4503599627370494", "4503599627370495", "4503599627370496", "4503599627370497", "4503599627370498", "9007199254740990", "9007199254740991", "9007199254740992", "9007199254740993", "9007199254740994", "18014398509481982", "18014398509481983", "18014398509481984", "18014398509481985", "18014398509481986", "36028797018963966", "36028797018963967", "36028797018963968", "36028797018963969", "36028797018963970", "72057594037927934", "72057594037927935", "72057594037927936", "72057594037927937", "72057594037927938", "144115188075855870", "144115188075855871", "144115188075855872", "144115188075855873", "144115188075855874", "288230376151711742", "288230376151711743", "288230376151711744", "288230376151711745", "288230376151711746", "576460752303423486", "576460752303423487", "576460752303423488", "576460752303423489", "576460752303423490", "1152921504606846974", "1152921504606846975", "1152921504606846976", "1152921504606846977", "1152921504606846978", "2305843009213693950", "2305843009213693951", "2305843009213693952", "2305843009213693953", "2305843009213693954", "4611686018427387902", "4611686018427387903", "4611686018427387904", "4611686018427387905", "4611686018427387906", "9223372036854775806", "9223372036854775807", "9223372036854775808", "9223372036854775809", "9223372036854775810", "18446744073709551614", "18446744073709551615", "18446744073709551616", "18446744073709551617", "18446744073709551618", "36893488147419103230", "36893488147419103231", "36893488147419103232", "36893488147419103233", "36893488147419103234" };
            var ints = o.Select(i => -BigInteger.Parse(i)).ToArray();
            Utils.RunWrite("ints_interesting_neg", ints);
        }

        [TestMethod]
        public void Writer_Double_Small()
        {
            Utils.RunWrite("doubles_small", new[]
            {
                    -5.0, -4.0, -3.0, -2.0, -1.0, 0.0, 1.0, 2.0, 3.0, 4.0, 5.0
            });
        }

        [TestMethod]
        public void Writer_Double_Interesting()
        {
            var writer = new Writer();
            var reader = new Reader();

            var actual = new[] { -3.14159, 3.14159, 4.0E11, 2.998E8, 6.626E-34 };
            
            var actualJson = writer.WriteJSON(actual);
            var expectedJson = Utils.LoadExample("doubles_interesting");
            var expected = reader.ReadJSON(expectedJson);

            for(var i = 0; i < actual.Length; i++)
            {
                var a = actual[i];
                var e = expected[i];
                Assert.AreEqual(a, e);
            }
        }

        [TestMethod]
        public void Writer_Uuid_One()
        {
            Utils.RunWrite("one_uuid", new Guid("5a2cbea3-e8c6-428b-b525-21239370dd55"));
        }

        [TestMethod]
        public void Writer_Uuid_List()
        {
            Utils.RunWrite("uuids", new[] {
                new Guid("5a2cbea3-e8c6-428b-b525-21239370dd55"),
                new Guid("d1dc64fa-da79-444b-9fa4-d4412f427289"),
                new Guid("501a978e-3a3e-4060-b3be-1cf2bd4b1a38"),
                new Guid("b3ba141a-a776-48e4-9fae-a28ea8571f58")
            });
        }

        [TestMethod]
        public void Writer_Uri_One()
        {
            Utils.RunWrite("one_uri", new Uri("http://example.com"));
        }

        [TestMethod]
        public void Writer_Uri_List()
        {
            Utils.RunWrite("uris", new[] {
                new Uri("http://example.com"),
                new Uri("ftp://example.com"),
                new Uri("file:///path/to/file.txt"),
                new Uri("http://www.詹姆斯.com/")
            });
        }

        [TestMethod]
        public void Writer_Date_Interesting()
        {
            Utils.RunWrite("dates_interesting", new[] {
                DateTime.Parse("1776-07-04T12:00:00.000-00:00"),
                DateTime.Parse("1970-01-01T00:00:00.000-00:00"),
                DateTime.Parse("2000-01-01T12:00:00.000-00:00"),
                DateTime.Parse("2014-04-07T22:17:17.000-00:00")});
        }

        [TestMethod]
        public void Writer_Vector_Symbols()
        {
            Utils.RunWrite("symbols", new[] {
                new Symbol("a"),
                new Symbol("ab"),
                new Symbol("abc"),
                new Symbol("abcd"),
                new Symbol("abcde"),
                new Symbol("a1"),
                new Symbol("b2"),
                new Symbol("c3"),
                new Symbol("a_b")
            });
        }

        [TestMethod]
        public void Writer_Vector_Keywords()
        {
            Utils.RunWrite("keywords", new[] {
                new Keyword("a"),
                new Keyword("ab"),
                new Keyword("abc"),
                new Keyword("abcd"),
                new Keyword("abcde"),
                new Keyword("a1"),
                new Keyword("b2"),
                new Keyword("c3"),
                new Keyword("a_b")
            });
        }

        [TestMethod]
        public void Writer_List_Simple()
        {
            Utils.RunWrite("list_simple", new List<int> {
                1,2,3
            });
        }

        [TestMethod]
        public void Writer_List_Empty()
        {
            Utils.RunWrite("list_empty", new List<int> { });
        }

        [TestMethod]
        public void Writer_List_Mixed()
        {
            Utils.RunWrite("list_mixed", new List<object>() {
               0, 1, 2.0, true, false, "five", new Keyword("six"), new Symbol("seven"), "~eight", null
           });
        }

        [TestMethod]
        public void Writer_List_Nested()
        {
            Utils.RunWrite("list_nested", new List<object>() {
                new List<object>() { 1, 2, 3 },
                new List<object>() { 0, 1, 2.0, true, false, "five", new Keyword("six"), new Symbol("seven"), "~eight", null }
           });
        }


        [TestMethod]
        public void Writer_Set_Simple()
        {
            Utils.RunWrite("set_simple", new HashSet<int> {
                1,3,2
            });
        }

        [TestMethod]
        public void Writer_Set_Empty()
        {
            Utils.RunWrite("set_empty", new HashSet<object> { });
        }

        [TestMethod]
        public void Writer_Set_Mixed()
        {
            //["~#set",[null,0,2.0,"~~eight",1,true,"five",false,"~$seven","~:six"]]
            Utils.RunWrite("set_mixed", new HashSet<object> {
                null, 0, 2.0, "~eight",1,true,"five",false,new Symbol("seven"), new Keyword("six")
            });
        }

        [TestMethod]
        public void Writer_Set_Nested()
        {
            //["~#set",[["^0",[1,3,2]],["^0",[null,0,2.0,"~~eight",1,true,"five",false,"~$seven","~:six"]]]]
            Utils.RunWrite("set_nested", new HashSet<object> {
                new HashSet<int>() {1,3,2 },
                new HashSet<object>() { null, 0, 2.0, "~eight",1,true,"five",false,new Symbol("seven"), new Keyword("six")}
            });
        }

        [TestMethod]
        public void Writer_Map_Simple()
        {
            Utils.RunWrite("map_simple", new Dictionary<Keyword, int>()
            {
                [new Keyword("c")] = 3,
                [new Keyword("b")] = 2,
                [new Keyword("a")] = 1
            });
        }

        [TestMethod]
        public void Writer_Map_Mixed()
        {
            Utils.RunWrite("map_mixed", new Dictionary<Keyword, object>()
            {
                [new Keyword("c")] = true,
                [new Keyword("b")] = "a string",
                [new Keyword("a")] = 1
            });
        }

        [TestMethod]
        public void Writer_Map_Nested()
        {
            //["^ ","~:simple",["^ ","~:c",3,"~:b",2,"~:a",1],"~:mixed",["^ ","~:c",true,"~:b","a string","~:a",1]]
            Utils.RunWrite("map_nested", new Dictionary<Keyword, object>()
            {
                [new Keyword("simple")] = new Dictionary<Keyword, int>()
                {
                    [new Keyword("c")] = 3,
                    [new Keyword("b")] = 2,
                    [new Keyword("a")] = 1
                },
                [new Keyword("mixed")] = new Dictionary<Keyword, object>()
                {
                    [new Keyword("c")] = true,
                    [new Keyword("b")] = "a string",
                    [new Keyword("a")] = 1
                },
            });
        }

        [TestMethod]
        public void Writer_Map_StringKeys()
        {
            Utils.RunWrite("map_string_keys", new Dictionary<string, int>()
            {
                ["second"] = 2,
                ["third"] = 3,
                ["first"] = 1
            });
        }

        [TestMethod]
        public void Writer_Map_NumericKeys()
        {
            //["^ ","~i1","one","~i2","two"]
            Utils.RunWrite("map_numeric_keys", new Dictionary<int, string>()
            {
                [1] = "one",
                [2] = "two",
            });
        }

        [TestMethod]
        public void Writer_Map_VectorKeys()
        {
            var two = new object[] { 2, 2 };
            Utils.RunWrite("map_vector_keys", new Dictionary<object[], string>()
            {
                [new object[] { 2, 2 }] = "two",
                [new object[] { 1, 1 }] = "one"
            });
        }

        [TestMethod]
        public void Writer_Map_10()
        {
            var map = new Dictionary<Keyword, int>();
            for (var i = 0; i < 10; i++)
            {
                var name = $"key{i:0000}";
                var k = new Keyword(name);
                map.Add(k, i);
            }
            Utils.RunWrite("map_10_items", map);
        }

        [TestMethod]
        public void Writer_Map_10_Nested()
        {
            //["^ ","~:f",["^ ","~:key0000",0,"~:key0001",1,"~:key0002",2,"~:key0003",3,"~:key0004",4,"~:key0005",5,"~:key0006",6,"~:key0007",7,"~:key0008",8,"~:key0009",9],"~:s",["^ ","^0",0,"^1",1,"^2",2,"^3",3,"^4",4,"^5",5,"^6",6,"^7",7,"^8",8,"^9",9]]
            var map = new Dictionary<Keyword, int>();
            for (var i = 0; i < 10; i++)
            {
                var name = $"key{i:0000}";
                var k = new Keyword(name);
                map.Add(k, i);
            }
            Utils.RunWrite("map_10_nested", new Dictionary<Keyword, Dictionary<Keyword,int>>
            {
                [new Keyword("f")] = map,
                [new Keyword("s")] = map
            });
        }

        [TestMethod]
        public void Writer_Map_1935_Nested()
        {
            var map = new Dictionary<Keyword, int>();
            for (var i = 0; i < 1935; i++)
            {
                var name = $"key{i:0000}";
                var k = new Keyword(name);
                map.Add(k, i);
            }
            Utils.RunWrite("map_1935_nested", new Dictionary<Keyword, Dictionary<Keyword, int>>
            {
                [new Keyword("f")] = map,
                [new Keyword("s")] = map
            });
        }

        [TestMethod]
        public void Writer_Map_1936_Nested()
        {
            var map = new Dictionary<Keyword, int>();
            for (var i = 0; i < 1936; i++)
            {
                var name = $"key{i:0000}";
                var k = new Keyword(name);
                map.Add(k, i);
            }
            Utils.RunWrite("map_1936_nested", new Dictionary<Keyword, Dictionary<Keyword, int>>
            {
                [new Keyword("f")] = map,
                [new Keyword("s")] = map
            });
        }

        [TestMethod]
        public void Writer_Map_1937_Nested()
        {
            var map = new Dictionary<Keyword, int>();
            for (var i = 0; i < 1937; i++)
            {
                var name = $"key{i:0000}";
                var k = new Keyword(name);
                map.Add(k, i);
            }
            Utils.RunWrite("map_1937_nested", new Dictionary<Keyword, Dictionary<Keyword, int>>
            {
                [new Keyword("f")] = map,
                [new Keyword("s")] = map
            });
        }

        [TestMethod]
        public void Writer_Vector_2charSymbol_Maps()
        {
            Symbol a = new Symbol("aa"), b = new Symbol("bb");
            Utils.RunWrite("maps_two_char_sym_keys", new[] {
                new Dictionary<Symbol, int>() {
                    [a] = 1,
                    [b] = 2
                },
                new Dictionary<Symbol, int>() {
                    [a] = 3,
                    [b] = 4
                },
                new Dictionary<Symbol, int>() {
                    [a] = 5,
                    [b] = 6
                }
            });
        }

        [TestMethod]
        public void Writer_Vector_3charSymbol_Maps()
        {
            Symbol a = new Symbol("aaa"), b = new Symbol("bbb");
            Utils.RunWrite("maps_three_char_sym_keys", new[] {
                new Dictionary<Symbol, int>() {
                    [a] = 1,
                    [b] = 2
                },
                new Dictionary<Symbol, int>() {
                    [a] = 3,
                    [b] = 4
                },
                new Dictionary<Symbol, int>() {
                    [a] = 5,
                    [b] = 6
                }
            });
        }

        [TestMethod]
        public void Writer_Vector_4charSymbol_Maps()
        {
            Symbol a = new Symbol("aaaa"), b = new Symbol("bbbb");
            Utils.RunWrite("maps_four_char_sym_keys", new[] {
                new Dictionary<Symbol, int>() {
                    [a] = 1,
                    [b] = 2
                },
                new Dictionary<Symbol, int>() {
                    [a] = 3,
                    [b] = 4
                },
                new Dictionary<Symbol, int>() {
                    [a] = 5,
                    [b] = 6
                }
            });
        }

        [TestMethod]
        public void Writer_Vector_2charString_Maps()
        {
            string a = "aa", b = "bb";
            Utils.RunWrite("maps_two_char_string_keys", new[] {
                new Dictionary<string, int>() {
                    [a] = 1,
                    [b] = 2
                },
                new Dictionary<string, int>() {
                    [a] = 3,
                    [b] = 4
                },
                new Dictionary<string, int>() {
                    [a] = 5,
                    [b] = 6
                }
            });
        }

        [TestMethod]
        public void Writer_Vector_3charString_Maps()
        {
            string a = "aaa", b = "bbb";
            Utils.RunWrite("maps_three_char_string_keys", new[] {
                new Dictionary<string, int>() {
                    [b] = 2,
                    [a] = 1
                },
                new Dictionary<string, int>() {
                    [b] = 4,
                    [a] = 3
                },
                new Dictionary<string, int>() {
                    [b] = 6,
                    [a] = 5
                }
            });
        }

        [TestMethod]
        public void Writer_Vector_4charString_Maps()
        {
            string a = "aaaa", b = "bbbb";
            Utils.RunWrite("maps_four_char_string_keys", new [] {
                new Dictionary<string, int>() {
                    [a] = 1,
                    [b] = 2
                },
                new Dictionary<string, int>() {
                    [a] = 3,
                    [b] = 4
                },
                new Dictionary<string, int>() {
                    [a] = 5,
                    [b] = 6                    
                }
            });
        }

        [TestMethod]
        public void Writer_Map_Unrecognized()
        {
            //[["~#abcde","~:anything"],["~#fghij","~:anything-else"]]
            Utils.RunWrite("maps_unrecognized_keys", new object[] {
                new object[] {
                    "~#abcde","~:anything"
                },
                new object[] {
                    "~#fghij","~:anything-else"
                }
            });
        }

        [TestMethod]
        public void Writer_Vector_Keywords_1935_Repeated()
        {
            var keywords = Enumerable.Range(0, 1935).Select(i => new Keyword($"key{i:0000}"));
            var repeats = keywords.Concat(keywords).ToArray();
            Utils.RunWrite("vector_1935_keywords_repeated_twice", repeats);
        }

        [TestMethod]
        public void Writer_Vector_Keywords_1936_Repeated()
        {
            var keywords = Enumerable.Range(0, 1936).Select(i => new Keyword($"key{i:0000}"));
            var repeats = keywords.Concat(keywords).ToArray();
            Utils.RunWrite("vector_1936_keywords_repeated_twice", repeats);
        }

        [TestMethod]
        public void Writer_Vector_Keywords_1937_Repeated()
        {
            var keywords = Enumerable.Range(0, 1937).Select(i => new Keyword($"key{i:0000}"));
            var repeats = keywords.Concat(keywords).ToArray();
            Utils.RunWrite("vector_1937_keywords_repeated_twice", repeats);
        }

        [TestMethod]
        public void Writer_Vector_Unrecognized()
        {
            Utils.RunWrite("vector_unrecognized_vals", new[] { "~Unrecognized" });
        }

        [TestMethod]
        public void Writer_Double_Special()
        {
            Utils.RunWrite("vector_special_numbers", new[] {
                    double.NaN, double.PositiveInfinity, double.NegativeInfinity
            });
        }
    }
}
