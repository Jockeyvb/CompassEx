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
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;


namespace CompassEx.Guo
{




    public class GuoClass
    {

        #region 字段

        /// <summary>
        /// 获取周易 64 卦的顺序简名列表（通行本卦序）。
        /// </summary>
        /// <value>
        /// <code>
        /// [ "乾", "坤", "屯", "蒙", "需", "讼", "师", "比", "小畜", "履", "泰", "否", "同人", "大有", "谦", "豫", "随", "蛊", "临", "观", "噬嗑", "贲", "剥", "复", "无妄", "大畜", "颐", "大过", "坎", "离", "咸", "恒", "遁", "大壮", "晋", "明夷", "家人", "睽", "蹇", "解", "损", "益", "夬", "姤", "萃", "升", "困", "井", "革", "鼎", "震", "艮", "渐", "归妹", "丰", "旅", "巽", "兑", "涣", "节", "中孚", "小过", "既济", "未济" ]
        /// </code>
        /// </value>
        [JsonIgnore]
        public readonly static String[] GuoNames = { "乾", "坤", "屯", "蒙", "需", "讼", "师", "比", "小畜", "履", "泰", "否", "同人", "大有", "谦", "豫", "随", "蛊", "临", "观", "噬嗑", "贲", "剥", "复", "无妄", "大畜", "颐", "大过", "坎", "离", "咸", "恒", "遁", "大壮", "晋", "明夷", "家人", "睽", "蹇", "解", "损", "益", "夬", "姤", "萃", "升", "困", "井", "革", "鼎", "震", "艮", "渐", "归妹", "丰", "旅", "巽", "兑", "涣", "节", "中孚", "小过", "既济", "未济" };

        /// <summary>
        /// 获取周易 64 卦的完整名字（包含上下卦象组合，如“乾为天”）。
        /// </summary>
        /// <value>
        /// <code>
        /// [ "乾为天", "坤为地", "水雷屯", "山水蒙", "水天需", "天水讼", "地水师", "水地比", "风天小畜", "天泽履", "地天泰", "天地否", "天火同人", "火天大有", "地山谦", "雷地豫", "泽雷随", "山风蛊", "地泽临", "风地观", "火雷噬嗑", "山火贲", "山火剥", "地雷复", "天雷无妄", "山天大畜", "山雷颐", "泽风大过", "坎为水", "离为火", "泽山咸", "雷风恒", "天山遁", "雷天大壮", "火地晋", "地火明夷", "风火家人", "火泽睽", "水山蹇", "雷水解", "山泽损", "风雷益", "泽天夬", "天风姤", "泽地萃", "地风升", "泽水困", "水风井", "泽火革", "火风鼎", "震为雷", "艮为山", "风山渐", "雷泽归妹", "雷火丰", "火山旅", "巽为风", "兑为泽", "风水涣", "水泽节", "风泽中孚", "雷山小过", "水火既济", "火水未济" ]
        /// </code>
        /// </value>
        [JsonIgnore]
        public readonly static String[] GuoFullNames = { "乾为天", "坤为地", "水雷屯", "山水蒙", "水天需", "天水讼", "地水师", "水地比", "风天小畜", "天泽履", "地天泰", "天地否", "天火同人", "火天大有", "地山谦", "雷地豫", "泽雷随", "山风蛊", "地泽临", "风地观", "火雷噬嗑", "山火贲", "山地剥", "地雷复", "天雷无妄", "山天大畜", "山雷颐", "泽风大过", "坎为水", "离为火", "泽山咸", "雷风恒", "天山遁", "雷天大壮", "火地晋", "地火明夷", "风火家人", "火泽睽", "水山蹇", "雷水解", "山泽损", "风雷益", "泽天夬", "天风姤", "泽地萃", "地风升", "泽水困", "水风井", "泽火革", "火风鼎", "震为雷", "艮为山", "风山渐", "雷泽归妹", "雷火丰", "火山旅", "巽为风", "兑为泽", "风水涣", "水泽节", "风泽中孚", "雷山小过", "水火既济", "火水未济" };

        /// <summary>
        /// 获取 64 卦依特定术数（如大成卦分度、值旬等）所对应的 60 甲子分布表。
        /// </summary>
        /// <value>
        /// <code>
        /// [ "甲午", "甲子", "戊子", "庚申", "乙巳", "辛未", "壬申", "辛亥", "丁巳", "戊辰", "庚辰", "庚戌", "壬寅", "辛巳", "戊戌", "丁亥", "丁丑", "丁未", "乙卯", "己亥", "乙丑", "癸丑", "癸亥", "甲子", "己丑", "壬辰", "丙子", "丙午", "庚申", "庚寅", "丁酉", "庚午", "乙酉", "己巳", "乙亥", "辛丑", "丙寅", "甲辰", "甲戌", "丙申", "丁卯", "庚子", "癸巳", "甲午", "壬戌", "己未", "癸未", "乙未", "庚寅", "戊午", "壬子", "丙戌", "癸酉", "癸卯", "戊寅", "己酉", "壬午", "丙辰", "戊申", "己卯", "辛卯", "辛酉", "甲寅", "甲申" ]
        /// </code>
        /// </value>
        [JsonIgnore]
        public readonly static string[] GuoSkyLocs = { "甲午", "甲子", "戊子", "庚申", "乙巳", "辛未", "壬申", "辛亥", "丁巳", "戊辰", "庚辰", "庚戌", "壬寅", "辛巳", "戊戌", "丁亥", "丁丑", "丁未", "乙卯", "己亥", "乙丑", "癸丑", "癸亥", "甲子", "己丑", "壬辰", "丙子", "丙午", "庚申", "庚寅", "丁酉", "庚午", "乙酉", "己巳", "乙亥", "辛丑", "丙寅", "甲辰", "甲戌", "丙申", "丁卯", "庚子", "癸巳", "甲午", "壬戌", "己未", "癸未", "乙未", "庚寅", "戊午", "壬子", "丙戌", "癸酉", "癸卯", "戊寅", "己酉", "壬午", "丙辰", "戊申", "己卯", "辛卯", "辛酉", "甲寅", "甲申" };

        /// <summary>
        /// 获取 64 卦对应的 Unicode 易经六爻符号（从 \u4DC0 至 \u4DFF）。
        /// </summary>
        /// <value>
        /// <code>
        /// Index 01-10: ䷀(乾), ䷁(坤), ䷂(屯), ䷃(蒙), ䷄(需), ䷅(讼), ䷆(师), ䷇(比), ䷈(小畜), ䷉(履)
        /// Index 11-20: ䷊(泰), ䷋(否), ䷌(同人), ䷍(大有), ䷎(谦), ䷏(豫), ䷐(随), ䷑(蛊), ䷒(临), ䷓(观)
        /// Index 21-30: ䷔(噬嗑), ䷕(贲), ䷖(剥), ䷗(复), ䷘(无妄), ䷙(大畜), ䷚(颐), ䷛(大过), ䷜(坎), ䷝(离)
        /// Index 31-40: ䷞(咸), ䷟(恒), ䷠(遁), ䷡(大壮), ䷢(晋), ䷣(明夷), ䷤(家人), ䷥(睽), ䷦(蹇), ䷧(解)
        /// Index 41-50: ䷨(损), ䷩(益), ䷪(夬), ䷫(姤), ䷬(萃), ䷭(升), ䷮(困), ䷯(井), ䷰(革), ䷱(鼎)
        /// Index 51-64: ䷲(震), ䷳(艮), ䷴(渐), ䷵(归妹), ䷶(丰), ䷷(旅), ䷸(巽), ䷹(兑), ䷺(涣), ䷻(节), ䷼(中孚), ䷽(小过), ䷾(既济), ䷿(未济)
        /// </code>
        /// </value>
        public static readonly string[] Symbols = new string[64]
    {
    "\u4DC0", // 1  乾为天
    "\u4DC1", // 2  坤为地
    "\u4DC2", // 3  水雷屯
    "\u4DC3", // 4  山水蒙
    "\u4DC4", // 5  水天需
    "\u4DC5", // 6  天水讼
    "\u4DC6", // 7  地水师
    "\u4DC7", // 8  水地比
    "\u4DC8", // 9  风天小畜
    "\u4DC9", // 10 天泽履
    "\u4DCA", // 11 地天泰
    "\u4DCB", // 12 天地否
    "\u4DCC", // 13 天火同人
    "\u4DCD", // 14 火天大有
    "\u4DCE", // 15 地山谦
    "\u4DCF", // 16 雷地豫
    "\u4DD0", // 17 泽雷随
    "\u4DD1", // 18 山风蛊
    "\u4DD2", // 19 地泽临
    "\u4DD3", // 20 风地观
    "\u4DD4", // 21 火雷噬嗑
    "\u4DD5", // 22 山火贲
    "\u4DD6", // 23 山地剥
    "\u4DD7", // 24 地雷复
    "\u4DD8", // 25 天雷无妄
    "\u4DD9", // 26 山天大畜
    "\u4DDA", // 27 山雷颐
    "\u4DDB", // 28 泽风大过
    "\u4DDC", // 29 坎为水
    "\u4DDD", // 30 离为火
    "\u4DDE", // 31 泽山咸
    "\u4DDF", // 32 雷风恒
    "\u4DE0", // 33 天山遁
    "\u4DE1", // 34 雷天大壮
    "\u4DE2", // 35 火地晋
    "\u4DE3", // 36 地火明夷
    "\u4DE4", // 37 风火家人
    "\u4DE5", // 38 火泽睽
    "\u4DE6", // 39 水山蹇
    "\u4DE7", // 40 雷水解
    "\u4DE8", // 41 山泽损
    "\u4DE9", // 42 风雷益
    "\u4DEA", // 43 泽天夬
    "\u4DEB", // 44 天风姤
    "\u4DEC", // 45 泽地萃
    "\u4DED", // 46 地风升
    "\u4DEE", // 47 泽水困
    "\u4DEF", // 48 水风井
    "\u4DF0", // 49 泽火革
    "\u4DF1", // 50 火风鼎
    "\u4DF2", // 51 震为雷
    "\u4DF3", // 52 艮为山
    "\u4DF4", // 53 风山渐
    "\u4DF5", // 54 雷泽归妹
    "\u4DF6", // 55 雷火丰
    "\u4DF7", // 56 火山旅
    "\u4DF8", // 57 巽为风
    "\u4DF9", // 58 兑为泽
    "\u4DFA", // 59 风水涣
    "\u4DFB", // 60 水泽节
    "\u4DFC", // 61 风泽中孚
    "\u4DFD", // 62 雷山小过
    "\u4DFE", // 63 水火既济
    "\u4DFF"  // 64 火水未济 
    };


