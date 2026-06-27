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

namespace CompassEx.Comm
{
    public static class Defined
    {

        /// <summary>
        /// 太极符号
        /// </summary>
        public static readonly string TaiChiSymbol = "\u262F";	//太极图

        /// <summary>
        /// 是否为真实年(龄),0为立春后，1立春前
        /// </summary>
        public enum FactYearEnum : uint
        {

            /// <summary>
            /// 立春后
            /// </summary>
            SpringAfter = 0,
            /// <summary>
            /// 立春前
            /// </summary>
            SpringBefore = 1,

        }
        /// <summary>
        /// 天干地支类
        /// </summary>
        public struct SkyLoc
        {
            public string SkyLocName;
            public SkyClass Sky;
            public LocClass Loc;

        }

        public readonly static String[] CNMonthName = { "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "冬", "腊" };//十二个月名称
        public readonly static String[] CNLocTwelve = { "建", "除", "满", "平", "定", "执", "破", "危", "成", "收", "开", "闭" };//地支十二决
        public readonly static String[] CNDayName = { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "二十", "廿一", "廿二", "廿三", "廿四", "廿五", "廿六", "廿七", "廿八", "廿九", "三十" };//农历的日期

        public readonly static String[] Star = { "角木蛟", "亢金龙", "氐土貉", "房日兔", "心月狐", "尾火虎", "箕水豹", "斗木獬", "牛金牛", "女土蝠", "虚日鼠", "危月燕", "室火猪", "壁水貐", "奎木狼", "娄金狗", "胃土雉", "昴日鸡", "毕月乌", "觜火猴", "参水猿", "井木犴", "鬼金羊", "柳土獐", "星日马", "张月鹿", "翼火蛇", "轸水蚓" };
        //二十八宿
        public readonly static String[] GoodDays = { "青龙", "明堂", "金匮", "天德", "玉堂", "司命", "天乙", "日合", "日禄", "喜神", "日马", "日建", "天官", "宝光", "福星" };
        public readonly static String[] BadDays = { "天刑", "朱雀", "白虎", "天牢", "玄武", "勾陈", "日破", "不遇", "日刑", "旬空", "路空", "日害" };

        //壬辰年＝虚日鼠 戊申月＝昴日鸡

        /// <summary>
        /// 24节气
        /// </summary>
        public readonly static string[] SeasonNames = { "立春", "雨水", "惊蛰", "春分", "清明", "谷雨", "立夏", "小满", "芒种", "夏至", "小暑", "大暑", "立秋", "处暑", "白露", "秋分", "寒露", "霜降", "立冬", "小雪", "大雪", "冬至", "小寒", "大寒" };


        /// <summary>
        /// 十二日建
        /// </summary>
        public readonly static string[] DayBuilds = { "建", "除", "满", "平", "定", "执", "破", "危", "成", "收", "开", "闭" };


        /// <summary>
        /// 天赦日
        /// </summary>
        public readonly static string[] SkyPardonDays = { "戊寅", "甲午", "戊申", "甲子" };

        /// <summary>
        /// 四废日
        /// </summary>
        public readonly static string[] FourScrapDays = { "庚申,辛酉", "壬子,癸亥", "甲寅,乙卯", "丙午,丁巳" };

        /// <summary>
        /// 十恶大败日
        /// </summary>
        /// 庚戌年见甲辰日，辛亥年见乙巳日，壬寅年见丙申日，癸巳年见丁亥日，甲戌年见庚辰日，甲辰年见戊戌日，乙亥年见辛巳日，乙未年见己丑日，丙寅年见壬申日，丁巳年见癸亥日。
        public readonly static string[] TenDefeatDays = { "庚戌=甲辰", "辛亥=乙巳", "壬寅=丙申", "癸巳=丁亥", "甲戌=庚辰", "甲辰=戊戌", "乙亥=辛巳", "乙未=己丑", "丙寅=壬申", "丁巳=癸亥" };
        /// <summary>
        /// 十灵日
        /// </summary>
        public readonly static string[] TenSpiritDays = { "甲辰", "乙亥", "丙辰", "丁酉", "戊午", "庚戌", "庚寅", "辛亥", "壬寅", "癸未" };
    }
}
