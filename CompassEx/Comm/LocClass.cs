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

using System;
using System.Collections.Generic;

namespace CompassEx.Comm
{
    /// <summary>
    /// 地支类
    /// </summary>
    public class LocClass
    {
        public static string[] LocNames = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };//地支
        public static string[] LocTimeValues = { "23:00-00:59", "01:00-02:59", "03:00-04:59", "05:00-06:59", "07:00-08:59", "09:00-10:49", "11:00-12:59", "13:00-14:59", "15:00-16:59", "17:00-18:59", "19:00-20:59", "21:00-22:59" };
        public string LocName;//地支名称
        public int Index;//地支位置
        public string FiveAttrName;//五行属性名 
        public string LocTimeValue;//时间值

        /// <summary>
        /// 返回时间是否在这个地支上
        /// </summary>
        /// <param name="d">日期对象，只取小时</param>
        /// <returns></returns>
        public bool TimeValueInRangIndex(DateTime d)
        {
            return TimeValueInRangIndex(d.ToString("HH"));
        }

        /// <summary>
        /// 返回时间是否在这个地支上
        /// </summary>
        /// <param name="HH">小时</param>
        /// <returns></returns>
        public bool TimeValueInRangIndex(string HH)
        {

            string[] ssd = LocTimeValue.Split('-');
            int hh = int.Parse(HH);
            if (Index == 0)
            {
                if (hh >= 23 || hh == 0) return true;//子时
            }
            string[] sd1 = ssd[0].Split(':');
            string[] sd2 = ssd[1].Split(':');

            if (hh >= int.Parse(sd1[0]) && hh <= int.Parse(sd2[0])) return true;

            return false;


        }

        /// <summary>
        /// 获得所有地支类集合
        /// </summary>
        /// <returns></returns>
        public static List<LocClass> GetAllLocClass()
        {
            List<LocClass> al = new List<LocClass>();
            foreach (string sn in LocNames)
            {
                al.Add(GetLocClass(sn));
            }
            return al;
        }


        /**
           *  根据地支名称返回地支类
           *  String LocName　地支名称
           */
        public static LocClass GetLocClass(string LocName)
        {
            int iPos = Array.IndexOf(LocNames, LocName);
            if (iPos == -1) return null;
            LocClass lc = GetLocClass(iPos);

            return lc;
        }


        /**
           *  根据地支的索引返回地支类
           *  @Int iLocIndex //地支索引
           */
        public static LocClass GetLocClass(int iLocIndex)
        {
            LocClass lc = new LocClass();
            if (iLocIndex == 0 || iLocIndex == 11)
            {//子，亥
                lc.FiveAttrName = "水";
            }
            else if (iLocIndex == 2 || iLocIndex == 3)
            {//寅卯
                lc.FiveAttrName = "木";
            }
            else if (iLocIndex == 5 || iLocIndex == 6)
            {//巳午
                lc.FiveAttrName = "火";
            }
            else if (iLocIndex == 8 || iLocIndex == 9)
            {//申酉
                lc.FiveAttrName = "金";
            }
            else
            {//丑辰未戌
                lc.FiveAttrName = "土";
            }
            lc.LocTimeValue = LocTimeValues[iLocIndex];
            lc.Index = iLocIndex;
            lc.LocName = LocNames[iLocIndex];
            return lc;
        }
    }

}
