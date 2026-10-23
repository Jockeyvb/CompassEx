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
using CommLib;
using CompassEx.Comm;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using static CommLib.Assist.JsonEx;


namespace CompassEx.Gua;

/// <summary>
/// 卦数合类型
/// </summary>
public enum GuaFitType
{
    None = 0,
    //===========================卦数合==========================
    /// <summary>
    /// 一六共宗水
    /// </summary>
    [Description("一六共宗水")]
    GongZong = 1,
    /// <summary>
    /// 二七同道火
    /// </summary>
    [Description("二七同道火")]
    TongDao = 2,
    /// <summary>
    /// 三八为朋木
    /// </summary>
    [Description("三八为朋木")] WeiPeng = 4,
    /// <summary>
    /// 四九为友金
    /// </summary>
    [Description("四九为友金")] WeiYou = 8,

    /// <summary>
    /// 五十同途土(合五、十、十五）
    /// </summary>
    [Description("五十同途土")] TongTu = 16,
    //===========================卦数合==========================
    //===========================卦理合==========================
    /// <summary>
    /// 则乾坤、震巽、坎离、艮兑
    /// </summary>
    [Description("雌雄正配")]
    CiXiong = TongTu * 2,
    //===========================卦理合==========================
}

/// <summary>
/// 表示一个六爻复卦（64卦）的数据结构与核心数理推演类。
/// </summary> 

public class GuaClass : IEquatable<GuaClass>
{

    #region 字段

    /// <summary>
    /// 64卦全卦简名
    /// </summary>
    [JsonIgnore]
    public static String[] Names = { "乾", "坤", "屯", "蒙", "需", "讼", "师", "比", "小畜", "履", "泰", "否", "同人", "大有", "谦", "豫", "随", "蛊", "临", "观", "噬嗑", "贲", "剥", "复", "无妄", "大畜", "颐", "大过", "坎", "离", "咸", "恒", "遁", "大壮", "晋", "明夷", "家人", "睽", "蹇", "解", "损", "益", "夬", "姤", "萃", "升", "困", "井", "革", "鼎", "震", "艮", "渐", "归妹", "丰", "旅", "巽", "兑", "涣", "节", "中孚", "小过", "既济", "未济" };

    /// <summary>
    /// 64卦全卦名字
    /// </summary>
    [JsonIgnore]
    public static String[] FullNames = { "乾为天", "坤为地", "水雷屯", "山水蒙", "水天需", "天水讼", "地水师", "水地比", "风天小畜", "天泽履", "地天泰", "天地否", "天火同人", "火天大有", "地山谦", "雷地豫", "泽雷随", "山风蛊", "地泽临", "风地观", "火雷噬嗑", "山火贲", "山地剥", "地雷复", "天雷无妄", "山天大畜", "山雷颐", "泽风大过", "坎为水", "离为火", "泽山咸", "雷风恒", "天山遁", "雷天大壮", "火地晋", "地火明夷", "风火家人", "火泽睽", "水山蹇", "雷水解", "山泽损", "风雷益", "泽天夬", "天风姤", "泽地萃", "地风升", "泽水困", "水风井", "泽火革", "火风鼎", "震为雷", "艮为山", "风山渐", "雷泽归妹", "雷火丰", "火山旅", "巽为风", "兑为泽", "风水涣", "水泽节", "风泽中孚", "雷山小过", "水火既济", "火水未济" };

    /// <summary>
    /// 64卦对应60甲子
    /// </summary>
    [JsonIgnore]
    public static string[] GuaSkyLocNames = { "甲午", "甲子", "戊子", "庚申", "乙巳", "辛未", "壬申", "辛亥", "丁巳", "戊辰", "庚辰", "庚戌", "壬寅", "辛巳", "戊戌", "丁亥", "丁丑", "丁未", "乙卯", "己亥", "乙丑", "癸丑", "癸亥", "甲子", "己丑", "壬辰", "丙子", "丙午", "庚申", "庚寅", "丁酉", "庚午", "乙酉", "己巳", "乙亥", "辛丑", "丙寅", "甲辰", "甲戌", "丙申", "丁卯", "庚子", "癸巳", "甲午", "壬戌", "己未", "癸未", "乙未", "庚寅", "戊午", "壬子", "丙戌", "癸酉", "癸卯", "戊寅", "己酉", "壬午", "丙辰", "戊申", "己卯", "辛卯", "辛酉", "甲寅", "甲申" };

