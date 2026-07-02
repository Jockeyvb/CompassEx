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

    /// <summary>
    /// 用于封装和标识罗盘对象的元数据及其实例的结构体。
    /// </summary>
    /// <remarks>
    /// 该结构体主要用于多类型罗盘数据块的统一存储（例如存入 <see cref="System.Collections.Generic.List{T}"/>），
    /// 它将罗盘对象的中文名称、运行时类型、实例引用以及所对应的角度范围（<see cref="CompassRangEX"/>）打包在一起，
    /// 并提供安全的类型分发与转换机制。
    /// </remarks>
    public struct CompassObjType
    {
        /// <summary>
        /// 罗盘对象的类型中文名称。
        /// </summary>
        /// <value>
        /// 例如：“先天八卦”、“后天八卦”、“天盘64卦、地盘64卦”等描述性文本。
        /// </value>
        public string ObjTypeCNName;

        /// <summary>
        /// 对应罗盘对象名称
        /// </summary>
        /// <value>
        /// 例如：“乾”、“姤”、“子”等描述性文本。
        /// </value>
        public string Name;
        /// <summary>
        /// 罗盘对象的运行时类型。
        /// </summary>
        /// <value>
        /// 存储该对象实例对应的真实 <see cref="Type"/> 信息。
        /// </value>
        public Type ObjType;

        /// <summary>
        /// 罗盘对象的底层实例引用。
        /// </summary>
        /// <value>
        /// 实际的业务对象（已被装箱为 <see cref="object"/>），使用时建议通过 <see cref="ToObjet{T}(out T)"/> 安全转型。
        /// </value>
        public object Obj;

        /// <summary>
        /// 罗盘对象所涵盖的角度范围。
        /// </summary>
        /// <value>
        /// 一个 <see cref="CompassRangEX"/> 实例，用于判定特定度数是否属于当前罗盘节点。
        /// </value>
        public CompassRangEX CRDegree;

        /// <summary>
        /// 将内部的 <see cref="Obj"/> 安全转换为指定的泛型类型，支持类型自动推导。
        /// </summary>
        /// <typeparam name="T">期望转换的目标输出类型。</typeparam>
        /// <param name="result">输出参数。如果转换成功，则返回转型后的 <typeparamref name="T"/> 类型实例；如果转换失败，则返回该类型的默认值（<see langword="default"/>）。</param>
        /// <returns>
        /// 如果 <see cref="Obj"/> 不为 <see langword="null"/> 且其类型兼容于 <typeparamref name="T"/>，则返回 <see langword="true"/>；否则返回 <see langword="false"/>。
        /// </returns>
        /// <remarks>
        /// 此方法内部使用 <see cref="Type.IsAssignableFrom(Type)"/> 进行安全的类型继承链检查。
        /// 采用 <see langword="out"/> 关键字设计，使得外部调用时可以通过 <c>out var</c> 语法省略泛型参数的具体书写，提高代码可读性。
        /// </remarks>
        /// <example>
        /// <code>
        /// CompassObjType item = GetCompassData();
        /// 
        /// // 利用 out var 特性，外部无需显式书写 &lt;GuoSubClass&gt;
        /// if (item.ToObjet(out GuoSubClass guoObj))
        /// {
        ///     // 转换成功，直接使用强类型变量 guoObj
        ///     Console.WriteLine($"成功获取到郭氏对象，名称为: {guoObj.Name}");
        /// }
        /// else
        /// {
        ///     Console.WriteLine("类型不匹配或对象为空。");
        /// }
        /// </code>
        /// </example>
        public bool ToObjet<T>(out T result)
        {
            // 检查实例是否不为空，且外部的目标类型 T 是否可以由当前的 ObjType 赋值（支持父类/接口兼容）
            if (this.Obj != null && typeof(T).IsAssignableFrom(this.ObjType))
            {
                result = (T)this.Obj;
                return true;
            }

            // 类型不匹配或对象为空时，清空返回参数并返回失败标记
            result = default;
            return false;
        }
    }


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
    public static class Defined
    {

        /// <summary>
        /// 太极符号
        /// </summary>
        public static readonly string TaiChiSymbol = "\u262F";  //太极图



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
