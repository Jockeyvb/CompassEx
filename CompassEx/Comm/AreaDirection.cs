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
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CompassEx.Comm
{
    /// <summary>
    /// 表示区域方位类，用于管理罗盘八方及中宫的名称、度数范围以及飞星轨迹映射。
    /// </summary>
    public class AreaDirection : IEquatable<AreaDirection>
    {
        /// <summary>
        /// 预定义八个方位及中宫的名称数组（按罗盘顺时针定义）。
        /// </summary>
        public static readonly string[] Names = { "正北", "东北", "正东", "东南", "正南", "西南", "正西", "西北", "中宫" };

        /// <summary>
        /// 根据飞星轨迹顺序排列的方位名称数组。
        /// </summary>
        public static readonly string[] FlyStarAreaNames = { "西北", "正西", "东北", "正南", "正北", "西南", "正东", "东南", "中宫" };

        /// <summary>
        /// 根据飞星轨迹顺序排列的飞星数值数组。
        /// </summary>
        public static readonly int[] FlyStarValues = { 6, 7, 8, 9, 1, 2, 3, 4, 5 };

        private static CompassRangEX[]? _Rangs;

        /// <summary>
        /// 获取默认的八个方位的度数范围，每个方位以 45 度为范围（中宫设为 -1）。
        /// </summary>
        /// <value>包含各个方位度数范围的 <see cref="CompassRangEX"/> 数组。</value>
        public static CompassRangEX[] Rangs
        {
            get
            {
                if (_Rangs == null)
                {
                    List<CompassRangEX> ls = new List<CompassRangEX>();
                    double last = 22.5;
                    for (int i = 0; i < Names.Length; i++)
                    {
                        if (i == 0)
                        {
                            ls.Add(new CompassRangEX(337.5, 22.5));
                        }
                        else if (i < Names.Length - 1)
                        {
                            ls.Add(new CompassRangEX(last, last + 45));
                            last += 45;
                        }
                        else // 五黄设置为-1
                        {
                            ls.Add(new CompassRangEX(-1, -1));
                        }
                    }
                    _Rangs = ls.ToArray();
                }

                return _Rangs;
            }
        }

        /// <summary>
        /// 获取当前方位的八方名称。
        /// </summary>
        /// <value>方位的字符串名称，例如 "正北"、"中宫" 等。</value>
        public string? Name { get; private set; }

        /// <summary>
        /// 获取当前方位的索引值。
        /// </summary>
        /// <value>对应 <see cref="Names"/> 数组中的索引整型值。</value>
        public int Index { get; private set; }

        /// <summary>
        /// 获取当前方位的度数范围对象。
        /// </summary>
        /// <value>表示该方位角度区间的 <see cref="CompassRangEX"/> 实例。</value>
        public CompassRangEX? Rang { get; private set; }

        /// <summary>
        /// 初始化 <see cref="AreaDirection"/> 类的新实例，使用指定的方位名称创建。
        /// </summary>
        /// <param name="name">方位的名称（例如："正北"、"中宫"等）。</param>
        public AreaDirection(string? name) : this(Names.IndexOf(name))
        {
        }

        /// <summary>
        /// 初始化 <see cref="AreaDirection"/> 类的新实例，基于指定的索引创建。
        /// </summary>
        /// <param name="index">方位的索引值。</param>
        /// <exception cref="ArgumentOutOfRangeException">当索引超出有效范围时抛出。</exception>
        public AreaDirection(int index)
        {
            if (index < 0 || index >= Names.Length) throw new ArgumentOutOfRangeException(nameof(index));

            this.Index = index;
            this.Name = Names[index];
            this.Rang = Rangs[index];
        }

        /// <summary>
        /// 根据飞星数值获取相关的区域方位类实例。
        /// </summary>
        /// <param name="flyStarValue">飞星的数值（如 1 到 9 之间的整数）。</param>
        /// <returns>返回对应的 <see cref="AreaDirection"/> 实例；若未找到则可能返回 <c>null</c>。</returns>
        public static AreaDirection? GetAreaDirectionByFlyStar(int flyStarValue)
        {
            int index = Names.IndexOf(FlyStarAreaNames[FlyStarValues.IndexOf(flyStarValue)]);

            return new AreaDirection(index);
        }

        /// <summary>
        /// 根据度数范围返回对应的区域方位类实例。
        /// </summary>
        /// <param name="cr">罗盘度数范围对象 <see cref="CompassRangEX"/>。</param>
        /// <returns>返回对应的 <see cref="AreaDirection"/> 实例；若未找到匹配范围则返回 <c>null</c>。</returns>
        public static AreaDirection? GetAreaDirection(CompassRangEX cr)
        {
            return GetAreaDirection(cr.Start);
        }

        /// <summary>
        /// 根据具体的罗盘角度数值返回对应的区域方位类实例。
        /// </summary>
        /// <param name="angle">角度值（0 到 360 度之间）。</param>
        /// <returns>返回对应的 <see cref="AreaDirection"/> 实例；若未找到匹配的区间则返回 <c>null</c>。</returns>
        public static AreaDirection? GetAreaDirection(double angle)
        {
            var rs = Rangs;
            for (int i = 0; i < rs.Length; i++)
            {
                if (rs[i].IsInRange(angle))
                {
                    return new AreaDirection(i);
                }
            }
            return null;
        }

        #region 显式实现对比、运算符和 Key 方法

        /// <summary>
        /// 确定指定的对象是否等于当前对象。
        /// </summary>
        /// <param name="obj">要与当前对象进行比较的对象。</param>
        /// <returns>如果指定的对象与当前对象相等，则为 <c>true</c>；否则为 <c>false</c>。</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as AreaDirection);
        }

        /// <summary>
        /// 指示当前对象是否等于同一类型的另一个对象。
        /// </summary>
        /// <param name="other">与此对象进行比较的对象。</param>
        /// <returns>如果当前对象等于 <paramref name="other"/> 参数，则为 <c>true</c>；否则为 <c>false</c>。</returns>
        bool IEquatable<AreaDirection>.Equals(AreaDirection other)
        {
            // 檢查是否為 null
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            // 使用 string 的比較方式（考慮大小寫或 null 的防禦）
            return string.Equals(this.Name, other.Name, StringComparison.Ordinal);
        }

        /// <summary>
        /// 用作特定类型的哈希函数。
        /// </summary>
        /// <returns>当前对象的哈希代码。</returns>
        public override int GetHashCode()
        {
            // 若 Name 可能為 null，可以用 HashCode.Combine 或字串自身的 GetHashCode
            return Name != null ? Name.GetHashCode() : 0;
        }

        /// <summary>
        /// 判断两个 <see cref="AreaDirection"/> 实例是否相等。
        /// </summary>
        /// <param name="left">左侧的实例。</param>
        /// <param name="right">右侧的实例。</param>
        /// <returns>如果两个实例相等，则为 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool operator ==(AreaDirection left, AreaDirection right)
        {
            if (left is null) return right is null;
            return ((IEquatable<AreaDirection>)left).Equals(right);
        }

        /// <summary>
        /// 判断两个 <see cref="AreaDirection"/> 实例是否不相等。
        /// </summary>
        /// <param name="left">左侧的实例。</param>
        /// <param name="right">右侧的实例。</param>
        /// <returns>如果两个实例不相等，则为 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool operator !=(AreaDirection left, AreaDirection right)
        {
            return !(left == right);
        }

        #endregion
    }
}