    public static readonly string[] Symbols = new string[64]
{"䷀",//乾
"䷁",//坤
"䷂",//屯
"䷃",//蒙
"䷄",//需
"䷅",//讼
"䷆",//师
"䷇",//比
"䷈",//小畜
"䷉",//履
"䷊",//泰
"䷋",//否
"䷌",//同人
"䷍",//大有
"䷎",//谦
"䷏",//豫
"䷐",//随
"䷑",//蛊
"䷒",//临
"䷓",//观
"䷔",//噬嗑
"䷕",//贲
"䷖",//剥
"䷗",//复
"䷘",//无妄
"䷙",//大畜
"䷚",//颐
"䷛",//大过
"䷜",//坎
"䷝",//离
"䷞",//咸
"䷟",//恒
"䷠",//遁
"䷡",//大壮
"䷢",//晋
"䷣",//明夷
"䷤",//家人
"䷥",//睽
"䷦",//蹇
"䷧",//解
"䷨",//损
"䷩",//益
"䷪",//夬
"䷫",//姤
"䷬",//萃
"䷭",//升
"䷮",//困
"䷯",//井
"䷰",//革
"䷱",//鼎
"䷲",//震
"䷳",//艮
"䷴",//渐
"䷵",//归妹
"䷶",//丰
"䷷",//旅
"䷸",//巽
"䷹",//兑
"䷺",//涣
"䷻",//节
"䷼",//中孚
"䷽",//小过
"䷾",//既济
"䷿"//未济
};



    /// <summary>
    /// 获取玄空大卦算法中对应的“卦运”（江东卦、江西卦、南北八大纯卦等一至九运）名称列表。
    /// </summary>
    /// <value>
    /// <code>
    /// 索引 0-8 分别对应：
    /// [ "一", "二", "三", "四", "(五运为中央)", "六", "七", "八", "九" ]
    /// </code>
    /// </value>
    public static readonly string[] GuaFateNames = { "一", "二", "三", "四", "", "六", "七", "八", "九" };

    #endregion

    #region 属性




    /// <summary>
    /// 本六爻卦的上卦与下卦的卦气合类型(在玄空大卦中能判断抽爻换象的吉凶）
    /// 于《玄空大卦透析》下册中，爻神篇中提到,经典《竹节赋》也提到相关卦理
    /// </summary>
    public GuaFitType GuaQiFit
    {
        get
        {
            return GetGuaFit(this.UpGua, this.DownGua);

        }
    }

    /// <summary>
    /// 上卦与下卦之间的关系
    /// </summary>
    public FiveAttrRule UpWithDownGuaRule { get => this.UpGua.FiveAttr.WhatRule(this.DownGua.FiveAttr); }


    /// <summary>
    /// 获取或设置原卦对象（若当前实例为变卦则指向原本卦，若无变动则为 <c>null</c>）。
    /// </summary>
    public GuaClass RawGua { get; private set; }

    /// <summary>
    /// 错卦或叫反卦
    /// </summary>
    [JsonIgnore]
    public GuaClass XorGua
    {
        get
        {
            var gc = new GuaClass(this.Index);
            gc.SetYaoDoing([0, 1, 2, 3, 4, 5]);
            gc = gc.GetChangeGua();
            gc.LoadAllYaos();
            return gc;
        }
    }


    /// <summary>
    /// 本卦是否有伏神
    /// </summary>
    public bool IsHadHideRelative { get { return this.Yaos.Count(x => x.HideRelative != null) > 0; } }

    private IReadOnlyList<GuaYao> _Yaos;

    /// <summary>
    /// 获取复卦由下卦与上卦组合而成的六爻干支与属性集合。
    /// </summary>
    /// <value>包含 6 个 <see cref="GuaYao"/> 对象的只读列表。</value>
    [JsonIgnore]
    public IReadOnlyList<GuaYao> Yaos
    {
        get
        {
            if (_Yaos == null) _Yaos = this.DownGua?.Yaos.Concat(this.UpGua.Yaos).ToList();
            return _Yaos;
        }
    }





    /// <summary>
    /// 爻值
    /// </summary>
    public int[] YaosValue { get => this.Yaos.Select(x => x.Value).ToArray(); }
    /// <summary>
    /// 获取当前六爻卦所对应的标准 Unicode 易经六爻图形符号。
    /// </summary>
    /// <value>返回单个 Unicode 易经卦象字符（例如：䷀、䷁、䷂等）。</value>
    /// <remarks>
    /// 该属性通过当前实例的 <see cref="Index"/> 属性作为索引，
    /// 动态前往静态字符集清册 <see cref="Symbols"/> 中提取对应的 Unicode 易经六爻符号（范围从 \u4DC0 至 \u4DFF）。
    /// </remarks>
    [JsonIgnore]
    public string Symbol { get { return Symbols[this.Index]; } }

