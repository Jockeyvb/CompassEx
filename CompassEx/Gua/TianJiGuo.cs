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

namespace CompassEx.Gua
{


    /// <summary>
    /// 表示天机出卦法的推演与判定实体类。
    /// </summary>
    /// <remarks>
    /// <para><b>文献记载：</b>本算法依据余胜唐、刘贲等老师的著作记载进行工程化实现，一般用于风水学中的“收山出煞”以及“三元命卦判断”等业务场景。</para>
    /// <para><b>算法原理：</b>根据六爻卦之卦宫（六纯卦）进行八卦爻变，通过京房易卦法的七世飞爻得出五卦（六爻卦），其上下拆分出的三爻卦集合（共 5 个）定义为“入卦”；其余未出现的 3 个三爻卦则定义为“出煞（出卦）”。</para>
    /// </remarks>
    public class TianJiGua
    {
        #region 属性

        /// <summary>
        /// 获取由当前卦宫推演出的七世飞爻卦（京房易卦序列）集合。
        /// </summary>
        /// <value>
        /// 包含 8 个 <see cref="GuaClass"/> 六爻卦对象的列表。
        /// </value>
        /// <remarks>
        /// 演变机制：由初爻开始往上变，以后一个最卦接着变出，共演变 7 次，包含本宫卦在内共计 8 个卦象。
        /// </remarks>
        public List<GuaClass> GuaList { get; private set; } = null;

        /// <summary>
        /// 获取当前天机卦局所对应的“入卦”后天八卦集合。
        /// </summary>
        /// <value>
        /// 键为后天八卦卦名，值为对应的 <see cref="GuaSubClass"/> 单卦对象字典。
        /// </value>
        public Dictionary<string, GuaSubClass> InGuaSubs { get; private set; }

        /// <summary>
        /// 获取当前天机卦局所对应的“出卦”（出煞）后天八卦集合。
        /// </summary>
        /// <value>
        /// 键为后天八卦卦名，值为对应的 <see cref="GuaSubClass"/> 单卦对象字典。
        /// </value>
        public Dictionary<string, GuaSubClass> OutGuaSubs { get; private set; }

        #endregion


        #region 构造函数

        /// <summary>
        /// 初始化 <see cref="TianJiGua"/> 类的新实例，自动根据传入的向卦计算出对应的天机入卦与出卦集合。
        /// </summary>
        /// <param name="ToGua">输入的罗盘向卦对象（六爻卦结构）。</param>
        /// <exception cref="ArgumentNullException">当传入的 <paramref name="ToGua"/> 对象为 <c>null</c> 时抛出此异常。</exception>
        /// <remarks>
        /// <para><b>初始化逻辑：</b></para>
        /// <list type="number">
        /// <item><description>通过向卦（<paramref name="ToGua"/>）自动寻找其所属的六纯卦卦宫（<c>GuaSelf</c>）。</description></item>
        /// <item><description>基于卦宫展开京房易卦飞爻算法，提取上下卦（三爻卦）去重后填充至 <see cref="InGuaSubs"/> 入卦集合。</description></item>
        /// <item><description>将未在入卦集合中出现的其余后天单卦提取出来，填充至 <see cref="OutGuaSubs"/> 出卦集合，用以进行出煞判定。</description></item>
        /// </list>
        /// </remarks>
        public TianJiGua(GuaClass ToGua)
        {
            if (ToGua == null)
                throw new ArgumentNullException(nameof(ToGua), "入参向卦不能为null");

            GuaClass GuaSelf = GuaClass.GetGuaClass(ToGua.GuaSelf.Name); // 取卦宫

            GuaList = GuaSelf.Get7HereYaoGua(); // 以卦宫来列出飞爻卦
            Dictionary<string, GuaSubClass> gscIns = new Dictionary<string, GuaSubClass>();

            // =========================== 获得入卦（三爻卦）后天 ===========================
            foreach (GuaClass gc in GuaList)
            {
                if (gscIns.ContainsKey(gc.DownGua.Name) == false)
                {
                    gscIns.Add(gc.DownGua.Name, gc.DownGua);
                }
                if (gscIns.ContainsKey(gc.UpGua.Name) == false)
                {
                    gscIns.Add(gc.UpGua.Name, gc.UpGua);
                }
            }
            this.InGuaSubs = gscIns; // 命卦的入卦（三爻卦）后天

            // =========================== 获得出卦（三爻卦）后天 ===========================
            Dictionary<string, GuaSubClass> gscOuts = new Dictionary<string, GuaSubClass>();
            foreach (string sN in GuaSubClass.BeforeGuaSubNames)
            {
                if (gscIns.ContainsKey(sN) == false && gscOuts.ContainsKey(sN) == false)
                {
                    gscOuts.Add(sN, GuaSubClass.GetGuaSub(sN, false));
                }
            }
            this.OutGuaSubs = gscOuts; // 命卦的出卦（三爻卦）后天
        }

        #endregion


        #region 方法

        /// <summary>
        /// 判定指定的后天八卦单卦在当前天机卦局中是否属于“出卦”。
        /// </summary>
        /// <param name="CompareGua">需要比对、判定的后天八卦单卦对象。</param>
        /// <returns>若该单卦存在于出卦集合（<see cref="OutGuaSubs"/>）中，则返回 <c>true</c>（代表已出卦）；否则返回 <c>false</c>。</returns>
        /// <remarks>
        /// <para>在玄空大卦风水体系中，主要通过此处的出卦逻辑进行<b>收山出煞</b>的度数判别（通常作用于罗盘 64 卦的分野上）。</para>
        /// <para><b>⚠️ 业务边界提示：</b>若您当前是要进行“三元命卦”的出卦判定，则需要严格按照纳甲法规则来进行推演。请不要使用本方法，改为调用 <c>FateGua.IsOutGua()</c> 方法。</para>
        /// </remarks>
        public bool IsOutGua(GuaSubClass CompareGua)
        {
            return OutGuaSubs.ContainsKey(CompareGua.Name);
        }

        #endregion
    }

}