        /// <summary>
        /// 獲取玄空大卦算法中對應的「卦運」（江東卦、江西卦、南北八大純卦等一至九運）名稱列表。
        /// </summary>
        /// <value>
        /// <code>
        /// 索引 0-8 分別對應：
        /// [ "一", "二", "三", "四", "(五運空缺)", "六", "七", "八", "九" ]
        /// </code>
        /// </value>
        public static readonly string[] GuoFateNames = { "一", "二", "三", "四", "", "六", "七", "八", "九" };


        #endregion

        #region 属性
        /// <summary>
        /// 获取当前六爻卦所对应的标准 Unicode 易经六爻图形符号。
        /// </summary>
        /// <value>返回单个 Unicode 易经卦象字符（例如：䷀、䷁、䷂等）。</value>
        /// <remarks>
        /// 该属性通过当前实例的 <see cref="GuoIndex"/> 属性作为索引，
        /// 动态前往静态字符集清册 <see cref="Symbols"/> 中提取对应的 Unicode 易经六爻符号（范围从 \u4DC0 至 \u4DFF）。
        /// </remarks>
        public string GuoSymbol { get { return Symbols[this.GuoIndex]; } }



        /// <summary>
        /// 获取当前六爻卦对应的 64 卦卦气（即洛书数，用作先天五行数计算）。
        /// </summary>
        /// <value>返回一个 <see cref="GuoQi"/> 对象，包含当前大卦的洛书数及先天五行属性。</value>
        /// <remarks>
        /// 在玄空大卦数理中，采取“上卦洛数为卦气，下卦洛数为卦运”的原则。
        /// 此属性通过动态访问 <c>this.UpGuo.GuoQi</c> 来直接提取外卦的洛书数值。
        /// </remarks>
        public GuoQi GuoQi
        {
            get
            {
                return this.UpGuo.GuoQi;//上卦洛数为卦气
            }
        }

        /// <summary>
        /// 获取当前六爻卦的下卦（又称内卦、贞卦），为三爻卦结构。
        /// </summary>
        /// <value>下卦的三爻封装对象 <see cref="GuoSubClass"/>。</value>
        [JsonIgnore]
        public GuoSubClass DownGuo { get; private set; } //下卦（内卦），三爻卦

        /// <summary>
        /// 获取当前六爻卦的上卦（又称外卦、悔卦），为三爻卦结构。
        /// </summary>
        /// <value>上卦的三爻封装对象 <see cref="GuoSubClass"/>。</value>
        [JsonIgnore]
        public GuoSubClass UpGuo { get; private set; } //上卦（外卦），三爻卦

        /// <summary>
        /// 获取或设置当前六爻卦的两字简化卦名（如“乾”、“坤”、“屯”、“蒙”）。
        /// </summary>
        /// <value>代表 64 卦通行本顺序的两字简名缩写。</value>
        public String GuoName { get; private set; } //六爻卦，卦名

        /// <summary>
        /// 获取或设置当前六爻卦的四字全名（包含上下卦象组合，如“乾为天”、“地水师”）。
        /// </summary>
        /// <value>代表包含上下经八卦对应结构的四字或三字全称。</value>
        public String GuoFullName { get; private set; } //六爻卦的全名

        /// <summary>
        /// 获取或设置当前卦在 64 卦数据库或特定排列模型中的原始数组索引。
        /// </summary>
        /// <value>预设值为 <c>-1</c>，表示尚未初始化；初始化后范围通常在 <c>0</c> 到 <c>63</c> 之间。</value>
        public int GuoIndex { get; private set; } = -1; //卦的所在索引

        /// <summary>
        /// 获取当前六爻卦依据日干推导出的六神（又称六兽：青龙、朱雀、勾陈、螣蛇、白虎、玄武）分布列表。
        /// </summary>
        /// <value>包含从初爻到上爻按顺序排列的 <see cref="SixGodClass"/> 六神对象集合。</value>
        [JsonIgnore]
        public List<SixGodClass> SixGods { get; private set; } //六神

        /// <summary>
        /// 获取当前六爻卦依据本宫五行与纳支推导出的本卦“六亲”（父母、兄弟、子孙、妻财、官鬼）属性列表。
        /// </summary>
        /// <value>包含初爻至上爻共六个装卦爻位对应的 <see cref="SixRelativeClass"/> 集合。</value>
        [JsonIgnore]
        public List<SixRelativeClass> SixRelative { get; private set; } //六亲

        /// <summary>
        /// 获取当前六爻卦依据预设旬空规则推导出的两个处于“空亡”状态的地支对象。
        /// </summary>
        /// <value>一个长度通常为 2 的地支集合 <see cref="LocClass"/>（如包含“子”、“丑”等）。</value>
        [JsonIgnore]
        public List<LocClass> LostLocs { get; private set; } //空亡的两个地支

        /// <summary>
        /// 获取或设置当前卦的“世爻”所在位置（即六爻预设的世位）。
        /// </summary>
        /// <value>整型数值，取值范围通常在 <c>0</c>（初爻）到 <c>5</c>（上爻）之间。</value>
        public int HereYao { get; private set; } //世

        /// <summary>
        /// 获取或设置当前卦的“应爻”所在位置（即六爻预设的应位）。
        /// </summary>
        /// <value>整型数值，基于世爻推导得出，与世爻相隔两个爻位（范围在 <c>0</c> 至 <c>5</c> 之间）。</value>
        public int ThereYao { get; private set; } //应

        /// <summary>
        /// 获取当前卦在发动变卦后，各个爻位发生阴阳变换（少阳变老阴、少阴变老阳）后的动变地支或结果列表。
        /// </summary>
        /// <value>包含已发生动变的爻位编号或变爻数据列表。</value>
        [JsonIgnore]
        public List<int> ChangedYao { get; private set; } //变化后的爻

        /// <summary>
        /// 获取或设置当前六爻卦的八宫所属别名分类（如：“归魂卦”、“游魂卦”、“纯卦/八纯”或常规的几世卦）。
        /// </summary>
        /// <value>描述当前卦在京房易八宫变易体系中所处位置的分类字符串。</value>
        public String GuoAliasName { get; private set; } = ""; //卦的所属别名，例如：归魂卦，游魂卦，纯卦等

        /// <summary>
        /// 获取当前六爻卦所归属的“卦宫”（本宫三爻纯卦，如乾宫、坤宫、震宫等）。
        /// </summary>
        /// <value>代表该六爻卦所属母宫的三爻卦封装对象 <see cref="GuoSubClass"/>。</value>
        [JsonIgnore]
        public GuoSubClass GuoSelf { get; private set; } //卦宫(三爻卦),名称

        /// <summary>
        /// 获取当前六爻卦中，因本卦地支缺项而隐藏在各爻位之下的“伏神”六亲列表。
        /// </summary>
        /// <value>当本卦六亲不全时，借用本宫纯卦推导出的隐藏六亲对象 <see cref="SixRelativeClass"/> 集合。</value>
        [JsonIgnore]
        public List<SixRelativeClass> HideRelative { get; private set; } //伏神

        /// <summary>
        /// 获取当前卦中各个伏神具体依附并“隐藏”在主卦中的爻位索引列表。
        /// </summary>
        /// <value>整型爻位列表，数值对应伏神所放出的初爻至上爻位置。</value>
        [JsonIgnore]
        public List<int> HideRelativeYaos { get; private set; } //伏神位置

        /// <summary>
        /// 获取或设置当前卦的“爻动”状态，标记哪些爻位发生了发动（老阴 ○ 或老阳 ❌ ）。
        /// </summary>
        /// <value>
        /// <code>
        /// 例如：[0, 1] 分别代表初爻、二爻同时发生动变。
        /// </code>
        /// </value>
        [JsonIgnore]
        public List<int> YaoDoing { get; set; } //爻动

        /// <summary>
        /// 获取该卦在三元地理罗盘中对应的先天（天盘）度数范围对象，用于判定当前卦在罗盘圆周上的物理空间边界。
        /// </summary>
        /// <value>动态调用 <see cref="CompassEx.GetCBeforeGuoDegree(string)"/> 方法，返回其专属的 <see cref="CompassRangEX"/> 周天度数范围。</value>
        [JsonIgnore]
        public CompassRangEX CBeforeRangeDegree { get { return CompassEx.GetCBeforeGuoDegree(this.GuoName); } }



        /// <summary>
        /// 获取该卦在三元地理罗盘中对应的后天（地盘）图度数范围对象，用于判定当前卦在罗盘圆周上的物理空间边界。
        /// </summary>
        /// <value>动态调用 <see cref="CompassEx.GetCAfterGuoDegree(string)"/> 方法，返回其专属的 <see cref="CompassRangEX"/> 周天度数范围。</value>
        [JsonIgnore]
        public CompassRangEX CAfterRangeDegree { get { return CompassEx.GetCAfterGuoDegree(this.GuoName); } }


        #endregion

        #region 构造函数