    /// <summary>
    /// 玄空大卦中上下卦先天洛数同时为阳或阴则为阳路顺行，否则阴阳相见为阴路逆（卦爻可按阳顺（左），阴逆（右）排）
    /// </summary>
    public bool IsSunRouter
    {
        get
        {


            return (this.DownGua.GuaQi.GuaQiNumber % 2 == 1 && this.UpGua.GuaQi.GuaQiNumber % 2 == 1 || this.DownGua.GuaQi.GuaQiNumber % 2 == 0 && this.UpGua.GuaQi.GuaQiNumber % 2 == 0);

        }

    }


    /// <summary>
    /// 获取当前六爻卦对应的 64 卦卦气（即洛书数，用作先天五行数计算）。
    /// </summary>
    /// <value>返回一个 <see cref="GuaQi"/> 对象，包含当前大卦的洛书数及先天五行属性。</value>
    /// <remarks>
    /// 在玄空大卦数理中，采取“上卦洛数为卦气，下卦洛数为卦运”的原则。
    /// 此属性通过动态访问 <c>this.UpGua.GuaQi</c> 来直接提取外卦的洛书数值。
    /// </remarks>
    [JsonIgnore]
    public GuaQi GuaQi
    {
        get
        {
            return this.UpGua.GuaQi;
        }
    }

    /// <summary>
    /// 获取当前复卦依易经传统卦序所映射的 60 甲子原天干地支结构对象。
    /// </summary>
    /// <value>一个封装好的 <see cref="SkyLoc"/> 对象。</value>
    /// <remarks>
    /// 术数中大成卦纳甲与大管局算法中，64 卦与 60 甲子的映射链条完全采用经典的周易本经卦序（从乾坤至既济未济）进行顺次对齐和绑定。
    /// </remarks>
    [JsonIgnore]
    public SkyLoc GuaSkyLoc
    {
        get
        {
            SkyLoc sl = new SkyLoc(GuaSkyLocNames[this.Index]);
            return sl;
        }
    }

    /// <summary>
    /// 根据干支是否有第二个卦，如果有则返回另一个卦，只有一个的返回null
    /// </summary>
    [JsonIgnore]
    public GuaClass? FromSkyLocSecGua
    {
        get
        {
            var gs = GetGuaBySkyLoc(this.GuaSkyLoc.Name);
            if (gs?.Count <= 1) return null;

            return gs.LastOrDefault(x => x.FullName != this.FullName);

        }
    }

    /// <summary>
    /// 计算并返回当前复卦对应的三元玄空“大卦运”名称（一运至九运）。
    /// </summary>
    /// <value>返回代表大卦运的一字中文数字字符串（如：“一”、“二”、“三”、“四”、“六”、“七”、“八”、“九”）。</value>
    /// <remarks>
    /// <b>卦运阴阳对比推导核心算法：</b><br/>
    /// 卦象爻是由下而上生发的。算法会将上卦（外卦）的三枚爻与下卦（内卦）对应的三枚爻进行从下往上的两两阴阳符号判等（取模 2 校验法）：
    /// <list type="bullet">
    /// <item><description><b>一运卦</b>：上卦下卦三爻的阴阳结构<b>完全相同</b>。</description></item>
    /// <item><description><b>九运卦</b>：上卦下卦三爻的阴阳结构<b>完全相反（全错卦）</b>。</description></item>
    /// <item><description><b>其它运卦</b>：根据初爻、中爻、上爻各自发生变换的错位组合，分别归入二、三、四、六、七、八运（注：五运归中，无独立五运大卦）。</description></item>
    /// </list>
    /// </remarks>
    [JsonIgnore]
    public string GuaFate
    {
        get
        {
            bool[] f = { true, true, true };

            for (int i = 0; i < 3; i++)
            {
                f[i] = this.UpGua.Yaos[i].Value % 2 == this.DownGua.Yaos[i].Value % 2;
            }
            if (f[0] == true && f[1] == true && f[2] == true)
            {
                return "一";
            }
            else if (f[0] == false && f[1] == false && f[2] == false)
            {
                return "九";
            }
            else if (f[0] == false && f[1] == true && f[2] == true)
            {
                return "八";
            }
            else if (f[0] == true && f[1] == false && f[2] == true)
            {
                return "七";
            }
            else if (f[0] == true && f[1] == true && f[2] == false)
            {
                return "六";
            }
            else if (f[0] == true && f[1] == false && f[2] == false)
            {
                return "二";
            }
            else if (f[0] == false && f[1] == true && f[2] == false)
            {
                return "三";
            }
            else
            {
                return "四";
            }
        }
    }

