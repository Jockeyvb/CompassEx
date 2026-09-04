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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;


namespace CompassEx.Gua
{
    /// <summary>
    /// 表示三爻卦（经卦/单卦）的类，集成了伏羲先天八卦、文王后天八卦、洛书九宫、爻象状态及罗盘度数等核心理气算法。
    /// </summary>
    public class GuaSubClass : IEquatable<GuaSubClass>
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
        public readonly static String[] BeforeGuaSubAttrNames = { "天", "泽", "火", "雷", "风", "水", "山", "地" };

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
        public readonly static string[] BeforeGuaSubReluNames = { "父", "少女", "中女", "长男", "长女", "中男", "少男", "母" };

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
        public readonly static string[] BeforeGuaSubNames = { "乾", "兑", "离", "震", "巽", "坎", "艮", "坤" };

        /// <summary>
        /// 获取八卦相对卦爻状态映射的字典集合。
        /// </summary>
        /// <value>
        /// 键为单字卦名，值为由下至上长度为 3 的整型爻象数组（1代表阳爻，0代表阴爻，-1代表无爻）。
        /// </value>
        public readonly static Dictionary<string, int[]> GuaSubYaoValues = new Dictionary<string, int[]> { { "乾", [1, 1, 1] }, { "兑", [1, 1, 0] }, { "离", [1, 0, 1] }, { "震", [1, 0, 0] }, { "巽", [0, 1, 1] }, { "坎", [0, 1, 0] }, { "艮", [0, 0, 1] }, { "坤", [0, 0, 0] }, { "黄", [-1, -1, -1] } };

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
        public readonly static String[] BeforeGuaSubNumerics = { "一", "二", "三", "四", "五", "六", "七", "八" };

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
        public readonly static string[] AfterGuaSubAttrNames = { "水", "地", "雷", "风", "黄", "天", "泽", "山", "火" };

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
        public readonly static string[] AfterGuaSubNames = { "坎", "坤", "震", "巽", "黄", "乾", "兑", "艮", "离" };

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
            "\u2630" ,  //乾 (天)
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
        public readonly static String[] AfterGuaSubNumerics = { "一", "二", "三", "四", "五", "六", "七", "八", "九" };

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
        public readonly static string[] Colors = { "白", "黑", "碧", "绿", "黄", "白", "赤", "白", "紫" };

        #endregion

        #region 属性

        /// <summary>
        /// 获取一个布尔值，指示当前三爻经卦在后天八卦体系中是否属于“阳卦”。
        /// </summary>
        /// <value><c>true</c> 代表当前单卦属于阳卦；<c>false</c> 则属于阴卦。</value>
        /// <remarks>
        /// <b>四阳卦判定法则：</b><br/>
        /// 根据后天八卦阴阳划分规范，属于“乾（父）、坎（中男）、艮（少男）、震（长男）”四纯单卦之一的判定为阳卦。<br/>
        /// 该属性通过在字符串 <c>"乾坎艮震"</c> 中高速检索当前卦名（<see cref="Name"/>）的索引来完成原子级断立。
        /// </remarks>
        public bool IsSun { get { return "乾坎艮震".IndexOf(this.Name) > -1; } }

        /// <summary>
        /// 获取或设置一个布尔值，指示当前三爻单卦在组合为六爻复卦时，是否担当“下卦”（即内卦、贞卦），否则为上卦。
        /// </summary>
        /// <value>预设值为 <c>true</c>。若在初始化时被标记为复卦的下卦，则为 <c>true</c>，上卦则为 <c>false</c>。</value>
        /// <remarks>
        /// 上、下卦的主要区别用于六爻卦的卦爻封装，包括六神、干支配算等。
        /// </remarks>
        public bool IsDownGua { get; private set; } = true;