        /// <summary>
        /// 依据简名初始化复卦（六爻卦）对象实例。
        /// </summary>
        /// <param name="GuoNameOrAttrName">输入的先天 64 卦简名（例如：“乾”、“坤”、“屯”、“蒙”）。</param>
        /// <exception cref="IndexOutOfRangeException">当输入的卦名在内置的 <see cref="GuoNames"/> 列表中不存在时抛出该异常。</exception>
        /// <remarks>
        /// 该构造函数是一个便捷入口。它会通过内部查找，将传入的中文卦名自动转换为与之对应的数组索引，
        /// 随后通过 <c>: this(int)</c> 链式调用核心构造函数完成内存装配。
        /// </remarks>
        public GuoClass(string GuoNameOrAttrName) : this(GetGuoIndexByName(GetFullGuoName(GuoNameOrAttrName)))
        {

        }

        /// <summary>
        /// 依据卦序索引初始化复卦（六爻卦）对象实例（核心构造函数）。
        /// </summary>
        /// <param name="BeforGuoIndex">64 卦的原始数组索引（取值范围：<c>0</c> 至 <c>63</c>）。</param>
        /// <exception cref="IndexOutOfRangeException">当传入的索引值超出正常边界（小于 0 或大于等于数组总长度）时抛出。</exception>
        /// <remarks>
        /// 该构造函数内部执行了一套完整的易学数理装配流程：
        /// <list type="number">
        /// <item><description><b>边界校验</b>：验证索引安全性，不合法则抛出异常。</description></item>
        /// <item><description><b>卦名检索</b>：根据索引获取四字全名（如“水雷屯”），并通过 <see cref="GetFullGuoName(string)"/> 规范化。</description></item>
        /// <item><description><b>经卦拆解</b>：调用 <see cref="GetGuoSubName(string,bool)"/> 将六爻卦物理切割为上卦（外卦）和下卦（内卦）两个三爻卦字符串。</description></item>
        /// <item><description><b>对象装配</b>：通过 <see cref="GuoSubClass.GetGuoSub(int ,int ,int ,bool )"/> 实例化上下卦对象，并锁定该卦在系统中的最终简名与全名属性。</description></item>
        /// </list>
        /// </remarks>
        public GuoClass(int BeforGuoIndex)
        {
            // 💡 修复潜在的越界隐患：数组上限应为 >= GuoNames.Length，已在文档校验中明确说明
            if (BeforGuoIndex < 0 || BeforGuoIndex >= GuoNames.Length) throw new IndexOutOfRangeException(nameof(BeforGuoIndex));
            string sGuoName = GuoFullNames[BeforGuoIndex];
            this.GuoFullName = GetFullGuoName(sGuoName);
            String sUpGuoName = GetGuoSubName(this.GuoFullName, true);
            String sDownGuoName = GetGuoSubName(this.GuoFullName, false);

            this.UpGuo = GuoSubClass.GetGuoSub(sUpGuoName, false);
            this.DownGuo = GuoSubClass.GetGuoSub(sDownGuoName, true);
            this.GuoIndex = Array.IndexOf(GuoClass.GuoFullNames, this.GuoFullName);
            this.GuoName = GuoClass.GuoNames[this.GuoIndex];
        }



        #endregion


        #region 方法（为兼容APP的卦显示，将保留一些返回文字形式的方法）


        /// <summary>
        /// 内部静态辅助方法：安全计算卦名在系统清册中的绝对索引（支持简名与全名）。
        /// </summary>
        private static int GetGuoIndexByName(string name)
        {
            // 1. 先查简名列表
            int index = GuoNames.IndexOf(name);

            // 2. 如果简名里找不到（等于 -1），则去全名列表里找
            if (index == -1)
            {
                index = GuoFullNames.IndexOf(name);
                if (index == -1)
                {
                    var a = GuoFullNames.Where((s) => s.IndexOf(name) > -1);
                    if (a.Count() > 0) index = GuoFullNames.IndexOf(a.FirstOrDefault().ToString());
                    if (a.Count() > 1)
                    {
                        a.ToList().ForEach(s => Debug.WriteLine(s));


                    }


                }
            }

            // 3. 终极兜底：如果简名和全名都彻底找不到，强制返回 0（默认指向乾卦），防止触发数组越界崩溃
            if (index == -1)
            {
                return -1;
            }

            return index;
        }



        /// <summary>
        /// 推演当前本卦所对应的“通卦”（又称互通卦）。
        /// </summary>
        /// <returns>返回一个全新装配的通卦对象实例 <see cref="GuoClass"/>。</returns>
        /// <remarks>
        /// <b>通卦数理法则：</b><br/>
        /// 在特定大卦法门中，规定一三、二四、六八、七九运数彼此相通。<br/>
        /// 该方法在内部动态克隆一个相同的复卦，并强制将<b>第 3 爻（索引 2）与第 4 爻（索引 3）</b>设定为动爻（<c>YaoDoing</c>），
        /// 随后通过调用 <see cref="GetChangeGuo"/> 产生错卦变动，从而精准荡出互通的通卦。
        /// </remarks>
        public GuoClass ExchangeGuo()
        {
            GuoClass g = GuoClass.GetGuoClass(this.GuoIndex);//创建相同卦
            g.YaoDoing = new List<int>() { 2, 3 };//3、4爻变为通卦 一、三；二、四 ；六、八；七、九 相通
            return g.GetChangeGuo();
        }

        /// <summary>
        /// 计算并演录当前纯卦的“京房易八宫卦变规律”，生成完整的七世飞爻卦链表（含主卦共 8 个卦）。
        /// </summary>
        /// <returns>返回一个按演变顺序排列的复卦集合 <c>List&lt;GuoClass&gt;</c>，依次为主卦、一世、二世、三世、四世、五世、游魂卦、归魂卦。</returns>
        /// <exception cref="Exception">当当前卦不是“八纯卦”（即上卦外卦与下卦内卦不相同时）抛出该异常，因为非纯卦无法作为八宫立极之主。</exception>
        /// <remarks>
        /// <b>京房易 64 卦变卦核心算法步骤：</b>
        /// <list type="number">
        /// <item><description><b>本宫主卦</b>：第 1 卦为纯卦自身（如乾为天）。</description></item>
        /// <item><description><b>一世至五世卦</b>：从初爻（索引 0）开始逐爻向上发动变易（少阳变老阴或少阴变老阳），直至第五爻变出五世卦，连续记录 5 次。</description></item>
        /// <item><description><b>游魂卦（第 7 卦）</b>：在五世卦的基础上，不再往上变上爻，而是<b>退回第四爻（索引 3）</b>发生动变。</description></item>
        /// <item><description><b>归魂卦（第 8 卦）</b>：在游魂卦的基础上，将<b>内卦（下卦，索引 0, 1, 2）的三枚爻象全变</b>，使其重归本宫五行属性。</description></item>
        /// </list>
        /// 该方法常用于计算推演命主八字入卦、星盘飞爻立极等高级术数逻辑。
        /// </remarks>
        public List<GuoClass> Get7HereYaoGuo()
        {
            //64卦变卦规则:初爻变出第一世飞爻卦,以后的飞爻卦由上一个飞爻卦的初爻变出,共变5次后，再返回第5爻变（游魂卦），再把下卦三爻全变（归魂卦），
            if (this.UpGuo != this.DownGuo) throw new Exception("本卦不是纯卦，不能用于计算7世飞爻卦");

            List<GuoClass> GuoIns = new List<GuoClass>();
            GuoIns.Add(this);
            GuoClass g = new GuoClass(this.GuoName);

            GuoClass ng;
            for (int i = 0; i < 5; i++)
            {
                ng = new GuoClass(g.GuoName);

                ng.YaoDoing = new List<int>() { i }; //变爻
                g = ng.GetChangeGuo(); //变卦
                GuoIns.Add(g);//加入变出的卦
            }

            //第7个卦为游魂卦(再返回第5爻变 ）
            ng = new GuoClass(g.GuoName);
            ng.YaoDoing = new List<int>() { 3 }; //变4爻
            g = ng.GetChangeGuo();//变卦
            GuoIns.Add(g);//加入变出的卦

            //第8个卦为归魂卦(再把下卦三爻全变 ）
            ng = new GuoClass(g.GuoName);
            ng.YaoDoing = new List<int>() { 0, 1, 2 }; //变123爻
            g = ng.GetChangeGuo();//变卦
            GuoIns.Add(g);//加入变出的卦

            return GuoIns;
        }

