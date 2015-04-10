using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransNET.Types;

namespace UnitTestTransNET
{
    [TestClass]
    public class ReaderUnitTests
    {
        [TestMethod]
        public void Reader_Null()
        {
            Utils.RunRead("nil", r => Assert.AreEqual(r,(object)null));
        }
        
        [TestMethod]
        public void Reader_Bool_True()
        {
            Utils.RunRead("true", r => Assert.AreEqual(r, true));
        }

        [TestMethod]
        public void Reader_Bool_False()
        {
            Utils.RunRead("false", r => Assert.AreEqual(r, false));
        }

        [TestMethod]
        public void Reader_Int_Zero()
        {
            Utils.RunRead("zero", r => Assert.AreEqual(r, 0));
        }

        [TestMethod]
        public void Reader_Int_One()
        {
            Utils.RunRead("one", r => Assert.AreEqual(r, 1));
        }

        [TestMethod]
        public void Reader_String_One()
        {
            Utils.RunRead("one_string", r => Assert.AreEqual(r, "hello"));
        }

        [TestMethod]
        public void Reader_Keyword_One()
        {
            Utils.RunRead("one_keyword", r => Assert.AreEqual(r, new Keyword("hello")));
        }

        [TestMethod]
        public void Reader_Symbol_One()
        {
            Utils.RunRead("one_symbol", r => Assert.AreEqual(r, new Symbol("hello")));
        }

        [TestMethod]
        public void Reader_Date_One()
        {
            Utils.RunRead("one_date", r => Assert.AreEqual(r, new DateTime(2000,1,1, 12,0,0, DateTimeKind.Utc)));
        }

        [TestMethod]
        public void Reader_Vectors_Simple()
        {
            Utils.RunRead("vector_simple", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                var o = new object[] {  1,2,3 };
                foreach (var i in Enumerable.Range(0, o.Length)) Assert.AreEqual(r[i], o[i]);
            });
        }

