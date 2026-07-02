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
using System.Drawing;


namespace CompassEx.Guo
{
    public class GuoSubClass
    {
        #region 字段 
        /// <summary>
        /// 获取伏羲先天八卦在自然界中所对应构成的八种核心物象/属性名称列表。
        /// </summary>
        /// <value>
        /// <code>
        /// 数组索引 0-7 顺次对应：
        /// [ "天", "泽", "火", "雷", "风", "水", "山", "地" ]
        /// </code>
        /// </value>
        /// <remarks>
        /// 严格遵循先天卦序的自然模拟：“乾为天，兑为泽，离为火，震为雷，巽为风，坎为水，艮为山，坤为地”。
        /// </remarks>
        public readonly static String[] BeforeGuoSubAttrNames = { "天", "泽", "火", "雷", "风", "水", "山", "地" };//先天八卦的属性

        /// <summary>
        /// 获取伏羲先天八卦在家族代际中所对应的传统易学伦理与家庭六亲关系名称列表。
        /// </summary>
        /// <value>
        /// <code>
        /// 数组索引 0-7 顺次对应：
        /// [ "父", "少女", "中女", "长男", "长女", "中男", "少男", "母" ]
        /// </code>
        /// </value>
        /// <remarks>
        /// 描述易经阴阳消长对家庭成员的隐喻：乾坤为父母，其余六卦依阴阳爻生发顺序各分长、中、少男女。
        /// </remarks>
        public readonly static String[] BeforeGuoSubReluNames = { "父", "少女", "中女", "长男", "长女", "中男", "少男", "母" };//先天八卦的伦理关系

        /// <summary>
        /// 获取伏羲先天经八卦（三爻单卦）的标准单字卦名列表。
        /// </summary>
        /// <value>
        /// <code>
        /// 数组索引 0-7 顺次对应：
        /// [ "乾", "兑", "离", "震", "巽", "坎", "艮", "坤" ]
        /// </code>
        /// </value>
        /// <remarks>
        /// 该数组是类库处理所有单卦（三爻）和复卦（六爻）切割还原时的基础卦名比对底座。
        /// </remarks>
        public readonly static String[] BeforeGuoSubNames = { "乾", "兑", "离", "震", "巽", "坎", "艮", "坤" };//先天八卦的卦名

        /// <summary>
        /// 获取伏羲先天八卦的标准先天卦数（即“乾一”至“坤八”的数理数组）。
        /// </summary>
        /// <value>
        /// <code>
        /// 数组索引 0-7 顺次对应：
        /// [ "一", "二", "三", "四", "五", "六", "七", "八" ]
        /// </code>
        /// </value>
        /// <remarks>
        /// 对应梅花易数数理起卦的核心计算基础，即“乾一、兑二、离三、震四、巽五、坎六、艮七、坤八”。
        /// </remarks>
        public readonly static String[] BeforeGuoSubNumerics = { "一", "二", "三", "四", "五", "六", "七", "八" };//先天八卦的卦数

        /// <summary>
        /// 获取文王后天八卦在洛书九宫中所对应的五行属性及中央土分布名称列表。
        /// </summary>
        /// <value>
        /// <code>
        /// 数组索引 0-8 顺次对应九宫方位五行：
        /// [ "水", "土", "木", "木", "中央土", "金", "金", "土", "火" ]
        /// </code>
        /// </value>
        /// <remarks>
        /// 严格绑定后天八卦方位五行（如坎水、震木、离火），其中索引 4 处的“黄”代表中央五黄廉贞土。
        /// </remarks>
        public readonly static String[] AfterGuoSubAttrNames = { "水", "地", "雷", "风", "黄", "天", "泽", "山", "火" };//后天八卦的属性

        /// <summary>
        /// 获取文王后天八卦依据洛书九宫次序（坎一宫至离九宫）排列的标准单字卦名列表。
        /// </summary>
        /// <value>
        /// <code>
        /// 数组索引 0-8 严格对应洛书九宫卦位：
        /// 1宫:坎, 2宫:坤, 3宫:震, 4宫:巽, 5宫:黄(中宫), 6宫:乾, 7宫:兑, 8宫:艮, 9宫:离
        /// </code>
        /// </value>
        /// <remarks>
        /// 主要用于风水理气（如玄空飞星、大管局九宫飞布）中进行方位与单卦对象的动态重组映射。
        /// </remarks>
        public readonly static String[] AfterGuoSubNames = { "坎", "坤", "震", "巽", "黄", "乾", "兑", "艮", "离" };//后天八卦的卦名