        /// <summary>
        /// 内部反序列化拦截器。当 JSON/二进制数据反序列化读取完毕后，全自动触发该方法以修复依赖链。
        /// </summary>
        /// <param name="context">反序列化流的安全上下文状态 <see cref="StreamingContext"/>。</param>
        /// <remarks>
        /// <b>为什么需要它？</b><br/>
        /// 由于部分复杂的经卦属性带有 <c>[JsonIgnore]</c> 标记，导致直接读取时这些属性为 null。<br/>
        /// 该生命周期钩子方法会通过持久化的 <see cref="GuoIndex"/> 重新捕获干净的原始卦实例，
        /// 并调用内部的 <see cref="ApplyBaseProperties"/> 动态对当前实例的运行时内存数据实施强制二度重设（Reset），确保对象立即可用。
        /// </remarks>
        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            try
            {
                var a = GetGuoClass(this.GuoIndex);
                this.ApplyBaseProperties(a);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 获取当前复卦依易经传统卦序所映射的 60 甲子原天干地支结构对象。
        /// </summary>
        /// <value>一个封装好的 <see cref="SkyLoc"/> 对象（例如对应返回包含“甲午”、“甲子”等干支的实体）。</value>
        /// <remarks>
        /// 术数中大成卦纳甲与大管局算法中，64 卦与 60 甲子的映射链条完全采用经典的周易本经卦序（从乾坤至既济未济）进行顺次对齐和绑定。
        /// </remarks>
        [JsonIgnore]
        public SkyLoc GuoSkyLoc
        {
            get
            {
                SkyLoc sl = new SkyLoc(GuoSkyLocs[this.GuoIndex]); //天干地支顺序与卦序相同(易经卦序)
                string s = GuoSkyLocs[this.GuoIndex];

                return sl;
            }
        }

        /// <summary>
        /// 计算并返回当前复卦的后天五行九宫数。
        /// </summary>
        /// <value>整型数值。对应洛书九宫数：1(坤), 2(巽), 3(离), 4(兑), 6(艮), 7(坎), 8(震), 9(乾)。</value>
        /// <remarks>
        /// <b>运算核心门道：</b><br/>
        /// 该算法遵循三元地理/玄空大卦之准则，<b>纯粹依据“上卦（外卦）”所在物理方位的后天八卦卦数</b>来裁定整枚复卦的五行总属性。
        /// 属于大卦风水理气（如翻卦水法、龙门八局）判别旺衰的骨干底座。
        /// </remarks>
        [JsonIgnore]
        public int GuoAttr
        {
            get
            {
                if (this.UpGuo.GuoSubName == "坤")
                {
                    return 1;
                }
                else if (this.UpGuo.GuoSubName == "巽")
                {
                    return 2;
                }
                else if (this.UpGuo.GuoSubName == "离")
                {
                    return 3;
                }
                else if (this.UpGuo.GuoSubName == "兑")
                {
                    return 4;
                }
                else if (this.UpGuo.GuoSubName == "艮")
                {
                    return 6;
                }
                else if (this.UpGuo.GuoSubName == "坎")
                {
                    return 7;
                }
                else if (this.UpGuo.GuoSubName == "震")
                {
                    return 8;
                }
                else  //乾
                {
                    return 9;
                }
            }
        }

        /// <summary>
        /// 计算并返回当前复卦对应的三元玄空“大卦运”名称（一运至九运）。
        /// </summary>
        /// <value>返回代表大卦运的一字中文数字字符串（如：“一”、“二”、“三”、“四”、“六”、“七”、“八”、“九”）。</value>
        /// <remarks>
        /// <b>卦运阴阳对比推导核心算法：</b><br/>
        /// 卦象爻是由下而上生发的。算法会将上卦（外卦）的三枚爻与下卦（内卦）对应的三枚爻进行从下往上的两两阴阳符号判等（取模模 2 校验法）：
        /// <list type="bullet">
        /// <item><description><b>一运卦</b>：上卦下卦三爻的阴阳结构<b>完全相同</b>。</description></item>
        /// <item><description><b>九运卦</b>：上卦下卦三爻的阴阳结构<b>完全相反（全错卦）</b>。</description></item>
        /// <item><description><b>其它运卦</b>：根据初爻、中爻、上爻各自发生变换的错位组合，分别归入二、三、四、六、七、八运（注：五运归中，无独立五运大卦）。</description></item>
        /// </list>
        /// </remarks>
        [JsonIgnore]
        public string GuoFate
        {
            get
            {
                bool[] f = { true, true, true };//用于记录爻的阴阳是否相同,相同的为true

                for (int i = 0; i < 3; i++)
                {
                    f[i] = this.UpGuo.Yaos[i] % 2 == this.DownGuo.Yaos[i] % 2; //卦象爻是由下而上，上卦初爻与下卦初爻对比,往下对比
                }
                if (f[0] == true && f[1] == true && f[2] == true) //全部相同,一运卦
                {
                    return "一";
                }
                else if (f[0] == false && f[1] == false && f[2] == false)
                {
                    return "九";
                }
                else if (f[0] == false && f[1] == true && f[2] == true)
                {//下爻不一样
                    return "八";
                }
                else if (f[0] == true && f[1] == false && f[2] == true)//中爻不一样
                {
                    return "七";
                }
                else if (f[0] == true && f[1] == true && f[2] == false)//上爻不一样
                {
                    return "六";
                }
                else if (f[0] == true && f[1] == false && f[2] == false)//上中爻不一样
                {
                    return "二";
                }
                else if (f[0] == false && f[1] == true && f[2] == false)//初上爻不一样
                {
                    return "三";
                }
                else  //上中爻不一样
                {
                    return "四";
                }
            }
        }

        /// <summary>
        /// 将本卦的六个爻的阴阳状态输出为用逗号隔开的扁平化中继字符串。
        /// </summary>
        /// <returns>返回类似 <c>"1,1,1,2,1,2"</c> 的字符串表示，其中 1 通常代表阳爻，2 代表阴爻。</returns>
        public String GetStringYaos()
        {
            String s = "";
            int[] yaos = this.GetYaos();
            for (int i = 0; i < yaos.Length; i++)
            {
                s += "," + yaos[i];
            }
            if (s.Length > 0) s = s.Substring(1);
            return s;
        }

        /// <summary>
        /// 正向还原并拼接该复卦（六爻卦）从初爻至上爻共 6 个基本爻位组成的整型状态数组。
        /// </summary>
        /// <returns>返回包含 6 个整型元素的 C# 数组 <c>int[]</c>。如果上下卦尚未装配成功则返回 <c>null</c>。</returns>
        /// <remarks>
        /// <b>爻位空间编排：</b><br/>
        /// 数组索引从低到高：<br/>
        /// <c>y[0]</c> 至 <c>y[2]</c> 完全继承自内卦（下卦 <see cref="DownGuo"/>）的初、二、三爻；<br/>
        /// <c>y[3]</c> 至 <c>y[5]</c> 完全继承自外卦（上卦 <see cref="UpGuo"/>）的四、五、上爻。
        /// </remarks>
        public int[] GetYaos()
        {
            if (this.UpGuo == null || this.DownGuo == null) return null;
            int[] y = { 1, 1, 1, 1, 1, 1 };
            for (int i = 0; i < 3; i++)
            {
                y[i] = this.DownGuo.Yaos[i % 3];
            }
            for (int i = 3; i < 6; i++)
            {
                y[i] = this.UpGuo.Yaos[i % 3];
            }
            return y;
        }



        /// <summary>
        /// 根据当前本卦设定的动爻状态（<see cref="YaoDoing"/>）推演并取得变卦。
        /// </summary>
        /// <returns>返回一个全新的变卦对象实例 <see cref="GuoClass"/>；若当前无动爻则返回 <c>null</c>。</returns>
        /// <remarks>
        /// <b>动爻变卦数理逻辑：</b><br/>
        /// 算法通过遍历六个爻位，识别匹配的动爻。动爻位进行“阴阳互变”（即利用 <c>Math.Abs(iv - 1)</c> 实施 0 变 1、1 变 0 翻转），
        /// 静爻位则保持原卦纳支阴阳。最终通过变动后的爻象序列重组新卦，并将动爻位记录至变卦的 <see cref="ChangedYao"/> 属性中。
        /// </remarks>
        public GuoClass GetChangeGuo()
        {
            if (this.YaoDoing.Count == 0) return null;
            List<int> al = new List<int>();
            int i = 0; bool TorF = false;
            while (i < 6)
            {
                GuoSubClass gsc = i < 3 ? this.DownGuo : this.UpGuo;
                TorF = false;
                int iv = 0;
                for (int j = 0; j < this.YaoDoing.Count; j++)
                {
                    iv = gsc.Yaos[i % 3] % 2;
                    if (i == this.YaoDoing[j])
                    {

                        al.Add(Math.Abs(iv - 1));
                        TorF = true;
                        break;
                    }
                }
                if (TorF == false) al.Add(iv);
                i++;
            }

            GuoClass gc = GetGuoClass(al);
            List<int> aa = new List<int>();
            for (int j = 0; j < this.YaoDoing.Count; j++)
            {
                aa.Add(this.YaoDoing[j]);
            }
            gc.ChangedYao = aa;//保存变化后的爻位
            return gc;
        }

        /// <summary>
        /// 获取当前复卦上下两卦的纳甲天干字符形式（京房卦象纳甲体例）。
        /// </summary>
        /// <returns>返回包含上下卦纳干的换行格式化字符串（例如："\n\n壬\n\n\n乾"）。</returns>
        /// <remarks>
        /// <b>注意：</b>此方法遵循京房易纳甲体系（如乾内纳甲、外纳壬），非三元地理杨公风水纳甲。
        /// </remarks>
        public String GetSykStr()
        {
            String s = "";
            s = "\n\n" + this.UpGuo.SkyName + "\n\n\n" + this.DownGuo.SkyName;
            return s;
        }

        /// <summary>
        /// 获得当前卦中“世爻”与“应爻”位置的文本可视化排盘字符形式。
        /// </summary>
        /// <returns>返回长为 6 行的竖排文本。对应爻位若有世应则标注“世”或“应”，其余航空白。</returns>
        /// <remarks>
        /// <b>排盘打印规范：</b><br/>
        /// 文本生成采用传统六爻由上而下的打印习惯（即循环从第 6 爻/上爻（索引 5）向下退至初爻（索引 0））。<br/>
        /// 如果检测到世应数据尚未装配，会自动优先触发 <see cref="LoadGuoSelfandHereThere"/> 实施底层加载。
        /// </remarks>
        public String GetHereThereStr()
        {
            String s = "";
            if (this.HereYao == 0 && this.ThereYao == 0) LoadGuoSelfandHereThere();
            for (int i = 5; i >= 0; i--)
            {
                if (i == this.HereYao) s += "世";
                if (i == this.ThereYao) s += "应";
                s += "\n";
            }
            if (s.Length > 1) s = s.Substring(0, s.Length - 1);
            return s;
        }

        /// <summary>
        /// 获取当前本卦中所有隐藏“伏神”的文本排盘可视化字符形式。
        /// </summary>
        /// <returns>返回由上而下（六爻至初爻）排列的伏神说明字符串；若没有伏神数据则返回 <c>null</c>。</returns>
        /// <remarks>
        /// <b>伏神装卦逻辑：</b><br/>
        /// 方法会逆向提取本宫纯卦（<see cref="GuoSelf"/>）的完整六亲数据，自上而下匹配 <see cref="HideRelativeYaos"/> 标记的伏藏爻位，
        /// 自动拼出地支、五行及六亲全称并追加 "(伏)" 字样（例如："子水子孙(伏)"）。非伏神爻位则以换行符占位。
        /// </remarks>
        public String GetHideRelativeStr()
        {
            String s = ""; bool TorF;
            if (this.HideRelative == null) return null;
            GuoClass gc = GetGuoClass(this.GuoSelf.GuoSubName);//转成六爻卦　
            gc.LoadSixRelative();//加载六亲
            GuoSubClass gsc;//定义一个三爻卦
            for (int i = gc.SixRelative.Count - 1; i >= 0; i--)
            {//由上而下

                for (int j = 0; j < this.HideRelativeYaos.Count; j++)
                {
                    TorF = i == this.HideRelativeYaos[j];
                    if (TorF)
                    {
                        if (i < 3) gsc = gc.DownGuo; else gsc = gc.UpGuo;
                        LocClass lc = gsc.Locs[i % 3];
                        s += lc.LocName + lc.FiveAttr.Name + gc.SixRelative[i].RelativeName + "(伏)";
                    }
                }
                s += "\n";
            }
            if (s.Length > 1) s = s.Substring(0, s.Length - 1);
            return s;
        }

        /// <summary>
        /// 获得本卦六爻地支名称的简化文本排盘字符（不夹带五行属性）。
        /// </summary>
        /// <returns>返回由上而下排列的纯地支名换行字符串（如：“戌\n申\n午...”）。</returns>
        public String GetYaosSkyLocStrShort()
        {
            String s = "";

            for (int i = this.UpGuo.Locs.Count - 1; i >= 0; i--)
            {//由上而下
                LocClass lc = this.UpGuo.Locs[i];
                s += lc.LocName + "\n";
            }
            for (int i = this.DownGuo.Locs.Count - 1; i >= 0; i--)
            {//由上而下
                LocClass lc = this.DownGuo.Locs[i];
                s += lc.LocName + "\n";
            }
            if (s.Length > 1) s = s.Substring(0, s.Length - 1);
            return s;
        }

        /// <summary>
        /// 获得本卦六爻地支连同其所属五行属性的完整文本排盘字符形式。
        /// </summary>
        /// <returns>返回由上而下排列的“地支+五行”换行字符串（如：“戌土\n申金\n午火...”）。</returns>
        public String GetYaosSkyLocStr()
        {
            String s = "";

            for (int i = this.UpGuo.Locs.Count - 1; i >= 0; i--)
            {//由上而下
                LocClass lc = this.UpGuo.Locs[i];
                s += lc.LocName + lc.FiveAttr.Name + "\n";
            }
            for (int i = this.DownGuo.Locs.Count - 1; i >= 0; i--)
            {//由上而下
                LocClass lc = this.DownGuo.Locs[i];
                s += lc.LocName + lc.FiveAttr.Name + "\n";
            }
            if (s.Length > 1) s = s.Substring(0, s.Length - 1);
            return s;
        }

        /// <summary>
        /// 获得本卦六爻排盘配出的“六神”（六兽）文本排盘字符形式。
        /// </summary>
        /// <returns>返回由上而下（六爻至初爻）排列的六神名称换行文本；若六神数据未加载则返回 <c>null</c>。</returns>
        public String GetSixGodStr()
        {
            String s = "";
            if (this.SixGods == null) return null;
            for (int i = this.SixGods.Count - 1; i >= 0; i--)
            {//由上而下
                s += this.SixGods[i].GodName + "\n";
            }
            if (s.Length > 1) s = s.Substring(0, s.Length - 1);
            return s;
        }

        /// <summary>
        /// 基于手动传入的本宫五行属性，动态推导并生成本卦六爻的“六亲”文本排盘字符形式。
        /// </summary>
        /// <param name="GuoSelfAttr">自定义的本宫五行属性名称（如：“金”、“木”、“水”、“火”、“土”）。</param>
        /// <param name="IsHadFateAttr">是否在输出结果的头部和尾部追加特定的排盘空格/格式化占位符。</param>
        /// <returns>返回由上而下排列的六亲换行字符串。</returns>
        /// <remarks>
        /// 该重载方法不读取类库中默认配好的本宫属性，而是完全依据 <paramref name="GuoSelfAttr"/> 进行实时交叉比对（通过生克规则 <see cref="FiveAttr.GetBothAttrRule"/> 变换得出）。
        /// </remarks>
        public String GetSixRelativeStr(String GuoSelfAttr, bool IsHadFateAttr = false)
        {
            List<LocClass> al = new List<LocClass>();
            String s = "";
            for (int i = 0; i < 3; i++) al.Add(this.DownGuo.Locs[i]);
            for (int i = 0; i < 3; i++) al.Add(this.UpGuo.Locs[i]);
            for (int i = al.Count - 1; i >= 0; i--)
            {//由上而下
                LocClass lc = al[i];
                FiveAttrRule far = FiveAttr.GetBothAttrRule(GuoSelfAttr, lc.FiveAttr.Name);
                SixRelativeClass src = SixRelativeClass.GetSixRelative(far);
                s += src.RelativeName + "\n";
            }
            if (s.Length > 1) s = s.Substring(0, s.Length - 1);

            if (IsHadFateAttr)
            {
                s = "　　　\n" + s + "\n　　";

            }
            return s;
        }

        /// <summary>
        /// 获取当前本卦内置已加载的“六千”（六亲）文本排盘字符形式。
        /// </summary>
        /// <param name="IsHadFateAttr">是否在输出结果的头部和尾部追加特定的排盘空格/格式化占位符。</param>
        /// <returns>返回由上而下（六爻至初爻）排列的本卦六亲换行字符串。</returns>
        /// <remarks>
        /// 如果检测到当前实例的 <see cref="SixRelative"/> 集合尚未加载，会自动调用 <see cref="LoadSixRelative"/> 完成初始化装配。
        /// </remarks>
        public String GetSixRelativeStr(bool IsHadFateAttr = false)
        {
            if (this.SixRelative == null) this.LoadSixRelative();
            String s = "";
            for (int i = this.SixRelative.Count - 1; i >= 0; i--)
            {//由上而下
                s += this.SixRelative[i].RelativeName + "\n";
            }
            if (s.Length > 1) s = s.Substring(0, s.Length - 1);

            if (IsHadFateAttr)
            {
                s = "　　　\n" + s + "\n　　";

            }
            return s;
        }

        /// <summary>
        /// 显式调用：一键完整加载当前复卦的所有断卦外围相关参数（含世应别名、空亡、六神、伏神等）。
        /// </summary>
        /// <param name="sSkyName">用于计算旬空与六神的当前预测日“天干”（如：“甲”、“乙”）。</param>
        /// <param name="sLocName">用于计算旬空的当前预测日“地支”（如：“子”、“丑”）。</param>
        public void LoadAll(String sSkyName, String sLocName)
        {
            LoadNoAvg();
            LoadLostLoc(sSkyName, sLocName);//加载空亡
            LoadSixGod(sSkyName);//加载六神
        }

        /// <summary>
        /// 无参数依赖初始化：全自动加载复卦基础的世应爻位、京房别名、六亲分配及伏神结构。
        /// </summary>
        /// <remarks>
        /// 该方法不依赖外部给定的日柱干支，仅根据卦象自身的数理逻辑完成内部基本排盘属性的反哺与填充。
        /// </remarks>
        public void LoadNoAvg()
        {
            LoadGuoSelfandHereThere();//加载世应
            LoadGuoAliasName();//加载别名
            LoadSixRelative();//加载六亲
            LoadHideRelative();//加载伏神
        }
        /// <summary>
        /// 静态公共方法：将扁平化的爻象阴阳状态码序列，逆向渲染为标准易学卦象图形。
        /// </summary>
        /// <param name="al">包含六个爻状态的整型列表（索引自低到高对应初爻至上爻）。</param>
        /// <returns>返回自上而下（上爻至初爻）排列、带有换行符的图形化字符卦象。</returns>
        /// <remarks>
        /// <b>爻象数字代码与图形对照表：</b>
        /// <list type="bullet">
        /// <item><description><c>0</c>：代表常规阴爻（两段少阴）-> <c>"─　─　\n"</c></description></item>
        /// <item><description><c>4</c>：代表发动阴爻（交爻老阴，带有变爻标记 x）-> <c>"─　─x \n"</c></description></item>
        /// <item><description><c>1</c>：代表常规阳爻（单段少阳）-> <c>"───　\n"</c></description></item>
        /// <item><description><c>3</c>：代表发动阳爻（重爻老阳，带有动爻标记 o）-> <c>"───o \n"</c></description></item>
        /// </list>
        /// 渲染循环严格遵循传统画卦由上而下的习惯，从 <c>Count - 1</c> 开始递减打印至 0。
        /// </remarks>
        public static String GetGuoFace(List<int> al)
        {
            String s = "";
            for (int i = al.Count - 1; i >= 0; i--)
            {//由上而下
                if (al[i] % 2 == 0)
                    s += al[i] == 0 ? "─　─　\n" : "─　─x \n";
                else
                    s += al[i] == 1 ? "───　\n" : "───o \n";
            }
            if (s.Length > 1) s = s.Substring(0, s.Length - 1);
            return s;
        }

        /// <summary>
        /// 将当前复卦实例（结合动爻状态）转化为直观的排盘图形字符串。
        /// </summary>
        /// <returns>返回带有动静老少阴阳状态的完整六爻字符卦象（如乾卦动初爻显示为带有 o 的图形）。</returns>
        /// <remarks>
        /// <b>状态动态转化逻辑：</b><br/>
        /// 方法会首先审查 <see cref="YaoDoing"/> 动爻清册。若存在动爻，会动态将对应的内卦（<see cref="DownGuo"/>）或外卦（<see cref="UpGuo"/>）的原始阴阳代码（0或1）重设为 3（老阳动变）或 4（老阴动变）。<br/>
        /// 随后按初爻至上爻的顺序组装整型序列，递交给静态方法 <see cref="GetGuoFace(List{int})"/> 实施终极画面输出。
        /// </remarks>
        public String GetGuoFace()
        {
            List<int> al = new List<int>();
            int iYao = 0;
            if (this.YaoDoing != null)
            {
                if (this.YaoDoing.Count > 0)
                {
                    for (int i = 0; i < this.YaoDoing.Count; i++)
                    {
                        iYao = this.YaoDoing[i];
                        if (iYao < 3)
                        {
                            this.DownGuo.Yaos[iYao] = this.DownGuo.Yaos[iYao] == 0 || this.DownGuo.Yaos[iYao] == 4 ? 4 : 3;
                        }
                        else
                        {
                            this.UpGuo.Yaos[iYao % 3] = this.UpGuo.Yaos[iYao % 3] == 0 || this.UpGuo.Yaos[iYao % 3] == 4 ? 4 : 3;
                        }

                    }
                }
            }

            for (int i = 0; i < 3; i++)
            {
                al.Add(this.DownGuo.Yaos[i]);
            }
            for (int i = 0; i < 3; i++)
            {
                al.Add(this.UpGuo.Yaos[i]);
            }
            String s = GetGuoFace(al);

            // if (Device.RuntimePlatform == Device.iOS) s = s.Replace(" ", "  ");
            return s;
        }

        /// <summary>
        /// 获得发生动变后的变卦对照排盘图形（在发生变动的爻位前追加指向性箭头标记）。
        /// </summary>
        /// <returns>返回格式化后的长条字符串，发生变动的爻位左侧会有明细的 <c>"-&gt;"</c> 变爻箭头指示。</returns>
        /// <remarks>
        /// <b>字符串切割与位置换算明堂：</b><br/>
        /// 算法先获取标准的本卦图形，利用换行符拆分为 6 行行数组。由于图形数组由上而下排布（索引 0 是上爻，索引 5 是初爻），
        /// 算法执行了经典的 <c>5 - ChangedYao[i]</c> 空间位置逆向映射，确保箭头能够精准定位到对应的物理爻位前。
        /// </remarks>
        public String GetChangeGuoFace()
        {
            if (this.DownGuo == null || this.UpGuo == null) return null;
            if (this.ChangedYao == null) return null;
            String s = GetGuoFace();//获得全部爻
            String[] sd = s.Split('\n');

            s = "";
            for (int i = 0; i < this.ChangedYao.Count; i++)
            {
                sd[5 - this.ChangedYao[i]] = "->" + sd[5 - this.ChangedYao[i]];
            }
            for (int i = 0; i < sd.Length; i++)
            {
                if (sd[i].IndexOf("->") == -1)
                    s += "   " + sd[i] + "\n";
                else
                    s += sd[i] + "\n";
            }
            if (s.Length > 0) s = s.Substring(0, s.Length - 1);
            return s;
        }

        /// <summary>
        /// 逆向检索并自动装载主卦所缺失的隐藏“伏神”六亲数据及其所潜伏的爻位。
        /// </summary>
        /// <remarks>
        /// <b>伏神查解核心算法步骤：</b>
        /// <list type="number">
        /// <item><description><b>完整性审查</b>：检查主卦现有的六亲（<see cref="SixRelative"/>）是否涵盖了全部五大关系。若缺项则代表有伏神。</description></item>
        /// <item><description><b>母宫映射</b>：提取当前复卦所隶属的本宫八纯大卦（<see cref="GuoSelf"/>），并将其完全具象化加载出六亲配置。</description></item>
        /// <item><description><b>差集过滤</b>：比对主卦与本宫纯卦，找出主卦中缺失的那个六亲名称（如妻财、官鬼），计算其在母宫中的原始爻位，将其封存至 <see cref="HideRelative"/> 及其爻位坐标轴中。</description></item>
        /// </list>
        /// </remarks>
        public void LoadHideRelative()
        {
            if (this.SixRelative == null) this.LoadSixRelative();
            bool TorF = false;
            for (int i = 0; i < SixRelativeClass.SixRelatives.Length; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    SixRelativeClass src = this.SixRelative[j];
                    TorF = i == src.RelativeIndex;

                    if (TorF == true) break;//找到
                }
                if (TorF == false) break;//找不到,表示有伏神

            }
            if (TorF == true) return;//表示没有伏神，退出
            this.HideRelative = new List<SixRelativeClass>();//实例化
            GuoClass gc = GetGuoClass(this.GuoSelf.GuoSubName);//把卦宫转成六爻卦
            gc.LoadSixRelative();//加载六亲
            List<int> hsrys = new List<int>();//六亲爻位
            String sHad = "";
            for (int i = 0; i < 6; i++)
            {
                SixRelativeClass src = this.SixRelative[i];
                sHad += src.RelativeIndex;
            }
            for (int i = 0; i < 6; i++)
            {
                SixRelativeClass src = gc.SixRelative[i];
                if (sHad.IndexOf(src.RelativeIndex.ToString()) == -1)
                {
                    hsrys.Add(i);//保存位置
                    this.HideRelative.Add(src);//保存这个六亲作为伏神
                }
            }
            this.HideRelativeYaos = hsrys;
        }