    /// <summary>
    /// 获取当前六爻卦的下卦（又称内卦、贞卦），为三爻卦结构。
    /// </summary>
    /// <value>下卦的三爻封装对象 <see cref="GuaSubClass"/>。</value>
    [JsonIgnore]
    public GuaSubClass DownGua { get; private set; }

    /// <summary>
    /// 获取当前六爻卦的上卦（又称外卦、悔卦），为三爻卦结构。
    /// </summary>
    /// <value>上卦的三爻封装对象 <see cref="GuaSubClass"/>。</value>
    [JsonIgnore]
    public GuaSubClass UpGua { get; private set; }

    /// <summary>
    /// 获取或设置当前六爻卦的两字简化卦名（如“乾”、“坤”、“屯”、“蒙”）。
    /// </summary>
    /// <value>代表 64 卦通行本顺序的两字简名缩写。</value>
    public String Name { get { return Names[this.Index]; } }

    /// <summary>
    /// 获取或设置当前六爻卦的四字全名（包含上下卦象组合，如“乾为天”、“地水师”）。
    /// </summary>
    /// <value>代表包含上下经八卦对应结构的四字或三字全称。</value>
    public string FullName { get { return FullNames[Index]; } }

    /// <summary>
    /// 获取或设置当前卦在 64 卦数据库或特定排列模型中的原始数组索引。
    /// </summary>
    /// <value>预设值为 <c>-1</c>，表示尚未初始化；初始化后范围通常在 <c>0</c> 到 <c>63</c> 之间。</value>
    public int Index { get; private set; } = -1;


    private List<string> _GuaAlias;

    /// <summary>
    /// 获取或设置当前六爻卦的八宫所属别名分类集合。
    /// </summary>
    public List<string> GuaAlias
    {
        get
        {
            if (_GuaAlias == null) _GuaAlias = new List<string>();
            return _GuaAlias;
        }
        internal set { _GuaAlias = value; }
    }

    /// <summary>
    /// 获取当前六爻卦的八宫所属别名分类文本（如：“归魂卦”、“游魂卦”、“纯卦/八纯”等）。
    /// </summary>
    /// <value>描述当前卦在京房易八宫变易体系中所处位置的分类字符串。</value>
    public string GuaAliasNames { get { return string.Join(",", this.GuaAlias); } }

    /// <summary>
    /// 获取当前六爻卦所归属的“卦宫”（本宫三爻纯卦，如乾宫、坤宫、震宫等）。
    /// </summary>
    /// <value>代表该六爻卦所属母宫的三爻卦封装对象 <see cref="GuaSubClass"/>。</value>
    [JsonIgnore]
    public GuaSubClass GuaSelf { get; internal set; }

    /// <summary>
    /// 获取该卦在三元地理罗盘中对应的先天（天盘）度数范围对象，用于判定当前卦在罗盘圆周上的物理空间边界。
    /// </summary>
    /// <value>动态调用 <see cref="C3Y.GetCBeforeGuaDegree(string)"/> 方法，返回其专属的 <see cref="C3Y"/> 周天度数范围。</value>
    [JsonIgnore]
    public CompassRangEX? CBeforeRangeDegree { get { return C3YEx.GetCBeforeGuaDegree(this.Name); } }

    /// <summary>
    /// 获取该卦在三元地理罗盘中对应的后天（地盘）度数范围对象，用于判定当前卦在罗盘圆周上的物理空间边界。
    /// </summary>
    /// <value>动态调用 <see cref="C3Y.GetCAfterGuaDegree(string)"/> 方法，返回其专属的 <see cref="C3Y"/> 周天度数范围。</value>
    [JsonIgnore]
    public CompassRangEX? CAfterRangeDegree { get { return C3YEx.GetCAfterGuaDegree(this.Name); } }

    #endregion

    #region 构造函数


