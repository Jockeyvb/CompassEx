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
using System.Collections.ObjectModel;
using System.Linq;

namespace CompassEx.Gua
{
    /// <summary>
    /// 翻卦九星法：风水三大核心勘测对象（龙、山、水）的方法分类。
    /// </summary>
    /// <remarks>
    /// <para><b>方法论说明 (Methodology Note)：</b></para>
    /// <para>在风水理论中，<see cref="Dragon"/>（龙法）与 <see cref="Hill"/>（山法）的公式推导与应用方法论本质上是一致的，均采用黄石公翻卦掌之「边起边落、中起中落」变爻轨迹；而 <see cref="Water"/>（水法）则基于一套完全不同的计算逻辑与原理运作（如辅星翻卦水法，向上起星）。</para>
    /// </remarks>
    public enum GuaFlipMethod
    {
        /// <summary>
        /// 龙法 (Dragon Methods) - 通常使用来龙卦。与山法计算逻辑一致。
        /// </summary>
        Dragon = 0,

        /// <summary>
        /// 山法 (Hill Methods) - 通常使用坐山卦。与龙法计算逻辑一致。
        /// </summary>
        Hill = 1,

        /// <summary>
        /// 水法 (Water Methods) - 通常使用向卦。拥有独立的计算逻辑（如辅星翻卦）。
        /// </summary>
        Water = 2
    }

    /// <summary>
    /// 黄石公翻卦掌与辅星翻卦水法核心数字化推导类。
    /// </summary>
    /// <remarks>
    /// 本类作为 <c>CompassEx.Gua</c> 命名空间下的运算引擎，负责执行底层八卦变爻翻转计算。
    /// 内部核心轨迹已通过 O(1) 效率的数字化索引进行无缝链式建模，全面兼容 C# 主程序调用与第三方 JavaScript 插件扩展。
    /// </remarks>
    public class GuaFlip
    {
        /// <summary>
        /// 纳甲翻卦标准的八卦本宫字符串序列。
        /// </summary>
        /// <value>
        /// 包含 8 个元素的只读集合，索引严格对应：0-离、1-巽、2-坤、3-兑、4-乾、5-艮、6-坎、7-震。
        /// </value>
        /// <remarks>
        /// 本数组作为全盘翻卦的基准码表，配合轨迹字典可快速还原出具体的八卦实例。
        /// </remarks>
        private static readonly ReadOnlyCollection<string> FlipGuas =
            new ReadOnlyCollection<string>(new string[] { "离", "巽", "坤", "兑", "乾", "艮", "坎", "震" });

        /// <summary>
        /// 杨公纳甲黄石公翻卦诀核心数字化轨迹字典（物理锁死，防恶意篡改）。
        /// </summary>
        /// <value>
        /// 字典的 Key 为基础本宫卦名，Value 为 8 步翻卦后得到的 <see cref="FlipGuas"/> 索引序列。
        /// </value>
        /// <remarks>
        /// 本数字化数学模型完美抽象了传统「黄石公九星翻卦掌」的底层规律：
        /// <list type="bullet">
        ///   <item><description><b>边起边落双双起（离兑乾震）：</b> 变爻轨迹由边缘爻位起翻，中爻落宫。</description></item>
        ///   <item><description><b>中起中落双双起（巽坤艮坎）：</b> 变爻轨迹由中间爻位起翻，边爻落宫。</description></item>
        /// </list>
        /// </remarks>
        private static readonly ReadOnlyDictionary<string, int[]> FlipGuasDC =
            new ReadOnlyDictionary<string, int[]>(new Dictionary<string, int[]> {
                { "离", new int[] { 0, 7, 3, 6, 2, 5, 1, 4 } },
                { "兑", new int[] { 3, 4, 0, 5, 1, 6, 2, 7 } },
                { "乾", new int[] { 4, 3, 7, 2, 6, 1, 5, 0 } },
                { "震", new int[] { 7, 0, 4, 1, 5, 2, 6, 3 } },
                { "巽", new int[] { 1, 6, 2, 7, 3, 4, 0, 5 } },
                { "坤", new int[] { 2, 5, 1, 4, 0, 7, 3, 6 } },
                { "艮", new int[] { 5, 2, 6, 3, 7, 0, 4, 1 } },
                { "坎", new int[] { 6, 1, 5, 0, 4, 3, 7, 2 } }
            });