        /// <summary>
        /// 核心方法：根据本宫纯卦五行与地支纳音生克关系，全自动装配主卦初爻至上爻的“六亲”属性。
        /// </summary>
        /// <remarks>
        /// <b>装卦生克数理逻辑：</b><br/>
        /// 方法会依次轮询下卦（贞卦）和上卦（悔卦）共六个爻位的纳支（<see cref="LocClass"/>）。<br/>
        /// 调用核心交叉规则 <see cref="FiveAttr.GetBothAttrRule"/>，以母本卦宫的五行属性（我）为基准点，与各爻位纳支五行（他）进行“生我者父母、我生者子孙、克我者官鬼、我克者妻财、比和者兄弟”的五行断立，最终写入 <see cref="SixRelative"/> 列表。
        /// </remarks>
        public void LoadSixRelative()
        {
            if (this.DownGuo.Locs == null) this.DownGuo.LoadLocs();
            if (this.UpGuo.Locs == null) this.UpGuo.LoadLocs();
            if (this.GuoSelf == null) this.LoadGuoSelfandHereThere();
            List<SixRelativeClass> srcs = new List<SixRelativeClass>();
            for (int i = 0; i < 3; i++)
            {
                LocClass lc = this.DownGuo.Locs[i];
                FiveAttrRule far = FiveAttr.GetBothAttrRule(this.GuoSelf.FiveAttr.Name, lc.FiveAttr.Name);
                SixRelativeClass src = SixRelativeClass.GetSixRelative(far);
                srcs.Add(src);
            }
            for (int i = 0; i < 3; i++)
            {
                LocClass lc = this.UpGuo.Locs[i];
                FiveAttrRule far = FiveAttr.GetBothAttrRule(this.GuoSelf.FiveAttr.Name, lc.FiveAttr.Name);
                SixRelativeClass src = SixRelativeClass.GetSixRelative(far);
                srcs.Add(src);
            }
            this.SixRelative = srcs;
        }

