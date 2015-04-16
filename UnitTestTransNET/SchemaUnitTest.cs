using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransNET;

namespace UnitTestTransNET
{
    [TestClass]
    public class SchemaUnitTest
    {
        [TestMethod]
        public void Schema_ParseBasic()
        {
            var schema = Schema.Parse(@"{
    'description':'a schema',
    'type':'Dictionary',
    'properties':{
        'name':{ 'type':'String' },
        'address':{
            'type':'Array',
            'items': { 'type':'String'}
        },
    }
}");
            schema.FieldNotFound = name =>
            {
                Assert.AreEqual(name, "foo");
            };

            schema.FieldPostValidation = (name, value) =>
            {
                switch(name)
                {
                case "name": Assert.AreEqual(value, "Foo"); break;
                default: break;
                }
            };

            Assert.IsTrue(schema.IsValid(new
            {
                name = "Foo",
                address = new[] { "foo", "bar" },
                foo = 123
            }));
        }
    }
}
