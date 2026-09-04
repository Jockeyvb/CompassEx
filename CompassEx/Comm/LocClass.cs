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
using System;
using System.Collections.Generic;

namespace CompassEx.Comm
{
    /// <summary>
    /// 地支类
    /// </summary>
    public class LocClass : IEquatable<LocClass>
    {

        #region 字段 

        /// <summary>
        /// 十二地支的经典字面名称全局静态只读序列。
        /// </summary>
        /// <value>
        /// 包含 12 个地支：从“子”开始至“亥”结束，索引对应 <c>0 ~ 11</c>。
        /// </value>
        public readonly static string[] LocNames = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };
        /// <summary>
        /// 十二地支在传统时辰中所对应的二十四小时制绝对时间段范围序列。
        /// </summary>
        /// <value>
        /// 包含 12 个时辰的时间区间字符串。順序嚴格對應：子、丑、寅、卯、辰、巳、午、未、申、酉、戌、亥。
        /// </value>
        /// <remarks>
        /// <list type="table">
        ///   <listheader>
        ///     <term>地支索引</term>
        ///     <description>傳統時辰 (Chinese Hour)</description>
        ///     <description>24小時制範圍 (24h Range)</description>
        ///   </listheader>
        ///   <item><term>0</term><description>子時 (Zi)</description><description>23:00 - 00:59</description></item>
        ///   <item><term>1</term><description>丑時 (Chou)</description><description>01:00 - 02:59</description></item>
        ///   <item><term>2</term><description>寅時 (Yin)</description><description>03:00 - 04:59</description></item>
        ///   <item><term>3</term><description>卯時 (Mao)</description><description>05:00 - 06:59</description></item>
        ///   <item><term>4</term><description>辰時 (Chen)</description><description>07:00 - 08:59</description></item>
        ///   <item><term>5</term><description>巳時 (Si)</description><description>09:00 - 10:59</description></item>
        ///   <item><term>6</term><description>午時 (Wu)</description><description>11:00 - 12:59</description></item>
        ///   <item><term>7</term><description>未時 (Wei)</description><description>13:00 - 14:59</description></item>
        ///   <item><term>8</term><description>申時 (Shen)</description><description>15:00 - 16:59</description></item>
        ///   <item><term>9</term><description>酉時 (You)</description><description>17:00 - 18:59</description></item>
        ///   <item><term>10</term><description>戌時 (Xu)</description><description>19:00 - 20:59</description></item>
        ///   <item><term>11</term><description>亥時 (Hai)</description><description>21:00 - 22:59</description></item>
        /// </list>
        /// </remarks>
        public readonly static string[] LocTimeValues = { "23:00-00:59", "01:00-02:59", "03:00-04:59", "05:00-06:59", "07:00-08:59", "09:00-10:59", "11:00-12:59", "13:00-14:59", "15:00-16:59", "17:00-18:59", "19:00-20:59", "21:00-22:59" };


        #endregion

        #region 属性

        /// <summary>
        /// 获取当前地支实例的字面名称（如“子”、“丑”等）。
        /// </summary>
        /// <value>
        /// 一个 <see cref="string"/> 字符串，表示当前地支的单字名称。
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// 获取当前地支在十二地支顺位序列中的绝对索引位置。
        /// </summary>
        /// <value>
        /// 一个 <see cref="int"/> 整数，有效取值范围为 <c>0 ~ 11</c>，对应 <see cref="LocNames"/> 中的位置。
        /// </value>
        public int Index { get { return LocNames.IndexOf(this.Name); } }


        /// <summary>
        /// 返回时辰点数值
        /// </summary>
        public int Hour { get { return int.Parse(LocTimeValues[this.Index].Substring(0, 2)); } }

        /// <summary>
        /// 地支月索引
        /// </summary>
        public int LocMonthIndex
        {
            get
            {
                int it = this.Index - 2;
                if (it < 0) it = it + 14;
                return it;
            }
        }

        /// <summary>
        /// 获取当前地支所归属的五行属性。
        /// </summary>
        /// <value>
        /// 返回一个 <see cref="FiveAttr"/> 对象（或枚举），代表该地支的五行特征（如寅卯属木、巳午属火等）。
        /// </value>
        public FiveAttr FiveAttr { get; private set; }

        /// <summary>
        /// 获取当前地支作为时辰时，对应的二十四小时制具体时间段字符串。
        /// </summary>
        /// <value>
        /// 一个形如 <c>"HH:mm-HH:mm"</c> 的时间区间字符串，动态映射自 <see cref="LocTimeValues"/> 数组。
        /// </value>
        public string LocTimeValue { get; private set; }

        #endregion


        #region 构造函数

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
        [JsonConstructor]
        public LocClass([JsonProperty(nameof(Index))] int LocIndex)
        {
            if (LocIndex < 0 || LocIndex > LocNames.Length - 1) throw new IndexOutOfRangeException();
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

            this.Name = LocNames[LocIndex];
        }

        #endregion

        #region 方法
        public override string ToString()
        {
            return this.Name.ToString();
        }
        /// <summary>
        /// 判定指定的日期时间对象是否落入当前地支所代表的时辰范围内（仅提取小时进行比对）。
        /// </summary>
        /// <param name="d">需要判定的完整 <see cref="DateTime"/> 日期时间对象。</param>
        /// <returns>若该时间属于当前地支的时辰区间则返回 <c>true</c>；否则返回 <c>false</c>。</returns>
        /// <remarks>
        /// 本方法是一个重载快捷方式，内部会将传入时间的 24 小时制小时部分格式化为字符串后，转发并链式调用给核心比对方法 <see cref="TimeValueInRangIndex(string)"/>。
        /// </remarks>
        public bool TimeValueInRangIndex(DateTime d)
        {
            return TimeValueInRangIndex(d.ToString("HH"));
        }