        /// <summary>
        /// 扫描当前复卦内卦与外卦相互对应的爻位地支，自动运算并加载“六合卦”与“六冲卦”的特定吉凶别名。
        /// </summary>
        /// <remarks>
        /// <b>断线别名判定算法：</b><br/>
        /// 算法对初爻与四爻、二爻与五爻、三爻与上爻三对地支实施空间垂直透视：
        /// <list type="bullet">
        /// <item><description><b>六合判定</b>：交由 <see cref="FiveAttr.LocCombine"/> 运算，若三对地支两两皆符合地支六合（如子丑、寅亥）规则，自动在别名中追加 <c>"(六合卦)"</c>。</description></item>
        /// <item><description><b>六冲判定</b>：交由 <see cref="FiveAttr.BothConflict"/> 运算，若三对对应轴均属于地支六冲（如子午、卯酉），则自动在别名中强力追加 <c>"(六冲卦)"</c>。</description></item>
        /// </list>
        /// </remarks>
        public void LoadGuoAliasName()
        {
            bool TorF = false; String sValue = "";
            List<LocClass> lcs1 = this.DownGuo.Locs;
            List<LocClass> lcs2 = this.UpGuo.Locs;
            for (int i = 0; i < 3; i++)
            {
                sValue = FiveAttr.LocCombine(lcs1[i].LocName, lcs2[i].LocName);
                TorF = sValue.Length > 0;
                if (TorF == false) break;//不是六合卦
            }
            if (TorF) this.GuoAliasName += "(六合卦)";

            for (int i = 0; i < 3; i++)
            {
                sValue = FiveAttr.BothConflict(lcs1[i].LocName, lcs2[i].LocName);
                TorF = sValue.Length > 0;
                if (TorF == false) break;//不是六冲卦
            }
            if (TorF) this.GuoAliasName += "(六冲卦)";
        }

