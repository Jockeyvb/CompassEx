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
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompassEx.Gua
{


    /// <summary>
    /// 表示玄空大卦体系中“卦气”（洛书数及先天五行生成数）的推演与判别实体类。
    /// </summary>
    /// <remarks>
    /// <para><b>术数原理：</b></para>
    /// <para>本类实现了风水学玄空大卦的核心法则。六爻大卦的卦气通常是以其<b>上卦（外卦）</b>的三爻单卦为准进行推演。</para>
    /// <para>通过将当前卦的先天卦位（几何空间角度范围）与罗盘后天八卦的方位进行重合对齐，从而提取出该卦位上所承载的后天洛书运数（<c>GuaQiNumber</c>），并最终映射出基于河图洛书的先天五行属性（生成数五行）。</para>
    /// </remarks>
    public class GuaQi
    {
        /// <summary>
        /// 河图洛书生成数与先天五行属性的全局静态映射字典。
        /// </summary>
        /// <value>
        /// 键为 1 至 15 的洛书或河图数，值为对应的 <see cref="FiveAttr"/> 五行实体（如一六水、二七火、三八木、四九金、五十及十五土）。
        /// </value>
        public readonly Dictionary<int, FiveAttr> GuaQiFiveAttr = new Dictionary<int, FiveAttr>
    {
        { 1, new FiveAttr("水") }, { 6, new FiveAttr("水") },
        { 2, new FiveAttr("火") }, { 7, new FiveAttr("火") },
        { 3, new FiveAttr("木") }, { 8, new FiveAttr("木") },
        { 4, new FiveAttr("金") }, { 9, new FiveAttr("金") },
        { 5, new FiveAttr("土") }, { 10, new FiveAttr("土") }, { 15, new FiveAttr("土") }
    };

        #region 属性

        /// <summary>
        /// 获取当前卦气实例所引用的三爻基础单卦（或六爻卦的上卦）对象。
        /// </summary>
        /// <value>
        /// 一个封装了特定方位与八卦元数据的 <see cref="GuaSubClass"/> 实例。
        /// </value>
        public GuaSubClass GuaSub { get; private set; }

        /// <summary>
        /// 动态获取当前卦气对应的河洛先天五行属性。
        /// </summary>
        /// <value>
        /// 返回一个 <see cref="FiveAttr"/> 对象，代表一六水、二七火等河洛数产生的五行能量特征。
        /// </value>
        /// <remarks>
        /// 该属性为动态计算属性，内部通过当前推导出的洛书运数（<see cref="GuaQiNumber"/>）直接在 <see cref="GuaQiFiveAttr"/> 字典中进行索引检索。
        /// </remarks>
        public FiveAttr FiveAttr { get { return this.GuaQiFiveAttr[this.GuaQiNumber]; } }

        /// <summary>
        /// 动态计算并获取当前单卦所落先天的空间卦位在罗盘上对应的后天洛书卦气数。
        /// </summary>
        /// <value>
        /// 一个 <see cref="int"/> 整数，代表一至九运的玄空大卦洛书数（通常为后天卦序数加一）。
        /// </value>
        /// <exception cref="Exception">当罗盘空间度数计算错位，或未能匹配到任何合法的八卦全覆盖范围时抛出此异常。</exception>
        /// <remarks>
        /// <b>★ LINQ 架构重构解析：</b>
        /// 已将历史版本中的繁琐循环检索（<c>foreach</c>）重构为声明式的 LINQ 一行流。算法利用 LINQ 遍历八卦元数据流，动态抽取各个单卦的后天绝对范围，一旦判定本卦的先天空间范围起点（<c>Start</c>）完美落入该后天区间内，即立刻中断检索并安全产出其后天量化值。
        /// </remarks>
        public int GuaQiNumber
        {
            get
            {
                // 利用 LINQ 优雅检索完全覆盖目标先天卦位起点的后天八卦对象
                var matchNumber = GuaSubClass.BeforeGuaSubNames
                    .Select(sn => GuaSubClass.GetGuaSub(sn))
                    .Where(gs => gs != null && this.GuaSub.CBeforRangeDegree.IsInRange(gs.CAfterRangeDegree.Start))
                    .Select(gs => (int?)(gs.AfterQuantity + 1))
                    .FirstOrDefault();

                if (matchNumber.HasValue)
                    return matchNumber.Value;

                throw new Exception("未找到当前卦的合法空间对齐方位，无法推导玄空卦气洛数。");
            }
        }

        #endregion


        #region 构造函数

        /// <summary>
        /// 基于指定的六爻大卦名称初始化 <see cref="GuaQi"/> 类的新实例。
        /// </summary>
        /// <param name="GuaName">输入的完整六爻卦名称（如“乾为天”、“雷地豫”等）。</param>
        /// <remarks>
        /// <b>推演路径：</b>本构造函数会先根据名称实例化对应的六爻大卦实体 <see cref="GuaClass"/>，随后将其隐式转发给专门针对六爻卦的重载构造函数完成进一步拆分。
        /// </remarks>
        public GuaQi(string GuaName) : this(new GuaClass(GuaName))
        {
        }



        /// <summary>
        /// 基于完整的六爻大卦对象初始化 <see cref="GuaQi"/> 类的新实例，默认自动提取其上卦（外卦）作为计算依据。
        /// </summary>
        /// <param name="g">传入的完整复合六爻大卦实体对象。</param>
        /// <remarks>
        /// <b>大运法则：</b>依据玄空大卦经典风水规范，六爻大卦的卦气完全依附于其<b>上卦（外卦，即 <see cref="GuaClass.UpGua"/>）</b>的物理磁场。因此，本构造函数通过快捷获取上卦单卦实体，隐式传递给单卦构造函数完成初始化。
        /// </remarks>
        public GuaQi(GuaClass g) : this(g.UpGua)
        {
        }


        /// <summary>
        /// 基于核心的三爻基础单卦对象初始化 <see cref="GuaQi"/> 类的新实例。
        /// </summary>
        /// <param name="gs">传入的纯三爻基础单卦实体对象。</param>
        /// <exception cref="NullReferenceException">当传入的单卦对象 <paramref name="gs"/> 实例为空（<c>null</c>）时抛出此异常。</exception>
        public GuaQi(GuaSubClass gs)
        {
            if (gs == null)
                throw new NullReferenceException(nameof(gs));
            GuaSub = gs;
        }


        #endregion


        #region 方法


        #endregion
    }


}
