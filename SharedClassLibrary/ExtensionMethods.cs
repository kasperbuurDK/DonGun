using System.Data;
using System.Reflection;

namespace SharedClassLibrary
{
    public static class ExtensionMethods
    {
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
    }

}