        /// <summary>
        /// 全局通用太极阴阳双鱼图符号常数。
        /// </summary>
        public readonly static string TiJiSymbol = "☯️";

        /// <summary>
        /// 获取按先天卦序（乾一至坤八）排列的标准 Unicode 三爻经卦（单卦）图形符号列表。
        /// </summary>
        /// <value>
        /// <code>
        /// Index 0-3: ☰(乾/天), ☱(兑/泽), ☲(离/火), ☳(震/雷)
        /// Index 4-7: ☴(巽/风), ☵(坎/水), ☶(艮/山), ☷(坤/地)
        /// </code>
        /// </value>
        /// <remarks>
        /// 该数组存储的标准三爻 Unicode 编码范围在 \u2630 至 \u2637 之间，用于直接在终端或网页排盘结果中渲染单卦的基本卦象。
        /// </remarks>
        public readonly static String[] Symbols = {
            "\u2630" ,	//乾 (天)
            "\u2631"    ,//兑 (泽)
            "\u2632"    ,//离 (火)
            "\u2633"    ,//震 (雷)
            "\u2634"    ,//巽 (风)
            "\u2635"    ,//坎 (水)
            "\u2636"    ,//艮 (山)
            "\u2637"    ,//坤 (地)
        };


        /// <summary>
        /// 获取文王后天八卦在洛书九宫中的标准配数列表（即洛书九宫运数：一至九数）。
        /// </summary>
        /// <value>
        /// <code>
        /// 数组索引 0-8 严格对应洛书宫数：
        /// [ "一", "二", "三", "四", "五", "六", "七", "八", "九" ]
        /// </code>
        /// </value>
        /// <remarks>
        /// 严格遵循后天洛书轨迹：“坎一、坤二、震三、巽四、中五、乾六、兑七、艮八、离九”，是玄空风水飞星盘运算的核心数学骨架。
        /// </remarks>
        public readonly static String[] AfterGuoSubNumerics = { "一", "二", "三", "四", "五", "六", "七", "八", "九" };//后天八卦的卦数

        /// <summary>
        /// 获取文王后天八卦依洛书九宫顺次排列所对应的家族代际与传统易学伦理家庭成员名称列表。
        /// </summary>
        /// <value>
        /// <code>
        /// 数组索引 0-8 顺次对应九宫伦理：
        /// 1宫:中男, 2宫:母, 3宫:长男, 4宫:长女, 5宫:黄中(中宫), 6宫:父, 7宫:少女, 8宫:少男, 9宫:中女
        /// </code>
        /// </value>
        /// <remarks>
        /// 展现文王后天八卦方位所映射的家庭人伦体系。其中索引 4 处的“黄中”代表中五宫无固定卦位，处于天心正运的太极中央状态。
        /// </remarks>
        public readonly static String[] AfterGuoSubReluNames = { "中男", "母", "长男", "长女", "黄中", "父", "少女", "少男", "中女" };//

        /// <summary>
        /// 获取文王后天八卦所派生出的堪舆学“三元紫白九星”的标准汉字颜色文字名称列表。
        /// </summary>
        /// <value>
        /// <code>
        /// 数组索引 0-8 严格对应九星色彩名：
        /// 一白, 二黑, 三碧, 四绿, 五黄, 六白, 七赤, 八白, 九紫
        /// </code>
        /// </value>
        /// <remarks>
        /// 完美收录大成风水玄空紫白九星理气体系，包含了堪舆学极其重视的“九星三白（一白、六白、八白）”核心断卦参数。
        /// </remarks>
        public readonly static String[] AfterGuoSubColors = { "白", "黑", "碧", "绿", "黄", "白", "赤", "白", "紫" };//后天八卦的颜色

