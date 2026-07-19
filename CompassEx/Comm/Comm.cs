// // Copyright (c) 2026 [Jockeyvb]. All rights reserved.
// // 
// // This file is part of [CompassEx].
// // [CompassEx] is free software: you can redistribute it and/or modify
// // it under the terms of the GNU Affero General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// //
// // For commercial use, you must obtain a commercial license from the author.
// // Contact: [Jockeyvb@gmail.com/微信:Jockeyvb1]
//

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace CompassEx.Comm
{
    /// <summary>
    /// 类库核心公共辅助工具类，提供全局初始化入口及基于反射的对象属性浅拷贝扩展。
    /// </summary>
    public static class Comm
    {

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

                if (!string.IsNullOrWhiteSpace(sJSON))
                {
                    JObject jO = JsonConvert.DeserializeObject<JObject>(sJSON);
                    string ivStr = jO["IsUserHide"]?.ToString();

                    if (!int.TryParse(ivStr, out int iv))
                    {
                        iv = 0;
                    }

                    if (iv == 0 || isGetUserHide)
                    {
                        dc.Add(sN, Convert.ToInt32(testEnum));
                    }
                }
                else
                {
                    dc.Add(sN, Convert.ToInt32(testEnum));
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
        private static JToken ConvertJson(string json)
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
        /// 全自动初始化罗盘引擎中赖以运行的所有底层基本数据。
        /// </summary>
        /// <remarks>
        /// <b>初始化加载名堂：</b><br/>
        /// 该方法在系统启动或反序列化时应被优先调用。它会通过内部流水线依次唤醒并充填两套最核心的周天度数数据：
        /// <list type="bullet">
        /// <item><description>调用 <see cref="C3Y.LoadAllCAfterGuas"/>：全量装载后天六十四卦罗盘圈层分度范围数据。</description></item>
        /// <item><description>调用 <see cref="C3Y.LoadAllCBeforeGuas"/>：全量装载伏羲先天六十四卦方圆图周天物理刻度数据。</description></item>
        /// </list>
        /// </remarks>
        public static void AllInit()
        {
            C3Y.LoadAllCAfterGuas();
            C3Y.LoadAllCBeforeGuas();


        }

        /// <summary>
        /// 扩展方法：利用运行时反射（Reflection）机制，强行从父级基类模板中将所有属性与字段的值“浅拷贝”反灌给当前继承类实例。
        /// </summary>
        /// <typeparam name="TChild">继承类（派生类）的具体类型，必须隐式继承自 <typeparamref name="TBase"/>。</typeparam>
        /// <typeparam name="TBase">基类（模板类）的具体类型。</typeparam>
        /// <param name="child">正在接受赋值的当前继承类（子类）目标对象实例。</param>
        /// <param name="baseTemplate">作为数据源的基类（父类）实体模板对象。</param>
        /// <remarks>
        /// <b>🛠️ 内部反射拷贝门道与避坑提示：</b>
        /// <list type="number">
        /// <item><description><b>属性遍历</b>：方法首先通过 <c>typeof(TBase).GetProperties()</c> 捕获父类公开属性，并通过 <c>prop.CanWrite</c> 安全拦截，防止对只读属性或受保护的 Get 块执行非法注入。</description></item>
        /// <item><description><b>字段反灌</b>：随后通过 <c>typeof(TBase).GetFields()</c> 递归提取底层物理字段并完成原子值覆写。</description></item>
        /// <item><description><b>⚠️ 性能与安全警告</b>：由于使用了运行时动态反射，该操作会带来较大的 CPU 耗能，<b>在高频循环或深层装卦排盘时应克制使用</b>。另外，它仅支持第一层浅拷贝，若字段中含有 <c>List&lt;T&gt;</c> 等引用类型，两端实例会共享同一份内存地址，修改时存在联动篡改的隐患，在跨变卦深度计算时需高度注意。</description></item>
        /// </list>
        /// 该方法常用于在 JSON 反序列化后期生命周期中，快速将多字段的静态基类参数直接克隆给派生实体类。
        /// </remarks>
        public static void ApplyBaseProperties<TChild, TBase>(this TChild child, TBase baseTemplate)
            where TChild : TBase
        {
            // 用反射复制（慎用，性能差且容易出错）
            foreach (var prop in typeof(TBase).GetProperties())
            {
                if (prop.CanWrite)
                {
                    prop.SetValue(child, prop.GetValue(baseTemplate));
                }
            }

            foreach (var f in typeof(TBase).GetFields())
            {
                f.SetValue(child, f.GetValue(baseTemplate));
            }
        }
    }

    /// <summary>
    /// 针对一维数组的高性能公共流式扩展方法封装类。
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// 扩展方法：在给定的强类型一维数组中，高效正向检索特定元素并返回其首次出现的零基（Zero-based）索引。
        /// </summary>
        /// <typeparam name="T">数组内部元素的泛型类型。</typeparam>
        /// <param name="array">当前正在执行检索的目标一维数组实体。</param>
        /// <param name="value">期望在数组中匹配定位的目标对象或数值。</param>
        /// <returns>返回匹配项在数组中的绝对索引位置（范围在 <c>0</c> 到 <c>Length - 1</c> 之间）；若全盘未匹配成功，则返回标准未找到标识 <c>-1</c>。</returns>
        /// <remarks>
        /// 该方法是对原生静态函数 <see cref="Array.IndexOf{T}(T[], T)"/> 的流式桥接封装，允许类库在内部对静态数组数据（如干支集、卦序表）直接像调用 List 一样使用流畅的 <c>array.IndexOf(value)</c> 语法，从而大幅减少语法噪音。
        /// </remarks>
        public static int IndexOf<T>(this T[] array, T value)
        {
            return Array.IndexOf(array, value);
        }
    }
}