        /// <summary>
        /// 获取当前三爻单卦所蕴含的卦气（即洛书九宫数及与其绑定的先天五行参数），又名先天洛数。
        /// </summary>
        /// <value>返回一个全新初始化的卦气实体对象 <see cref="GuaQi"/>。</value>
        /// <remarks>
        /// 在玄空大卦及罗盘理气算法中，本单卦会作为参数被直接传入 <c>new GuaQi(this)</c> 构造函数中，
        /// 动态反哺出当前位置最精准的洛书数理指标，常用于后续六爻大卦的宏观卦气判定。
        /// </remarks>
        [JsonIgnore]
        public GuaQi GuaQi
        {
            get
            {
                return new GuaQi(this);
            }
        }

        /// <summary>
        /// 获取当前单卦在先天八卦序列中的索引位置（0-7）。
        /// </summary>
        /// <value>整型索引值，通过在 <see cref="BeforeGuaSubNames"/> 中检索当前卦名计算得出。</value>
        public int BeforeGuaIndex { get { return BeforeGuaSubNames.IndexOf(this.Name); } }

        /// <summary>
        /// 获取当前单卦在后天八卦序列中的索引位置（0-8）。
        /// </summary>
        /// <value>整型索引值，通过在 <see cref="AfterGuaSubNames"/> 中检索当前卦名计算得出。</value>
        public int AfterGuaIndex { get { return AfterGuaSubNames.IndexOf(this.Name); } }

        private IReadOnlyList<GuaYao> _yaos;

        /// <summary>
        /// 获取当前三爻经卦的爻象阴阳状态码数组（由下而上）。
        /// </summary>
        /// <value>只读爻对象列表，默认长度为 3。取值规范中：0代表阴爻，1代表阳爻。</value>
        public IReadOnlyList<GuaYao> Yaos
        {
            get
            {
                if (_yaos == null) _yaos = GuaYao.GetGuaSubYaos(this);
                return _yaos;
            }
        }

        /// <summary>
        /// 获取当前经卦在家族代际中所对应的传统易学伦理名称（如：“父”、“母”、“长男”、“少女”）。
        /// </summary>
        /// <value>符合《说卦传》经典人伦隐喻的中文分类字符串。</value>
        public string GuaSubReluName { get { return BeforeGuaSubReluNames[this.BeforeGuaIndex]; } }

        /// <summary>
        /// 获取当前经卦在文王后天八卦及洛书紫白九星体例中所映射的标准颜色文字属性（如：“白”、“黑”、“紫”）。
        /// </summary>
        /// <value>代表九星色彩名录的单字名称。</value>
        public string Color { get { return Colors[this.AfterGuaIndex]; } }

        /// <summary>
        /// 计算并获取当前单卦对应的文王后天八卦洛书九宫绝对数（整型：1 至 9）。
        /// </summary>
        /// <value>整型数值。通过在 <see cref="AfterGuaSubNumerics"/> 中反查中文数（如“八”）的索引，动态加 1 转换得出其九宫绝对物理运数。</value>
        public int AfterQuantity { get { return AfterGuaSubNumerics.IndexOf(this.AfterGuaSubCNQuantity) + 1; } }

        /// <summary>
        /// 计算并获取当前单卦对应的伏羲先天八卦绝对数（整型：1 至 8）。
        /// </summary>
        /// <value>整型数值。通过先天索引加 1 计算得出（乾一至坤八）。</value>
        [JsonIgnore]
        public int BeforeQuanity { get { return this.BeforeGuaIndex + 1; } }

        /// <summary>
        /// 获取当前经卦的标准单字名称（如：“乾”、“坤”、“震”、“巽”）。
        /// </summary>
        /// <value>代表三爻单卦的核心基础单字卦名。</value>
        public string Name { get; private set; }

        /// <summary>
        /// 获取当前经卦在自然界中模拟构成的核心物象/属性名称（如：“天”、“地”、“雷”、“风”）。
        /// </summary>
        /// <value>代表经八卦自然物象的单字名称。</value>
        public string AttrName { get { return BeforeGuaSubAttrNames[this.BeforeGuaIndex]; } }

