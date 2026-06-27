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
using System.Runtime.Serialization;


namespace CompassEx.Guo
{




    public class GuoClass
    {
        /// <summary>
        /// 64卦全卦简名
        /// </summary>
        [JsonIgnore]
        public readonly static String[] GuoNames = { "乾", "坤", "屯", "蒙", "需", "讼", "师", "比", "小畜", "履", "泰", "否", "同人", "大有", "谦", "豫", "随", "蛊", "临", "观", "噬嗑", "贲", "剥", "复", "无妄", "大畜", "颐", "大过", "坎", "离", "咸", "恒", "遁", "大壮", "晋", "明夷", "家人", "睽", "蹇", "解", "损", "益", "夬", "姤", "萃", "升", "困", "井", "革", "鼎", "震", "艮", "渐", "归妹", "丰", "旅", "巽", "兑", "涣", "节", "中孚", "小过", "既济", "未济" };

        /// <summary>
        /// 64卦全卦名字
        /// </summary>
        [JsonIgnore]
        public readonly static String[] GuoFullNames = { "乾为天", "坤为地", "水雷屯", "山水蒙", "水天需", "天水讼", "地水师", "水地比", "风天小畜", "天泽履", "地天泰", "天地否", "天火同人", "火天大有", "地山谦", "雷地豫", "泽雷随", "山风蛊", "地泽临", "风地观", "火雷噬嗑", "山火贲", "山地剥", "地雷复", "天雷无妄", "山天大畜", "山雷颐", "泽风大过", "坎为水", "离为火", "泽山咸", "雷风恒", "天山遁", "雷天大壮", "火地晋", "地火明夷", "风火家人", "火泽睽", "水山蹇", "雷水解", "山泽损", "风雷益", "泽天夬", "天风姤", "泽地萃", "地风升", "泽水困", "水风井", "泽火革", "火风鼎", "震为雷", "艮为山", "风山渐", "雷泽归妹", "雷火丰", "火山旅", "巽为风", "兑为泽", "风水涣", "水泽节", "风泽中孚", "雷山小过", "水火既济", "火水未济" };

        /// <summary>
        /// 64卦对应60甲子
        /// </summary>
        [JsonIgnore]
        public readonly static string[] GuoSkyLocs = { "甲午", "甲子", "戊子", "庚申", "乙巳", "辛未", "壬申", "辛亥", "丁巳", "戊辰", "庚辰", "庚戌", "壬寅", "辛巳", "戊戌", "丁亥", "丁丑", "丁未", "乙卯", "己亥", "乙丑", "癸丑", "癸亥", "甲子", "己丑", "壬辰", "丙子", "丙午", "庚申", "庚寅", "丁酉", "庚午", "乙酉", "己巳", "乙亥", "辛丑", "丙寅", "甲辰", "甲戌", "丙申", "丁卯", "庚子", "癸巳", "甲午", "壬戌", "己未", "癸未", "乙未", "庚寅", "戊午", "壬子", "丙戌", "癸酉", "癸卯", "戊寅", "己酉", "壬午", "丙辰", "戊申", "己卯", "辛卯", "辛酉", "甲寅", "甲申" };

        /// <summary>
        /// 64卦符号
        /// </summary>
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
        /// 卦气（洛数）
        /// </summary>
        public int GuoQi
        {
            get
            {

                foreach (string sn in GuoSubClass.BeforeGuoSubNames)
                {
                    var gs = GuoSubClass.GetGuoSub(sn);
                    var CR = gs.CAfterRangeDegree;
                    if (this.UpGuo.CBeforRangeDegree.IsInRange(CR.Start))
                        return gs.AfterQuantity + 1;
                }
                return -1;
            }

        }


        /// <summary>
        /// 下卦（内卦），三爻卦
        /// </summary>
        [JsonIgnore]
        public GuoSubClass DownGuo;//下卦（内卦），三爻卦
        /// <summary>
        /// 上卦（外卦），三爻卦
        /// </summary>
        [JsonIgnore]
        public GuoSubClass UpGuo;//上卦（外卦），三爻卦
        /// <summary>
        /// 六爻卦，卦名(简)
        /// </summary>
        public String GuoName;//六爻卦，卦名
        /// <summary>
        /// 六爻卦的全名
        /// </summary>
        public String GuoFullName;//六爻卦的全名
        /// <summary>
        /// 卦的所在索引
        /// </summary>
        public int GuoIndex = -1;//卦的所在索引
        /// <summary>
        /// 六神
        /// </summary>
        [JsonIgnore]
        public List<SixGodClass> SixGods;//六神
        /// <summary>
        /// 六亲
        /// </summary>
        [JsonIgnore]
        public List<SixRelativeClass> SixRelative;//六亲
        /// <summary>
        /// 空亡的两个地支
        /// </summary>
        [JsonIgnore]
        public List<LocClass> LostLocs;//空亡的两个地支
        /// <summary>
        /// 世爻
        /// </summary>
        public int HereYao;//世
        /// <summary>
        /// 应爻
        /// </summary>
        public int ThereYao;//应

