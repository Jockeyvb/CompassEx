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


using CompassEx.Gua;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CompassEx.Comm
{
    /// <summary>
    /// 表示紫白飞星类，用于管理飞星的名称、索引、数值、九宫方位以及对应的后天八卦属性。
    /// <para>紫白飞星：年、月紫白，入中宫之星逆推，分布九宫位是顺飞。</para>
    /// <para>为了方便使用，年紫白飞星方法：<see cref="YearFlyStar(int)"/> 的年值必须是立春后的年值。</para>
    /// <para>月紫白飞星方法：<see cref="MonthFlyStars(LocClass)"/>（注：实际方法为 <see cref="MonthFlyStars(LocClass)"/>）需要传入年支类获得布满十二月飞星值，但九宫方位为 <see langword="null"/>，因为九宫飞布不存在。若要获得所有十二月的九宫方位应该使用 <see cref="GetFlyStars(int, bool, bool)"/> 方法重新获得，每月飞布宫位如：<c>GetFlyStars(MonthFlyStar.First().StarValue, true, true)</c> 获得正月飞布情况。</para>
    /// <para>日紫白飞星：是按节气冬至后第一个甲子为一白顺飞，乙丑为二黑坤，夏至后第一个甲子为一日逆飞，乙丑为九紫。</para>
    /// <para>时紫白飞星：是按天干阴阳顺逆飞，阳为顺飞，阴为逆飞。</para>
    /// <para><span style="font-weight:bold;color:red;">重点是：每一个紫白中宫为 StarValue（星值），年、月、日、时紫白应该有独立的飞布宫位，则可使用九宫方位应使用 <see cref="GetFlyStars(int, bool, bool)"/> 方法重新获得。例如每月飞布宫位：<c>GetFlyStars(MonthFlyStar.First().StarValue, true, true)</c> 读取属性 <see cref="StarDirection"/> 获得九宫方位信息。</span></para>
    /// </summary>
    public class FlyStar
    {
        /// <summary>
        /// 获取飞星信息全名（格式：区域方位名称：飞星名称）。
        /// </summary>
        public string FullName
        {
            get
            {
                return StarDirection != null ? this.StarDirection?.Name + "：" + this.Name : this.Name;
            }
        }

        /// <summary>
        /// 获取飞星名称（包含后天卦数、颜色及五行或中宫标识）。
        /// </summary>
        public string Name
        {
            get
            {
                string sn = this.FlyStarGuaSub.Name;
                if (sn == "黄") sn = "中";
                return this.FlyStarGuaSub.AfterGuaSubCNQuantity + this.FlyStarGuaSub.Color + sn;
            }
        }

        /// <summary>
        /// 获取飞星的宫位索引（后天宫位，范围通常为 0-8）。
        /// </summary>
        public int AreaIndex { get; private set; }

        /// <summary>
        /// 获取入中数（中宫飞星原始数值）。
        /// </summary>
        public int CenterStarValue { get; private set; }
        /// <summary>
        /// 是否顺飞
        /// </summary>
        public bool IsAscending { get; private set; }

        /// <summary>
        /// 获取当前飞星数值（1-9）。
        /// </summary>
        public int StarValue { get; private set; }

        /// <summary>
        /// 获取飞星对应的后天卦信息。
        /// </summary>
        public GuaSubClass FlyStarGuaSub { get { return GuaSubClass.GetAfterGuaSub(this.StarValue - 1); } }

        /// <summary>
        /// 获取宫位对应的后天卦信息。
        /// </summary>
        public GuaSubClass AreaGuaSub { get { return GuaSubClass.GetAfterGuaSub(this.AreaIndex); } }

        /// <summary>
        /// 获取飞星对应的区域方位类实例。若无方位信息则为 <see langword="null"/>。
        /// </summary>
        public AreaDirection? StarDirection { get; private set; } = default!;

        /// <summary>
        /// 初始化 <see cref="FlyStar"/> 类的新实例，使用指定的后天卦名创建。
        /// </summary>
        /// <param name="afterGuaSubName">后天卦的名称。</param>
        /// <param name="CenterStarValue">飞星入中数，默认为 5。</param>
        /// <param name="IsAscending">是否顺飞（<see langword="true"/> 为顺飞，<see langword="false"/> 为逆飞），默认为 <see langword="true"/>。</param>
        public FlyStar(string afterGuaSubName, int CenterStarValue = 5, bool IsAscending = true) : this(GuaSubClass.AfterGuaSubNames.IndexOf(afterGuaSubName), CenterStarValue, IsAscending)
        {
        }

        /// <summary>
        /// 初始化 <see cref="FlyStar"/> 类的新实例，基于指定的后天卦索引创建。
        /// </summary>
        /// <param name="afterGuaSubIndex">后天卦位（宫位）的索引值 (0-8)。</param>
        /// <param name="CenterStarValue">飞星入中数，默认为 5。</param>
        /// <param name="IsAscending">是否顺飞（<see langword="true"/> 为顺飞，<see langword="false"/> 为逆飞），默认为 <see langword="true"/>。</param>
        /// <exception cref="ArgumentOutOfRangeException">当索引或入中数超出有效范围时抛出。</exception>
        [JsonConstructor]
        public FlyStar([JsonProperty(nameof(AreaIndex))] int afterGuaSubIndex, [JsonProperty(nameof(CenterStarValue))] int CenterStarValue = 5, [JsonProperty(nameof(IsAscending))] bool IsAscending = true)
        {
            if (afterGuaSubIndex < 0 || afterGuaSubIndex >= GuaSubClass.AfterGuaSubNames.Length)
                throw new ArgumentOutOfRangeException(nameof(afterGuaSubIndex));
            if (CenterStarValue < 1 || CenterStarValue > 9)
                throw new ArgumentOutOfRangeException(nameof(CenterStarValue));

            this.CenterStarValue = CenterStarValue;
            this.AreaIndex = afterGuaSubIndex;
            this.IsAscending = IsAscending;
            int baseValue = this.AreaIndex + 1; // 0=坎->1, 1=坤->2 ...
            int ipos;

            if (IsAscending)
            {
                // 顺飞公式
                ipos = baseValue + (CenterStarValue - 5);
                // 确保结果严格落在 1-9 的九宫范围内
                ipos = (ipos - 1) % 9 + 1;
                if (ipos < 1) ipos += 9;
            }
            else
            {
                // 逆飞正确公式：以 5 为基准反转位移
                ipos = (5 - baseValue) + CenterStarValue;
                ipos = (ipos - 1 + 90) % 9 + 1;
            }

            this.StarValue = ipos; // 当前宫位的飞星值 

            StarDirection = AreaDirection.GetAreaDirectionByFlyStar(this.AreaIndex + 1);
        }

        /// <summary>
        /// 获取所有后天卦位对应的飞星信息集合。
        /// </summary>
        /// <param name="CenterStarValue">入中飞星值，默认为 5。</param>
        /// <param name="IsAscending">是否顺飞，默认为 <see langword="true"/>。</param>
        /// <param name="SortStar5First">是否从五黄排序，默认为 <see langword="true"/>。</param>
        /// <returns>返回包含所有九宫方位飞星实例的列表（<see cref="List{FlyStar}"/>）。</returns>
        public static List<FlyStar> GetFlyStars(int CenterStarValue = 5, bool IsAscending = true, bool SortStar5First = true)
        {
            List<FlyStar> ls = new List<FlyStar>();
            for (int i = 0; i < GuaSubClass.AfterGuaSubNames.Length; i++)
            {
                FlyStar fs = new FlyStar(i, CenterStarValue, IsAscending);
                ls.Add(fs);
            }
            if (SortStar5First)
            {
                List<FlyStar> nls = new List<FlyStar>();
                int[] sI = [4, 5, 6, 7, 8, 0, 1, 2, 3]; // 按宫位排
                for (int i = 0; i < sI.Length; i++)
                {
                    nls.Add(ls.Where(x => x.AreaIndex == sI[i]).First());
                }
                ls = nls;
            }
            return ls;
        }

        /// <summary>
        /// 获得年紫白飞星信息（中宫信息）。
        /// </summary>
        /// <param name="Year">年值（必须是立春后）。</param>
        /// <returns>返回对应年份的入中飞星实例（<see cref="FlyStar"/>）。</returns>
        public static FlyStar YearFlyStar(int Year)
        {
            (int Year, int CenterStarValue) BaseValue = (2026, 1);
            int iy = 0;
            if (Year == BaseValue.Year)
            {
                iy = BaseValue.CenterStarValue;
            }
            else
            {
                iy = (Year - BaseValue.Year);
                if (iy >= 0) iy -= BaseValue.CenterStarValue;
                if (iy < 0) iy = 9 - (BaseValue.CenterStarValue - iy) % 9;
                Debug.Print((iy % 9).ToString());
                iy = 9 - (iy % 9);
            }

            return new FlyStar(4, iy, true); // 固定取五黄宫值，顺飞
        }

        /// <summary>
        /// 获得十二个月的月紫白飞星信息（中宫信息），返回字典中 Key 为地支类（如寅为正月，卯为二月）。
        /// <para>注：十二月紫白本身不存在方位信息，为防止误解已将 <see cref="StarDirection"/> 设置为 <see langword="null"/>。若需详细获得每月中的方位信息，请使用入参调用 <see cref="GetFlyStars(int, bool, bool)"/> 获取月中飞布情况。</para>
        /// </summary>
        /// <param name="YearLoc">年支实例（<see cref="LocClass"/>）。</param>
        /// <returns>返回地支与月飞星中宫信息的字典。</returns>
        public static Dictionary<LocClass, FlyStar> MonthFlyStars(LocClass YearLoc)
        {
            string sc = YearLoc.Name;
            List<FlyStar> ls = new List<FlyStar>();
            if ("子午卯酉".IndexOf(sc) > -1) // 8 入中
            {
                ls = FlyStar.GetFlyStars(8, false);
            }
            else if ("辰戌丑未".IndexOf(sc) > -1) // 5 入中
            {
                ls = FlyStar.GetFlyStars(5, false);
            }
            else // 2 入中
            {
                ls = FlyStar.GetFlyStars(2, false);
            }

            Dictionary<LocClass, FlyStar> dc = new();
            for (int i = 0; i < ls.Count; i++) // 寅为正月 -> 戌月
            {
                ls[i].StarDirection = null; // 因为十二月紫白不存在方位信息，为不误解设置 null
                dc.Add(new LocClass(LocClass.LocNames[i + 2]), ls[i]);
            }
            dc.Add(new LocClass(LocClass.LocNames[11]), ls[0]); // 亥
            dc.Add(new LocClass(LocClass.LocNames[0]), ls[1]);  // 子
            dc.Add(new LocClass(LocClass.LocNames[1]), ls[2]);  // 丑

            return dc;
        }

        /// <summary>
        /// 获得十二个月的月紫白飞星信息（中宫信息），返回字典中 Key 为干支类（<see cref="SkyLoc"/>）。
        /// <para>注：十二月紫白本身不存在方位信息，若需详细获得每月中的方位信息，请使用入参调用 <see cref="GetFlyStars(int, bool, bool)"/> 获取月中飞布情况。</para>
        /// </summary>
        /// <param name="YearSL">年干支实例（<see cref="SkyLoc"/>）。</param>
        /// <returns>返回干支和月飞星信息的字典。</returns>
        public static Dictionary<SkyLoc, FlyStar> MonthFlyStars(SkyLoc YearSL)
        {
            var ldc = MonthFlyStars(YearSL.Loc);
            Dictionary<SkyLoc, FlyStar> lsdc = new();
            foreach (var kvp in ldc)
            {
                lsdc.Add(SkyLoc.FiveTiger(YearSL.Sky, kvp.Key), kvp.Value);
            }

            return lsdc;
        }

        /// <summary>
        /// 获取指定日期的日紫白飞星信息（不包括当天的九宫方位信息）。
        /// <para>注：日紫白本身暂无默认方位信息，若需详细获得每日中的方位信息，请使用属性 <see cref="StarValue"/> 作为入参调用 <see cref="GetFlyStars(int, bool, bool)"/> 获得飞布情况。</para>
        /// </summary>
        /// <param name="dt">公历日期。</param>
        /// <returns>返回包含日干支与飞星信息的元组。</returns>
        public static (SkyLoc DaySL, FlyStar FlyStar) DayFlyStar(DateTime dt)
        {
            if (dt == DateTime.MinValue) return default;

            var sd = dt.ToSolarDay();
            var sld = sd.GetSixtyCycleDay();
            var ns = sld.NineStar;
            int AfterGuaSubIndex = ns.Index; // 后天八卦位置（飞星位置）
            var fs = new FlyStar(4, AfterGuaSubIndex + 1); // 固定为卦位入中宫位，读取5五黄位置
            fs.StarDirection = null; // 因为紫白暂不带方位信息，设为 null 避免误解
            return (new SkyLoc(sld.GetName()), fs);
        }

        /// <summary>
        /// 根据公历年月获取当月所有日期的日紫白飞星信息集合（不包括当天的九宫方位信息）。
        /// <para>注：日紫白本身暂无默认方位信息，若需详细获得每日中的方位信息，请使用属性 <see cref="StarValue"/> 作为入参调用 <see cref="GetFlyStars(int, bool, bool)"/> 获得飞布情况。</para>
        /// </summary>
        /// <param name="dt">公历日期（指定年月）。</param>       
        /// <returns>返回日期干支与飞星信息的字典，若日期无效则返回 <see langword="null"/>。</returns>
        public static Dictionary<SkyLoc, FlyStar>? DayFlyStars(DateTime dt)
        {
            if (dt == DateTime.MinValue) return default;
            Dictionary<SkyLoc, FlyStar> dc = new();
            int MaxDays = DateTime.DaysInMonth(dt.Year, dt.Month);
            for (int i = 1; i <= MaxDays; i++)
            {
                var fs = DayFlyStar(new DateTime(dt.Year, dt.Month, i));
                dc[fs.DaySL] = fs.FlyStar;
            }

            return dc;
        }



        /// <summary>
        /// 根据月支判断是否自动获得时飞星的顺逆（非午月子月则是强制自动识别阳顺阴逆为ture，否则为false）
        /// </summary>
        /// <param name="MonthLoc">月支</param>
        /// <returns></returns>
        public static bool HuorFlyStarIsAutoSort(LocClass MonthLoc)
        {
            return "午子".IndexOf(MonthLoc.Name) == -1; //非午月子月则是强制自动识别阳顺阴逆，否则按手动传入(out  出参)
        }

        /// <summary>
        /// 根据月令（阳顺阴逆）获得时紫白飞星信息（不包含当前时辰的九宫方位信息）。
        /// <para>注：时紫白本身暂无默认方位信息，若需详细获得每日中的方位信息，请使用属性 <see cref="FlyStar.StarValue"/> 作为入参调用 <see cref="FlyStar.GetFlyStars(int, bool, bool)"/> 获得飞布情况。</para>
        /// </summary>
        /// <param name="MonthLoc">月支。用于辅助判断阴阳顺逆，只有午月（通常夏至前后）和亥月（通常冬至前后）由于跨节气需要手动指定顺逆。</param>
        /// <param name="DayLoc">日支。用于根据顺逆规则推出子时入中宫之数。</param>
        /// <param name="HourLoc">要获取的目标时支（如子、丑、寅等）。</param>
        /// <param name="IsAsc">顺逆标志（<see langword="true"/> 为顺飞，<see langword="false"/> 为逆飞）。在无法自动识别的特殊月份时作为手动传入的依据。</param>
        /// <param name="IsAutoSort">输出参数（<see langword="out"/>）。指示是否为非午月、子月（代码中判断逻辑对应“午子”），若是则强制自动识别阳顺阴逆；否则使用传入的 <paramref name="IsAsc"/>。</param>
        /// <returns>返回对应 <paramref name="HourLoc"/> 时辰的时紫白飞星实例（<see cref="FlyStar"/>），若无效则可能返回 <see langword="null"/>。</returns>
        public static FlyStar? HourFlyStar(LocClass MonthLoc, LocClass DayLoc, LocClass HourLoc, bool IsAsc, out bool IsAutoSort)
        {
            //1、冬至以后到夏至以前（阳遁）顺飞，夏至后冬至前（阴遁）逆飞

            //2、日支
            //冬至后：  子午卯酉日：子时起 一白（1）  辰戌丑未日：子时起 四绿（4）  寅申巳亥日：子时起 七赤（7）  
            //夏至后：  子午卯酉日：子时起 九紫（9）  辰戌丑未日：子时起 六白（6）  寅申巳亥日：子时起 三碧（3）

            //======月支辅助判断顺逆，只有午月（夏至）和亥月（冬至）需要手动指定顺逆,因午月亥月无法确定节气=========
            IsAutoSort = "午子".IndexOf(MonthLoc.Name) == -1; //非午月子月则是强制自动识别阳顺阴逆，否则按手动传入(out  出参)
            if (MonthLoc.Index > 0 && MonthLoc.Index < 6)//冬至后 丑月至巳月(5个月） 为阳遁顺
            {
                IsAsc = true;
            }
            else if (MonthLoc.Index > 6 && MonthLoc.Index <= 11)//夏至后 未月至亥月 为阴遁逆
            {
                IsAsc = false;

            }//剩下的是午月或亥月按手动传入顺序
             //======月支辅助判断顺逆，只有午月（夏至）和亥月（冬至）需要手动指定顺逆,因午月亥月无法确定节气=========

            //============日支按顺逆推出入中宫之数=================================
            string dl = DayLoc.Name;
            int HourStarValue = 0;
            if ("子午卯酉".IndexOf(dl) > -1) // 阳遁顺1, 阴遁逆9
            {
                HourStarValue = IsAsc ? 1 : 9;
            }
            else if ("辰戌丑未".IndexOf(dl) > -1) // 阳遁顺4, 阴遁逆6
            {
                HourStarValue = IsAsc ? 4 : 6;
            }
            else  // 阳遁顺4, 阴遁逆6
            {
                HourStarValue = IsAsc ? 7 : 3;
            }
            //============日支按顺逆推出入中宫之数=================================

            int hlIndex = LocClass.LocNames.IndexOf(HourLoc.Name);//取出读取时支位置

            hlIndex = hlIndex % 9; //共12个地支，超出的用余数处理
            hlIndex += 4; //五黄为中宫(子为0）
            hlIndex = hlIndex % 9;
            var fs = new FlyStar(hlIndex, HourStarValue, IsAsc);//按时星去取相关宫位
            fs.StarDirection = null;// 因为紫白不含独立方位，设为 null 避免误解


            return fs;

        }

        /// <summary>
        /// 根据月令（阳顺阴逆），获得当天所有的时紫白飞星信息（不包含当前时辰的九宫方位信息）。
        /// <para>注：时紫白本身暂无默认方位信息，若需详细获得每日中的方位信息，请使用属性 <see cref="FlyStar.StarValue"/> 作为入参调用 <see cref="FlyStar.GetFlyStars(int, bool, bool)"/> 获得飞布情况。</para>
        /// </summary>
        /// <param name="MonthLoc">月支。用于辅助判断阴阳顺逆，只有午月（通常夏至前后）和亥月（通常冬至前后）由于跨节气需要手动指定顺逆。</param>
        /// <param name="DayLoc">日支。用于根据顺逆规则推出子时入中宫之数。</param>
        /// <param name="IsAsc">顺逆标志（<see langword="true"/> 为顺飞，<see langword="false"/> 为逆飞）。在无法自动识别的特殊月份时作为手动传入的依据。</param>
        /// <param name="IsAutoSort">输出参数（<see langword="out"/>）。指示是否自动识别了顺逆关系。</param>
        /// <returns>返回包含全天十二个时辰地支与对应时紫白飞星信息的字典（<see cref="Dictionary{LocClass, FlyStar}"/>）。</returns>
        public static Dictionary<LocClass, FlyStar> HourFlyStars(LocClass MonthLoc, LocClass DayLoc, bool IsAsc, out bool IsAutoSort)
        {

            Dictionary<LocClass, FlyStar> dc = new();
            bool b = false;
            for (int i = 0; i < LocClass.LocNames.Length; i++)
            {
                var hlc = new LocClass(i);

                var fs = HourFlyStar(MonthLoc, DayLoc, hlc, IsAsc, out b);
                dc[hlc] = fs;

            }
            IsAutoSort = b;
            return dc;
        }

        /// <summary>
        /// 获取指定时间的时紫白飞星信息（不包含当前时辰的九宫方位信息）。
        /// <para>注：时紫白本身暂无默认方位信息，若需详细获得每日中的方位信息，请使用属性 <see cref="StarValue"/> 作为入参调用 <see cref="GetFlyStars(int, bool, bool)"/> 获得飞布情况。</para>
        /// </summary>
        /// <param name="dt">要获得的公历时间（<see cref="DateTime"/>）。</param>
        /// <returns>返回包含时辰干支与带方位飞星信息的元组。</returns>
        public static (SkyLoc HourSL, FlyStar FlyStar) HourFlyStar(DateTime dt)
        {
            if (dt == DateTime.MinValue) return default;

            var sd = dt.ToSolarTime();
            var sld = sd.GetSixtyCycleHour();
            var ns = sld.NineStar;
            string nn = ns.GetName();
            int AfterGuaSubIndex = GuaSubClass.AfterGuaSubNumerics.IndexOf(nn); // 后天八卦位置（飞星位置）
            var fs = new FlyStar(4, AfterGuaSubIndex + 1); // 固定为卦位入中宫位，读取5五黄位置
            fs.StarDirection = null; // 因为紫白不含独立方位，设为 null 避免误解
            return (new SkyLoc(sld.GetName()), fs);
        }

        /// <summary>
        /// 获取当天所有时辰的时紫白飞星信息（不包含对应时辰的九宫方位信息）。 
        /// <para>注：时紫白本身暂无默认方位信息，若需详细获得每日中的方位信息，请使用属性 <see cref="StarValue"/> 作为入参调用 <see cref="GetFlyStars(int, bool, bool)"/> 获得飞布情况。</para>
        /// </summary>
        /// <param name="dt">当天的公历日期（<see cref="DateTime"/>）。</param>

        /// <returns>返回当日所有时辰干支与飞星信息的字典，若日期无效则返回 <see langword="null"/>。</returns>
        public static Dictionary<SkyLoc, FlyStar>? HourFlyStars(DateTime dt)
        {
            if (dt == DateTime.MinValue) return default;
            Dictionary<SkyLoc, FlyStar> dc = new();
            for (int i = 0; i < 24; i += 2)
            {
                DateTime tmpdt = new DateTime(dt.Year, dt.Month, dt.Day, i, 30, 00); // 日默认为 30分钟
                var lf = HourFlyStar(tmpdt);

                dc.Add(lf.HourSL, lf.FlyStar);
            }

            return dc;
        }

        /// <summary>
        /// 返回当前飞星的默认全名字符串表示。
        /// </summary>
        /// <returns>返回 <see cref="FullName"/> 属性的值。</returns>
        public override string ToString()
        {
            return this.FullName;
        }
    }




}