        /// <summary>
        /// 获取当前单卦所属的易学核心五行实体对象。
        /// </summary>
        /// <value>包含生克属性的 <see cref="FiveAttr"/> 五行实体对象。</value>
        public FiveAttr FiveAttr { get { return GetFiveAttrName(this.Name); } }

        /// <summary>
        /// 获取当前单卦在先天卦序中对应的中文数字卦数名称（如：“一”、“二”、“三”）。
        /// </summary>
        /// <value>返回源自静态清册 <see cref="BeforeGuaSubNumerics"/> 的一字中文数字。</value>
        [JsonIgnore]
        public string BeforeGuaSubCNQuantity { get { return BeforeGuaSubNumerics[this.BeforeGuaIndex]; } }

        /// <summary>
        /// 获取当前单卦在后天洛书九宫中对应的中文数字运数名称（如：“一”、“二”、“九”）。
        /// </summary>
        /// <value>返回源自静态清册 <see cref="AfterGuaSubNumerics"/> 的一字中文数字。</value>
        public string AfterGuaSubCNQuantity { get { return AfterGuaSubNumerics[this.AfterGuaIndex]; } }

        /// <summary>
        /// 获取当前三爻单卦所对应的 Unicode 三爻经卦图形符号。
        /// </summary>
        /// <value>返回单个标准的 Unicode 经卦字符（如：☰、☱、☲ 等）。</value>
        /// <remarks>
        /// 内部通过 <see cref="BeforeGuaIndex"/> 先天索引，动态前往静态图形库 <see cref="Symbols"/> 中进行高精度内容提取。
        /// </remarks>
        [JsonIgnore]
        public string Symbol { get { return Symbols[this.BeforeGuaIndex]; } }

        /// <summary>
        /// 依据当前单卦名称，在三元地理（天盘）后天罗盘圈层中动态匹配并返回其所管辖的周天度数范围对象。
        /// </summary>
        /// <value>动态调用 <see cref="CompassEx.GetAfterGuaSubDegree(GuaSubClass)"/>，返回其专属的 <see cref="CompassRangEX"/> 后天周天空间物理边界。</value>
        public CompassRangEX CAfterRangeDegree { get { return CompassEx.GetAfterGuaSubDegree(this.Name); } }

        /// <summary>
        /// 依据当前单卦名称，在三元地理（地盘）伏羲先天罗盘方圆图圈层中动态匹配并返回其所管辖的周天度数范围对象。
        /// </summary>
        /// <value>动态调用 <see cref="CompassEx.GetBeforGuaSubDegree(GuaSubClass)"/>，返回其专属的 <see cref="CompassRangEX"/> 先天周天空间物理边界。</value>
        public CompassRangEX CBeforRangeDegree { get { return CompassEx.GetBeforGuaSubDegree(this.Name); } }

        #endregion

        #region 构造函数

        /// <summary>
        /// 依据单字简名初始化三爻经卦（单卦）对象实例。
        /// </summary>
        /// <param name="GuaName">输入的单字经卦名（例如：“乾”、“坤”、“坎”、“离”）。</param>     
        /// <exception cref="IndexOutOfRangeException">当输入的卦名在内置的经卦清册中不存在时抛出该异常。</exception>
        /// <remarks>
        /// <b>⚠️ 警告与调用规范：</b><br/>
        /// 使用此构造函数创建的经卦实例，其位置属性（<see cref="IsDownGua"/>）默认会被赋予 <c>true</c>（即默认作为下卦）。<br/>
        /// 如果您是在组合组装完整的六爻复卦，请优先改用工厂方法 <see cref="GuaSubClass.GetGuaSub(string, bool)"/>，以便明确指定该单卦是担当“上卦（外卦）”还是“下卦（内卦）”。
        /// </remarks>
        public GuaSubClass(string GuaName) : this(AfterGuaSubNames.IndexOf(GuaName))
        {
        }

