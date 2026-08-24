using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Xml.Linq;

namespace CommLib
{
    public static class Comm
    {

        /// <summary>
        /// 文字转义HTML
        /// </summary>
        /// <param name="sText"></param>
        /// <returns></returns>
        public static string HTMLEncode(this string sText)
        {
            string htmlEncoded = WebUtility.HtmlEncode(sText);
            htmlEncoded = htmlEncoded.Replace(" ", "&nbsp;");
            return htmlEncoded;

        }

        /// <summary>
        /// HTML转义文字
        /// </summary>
        /// <param name="sHTML"></param>
        /// <returns></returns>
        public static string HTMLDeCode(this string sHTML)
        {
            string st = WebUtility.HtmlDecode(sHTML);
            st = st.Replace("&nbsp;", " ");
            return st;

        }


        /// <summary>
        /// 数字转在中文数字
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToCNName(this int d)
        {
            if (d < 0) return "";
            char[] n = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
            char[] s = ['零', '一', '二', '三', '四', '五', '六', '七', '八', '九'];
            string str = d.ToString();
            string sv = "";
            foreach (char c in str)
            {
                sv += s[Array.IndexOf(n, c)];
            }
            return sv;
        }




        /// <summary>
        /// 【擴充方法】將 System.Drawing.Color 完美轉換為網頁標準的 HEX 字串（如 #1BD1A5）
        /// </summary>
        /// <param name="color">System.Drawing.Color 實例</param>
        /// <param name="includeAlpha">是否包含透明度通道（預設不包含，返回 6 碼 #RRGGBB）</param>
        public static string ToHex(this Color color, bool includeAlpha = false)
        {
            if (includeAlpha)
            {
                // 返回 8 碼（包含透明度）：#AARRGGBB
                return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
            }
            else
            {
                // 返回常規網頁 6 碼：#RRGGBB
                return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
            }
        }

        /// <summary>
        /// 获得标志（Flags）枚举实例中包含多少个具体的枚举值集合。
        /// </summary>
        /// <param name="enumConstant">枚举实例常量。</param>
        /// <returns>返回包含的枚举整型数值集合。</returns>
        public static List<int> GetEnumValues(this Enum enumConstant)
        {
            var iL = new List<int>();
            int constantValue = Convert.ToInt32(enumConstant);

            foreach (var testEnum in Enum.GetValues(enumConstant.GetType()))
            {
                int testValue = Convert.ToInt32(testEnum);
                // 模拟 VB.NET 的位与运算 (EnumConstant And testEnum)
                if ((constantValue & testValue) == testValue && testValue != 0)
                {
                    iL.Add(testValue);
                }
            }

            return iL;
        }

        /// <summary>
        /// 返回枚举类型的说明和对应数值的字典集合。
        /// </summary>
        /// <param name="enumType">枚举的运行时类型，可用 <c>typeof(EnumType)</c> 获取。</param>
        /// <param name="isGetUserHide">是否获取用户隐藏的枚举项，默认为 <see langword="false"/>。</param>
        /// <param name="isRemoveHtml">Ref 开关：是否移除说明中的 HTML 标签，默认为 <see langword="false"/>。</param>
        /// <returns>返回以枚举描述为键、枚举整型数值为值的字典集合。</returns>
        public static Dictionary<string, int> EnumToDict(Type enumType, bool isGetUserHide = false, bool isRemoveHtml = false)
        {
            var dc = new Dictionary<string, int>();

            foreach (Enum testEnum in Enum.GetValues(enumType))
            {
                // 注意：此处调用了下面定义的扩展方法
                string sN = testEnum.GetEnumDescription(opEQ: true);
                if (isRemoveHtml)
                {
                    sN = RemoveHtmlTag(sN);
                }

                FieldInfo field = enumType.GetField(testEnum.ToString());
                var cd = (CategoryAttribute[])field.GetCustomAttributes(typeof(CategoryAttribute), false);
                string sJSON = cd.Length > 0 ? cd[0].Category : "";
                int iv = Convert.ToInt32(testEnum);


                if (!string.IsNullOrWhiteSpace(sJSON))
                {
                    JObject jO = JsonConvert.DeserializeObject<JObject>(sJSON);
                    string ivStr = jO["IsUserHide"]?.ToString();

                    if (!int.TryParse(ivStr, out int ih))
                    {
                        ih = 0;
                    }

                    if (ih == 0 && isGetUserHide)
                    {
                        dc.Add(sN, iv);
                    }
                    else if (iv > 0)
                    {
                        dc.Add(sN, iv);
                    }
                }
                else
                {
                    dc.Add(sN, iv);
                }
            }

            return dc;
        }

        /// <summary>
        /// 返回当前标志枚举实例中所包含的有效枚举项的说明和数值字典。
        /// </summary>
        /// <param name="enumConstant">枚举实例常量。</param>
        /// <returns>返回符合位运算匹配的枚举描述与数值字典。</returns>
        public static Dictionary<string, int> GetEnumDict(this Enum enumConstant)
        {
            var dc = new Dictionary<string, int>();

            foreach (Enum testEnum in Enum.GetValues(enumConstant.GetType()))
            {
                if (enumConstant.HasFlag(testEnum) && Convert.ToInt32(testEnum) != 0)
                {
                    string sN = testEnum.GetEnumDescription(opEQ: true);
                    dc.Add(sN, Convert.ToInt32(testEnum));
                }
            }

            return dc;
        }