        [TestMethod]
        public void Reader_Vectors_Empty()
        {
            Utils.RunRead("vector_empty", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                Assert.AreEqual(r.Length, 0);
            });
        }

        [TestMethod]
        public void Reader_Vectors_Mixed()
        {
            Utils.RunRead("vector_mixed", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                var o = new object[] { 0,1,2.0,true,false,"five",new Keyword("six"),new Symbol("seven"),"~eight",null};
                foreach(var i in Enumerable.Range(0,o.Length)) Assert.AreEqual(r[i], o[i]);
            });
        }

        [TestMethod]
        public void Reader_Vectors_Nested()
        {
            Utils.RunRead("vector_nested", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                Assert.AreEqual(r.Length, 2);

                var a = r[0];
                Assert.IsInstanceOfType(a, typeof(object[]));
                var ao = new object[] { 1, 2, 3 };
                foreach (var i in Enumerable.Range(0, ao.Length)) Assert.AreEqual(a[i], ao[i]);

                var b = r[1];
                Assert.IsInstanceOfType(b, typeof(object[]));
                var bo = new object[] { 0, 1, 2.0, true, false, "five", new Keyword("six"), new Symbol("seven"), "~eight", null };
                foreach (var i in Enumerable.Range(0, bo.Length)) Assert.AreEqual(b[i], bo[i]);
            });
        }

        [TestMethod]
        public void Reader_String_Small_Vector()
        {
            Utils.RunRead("small_strings", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                var o = new[] { "", "a", "ab", "abc", "abcd", "abcde", "abcdef" };
                foreach (var i in Enumerable.Range(0, o.Length)) Assert.AreEqual(r[i], o[i]);
            });
        }

        [TestMethod]
        public void Reader_String_Tilde_Vector()
        {
            Utils.RunRead("strings_tilde", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                var o = new[] { "~", "~a", "~ab", "~abc", "~abcd", "~abcde", "~abcdef" };
                foreach (var i in Enumerable.Range(0, o.Length)) Assert.AreEqual(r[i], o[i]);
            });
        }

        [TestMethod]
        public void Reader_String_Hash_Vector()
        {
            Utils.RunRead("strings_hash", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                var o = new[] { "#", "#a", "#ab", "#abc", "#abcd", "#abcde", "#abcdef" };
                foreach (var i in Enumerable.Range(0, o.Length)) Assert.AreEqual(r[i], o[i]);
            });
        }

        [TestMethod]
        public void Reader_String_Hat_Vector()
        {
            Utils.RunRead("strings_hat", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                var o = new[] { "^", "^a", "^ab", "^abc", "^abcd", "^abcde", "^abcdef" };
                foreach (var i in Enumerable.Range(0, o.Length)) Assert.AreEqual(r[i], o[i]);
            });
        }

        [TestMethod]
        public void Reader_Int_Small_Vector()
        {
            Utils.RunRead("small_ints", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                var o = new[] { -5,-4,-3,-2,-1,0,1,2,3,4,5 };
                foreach (var i in Enumerable.Range(0, o.Length)) Assert.AreEqual(r[i], o[i]);
            });
        }


        [TestMethod]
        public void Reader_Int_List()
        {
            Utils.RunRead("ints", r =>
            {
                for (var i = 0; i < 11; i++) Assert.AreEqual(r[i], i);
            });
        }

        [TestMethod]
        public void Reader_Int_Interesting()
        {
            Utils.RunRead("ints_interesting", r =>
            {
                var o = new string[] { "-1", "0", "1", "2", "3", "0", "1", "2", "3", "4", "2", "3", "4", "5", "6", "6", "7", "8", "9", "10", "14", "15", "16", "17", "18", "30", "31", "32", "33", "34", "62", "63", "64", "65", "66", "126", "127", "128", "129", "130", "254", "255", "256", "257", "258", "510", "511", "512", "513", "514", "1022", "1023", "1024", "1025", "1026", "2046", "2047", "2048", "2049", "2050", "4094", "4095", "4096", "4097", "4098", "8190", "8191", "8192", "8193", "8194", "16382", "16383", "16384", "16385", "16386", "32766", "32767", "32768", "32769", "32770", "65534", "65535", "65536", "65537", "65538", "131070", "131071", "131072", "131073", "131074", "262142", "262143", "262144", "262145", "262146", "524286", "524287", "524288", "524289", "524290", "1048574", "1048575", "1048576", "1048577", "1048578", "2097150", "2097151", "2097152", "2097153", "2097154", "4194302", "4194303", "4194304", "4194305", "4194306", "8388606", "8388607", "8388608", "8388609", "8388610", "16777214", "16777215", "16777216", "16777217", "16777218", "33554430", "33554431", "33554432", "33554433", "33554434", "67108862", "67108863", "67108864", "67108865", "67108866", "134217726", "134217727", "134217728", "134217729", "134217730", "268435454", "268435455", "268435456", "268435457", "268435458", "536870910", "536870911", "536870912", "536870913", "536870914", "1073741822", "1073741823", "1073741824", "1073741825", "1073741826", "2147483646", "2147483647", "2147483648", "2147483649", "2147483650", "4294967294", "4294967295", "4294967296", "4294967297", "4294967298", "8589934590", "8589934591", "8589934592", "8589934593", "8589934594", "17179869182", "17179869183", "17179869184", "17179869185", "17179869186", "34359738366", "34359738367", "34359738368", "34359738369", "34359738370", "68719476734", "68719476735", "68719476736", "68719476737", "68719476738", "137438953470", "137438953471", "137438953472", "137438953473", "137438953474", "274877906942", "274877906943", "274877906944", "274877906945", "274877906946", "549755813886", "549755813887", "549755813888", "549755813889", "549755813890", "1099511627774", "1099511627775", "1099511627776", "1099511627777", "1099511627778", "2199023255550", "2199023255551", "2199023255552", "2199023255553", "2199023255554", "4398046511102", "4398046511103", "4398046511104", "4398046511105", "4398046511106", "8796093022206", "8796093022207", "8796093022208", "8796093022209", "8796093022210", "17592186044414", "17592186044415", "17592186044416", "17592186044417", "17592186044418", "35184372088830", "35184372088831", "35184372088832", "35184372088833", "35184372088834", "70368744177662", "70368744177663", "70368744177664", "70368744177665", "70368744177666", "140737488355326", "140737488355327", "140737488355328", "140737488355329", "140737488355330", "281474976710654", "281474976710655", "281474976710656", "281474976710657", "281474976710658", "562949953421310", "562949953421311", "562949953421312", "562949953421313", "562949953421314", "1125899906842622", "1125899906842623", "1125899906842624", "1125899906842625", "1125899906842626", "2251799813685246", "2251799813685247", "2251799813685248", "2251799813685249", "2251799813685250", "4503599627370494", "4503599627370495", "4503599627370496", "4503599627370497", "4503599627370498", "9007199254740990", "9007199254740991", "9007199254740992", "9007199254740993", "9007199254740994", "18014398509481982", "18014398509481983", "18014398509481984", "18014398509481985", "18014398509481986", "36028797018963966", "36028797018963967", "36028797018963968", "36028797018963969", "36028797018963970", "72057594037927934", "72057594037927935", "72057594037927936", "72057594037927937", "72057594037927938", "144115188075855870", "144115188075855871", "144115188075855872", "144115188075855873", "144115188075855874", "288230376151711742", "288230376151711743", "288230376151711744", "288230376151711745", "288230376151711746", "576460752303423486", "576460752303423487", "576460752303423488", "576460752303423489", "576460752303423490", "1152921504606846974", "1152921504606846975", "1152921504606846976", "1152921504606846977", "1152921504606846978", "2305843009213693950", "2305843009213693951", "2305843009213693952", "2305843009213693953", "2305843009213693954", "4611686018427387902", "4611686018427387903", "4611686018427387904", "4611686018427387905", "4611686018427387906", "9223372036854775806", "9223372036854775807", "9223372036854775808", "9223372036854775809", "9223372036854775810", "18446744073709551614", "18446744073709551615", "18446744073709551616", "18446744073709551617", "18446744073709551618", "36893488147419103230", "36893488147419103231", "36893488147419103232", "36893488147419103233", "36893488147419103234" };

                for (var i = 0; i < o.Length; i++)
                {
                    var actual = r[i];
                    var expected = BigInteger.Parse(o[i]);
                    Assert.AreEqual(expected, actual);
                }
            });
        }

        [TestMethod]
        public void Reader_Int_InterestingNegative()
        {
            Utils.RunRead("ints_interesting_neg", r =>
            {
                var o = new string[] { "1", "0", "-1", "-2", "-3", "0", "-1", "-2", "-3", "-4", "-2", "-3", "-4", "-5", "-6", "-6", "-7", "-8", "-9", "-10", "-14", "-15", "-16", "-17", "-18", "-30", "-31", "-32", "-33", "-34", "-62", "-63", "-64", "-65", "-66", "-126", "-127", "-128", "-129", "-130", "-254", "-255", "-256", "-257", "-258", "-510", "-511", "-512", "-513", "-514", "-1022", "-1023", "-1024", "-1025", "-1026", "-2046", "-2047", "-2048", "-2049", "-2050", "-4094", "-4095", "-4096", "-4097", "-4098", "-8190", "-8191", "-8192", "-8193", "-8194", "-16382", "-16383", "-16384", "-16385", "-16386", "-32766", "-32767", "-32768", "-32769", "-32770", "-65534", "-65535", "-65536", "-65537", "-65538", "-131070", "-131071", "-131072", "-131073", "-131074", "-262142", "-262143", "-262144", "-262145", "-262146", "-524286", "-524287", "-524288", "-524289", "-524290", "-1048574", "-1048575", "-1048576", "-1048577", "-1048578", "-2097150", "-2097151", "-2097152", "-2097153", "-2097154", "-4194302", "-4194303", "-4194304", "-4194305", "-4194306", "-8388606", "-8388607", "-8388608", "-8388609", "-8388610", "-16777214", "-16777215", "-16777216", "-16777217", "-16777218", "-33554430", "-33554431", "-33554432", "-33554433", "-33554434", "-67108862", "-67108863", "-67108864", "-67108865", "-67108866", "-134217726", "-134217727", "-134217728", "-134217729", "-134217730", "-268435454", "-268435455", "-268435456", "-268435457", "-268435458", "-536870910", "-536870911", "-536870912", "-536870913", "-536870914", "-1073741822", "-1073741823", "-1073741824", "-1073741825", "-1073741826", "-2147483646", "-2147483647", "-2147483648", "-2147483649", "-2147483650", "-4294967294", "-4294967295", "-4294967296", "-4294967297", "-4294967298", "-8589934590", "-8589934591", "-8589934592", "-8589934593", "-8589934594", "-17179869182", "-17179869183", "-17179869184", "-17179869185", "-17179869186", "-34359738366", "-34359738367", "-34359738368", "-34359738369", "-34359738370", "-68719476734", "-68719476735", "-68719476736", "-68719476737", "-68719476738", "-137438953470", "-137438953471", "-137438953472", "-137438953473", "-137438953474", "-274877906942", "-274877906943", "-274877906944", "-274877906945", "-274877906946", "-549755813886", "-549755813887", "-549755813888", "-549755813889", "-549755813890", "-1099511627774", "-1099511627775", "-1099511627776", "-1099511627777", "-1099511627778", "-2199023255550", "-2199023255551", "-2199023255552", "-2199023255553", "-2199023255554", "-4398046511102", "-4398046511103", "-4398046511104", "-4398046511105", "-4398046511106", "-8796093022206", "-8796093022207", "-8796093022208", "-8796093022209", "-8796093022210", "-17592186044414", "-17592186044415", "-17592186044416", "-17592186044417", "-17592186044418", "-35184372088830", "-35184372088831", "-35184372088832", "-35184372088833", "-35184372088834", "-70368744177662", "-70368744177663", "-70368744177664", "-70368744177665", "-70368744177666", "-140737488355326", "-140737488355327", "-140737488355328", "-140737488355329", "-140737488355330", "-281474976710654", "-281474976710655", "-281474976710656", "-281474976710657", "-281474976710658", "-562949953421310", "-562949953421311", "-562949953421312", "-562949953421313", "-562949953421314", "-1125899906842622", "-1125899906842623", "-1125899906842624", "-1125899906842625", "-1125899906842626", "-2251799813685246", "-2251799813685247", "-2251799813685248", "-2251799813685249", "-2251799813685250", "-4503599627370494", "-4503599627370495", "-4503599627370496", "-4503599627370497", "-4503599627370498", "-9007199254740990", "-9007199254740991", "-9007199254740992", "-9007199254740993", "-9007199254740994", "-18014398509481982", "-18014398509481983", "-18014398509481984", "-18014398509481985", "-18014398509481986", "-36028797018963966", "-36028797018963967", "-36028797018963968", "-36028797018963969", "-36028797018963970", "-72057594037927934", "-72057594037927935", "-72057594037927936", "-72057594037927937", "-72057594037927938", "-144115188075855870", "-144115188075855871", "-144115188075855872", "-144115188075855873", "-144115188075855874", "-288230376151711742", "-288230376151711743", "-288230376151711744", "-288230376151711745", "-288230376151711746", "-576460752303423486", "-576460752303423487", "-576460752303423488", "-576460752303423489", "-576460752303423490", "-1152921504606846974", "-1152921504606846975", "-1152921504606846976", "-1152921504606846977", "-1152921504606846978", "-2305843009213693950", "-2305843009213693951", "-2305843009213693952", "-2305843009213693953", "-2305843009213693954", "-4611686018427387902", "-4611686018427387903", "-4611686018427387904", "-4611686018427387905", "-4611686018427387906", "-9223372036854775806", "-9223372036854775807", "-9223372036854775808", "-9223372036854775809", "-9223372036854775810", "-18446744073709551614", "-18446744073709551615", "-18446744073709551616", "-18446744073709551617", "-18446744073709551618", "-36893488147419103230", "-36893488147419103231", "-36893488147419103232", "-36893488147419103233", "-36893488147419103234" };

                for (var i = 0; i < o.Length; i++)
                {
                    var actual = r[i];
                    var expected = BigInteger.Parse(o[i]);
                    Assert.AreEqual(expected, actual);
                }
            });
        }

        [TestMethod]
        public void Reader_Double_Small()
        {
            Utils.RunRead("doubles_small", r =>
            {
                var o = new double[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 };

                for (var i = 0; i < o.Length; i++)
                {
                    var actual = r[i];
                    var expected = o[i];
                    Assert.AreEqual(expected, actual);
                }
            });
        }

        [TestMethod]
        public void Reader_Double_Interesting()
        {
            Utils.RunRead("doubles_interesting", r =>
            {
                var o = new[] { -3.14159, 3.14159, 4.0E11, 2.998E8, 6.626E-34 };

                for (var i = 0; i < o.Length; i++)
                {
                    var actual = r[i];
                    var expected = o[i];
                    Assert.AreEqual(expected, actual);
                }
            });
        }

        [TestMethod]
        public void Reader_Uuid_One()
        {
            Utils.RunRead("one_uuid", r =>
            {
                Assert.AreEqual(r, new Guid("5a2cbea3-e8c6-428b-b525-21239370dd55"));
            });
        }

        [TestMethod]
        public void Reader_Uuid_List()
        {
            Utils.RunRead("uuids", r =>
            {
                var o = new[] {
                    "5a2cbea3-e8c6-428b-b525-21239370dd55",
                    "d1dc64fa-da79-444b-9fa4-d4412f427289",
                    "501a978e-3a3e-4060-b3be-1cf2bd4b1a38",
                    "b3ba141a-a776-48e4-9fae-a28ea8571f58"
                };

                for (var i = 0; i < o.Length; i++)
                {
                    var actual = r[i];
                    var expected = new Guid(o[i]);
                    Assert.AreEqual(expected, actual);
                }
            });
        }

        [TestMethod]
        public void Reader_Uri_One()
        {
            Utils.RunRead("one_uri", r =>
            {
                Assert.AreEqual(new Uri("http://example.com"), r);
            });
        }

        [TestMethod]
        public void Reader_Uri_List()
        {
            Utils.RunRead("uris", r =>
            {
                var uris = new[]
                {
                    "http://example.com",
                    "ftp://example.com",
                    "file:///path/to/file.txt",
                    "http://www.詹姆斯.com"
                };

                for (var i = 0; i < uris.Length; i++)
                {
                    var actual = r[i];
                    var expected = new Uri(uris[i]);
                    Assert.AreEqual(expected, actual);
                }
            });
        }

        [TestMethod]
        public void Reader_Date_Interesting()
        {
            Utils.RunRead("dates_interesting", r =>
            {
                var dates = new[]
                {
                    "1776-07-04T12:00:00.000-00:00",
                    "1970-01-01T00:00:00.000-00:00",
                    "2000-01-01T12:00:00.000-00:00",
                    "2014-04-07T22:17:17.000-00:00"
                };

                for (var i = 0; i < dates.Length; i++)
                {
                    var actual = r[i];
                    var expected = DateTime.Parse(dates[i]).ToUniversalTime();
                    Assert.AreEqual(expected, actual);
                }
            });
        }

        [TestMethod]
        public void Reader_Vector_Symbols()
        {
            Utils.RunRead("symbols", r =>
            {
                var symbols = new[]
                {
                    "a","ab","abc","abcd","abcde","a1","b2","c3","a_b"
                };

                for (var i = 0; i < symbols.Length; i++)
                {
                    var actual = r[i];
                    var expected = new Symbol(symbols[i]);
                    Assert.AreEqual(expected, actual);
                }
            });
        }

        [TestMethod]
        public void Reader_Vector_Keywords()
        {
            Utils.RunRead("keywords", r =>
            {
                var symbols = new[]
                {
                    "a","ab","abc","abcd","abcde","a1","b2","c3","a_b"
                };

                for (var i = 0; i < symbols.Length; i++)
                {
                    var actual = r[i];
                    var expected = new Keyword(symbols[i]);
                    Assert.AreEqual(expected, actual);
                }
            });
        }

        [TestMethod]
        public void Reader_List_Simple()
        {
            Utils.RunRead("list_simple", r =>
            {
                Assert.IsInstanceOfType(r,typeof(IEnumerable<object>));
                var o = new[]{1,2,3};

                for (var i = 0; i < o.Length; i++)
                {
                    var actual = r[i];
                    var expected = o[i];
                    Assert.AreEqual(expected, actual);
                }
            });
        }

        [TestMethod]
        public void Reader_List_Empty()
        {
            Utils.RunRead("list_empty", r =>
            {
                Assert.IsInstanceOfType(r, typeof(IEnumerable<object>));
                Assert.AreEqual(r.Count, 0);
            });
        }

        [TestMethod]
        public void Reader_List_Mixed()
        {
            Utils.RunRead("list_mixed", r =>
            {
                Assert.IsInstanceOfType(r, typeof(IEnumerable<object>));
                var o = new object[] { 0, 1, 2.0, true, false, "five", new Keyword("six"), new Symbol("seven"), "~eight", null };
                foreach (var i in Enumerable.Range(0, o.Length)) Assert.AreEqual(r[i], o[i]);
            });
        }

        [TestMethod]
        public void Reader_List_Nested()
        {
            Utils.RunRead("list_nested", r =>
            {
                Assert.IsInstanceOfType(r, typeof(IEnumerable<object>));
                Assert.AreEqual(r.Count, 2);

                var a = r[0];
                Assert.IsInstanceOfType(r, typeof(IEnumerable<object>));
                var ao = new object[] { 1, 2, 3 };
                foreach (var i in Enumerable.Range(0, ao.Length)) Assert.AreEqual(a[i], ao[i]);

                var b = r[1];
                Assert.IsInstanceOfType(r, typeof(IEnumerable<object>));
                var bo = new object[] { 0, 1, 2.0, true, false, "five", new Keyword("six"), new Symbol("seven"), "~eight", null };
                foreach (var i in Enumerable.Range(0, bo.Length)) Assert.AreEqual(b[i], bo[i]);
            });
        }


        [TestMethod]
        public void Reader_Set_Simple()
        {
            Utils.RunRead("set_simple", r =>
            {
                Assert.IsInstanceOfType(r, typeof(ISet<object>));
                var o = new object[] { 1, 2, 3 };
                foreach (var i in o) Assert.IsTrue(r.Contains(i));
            });
        }

        [TestMethod]
        public void Reader_Set_Empty()
        {
            Utils.RunRead("set_empty", r =>
            {
                Assert.IsInstanceOfType(r, typeof(ISet<object>));
                Assert.AreEqual(r.Count, 0);
            });
        }

        [TestMethod]
        public void Reader_Set_Mixed()
        {
            Utils.RunRead("set_mixed", r =>
            {
                Assert.IsInstanceOfType(r, typeof(ISet<object>));
                var o = new object[] { null, 0, 2.0, "~eight", 1, true, "five", false, new Symbol("seven"), new Keyword("six") };
                foreach(var i in o) Assert.IsTrue(r.Contains(i));
            });
        }

        [TestMethod]
        public void Reader_Set_Nested()
        {
            Utils.RunRead("set_nested", r =>
            {
                Assert.IsInstanceOfType(r, typeof(ISet<dynamic>));
                Assert.AreEqual(r.Count, 2);

                var a = new HashSet<dynamic>() { 1, 2, 3 };
                var b = new HashSet<dynamic>() { 0, 1, 2.0, true, false, "five", new Keyword("six"), new Symbol("seven"), "~eight", null };

                foreach (ISet<dynamic> s in r)
                {
                    var valid = false;
                    if (s.Count == 3)
                    {
                        foreach (var i in s) Assert.IsTrue(a.Contains(i));
                        valid = true;
                    }
                    else if (s.Count == 10)
                    {
                        foreach (var i in s) Assert.IsTrue(b.Contains(i));
                        valid = true;
                    }
                    Assert.IsTrue(valid);
                }
            });
        }

        [TestMethod]
        public void Reader_Map_Simple()
        {
            Utils.RunRead("map_simple", r =>
            {
                Assert.IsInstanceOfType(r, typeof(IDictionary<dynamic, dynamic>));
                var o = new Dictionary<dynamic,dynamic>(){ 
                    [new Keyword("c")] = 3,
                    [new Keyword("b")] = 2,
                    [new Keyword("a")] = 1
                };

                foreach(var kvp in o)
                {
                    Assert.AreEqual(r[kvp.Key], kvp.Value);
                }
            });
        }

        [TestMethod]
        public void Reader_Map_Mixed()
        {
            Utils.RunRead("map_mixed", r =>
            {
                Assert.IsInstanceOfType(r, typeof(IDictionary<dynamic, dynamic>));
                var o = new Dictionary<dynamic, dynamic>()
                {
                    [new Keyword("c")] = true,
                    [new Keyword("b")] = "a string",
                    [new Keyword("a")] = 1
                };

                foreach (var kvp in o)
                {
                    Assert.AreEqual(r[kvp.Key], kvp.Value);
                }
            });
        }

        [TestMethod]
        public void Reader_Map_Nested()
        {
            Utils.RunRead("map_nested", r =>
            {
                Assert.IsInstanceOfType(r, typeof(IDictionary<dynamic, dynamic>));

                var a = new Dictionary<dynamic, dynamic>()
                {
                    [new Keyword("c")] = 3,
                    [new Keyword("b")] = 2,
                    [new Keyword("a")] = 1
                };
                var simple = r[new Keyword("simple")];
                foreach (var kvp in simple) Assert.AreEqual(simple[kvp.Key], kvp.Value);
                
                var b = new Dictionary<dynamic, dynamic>()
                {
                    [new Keyword("c")] = true,
                    [new Keyword("b")] = "a string",
                    [new Keyword("a")] = 1
                };
                var mixed = r[new Keyword("mixed")];
                foreach (var kvp in mixed) Assert.AreEqual(mixed[kvp.Key], kvp.Value);
            });
        }

        [TestMethod]
        public void Reader_Map_StringKeys()
        {
            Utils.RunRead("map_string_keys", r =>
            {
                Assert.IsInstanceOfType(r, typeof(IDictionary<dynamic, dynamic>));

                var a = new Dictionary<dynamic, dynamic>()
                {
                    ["second"] = 2,
                    ["third"] = 3,
                    ["first"] = 1
                };
                foreach (var kvp in r) Assert.AreEqual(r[kvp.Key], kvp.Value);
            });
        }

        [TestMethod]
        public void Reader_Map_NumericKeys()
        {
            Utils.RunRead("map_numeric_keys", r =>
            {
                Assert.IsInstanceOfType(r, typeof(IDictionary<dynamic, dynamic>));

                var a = new Dictionary<dynamic, dynamic>()
                {
                    [1] = "one",
                    [2] = "two",
                };
                foreach (var kvp in r) Assert.AreEqual(r[kvp.Key], kvp.Value);
            });
        }

        [TestMethod]
        public void Reader_Map_VectorKeys()
        {
            Utils.RunRead("map_vector_keys", r =>
            {
                Assert.IsInstanceOfType(r, typeof(IDictionary<dynamic, dynamic>));
                var a = new object[] { 2, 2 };
                var b = new object[] { 1, 1 };

                foreach(var kvp in r)
                {
                    var valid = false;

                    var k = kvp.Key;
                    var v = kvp.Value;
                    Assert.IsInstanceOfType(k, typeof(object[]));

                    if (k[0] == 2 && k[1] == 2 && v == "two") valid = true;
                    else if (k[0] == 1 && k[1] == 1 && v == "one") valid = true;

                    Assert.IsTrue(valid);
                }
            });
        }

        [TestMethod]
        public void Reader_Map_10()
        {
            Utils.RunRead("map_10_items", r =>
            {
                Assert.IsInstanceOfType(r, typeof(IDictionary<dynamic, dynamic>));

                var a = new Dictionary<dynamic, dynamic>();
                foreach (var i in Enumerable.Range(0, 10))
                {
                    var keyword = new Keyword($"key{i:0000}");
                    a.Add(keyword, i);
                };
                foreach (var kvp in a) Assert.AreEqual(r[kvp.Key], kvp.Value);
            });
        }

        [TestMethod]
        public void Reader_Map_10_Nested()
        {
            Utils.RunRead("map_10_nested", r =>
            {
                Assert.IsInstanceOfType(r, typeof(IDictionary<dynamic, dynamic>));

                var a = new Dictionary<dynamic, dynamic>();
                foreach(var i in Enumerable.Range(0,10))
                {
                    var keyword = new Keyword($"key{i:0000}");
                    a.Add(keyword, i);
                };

                var f = r[new Keyword("f")];
                foreach (var kvp in a) Assert.AreEqual(f[kvp.Key], kvp.Value);

                var s = r[new Keyword("s")];
                foreach (var kvp in a) Assert.AreEqual(s[kvp.Key], kvp.Value);
            });
        }
        
        [TestMethod]
        public void Reader_Map_1935_Nested()
        {
            Utils.RunRead("map_1935_nested", r =>
            {
                Assert.IsInstanceOfType(r, typeof(IDictionary<dynamic, dynamic>));

                var a = new Dictionary<dynamic, dynamic>();
                foreach (var i in Enumerable.Range(0, 1935))
                {
                    var keyword = new Keyword($"key{i:0000}");
                    a.Add(keyword, i);
                };

                var f = r[new Keyword("f")];
                foreach (var kvp in a) Assert.AreEqual(f[kvp.Key], kvp.Value);

                var s = r[new Keyword("s")];
                foreach (var kvp in a) Assert.AreEqual(s[kvp.Key], kvp.Value);
            });
        }

        [TestMethod]
        public void Reader_Map_1936_Nested()
        {
            Utils.RunRead("map_1936_nested", r =>
            {
                Assert.IsInstanceOfType(r, typeof(IDictionary<dynamic, dynamic>));

                var a = new Dictionary<dynamic, dynamic>();
                foreach (var i in Enumerable.Range(0, 1936))
                {
                    var keyword = new Keyword($"key{i:0000}");
                    a.Add(keyword, i);
                };

                var f = r[new Keyword("f")];
                foreach (var kvp in a) Assert.AreEqual(f[kvp.Key], kvp.Value);

                var s = r[new Keyword("s")];
                foreach (var kvp in a) Assert.AreEqual(s[kvp.Key], kvp.Value);
            });
        }

        [TestMethod]
        public void Reader_Map_1937_Nested()
        {
            Utils.RunRead("map_1937_nested", r =>
            {
                Assert.IsInstanceOfType(r, typeof(IDictionary<dynamic, dynamic>));

                var a = new Dictionary<dynamic, dynamic>();
                foreach (var i in Enumerable.Range(0, 1937))
                {
                    var keyword = new Keyword($"key{i:0000}");
                    a.Add(keyword, i);
                };

                var f = r[new Keyword("f")];
                foreach (var kvp in a) Assert.AreEqual(f[kvp.Key], kvp.Value);

                var s = r[new Keyword("s")];
                foreach (var kvp in a) Assert.AreEqual(s[kvp.Key], kvp.Value);
            });
        }

        [TestMethod]
        public void Reader_Vector_2charSymbol_Maps()
        {
            Utils.RunRead("maps_two_char_sym_keys", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                var aa = new Symbol("aa");
                var bb = new Symbol("bb");
                Assert.AreEqual(r[0][aa], 1);
                Assert.AreEqual(r[0][bb], 2);
                Assert.AreEqual(r[1][aa], 3);
                Assert.AreEqual(r[1][bb], 4);
                Assert.AreEqual(r[2][aa], 5);
                Assert.AreEqual(r[2][bb], 6);
            });
        }

        [TestMethod]
        public void Reader_Vector_3charSymbol_Maps()
        {
            Utils.RunRead("maps_three_char_sym_keys", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                var aaa = new Symbol("aaa");
                var bbb = new Symbol("bbb");
                Assert.AreEqual(r[0][bbb], 2);
                Assert.AreEqual(r[0][aaa], 1);
                Assert.AreEqual(r[1][bbb], 4);
                Assert.AreEqual(r[1][aaa], 3);
                Assert.AreEqual(r[2][bbb], 6);
                Assert.AreEqual(r[2][aaa], 5);
            });
        }

        [TestMethod]
        public void Reader_Vector_4charSymbol_Maps()
        {
            Utils.RunRead("maps_four_char_sym_keys", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                var aaaa = new Symbol("aaaa");
                var bbbb = new Symbol("bbbb");
                Assert.AreEqual(r[0][bbbb], 2);
                Assert.AreEqual(r[0][aaaa], 1);
                Assert.AreEqual(r[1][bbbb], 4);
                Assert.AreEqual(r[1][aaaa], 3);
                Assert.AreEqual(r[2][bbbb], 6);
                Assert.AreEqual(r[2][aaaa], 5);
            });
        }

        [TestMethod]
        public void Reader_Vector_2charString_Maps()
        {
            Utils.RunRead("maps_two_char_string_keys", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                var aa = "aa";
                var bb = "bb";
                Assert.AreEqual(r[0][aa], 1);
                Assert.AreEqual(r[0][bb], 2);
                Assert.AreEqual(r[1][aa], 3);
                Assert.AreEqual(r[1][bb], 4);
                Assert.AreEqual(r[2][aa], 5);
                Assert.AreEqual(r[2][bb], 6);
            });
        }

        [TestMethod]
        public void Reader_Vector_3chaString_Maps()
        {
            Utils.RunRead("maps_three_char_string_keys", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                var aaa = "aaa";
                var bbb = "bbb";
                Assert.AreEqual(r[0][bbb], 2);
                Assert.AreEqual(r[0][aaa], 1);
                Assert.AreEqual(r[1][bbb], 4);
                Assert.AreEqual(r[1][aaa], 3);
                Assert.AreEqual(r[2][bbb], 6);
                Assert.AreEqual(r[2][aaa], 5);
            });
        }

        [TestMethod]
        public void Reader_Vector_4charStringy_Maps()
        {
            Utils.RunRead("maps_four_char_string_keys", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                var aaaa = "aaaa";
                var bbbb = "bbbb";
                Assert.AreEqual(r[0][bbbb], 2);
                Assert.AreEqual(r[0][aaaa], 1);
                Assert.AreEqual(r[1][bbbb], 4);
                Assert.AreEqual(r[1][aaaa], 3);
                Assert.AreEqual(r[2][bbbb], 6);
                Assert.AreEqual(r[2][aaaa], 5);
            });
        }

        [TestMethod]
        public void Reader_Map_Unrecognized()
        {
            Utils.RunRead("map_unrecognized_vals", r =>
            {
                Assert.IsInstanceOfType(r, typeof(IDictionary<dynamic,dynamic>));
                var k = new Keyword("key");
                var v = r[k];
                Assert.AreEqual(v, "~Unrecognized");
            });
        }

        [TestMethod]
        public void Reader_Vector_Keywords_1935_Repeated()
        {
            Utils.RunRead("vector_1935_keywords_repeated_twice", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                var max = 1935;
                foreach (var j in Enumerable.Range(0, 2))
                {
                    foreach (var i in Enumerable.Range(0, max))
                    {
                        var expected = new Keyword($"key{i:0000}");
                        var actual = r[j*max + i];
                        Assert.AreEqual(expected,actual);
                    }
                }
            });
        }

        [TestMethod]
        public void Reader_Vector_Keywords_1936_Repeated()
        {
            Utils.RunRead("vector_1936_keywords_repeated_twice", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                var max = 1936;
                foreach (var j in Enumerable.Range(0, 2))
                {
                    foreach (var i in Enumerable.Range(0, max))
                    {
                        var expected = new Keyword($"key{i:0000}");
                        var actual = r[j * max + i];
                        Assert.AreEqual(expected, actual);
                    }
                }
            });
        }

        [TestMethod]
        public void Reader_Vector_Keywords_1937_Repeated()
        {
            Utils.RunRead("vector_1937_keywords_repeated_twice", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                var max = 1937;
                foreach (var j in Enumerable.Range(0, 2))
                {
                    foreach (var i in Enumerable.Range(0, max))
                    {
                        var expected = new Keyword($"key{i:0000}");
                        var actual = r[j * max + i];
                        Assert.AreEqual(expected, actual);
                    }
                }
            });
        }

        [TestMethod]
        public void Reader_Vector_Unrecognized()
        {
            Utils.RunRead("vector_unrecognized_vals", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                var v = r[0];
                Assert.AreEqual(v, "~Unrecognized");
            });
        }

        [TestMethod]
        public void Reader_Double_Special()
        {
            Utils.RunRead("vector_special_numbers", r =>
            {
                Assert.IsInstanceOfType(r, typeof(object[]));
                var o = new[] { double.NaN, double.PositiveInfinity, double.NegativeInfinity };
                foreach (var i in Enumerable.Range(0, o.Length))
                {
                    var expected = o[i];
                    var actual = r[i];
                    Assert.AreEqual(expected,actual);
                }
            });
        }
    }
}