    /// <summary>
    /// JSON反序列化构造函数，用于在反序列化过程中正确加载六爻卦对象及其爻动状态。
    /// </summary>
    /// <param name="GuaYaos"></param>
    /// <param name="YaosValue"></param>
    /// <param name="FullName"></param>
    /// <exception cref="Exception"></exception>
    [JsonConstructor]
    private GuaClass([JsonProperty(nameof(Yaos))] IList<GuaYao> GuaYaos, [JsonProperty(nameof(YaosValue))] int[] YaosValue, [JsonProperty(nameof(FullName))] string FullName)
    {
        bool IsLoaded = false;
        try
        {

            if (GuaYaos?.Count() == 6)
            {
                GuaClass g = GuaClass.GetGuaClass(GuaYaos.Select(x => x.Value).ToArray());

                this.ApplyBaseProperties(g);
                IsLoaded = true;
            }
            else if (YaosValue != null && YaosValue.Length == 6)
            {
                GuaClass g = GuaClass.GetGuaClass(YaosValue);

                this.ApplyBaseProperties(g);
                IsLoaded = true;
            }
            else
            {
                GuaClass g = new GuaClass(FullName);
                this.ApplyBaseProperties(g);
                IsLoaded = true;
            }
            this.LoadAllYaos();
        }
        catch (Exception ex)
        {
            throw new Exception("JSON反序列化失败！", ex);
        }



    }



    /// <summary>
    /// 依据简名初始化复卦（六爻卦）对象实例。
    /// </summary>
    /// <param name="GuaNameOrAttrName">输入的先天 64 卦简名（例如：“乾”、“坤”）或属性名，可参考：<see cref="FullNames"/>。</param>
    /// <exception cref="IndexOutOfRangeException">当输入的卦名在内置的 <see cref="Names"/> 列表中不存在时抛出该异常。</exception>

    public GuaClass([JsonProperty(nameof(FullName))] string GuaNameOrAttrName) : this(GetGuaIndexByName(GuaNameOrAttrName))
    {
    }

    /// <summary>
    /// 依据卦序索引初始化复卦（六爻卦）对象实例（核心构造函数）。
    /// </summary>
    /// <param name="GuaIndex">64 卦的原始数组索引（取值范围：<c>0</c> 至 <c>63</c>）。</param>
    /// <exception cref="IndexOutOfRangeException">当传入的索引值超出正常边界时抛出。</exception>

    public GuaClass(int GuaIndex)
    {
        if (GuaIndex < 0 || GuaIndex >= Names.Length) throw new IndexOutOfRangeException(nameof(GuaIndex));
        this.Index = GuaIndex;

        string sN1 = this.FullName.Substring(0, 1);
        string sN2 = this.FullName.Substring(1, 1);
        if (sN2 == "为")
        {
            this.DownGua = GuaSubClass.GetGuaSub(sN1, true);
            this.UpGua = GuaSubClass.GetGuaSub(sN1, false);
        }
        else
        {
            this.UpGua = GuaSubClass.GetGuaSub(sN1, false);
            this.DownGua = GuaSubClass.GetGuaSub(sN2, true);
        }
    }

    #endregion

    #region 方法

    /// <summary>
    /// 设置爻动的位置（0 为初爻），设置后可以使用 <see cref="GetChangeGua"/> 来加载变出的卦。
    /// </summary>
    /// <param name="YaosPos">要设置爻动的位置数组。</param>
    /// <param name="IsCancel">是否取消爻动状态。</param>
    /// <exception cref="ArgumentOutOfRangeException">当传入的动爻数量为 0 或超过 6 个时抛出。</exception>
    public void SetYaoDoing(int[] YaosPos, bool IsCancel = false)
    {
        if (YaosPos.Length == 0 || YaosPos.Length > 6) throw new ArgumentOutOfRangeException(nameof(YaosPos), "爻动数量不能为0或超过6");

        foreach (int v in YaosPos)
        {
            this.Yaos[v].IsDoing = !IsCancel;
        }
    }



    /// <summary>
    /// 推演当前本卦所对应的“通卦”（又称互通卦）。
    /// </summary>
    /// <returns>返回一个全新装配的通卦对象实例 <see cref="GuaClass"/>。</returns>
    /// <remarks>
    /// <b>通卦数理法则：</b><br/>
    /// 在特定大卦法门中，规定一三、二四、六八、七九运数彼此相通。<br/>
    /// 该方法在内部动态克隆一个相同的复卦，并强制将<b>第 3 爻（索引 2）与第 4 爻（索引 3）</b>设定为动爻，
    /// 随后通过调用 <see cref="GetChangeGua"/> 产生错卦变动，从而精准荡出互通的通卦。
    /// </remarks>
    public GuaClass GetExchangeGua()
    {
        GuaClass g = new GuaClass(this.Index);
        g.SetYaoDoing(new int[] { 2, 3 });

        return g.GetChangeGua();
    }