        /// <summary>
        /// 依据后天卦序索引初始化三爻经卦对象实例（核心构造函数）。
        /// </summary>
        /// <param name="iAfterGuaIndex">后天八卦索引值（兼容五黄中宫，索引为 4，注意五黄无卦无爻）。</param>
        /// <exception cref="IndexOutOfRangeException">当传入的索引超出合法安全边界（小于 0 或大于等于数组总长度）时抛出。</exception>
        /// <remarks>
        /// <b>⚠️ 调用规范：</b><br/>
        /// 使用此构造函数创建的单卦类默认为下卦。在进行宏观六爻复卦装配时，建议使用具备显式方位标识的工厂方法 <see cref="GuaSubClass.GetGuaSub(int, bool)"/>。<br/><br/>
        /// <b>经卦底层数理装配流程：</b>
        /// <list type="number">
        /// <item><description><b>安全边界校验</b>：校验输入索引。若执行不合法则瞬间熔断并抛出越界异常。</description></item>
        /// <item><description><b>爻象阴阳重组（Switch 分布）</b>：依据先天八卦符号编码（0代表阴爻，1代表阳爻），由下而上对长为 3 的 <see cref="Yaos"/> 数组进行赋值。</description></item>
        /// <item><description><b>基础术数属性反哺</b>：通过索引同步锁定并充填物象（<see cref="AttrName"/>）、六亲伦理（<see cref="GuaSubReluName"/>）和基本单字卦名。</description></item>
        /// <item><description><b>跨体系方位映射</b>：通过后天八卦清册反查其在文王后天九宫中的绝对物理位置，进而将对应方位的紫白九星颜色（<see cref="Color"/>）以及最终的生克五行属性（<see cref="FiveAttr"/>）彻底装载完毕。</description></item>
        /// </list>
        /// </remarks>
        [JsonConstructor]
        public GuaSubClass([JsonProperty(nameof(AfterGuaIndex))] int iAfterGuaIndex)
        {
            if (iAfterGuaIndex < 0 || iAfterGuaIndex >= AfterGuaSubNames.Length) throw new IndexOutOfRangeException();
            this.Name = AfterGuaSubNames[iAfterGuaIndex]; //以名称为主
        }

        #endregion

        #region 方法

        /// <summary>
        /// 将当前单卦转换为六爻复卦（上下三爻卦完全相同，即八纯卦）。
        /// </summary>
        /// <returns>返回新生成的六爻重卦实例 <see cref="GuaClass"/>。</returns>
        public GuaClass ToGuaClass()
        {
            return new GuaClass(this.Name);
        }