        /// <summary>
        /// 获得枚举的说明，可分权重（Level）进行排序或过滤显示。
        /// </summary>
        /// <param name="enumConstant">枚举实例常量。</param>
        /// <param name="splitChar">分隔符，默认项为逗号。</param>
        /// <param name="isReturnMaxLevelOnly">是否仅返回最大权重值。可在枚举项上使用 <c>[Category("{'level':2}")]</c> 确定权重。若为 <see langword="false"/> 则返回全部有效说明。</param>
        /// <param name="includeZero">是否附加包含零（0）的说明。若为 <see langword="false"/>，则只有在枚举值本身为 0 时才显示默认说明。</param>
        /// <param name="hasValue">显示内容是否包含数据值前缀（例如：1.说明）。</param>
        /// <param name="opEQ">是否使用相等操作符判定。若为 <see langword="true"/> 则使用等于判定，默认为 <see langword="false"/>（即使用位运算）。</param>
        /// <param name="orderbyLevelDesc">是否按照 JSON 配置中的 LEVEL 属性进行降序排序。</param>
        /// <returns>返回组合后的枚举描述字符串。</returns>
        public static string GetEnumDescription(
            this Enum enumConstant,
            string splitChar = ",",
            bool isReturnMaxLevelOnly = false,
            bool includeZero = false,
            bool hasValue = false,
            bool opEQ = false,
            bool orderbyLevelDesc = false)
        {
            var iL = new List<Enum>();
            int constantValue = Convert.ToInt32(enumConstant);
            Type enumType = enumConstant.GetType();

            if (constantValue != 0)
            {
                foreach (Enum testEnum in Enum.GetValues(enumType))
                {
                    int testValue = Convert.ToInt32(testEnum);
                    if (opEQ)
                    {
                        if (Equals(enumConstant, testEnum)) iL.Add(testEnum);
                    }
                    else
                    {
                        if ((constantValue & testValue) == testValue && testValue != 0) iL.Add(testEnum);
                    }
                }
            }
            else if (includeZero)
            {
                FieldInfo field = enumType.GetField(enumConstant.ToString());
                if (field != null)
                {
                    var attr = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    return attr.Length > 0 ? attr[0].Description : enumConstant.ToString();
                }
                return enumConstant.ToString();
            }

            if (orderbyLevelDesc)
            {
                var dc = new Dictionary<Enum, string>();
                foreach (var ev in iL)
                {
                    FieldInfo field = enumType.GetField(ev.ToString());
                    var cd = (CategoryAttribute[])field.GetCustomAttributes(typeof(CategoryAttribute), false);
                    string sJSON = cd.Length > 0 ? cd[0].Category : "";

                    dc.Add(ev, "");
                    JToken jt = ConvertJson(sJSON);
                    if (jt != null && jt.Type == JTokenType.Object)
                    {
                        var jO = (JObject)jt;
                        if (jO.ContainsKey("level"))
                        {
                            dc[ev] = jO["level"]?.ToString() ?? "";
                        }
                    }
                }

                if (dc.Count > 0)
                {
                    var rs = dc.OrderByDescending(kv => kv.Value).ToList();
                    iL = rs.Select(kv => kv.Key).ToList();
                }
            }

            string s = "";
            int iMax = 0;
            Enum eMax = enumConstant;

            foreach (var ev in iL)
            {
                FieldInfo field = enumType.GetField(ev.ToString());
                var attr = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                string sNV = attr.Length > 0 ? attr[0].Description : ev.ToString();

                if (hasValue && !string.IsNullOrEmpty(sNV))
                {
                    // 转换原 VB XML 节点解析逻辑
                    XDocument xD = XDocument.Parse(sNV);
                    XElement xE = xD.Root; // 原 VB FirstNode 对应 Root 节点安全读取
                    if (xE != null)
                    {
                        xE.Value = Convert.ToInt32(ev) + "." + xE.Value;
                        sNV = xE.ToString();
                    }
                }

                s += splitChar + sNV;

                if (isReturnMaxLevelOnly)
                {
                    var cd = (CategoryAttribute[])field.GetCustomAttributes(typeof(CategoryAttribute), false);
                    string sJSON = cd.Length > 0 ? cd[0].Category : "";
                    if (!string.IsNullOrWhiteSpace(sJSON))
                    {
                        JObject jO = JsonConvert.DeserializeObject<JObject>(sJSON);
                        string ivStr = jO["level"]?.ToString();
                        if (int.TryParse(ivStr, out int iv))
                        {
                            if (iMax < iv)
                            {
                                iMax = iv;
                                eMax = ev;
                            }
                        }
                    }
                }
            }

            if (isReturnMaxLevelOnly)
            {
                return eMax.GetEnumDescription(splitChar, false, includeZero, hasValue, opEQ, false);
            }

            if (!string.IsNullOrEmpty(s))
            {
                s = s.Substring(splitChar.Length);
            }

            return s;
        }

        #region 外部依赖占位方法（请根据您项目中实际的工具类进行对接）

        /// <summary>
        /// 移除字符串中的 HTML 标签。
        /// </summary>
        private static string RemoveHtmlTag(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            // 示例：此处应当使用您的现有公共库，或使用正则过滤
            return System.Text.RegularExpressions.Regex.Replace(input, "<[^>]*>", "");
        }

        /// <summary>
        /// 将字符串安全转换为 JToken。
        /// </summary>
        private static JToken? ConvertJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            try
            {
                return JToken.Parse(json);
            }
            catch
            {
                return null;
            }
        }

        #endregion



        /// <summary>
        /// 如果转换失败将会返回Guid.Empty
        /// </summary>
        /// <param name="sv">要转换的字符串</param>
        /// <returns></returns>
        public static Guid ConvertGUID(string sv)
        {

            if (string.IsNullOrWhiteSpace(sv)) return Guid.Empty;
            if (Guid.TryParse(sv.Trim(), out Guid g))
            {
                return g;
            }

            return Guid.Empty;
        }

    }
}