        /// <summary>
        /// 已变化后的爻
        /// </summary>
        [JsonIgnore]
        public List<int> ChangedYao;//变化后的爻

        /// <summary>
        /// 卦的所属别名，例如：归魂卦，游魂卦，纯卦等
        /// </summary>
        public String GuoAliasName = "";//卦的所属别名，例如：归魂卦，游魂卦，纯卦等
        /// <summary>
        /// 卦宫(三爻卦),名称
        /// </summary>
        [JsonIgnore]
        public GuoSubClass GuoSelf;//卦宫(三爻卦),名称
        /// <summary>
        /// 伏神
        /// </summary>
        [JsonIgnore]
        public List<SixRelativeClass> HideRelative;//伏神
        /// <summary>
        /// 伏神位置
        /// </summary>
        [JsonIgnore]
        public List<int> HideRelativeYaos;//伏神位置
        /// <summary>
        /// 爻动 [0,1]表示动初、二爻
        /// </summary>
        [JsonIgnore]
        public List<int> YaoDoing;//爻动

        /// <summary>
        /// 罗盘中的度数范围对象，表示这个卦在罗盘中的位置范围
        /// </summary>
        [JsonIgnore]
        public CompassRangEX CBeforRangeDegree { get { return CompassEx.GetCBeforGuoDegree(this.GuoName); } }


        /// <summary>
        /// 先天64卦名与符号
        /// </summary>
        public static readonly Dictionary<string, string> BeforGuoDC = new Dictionary<string, string> { { "坤", "\u4DC1" }, { "剥", "\u4DFE" }, { "比", "\u4DC7" }, { "观", "\u4DD3" }, { "豫", "\u4DFB" }, { "晋", "\u4DFA" }, { "萃", "\u4DF9" }, { "否", "\u4DCB" }, { "谦", "\u4DCF" }, { "艮", "\u4DF6" }, { "蹇", "\u4DF5" }, { "渐", "\u4DF4" }, { "小过", "\u4DF3" }, { "旅", "\u4DF2" }, { "咸", "\u4DDE" }, { "遯", "\u4DF0" }, { "师", "\u4DC6" }, { "蒙", "\u4DDE" }, { "坎", "\u4DDC" }, { "涣", "\u4DEC" }, { "解", "\u4DEB" }, { "未济", "\u4DEA" }, { "困", "\u4DE9" }, { "讼", "\u4DC5" }, { "升", "\u4DE7" }, { "蛊", "\u4DD1" }, { "井", "\u4DE5" }, { "巽", "\u4DE4" }, { "恒", "\u4DDF" }, { "鼎", "\u4DE2" }, { "大过", "\u4DDB" }, { "姤", "\u4DE0" }, { "乾", "\u4DC0" }, { "夬", "\u4DE8" }, { "大有", "\u4DCD" }, { "大壮", "\u4DDB" }, { "小畜", "\u4DC8" }, { "需", "\u4DC4" }, { "大畜", "\u4DD9" }, { "泰", "\u4DCA" }, { "履", "\u4DC9" }, { "兑", "\u4DDF" }, { "睽", "\u4DE6" }, { "归妹", "\u4DE5" }, { "中孚", "\u4DDC" }, { "节", "\u4DEB" }, { "损", "\u4DD9" }, { "临", "\u4DD2" }, { "同人", "\u4DCC" }, { "革", "\u4DE3" }, { "离", "\u4DDD" }, { "丰", "\u4DCD" }, { "家人", "\u4DE4" }, { "既济", "\u4DE9" }, { "贲", "\u4DD5" }, { "明夷", "\u4DCE" }, { "无妄", "\u4DD8" }, { "随", "\u4DD0" }, { "噬嗑", "\u4DD4" }, { "震", "\u4DC6" }, { "益", "\u4DCF" }, { "屯", "\u4DC2" }, { "颐", "\u4DDA" }, { "复", "\u4DD7" } };

        /// <summary>
        /// 本卦中的通卦
        /// </summary>
        /// <returns></returns>
        public GuoClass ExchangeGuo()
        {
            GuoClass g = GuoClass.GetGuoClass(this.GuoIndex);//创建相同卦
            g.YaoDoing = new List<int>() { 2, 3 };//3、4爻变为通卦 一、三；二、四 ；六、八；七、九 相通
            return g.GetChangeGuo();



        }



