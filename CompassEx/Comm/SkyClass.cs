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
    /// 天干类
    /// </summary>
    public class SkyClass
    {
        #region 字段

        /// <summary>
        /// 十天干的经典字面名称全局静态只读序列。
        /// </summary>
        /// <value>
        /// 包含 10 个天干：从“甲”开始至“癸”结束，索引对应 <c>0 ~ 9</c>。
        /// </value>
        public static string[] SkyNames = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };

        #endregion


        #region 属性

        /// <summary>
        /// 获取当前天干实例在阴阳判定中是否属于“阳干”。
        /// </summary>
        /// <value>
        /// 若属于阳干（如甲、丙、戊、庚、壬）则返回 <c>true</c>；若属于阴干（如乙、丁、己..）则返回 <c>false</c>。
        /// </value>
        /// <remarks>
        /// 判定原理：依据天干在序列中的绝对索引位置进行取模运算（<c>Index % 2 == 0</c>），偶数索引为阳，奇数索引为阴。
        /// </remarks>
        public bool IsSun { get { return this.Index % 2 == 0; } }

        /// <summary>
        /// 获取当前天干实例的字面名称（如“甲”、“乙”等）。
        /// </summary>
        /// <value>
        /// 一个 <see cref="string"/> 字符串，表示当前天干的单字名称。
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// 获取当前天干在十天干顺位序列中的绝对索引位置。
        /// </summary>
        /// <value>
        /// 一个 <see cref="int"/> 整数，有效取值范围为 <c>0 ~ 9</c>，对应 <see cref="SkyNames"/> 中的位置。
        /// </value>
        public int Index { get; private set; }

        /// <summary>
        /// 获取当前天干所归属的五行属性。
        /// </summary>
        /// <value>
        /// 返回一个 <see cref="FiveAttr"/> 对象（或枚举），代表该天干的五行特征（如甲乙属木、丙丁属火等）。
        /// </value>
        public FiveAttr FiveAttr { get; private set; }

        #endregion


        #region 构造函数

        /// <summary>
        /// 基于指定的天干单字字面名称初始化 <see cref="SkyClass"/> 类的新实例。
        /// </summary>
        /// <param name="sSkyName">要创建的天干名称（如“甲”、“乙”等）。</param>
        /// <exception cref="IndexOutOfRangeException">当传入的名称不在全局静态元数据 <see cref="SkyNames"/> 中时抛出此异常。</exception>
        /// <remarks>
        /// 内部机制会先通过名称检索其对应的整型索引，随后将其隐式链式传递给带索引参数的构造函数。
        /// </remarks>
        public SkyClass(string sSkyName) : this(Array.IndexOf(SkyNames, sSkyName))
        {
        }

        /// <summary>
        /// 基于十天干序列中的整型索引初始化 <see cref="SkyClass"/> 类的新实例，并自动动态装配其五行属性。
        /// </summary>
        /// <param name="iSkyIndex">天干在序列中的绝对索引位置，有效取值范围为 <c>0 ~ 9</c>。可参考全局静态字段：<see cref="SkyNames"/>。</param>
        /// <exception cref="IndexOutOfRangeException">当传入的索引小于 0 或大于等于 10 时抛出此异常。</exception>
        /// <remarks>
        /// <para><b>五行流转分配矩阵：</b></para>
        /// <para>本构造函数内部实现了经典正统的天干五行映射机制：</para>
        /// <list type="bullet">
        /// <item><description>索引 <c>0, 1</c>（甲、乙） <c> ==&gt; </c> 自动实例化为 <b>木</b> 属性</description></item>
        /// <item><description>索引 <c>2, 3</c>（丙、丁） <c> ==&gt; </c> 自动实例化为 <b>火</b> 属性</description></item>
        /// <item><description>索引 <c>4, 5</c>（戊、己） <c> ==&gt; </c> 自动实例化为 <b>土</b> 属性</description></item>
        /// <item><description>索引 <c>6, 7</c>（庚、辛） <c> ==&gt; </c> 自动实例化为 <b>金</b> 属性</description></item>
        /// <item><description>索引 <c>8, 9</c>（壬、癸） <c> ==&gt; </c> 自动实例化为 <b>水</b> 属性（★已修复历史版本误指为土的隐患）</description></item>
        /// </list>
        /// </remarks>
        public SkyClass(int iSkyIndex)
        {
            if (iSkyIndex < 0 || iSkyIndex >= SkyNames.Length)
                throw new IndexOutOfRangeException();

            if (iSkyIndex < 2)
            {
                this.FiveAttr = new FiveAttr("木");
            }
            else if (iSkyIndex == 2 || iSkyIndex == 3)
            {
                this.FiveAttr = new FiveAttr("火");
            }
            else if (iSkyIndex == 4 || iSkyIndex == 5)
            {
                this.FiveAttr = new FiveAttr("土");
            }
            else if (iSkyIndex == 6 || iSkyIndex == 7)
            {
                this.FiveAttr = new FiveAttr("金");
            }
            else
            {
                // ★ 核心修复：壬癸对应的最后一项五行应当是“水”，原代码写成了“土”
                this.FiveAttr = new FiveAttr("水");
            }

            this.Index = iSkyIndex;
            this.Name = SkyNames[iSkyIndex];
        }

        #endregion
        #region 方法
        /// <summary>
        /// 根据指定的天干单字字面名称，利用 LINQ 动态检索并实例化对应的天干类实体。
        /// </summary>
        /// <param name="sSkyName">要查询的天干字面名称（如“甲”、“乙”等）。</param>
        /// <returns>若在全局元数据中成功匹配到该天干名，则返回对应填充好五行及索引属性的 <see cref="SkyClass"/> 实例；若输入的字符非法或不存在，则返回 <c>null</c>。</returns>
        public static SkyClass GetSkyClass(string sSkyName)
        {
            int iPos = SkyNames.IndexOf(sSkyName);
            return GetSkyClass(iPos);
        }

        /// <summary>
        /// 根据指定的全局静态顺位索引，动态实例化对应的天干类实体。
        /// </summary>
        /// <param name="iSkyIndex">天干在序列中的绝对索引位置，有效取值范围为 <c>0 ~ 9</c>。</param>
        /// <returns>返回带有完整五行属性绑定、索引以及名称的 <see cref="SkyClass"/> 天干类实体对象。</returns>
        /// <exception cref="IndexOutOfRangeException">当传入的 <paramref name="iSkyIndex"/> 小于 0 或大于等于 10 时，内部构造函数会抛出此异常。</exception>
        /// <remarks>
        /// <b>内部对象装配：</b>该静态工厂方法通过链式调用带参构造函数 <see cref="SkyClass(int)"/> 来实现实体的动态创建，新对象会自动将对应的天干名称与正统五行矩阵进行映射挂载。
        /// </remarks>
        public static SkyClass GetSkyClass(int iSkyIndex)
        {
            return new SkyClass(iSkyIndex);
        }

        #endregion


    }
}
