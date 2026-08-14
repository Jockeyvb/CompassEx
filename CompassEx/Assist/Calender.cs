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
using System;
using System.Collections.Generic;
using System.Text;

namespace CompassEx.Assist
{
    public static class Calender
    {
        /// <summary>
        /// 天赦日
        /// </summary>
        public static readonly string[] SkyPardonDays = { "戊寅", "甲午", "戊申", "甲子" };

        /// <summary>
        /// 四废日
        /// </summary>
        public static readonly string[] FourScrapDays = { "庚申,辛酉", "壬子,癸亥", "甲寅,乙卯", "丙午,丁巳" };

        /// <summary>
        /// 十恶大败日
        /// </summary>
        /// 庚戌年见甲辰日，辛亥年见乙巳日，壬寅年见丙申日，癸巳年见丁亥日，甲戌年见庚辰日，甲辰年见戊戌日，乙亥年见辛巳日，乙未年见己丑日，丙寅年见壬申日，丁巳年见癸亥日。
        public static readonly string[] TenDefeatDays = { "庚戌=甲辰", "辛亥=乙巳", "壬寅=丙申", "癸巳=丁亥", "甲戌=庚辰", "甲辰=戊戌", "乙亥=辛巳", "乙未=己丑", "丙寅=壬申", "丁巳=癸亥" };
        /// <summary>
        /// 十灵日
        /// </summary>
        public static readonly string[] TenSpiritDays = { "甲辰", "乙亥", "丙辰", "丁酉", "戊午", "庚戌", "庚寅", "辛亥", "壬寅", "癸未" };


        /// <summary>
        /// 十灵日
        /// </summary>
        /// <param name="FSLT"></param>
        /// <returns></returns>
        public static bool IsTenSpiritDay(FourSkyLocType FSLT)
        {


            return TenSpiritDays.IndexOf(FSLT.DaySLName) > -1;

        }

        /// <summary>
        /// 是否为十恶大败日
        /// </summary>
        /// <returns></returns>
        public static bool IsTenDefeatDay(DateTime d)
        {
            var FSLT = d.ToFourSkyLocType();
            return IsTenDefeatDay(FSLT);
        }



        /// <summary>
        /// 是否为十恶大败日
        /// </summary>
        /// <param name="FSLT"></param>
        /// <returns></returns>
        public static bool IsTenDefeatDay(FourSkyLocType FSLT)
        {
            foreach (string s in TenDefeatDays)
            {
                string[] sd = s.Split('=');
                if (FSLT.YearSLName.Equals(sd[0]))//年
                {
                    return FSLT.DaySLName.Equals(sd[1]);
                }
            }

            return false;

        }



        /// <summary>
        /// 是否为四废日
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool IsFourScrapDay(DateTime d)
        {
            var FSLT = d.ToFourSkyLocType();
            return IsFourScrapDay(FSLT);
        }


        /// <summary>
        /// 是否为四废日
        /// </summary>
        /// <param name="FSLT"></param>
        /// <returns></returns>
        public static bool IsFourScrapDay(FourSkyLocType FSLT)
        {
            int iPos = LocClass.LocNames.IndexOf(FSLT.MonthSLName.Substring(1));
            if (iPos >= 2 && iPos <= 4)//春
            {
                return FourScrapDays[0].IndexOf(FSLT.DaySLName) > -1;
            }
            else if (iPos >= 5 && iPos <= 7)//夏
            {
                return FourScrapDays[1].IndexOf(FSLT.DaySLName) > -1; ;
            }
            else if (iPos >= 8 && iPos <= 10)//秋
            {
                return FourScrapDays[2].IndexOf(FSLT.DaySLName) > -1; ;
            }
            else//冬
            {
                return FourScrapDays[3].IndexOf(FSLT.DaySLName) > -1; ;
            }

        }


        /// <summary>
        /// 是否为天赦日
        /// </summary>
        /// <returns></returns>
        public static bool IsSkyPardonDay(FourSkyLocType FSLT)
        {

            int iPos = LocClass.LocNames.IndexOf(FSLT.MonthSLName.Substring(1));
            if (iPos >= 2 && iPos <= 4)//春
            {
                return FSLT.DaySLName == SkyPardonDays[0];
            }
            else if (iPos >= 5 && iPos <= 7)//夏
            {
                return FSLT.DaySLName == SkyPardonDays[1];
            }
            else if (iPos >= 8 && iPos <= 10)//秋
            {
                return FSLT.DaySLName == SkyPardonDays[2];
            }
            else//冬
            {
                return FSLT.DaySLName == SkyPardonDays[3];
            }

        }


        /// <summary>
        /// 是否为天赦日
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool IsSkyPardonDay(DateTime d)
        {
            var FSLT = d.ToFourSkyLocType();
            return IsSkyPardonDay(FSLT);

        }
    }
}