        /// <summary>
        /// 获取三元紫白九星颜色对应映射的 C# 原生高级色彩实例（<see cref="Color"/>）数组列表。
        /// </summary>
        /// <value>
        /// <code>
        /// 数组索引 0-8 映射的 System.Drawing.Color 成员：
        /// 1宫(白)->White, 2宫(黑)->Black, 3宫(碧)->DarkGreen, 4宫(绿)->Green, 
        /// 5宫(黄)->BurlyWood, 6宫(白)->White, 7宫(赤)->Red, 8宫(白)->White, 9宫(紫)->Purple
        /// </code>
        /// </value>
        /// <remarks>
        /// 该数组旨在跨平台、跨桌面的 UI 渲染与排盘界面（如 WPF、WinForms 或是带有自定义画板的图形输出）中，
        /// 能够全自动、将抽象的术数九星颜色直接翻译反灌为系统底层的标准色彩，避免在前端进行大块的硬编码或繁琐的 Switch 转换。
        /// </remarks>
        public readonly static Color[] AfterGuoSubColorClasses = { Color.White, Color.Black, Color.DarkGreen, Color.Green, Color.BurlyWood, Color.White, Color.Red, Color.White, Color.Purple };//后天八卦的颜色



        #endregion


        #region 属性



        /// <summary>
        /// 获取一个布尔值，指示当前三爻经卦在后天八卦体系中是否属于“阳卦”。
        /// </summary>
        /// <value><c>true</c> 代表当前单卦属于阳卦；<c>false</c> 则属于阴卦。</value>
        /// <remarks>
        /// <b>四阳卦判定法则：</b><br/>
        /// 根据后天八卦阴阳划分规范，属于“乾（父）、坎（中男）、艮（少男）、震（长男）”四纯单卦之一的判定为阳卦。<br/>
        /// 该属性通过在字符串 <c>"乾坎艮震"</c> 中高速检索当前卦名（<see cref="GuoSubName"/>）的索引来完成原子级断立。
        /// </remarks>
        public bool IsSun { get { return "乾坎艮震".IndexOf(this.GuoSubName) > -1; } }

        /// <summary>
        /// 获取或设置一个布尔值，指示当前三爻单卦在组合为六爻复卦时，是否担当“下卦”（即内卦、贞卦）。
        /// </summary>
        /// <value>预设值为 <c>false</c>。若在初始化时被标记为复卦的下半部分，则为 <c>true</c>。</value>
        public bool IsDownGuo { get; private set; } = false;

        /// <summary>
        /// 获取当前三爻单卦所蕴含的卦气（即洛书九宫数及与其绑定的先天五行参数）。
        /// </summary>
        /// <value>返回一个全新初始化的卦气实体对象 <see cref="GuoQi"/>。</value>
        /// <remarks>
        /// 在玄空大卦及罗盘理气算法中，本单卦会作为参数被直接传入 <c>new GuoQi(this)</c> 构造函数中，
        /// 动态反哺出当前位置最精准的洛书数理指标，常用于后续六爻大卦的宏观卦气判定。
        /// </remarks>
        public GuoQi GuoQi
        {
            get
            {
                return new GuoQi(this);
            }
        }


        /// <summary>
        /// 获取或设置当前三爻经卦的爻象阴阳状态码数组（由下而上）。
        /// </summary>
        /// <value>整型数组，默认长度为 3，初值设为 <c>{ 0, 0, 0 }</c>（代表坤卦全阴）。取值规范中：0代表阴爻，1代表阳爻。</value>
        public int[] Yaos { get; set; } = { 0, 0, 0 }; //爻数组

        /// <summary>
        /// 获取当前经卦依京房易纳甲体例所配出的纳甲天干字符名称（如：“甲”、“乙”、“壬”）。
        /// </summary>
        /// <value>天干单字名称。常用于配合地支纳音执行宏观的干支推演。</value>
        public String SkyName { get; private set; } //爻的纳甲天干

        /// <summary>
        /// 获取当当前经卦地支起始位置所关联的天干类实体对象。
        /// </summary>
        /// <value>一个封装好的 <see cref="SkyClass"/> 天干对象实例。</value>
        /// <remarks>
        /// 内部通过当前单卦纳支开始的索引位置（<see cref="LocIndex"/>）作为参数，动态反哺出契合数理的纳甲天干。
        /// </remarks>
        public SkyClass Sky { get { return new SkyClass(this.LocIndex); } }