    /// <summary>
    /// 计算并演录当前纯卦的“京房易八宫卦变规律”，生成完整的七世飞爻卦链表（含主卦共 8 个卦）。
    /// </summary>
    /// <returns>返回一个按演变顺序排列的复卦集合 <c>List&lt;GuaClass&gt;</c>，依次为主卦、一世、二世、三世、四世、五世、游魂卦、归魂卦。</returns>
    /// <exception cref="Exception">当当前卦不是“八纯卦”时抛出该异常，因为非纯卦无法作为八宫立极之主。</exception>
    public List<GuaClass> Get7HereYaoGua()
    {
        if (this.UpGua.Name != this.DownGua.Name) throw new Exception("本卦不是纯卦，不能用于计算7世飞爻卦");

        List<GuaClass> GuaIns = new List<GuaClass>();
        GuaIns.Add(this);
        GuaClass g = new GuaClass(this.Name);

        GuaClass ng;
        for (int i = 0; i < 5; i++)
        {
            ng = new GuaClass(g.Name);
            ng.SetYaoDoing(new int[] { i });
            g = ng.GetChangeGua()!;
            GuaIns.Add(g);
        }

        ng = new GuaClass(g.Name);
        ng.SetYaoDoing(new int[] { 3 });
        g = ng.GetChangeGua()!;
        GuaIns.Add(g);

        ng = new GuaClass(g.Name);
        ng.SetYaoDoing(new int[] { 0, 1, 2 });
        g = ng.GetChangeGua()!;
        GuaIns.Add(g);

        return GuaIns;
    }



    /// <summary>
    /// 根据当前本卦设定的动爻状态推演并取得变卦。
    /// </summary>
    /// <returns>返回一个全新的变卦对象实例 <see cref="GuaClass"/>；若当前无动爻则返回 <c>null</c>。</returns>
    public GuaClass? GetChangeGua()
    {
        if (!this.Yaos.Where(x => x.IsDoing).Any()) return null;

        int[] ar = [this.Yaos[0].ChangingValue, this.Yaos[1].ChangingValue, this.Yaos[2].ChangingValue];

        string DownGuaName = GuaSubClass.GuaSubYaoValues.Where(x => x.Value.SequenceEqual(ar)).Select(x => x.Key).FirstOrDefault();
        if (string.IsNullOrWhiteSpace(DownGuaName)) throw new Exception("不能找到相关卦象");
        ar = [this.Yaos[3].ChangingValue, this.Yaos[4].ChangingValue, this.Yaos[5].ChangingValue];
        string UpGuaName = GuaSubClass.GuaSubYaoValues.Where(x => x.Value.SequenceEqual(ar)).Select(x => x.Key).FirstOrDefault();
        if (string.IsNullOrWhiteSpace(UpGuaName)) throw new Exception("不能找到相关卦象");

        string sGuaAttrName = "";

        if (UpGuaName.Equals(DownGuaName))//如果上下卦一样，则取单字即可
        {
            sGuaAttrName = UpGuaName;
        }
        else
        {
            sGuaAttrName = GuaSubClass.BeforeGuaSubAttrNames[GuaSubClass.BeforeGuaSubNames.IndexOf(UpGuaName)] + GuaSubClass.BeforeGuaSubAttrNames[GuaSubClass.BeforeGuaSubNames.IndexOf(DownGuaName)];
        }

        GuaClass gc = new GuaClass(sGuaAttrName);
        gc.RawGua = this;
        gc.RawGua.LoadAllYaos();
        return gc;
    }

    /// <summary>
    /// 显式调用：一键完整加载当前复卦的所有断卦外围相关参数（含世应别名、空亡、六神、伏神等）。
    /// </summary>
    /// <param name="DaySL">当日干支对象 <see cref="SkyLoc"/>；若为 <c>null</c> 则不加载六神和空亡。</param>
    public void LoadAllYaos(SkyLoc DaySL = null!)
    {
        if (this.Yaos == null) return;
        SkyClass sc = DaySL == null! ? null! : DaySL.Sky;
        GuaYao.LoadGuaYaos(this, sc);
    }