        /// <summary>
        /// 根据输入的初始主卦，推导并生成其完整的 8 步翻卦后天八卦序列。
        /// </summary>
        /// <param name="gs">要执行翻卦的基础主卦实例 (<see cref="GuaSubClass"/>)。</param>
        /// <returns>返回一个包含 8 个严格按翻卦掌诀顺序排列的 <see cref="GuaSubClass"/> 独立对象列表。</returns>
        /// <exception cref="ArgumentNullException">当传入的主卦对象为 null 时抛出。</exception>
        /// <exception cref="KeyNotFoundException">当传入的卦名不在标准的后天八卦序列中时抛出。</exception>
        public static List<GuaSubClass> GetGuaFlipGuas(GuaSubClass gs)
        {
            if (gs == null) throw new ArgumentNullException(nameof(gs));

            // 使用 TryGetValue 提升字典查找的防御健壮性
            if (!FlipGuasDC.TryGetValue(gs.Name, out int[] fg))
            {
                throw new KeyNotFoundException($"[数理错误] 未能在纳甲翻卦字典中寻获对应的本宫卦名: {gs.Name}");
            }

            // 🚀 利用 LINQ 结合 Lambda 一行流高效批量实例化生成 8 步卦象序列
            List<GuaSubClass> ls = fg.Select(i => new GuaSubClass(FlipGuas[i])).ToList();
            return ls;
        }

        /// <summary>
        /// 根据指定的勘测分类（龙、山、水），获得翻卦后全盘八卦与游年九星的完整对应关系字典。
        /// </summary>
        /// <param name="gs">要执行翻卦的基础主卦实例 (<see cref="GuaSubClass"/>)。</param>
        /// <param name="m">当前的翻卦九星勘测方法分类 (<see cref="GuaFlipMethod"/>)。</param>
        /// <returns>返回以 <see cref="GuaSubClass"/> 卦象为键，<see cref="NineStar"/> 游年星曜为值的配星映射字典。</returns>
        /// <remarks>
        /// <para><b>吉凶配星数理逻辑对照表：</b></para>
        /// <list type="table">
        ///   <listheader>
        ///     <term>勘测分类 (Method)</term>
        ///     <description>游年星曜演变顺序 (Star Progression Path)</description>
        ///   </listheader>
        ///   <item>
        ///     <term>龙法 (Dragon) / 山法 (Hill)</term>
        ///     <description>1辅(伏位) → 2贪(生气) → 3巨(天医) → 4禄(祸害) → 5文(六煞) → 6廉(五鬼) → 7武(延年) → 8破(绝命)</description>
        ///   </item>
        ///   <item>
        ///     <term>水法 (Water)</term>
        ///     <description>1辅(伏位) → 2武(延年) → 3破(绝命) → 4廉(五鬼) → 5贪(生气) → 6巨(天医) → 7禄(祸害) → 8文(六煞) （辅星水法特殊换算倒序轨迹）</description>
        ///   </item>
        /// </list>
        /// </remarks>
        public static Dictionary<GuaSubClass, NineStar> GetGuaFlipNineStarDC(GuaSubClass gs, GuaFlipMethod m)
        {
            // 1. 获取该本宫主卦对应的 8 步基础翻卦列表
            var ls = GetGuaFlipGuas(gs);
            var dc = new Dictionary<GuaSubClass, NineStar>();

            // 2. 区分流派：龙法与山法共享黄石公九星正序轨迹
            if (m is GuaFlipMethod.Dragon or GuaFlipMethod.Hill)
            {
                for (int i = 0; i < ls.Count; i++)
                {
                    // 内部通过 0-7 索引由 NineStar 工具类直接返回对应序位的静态星曜实例
                    dc.Add(ls[i], NineStar.GetGuaFlipEightStar(i));
                }
            }
            // 3. 区分流派：水法执行辅星翻卦特殊序列映射
            else if (m == GuaFlipMethod.Water)
            {
                // 🚀 核心水法轨迹映射数组（使用传统大括号写法，彻底防止 Markdown 语法渲染破坏）：
                // 1辅(0)、2武(6)、3破(7)、4廉(5)、5贪(1)、6巨(2)、7禄(3)、8文(4)
                int[] iWaters = new int[] { 0, 6, 7, 5, 1, 2, 3, 4 };

                for (int i = 0; i < ls.Count; i++)
                {
                    // 将经过水法洗牌后重组的星曜索引安全注入结果字典
                    dc.Add(ls[i], NineStar.GetGuaFlipEightStar(iWaters[i]));
                }
            }

            return dc;
        }
    }
}

