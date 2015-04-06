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
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Int_InterestingNegative()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Double_Small()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Double_Interesting()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Uuid_One()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Uuid_List()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Uri_One()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Writer_Uri_List()
        {
            Assert.IsTrue(false);
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
            Assert.IsTrue(false);
        }
    }
}