    /// <summary>
    /// 根据卦的属性或简称返回标准卦全称。
    /// </summary>
    /// <param name="GuaNameOrAttrName">卦的简称或属性（如：天山、乾等）。</param>
    /// <returns>返回对应的标准全称字符串。</returns>
    private static int GetGuaIndexByName(string GuaNameOrAttrName)
    {
        if (string.IsNullOrWhiteSpace(GuaNameOrAttrName)) return -1;
        string sn = GuaNameOrAttrName.Trim();
        int iPos = -1;
        if (sn.Length <= 2)
        {
            iPos = Array.IndexOf(Names, sn);
            if (iPos > -1) return iPos;
        }
        iPos = Array.FindIndex(FullNames, x => x.IndexOf(sn) > -1);
        if (iPos > -1) return iPos;

        return iPos;
    }

    /// <summary>
    /// 静态工厂方法：依据 64 卦对应的 60 甲子干支名称反向检索并创建对应的复卦对象实例。
    /// </summary>
    /// <param name="GuaSkyLocName">甲子干支名称（部分甲子对应两卦，如甲子对应坤为地与地雷复）。</param>
    /// <returns>返回匹配成功的复卦列表 <c>List&lt;GuaClass&gt;</c>。</returns>
    public static List<GuaClass>? GetGuaBySkyLoc(string GuaSkyLocName)
    {
        return GuaSkyLocNames
            .Select((x, index) => new { Name = x, Index = index })
            .Where(x => x.Name.IndexOf(GuaSkyLocName) > -1)
            .Select(x => new GuaClass(x.Index))
            .ToList();
    }

    /// <summary>
    /// 根据 6 爻数值数组返回对应的复卦对象。
    /// </summary>
    /// <param name="iYaos">包含 6 个爻值的整型数组。（0为阴爻、1为阳爻，2为老阴（动爻），3为老阳（动爻）)</param>
    /// <returns>返回匹配的 <see cref="GuaClass"/> 实例；若数组长度不合法则返回 <c>null</c>。</returns>
    public static GuaClass GetGuaClass(int[] iYaos)
    {
        if (iYaos.Length != 6) throw new ArgumentOutOfRangeException(nameof(iYaos));

        GuaSubClass gsc = GuaSubClass.GetGuaSub(iYaos[0] % 2, iYaos[1] % 2, iYaos[2] % 2, true)!;
        if (gsc == null) throw new Exception("根据爻不能找到卦象");
        String sAttrName2 = gsc.AttrName;
        gsc = GuaSubClass.GetGuaSub(iYaos[3] % 2, iYaos[4] % 2, iYaos[5] % 2, false)!;
        if (gsc == null) throw new Exception("根据爻不能找到卦象");
        String sAttrName1 = gsc.AttrName;
        String sAttrName = "";
        if (sAttrName1.Equals(sAttrName2))
        {
            sAttrName = GuaSubClass.AfterGuaSubNames[GuaSubClass.AfterGuaSubAttrNames.IndexOf(sAttrName1)];
        }
        else
        {
            sAttrName = sAttrName1 + sAttrName2;
        }
        GuaClass gc = new GuaClass(sAttrName);
        int[] iPos = iYaos.Select((x, index) => x > 1 ? index : -1).Where(x => x > -1).ToArray();//计算出动爻的位置
        if (iPos.Any()) gc.SetYaoDoing(iPos, false);//设置相关位置为动爻

        return gc;
    }

    /// <summary>
    /// 获得本卦与其他卦的卦气相合类型
    /// </summary>
    /// <param name="WithGua">要计算的卦</param>
    /// <returns></returns>
    public GuaFitType GetGuaQiFit(GuaClass WithGua)
    {
        return GetGuaNumFit(this.GuaQi.GuaQiNumber, WithGua.GuaQi.GuaQiNumber);
    }




    /// <summary>
    /// 获得本卦与其他卦的卦运相合类型
    /// </summary>
    /// <param name="WithGua">要计算的卦象</param>
    /// <returns></returns>
    public GuaFitType GetGuaFateFit(GuaClass WithGua)
    {
        int[] nums = [GuaFateNames.IndexOf(this.GuaFate) + 1, GuaFateNames.IndexOf(WithGua.GuaFate) + 1];

        return GetGuaNumFit(nums[0], nums[1]);
    }

