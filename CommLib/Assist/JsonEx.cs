using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace CommLib.Assist;


/// <summary>
/// 序列化扩展类
/// </summary>
public static class JsonEx
{

    //============================序列化时动态忽略指定属性名的转换器===========================
    /*
     * var settings = new JsonSerializerSettings
{
    Converters = { new IgnorePropertiesConverter("Gua", "Parent", "Items") }
};

var json = JsonConvert.SerializeObject(bigGua, settings);
     * 
     */

    /// <summary>
    /// 序列化时动态忽略指定属性名
    /// </summary>
    public class IgnorePropertiesConverter : JsonConverter
    {
        /// <summary>
        /// 要忽略的属性名集合（不区分大小写）
        /// </summary>
        public HashSet<string> IgnoreProperties { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public IgnorePropertiesConverter(params string[] propertiesToIgnore)
        {
            IgnoreProperties = new HashSet<string>(propertiesToIgnore, StringComparer.OrdinalIgnoreCase);
        }

        public override bool CanRead => false; // 反序列化走原生

        public override bool CanConvert(Type objectType)
            => true; // 所有类型都走，内部判断属性名

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var type = value.GetType();

            // 基础类型直接写值
            if (type.IsPrimitive || value is string)
            {
                writer.WriteValue(value);
                return;
            }

            // 集合：手动遍历，每个元素递归
            if (value is System.Collections.IEnumerable enumerable && !(value is System.Collections.IDictionary) && !(value is string))
            {
                writer.WriteStartArray();
                foreach (var item in enumerable)
                {
                    serializer.Serialize(writer, item);
                }
                writer.WriteEndArray();
                return;
            }

            // 复杂对象：遍历属性，跳过忽略列表
            writer.WriteStartObject();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead
                            && !IgnoreProperties.Contains(p.Name)  // ← 核心：跳过指定属性
                            && !p.GetIndexParameters().Any());

            foreach (var prop in properties)
            {
                var propValue = prop.GetValue(value);
                writer.WritePropertyName(prop.Name);

                if (propValue == null)
                {
                    writer.WriteNull();
                    continue;
                }

                serializer.Serialize(writer, propValue);
            }
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            => serializer.Deserialize(reader, objectType);
    }
    //============================序列化时动态忽略指定属性名的转换器===========================

    //================================转换Color类===================================
    /// <summary>
    /// 静态临时转换是否为Web Css #RRGGBBAA 格式,为false时： #AARRGGBB
    /// </summary>
    public static bool IsConvertWebColor = true;
    public class ColorJsonConverter : JsonConverter<Color>
    {
        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToHex(IsToWeb: IsConvertWebColor));   // Xamarin.Forms.Color 有 ToHex() 扩展方法
        }

        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string hex = reader?.Value?.ToString()!;
            return string.IsNullOrWhiteSpace(hex) ? Color.Transparent : Comm.ColorFromHex(hex, IsConvertWebColor);
        }
    }



    //================================转换Color类===================================
}