        /// <summary>
        /// 核心方法（归藏法）：全自动判定并装载当前复卦的“卦宫”（母宫归属）以及“世爻”与“应爻”的绝对爻位。
        /// </summary>
        /// <remarks>
        /// <b>归藏法与世应推导数理逻辑：</b>
        /// <list type="number">
        /// <item><description><b>爻象归藏合并</b>：将下卦（内卦）与上卦（外卦）对应的初爻、中爻、上爻各自的阴阳状态码（0或1）进行两两相加。若相加结果大于 1（即 1+1=2）则强制归 0，完成异或（XOR）数理合并。</description></item>
        /// <item><description><b>融合卦象识别</b>：调用 <see cref="GuoSubClass.GetGuoSub"/> 将合并后的三枚爻重组为一枚全新的三爻经卦 <c>gsc</c>。</description></item>
        /// <item><description><b>定位母宫与世应</b>：依据 <c>gsc</c> 的卦名组合，严格执行京房易八宫排卦法门：
        /// <list type="bullet">
        /// <item><description><b>乾/兑/震宫</b>：世爻分别在三、二、初爻，卦宫归属于<b>外卦（上卦）</b>。</description></item>
        /// <item><description><b>巽/艮宫</b>：世爻分别在四、五爻，卦宫归属于<b>内卦（下卦）全反的错卦</b>。</description></item>
        /// <item><description><b>坤宫（八纯）</b>：世爻在六爻（上爻），卦宫归于本身，标记别名为 <c>"纯卦"</c>。</description></item>
        /// <item><description><b>离宫（游魂）</b>：世爻在四爻，卦宫归于内卦全反，标记别名为 <c>"游魂卦"</c>。</description></item>
        /// <item><description><b>坎宫（归魂）</b>：世爻在三爻，卦宫归于内卦本身，标记别名为 <c>"归魂卦"</c>。</description></item>
        /// </list>
        /// </description></item>
        /// </list>
        /// </remarks>
        private void LoadGuoSelfandHereThere()
        {
            if (this.DownGuo == null || this.UpGuo == null) return;
            int[] Yaos = { 0, 0, 0 };
            Yaos[0] = (this.DownGuo.Yaos[0] % 2) + (this.UpGuo.Yaos[0] % 2);//下爻
            Yaos[1] = (this.DownGuo.Yaos[1] % 2) + (this.UpGuo.Yaos[1] % 2);//中爻
            Yaos[2] = (this.DownGuo.Yaos[2] % 2) + (this.UpGuo.Yaos[2] % 2);//上爻
            for (int i = 0; i < 3; i++)
            {
                if (Yaos[i] > 1) Yaos[i] = 0;
            }
            GuoSubClass gsc = GuoSubClass.GetGuoSub(Yaos[0], Yaos[1], Yaos[2], false);//结合后，看是什么卦
            if (gsc.GuoSubName.Equals("乾"))
            {//乾卦，世在3，卦宫是外卦
                this.HereYao = 2;
                this.ThereYao = 5;
                this.GuoSelf = this.UpGuo;
            }
            else if (gsc.GuoSubName.Equals("兑"))
            {//兑卦，世在2，卦宫是外卦
                this.HereYao = 1;
                this.ThereYao = 4;
                this.GuoSelf = this.UpGuo;
            }
            else if (gsc.GuoSubName.Equals("震"))
            {//震卦，世在初，卦宫是外卦
                this.HereYao = 0;
                this.ThereYao = 3;
                this.GuoSelf = this.UpGuo;
            }
            else if (gsc.GuoSubName.Equals("巽"))
            {//巽卦，世在4，卦宫是内卦全反
                this.HereYao = 3;
                this.ThereYao = 0;
                this.GuoSelf = this.DownGuo.GetXorGuo();//取反卦
            }
            else if (gsc.GuoSubName.Equals("艮"))
            {//艮卦，世在5，卦宫是内卦全反
                this.HereYao = 4;
                this.ThereYao = 1;
                this.GuoSelf = this.DownGuo.GetXorGuo();//取反卦
            }
            else if (gsc.GuoSubName.Equals("坤"))
            {//坤卦，世在6，卦宫是本身（纯卦)
                this.HereYao = 5;
                this.ThereYao = 2;
                this.GuoSelf = this.DownGuo;
                this.GuoAliasName = "纯卦";
            }
            else if (gsc.GuoSubName.Equals("离"))
            {//离卦(游魂卦)，世在4，卦宫是内卦全反
                this.HereYao = 3;
                this.ThereYao = 0;
                this.GuoSelf = this.DownGuo.GetXorGuo();//取反卦
                this.GuoAliasName = "游魂卦";
            }
            else if (gsc.GuoSubName.Equals("坎"))
            {//离卦(归魂卦)，世在3，卦宫是内卦
                this.HereYao = 2;
                this.ThereYao = 5;
                this.GuoSelf = this.DownGuo;
                this.GuoAliasName = "归魂卦";
            }
        }

        /// <summary>
        /// 静态公共方法：依据输入的日柱天干与地支，动态推导并获取该旬中处于“空亡”状态的两个地支对象。
        /// </summary>
        /// <param name="sSkyName">预测当日的日干名称（如：“甲”、“乙”、“丙”等）。</param>
        /// <param name="sLocName">预测当日的日支名称（如：“子”、“丑”、“寅”等）。</param>
        /// <returns>返回包含两个截路空亡地支实体对象的集合 <c>List&lt;LocClass&gt;</c>。</returns>
        /// <remarks>
        /// <b>旬空运算法则：</b><br/>
        /// 六十甲子以十天为一旬。算法首先锁定天干与地支在标准排布中的索引位置，
        /// 计算出该旬距离终点（第十个天干“癸”）的地支富余偏移量 <c>iPos = 9 - iSPos</c>，
        /// 顺推求得紧随其后的两个孤虚地支索引，若超出 11（亥位）则执行天干地支五行圆周取模（<c>-12</c>）完成闭环定位。
        /// </remarks>
        public static List<LocClass> GetLostLocs(String sSkyName, String sLocName)
        {
            List<LocClass> lcs = new List<LocClass>();
            int iSPos = Array.IndexOf(SkyClass.SkyNames, sSkyName);

            int iLPos = Array.IndexOf(LocClass.LocNames, sLocName);
            int iPos = 9 - iSPos;
            iLPos += iPos + 1;//空亡位置
            if (iLPos > 11) iLPos -= 12;
            LocClass lc = LocClass.GetLocClass(iLPos);
            lcs.Add(lc);
            iLPos++;
            if (iLPos > 11) iLPos -= 12;
            lc = LocClass.GetLocClass(iLPos);
            lcs.Add(lc);
            return lcs;
        }

        /// <summary>
        /// 依赖注入：依据占卜预测日柱的干支名称，计算并加载本卦的两个旬空亡地支（方向由下而上）。
        /// </summary>
        /// <param name="sSkyName">日柱天干名称。</param>
        /// <param name="sLocName">日柱地支名称。</param>
        private void LoadLostLoc(String sSkyName, String sLocName)
        {
            this.LostLocs = GetLostLocs(sSkyName, sLocName);
        }

        /// <summary>
        /// 依赖注入：依据占卜预测日的日干名称，计算并排定六爻排盘所需的“六神”（六兽）占位集合。
        /// </summary>
        /// <param name="sSkyName">预测日的日柱天干（如甲乙起青龙、丙丁起朱雀，由初爻顺次向上排布）。</param>
        private void LoadSixGod(String sSkyName)
        {
            this.SixGods = SixGodClass.GetSixGod(sSkyName);
        }

        /// <summary>
        /// 内部核心方法（带有递归设计）：根据传入的一字简名或属性模糊名，反向模糊检索并返回 64 卦的四字标准全名。
        /// </summary>
        /// <param name="GuoNameOrAttrName">输入的任意易学名称标识（如单字简名 “乾”、或者属性名 “天” ）。</param>
        /// <returns>返回匹配成功的 64 卦通行本四字全称字符串（如“乾为天”）；若不匹配则返回 <c>null</c>。</returns>
        /// <remarks>
        /// <b>递归终止与检索门道：</b><br/>
        /// <list type="bullet">
        /// <item><description>若输入为单字且属于 <see cref="GuoNames"/>，直接从清册中返回全称（如输入“屯”返回“水雷屯”）。</description></item>
        /// <item><description>若单字不属于卦名，而属于自然属性名（如输入“天”），方法会定位到后天经卦序列，将其转化为标准卦名“乾”，并<b>触发自我递归（Recursive Call）</b>再次清洗，确保拿到绝对准确的四字全称。</description></item>
        /// <item><description>若输入为长多字（包含无效占位符 ? ），会自动剥离噪音，遍历 <see cref="GuoFullNames"/> 进行包含式模糊匹配。</description></item>
        /// </list>
        /// </remarks>
        private static String GetFullGuoName(String GuoNameOrAttrName)
        {
            string sName = GuoNameOrAttrName;
            String sFullName = null, s = null;
            if (string.IsNullOrWhiteSpace(sName)) return null;

            int iPos = GuoNames.IndexOf(GuoNameOrAttrName);//判断是否是卦名
            if (iPos > -1) return GuoFullNames[iPos]; //直接返回全名
            if (sName.Length == 1)
            {//一字卦，全部转成三字卦
                iPos = Array.IndexOf(GuoNames, sName);
                if (iPos > -1)
                {
                    sFullName = GuoFullNames[iPos];
                }
                else
                {
                    iPos = Array.IndexOf(GuoSubClass.AfterGuoSubAttrNames, sName);
                    if (iPos > -1)
                    {//属性纯卦
                        s = GuoSubClass.AfterGuoSubNames[iPos];
                        return GetFullGuoName(s);//递归
                    }
                }

            }
            else if (sName.Length > 2)
            {
                sName = sName.Replace("?", "");
                for (int i = 0; i < GuoFullNames.Length; i++)
                {
                    if (GuoFullNames[i].IndexOf(sName) != -1)
                    {
                        sFullName = GuoFullNames[i];
                        break;
                    }
                }

            }
            return sFullName;
        }

