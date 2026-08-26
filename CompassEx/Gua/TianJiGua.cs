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
    /// <para><b>书籍记载：</b>本算法依据余胜唐、刘贲等老师的著作记载进行工程化实现，一般用于风水学中的“收山出煞”(非三元命卦)业务场景。
    /// <br/>在立向时可使用<see cref="GetOutGuas"/> 方法获得所有出卦的六爻卦
    /// <br />若要使用“三元命卦判断”请查看<see cref="FateGua"/>及<see cref="FateGua.FateGua(DateTime, string, GuaClass)"/>。</para>
    /// <para><b>算法原理：</b>根据六爻卦之卦宫（六纯卦）进行八卦爻变，通过京房易卦法的七世飞爻得出五卦（六爻卦），其上下拆分出的三爻卦集合（共 5 个）定义为“入卦”；其余未出现的 3 个三爻卦则定义为“出煞（出卦）”。
    /// </para>
    /// <b><font color="red">立向时必须注意不能立在卦中的伏神所在的爻<see cref="YaoTypes"/>属性，注意检视：<see cref="PlaceYaosJFNaJiaType"/>类型和<see cref="PlaceYaosJFNaJiaType.HideRelative"/>伏神所在之爻应该避开</font></b>
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
        public List<GuaClass>? GuaList { get; private set; } = null;

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


        /// <summary>
        /// 获取向卦立的爻数信息数组（使用三元盘卦构建）。
        /// </summary>
        /// <value>
        /// 存储该卦所临爻位的京房纳甲与六亲信息。该属性返回值遵循以下规则：
        /// <list type="bullet">
        /// <item><description>支持返回 <see langword="null"/>，表示当前处于“无临爻”状态。</description></item>
        /// <item><description>数组长度限制为 <c>0</c> 到 <c>2</c>，即最多只会存储 2 个临爻的序号及数据(只能相邻两个爻）。</description></item>
        /// <item><description>数组元素中完整包含了对应爻位的六亲属性与纳甲干支。</description></item>
        /// </list>
        /// </value>
        /// <remarks>
        /// <para><b>⚠️ 核心使用条件：</b></para>
        /// <para>本属性默认可能无值。在使用前，必须先调用 <see cref="TianJiGua(CGuaClass)"/> 方法对当前实例进行构建，随后该属性才会被赋予相应的计算值。</para>
        /// <para>该属性基于三元盘卦体系构建，关联对象请参见 <see cref="CGuaClass"/>。</para>
        /// </remarks>
        /// <seealso cref="PlaceYaosJFNaJiaType"/>
        /// <seealso cref="CGuaClass"/>
        public PlaceYaosJFNaJiaType[] PlaceYaoTypes =>
      PlaceYaos?.Select(i => YaoTypes[i]).ToArray() ?? Array.Empty<PlaceYaosJFNaJiaType>();

        /// <summary>
        /// 保存临爻
        /// </summary>
        private int[] PlaceYaos { get; set; }

        /// <summary>
        /// 获取从普通卦中读取的所有京房纳甲爻类型完整信息数组。
        /// </summary>
        /// <value>
        /// 包含该卦所有爻位完整排盘数据的 <see cref="PlaceYaosJFNaJiaType"/> 结构体数组。
        /// <para>与仅包含向卦临爻（最多2个）的 <see cref="PlaceYaoTypes"/> 不同，本属性通常用于存储和获取该卦完整的六个爻位（从初爻到上爻）的纳甲、六亲及伏神基础数据。</para>
        /// </value>        
        /// <seealso cref="PlaceYaosJFNaJiaType"/>
        /// <seealso cref="PlaceYaoTypes"/>
        public PlaceYaosJFNaJiaType[] YaoTypes { get; private set; }


        #endregion


        #region 构造函数
        /// <summary>
        /// 加载三元罗盘上的 64 卦中的向卦类实例。
        /// </summary>
        /// <param name="ToGua">
        /// 传入的向卦对象实例（<see cref="CGuaClass"/>）。
        /// <para>内部将基于该对象的临爻位置集合（<c>PlaceYaos</c>）进行动态数据提取与纳甲绑定。</para>
        /// </param>
        /// <remarks>
        /// <para><b>🔄 易学数据加载与投影流程：</b></para>
        /// <list type="number">
        /// <item>
        /// <description>
        /// <b>状态激活：</b>依次调用 <c>LoadHideRelative()</c> 与 <c>LoadSixRelative()</c> 方法，确保向卦实例的伏神和六亲底层数据被完整加载至内存。
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <b>数理组装：</b>通过 LINQ 表达式遍历临爻索引集合，按爻位从向卦中精准提取对应的纳甲干支（<c>SkyLocs</c>）、六亲（<c>SixRelative</c>）以及伏神数据。
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <b>结果交付：</b>最终将提取的数据转换为全新的 <see cref="PlaceYaosJFNaJiaType"/> 结构体数组，并赋值给当前实例的 <see cref="PlaceYaoTypes"/> 属性。
        /// </description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <seealso cref="CGuaClass"/>
        /// <seealso cref="PlaceYaosJFNaJiaType"/>
        /// <seealso cref="PlaceYaoTypes"/>
        public TianJiGua(CGuaClass ToGua) : this(ToGua as GuaClass)
        {




            PlaceYaos = ToGua.PlaceYaos;




        }





        /// <summary>
        /// 天机出卦法类的构造函数。
        /// </summary>
        /// <remarks>
        /// 本构造函数专门用于风水理气中的“收山出煞”判定。
        /// 通过推导当前向卦所属卦宫的飞爻状态，划分出哪些后天三爻卦属于理气相合的“入卦（吉）”，哪些属于犯煞的“出卦（凶）”。
        /// <b>注意：</b> 本构造函数不用于判断个人本命卦是否出卦。若需推演个人命卦与住宅的出入卦吉凶，请参阅 <see cref="FateGua"/>。
        /// </remarks>
        /// <param name="ToGua">罗盘24山向对应的六爻向卦实例。作为理气推演的基准立向，不可为空。</param>
        /// <exception cref="ArgumentNullException">当传入的向卦参数 <paramref name="ToGua"/> 为 <see langword="null"/> 时抛出。</exception>
        public TianJiGua(GuaClass ToGua)
        {
            // -----------------------------------------------------------------
            // 1. 参数合法性校验
            // -----------------------------------------------------------------
            if (ToGua == null)
                throw new ArgumentNullException(nameof(ToGua), "入参向卦不能为null");

            ToGua.LoadAllYaos();




            //=====================设置六亲、干枝、伏神===================
            int[] Yaos = [0, 1, 2, 3, 4, 5];
            YaoTypes = Yaos.Select(i =>
            {

                PlaceYaosJFNaJiaType pyt = new PlaceYaosJFNaJiaType();
                pyt.PlaceYao = i;
                pyt.SkyLoc = ToGua.Yaos[i].SkyLoc;
                pyt.SixRelative = ToGua.Yaos[i].SixRelative;
                var lsHRY = ToGua.Yaos.Where(x => x.HideRelative != null).ToList();
                if (lsHRY.Any())//无伏神不用处理
                {
                    for (int j = 0; j < ToGua.Yaos.Count(); j++)
                    {
                        if (lsHRY[j].HideRelative != null)
                        {
                            pyt.HideRelative = lsHRY[j].HideRelative;
                            break;
                        }

                    }
                }

                return pyt;

            }).ToArray();
            //=====================设置六亲、干枝、伏神===================


            // -----------------------------------------------------------------
            // 2. 提取后天八卦卦宫基准
            // -----------------------------------------------------------------
            // 根据当前向卦所归属的卦宫名称（如乾宫、坎宫等），获取该卦宫的完整元旦盘/基准卦实例
            GuaClass GuaSelf = new GuaClass(ToGua.GuaSelf.Name); // 取卦宫

            // 以当前大卦宫为本位，通过特定的飞爻规律（如一世卦至归魂卦），演化并列出与其同气连枝的 7 个六爻飞爻卦
            GuaList = GuaSelf.Get7HereYaoGua(); // 以卦宫来列出飞爻卦

            // 声明临时字典，用于对演化出来的三爻单卦（纯卦）进行去重和收集
            Dictionary<string, GuaSubClass> gscIns = new Dictionary<string, GuaSubClass>();

            // =================================================================
            // 3. 获得入卦（三爻卦）后天 —— 即理气相合、收山入煞的吉祥方位卦
            // =================================================================
            // 遍历由卦宫飞爻演化出的所有大卦，将其拆解为上卦（外卦）和下卦（内卦）两个后天三爻单卦
            foreach (GuaClass gc in GuaList)
            {
                // 若字典中尚未包含该飞爻大卦的“下卦”，则将其归纳为“入卦”范围
                if (gscIns.ContainsKey(gc.DownGua.Name) == false)
                {
                    gscIns.Add(gc.DownGua.Name, gc.DownGua);
                }
                // 若字典中尚未包含该飞爻大卦的“上卦”，同样将其归纳为“入卦”范围
                if (gscIns.ContainsKey(gc.UpGua.Name) == false)
                {
                    gscIns.Add(gc.UpGua.Name, gc.UpGua);
                }
            }
            // 最终将收集去重后的字典，赋值给类成员属性，确立当前向水理气下的“入卦”范围
            this.InGuaSubs = gscIns; // 入卦（三爻卦）后天

            // =================================================================
            // 4. 获得出卦（三爻卦）后天 —— 即理气不合、犯煞出卦的凶险方位卦
            // =================================================================
            Dictionary<string, GuaSubClass> gscOuts = new Dictionary<string, GuaSubClass>();

            // 遍历后天八卦所有的标准卦名（乾、坤、震、巽、坎、离、艮、兑）
            foreach (string sN in GuaSubClass.BeforeGuaSubNames)
            {
                // 差集比对：如果某个标准卦名【不属于】上面推导出的“入卦”范围，且【未被】记录到出卦字典中
                if (gscIns.ContainsKey(sN) == false && gscOuts.ContainsKey(sN) == false)
                {
                    // 则证明该卦位理气不合，属于“出煞/出卦”的范畴，将其加载并存入出卦字典中
                    gscOuts.Add(sN, GuaSubClass.GetGuaSub(sN, false));
                }
            }
            // 将求差集后得到的出卦单卦集合，赋值给类成员属性，确立当前立向下的“出卦”煞位
            this.OutGuaSubs = gscOuts; // 出卦（三爻卦）后天
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
            return C3YEx.CBeforeGuas
                .Where(kv => IsOutGua(kv.Value))
                .ToDictionary(kv => kv.Key, kv => kv.Value);
        }


        #endregion
    }

}
