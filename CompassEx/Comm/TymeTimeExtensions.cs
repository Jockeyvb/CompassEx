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


using CompassEx.Assist;
using CompassEx.Gua;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using tyme.culture.nine;
using tyme.culture.star.nine;
using tyme.culture.star.twentyeight;
using tyme.lunar;
using tyme.sixtycycle;
using tyme.solar;

namespace CompassEx.Comm
{

    /// <summary>
    /// 针对元贞历法核心库（Tyme）时间与日期对象的 C# 官方标准 <see cref="DateTime"/> 转换扩展工具类。
    /// </summary>
    /// <remarks>
    /// 本静态类通过提供高效的链式扩展方法（Extension Methods），实现了将三方历法库中的公历时间（<see cref="SolarTime"/>）与公历日期（<see cref="SolarDay"/>）快速转换为 .NET 原生的时间结构体，常用于前后端排盘结果的数据对接与串联。
    /// </remarks>
    public static class TymeTimeExtensions
    {





        /// <summary>
        /// 扩展节气交节时间
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static SolarTime GetTermTime(this SolarTerm d)
        {
            return d.JulianDay.GetSolarTime();
        }

        /// <summary>
        /// 输出全称，包括：宿 + 七曜 + 动物
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToFullString(this TwentyEightStar d)
        {
            return d.GetName() + d.SevenStar + d.GetAnimal();
        }


        /// <summary>
        /// 二十八宿4种颜色 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="IsDrak"></param>
        /// <returns></returns>
        public static Color ToFourColor(this TwentyEightStar d, bool IsDrak = false)
        {
            int index = TwentyEightStar.Names.IndexOf(d.GetName());
            if (index / 7 == 0)
            {
                return Color.Green;
            }
            else if (index / 7 == 1)
            {
                return Color.Black;
            }
            else if (index / 7 == 2)
            {
                return IsDrak ? Color.White : Color.DimGray;
            }
            else if (index / 7 == 3)
            {
                return Color.Red;
            }
            return Color.Black;
        }



        /// <summary>
        /// 九星颜色
        /// </summary>
        /// <param name="d"></param>
        /// <param name="IsDrak"></param>
        /// <returns></returns>
        public static Color ToColor(this tyme.culture.star.nine.NineStar d, bool IsDrak = false)
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
                return Color.FromArgb(255, 141, 62, 137);
            }

