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
        /// <summary>
        /// 地支名称数组
        /// </summary>
        public readonly static string[] LocNames = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };//地支
        /// <summary>
        /// 地支对应的时间值
        /// </summary>
        public readonly static string[] LocTimeValues = { "23:00-00:59", "01:00-02:59", "03:00-04:59", "05:00-06:59", "07:00-08:59", "09:00-10:49", "11:00-12:59", "13:00-14:59", "15:00-16:59", "17:00-18:59", "19:00-20:59", "21:00-22:59" };

        /// <summary>
        /// 地支名称
        /// </summary>
        public string LocName { get; private set; }//地支名称
        /// <summary>
        /// 地支索引
        /// </summary>
        public int Index { get; private set; }//地支位置

        /// <summary>
        /// 五行属
        /// </summary>
        public FiveAttr FiveAttr { get; private set; }//五行属性名 

        /// <summary>
        /// 地支的时间值
        /// </summary>
        public string LocTimeValue { get; private set; }//时间值






        /// <summary>
        /// 地支构造函数
        /// </summary>
        /// <param name="LocName">地支名称</param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public LocClass(string LocName) : this(Array.IndexOf(LocNames, LocName))
        {

        }

        /// <summary>
        /// 地支构造函数
        /// </summary>
        /// <param name="LocIndex">地支所在的索引，参考：【<see cref="LocNames"/>】</param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public LocClass(int LocIndex)
        {
            if (LocIndex < 0 || LocIndex > LocNames.Length) throw new IndexOutOfRangeException();
            if (LocIndex == 0 || LocIndex == 11)//子亥
            {
                this.FiveAttr = new FiveAttr("水");
            }
            else if (LocIndex == 2 || LocIndex == 3)
            {
                this.FiveAttr = new FiveAttr("木");
            }
            else if (LocIndex == 5 || LocIndex == 6)
            {
                this.FiveAttr = new FiveAttr("火");
            }
            else if (LocIndex == 8 || LocIndex == 9)
            {
                this.FiveAttr = new FiveAttr("金");
            }
            else
            {
                this.FiveAttr = new FiveAttr("土");
            }
            this.LocTimeValue = LocTimeValues[LocIndex];
            this.Index = LocIndex;
            this.LocName = LocNames[LocIndex];
        }

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
            LocClass lc = new LocClass(iLocIndex);

            lc.LocTimeValue = LocTimeValues[iLocIndex];
            lc.Index = iLocIndex;
            lc.LocName = LocNames[iLocIndex];
            return lc;
        }
    }

}