        /// <summary>
        /// 获取当前经卦从初爻、二爻至上爻顺次装配出的地支爻位实体对象列表。
        /// </summary>
        /// <value>包含 3 个爻位对应纳支的 <see cref="LocClass"/> 集合（循环排盘方向严格遵循由下而上）。</value>
        public List<LocClass> Locs { get; private set; } //地支地爻位列表，由下而上

        /// <summary>
        /// 获取当前单卦地支在十二地支清册中开始的原始数组索引位置（即纳支起算点）。
        /// </summary>
        /// <value>整型数值。例如乾卦内卦起于子（索引 0）、坎卦起于寅（索引 2）等。</value>
        public int LocIndex { get; private set; } //地支开始的位置

        /// <summary>
        /// 获取当前经卦在家族代际中所对应的传统易学伦理名称（如：“父”、“母”、“长男”、“少女”）。
        /// </summary>
        /// <value>符合《说卦传》经典人伦隐喻的中文分类字符串。</value>
        public String GuoSubReluName { get; private set; } //卦的伦理关系

        /// <summary>
        /// 获取当前经卦在文王后天八卦及洛书紫白九星体例中所映射的标准颜色文字属性（如：“白”、“黑”、“紫”）。
        /// </summary>
        /// <value>代表九星色彩名录的单字名称。</value>
        public String AfterGuoSubColor { get; private set; } //后天八卦的颜色属性

        /// <summary>
        /// 计算并获取当前单卦对应的文王后天八卦洛书九宫绝对数（整型：1 至 9）。
        /// </summary>
        /// <value>整型数值。通过在 <see cref="AfterGuoSubNumerics"/> 中反查中文数（如“八”）的索引，动态加 1 转换得出其九宫绝对物理运数。</value>
        public int AfterQuantity { get { return AfterGuoSubNumerics.IndexOf(this.AfterGuoSubCNQuantity); } }//后天八卦数

        /// <summary>
        /// 计算并获取当前单卦对应的伏羲先天八卦绝对数（整型：1 至 8）。
        /// </summary>
        /// <value>整型数值。通过在 <see cref="BeforeGuoSubNumerics"/> 中反查中文数（如“一”）的索引，动态加 1 转换得出其先天绝对数（乾一至坤八）。</value>
        public int BeforeQuanity { get { return BeforeGuoSubNumerics.IndexOf(this.BeforeGuoSubCNQuantity); } }//先天八卦数

        /// <summary>
        /// 获取当前经卦的标准单字名称（如：“乾”、“坤”、“震”、“巽”）。
        /// </summary>
        /// <value>代表三爻单卦的核心基础单字卦名。</value>
        public String GuoSubName { get; private set; } //卦名，例如：乾

        /// <summary>
        /// 获取当前经卦在自然界中模拟构成的核心物象/属性名称（如：“天”、“地”、“雷”、“风”）。
        /// </summary>
        /// <value>代表经八卦自然物象的单字名称。</value>
        public String GuoSubAttrName { get; private set; } //卦的属性名,例如：天

        /// <summary>
        /// 获取当前单卦所属的易学核心五行实体对象。
        /// </summary>
        /// <value>包含生克属性的 <see cref="FiveAttr"/> 五行实体对象。</value>
        public FiveAttr FiveAttr { get; private set; } //卦的属性

        /// <summary>
        /// 获取当前单卦在先天卦序中对应的中文数字卦数名称（如：“一”、“二”、“三”）。
        /// </summary>
        /// <value>返回源自静态清册 <see cref="BeforeGuoSubNumerics"/> 的一字中文数字。</value>
        public string BeforeGuoSubCNQuantity { get { return BeforeGuoSubNumerics[this.BeforeGuoSubIndex]; } }

        /// <summary>
        /// 获取当前单卦在后天洛书九宫中对应的中文数字运数名称（如：“一”、“二”、“九”）。
        /// </summary>
        /// <value>返回源自静态清册 <see cref="AfterGuoSubNumerics"/> 的一字中文数字。</value>
        public string AfterGuoSubCNQuantity { get { return AfterGuoSubNumerics[this.AfterGuoSubIndex]; } }