        /// <summary>
        /// 判定指定的小时数（24小时制字符串）是否落入当前地支所代表的时辰范围内。
        /// </summary>
        /// <param name="HH">24 小时制的小时字符串（如 "08"、"23" 等）。</param>
        /// <returns>若该小时数属于当前地支的时辰区间则返回 <c>true</c>；否则返回 <c>false</c>。</returns>
        /// <remarks>
        /// <para><b>时辰边界算法解析：</b></para>
        /// <para>由于<b>子时（Index = 0）</b>具有跨天交界的特殊性（即深夜 23:00 至次日凌晨 01:00），无法通过简单的单一区间闭合匹配。因此，当当前地支为子时时，系统会优先触发特殊防御逻辑：只要小时数大于等于 <c>23</c> 或等于 <c>0</c>，即直接判定命中并返回 <c>true</c>。</para>
        /// <para>对于其它普通时辰，方法会动态解构 <see cref="LocTimeValue"/> 的起止字符串，并将其转化为整型进行绝对闭区间匹配。</para>
        /// </remarks>
        public bool TimeValueInRangIndex(string HH)
        {
            string[] ssd = LocTimeValue.Split('-');
            int hh = int.Parse(HH);

            // ★ 特殊处理：子时跨天判定
            if (Index == 0)
            {
                if (hh >= 23 || hh == 0) return true; // 子时
            }

            string[] sd1 = ssd[0].Split(':');
            string[] sd2 = ssd[1].Split(':');

            if (hh >= int.Parse(sd1[0]) && hh <= int.Parse(sd2[0])) return true;

            return false;
        }

        /// <summary>
        /// 批量获取系统中完整配置的十二地支类实体集合。
        /// </summary>
        /// <returns>返回一个包含 12 个 <see cref="LocClass"/> 地支对象的 <see cref="List{T}"/> 列表，顺序完全对应二十四山及传统地支流转顺位。</returns>
        public static List<LocClass> GetAllLocClass()
        {
            List<LocClass> al = new List<LocClass>();
            foreach (string sn in LocNames)
            {
                al.Add(GetLocClass(sn));
            }
            return al;
        }

        /// <summary>
        /// 根据指定的地支单字字面名称，动态检索并实例化对应的地支类实体。
        /// </summary>
        /// <param name="LocName">要查询的地支名称（如“子”、“丑”等）。</param>
        /// <returns>若在全局元数据中成功匹配到该地支名，则返回填充好业务属性的 <see cref="LocClass"/> 实例；若输入的字符非法或不存在，则返回 <c>null</c>。</returns>
        public static LocClass? GetLocClass(string LocName)
        {
            int iPos = Array.IndexOf(LocNames, LocName);
            if (iPos == -1) return null;
            LocClass lc = GetLocClass(iPos);

            return lc;
        }

        /// <summary>
        /// 根据指定的全局静态顺位索引，动态检索并实例化对应的地支类实体。
        /// </summary>
        /// <param name="iLocIndex">地支在序列中的绝对索引位置，有效取值范围为 <c>0 ~ 11</c>。</param>
        /// <returns>返回带有完整时辰时间段、位置序列等属性绑定的 <see cref="LocClass"/> 地支类实体对象。</returns>
        /// <remarks>
        /// <b>内部对象装配：</b>该静态工厂方法在构造新实例的同时，会自动映射并填充对应的静态元数据缓存，包括 <see cref="LocTimeValue"/>、<see cref="LocClass( int )"/>  
        /// </remarks>
        public static LocClass GetLocClass(int iLocIndex)
        {
            LocClass lc = new LocClass(iLocIndex);

            lc.LocTimeValue = LocTimeValues[iLocIndex];

            lc.Name = LocNames[iLocIndex];
            return lc;
        }





        #region 显式实现对比、运算符和Key 方法
        // 1. 一般的 Equals(object)，內部可以轉型並利用顯式介面來比對
        public override bool Equals(object obj)
        {
            if (obj is LocClass other)
            {
                return ((IEquatable<LocClass>)this).Equals(other);
            }
            return false;
        }

        // 2. 顯式實作 IEquatable<LocClass>.Equals
        bool IEquatable<LocClass>.Equals(LocClass other)
        {
            // 檢查是否為 null
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            // 使用 string 的比較方式（考慮大小寫或 null 的防禦）
            return string.Equals(this.Name, other.Name, StringComparison.Ordinal);
        }

        // 3. 務必配合 Name 計算 HashCode
        public override int GetHashCode()
        {
            // 若 Name 可能為 null，可以用 HashCode.Combine 或字串自身的 GetHashCode
            return Name != null ? Name.GetHashCode() : 0;
        }

        // 4. (選用) 重載 == 與 != 運算子，建議透過介面轉型來呼叫
        public static bool operator ==(LocClass left, LocClass right)
        {
            if (left is null) return right is null;
            return ((IEquatable<LocClass>)left).Equals(right);
        }

        public static bool operator !=(LocClass left, LocClass right)
        {
            return !(left == right);
        }


        #endregion

        #endregion

    }

}
