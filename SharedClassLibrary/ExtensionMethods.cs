﻿using Newtonsoft.Json;
using System.Data;
using System.IO.Compression;
using System.Reflection;
using System.Text;

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

        public static T CopyObject<T>(this T obj) where T : class, new()
        {
            string clone = obj.TypeToJson();
            return clone.JsonToType<T>();
        }

        public static TC DownCast<TP, TC>(this TP parent) where TC : TP, new()
        {
            string clone = parent.TypeToJson();
            TC? type = JsonConvert.DeserializeObject<TC>(clone);
            if (type is not null)
                return type;
            else
                return new();
        }

        public static string Compress(this string str)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(str);
            using MemoryStream msgIn = new(bytes);
            using MemoryStream msgOut = new();
            using (GZipStream gZipMsg = new(msgOut, CompressionMode.Compress))
            {
                msgIn.CopyTo(gZipMsg);
            }
            return Convert.ToBase64String(msgOut.ToArray());
        }

        public static string Decompress(this string str)
        {
            byte[] bytes = Convert.FromBase64String(str);
            using MemoryStream msgIn = new(bytes);
            using MemoryStream msgOut = new();
            using (var gs = new GZipStream(msgIn, CompressionMode.Decompress))
            {
                gs.CopyTo(msgOut);
            }
            return Encoding.Unicode.GetString(msgOut.ToArray());
        }
    }

}