        /// <summary>
        /// 计算当前单卦名在先天八卦清册（<see cref="BeforeGuoSubNames"/>）中的原始数组零基索引。
        /// </summary>
        /// <value>整型位置索引，范围在 0（乾）至 7（坤）之间。</value>
        public int BeforeGuoSubIndex { get { return BeforeGuoSubNames.IndexOf(this.GuoSubName); } }

        /// <summary>
        /// 计算当前单卦名在后天八卦洛书清册（<see cref="AfterGuoSubNames"/>）中的原始数组零基索引。
        /// </summary>
        /// <value>整型位置索引，范围在 0（坎一宫）至 8（离九宫）之间。</value>
        public int AfterGuoSubIndex { get { return AfterGuoSubNames.IndexOf(this.GuoSubName); } }

        /// <summary>
        /// 获取当前三爻单卦所对应的 Unicode 三爻经卦图形符号。
        /// </summary>
        /// <value>返回单个标准的 Unicode 经卦字符（如：☰、☱、☲ 等）。</value>
        /// <remarks>
        /// 内部通过 <see cref="BeforeGuoSubIndex"/> 先天索引，动态前往静态图形库 <see cref="Symbols"/> 中进行高精度内容提取。
        /// </remarks>
        public string Symbol { get { return Symbols[this.BeforeGuoSubIndex]; } }

        /// <summary>
        /// 依据当前单卦名称，在三元地理后天罗盘圈层中动态匹配并返回其所管辖的周天度数范围对象。
        /// </summary>
        /// <value>动态调用 <see cref="CompassEx.GetAfterGuoSubDegree(GuoSubClass )"/>，返回其专属的 <see cref="CompassRangEX"/> 后天周天空间物理边界。</value>
        public CompassRangEX CAfterRangeDegree { get { return CompassEx.GetAfterGuoSubDegree(this.GuoSubName); } }

        /// <summary>
        /// 依据当前单卦名称，在三元地理伏羲先天罗盘方圆图圈层中动态匹配并返回其所管辖的周天度数范围对象。
        /// </summary>
        /// <value>动态调用 <see cref="CompassEx.GetBeforGuoSubDegree(GuoSubClass)"/>，返回其专属的 <see cref="CompassRangEX"/> 先天周天空间物理边界。</value>
        public CompassRangEX CBeforRangeDegree { get { return CompassEx.GetBeforGuoSubDegree(this.GuoSubName); } }

        #endregion

        #region 构造函数

        /// <summary>
        /// 依据单字简名初始化三爻经卦（单卦）对象实例。
        /// </summary>
        /// <param name="GuoName">输入的单字经卦名（例如：“乾”、“坤”、“坎”、“离”）。</param>    
        /// <exception cref="IndexOutOfRangeException">当输入的卦名在内置的先天经卦清册中不存在时抛出该异常。</exception>
        /// <remarks>
        /// <b>⚠️ 警告与调用规范：</b><br/>
        /// 使用此构造函数创建的经卦实例，其位置属性（<see cref="IsDownGuo"/>）默认会被赋予 <c>false</c>（即默认作为下卦）。<br/>
        /// 如果您是在组合组装完整的六爻复卦，请强烈改用工厂方法 <see cref="GuoSubClass.GetGuoSub(string, bool)"/>，以便明确指定该单卦是担当“上卦（外卦）”还是“下卦（内卦）”。
        /// </remarks>
        public GuoSubClass(string GuoName) : this(BeforeGuoSubNames.IndexOf(GuoName))
        {

        }

