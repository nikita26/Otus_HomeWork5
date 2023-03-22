using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Otus_HomeWork5.CSV
{
    /// <summary>
    /// Конвертер для работы с CSV форматом
    /// </summary>
    public static class CSVConver
    {
        /// <summary>
        /// Объект в CSV строку
        /// </summary>
        /// <returns>Строка в CSV формате</returns>
        public static string SerializeObject(object obj)
        {
            List<string> result = new List<string>();

            var type = obj.GetType();
            var publicProps = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var p in publicProps)
            {
                var field = p.Name;
                var pValue = p.GetValue(obj);

                result.Add($"{p.Name}:{pValue}");
            }
            return String.Join(';', result);
        }

        /// <summary>
        /// CSV строка в объект
        /// </summary>
        /// <returns>Объект с указанным типом</returns>
        public static T DeserializeObject<T>(string str) where T : new()
        {
            var result = new T();
            var type = typeof(T);

            var fields = str.Split(';');
            foreach (var field in fields)
            {
                var data = field.Split(":");
                var prop = type.GetProperty(data[0], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (prop != null)
                {
                    var t = Type.GetType(prop.PropertyType.FullName);
                    if (t == typeof(double))
                        data[1] = data[1].Replace(',', '.');
                    var value = TypeDescriptor.GetConverter(t).ConvertFromInvariantString(data[1]);
                    prop.SetValue(result, value);
                }
            }
            return result;
        }
    }
}
