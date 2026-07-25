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
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CompassEx
{

    /// <summary>
    /// 表示风水罗盘上的二十四山盘层类型。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 本枚举采用位标记（Bitwise Flags）设计，支持使用按位或（OR）运算符进行多盘层的组合与判定。
    /// </para>
    /// <para>
    /// 三盘（地盘、天盘、人盘）在罗盘上呈同心圆错位排列，各自对齐不同的磁针方位，并应用于不同的风水堪舆范畴（如立向、消砂、纳水）。
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // 检查是否包含地盘或天盘
    /// HillType activePlates = HillType.CHill | HillType.SHill;
    /// if ((activePlates &amp; HillType.CHill) != 0) { /* 执行地盘逻辑 */ }
    /// </code>
    /// </example>
    [Flags]
    [Description("罗盘二十四山盘层类型")]
    public enum HillType : byte
    {
        /// <summary>
        /// 未指定或无效的盘层。
        /// </summary>
        [Description("无")]
        None = 0,

        /// <summary>
        /// 地盘正针二十四山。
        /// </summary>
        /// <remarks>
        /// 以正南北（磁针指向）为方位基准。主要用于测量山脉来龙走向（格龙）以及决定建筑物的坐向（立向），主掌内堂气场与乘气。
        /// </remarks>
        [Description("地盘二十四山")]
        CHill = 1,

        /// <summary>
        /// 天盘缝针二十四山。
        /// </summary>
        /// <remarks>
        /// 方位较地盘正针顺时针（向右）偏转 7.5 度。主要用于观看与测量水口、河流、道路等动态流水的来去方位（纳水），主掌财禄与富贵。
        /// </remarks>
        [Description("天盘二十四山")]
        SHill = 2,

        /// <summary>
        /// 人盘中针二十四山。
        /// </summary>
        /// <remarks>
        /// 方位较地盘正针逆时针（向左）偏转 7.5 度。主要用于测量周围静态山峰、高大建筑物（砂）的方位与五行生克（消砂），主掌人丁、健康与贵人。
        /// </remarks>
        [Description("人盘二十四山")]
        RHill = 4
    }


    /// <summary>
    /// 表示罗盘的二十四山及其相关属性与推演方法的实体类。
    /// </summary>
    /// <remarks>
    /// 二十四山是风水罗盘的核心组成部分，本类提供了基于正针二十四山的名称定位、阴阳属性判别、绝对度数范围计算，以及与之关联的后天八卦与先天 64 卦的嵌套推演功能。
    /// </remarks>
    public class CHill : IEquatable<CHill>
    {
        #region 字段

        /// <summary>
        /// 罗盘地盘正针的二十四山名称序列。
        /// </summary>
        /// <value>
        /// 包含 24 个山名，按照顺时针方向排列，以“壬”山为起始点，每山各占 15 度。
        /// </value>
        public static readonly String[] C24HillNames = { "壬", "子", "癸", "丑", "艮", "寅", "甲", "卯", "乙", "辰", "巽", "巳", "丙", "午", "丁", "未", "坤", "申", "庚", "酉", "辛", "戌", "乾", "亥" };

        /// <summary>
        /// 净阴净阳法则中归属于“阳”的十二山名称序列。
        /// </summary>
        /// <value>
        /// 包含 12 个属阳的山名：天干（壬、丙、甲、庚）、地支（寅、申、巳、亥）以及四维（艮、坤、巽、乾）。
        /// </value>
        public static readonly string[] C24HillSunNames = { "壬", "丙", "甲", "庚", "巽", "巳", "亥", "乾", "坤", "申", "艮", "寅" };

        #endregion


        #region 属性

        /// <summary>
        /// 获取当前山名是否在阴阳判定中属于“阳”。
        /// </summary>
        /// <value>
        /// 若属于净阳则返回 <c>true</c>；若属于净阴则返回 <c>false</c>。
        /// </value>
        /// <remarks>        
        /// 这里的阴阳判定基于二十四山净阴净阳理论。
        /// </remarks>
        public bool IsSun { get { return C24HillSunNames.IndexOf(this.Name) > -1; } }
        /// <summary>
        /// 24山类型   
        /// </summary>
        public HillType hillType { get; private set; } = HillType.CHill;

        /// <summary>
        /// 获取或设置当前二十四山对象的具体名称（如“壬”、“子”等）。
        /// </summary>
        /// <value>
        /// 一个 <see cref="string"/> 字符串，表示当前方位在二十四山罗盘中的字面名称。
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// 动态获取当前二十四山在罗盘上对应的绝对度数范围。
        /// </summary>
        /// <value>
        /// 返回一个封装了起始和结束角度的 <see cref="CompassRangEX"/> 对象。
        /// </value>
        /// <remarks>
        /// <para><b>计算依据：</b></para>
        /// <para>该属性为只读属性，其内部会直接通过调用 <see cref="CompassEx.Get24HillDegree(string,HillType)"/> 方法，传入当前实例的 <see cref="Name"/> 实时计算得出。</para>
        /// <para>例如：当 <see cref="Name"/> 为“壬”时，返回的度数区间范围即为 <c>337.5 ~ 352.5</c> 度。</para>
        /// </remarks>
        public CompassRangEX CRangeDegree { get { return CompassEx.Get24HillDegree(this.Name, this.hillType); } }

        #endregion


        #region 构造函数

        /// <summary>
        /// 基于指定的山名初始化 <see cref="CHill"/> 类的新实例。
        /// </summary>
        /// <param name="sName">要创建的二十四山名称（如“壬”、“子”、“癸”等）。</param>
        /// <param name="ht">当前山头所属的盘层类型。默认值为 <see cref="HillType.CHill"/>（地盘）。</param>
        /// <remarks>
        /// <para>
        /// 内部机制会先通过名称在 <see cref="C24HillNames"/> 数组中检索其对应的整型索引，随后将其隐式链式传递给带索引参数的构造函数。
        /// </para>
        /// <para>
        /// <b>注意：</b>如果传入的 <paramref name="sName"/> 在数组中不存在，检索索引将为 <c>-1</c>，此时会链式触发 <see cref="ArgumentOutOfRangeException"/> 异常。
        /// </para>
        /// </remarks>
        public CHill(string sName, HillType ht = HillType.CHill) : this(C24HillNames.IndexOf(sName), ht)
        {
        }

        /// <summary>
        /// 基于二十四山序列中的整型索引初始化 <see cref="CHill"/> 类的新实例。
        /// </summary>
        /// <param name="iIndex">二十四山在序列中的索引，有效取值范围为 <c>0 ~ 23</c>。</param>
        /// <param name="ht">当前山头所属的盘层类型。默认值为 <see cref="HillType.CHill"/>（地盘）。</param>
        /// <exception cref="ArgumentOutOfRangeException">当传入的索引小于 0 或大于等于 24 时抛出此异常。</exception>
        public CHill(int iIndex, HillType ht = HillType.CHill)
        {
            if (iIndex < 0 || iIndex >= C24HillNames.Length)
                throw new ArgumentOutOfRangeException(nameof(iIndex), "索引必须在0到23之间");

            this.Name = C24HillNames[iIndex];
            this.hillType = ht;
        }

        #endregion



        #region 方法

        /// <summary>
        /// 获取二十四山方位对应的角度范围。
        /// </summary>
        /// <remarks>
        /// <para>二十四山是风水学与中国传统历法中用于标示方位的重要概念，将圆周 360 度等分为 24 个扇区，每个扇区占 15 度。</para>
        /// <para>本方法通过传入山名和类型，返回对应的 <see cref="CompassRangEX"/> 角度范围对象。</para>
        /// </remarks>
        /// <param name="HillName">二十四山的名称（例如："子"、"午"、"卯"、"酉"、"乾"、"巽"、"艮"、"坤" 等）。</param>
        /// <param name="ht">山向类型，默认为中国传统二十四山（<see cref="HillType.CHill"/>）。</param>
        /// <returns>返回包含起始角度、终止角度及中心角度的 <see cref="CompassRangEX"/> 方位范围对象。</returns>
        /// <exception cref="System.ArgumentNullException">当 <paramref name="HillName"/> 为 null 或空字符串时抛出。</exception>
        /// <example>
        /// <code>
        /// // 示例：获取传统中国二十四山中“子”山的角度范围
        /// CompassRangEX range = CompassEx.Get24HillDegree("子");
        /// Console.WriteLine($"子山范围：{range.StartDegree}° 至 {range.EndDegree}°");
        /// </code>
        /// </example>
        public static CompassRangEX Get24HillDegree(string HillName, HillType ht = HillType.CHill)
        {
            return CompassEx.Get24HillDegree(HillName, ht);
        }


        /// <summary>
        /// 动态计算并获取地盘正针本山所对应的后天八卦单卦对象。
        /// </summary>
        /// <returns>返回本山度数所在的 <see cref="GuaSubClass"/> 后天八卦方位对象。</returns>
        /// <remarks>
        /// 其内部原理是以当前二十四山绝对范围的起始度数（<see cref="CompassRangEX.Start"/>）作为基准点，动态构建临时的 <see cref="CompassEx"/> 罗盘实例来进行反向卦象推演。
        /// </remarks>
        public GuaSubClass GetAfterGua()
        {
            CompassEx ce = new CompassEx(this.CRangeDegree.Start);
            return ce.GetAfterGuaSub();
        }

        /// <summary>
        /// 获取当前正针二十四山度数范围内所包含、跨越的所有天盘先天 64 卦对象集合。
        /// </summary>
        /// <returns>返回一个以 <see cref="CompassRangEX"/> 为键、<see cref="GuaClass"/> 为值的字典集合，包含所有落入该山范围内的卦象。</returns>
        /// <remarks>
        /// <para><b>⚠️ 边界算法解析：</b></para>
        /// <para>由于二十四山的分野（每山 15 度）与 64 卦的分野（每卦 5.625 度）在几何上并不整齐重合，存在交错跨越的现象。</para>
        /// <para>为了实现高精度的卦象检索，本方法在比对结束边界时，对结束度数执行了减去 <c>0.01</c> 度的微小容差修正（即 <c>kv.Key.End - 0.01</c>），从而能够完美、精确地将临界卦象对象拉入当前二十四山的归属区间内。</para>
        /// </remarks>
        public Dictionary<CompassRangEX, GuaClass> GetCBeforGuas()
        {
            Dictionary<CompassRangEX, GuaClass> dc = new Dictionary<CompassRangEX, GuaClass>();
            CompassRangEX CRE = this.CRangeDegree;
            foreach (var kv in C3Y.CBeforeGuas)
            {
                if (CRE.IsInRange(kv.Key.Start) || CRE.IsInRange(kv.Key.End - 0.01))
                {
                    dc.Add(kv.Key, kv.Value);
                }
            }
            return dc;
        }




        /// <summary>
        /// 确定指定的对象是否等于当前 <see cref="CHill"/> 实例。
        /// </summary>
        /// <param name="obj">要与当前实例进行比较的对象。</param>
        /// <returns>如果指定的对象等于当前实例，则为 <c>true</c>；否则为 <c>false</c>。</returns>
        public override bool Equals(object obj)
        {
            // 使用 C# 7.0 模式匹配，同时进行 null 检查和类型转换，更优雅高效
            if (obj is CHill other)
            {
                return ((IEquatable<CHill>)this).Equals(other);
            }
            return false;
        }

        /// <summary>
        /// 返回当前 <see cref="CHill"/> 实例的哈希代码。
        /// </summary>
        /// <returns>当前实例的 32 位有符号整数哈希代码。</returns>
        /// <remarks>
        /// 算法遵循乘法哈希机制（非对称质数相乘），将参与相等性判定的 <see cref="Name"/> 和 <see cref="hillType"/> 字段进行组合计算，
        /// 以确保在散列表（如 <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/>）中具备优秀的分布均匀性。
        /// </remarks>
        public override int GetHashCode()
        {
            // 分配初始质数（种子值）
            int hash = 17;

            // 阶梯式引入所有参与 Equals 判定的字段，乘以另一个质数（如 23 或 31）防止哈希碰撞
            unchecked // 允许整型溢出，不触发异常
            {
                hash = (hash * 23) + (Name != null ? Name.GetHashCode() : 0);
                hash = (hash * 23) + hillType.GetHashCode();
            }

            return hash;
        }

        /// <summary>
        /// 指示当前 <see cref="CHill"/> 实例是否等于同类型的另一个实例。
        /// </summary>
        /// <param name="other">要与当前实例进行比较的另一个 <see cref="CHill"/> 实例。</param>
        /// <returns>如果两个实例的名称和盘层类型完全相同，则为 <c>true</c>；否则为 <c>false</c>。</returns>
        bool IEquatable<CHill>.Equals(CHill other)
        {
            if (other == null) return false;

            // 核心判定：只有山头名称且所属盘层类型完全一致，才视为同一个对象
            return this.Name == other.Name && this.hillType == other.hillType;
        }
        #endregion
    }


}