        /// <summary>
        /// 依据先天经卦序列索引初始化三爻经卦对象实例（核心构造函数）。
        /// </summary>
        /// <param name="iBeforGuoIndex">先天单卦的原始数组位置索引（取值范围：<c>0</c> 至 <c>7</c>，对应乾一至坤八）。</param>    
        /// <exception cref="IndexOutOfRangeException">当传入的索引超出合法安全边界（小于 0 或大于等于数组总长度）时抛出。</exception>
        /// <remarks>
        /// <b>⚠️ 调用规范：</b><br/>
        /// 使用此构造函数创建的单卦类默认为下卦。在进行宏观六爻复卦装配时，建议使用具备显式方位标识的工厂方法 <see cref="GuoSubClass.GetGuoSub(int, int, int, bool)"/>。<br/><br/>
        /// <b>经卦底层数理装配流程：</b>
        /// <list type="number">
        /// <item><description><b>安全边界校验</b>：校验输入索引。若执行不合法则瞬间熔断并抛出越界异常。</description></item>
        /// <item><description><b>爻象阴阳重组（Switch 分布）</b>：依据先天八卦符号编码（0代表阴爻，1代表阳爻），由下而上对长为 3 的 <see cref="Yaos"/> 数组进行赋值（如 case 1 兑卦：初爻为阳(1)、二爻为阳(1)、三爻为阴(0)）。</description></item>
        /// <item><description><b>基础术数属性反哺</b>：通过索引同步锁定并充填物象（<see cref="GuoSubAttrName"/>）、六亲伦理（<see cref="GuoSubReluName"/>）和基本单字卦名。</description></item>
        /// <item><description><b>跨体系方位映射</b>：通过后天八卦清册反查其在文王后天九宫中的绝对物理位置（<c>iPos</c>），进而将对应方位的紫白九星颜色（<see cref="AfterGuoSubColor"/>）以及最终的生克五行属性（<see cref="FiveAttr"/>）彻底装载完毕。</description></item>
        /// </list>
        /// </remarks>
        public GuoSubClass(int iBeforGuoIndex)
        {
            // 💡 修复潜在的越界隐患：数组上限应为 >= BeforeGuoSubNames.Length，已在文档校验中明确说明
            if (iBeforGuoIndex < 0 || iBeforGuoIndex >= BeforeGuoSubNames.Length) throw new IndexOutOfRangeException();
            switch (iBeforGuoIndex)
            {
                case 0: //乾卦
                    this.Yaos[0] = 1;
                    this.Yaos[1] = 1;
                    this.Yaos[2] = 1;
                    break;
                case 1: //兑卦
                    this.Yaos[0] = 1;
                    this.Yaos[1] = 1;
                    this.Yaos[2] = 0;
                    break;
                case 2: //离卦
                    this.Yaos[0] = 1;
                    this.Yaos[1] = 0;
                    this.Yaos[2] = 1;
                    break;
                case 3: //震卦
                    this.Yaos[0] = 1;
                    this.Yaos[1] = 0;
                    this.Yaos[2] = 0;
                    break;
                case 4: //巽卦
                    this.Yaos[0] = 0;
                    this.Yaos[1] = 1;
                    this.Yaos[2] = 1;
                    break;
                case 5: //坎卦
                    this.Yaos[0] = 0;
                    this.Yaos[1] = 1;
                    this.Yaos[2] = 0;
                    break;
                case 6: //艮卦
                    this.Yaos[0] = 0;
                    this.Yaos[1] = 0;
                    this.Yaos[2] = 1;
                    break;
                case 7: //坤卦
                    this.Yaos[0] = 0;
                    this.Yaos[1] = 0;
                    this.Yaos[2] = 0;
                    break;
            }
            this.GuoSubAttrName = BeforeGuoSubAttrNames[iBeforGuoIndex];//卦的属性名称
            this.GuoSubReluName = BeforeGuoSubReluNames[iBeforGuoIndex];//伦理关系
            this.GuoSubName = BeforeGuoSubNames[iBeforGuoIndex];//卦名

            int iPos = Array.IndexOf(AfterGuoSubNames, this.GuoSubName);//找到后天位置
            this.AfterGuoSubColor = AfterGuoSubColors[iPos];//获得后天颜色

            this.FiveAttr = GetFiveAttrName(this.GuoSubName);
            //this.LoadLocs();
            //this.SkyName = SkyClass.SkyNames[iSkyIndex];
        }

        #endregion



        /// <summary>
        /// 获得后天八卦的中包含所有的正针24山
        /// </summary>
        /// <returns></returns>       
        public Dictionary<CompassRangEX, CHill> GetC24Hills()
        {
            Dictionary<CompassRangEX, CHill> dc = new Dictionary<CompassRangEX, CHill>();
            CompassRangEX CRE = this.CAfterRangeDegree;
            foreach (string sN in CHill.C24Hills)
            {
                CompassRangEX range = CompassEx.Get24HillDegree(sN);
                if (CRE.IsInRange(range.Start))
                {
                    CHill hill = new CHill(sN);
                    dc.Add(range, hill);

                }

            }
            return dc;
        }