        /// <summary>
        /// 获得当前卦的6世飞爻卦,京房易卦变(7世飞爻荡出，用于计算命命的入卦,此是64卦变卦规则)，一般用于六冲（纯）卦
        /// </summary>
        /// <returns></returns>
        public List<GuoClass> Get6HereYaoGuo()
        {
            //64卦变卦规则:初爻变出第一世飞爻卦,以后的飞爻卦由上一个飞爻卦的初爻变出,共变5次后，再返回第5爻变（游魂卦），再把下卦三爻全变（归魂卦），
            List<GuoClass> GuoIns = new List<GuoClass>();
            GuoIns.Add(this);
            var g = new GuoClass();
            g.ApplyBaseProperties(this);//复制本卦
            GuoClass ng;
            for (int i = 0; i < 5; i++)
            {
                ng = new GuoClass();
                ng.ApplyBaseProperties(g);//复制上次的卦
                ng.YaoDoing = new List<int>() { i }; //变爻
                g = ng.GetChangeGuo();//变卦
                GuoIns.Add(g);//加入变出的卦

            }
            //第7个卦为游魂卦(再返回第5爻变 ）
            ng = new GuoClass();
            ng.ApplyBaseProperties(g);//复制上次的卦
            ng.YaoDoing = new List<int>() { 3 }; //变4爻
            g = ng.GetChangeGuo();//变卦
            GuoIns.Add(g);//加入变出的卦


            //第8个卦为归魂卦(再把下卦三爻全变 ）
            ng = new GuoClass();
            ng.ApplyBaseProperties(g);//复制上次的卦
            ng.YaoDoing = new List<int>() { 0, 1, 2 }; //变123爻
            g = ng.GetChangeGuo();//变卦
            GuoIns.Add(g);//加入变出的卦


            return GuoIns;
        }



        /// <summary>
        /// 反序列化完成后自动执行的方法（最适合用来重设属性）
        /// </summary>
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
        /// 返回卦对应原天干地去结构类型
        /// </summary>
        [JsonIgnore]
        public Defined.SkyLoc GuoSkyLoc
        {
            get
            {
                Defined.SkyLoc ls = new Defined.SkyLoc();
                string s = GuoSkyLocs[this.GuoIndex];
                ls.SkyLocName = s;
                ls.Sky = SkyClass.GetSkyClass(s.Substring(0, 1));
                ls.Loc = LocClass.GetLocClass(s.Substring(1, 1));
                return ls;
            }
        }




        /// <summary>
        /// 返回复卦的五行(上卦所在位置的后天数)
        /// </summary>
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
        /// 卦运名称
        /// </summary>
        public static readonly string[] GuoFateNames = { "一", "二", "三", "四", "", "六", "七", "八", "九" };
        /// <summary>
        /// 返回复卦运
        /// </summary>
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
        /// 获得爻的字符排列
        /// </summary>
        /// <returns></returns>
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
        /// 返回本卦(复卦)的六爻数组
        /// </summary>
        /// <returns></returns>
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
        /// 根据本卦的动爻取得变卦
        /// </summary>
        /// <returns></returns>
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
        /// 获得天干的字符形式
        /// </summary>
        /// <returns></returns>
        public String GetSykStr()
        {
            String s = "";
            s = "\n\n" + this.UpGuo.SkyName + "\n\n\n" + this.DownGuo.SkyName;
            return s;
        }

