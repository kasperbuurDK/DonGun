using System.Data;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Text.Json;
using DevExpress.DirectX.Common;
using Newtonsoft.Json;

namespace SharedClassLibrary
{
    public static class ExtensionMethods
    {
        public static float Remap(this int value, int from1, int to1, float from2, float to2)
        {
            return ((float)value).Remap(from1, to1, from2, to2);
        }

        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static List<T>? ToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new();
                foreach (var row in table.AsEnumerable())
                {
                    T obj = new();
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo? propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo?.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    list.Add(obj);
                }
                return list;
            }
            catch
            {
                return default;
            }
        }

        public static T JsonToType<T>(this string data) where T : class, new()
        {
            T? type = JsonConvert.DeserializeObject<T>(data);
            if (type is not null)
                return type;
            else
                return new();
        }

        public static string TypeToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static bool PopulateObjectWithJson<T>(this T obj, string j)
        {

            if (obj is null) return false;
            
            JsonConvert.PopulateObject(j, obj);
            return true;
        }

        public static T CopyObject<T>(this T obj) where T : class, new()
        {
            string clone = obj.TypeToJson();
            return clone.JsonToType<T>(); 
        }
    }

}
