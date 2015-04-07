using System;
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
            Utils.RunWrite("one_keyword", new Symbol("hello"));
        }

        [TestMethod]
        public void Writer_Date_One()
        {
            Utils.RunWrite("one_date", new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc));
        }

        [TestMethod]
        public void Writer_Vectors_Simple()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Vectors_Empty()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Vectors_Mixed()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Vectors_Nested()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_String_Small_Vector()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_String_Tilde_Vector()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_String_Hash_Vector()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_String_Hat_Vector()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Int_Small_Vector()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Int_List()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Int_Interesting()
        {
            var o = new string[] { "-1", "0", "1", "2", "3", "0", "1", "2", "3", "4", "2", "3", "4", "5", "6", "6", "7", "8", "9", "10", "14", "15", "16", "17", "18", "30", "31", "32", "33", "34", "62", "63", "64", "65", "66", "126", "127", "128", "129", "130", "254", "255", "256", "257", "258", "510", "511", "512", "513", "514", "1022", "1023", "1024", "1025", "1026", "2046", "2047", "2048", "2049", "2050", "4094", "4095", "4096", "4097", "4098", "8190", "8191", "8192", "8193", "8194", "16382", "16383", "16384", "16385", "16386", "32766", "32767", "32768", "32769", "32770", "65534", "65535", "65536", "65537", "65538", "131070", "131071", "131072", "131073", "131074", "262142", "262143", "262144", "262145", "262146", "524286", "524287", "524288", "524289", "524290", "1048574", "1048575", "1048576", "1048577", "1048578", "2097150", "2097151", "2097152", "2097153", "2097154", "4194302", "4194303", "4194304", "4194305", "4194306", "8388606", "8388607", "8388608", "8388609", "8388610", "16777214", "16777215", "16777216", "16777217", "16777218", "33554430", "33554431", "33554432", "33554433", "33554434", "67108862", "67108863", "67108864", "67108865", "67108866", "134217726", "134217727", "134217728", "134217729", "134217730", "268435454", "268435455", "268435456", "268435457", "268435458", "536870910", "536870911", "536870912", "536870913", "536870914", "1073741822", "1073741823", "1073741824", "1073741825", "1073741826", "2147483646", "2147483647", "2147483648", "2147483649", "2147483650", "4294967294", "4294967295", "4294967296", "4294967297", "4294967298", "8589934590", "8589934591", "8589934592", "8589934593", "8589934594", "17179869182", "17179869183", "17179869184", "17179869185", "17179869186", "34359738366", "34359738367", "34359738368", "34359738369", "34359738370", "68719476734", "68719476735", "68719476736", "68719476737", "68719476738", "137438953470", "137438953471", "137438953472", "137438953473", "137438953474", "274877906942", "274877906943", "274877906944", "274877906945", "274877906946", "549755813886", "549755813887", "549755813888", "549755813889", "549755813890", "1099511627774", "1099511627775", "1099511627776", "1099511627777", "1099511627778", "2199023255550", "2199023255551", "2199023255552", "2199023255553", "2199023255554", "4398046511102", "4398046511103", "4398046511104", "4398046511105", "4398046511106", "8796093022206", "8796093022207", "8796093022208", "8796093022209", "8796093022210", "17592186044414", "17592186044415", "17592186044416", "17592186044417", "17592186044418", "35184372088830", "35184372088831", "35184372088832", "35184372088833", "35184372088834", "70368744177662", "70368744177663", "70368744177664", "70368744177665", "70368744177666", "140737488355326", "140737488355327", "140737488355328", "140737488355329", "140737488355330", "281474976710654", "281474976710655", "281474976710656", "281474976710657", "281474976710658", "562949953421310", "562949953421311", "562949953421312", "562949953421313", "562949953421314", "1125899906842622", "1125899906842623", "1125899906842624", "1125899906842625", "1125899906842626", "2251799813685246", "2251799813685247", "2251799813685248", "2251799813685249", "2251799813685250", "4503599627370494", "4503599627370495", "4503599627370496", "4503599627370497", "4503599627370498", "9007199254740990", "9007199254740991", "9007199254740992", "9007199254740993", "9007199254740994", "18014398509481982", "18014398509481983", "18014398509481984", "18014398509481985", "18014398509481986", "36028797018963966", "36028797018963967", "36028797018963968", "36028797018963969", "36028797018963970", "72057594037927934", "72057594037927935", "72057594037927936", "72057594037927937", "72057594037927938", "144115188075855870", "144115188075855871", "144115188075855872", "144115188075855873", "144115188075855874", "288230376151711742", "288230376151711743", "288230376151711744", "288230376151711745", "288230376151711746", "576460752303423486", "576460752303423487", "576460752303423488", "576460752303423489", "576460752303423490", "1152921504606846974", "1152921504606846975", "1152921504606846976", "1152921504606846977", "1152921504606846978", "2305843009213693950", "2305843009213693951", "2305843009213693952", "2305843009213693953", "2305843009213693954", "4611686018427387902", "4611686018427387903", "4611686018427387904", "4611686018427387905", "4611686018427387906", "9223372036854775806", "9223372036854775807", "9223372036854775808", "9223372036854775809", "9223372036854775810", "18446744073709551614", "18446744073709551615", "18446744073709551616", "18446744073709551617", "18446744073709551618", "36893488147419103230", "36893488147419103231", "36893488147419103232", "36893488147419103233", "36893488147419103234" };

            var ints = o.Select(i => BigInteger.Parse(i));
            Utils.RunWrite("ints_interesting",ints);
        }

        [TestMethod]
        public void Writer_Int_InterestingNegative()
        {
            Assert.IsTrue(false);
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
            Utils.RunWrite("doubles_interesting", new []{ -3.14159, 3.14159, 4.0E11, 2.998E8, 6.626E-34 });
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
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Vector_Keywords()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_List_Simple()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_List_Empty()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_List_Mixed()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_List_Nested()
        {
            Assert.IsTrue(false);
        }


        [TestMethod]
        public void Writer_Set_Simple()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Set_Empty()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Set_Mixed()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Set_Nested()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Map_Simple()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Map_Mixed()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Map_Nested()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Map_StringKeys()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Map_NumericKeys()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Map_VectorKeys()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Map_10()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Map_10_Nested()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Map_1935_Nested()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Map_1936_Nested()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Map_1937_Nested()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Vector_2charSymbol_Maps()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Vector_3charSymbol_Maps()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Vector_4charSymbol_Maps()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Vector_2charString_Maps()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Vector_3chaString_Maps()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Vector_4charStringy_Maps()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Map_Unrecognized()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Vector_Keywords_1935_Repeated()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Vector_Keywords_1936_Repeated()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Vector_Keywords_1937_Repeated()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Vector_Unrecognized()
        {
            Assert.IsTrue(false);
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
