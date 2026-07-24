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
    /// 勘测系统中的九星模型分类（定义类别实例化时采用的星曜数理体系）。
    /// 紫白九星、翻卦九星。
    /// </summary>
    /// <remarks>
    /// <para>本枚举用于区分基础易学方位星曜与进阶翻卦理气星曜的数理边界：</para>
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       <strong><see cref="AfterGuaNineStar"/> (后天八卦九星)：</strong> 严格对应后天八卦八方宫位以及中央五黄廉贞星（共九星）。其星曜名称与峦头山星一致，但底层五行生克属性存在本质差异。
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       <strong><see cref="FlipGuaNineStar"/> (翻卦九星)：</strong> 依据黄石公翻卦掌或辅星水法变爻推导而出的理气星曜。在底层运算中，将左辅星与右弼星合并为“辅星”进行归化，本质上为八星序列。
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    public enum NineStarType
    {
        /// <summary>
        /// 后天八卦九星（紫白九星）类型（包含五黄中宫）。
        /// </summary>
        AfterGuaNineStar = 0,

        /// <summary>
        /// 翻卦九星类型（实质为合辅弼之后的游年八星）。
        /// </summary>
        FlipGuaNineStar = 1
    }
    /// <summary>
    /// 周易数理与风水理气核心：九星（游年八星/紫白九星）的数据抽象模型与静态工厂类。
    /// </summary>
    /// <remarks>
    /// 本类作為平台级别的「材料供应商」，保持极致的架构克制。内部仅提供绝对客观的星曜名称、类型、吉凶逻辑判定以及
    /// 游年映射，<b>不包含任何带有主观色彩的文字评语或顏色渲染样式控制</b>，完美支援外部 C# 主程序调度与第三方动态 JavaScript 插件扩充。
    /// </remarks>
    public class NineStar
    {
        /// <summary>
        /// 后天八卦九星（紫白九星）的完整中文名称常量序列。
        /// </summary>
        /// <value>
        /// 包含 9 个元素的字符串数组，顺序严格对应紫白九星：贪狼、巨门、禄存、文曲、廉贞、武曲、破军、左辅、右弼。
        /// </value>
        /// <remarks>
        /// 本数组主要用于需要全称显化输出的场景（如风水综合报告、高精度罗盘盘面文字渲染）。
        /// </remarks>
        /// <seealso cref="NineStarNames"/>
        public static readonly string[] NineStarFullNames = ["贪狼", "巨门", "禄存", "文曲", "廉贞", "武曲", "破军", "左辅", "右弼"];

        /// <summary>
        /// 后天八卦九星（紫白九星）的单字简称常量序列。
        /// </summary>
        /// <value>
        /// 包含 9 个元素的字符串数组，顺序与 <see cref="NineStarFullNames"/> 完全一致：贪、巨、禄、文、廉、武、破、辅、弼。
        /// </value>
        /// <remarks>
        /// 主要用于排版空间受限、UI 宫格紧凑型界面显示或调试终端的日志输出。
        /// </remarks>
        /// <seealso cref="NineStarFullNames"/>
        public static readonly string[] NineStarNames = ["贪", "巨", "禄", "文", "廉", "武", "破", "辅", "弼"];

        /// <summary>
        /// 翻卦理气核心：游年九星（实质为八星）的完整名称常量序列。
        /// </summary>
        /// <value>
        /// 默认遵循山法/龙法正序排列：辅弼、贪狼、巨门、禄存、文曲、廉贞、武曲、破军。（水法通过外部映射数组进行变换）。
        /// </value>
        public static readonly string[] EightStarFullNames = ["辅弼", "贪狼", "巨门", "禄存", "文曲", "廉贞", "武曲", "破军"];

        /// <summary>
        /// 翻卦理气核心：游年九星（实质为八星）的单字简称常量序列。
        /// </summary>
        /// <value>
        /// 默认遵循山法/龙法正序排列：辅、贪、巨、禄、文、廉、武、破。（水法通过外部映射数组进行变换）。
        /// </value>
        public static readonly string[] EightStarNames = ["辅", "贪", "巨", "禄", "文", "廉", "武", "破"];

        /// <summary>
        /// 翻卦星曜单字简称与八宅游年标准名称的静态常量对照字典。
        /// </summary>
        /// <value>
        /// 包含 8 组映射关系，实现从星曜名（Key）到游年名（Value）的高效 O(1) 数字化转换。
        /// </value>
        public static readonly Dictionary<string, string> EightHouseStarNameDC = new Dictionary<string, string>
    {
        { "辅", "伏位" }, { "贪", "生气" }, { "巨", "天医" }, { "禄", "祸害" },
        { "文", "六煞" }, { "廉", "五鬼" }, { "武", "延年" }, { "破", "绝命" }
    };

        /// <summary>
        /// 获取当前星曜实例的数理分类类型 (<see cref="NineStarType"/>)。
        /// </summary>
        public NineStarType Type { get; private set; }

        /// <summary>
        /// 核心数理逻辑判定：基于传统的“四吉四凶”原则，快速验证当前星曜是否属于“四吉星”（辅、贪、巨、武）。
        /// </summary>
        /// <value>
        /// 若属于伏位、生气、天医、延年对应的星曜则返回 <c>true</c>；若属于祸害、六煞、五鬼、绝命对应的星曜则返回 <c>false</c>。
        /// </value>
        /// <remarks>
        /// 本属性采用高效的字串包含式（<c>IndexOf</c>）底层无感算法，规避了繁琐的逻辑分支判定，专门为高并发排盘渲染和外部 JavaScript 沙箱过滤提供纯净、无偏见的布尔开关（IsGood）。
        /// </remarks>
        public bool IsGood { get { return "辅贪巨武".IndexOf(Name ?? string.Empty) > -1; } }

        /// <summary>
        /// 获取当前星曜对应的八宅游年标准名称（如：伏位、生气）。
        /// </summary>
        /// <value>
        /// 当星曜类型为 <see cref="NineStarType.AfterGuaNineStar"/> 时，该值默认为空字符串 <c>""</c>。
        /// </value>
        public string? EightHouseStarName { get; private set; } = "";

        /// <summary>
        /// 获取当前星曜的单字简称（如：“贪”、“巨”）。
        /// </summary>
        /// <remarks>
        /// 该属性对外提供全局只读（get）权限，写权限（set）锁死在类内部与工厂方法中，确保核心盘面数据不被外部代码任意篡改。
        /// </remarks>
        public string? Name { private set; get; }

        /// <summary>
        /// 获取当前星曜的完整中文名称（如：“贪狼”、“巨门”）。
        /// </summary>
        /// <remarks>
        /// 经过严谨重构，已彻底修正因 Getter/Setter 权限写反导致的 JSON 序列化蒸发 BUG，可完美、完整地导出至外部接口。
        /// </remarks>
        public string? FullName { private set; get; }

        /// <summary>
        /// 静态工厂方法：根据传入的后天八卦基础单卦对象，快速构建并返回其对应的后天八卦九星实例。
        /// </summary>
        /// <param name="gs">后天八卦单卦实例对象 (<see cref="GuaSubClass"/>)。</param>
        /// <returns>返回包含后天八卦九星元数据信息的 <see cref="NineStar"/> 独立对象。</returns>
        /// <exception cref="ArgumentNullException">当传入的单卦对象为 null 时抛出。</exception>
        public static NineStar GetNineStarByAfterGua(GuaSubClass gs)
        {
            if (gs == null) throw new ArgumentNullException(nameof(gs));

            NineStar ns = new();
            ns.Type = NineStarType.AfterGuaNineStar;
            ns.Name = NineStarNames[gs.AfterGuaSubIndex];
            // 🚀 顺手帮您微调对齐了之前的拷贝手误：FullName 应当精准映射到九星 FullNames 数组中
            ns.FullName = NineStarFullNames[gs.AfterGuaSubIndex];

            return ns;
        }

        /// <summary>
        /// 静态工厂方法：根据翻卦掌诀产生的步进数字化索引，快速构建并返回对应的翻卦九星（实际为游年八星）实例。
        /// </summary>
        /// <param name="Index">基于翻卦轨迹数组转换后的星曜逻辑索引（有效范围：0 到 7）。</param>
        /// <returns>返回包含完整翻卦配星与八宅游年映射信息的 <see cref="NineStar"/> 独立对象。</returns>
        /// <exception cref="ArgumentOutOfRangeException">当传入的索引超出 0-7 的数组边界时抛出。</exception>
        public static NineStar GetFlipGuaEightStar(int Index)
        {
            if (Index < 0 || Index >= EightStarNames.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(Index), $"[数理越界] 翻卦配星索引必须位于 0 至 {EightStarNames.Length - 1} 之间。");
            }

            NineStar ns = new();
            ns.Type = NineStarType.FlipGuaNineStar;      // 锁定类型为翻卦九星
            ns.Name = EightStarNames[Index];           // 精准映射单字简称（如：“辅”）
            ns.FullName = EightStarFullNames[Index];   // 精准映射完整全称（如：“辅弼”）

            // 依靠静态只读字典，秒级推导出对应的八宅游年星曜名称（如：“伏位”）
            ns.EightHouseStarName = EightHouseStarNameDC[ns.Name];

            return ns;
        }
    }
}

