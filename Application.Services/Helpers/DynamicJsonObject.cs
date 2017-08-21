using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Globalization;

namespace Application.Services.Helpers
{
    public class DynamicJsonObject : DynamicObject
    {
        private readonly IDictionary<string, string> _values;

        public DynamicJsonObject(IDictionary<string, string> values)
        {
            Debug.Assert(values != null);
            _values = values.ToDictionary(p => p.Key, p => JsonConvert.SerializeObject(p.Value), StringComparer.OrdinalIgnoreCase);
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = null;

            if (binder.Type.IsAssignableFrom(_values.GetType()))
            {
                result = _values;
            }
            else
            {
                throw new InvalidOperationException();
            }

            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = GetValue(binder.Name);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _values[binder.Name] = JsonConvert.SerializeObject(value);
            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            string key = GetKey(indexes);
            if (!String.IsNullOrEmpty(key))
            {
                _values[key] = JsonConvert.SerializeObject(value);
            }
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            string key = GetKey(indexes);
            result = null;
            if (!String.IsNullOrEmpty(key))
            {
                result = GetValue(key);
            }
            return true;
        }

        private static string GetKey(object[] indexes)
        {
            if (indexes.Length == 1)
            {
                return (string)indexes[0];
            }
            return null;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _values.Keys;
        }

        private object GetValue(string name)
        {
            string result;
            if (_values.TryGetValue(name, out result))
            {
                return result;
            }
            return null;
        }
    }
}