    /// <summary>
    /// 六爻卦合，适合卦气与雌雄正配合，利用上下卦是否相合，卦运则只是数不能用于雌雄正配
    /// </summary>
    /// <param name="Gua1"></param>
    /// <param name="Gua2"></param>
    /// <returns></returns>
    public static GuaFitType GetGuaFit(GuaSubClass Gua1, GuaSubClass Gua2)
    {
        GuaFitType gft = GetGuaNumFit(Gua1.GuaQi.GuaQiNumber, Gua2.GuaQi.GuaQiNumber);//卦气的数合
        gft = gft | GuaLiFit(Gua1, Gua2); //卦理合
        return gft;
    }


    /// <summary>
    /// 卦理合
    /// </summary>
    /// <param name="Gua1">卦1</param>
    /// <param name="Gua2">卦2</param>
    /// <returns></returns>
    public static GuaFitType GuaLiFit(GuaSubClass Gua1, GuaSubClass Gua2)
    {
        string sn = Gua1.Name + Gua2.Name;
        sn += sn; //多一次循环

        string[] sd = ["乾坤", "震巽", "坎离", "艮兑"];//雌雄正配

        bool b = sd.Any(x => sn.IndexOf(x) > -1);
        if (b) return GuaFitType.CiXiong;

        return GuaFitType.None;


    }

    /// <summary>
    /// 卦数合
    /// 卦气与卦运数合静态方法（只传数值），当使用卦气合时直接传卦气值，
    /// 当传卦运时则传索引+1,因无五运卦，数组第五个数留空不影响索引+1
    /// </summary>
    /// <param name="num1"></param>
    /// <param name="num2"></param>
    /// <returns></returns>
    public static GuaFitType GetGuaNumFit(int num1, int num2)
    {

        int[] nums = [num1, num2];

        if ((nums[0] == 1 && nums[1] == 6 || nums[0] == 6 && nums[1] == 1))
        {
            return GuaFitType.GongZong;
        }
        else if (nums[0] == 2 && nums[1] == 7 || nums[0] == 7 && nums[1] == 2)
        {
            return GuaFitType.TongDao;
        }
        else if (nums[0] == 3 && nums[1] == 8 || nums[0] == 8 && nums[1] == 3)
        {
            return GuaFitType.WeiPeng;
        }
        else if (nums[0] == 4 && nums[1] == 9 || nums[0] == 9 && nums[1] == 4)
        {
            return GuaFitType.WeiPeng;
        }
        else if (nums[0] + nums[1] == 5 || nums[0] + nums[1] == 10 || nums[0] + nums[1] == 15)
        {
            return GuaFitType.TongTu;
        }

        return GuaFitType.None;
    }


    #region 显式实现对比、运算符和 Key 方法

    /// <summary>
    /// 判断当前对象是否与指定的对象相等。
    /// </summary>
    /// <param name="obj">要进行比较的目标对象。</param>
    /// <returns>若相等则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
    public override bool Equals(object obj)
    {
        if (obj is not GuaClass other)
        {
            return false;
        }
        return this.Name == other.Name;
    }

    /// <summary>
    /// 判断当前卦对象是否与另一个 <see cref="GuaClass"/> 对象相等。
    /// </summary>
    /// <param name="other">要比较的另一个 <see cref="GuaClass"/> 实例。</param>
    /// <returns>若卦名完全一致则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
    bool IEquatable<GuaClass>.Equals(GuaClass other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return string.Equals(this.Name, other.Name, StringComparison.Ordinal);
    }

    /// <summary>
    /// 获取当前对象的哈希码。
    /// </summary>
    /// <returns>返回基于卦名计算的哈希整数。</returns>
    public override int GetHashCode()
    {
        return Name != null ? Name.GetHashCode() : 0;
    }

    /// <summary>
    /// 检查两个 <see cref="GuaClass"/> 实例是否相等。
    /// </summary>
    /// <param name="left">左侧操作数。</param>
    /// <param name="right">右侧操作数。</param>
    /// <returns>若相等返回 <c>true</c>，否则返回 <c>false</c>。</returns>
    public static bool operator ==(GuaClass left, GuaClass right)
    {
        if (left is null) return right is null;
        return ((IEquatable<GuaClass>)left).Equals(right);
    }

    /// <summary>
    /// 检查两个 <see cref="GuaClass"/> 实例是否不相等。
    /// </summary>
    /// <param name="left">左侧操作数。</param>
    /// <param name="right">右侧操作数。</param>
    /// <returns>若不相等返回 <c>true</c>，否则返回 <c>false</c>。</returns>
    public static bool operator !=(GuaClass left, GuaClass right)
    {
        return !(left == right);
    }

    #endregion

    #endregion






}


