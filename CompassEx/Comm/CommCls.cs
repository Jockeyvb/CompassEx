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

using CompassEx.Comm;
using CompassEx.Gua;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using tyme.lunar;
using tyme.sixtycycle;
using tyme.solar;

namespace CompassEx
{
    /// <summary>
    /// 类库核心公共辅助工具类，提供全局初始化入口及基于反射的对象属性浅拷贝扩展。
    /// </summary>
    public static class CommCls
    {

        /// <summary>
        ///全部符号集合
        /// </summary>
        public static List<string> AllSymbols => GuaSubClass.Symbols.ToList().Union(GuaClass.Symbols.ToList()).ToList().Union([GuaSubClass.TiJiSymbol]).ToList();
        /// <summary>
        /// 判断值是否为符合或包含符合
        /// </summary>
        /// <param name="sV">要查的值</param>
        /// <param name="IsIndexOf">是否查询包含</param>
        /// <returns></returns>
        public static bool IsSymbol(this string sV, bool IsIndexOf = false)
        {
            bool b = AllSymbols.Contains(sV);
            if (b) return true;
            if (IsIndexOf)
            {
                b = AllSymbols.Any(x => sV.IndexOf(x) > -1);
            }
            return b;
        }


        /// <summary>
        /// 获得当前日期年份在立春前或后，如果是立春前，则用前一年计算命卦
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static FactYearEnum GetFactYear(DateTime d)
        {
            int iy = d.Year;
            // 获取当前公历年份的“立春”节气精确交节时刻（包含具体时分秒）
            SolarTerm term = SolarTerm.FromName(d.Year, "立春");

            FactYearEnum IsFact = d >= term.GetSolarDay().ToDateTime() ? FactYearEnum.SpringAfter : FactYearEnum.SpringBefore;//先判断是否为当年的立春后，如果是立春前，则用前一年计算命卦
            return IsFact;
        }


        /// <summary>
        /// 根据四柱获得时间(反推)
        /// </summary>
        /// <param name="st">四柱数据</param>
        /// <param name="iEndYear">从推年份起（逆向），默认为0时，则会默认当前年份</param>
        /// <returns></returns>

        public static DateTime FindTimeByFSLT(string[] st, int iEndYear = 0)
        {

            DateTime dt = DateTime.MinValue;

            if (iEndYear == 0) iEndYear = DateTime.Now.Year;

            if (st.Length == 0) return DateTime.MinValue;

            if (st.Length == 2) st = ["", st[0], st[1], ""]; //只有月柱和日柱时，补充空年柱和空时柱

            SixtyCycleYear cy = null;
            SixtyCycleMonth cm = null;
            SixtyCycleDay cd = null;
            SixtyCycleHour ch = null;
            if (st.Length == 4) //四柱完整或月柱和日柱
            {
                while (iEndYear > 1900)
                {
                    cy = new SixtyCycleYear(iEndYear);//年柱
                    if (cy.SixtyCycle.GetName() == st[0] || string.IsNullOrWhiteSpace(st[0]))
                    {
                        foreach (SixtyCycleMonth m in cy.Months)
                        {
                            if (m.GetName() == st[1])
                            {
                                cm = m;
                                break;
                            }
                        }
                        if (cm != null)//月柱找到则找日柱
                        {
                            foreach (SixtyCycleDay d in cm.Days)
                            {
                                if (d.GetName() == st[2])
                                {
                                    cd = d;
                                    break;
                                }
                            }
                        }
                        if (cd != null)
                        {
                            foreach (var h in cd.Hours)
                            {
                                if (h.GetName() == st[3] || string.IsNullOrWhiteSpace(st[3]))
                                {
                                    ch = h;
                                    return h.SolarTime.ToDateTime();

                                }
                            }
                        }

                    }

                    iEndYear--;//如果月柱不匹配，则继续往前推年份
                }

            }


            return dt;
        }



        /// <summary>
        /// 五行颜色扩展
        /// </summary>
        /// <param name="d"></param>
        /// <param name="IsDrak"></param>
        /// <returns></returns>
        public static Color ToColor(this FiveAttr d, bool IsDrak = false)
        {
            if (d.Name.IndexOf("金") > -1)
            {
                return IsDrak ? Color.Gold : Color.Goldenrod;
            }
            else if (d.Name.IndexOf("水") > -1)
            {
                return IsDrak ? Color.LightGray : Color.Black;
            }

            else if (d.Name.IndexOf("木") > -1)
            {
                return Color.Green;
            }
            else if (d.Name.IndexOf("土") > -1)
            {
                return IsDrak ? Color.Yellow : Color.DarkGoldenrod;
            }
            else if (d.Name.IndexOf("火") > -1)
            {
                return IsDrak ? Color.Red : Color.Purple;
            }


            return Color.Black;
        }


        /// <summary>
        /// 九星颜色
        /// </summary>
        /// <param name="d"></param>
        /// <param name="IsDrak"></param>
        /// <returns></returns>
        public static Color ToColor(this GuaSubClass d, bool IsDrak = false)
        {
            if (d.Color.IndexOf("白") > -1)
            {
                return IsDrak ? Color.White : Color.DimGray;
            }
            else if (d.Color.IndexOf("黑") > -1)
            {
                return Color.Black;
            }
            else if (d.Color.IndexOf("碧") > -1)
            {
                return IsDrak ? Color.FromArgb(255, 27, 209, 165) : Color.LightSeaGreen;
            }
            else if (d.Color.IndexOf("绿") > -1)
            {
                return Color.Green;
            }
            else if (d.Color.IndexOf("黄") > -1)
            {
                return IsDrak ? Color.Yellow : Color.Goldenrod;
            }
            else if (d.Color.IndexOf("赤") > -1)
            {
                return Color.Red;
            }
            else if (d.Color.IndexOf("紫") > -1)
            {
                return Color.Purple;
            }

            return Color.Black;
        }



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
            if (array == null) return -1; // <--- 加上这一句，瞬间免疫 NullReferenceExceptio
            return Array.IndexOf(array, value);
        }


    }


}