            return Color.Black;
        }

        /// <summary>
        /// 获得农历全部名称
        /// </summary>
        /// <param name="d"></param>
        /// <param name="SplitString"></param>
        /// <returns></returns>
        public static string ToFullCNName(this DateTime d, string SplitString = "-")
        {
            var vp = d.ToLunar();

            string st = vp.ly.GetName() + SplitString + vp.lm.GetName() + SplitString + vp.ld.GetName() + SplitString + vp.lh.GetName();

            return st;
        }

        /// <summary>
        /// 农历时输出完整的四柱
        /// </summary>
        /// <param name="d"></param>
        /// <param name="SplitString"></param>
        /// <returns></returns>
        public static string TofourSkyLocString(this LunarHour d, string SplitString = " ")
        {
            var h = d.GetSixtyCycleHour();
            return h.Year.GetName() + SplitString + h.Month.GetName() + SplitString + h.Day.GetName() + SplitString + h.GetName();
        }

        /// <summary>
        /// 【擴充方法】將 DateTime 一鍵轉換為包含农历时
        /// </summary>
        // 💡 優化：直接定義具名元組傳回型別，不再寫 ValueTuple 關鍵字
        public static LunarHour ToLunarHour(this DateTime d)
        {
            // 1. 降維拆解原生時間
            SolarTime sd = d.ToSolarTime();

            // 2. 獲取時、日、月、年各層級歷法對象
            var lh = sd.GetLunarHour();


            // 3. 【核心修正】直接回傳字面量元組，變數順序與型別必須與頭部宣告完全一致
            return lh;
        }


        /// <summary>
        /// 【擴充方法】將 DateTime 一鍵轉換為包含年、月、日、時完整干支歷法對象的具名元組
        /// </summary>
        // 💡 優化：直接定義具名元組傳回型別，不再寫 ValueTuple 關鍵字
        public static (LunarYear ly, LunarMonth lm, LunarDay ld, LunarHour lh) ToLunar(this DateTime d)
        {
            // 1. 降維拆解原生時間
            SolarTime sd = d.ToSolarTime();

            // 2. 獲取時、日、月、年各層級歷法對象
            var lh = sd.GetLunarHour();
            var ld = d.ToLunarDay();
            var lm = new LunarMonth(ld.Year, ld.Month);

            // 3. 【核心修正】直接回傳字面量元組，變數順序與型別必須與頭部宣告完全一致
            return (lm.LunarYear, lm, ld, lh);
        }


        /// <summary>
        /// 扩展DateTime 转为农历日
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static LunarDay ToLunarDay(this DateTime d)
        {
            // 降维拆解 .NET 原生时间组件，通过构造函数直接实例化 Tyme 阳历时间对象
            SolarDay sd = d.ToSolarDay();
            var ld = sd.GetLunarDay();
            return ld;
        }



        /// <summary>
        /// 扩展DateTime 转为阳历日
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static SolarDay ToSolarDay(this DateTime d)
        {
            // 降维拆解 .NET 原生时间组件，通过构造函数直接实例化 Tyme 阳历时间对象
            return new SolarDay(d.Year, d.Month, d.Day);
        }
        /// <summary>
        /// 【扩展方法】将 .NET 原生的 <see cref="DateTime"/> 快速转化为 Tyme 历法库专用的 <see cref="SolarTime"/> 高精度阳历时间对象。
        /// </summary>
        /// <param name="d">需要进行类型转换的 .NET 标准系统日期时间实例（通常为北京时间或现场真太阳时）。</param>
        /// <returns>
        /// 返回一个高精度的 <see cref="SolarTime"/> 实体，内部完美继承传入时间的年、月、日、时、分、秒等时空刻度数值。
        /// </returns>
        /// <remarks>
        /// 本方法作为系统高频使用的核心桥接管道，常用于将电脑当前系统时间（如 <see cref="DateTime.Now"/>）或从数据库读取的勘测时间，
        /// 无缝转换为历法库模型，以便后续进行高精度的立春换年柱、节气换月柱等核心易学理气数理推演。
        /// </remarks>
        public static SolarTime ToSolarTime(this DateTime d)
        {
            // 降维拆解 .NET 原生时间组件，通过构造函数直接实例化 Tyme 阳历时间对象
            return new SolarTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second);
        }


        /// <summary>
        /// 将 Tyme 历法库的公历时间对象转换为 .NET 标准的 <see cref="DateTime"/> 实例。
        /// </summary>
        /// <param name="st">被扩展的当前 <see cref="SolarTime"/> 公历时间对象实例。</param>
        /// <returns>返回一个包含完整年、月、日、时、分、秒信息的 <see cref="DateTime"/> 结构体对象。</returns>
        /// <remarks>
        /// <b>转换细节：</b>该方法通过显式提取输入对象的具体时分秒数值进行直接映射，生成的 <see cref="DateTime"/> 实例默认其 <see cref="DateTime.Kind"/> 属性为 <see cref="DateTimeKind.Unspecified"/>。
        /// </remarks> // 👈 這裡之前漏掉了閉合標籤，現已補上
        public static DateTime ToDateTime(this SolarTime st)
        {
            return new DateTime(st.Year, st.Month, st.Day, st.Hour, st.Minute, st.Second);
        }

        /// <summary>
        /// 将 Tyme 历法库的公历日期对象转换为 .NET 标准的 <see cref="DateTime"/> 实例（零点时刻）。
        /// </summary>
        /// <param name="sd">被扩展的当前 <see cref="SolarDay"/> 公历日期对象实例。</param>
        /// <returns>返回一个代表该日期当天凌晨零点整（<c>00:00:00</c>）的 <see cref="DateTime"/> 结构体对象。</returns>
        public static DateTime ToDateTime(this SolarDay sd)
        {
            return new DateTime(sd.Year, sd.Month, sd.Day);
        }
    }


}