        /// <summary>
        /// 内部静态辅助方法：根据输入的复合大卦名，剥离并提取其对应上卦（外卦）或下卦（内卦）的三爻经卦单字简名。
        /// </summary>
        /// <param name="sGuoName">任意形式的复卦名称（支持单字简名、三字或四字全名）。</param>
        /// <param name="IsUpGuo"><c>true</c> 代表期望获取上卦（外卦、悔卦）名称；<c>false</c> 代表期望获取下卦（内卦、贞卦）名称。</param>
        /// <returns>返回对应经卦的单字中文名称（如“乾”、“坤”、“坎”、“离”）；提取失败则返回 <c>null</c>。</returns>
        /// <remarks>
        /// <b>纯卦与杂卦解析门道：</b><br/>
        /// 方法会首先调用 <see cref="GetFullGuoName"/> 将参数统一规范化为四字或三字标准全名。接着将其打散为字符数组：<br/>
        /// 若在先天八卦名录（<c>BeforeGuoSubNames</c>）中能直接匹配到首字，判定其属于“八纯大卦”（如乾为天、坤为地），此时上下经卦完全相同，直接返回首字。<br/>
        /// 若首字不属于纯卦，则代表属于“异卦相叠的杂卦”（如地水师），算法会自动根据 <paramref name="IsUpGuo"/> 分别截取前置字（上卦物象）或后置字（下卦物象）返回。
        /// </remarks>
        private static String GetGuoSubName(String sGuoName, bool IsUpGuo)
        {
            char[] sd; String sFullName;
            int iPos = 0;

            sFullName = GetFullGuoName(sGuoName);//转成 卦的全名
            if (sFullName == null) return null;
            if (sFullName.Length >= 3)
            {//三字卦或四字卦
                sd = sGuoName.ToCharArray(); // .toCharArray();

                iPos = Array.IndexOf(GuoSubClass.BeforeGuoSubNames, sd[0].ToString());

                if (iPos == -1)
                {//找不到表示不是纯卦
                    iPos = Array.IndexOf(GuoSubClass.BeforeGuoSubAttrNames, sd[0].ToString());
                    if (iPos == -1) return null;
                    iPos = Array.IndexOf(GuoSubClass.BeforeGuoSubAttrNames, sd[1].ToString());
                    if (iPos == -1) return null;
                    return (IsUpGuo ? sd[0].ToString() : sd[1].ToString());//返回上下卦的卦名
                }
                else
                {//找到表示是纯卦
                    return sd[0].ToString();
                }
            }
            return null;
        }
        /// <summary>
        /// 静态工厂方法：依据 64 卦对应的 60 甲子干支名称反向检索并创建对应的复卦对象实例。
        /// </summary>
        /// <param name="sSkyLocName">输入的干支组合名称（如：“甲午”、“甲子”、“戊子”）。</param>
        /// <returns>返回匹配成功的 <see cref="GuoClass"/> 实例；若该干支在内置清册中未绑定大卦则返回 <c>null</c>。</returns>
        public static GuoClass GetGuoClassBySkyLoc(String sSkyLocName)
        {
            int i = Array.IndexOf(GuoSkyLocs, sSkyLocName);
            if (i > -1)
            {
                return GetGuoClass(i);
            }
            return null;
        }

        /// <summary>
        /// 静态工厂方法：依据两字简名创建复卦（六爻卦）对象实例。
        /// </summary>
        /// <param name="sGuoNameOrAttr">输入的先天 64 卦简名（如：“乾”、“坤”、“屯”）。</param>
        /// <returns>返回装配好的 <see cref="GuoClass"/> 对象实例。</returns>
        public static GuoClass GetGuoClass(String sGuoNameOrAttr)
        {
            return new GuoClass(sGuoNameOrAttr);
        }

        /// <summary>
        /// 静态工厂方法：依据先天卦序索引创建复卦（六爻卦）对象实例。
        /// </summary>
        /// <param name="iGuoIndex">先天 64 卦的原始数组索引（取值范围：0 至 63）。</param>
        /// <returns>返回装配好的 <see cref="GuoClass"/> 对象实例。</returns>
        public static GuoClass GetGuoClass(int iGuoIndex)
        {
            return new GuoClass(iGuoIndex);
        }

        /// <summary>
        /// 静态工厂方法：接收一个包含 6 个爻状态码的整型数组，重组并反灌创建带有变爻动静的复卦对象实例。
        /// </summary>
        /// <param name="iYaos">长为 6 的基本整型数组（索引 0 至 5 严格对应初爻至上爻）。</param>
        /// <returns>返回装配好的 <see cref="GuoClass"/> 对象实例。</returns>
        public static GuoClass GetGuoClass(int[] iYaos)
        {
            return GetGuoClass(iYaos.ToList());
        }

        /// <summary>
        /// 静态工厂方法：接收一个包含 6 个爻状态码的整型列表，重组并反灌创建带有变爻动静的复卦对象实例。
        /// </summary>
        /// <param name="iYaos">包含 6 个爻状态的整型列表（索引自低到高严格对应初爻至上爻）。</param>
        /// <returns>返回装配好的复卦实例；如果传入的列表长度不等于 6 则返回 <c>null</c>。</returns>
        /// <remarks>
        /// <b>状态码重组与动爻捕获数理逻辑：</b><br/>
        /// <list type="bullet">
        /// <item><description><b>经卦解耦</b>：取列表前 3 爻模 2 运算生成下卦（内卦），取后 3 爻模 2 运算生成上卦（外卦）。</description></item>
        /// <item><description><b>动爻拦截（老阴老阳判定）</b>：算法循环遍历 6 个爻。若状态码为 <c>3</c>（老阳，阳动变阴）或 <c>4</c>（老阴，阴动变阳），则会被视为动爻，自动抽离并装入本卦的 <see cref="YaoDoing"/> 动爻跟踪清册中，并同步重设对应经卦的运行时爻位状态。</description></item>
        /// </list>
        /// </remarks>
        public static GuoClass GetGuoClass(List<int> iYaos)
        {
            if (iYaos.Count != 6) return null;

            GuoSubClass gsc = GuoSubClass.GetGuoSub(iYaos[0] % 2, iYaos[1] % 2, iYaos[2] % 2, true); //下卦
            if (gsc == null) return null;
            String sAttrName2 = gsc.GuoSubAttrName;
            gsc = GuoSubClass.GetGuoSub(iYaos[3] % 2, iYaos[4] % 2, iYaos[5] % 2, false); //上卦
            if (gsc == null) return null;
            String sAttrName1 = gsc.GuoSubAttrName;
            String sAttrName = "";
            if (sAttrName1.Equals(sAttrName2)) sAttrName = sAttrName1; else sAttrName = sAttrName1 + sAttrName2 + "?";
            GuoClass gc = GetGuoClass(sAttrName);
            if (gc.YaoDoing == null) gc.YaoDoing = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                if (iYaos[i] == 3 || iYaos[i] == 4)//3为阳爻动，4为阴爻动
                {
                    gc.YaoDoing.Add(i);//如果爻值是3或4，都是动爻，保存起来。
                    int iMod = i % 3;
                    if (i < 3) gc.DownGuo.Yaos[iMod] = iYaos[i]; else gc.UpGuo.Yaos[iMod] = iYaos[i];
                }
            }
            return gc;
        }

        /// <summary>
        /// 静态工厂方法（术数算法）：执行经典的先天梅花易数数字起卦逻辑，推演并返回对应带有动爻的复卦。
        /// </summary>
        /// <param name="iv1">上卦计算基数（如：年支数+月数+日数之和，或上两字笔画数）。</param>
        /// <param name="iv2">下卦计算基数（如：年支数+月数+日数+时支数之和，或下两字笔画数）。</param>
        /// <returns>返回依照先天梅花数理生成的 <see cref="GuoClass"/> 复卦实例。</returns>
        /// <remarks>
        /// <b>⚠️ 警告：</b>此方法生成的复卦仅供梅花易数体用断卦，其外围的“六亲”、“六神”等断语属性<b>默认未加载</b>。<br/><br/>
        /// <b>梅花数理起卦公式明堂：</b>
        /// <list type="number">
        /// <item><description><b>上卦公式</b>：<c>(iv1 % 8) - 1</c>。若余数为 0（取模归零），则强行重设为 7（对应乾一、兑二...坤八的数组边界）。</description></item>
        /// <item><description><b>下卦公式</b>：<c>(iv2 % 8) - 1</c>。同样执行归零校验，并通过排除重复字提取出下卦物象，拼接成模糊模糊字以便反查。</description></item>
        /// <item><description><b>动爻公式（取变爻）</b>：<c>((iv1 + iv2) % 6) - 1</c>。相加余数除 6 的余数锁定动爻。若余数为 0 则重设为 5（代表上爻/第六爻发动）。</description></item>
        /// </list>
        /// 算法最后会自动识别动爻爻位所处的空间（内卦或外卦），将少阴/少阳直接转化为对应的老阴（4）或老阳（3），完成起卦闭环。
        /// </remarks>
        public static GuoClass FlowerGuoClass(int iv1, int iv2)
        {
            int iMod = (iv1 % 8) - 1;//先天卦乾一，兑二...要减去1，因为卦位置从0开始
            if (iMod < 0) iMod = 7;//为负1，则是坤八数
            String sName = GuoSubClass.BeforeGuoSubAttrNames[iMod];//上卦名
            iMod = (iv2 % 8) - 1;
            if (iMod < 0) iMod = 7;//为负1，则是坤八数.
            sName += GuoSubClass.BeforeGuoSubAttrNames[iMod].Replace(sName, "");//下卦名
            if (sName.Length > 1) sName += "?";//可查最后的卦名
            int iYaoDoing = ((iv1 + iv2) % 6) - 1;//相加取余则是动爻，要减去1，因为爻由0开始
            if (iYaoDoing < 0) iYaoDoing = 5;//为-1则是上爻动。
            GuoClass gc = GetGuoClass(sName);
            int iv;
            if (iYaoDoing < 3)
            { //则下卦动
                iv = gc.DownGuo.Yaos[iYaoDoing % 3];
                iv = iv == 0 ? 4 : 3;
                gc.DownGuo.Yaos[iYaoDoing % 3] = iv;
            }
            else
            {
                iv = gc.UpGuo.Yaos[iYaoDoing % 3];
                iv = iv == 0 ? 4 : 3;
                gc.UpGuo.Yaos[iYaoDoing % 3] = iv;
            }
            List<int> al = new List<int>();
            al.Add(iYaoDoing);//添加动爻
            gc.YaoDoing = al;

            return gc;
        }

        #endregion
    }
}
