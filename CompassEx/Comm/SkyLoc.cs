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
    /// 表示天干地支组合（如“甲子”、“乙丑”等六十甲子单元）的实体类。
    /// </summary>
    /// <remarks>
    /// 本类作为干支历法推排的核心单元，通过将一个独立的天干实例（<see cref="SkyClass"/>）与一个独立的地支实例（<see cref="LocClass"/>）进行复合封装，提供了干支合称输出、跨类索引校验以及干支对象的动态初始化功能。
    /// </remarks>
    public class SkyLoc
    {
        #region 属性

        /// <summary>
        /// 获取当前天干地支组合的完整字面名称（如“甲子”、“丙寅”等）。
        /// </summary>
        /// <value>
        /// 一个由两个汉字组成的 <see cref="string"/> 字符串，动态拼接自 <see cref="SkyClass.Name"/> 与 <see cref="LocClass.Name"/>。
        /// </value>
        public string SkyLocName { get { return Sky.Name + Loc.Name; } }

        /// <summary>
        /// 获取当前干支组合中的天干实例对象。
        /// </summary>
        /// <value>
        /// 包含完整阴阳、五行及索引元数据的 <see cref="SkyClass"/> 实例。
        /// </value>
        public SkyClass Sky { get; private set; }

        /// <summary>
        /// 获取当前干支组合中的地支实例对象。
        /// </summary>
        /// <value>
        /// 包含完整时辰、阴阳、五行及索引元数据的 <see cref="LocClass"/> 实例。
        /// </value>
        public LocClass Loc { get; private set; }

        #endregion


        #region 构造函数

        /// <summary>
        /// 基于指定的双字干支组合名称（如“甲子”）初始化 <see cref="SkyLoc"/> 类的新实例。
        /// </summary>
        /// <param name="SkyLocName">由天干和地支按顺序组成的双字字符串（例如：“甲子”）。</param>
        /// <exception cref="IndexOutOfRangeException">当输入的字符串长度不足、或拆分出的单字无法在全局干支元数据中找到对应索引时抛出此异常。</exception>
        /// <remarks>
        /// <para><b>解构原理：</b></para>
        /// <para>方法内部会自动提取 <paramref name="SkyLocName"/> 的第 1 个字符（索引 0）作为天干字面量，提取第 2 个字符（索引 1）作为地支字面量。</para>
        /// <para>随后，通过分别检索它们在 <see cref="SkyClass.SkyNames"/> 和 <see cref="LocClass.LocNames"/> 中的位置，隐式链式传递给核心的双索引构造函数完成装配。</para>
        /// </remarks>
        public SkyLoc(string SkyLocName) : this(SkyClass.SkyNames.IndexOf(SkyLocName[0].ToString()), LocClass.LocNames.IndexOf(SkyLocName[1].ToString()))
        {
        }

        /// <summary>
        /// 基于指定的天干序列索引与地支序列索引初始化 <see cref="SkyLoc"/> 类的新实例。
        /// </summary>
        /// <param name="SkyIndex">天干在十天干序列中的绝对索引位置，有效取值范围为 <c>0 ~ 9</c>。可参考：<see cref="SkyClass.SkyNames"/>。</param>
        /// <param name="LocIndex">地支在十二地支序列中的绝对索引位置，有效取值范围为 <c>0 ~ 11</c>。可参考：<see cref="LocClass.LocNames"/>。</param>
        /// <exception cref="IndexOutOfRangeException">当传入的天干索引或地支索引超出其各自对应的合法物理数组边界时抛出此异常。</exception>
        /// <remarks>
        /// <b>★ 边界安全重构说明：</b>
        /// 已修正历史版本中对边界检查使用大于号（<c>&gt;</c>）导致的拦截穿透漏洞（例如：当 <paramref name="SkyIndex"/> 传入 <c>10</c> 时原逻辑无法拦截，会在下一步初始化时引发系统崩溃）。现已全面收紧为严谨的元素范围闭区间防御。
        /// </remarks>
        public SkyLoc(int SkyIndex, int LocIndex)
        {
            // ★ 核心修正：收紧边界检查，将原先的 > 替换为 >=，防止索引等于 Length 时的穿透崩溃
            if (SkyIndex < 0 || SkyIndex >= SkyClass.SkyNames.Length)
                throw new IndexOutOfRangeException(nameof(SkyIndex));

            if (LocIndex < 0 || LocIndex >= LocClass.LocNames.Length)
                throw new IndexOutOfRangeException(nameof(LocIndex));

            this.Sky = new SkyClass(SkyIndex);
            this.Loc = new LocClass(LocIndex);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取经典正统六十甲子干支组合的字符串序列（只读原生数组）。
        /// </summary>
        /// <returns>返回一个长度为 60 的 <see cref="string"/> 原生数组，按历法流转顺序包含“甲子”到“癸亥”。</returns>
        /// <remarks>
        /// <para><b>历法推演原理与算法重构：</b></para>
        /// <para>本方法已重构为现代高精度的<b>单层模运算算法</b>。消除了历史版本中双重嵌套循环（<c>int j = i</c>）导致的干支横向错配与漏项缺陷。</para>
        /// <para>由于十天干与十二地支的最小公倍数为 60，本算法通过单层 <c>0 ~ 59</c> 闭环流转，对天干基数 10 和地支基数 12 进行动态取模（<c>%</c>），确保干支双轨道同步顺时针推进，完美符合《黄帝内经》及传统干支历法规范。</para>
        /// </remarks>
        public static string[] Get60SkyLocNames()
        {
            string[] ls = new string[60];

            // 使用单层循环与取模，精准排出天干地支同步流转的六十甲子
            for (int k = 0; k < 60; k++)
            {
                int skyIndex = k % 10;
                int locIndex = k % 12;
                ls[k] = SkyClass.SkyNames[skyIndex] + LocClass.LocNames[locIndex];
            }

            return ls;
        }

        /// <summary>
        /// 获取包含完整天干地支对象绑定的六十甲子实体单元序列（只读原生数组）。
        /// </summary>
        /// <returns>返回一个包含 60 个 <see cref="SkyLoc"/> 复合实体的原生数组，按历法顺序排列。</returns>
        /// <remarks>
        /// <b>性能优化说明：</b>本方法通过单层取模算法动态实例化 60 个干支单元。由于六十甲子属于体系内的核心只读静态元数据，重构后直接返回原生数组，避免了动态列表（<c>List</c>）扩容带来的二次内存拷贝与垃圾回收（GC）开销。
        /// </remarks>
        public static SkyLoc[] Get60SkyLoc()
        {
            SkyLoc[] ls = new SkyLoc[60];

            // 同步利用最小公倍数模算法，构建高内聚的干支复合实体
            for (int k = 0; k < 60; k++)
            {
                int skyIndex = k % 10;
                int locIndex = k % 12;
                ls[k] = new SkyLoc(skyIndex, locIndex);
            }

            return ls;
        }

        #endregion



    }

}
