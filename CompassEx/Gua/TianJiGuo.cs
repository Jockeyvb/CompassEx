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
        /// 判断指定卦象是否属于“天机出卦”（用于玄空大卦物理峦头的收山出煞）
        /// 三元命卦的出卦判断不一样,要了解天机出卦法之人命卦出卦请看<see cref="FateGua.IsOutGua"/>
        /// </summary>
        /// <remarks>
        /// <para>
        /// <b>玄空大卦天机出卦法理论概述：</b><br/>
        /// 在玄空大卦（六十四卦）理气中，三元地理以“一运、二运、三运、四运”为江东卦（地元），
        /// “六运、七运、八运、九运”为江西卦（天元），“五运”天心顺逆分行。<br/>
        /// 所谓<b>出卦</b>，是指龙、山、向、水的气场没有处于同一个父母卦（或同运、通气）的管辖范围内，
        /// 导致阴阳差错、气场杂乱。在峦头修造中，若犯“天机出卦”，则无法达到“收山出煞”的效果，主凶。
        /// </para>
        /// <para>
        /// <b>本方法校验逻辑：</b><br/>
        /// 1. 提取对比卦（<paramref name="CompareGua"/>）的上卦（<c>UpGua</c>）在后天八卦或大卦系统中的量化数值（<c>AfterQuantity</c>）。<br/>
        /// 2. 在预设的出卦字典表（<c>OutGuaSubs</c>）中进行检索。<br/>
        /// 3. 若存在匹配记录，说明该卦象已跨越父母卦界限，判定为“出卦”（返回 <c>true</c>）；反之则为“不出卦”（返回 <c>false</c>）。
        /// </para>
        /// </remarks>
        /// <param name="CompareGua">需要进行出卦鉴定与比对的源卦象对象（<see cref="GuaClass"/>）。</param>
        /// <returns>
        /// 如果该卦象符合天机出卦规则，则返回 <see langword="true"/>（即属于出卦，峦头断为不吉）；
        /// 如果属于大卦内气、合局通气，则返回 <see langword="false"/>。
        /// </returns>
        /// <example>
        /// <code>
        /// GuaClass currentGua = GetCurrentGua();
        /// if (analyzer.IsOutGua(currentGua))
        /// {
        ///     // 犯天机出卦，需调整向线或进行收山出煞消砂化解
        ///     Console.WriteLine("警告：此局犯天机出卦！");
        /// }
        /// </code>
        /// </example>
        public bool IsOutGua(GuaClass CompareGua)
        {
            var r = OutGuaSubs.Where(gs => gs.Value.Name == CompareGua.UpGua.Name);
            //  Debug.Print(r.Any().ToString());
            return r.Any(); // 优化点：使用 Any() 比 Count() > 0 性能更好，内部只要找到一个就立即返回
        }

        /// <summary>
        /// 获得天盘六十四卦中所有属于“出卦”的卦象字典集合。
        /// 三元命卦的出卦判断不一样,要了解天机出卦法之人命卦出卦请看<see cref="FateGua.IsOutGua"/>
        /// </summary>
        /// <remarks>
        /// <para>
        /// <b>天盘六十四卦与出卦应用：</b><br/>
        /// 在玄空大卦理气中，天盘主要用于<b>收纳外气、消砂纳水</b>（部分流派亦用于推算天时公转之气）。
        /// 本方法通过遍历当前天盘中所有的初始卦象配置（<c>CompassEx.CBeforeGuas</c>），
        /// 逐一调用 <see cref="IsOutGua(GuaClass)"/> 方法进行天机出卦法的法理鉴定。
        /// </para>
        /// <para>
        /// <b>业务逻辑与过滤机制：</b><br/>
        /// 1. <b>高阶筛选 (Where)：</b> 利用 LINQ 表达式对天盘的方位卦象映射进行断言筛选，仅保留判定结果为“出卦”的条目。<br/>
        /// 2. <b>结构重组 (ToDictionary)：</b> 将筛选出的 <see cref="KeyValuePair{CompassRangEX, GuaClass}"/> 集合重新构建为强类型的字典。
        /// 此字典常用于后续的峦头风水吉凶断验，或在绘制罗盘时对出卦方位进行特殊的红线警告或煞位标注。
        /// </para>
        /// </remarks>
        /// <returns>
        /// 返回一个 <see cref="Dictionary{CompassRangEX, GuaClass}"/> 字典集合。<br/>
        /// 键（Key）为罗盘方位区间对象（<see cref="CompassRangEX"/>），
        /// 值（Value）为该方位上对应且犯了“天机出卦”的六十四卦卦象对象（<see cref="GuaClass"/>）。
        /// </returns>
        /// <seealso cref="IsOutGua(GuaClass)"/>
        /// <example>
        /// <code>
        /// // 示例：获取所有天盘出卦方位，并打印出对应的方位名称与卦名
        /// Dictionary&lt;CompassRangEX, GuaClass&gt; outGuas = compassAnalyzer.GetOutGuas();
        /// foreach (var kvp in outGuas)
        /// {
        ///     Console.WriteLine($"方位 [{kvp.Key.Name}] 对应的卦象 [{kvp.Value.GuaName}] 犯天机出卦，收山出煞时应当避开。");
        /// }
        /// </code>
        /// </example>
        public Dictionary<CompassRangEX, GuaClass> GetOutGuas()
        {
            return CompassEx.CBeforeGuas
                .Where(kv => IsOutGua(kv.Value))
                .ToDictionary(kv => kv.Key, kv => kv.Value);
        }


        #endregion
    }

}