        /// <summary>
        /// 获取后天八卦中包含的所有正针 24 山罗盘度数映射集合。
        /// </summary>
        /// <returns>返回一个字典，键为经卦管辖的度数范围 <see cref="CompassRangEX"/>，值为对应的 24 山山向对象 <see cref="CHill"/>。</returns>        
        public Dictionary<CompassRangEX, CHill> GetC24Hills()
        {
            Dictionary<CompassRangEX, CHill> dc = new Dictionary<CompassRangEX, CHill>();
            CompassRangEX CRE = this.CAfterRangeDegree;
            foreach (string sN in CHill.C24HillNames)
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
        /// 获取后天八卦中所包含的所有先天 64 卦度数映射集合。
        /// </summary>
        /// <returns>返回一个字典，键为先天 64 卦的周天度数区间 <see cref="CompassRangEX"/>，值为对应的六爻卦实例 <see cref="GuaClass"/>。</returns>        
        public Dictionary<CompassRangEX, GuaClass> GetCBeforGuas()
        {
            Dictionary<CompassRangEX, GuaClass> dc = new Dictionary<CompassRangEX, GuaClass>();
            CompassRangEX CRE = this.CAfterRangeDegree;
            foreach (var kv in C3YEx.CBeforeGuas)
            {
                if (CRE.IsInRange(kv.Key.Start))
                {
                    dc.Add(kv.Key, kv.Value);
                }
            }
            return dc;
        }

        /// <summary>
        /// 获取当前单卦的反卦（即各爻阴阳属性全部取反后的对立经卦）。
        /// </summary>
        /// <returns>返回取反后生成的全新 <see cref="GuaSubClass"/> 经卦实例。</returns>
        public GuaSubClass GetXorGua()
        {
            int[] iYaos = { 0, 0, 0 };

            for (int i = 0; i < 3; i++)
            {
                iYaos[i] = this.Yaos[i].Value % 2 == 0 ? 1 : 0; // 1.4版本修复：当数值大于1时的求模判断容错
            }
            return GetGuaSub(iYaos[0], iYaos[1], iYaos[2], this.IsDownGua);
        }

        /// <summary>
        /// 根据卦的前三爻状态（由下而上），获取对应的先天三爻卦类实例。
        /// </summary>
        /// <param name="iYao1">初爻状态（0代表阴，1代表阳）。</param>
        /// <param name="iYao2">二爻状态（0代表阴，1代表阳）。</param>
        /// <param name="iYao3">三爻状态（0代表阴，1代表阳）。</param>
        /// <param name="IsDownGua">指示当前经卦是否为复卦的下卦（内卦）。</param>
        /// <returns>若匹配成功则返回对应的 <see cref="GuaSubClass"/> 实例；若无匹配项则返回 <c>null</c>。</returns>
        public static GuaSubClass? GetGuaSub(int iYao1, int iYao2, int iYao3, bool IsDownGua)
        {
            GuaSubClass gsc;
            for (int i = 0; i < AfterGuaSubNames.Length; i++)
            {
                gsc = GetGuaSub(i, IsDownGua);
                if (i != 4) // 五黄无卦无爻
                {
                    if (gsc.Yaos[0].Value == iYao1 % 2 && gsc.Yaos[1].Value == iYao2 % 2 && gsc.Yaos[2].Value == iYao3 % 2)
                    {
                        return gsc; // 找到匹配项
                    }
                }
            }
            return null; // 未找到
        }

        /// <summary>
        /// 根据后天八卦名称获取三爻卦类实例（内部自动处理兼容五黄无卦的情况）。
        /// </summary>
        /// <param name="GuaName">后天单字卦名（如：“坎”、“离”等）。</param>
        /// <param name="IsDownGua">指示当前经卦是否为下卦，默认为 <c>true</c>。</param>
        /// <returns>返回对应的 <see cref="GuaSubClass"/> 实例。</returns>
        public static GuaSubClass GetAfterGuaSub(string GuaName, bool IsDownGua = true)
        {
            return GetAfterGuaSub(AfterGuaSubNames.IndexOf(GuaName), IsDownGua);
        }

        /// <summary>
        /// 根据后天八卦索引获取三爻卦类实例（内部自动处理兼容五黄无卦的情况）。
        /// </summary>
        /// <param name="GuaSubIndex">后天卦序索引值。</param>
        /// <param name="IsDownGua">指示当前经卦是否为下卦，默认为 <c>true</c>。</param>
        /// <returns>返回对应的 <see cref="GuaSubClass"/> 实例。</returns>
        public static GuaSubClass GetAfterGuaSub(int GuaSubIndex, bool IsDownGua = true)
        {
            return GetGuaSub(GuaSubIndex, IsDownGua);
        }

        /// <summary>
        /// 根据卦的自然物象属性或单字卦名，获取对应的三爻卦类实例。
        /// </summary>
        /// <param name="sAttrOrGuaName">属性名或卦名（例如：“天”或“乾”）。</param>
        /// <param name="IsDownGua">指示当前经卦是否为下卦（内卦），默认为 <c>true</c>。</param>
        /// <returns>若匹配成功返回 <see cref="GuaSubClass"/> 实例；否则返回 <c>null</c>。</returns> 
        public static GuaSubClass? GetGuaSub(string sAttrOrGuaName, bool IsDownGua = true)
        {
            int iPos = Array.IndexOf(AfterGuaSubAttrNames, sAttrOrGuaName);

            if (iPos == -1)
            {
                iPos = Array.IndexOf(AfterGuaSubNames, sAttrOrGuaName);
                if (iPos == -1) return null; // 无法匹配
            }

            GuaSubClass gsc = GetGuaSub(iPos, IsDownGua);
            return gsc;
        }

        /// <summary>
        /// 根据后天卦序索引及上下卦标记，获得对应的三爻卦类实例。
        /// </summary>
        /// <param name="AfterGuaIndex">后天卦序索引。</param>
        /// <param name="IsDownGua">指示当前经卦是否为下卦。</param>
        /// <returns>返回新初始化的 <see cref="GuaSubClass"/> 实例。</returns>
        public static GuaSubClass GetGuaSub(int AfterGuaIndex, bool IsDownGua)
        {
            GuaSubClass gsc = new GuaSubClass(AfterGuaIndex);
            gsc.IsDownGua = IsDownGua;
            gsc.Name = AfterGuaSubNames[AfterGuaIndex]; // 赋予卦名
            return gsc;
        }

        /// <summary>
        /// 根据经卦名称获取对应的五行属性。
        /// </summary>
        /// <param name="sGuaSubName">单字经卦名称。</param>
        /// <returns>若匹配成功返回对应的 <see cref="FiveAttr"/> 五行实体；若无法匹配则返回 <c>null</c>。</returns>
        private static FiveAttr? GetFiveAttrName(String sGuaSubName)
        {
            if (sGuaSubName.Equals("乾") || sGuaSubName.Equals("兑")) return new FiveAttr("金");
            if (sGuaSubName.Equals("坤") || sGuaSubName.Equals("艮")) return new FiveAttr("土");
            if (sGuaSubName.Equals("震") || sGuaSubName.Equals("巽")) return new FiveAttr("木");
            if (sGuaSubName.Equals("坎")) return new FiveAttr("水");
            if (sGuaSubName.Equals("离")) return new FiveAttr("火");

            return null;
        }

        #region 显式实现对比、运算符和 Key 方法

        /// <summary>
        /// 判断当前对象是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">要与当前对象进行比较的另一个对象。</param>
        /// <returns>如果对象相等则返回 <c>true</c>；否则返回 <c>false</c>。</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as GuaSubClass);
        }