        /// <summary>
        /// 获得世应的字符形式
        /// </summary>
        /// <returns></returns>
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
        /// 获得伏神的字符形式
        /// </summary>
        /// <returns></returns>
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
                        s += lc.LocName + lc.FiveAttrName + gc.SixRelative[i].RelativeName + "(伏)";
                    }
                }
                s += "\n";
            }
            if (s.Length > 1) s = s.Substring(0, s.Length - 1);
            return s;
        }

        /// <summary>
        /// 获得六爻的天干地支字符形式(不加入五行)
        /// </summary>
        /// <returns></returns>
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
        /// 获得六爻的天干地支字符形式
        /// </summary>
        /// <returns></returns>
        public String GetYaosSkyLocStr()
        {
            String s = "";

            for (int i = this.UpGuo.Locs.Count - 1; i >= 0; i--)
            {//由上而下
                LocClass lc = this.UpGuo.Locs[i];
                s += lc.LocName + lc.FiveAttrName + "\n";
            }
            for (int i = this.DownGuo.Locs.Count - 1; i >= 0; i--)
            {//由上而下
                LocClass lc = this.DownGuo.Locs[i];
                s += lc.LocName + lc.FiveAttrName + "\n";
            }
            if (s.Length > 1) s = s.Substring(0, s.Length - 1);
            return s;
        }

        /// <summary>
        /// 获得六神的字符形式
        /// </summary>
        /// <returns></returns>
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
        /// 获得六亲的字符形式
        /// </summary>
        /// <param name="GuoSelfAttr"></param>
        /// <param name="IsHadFateAttr"></param>
        /// <returns></returns>
        public String GetSixRelativeStr(String GuoSelfAttr, bool IsHadFateAttr = false)
        {
            List<LocClass> al = new List<LocClass>();
            String s = "";
            for (int i = 0; i < 3; i++) al.Add(this.DownGuo.Locs[i]);
            for (int i = 0; i < 3; i++) al.Add(this.UpGuo.Locs[i]);
            for (int i = al.Count - 1; i >= 0; i--)
            {//由上而下
                LocClass lc = al[i];
                FiveAttr.FiveAttrRule far = FiveAttr.GetBothAttrRule(GuoSelfAttr, lc.FiveAttrName);
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
        /// 获得六亲的字符形式
        /// </summary>
        /// <param name="IsHadFateAttr"></param>
        /// <returns></returns>
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
        /// 加载所有参数
        /// </summary>
        /// <param name="sSkyName"></param>
        /// <param name="sLocName"></param>
        public void LoadAll(String sSkyName, String sLocName)
        {
            LoadNoAvg();
            LoadLostLoc(sSkyName, sLocName);//加载空亡
            LoadSixGod(sSkyName);//加载六神
        }
        /// <summary>
        /// 无参数加载：世应、别名、六亲、伏神
        /// </summary>
        public void LoadNoAvg()
        {
            LoadGuoSelfandHereThere();//加载世应
            LoadGuoAliasName();//加载别名
            LoadSixRelative();//加载六亲
            LoadHideRelative();//加载伏神

        }


        /// <summary>
        /// 把爻转成卦的图形形式
        /// </summary>
        /// <param name="al"></param>
        /// <returns></returns>
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
        /// 把卦转成图形形式
        /// </summary>
        /// <returns></returns>
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
        /// 把卦转成图形形式
        /// </summary>
        /// <returns></returns>
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
        /// 加载伏神
        /// </summary>
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
        /// 加载卦六亲
        /// </summary>
        public void LoadSixRelative()
        {
            if (this.DownGuo.Locs == null) this.DownGuo.LoadLocs();
            if (this.UpGuo.Locs == null) this.UpGuo.LoadLocs();
            if (this.GuoSelf == null) this.LoadGuoSelfandHereThere();
            List<SixRelativeClass> srcs = new List<SixRelativeClass>();
            for (int i = 0; i < 3; i++)
            {
                LocClass lc = this.DownGuo.Locs[i];
                FiveAttr.FiveAttrRule far = FiveAttr.GetBothAttrRule(this.GuoSelf.FiveAttrName, lc.FiveAttrName);
                SixRelativeClass src = SixRelativeClass.GetSixRelative(far);
                srcs.Add(src);
            }
            for (int i = 0; i < 3; i++)
            {
                LocClass lc = this.UpGuo.Locs[i];
                FiveAttr.FiveAttrRule far = FiveAttr.GetBothAttrRule(this.GuoSelf.FiveAttrName, lc.FiveAttrName);
                SixRelativeClass src = SixRelativeClass.GetSixRelative(far);
                srcs.Add(src);
            }
            this.SixRelative = srcs;
        }
        /// <summary>
        /// 加载卦宫的别名
        /// </summary>
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
        /// 定卦宫 和世应
        /// </summary>
        public void LoadGuoSelfandHereThere()
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
        /// 获得空亡的两个地支
        /// </summary>
        /// <param name="sSkyName"></param>
        /// <param name="sLocName"></param>
        /// <returns></returns>
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
        /// 加载空亡(由下而上)
        /// </summary>
        /// <param name="sSkyName"></param>
        /// <param name="sLocName"></param>
        public void LoadLostLoc(String sSkyName, String sLocName)
        {

            this.LostLocs = GetLostLocs(sSkyName, sLocName);
        }

        /// <summary>
        /// 加载六神(由下而上)
        /// </summary>
        /// <param name="sSkyName"></param>
        public void LoadSixGod(String sSkyName)
        {

            this.SixGods = SixGodClass.GetSixGod(sSkyName);
        }
        /// <summary>
        /// 根据传入的卦名或属性名获得的全名
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        private static String GetFullGuoName(String sName)
        {
            String sFullName = null, s = null;
            if (sName == null) return null;
            if (sName.Length == 0) return null;
            int iPos;
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
        /// 根据传入的卦名获得三爻卦的卦名或属性名
        /// </summary>
        /// <param name="sGuoName"></param>
        /// <param name="IsUpGuo"></param>
        /// <returns></returns>
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
        /// 根据天干地支创建卦象类
        /// </summary>
        /// <param name="sSkyLocName"></param>
        /// <returns></returns>
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
        /// 根据卦名来创建卦类(六爻卦)
        /// </summary>
        /// <param name="sGuoName"></param>
        /// <returns></returns>
        public static GuoClass GetGuoClass(String sGuoName)
        {
            GuoClass gc = new GuoClass();
            gc.GuoFullName = GetFullGuoName(sGuoName);
            String sUpGuoName = GetGuoSubName(gc.GuoFullName, true);
            String sDownGuoName = GetGuoSubName(gc.GuoFullName, false);

            gc.UpGuo = GuoSubClass.GetGuoSub(sUpGuoName, false);
            gc.DownGuo = GuoSubClass.GetGuoSub(sDownGuoName, true);
            gc.GuoIndex = Array.IndexOf(GuoClass.GuoFullNames, gc.GuoFullName);
            gc.GuoName = GuoClass.GuoNames[gc.GuoIndex];
            return gc;
        }

        /// <summary>
        /// 根据卦的索引来创建卦类(六爻卦)
        /// </summary>
        /// <param name="iGuoIndex"></param>
        /// <returns></returns>
        public static GuoClass GetGuoClass(int iGuoIndex)
        {
            String sGuoFullName = GuoFullNames[iGuoIndex];

            GuoClass gc = new GuoClass();
            gc.GuoFullName = sGuoFullName;
            String sUpGuoName = GetGuoSubName(sGuoFullName, true);
            String sDownGuoName = GetGuoSubName(sGuoFullName, false);

            gc.UpGuo = GuoSubClass.GetGuoSub(sUpGuoName, false);
            gc.DownGuo = GuoSubClass.GetGuoSub(sDownGuoName, true);
            gc.GuoIndex = Array.IndexOf(GuoClass.GuoFullNames, gc.GuoFullName);
            gc.GuoName = GuoClass.GuoNames[gc.GuoIndex];
            return gc;
        }


        /// <summary>
        /// 根据爻的数据，只能是6个爻来创建卦类(六爻卦)
        /// </summary>
        /// <param name="iYaos"></param>
        /// <returns></returns>
        public static GuoClass GetGuoClass(int[] iYaos)
        {
            List<int> al = new List<int>();
            for (int i = 0; i < iYaos.Length; i++)
            {
                al.Add(iYaos[i]);
            }
            return GetGuoClass(al);
        }

        /// <summary>
        /// 根据爻的数据，只能是6个爻来创建卦类(六爻卦)
        /// </summary>
        /// <param name="iYaos"></param>
        /// <returns></returns>
        public static GuoClass GetGuoClass(List<int> iYaos)
        {

            if (iYaos.Count != 6) return null;

            GuoSubClass gsc = GuoSubClass.GetGuoSub(iYaos[0] % 2, iYaos[1] % 2, iYaos[2] % 2, true);
            if (gsc == null) return null;
            String sAttrName2 = gsc.GuoSubAttrName;
            gsc = GuoSubClass.GetGuoSub(iYaos[3] % 2, iYaos[4] % 2, iYaos[5] % 2, false);
            if (gsc == null) return null;
            String sAttrName1 = gsc.GuoSubAttrName;
            String sAttrName = "";
            if (sAttrName1.Equals(sAttrName2)) sAttrName = sAttrName1; else sAttrName = sAttrName1 + sAttrName2 + "?";
            GuoClass gc = GetGuoClass(sAttrName);
            if (gc.YaoDoing == null) gc.YaoDoing = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                if (iYaos[i] == 3 || iYaos[i] == 4)
                {
                    gc.YaoDoing.Add(i);//如果爻值是3或4，都是动爻，保存起来。
                    int iMod = i % 3;
                    if (i < 3) gc.DownGuo.Yaos[iMod] = iYaos[i]; else gc.UpGuo.Yaos[iMod] = iYaos[i];
                }

            }
            return gc;
        }
        /// <summary>
        ///  梅花占卦（先天），返回这个卦(注意，返回的卦，六亲，六神等没有加载)    
        /// </summary>
        /// <param name="iv1">为上卦</param>
        /// <param name="iv2">为下卦</param>
        /// <returns></returns>
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
    }
}