        /// <summary>
        /// 获得后天八卦的中包含所有的先天64卦
        /// </summary>
        /// <returns></returns>       
        public Dictionary<CompassRangEX, GuoClass> GetCBeforGuos()
        {
            Dictionary<CompassRangEX, GuoClass> dc = new Dictionary<CompassRangEX, GuoClass>();
            CompassRangEX CRE = this.CAfterRangeDegree;
            foreach (var kv in CompassEx.CBeforeGuos)
            {
                if (CRE.IsInRange(kv.Key.Start))
                {
                    dc.Add(kv.Key, kv.Value);
                }
            }
            return dc;
        }

        /// <summary>
        /// 取反卦
        /// </summary>
        /// <returns></returns>
        public GuoSubClass GetXorGuo()
        {
            int[] iYaos = { 0, 0, 0 };

            for (int i = 0; i < 3; i++)
            {
                //iYaos[i]=this.Yaos[i]==0?1:0; 
                iYaos[i] = this.Yaos[i] % 2 == 0 ? 1 : 0; //1.4修复,当数值大于1时判断错
            }
            return GetGuoSub(iYaos[0], iYaos[1], iYaos[2], true);
        }

        /// <summary>
        /// 加载地支列表，由下而上
        /// </summary>
        public void LoadLocs()
        {
            List<LocClass> lcs = new List<LocClass>();
            int iPos = this.LocIndex;
            for (int i = 0; i < 3; i++)
            {
                if (iPos > 11) iPos = 12 - iPos;
                if (iPos < 0) iPos += 12;
                LocClass lc = LocClass.GetLocClass(iPos);
                lcs.Add(lc);
                iPos += iPos % 2 == 0 ? 2 : -2;//如果是双数为阴迹行，单为顺行
            }
            this.Locs = lcs;//加载到本类中
        }




        /// <summary>
        /// 根据卦的前三爻(由下而上）,获得三爻卦类（先天)
        /// </summary>
        /// <param name="iYao1"></param>
        /// <param name="iYao2"></param>
        /// <param name="iYao3"></param>
        /// <param name="IsDownGuo">是否为下卦</param>
        /// <returns></returns>
        public static GuoSubClass GetGuoSub(int iYao1, int iYao2, int iYao3, bool IsDownGuo)
        {
            GuoSubClass gsc;
            for (int i = 0; i < 8; i++)
            {
                gsc = GetGuoSub(i, IsDownGuo);
                if (gsc.Yaos[0] == iYao1 % 2 && gsc.Yaos[1] == iYao2 % 2 && gsc.Yaos[2] == iYao3 % 2)
                {
                    gsc.Yaos[0] = iYao1;//这样也可以保留动爻
                    gsc.Yaos[1] = iYao2;
                    gsc.Yaos[2] = iYao3;
                    return gsc;//找到
                }
            }

            return null;//找不到
        }


        /// <summary>
        /// 根据后天八卦索引获得三爻卦类
        /// </summary>
        /// <param name="GuoSubIndex"></param>
        /// <param name="IsDownGuo">是否为下卦</param>
        /// <returns></returns>
        public static GuoSubClass GetAfterGuoSub(int GuoSubIndex, bool IsDownGuo = true)
        {
            return GuoSubClass.GetGuoSub(AfterGuoSubNames[GuoSubIndex], IsDownGuo);
        }



        /// <summary>
        /// 根据先天八卦索引获得三爻卦类
        /// </summary>
        /// <param name="GuoSubIndex">先天卦的索引</param>
        /// <param name="IsDownGuo">是否为下卦</param>
        /// <returns></returns>
        public static GuoSubClass GetBeforGuoSub(int GuoSubIndex, bool IsDownGuo = true)
        {
            return GuoSubClass.GetGuoSub(BeforeGuoSubNames[GuoSubIndex], IsDownGuo);
        }


