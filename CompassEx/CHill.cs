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

namespace CompassEx
{
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
        /// <para>该属性为只读属性，其内部会直接通过调用 <see cref="CompassEx.Get24HillDegree(string)"/> 方法，传入当前实例的 <see cref="Name"/> 实时计算得出。</para>
        /// <para>例如：当 <see cref="Name"/> 为“壬”时，返回的度数区间范围即为 <c>337.5 ~ 352.5</c> 度。</para>
        /// </remarks>
        public CompassRangEX CRangeDegree { get { return CompassEx.Get24HillDegree(this.Name); } }

        #endregion


        #region 构造函数

        /// <summary>
        /// 基于指定的山名初始化 <see cref="CHill"/> 类的新实例。
        /// </summary>
        /// <param name="sName">要创建的二十四山名称（如“壬”、“子”等）。</param>
        /// <remarks>
        /// 内部机制会先通过名称在 <see cref="C24HillNames"/> 数组中检索其对应的整型索引，随后将其隐式链式传递给带索引参数的构造函数。
        /// </remarks>
        public CHill(string sName) : this(C24HillNames.IndexOf(sName))
        {
        }

        /// <summary>
        /// 基于二十四山序列中的整型索引初始化 <see cref="CHill"/> 类的新实例。
        /// </summary>
        /// <param name="iIndex">二十四山在序列中的索引，有效取值范围为 <c>0 ~ 23</c>。</param>
        /// <exception cref="ArgumentOutOfRangeException">当传入的索引小于 0 或大于等于 24 时抛出此异常。</exception>
        public CHill(int iIndex)
        {
            if (iIndex < 0 || iIndex >= C24HillNames.Length)
                throw new ArgumentOutOfRangeException("iIndex", "索引必须在0到23之间");
            this.Name = C24HillNames[iIndex];
        }

        #endregion


        #region 方法

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




        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            // 强制转换为接口类型去调用，这样就能精准找到你下面写的方法，打破死循环！
            return ((IEquatable<CHill>)this).Equals(obj as CHill);
        }
        // 新增哈希方法，参与相等判断的字段全部组合计算
        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }

        bool IEquatable<CHill>.Equals(CHill other)
        {
            if (other == null) return false;
            return other.Name == this.Name;
        }
        #endregion
    }


}
