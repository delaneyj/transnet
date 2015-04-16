using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TransNET
{
    public enum TransitType
    {
        Null,
        String,
        Integer,
        UnsignedInteger,
        Long,
        Double,
        DateTime,
        Guid,
        Array,
        List,
        Dictionary,
        Set
    }

    public class Schema
    {
        public readonly string Description;

        public readonly TransitType TransitType;
        private readonly Dictionary<string, Schema> children = new Dictionary<string, Schema>();
        public Action<string> FieldNotFound = null;
        public Action<string,object> FieldPostValidation = null;

        public static Schema Parse(string schemaJson)
        {
            var jsonSchema = JObject.Parse(schemaJson);
            return new Schema(jsonSchema);
        }

        public Schema(JObject jsonSchema)
        {
            Description = jsonSchema.Value<string>("description");
            var type = jsonSchema.Value<string>("type");
            TransitType = (TransitType)Enum.Parse(typeof(TransitType), type);

            switch(TransitType)
            {
                case TransitType.Dictionary:
                {
                    var properties = jsonSchema.Value<JObject>("properties");
                    foreach(var kvp in properties)
                    {
                        var name = kvp.Key;
                        var value = new Schema((JObject)kvp.Value);
                        children.Add(name, value);
                    }
                    break;
                }
            case TransitType.Array:
                {
                    var items = jsonSchema.Value<JObject>("items");
                    var arraySchema = new Schema(items);
                    children.Add("array",arraySchema);
                    break;
                }
                default: break;
            }
        }

        public bool IsValid(object o)
        {
            switch (TransitType)
            {
            case TransitType.Null: return o == null;
            case TransitType.String: return o is string;
            case TransitType.Integer: return o is int;
            case TransitType.UnsignedInteger: return o is uint;
            case TransitType.Long:return o is long;
            case TransitType.Double: return o is float || o is double;
            case TransitType.DateTime: return o is DateTime;
            case TransitType.Guid: return o is Guid;            
            case TransitType.Array:
                {
                    var array = o as Array;
                    if (array == null) return false;

                    var arraySchema = children["array"];
                    foreach (var i in array)
                    {
                        if (!arraySchema.IsValid(i)) return false;
                    }

                    return true;
                }
            case TransitType.Dictionary:
                {
                    if (o is IDictionary<object, object>)
                    {
                        Debugger.Break();
                        return false;
                    }
                    else
                    {
                        foreach (var prop in o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                        {
                            var name = prop.Name;
                            var value = prop.GetValue(o);

                            Schema childSchema;
                            if (!children.TryGetValue(name, out childSchema)) 
                            {
                                if (FieldNotFound != null) FieldNotFound(name);
                                continue;
                            }
                            else
                            { 
                                var childValid = childSchema.IsValid(value);
                                if (childValid) HandlePostValidation(name,value);
                                else return false;
                            }                           
                        }

                        return true;
                    }
                }
            default: Debugger.Break(); throw new NotImplementedException();
            }

            return false;
        }

        private void HandlePostValidation(string name, object value)
        {
            if(FieldPostValidation != null) FieldPostValidation(name, value);
        }

        public static Schema FromObject(dynamic o)
        {
            return null;
        }
    }
}