        /// <summary>
        /// 判断当前三爻卦实例是否与另一个指定的 <see cref="GuaSubClass"/> 实例相等。
        /// </summary>
        /// <param name="other">要比较的另一个三爻卦实例。</param>
        /// <returns>如果两者的卦名完全一致则返回 <c>true</c>；否则返回 <c>false</c>。</returns>
        bool IEquatable<GuaSubClass>.Equals(GuaSubClass other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(this.Name, other.Name, StringComparison.Ordinal);
        }

        /// <summary>
        /// 获取当前经卦实例的哈希代码。
        /// </summary>
        /// <returns>返回基于卦名字段计算得到的整型哈希值。</returns>
        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }

        /// <summary>
        /// 检查两个三爻卦实例是否相等。
        /// </summary>
        /// <param name="left">左侧三爻卦实例。</param>
        /// <param name="right">右侧三爻卦实例。</param>
        /// <returns>相等返回 <c>true</c>；否则返回 <c>false</c>。</returns>
        public static bool operator ==(GuaSubClass left, GuaSubClass right)
        {
            if (left is null) return right is null;
            return ((IEquatable<GuaSubClass>)left).Equals(right);
        }

        /// <summary>
        /// 检查两个三爻卦实例是否不相等。
        /// </summary>
        /// <param name="left">左侧三爻卦实例。</param>
        /// <param name="right">右侧三爻卦实例。</param>
        /// <returns>不相等返回 <c>true</c>；否则返回 <c>false</c>。</returns>
        public static bool operator !=(GuaSubClass left, GuaSubClass right)
        {
            return !(left == right);
        }

        #endregion

        #endregion 
    }
}
