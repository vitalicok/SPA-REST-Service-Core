using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Application.Services.Helpers
{
    public static class ConversionUtil
    {

        private static MethodInfo _stringToEnumMethod;

        public static string ToString<T>(T obj)
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return obj.ToString();
            }
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            if ((converter != null) && converter.CanConvertTo(typeof(string)))
            {
                return converter.ConvertToInvariantString(obj);
            }
            return null;
        }
    }
}