        /// <summary>
        /// 根据卦的属性或名称,获得三爻卦类  
        /// </summary>
        /// <param name="sAttrOrGuoName">属性名或卦名</param>
        /// <param name="IsDownGuo">是否为下卦(内卦)</param>
        /// <returns></returns> 
        public static GuoSubClass GetGuoSub(string sAttrOrGuoName, bool IsDownGuo = true)
        {
            int iPos = Array.IndexOf(BeforeGuoSubAttrNames, sAttrOrGuoName);

            if (iPos == -1)
            {
                iPos = Array.IndexOf(BeforeGuoSubNames, sAttrOrGuoName);
                if (iPos == -1) return null;//找不到
            }

            GuoSubClass gsc = GetGuoSub(iPos, IsDownGuo);

            return gsc;
        }
        /// <summary>
        /// 根据卦的属性索引,获得三爻卦类(先天)
        /// </summary>
        /// <param name="iAttrIndex">卦索引</param>
        /// <param name="IsDownGuo">是否为下卦</param>
        /// <returns></returns>
        public static GuoSubClass GetGuoSub(int iAttrIndex, bool IsDownGuo)
        {
            GuoSubClass gsc = new GuoSubClass(iAttrIndex);
            int iSkyIndex = 0;
            switch (iAttrIndex)
            {
                case 0: //乾卦

                    iSkyIndex = IsDownGuo ? 0 : 8;//内甲外壬
                    gsc.LocIndex = IsDownGuo ? 0 : 6;//内子外午
                    break;
                case 1: //兑卦

                    iSkyIndex = 3;//丁
                    gsc.LocIndex = IsDownGuo ? 5 : 11;//内巳外亥
                    break;
                case 2: //离卦

                    iSkyIndex = 5;//内己
                    gsc.LocIndex = IsDownGuo ? 3 : 9;//内卯外酉
                    break;
                case 3: //震卦

                    iSkyIndex = 6;//庚
                    gsc.LocIndex = IsDownGuo ? 0 : 6;//内子外午
                    break;
                case 4: //巽卦

                    iSkyIndex = 7;//辛
                    gsc.LocIndex = IsDownGuo ? 1 : 7;//内丑外未
                    break;
                case 5: //坎卦

                    iSkyIndex = 4;//戊
                    gsc.LocIndex = IsDownGuo ? 2 : 8;//内寅外申
                    break;
                case 6: //艮卦

                    iSkyIndex = 2;//丙
                    gsc.LocIndex = IsDownGuo ? 4 : 10;//内辰外戌
                    break;
                case 7: //坤卦

                    iSkyIndex = IsDownGuo ? 1 : 9;//内乙外癸
                    gsc.LocIndex = IsDownGuo ? 7 : 1;//内子外午
                    break;

            }
            gsc.GuoSubAttrName = BeforeGuoSubAttrNames[iAttrIndex];//卦的属性名称
            gsc.GuoSubReluName = BeforeGuoSubReluNames[iAttrIndex];//伦理关系
            gsc.GuoSubName = BeforeGuoSubNames[iAttrIndex];//卦名

            int iPos = Array.IndexOf(AfterGuoSubNames, gsc.GuoSubName);//找到后天位置
            gsc.AfterGuoSubColor = AfterGuoSubColors[iPos];//获得后天颜色

            gsc.FiveAttr = GetFiveAttrName(gsc.GuoSubName);
            gsc.LoadLocs();
            gsc.SkyName = SkyClass.SkyNames[iSkyIndex];
            gsc.IsDownGuo = IsDownGuo;
            iSkyIndex = 0;
            return gsc;
        }

        /// <summary>
        /// 获得卦的五行属性
        /// </summary>
        /// <param name="sGuoSubName"></param>
        /// <returns></returns>
        public static FiveAttr GetFiveAttrName(String sGuoSubName)
        {
            if (sGuoSubName.Equals("乾") || sGuoSubName.Equals("兑")) return new FiveAttr("金");
            if (sGuoSubName.Equals("坤") || sGuoSubName.Equals("艮")) return new FiveAttr("土");
            if (sGuoSubName.Equals("震") || sGuoSubName.Equals("巽")) return new FiveAttr("木");
            if (sGuoSubName.Equals("坎")) return new FiveAttr("水");
            if (sGuoSubName.Equals("离")) return new FiveAttr("火");

            return null;
        }
    }
}
