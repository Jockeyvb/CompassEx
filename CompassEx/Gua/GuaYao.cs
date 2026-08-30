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

using CompassEx.Assist;
using CompassEx.Comm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CompassEx.Gua
{
    public class GuaYao
    {

        /// <summary>
        /// 六爻的固定名称
        /// </summary>
        public static readonly string[] SixYaoNames = ["初爻", "二爻", "三爻", "四爻", "五爻", "上爻"];

        /// <summary>
        /// 阴、阳、老阴、老阳爻的值
        /// </summary>
        public static readonly int[] Values = [0, 1, 2, 3];
        /// <summary>
        /// 名称
        /// </summary>
        public static readonly string[] Names = ["阴", "阳", "老阴", "老阳"];
        /// <summary>
        /// 爻对应的图形
        /// </summary>
        public static readonly string[] Faces = ["━ ━ ", "━━━ ", "━ ━x", "━━━o"];

        /// <summary>
        /// 爻所在的位置0为初爻
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// 爻值
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// 爻名称
        /// </summary>
        public string? Name { get { return Names[Values.IndexOf(this.Value)]; } }

        /// <summary>
        /// 爻图形
        /// </summary>
        public string? Face { get { return Faces[Values.IndexOf(this.Value)]; } }


        /// <summary>
        /// 爻中的干支
        /// </summary>
        public SkyLoc SkyLoc { get; private set; } = default!;

        /// <summary>
        /// 爻中的六亲
        /// </summary>
        public SixRelativeClass? SixRelative { get; private set; } = default!;

        /// <summary>
        /// 伏神(六亲)(无伏神时为null)
        /// </summary>
        public SixRelativeClass? HideRelative { get; private set; } = default!;

        /// <summary>
        /// 是否为世爻
        /// </summary>
        public bool IsHereYao { get; private set; } = false;

        /// <summary>
        /// 是否为应爻
        /// </summary>
        public bool IsThereYao { get; private set; } = false;


        private bool _IsDoing = false;
        /// <summary>
        /// 是否爻动(一般使用GuaClass 中的 SetYaoDoing()方法设定 ）
        /// </summary>
        public bool IsDoing
        {
            get => _IsDoing;
            internal set
            {
                _IsDoing = value;
                this.Value = _IsDoing ? (this.Value % 2) + 2 : this.Value % 2; //设置爻动或取消爻动 

            }
        }

        /// <summary>
        /// 将要变出的爻值(如果不是动爻则返回原来的爻值<see cref="Value"/>
        /// </summary>
        public int ChangingValue { get { return this.Value > 1 ? (this.Value - 1) % 2 : this.Value; } }


        /// <summary>
        /// 六神
        /// </summary>
        public SixGodClass SixGod { get; private set; } = default!;




        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="GuaYaoValue">卦爻值</param>
        private GuaYao(int GuaYaoValue)
        {
            if (GuaYaoValue < 0 || GuaYaoValue >= Values.Max()) throw new ArgumentException($"卦爻值异常：{GuaYaoValue}");

            this.Value = GuaYaoValue;
        }

        /// <summary>
        /// 获得本爻的HTML
        /// </summary>
        /// <param name="Heightpx"></param>
        /// <param name="Style"></param>
        /// <param name="markStyle"></param>
        /// <returns></returns>
        public string GetFaceToHTML(int Heightpx = 24, string Style = "", string markStyle = "")
        {
            return GetYaoFaceToHTML(this.Value, Heightpx, Style, markStyle);

        }


        /// <summary>
        /// 加载六爻卦的卦爻其它属性
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static void LoadGuaYaos(GuaClass g, SkyClass DaySky = null!)
        {
            if (g == null!) throw new ArgumentNullException(nameof(g));


            LoadGuaSelfandHereThere(g); //加载世应、卦宫与卦类的别名
            LoadSixRelative(g);//加载六亲
            if (DaySky != null!)
            {
                LoadSixGod(g, DaySky); //加载六神
            }



        }




        /// <summary>
        /// 获得三爻卦卦爻类(只加载干支)
        /// </summary>
        /// <param name="gs"></param>
        /// <returns></returns>
        public static List<GuaYao>? GetGuaSubYaos(GuaSubClass gs)
        {
            if (gs == null!) throw new ArgumentNullException(nameof(gs));
            if (gs.Name == "黄") return null;//五黄无卦
            int[] GuaYaoValues = GuaSubClass.GuaSubYaoValues[gs.Name];
            if (GuaYaoValues.Length != 3) throw new ArgumentException("卦爻数x组必须为3的长度" + nameof(GuaYaoValues));
            List<GuaYao> ls = new List<GuaYao>();
            bool IsDownGua = gs.IsDownGua;
            int SkyIndex = 0;
            int LocIndex = 0;
            switch (gs.BeforeGuaIndex)
            {
                case 0: //乾卦

                    SkyIndex = IsDownGua ? 0 : 8;//内甲外壬
                    LocIndex = IsDownGua ? 0 : 6;//内子外午
                    break;
                case 1: //兑卦

                    SkyIndex = 3;//丁
                    LocIndex = IsDownGua ? 5 : 11;//内巳外亥
                    break;
                case 2: //离卦

                    SkyIndex = 5;//内己
                    LocIndex = IsDownGua ? 3 : 9;//内卯外酉
                    break;
                case 3: //震卦

                    SkyIndex = 6;//庚
                    LocIndex = IsDownGua ? 0 : 6;//内子外午
                    break;
                case 4: //巽卦

                    SkyIndex = 7;//辛
                    LocIndex = IsDownGua ? 1 : 7;//内丑外未
                    break;
                case 5: //坎卦

                    SkyIndex = 4;//戊
                    LocIndex = IsDownGua ? 2 : 8;//内寅外申
                    break;
                case 6: //艮卦

                    SkyIndex = 2;//丙
                    LocIndex = IsDownGua ? 4 : 10;//内辰外戌
                    break;
                case 7: //坤卦

                    SkyIndex = IsDownGua ? 1 : 9;//内乙外癸
                    LocIndex = IsDownGua ? 7 : 1;//内子外午
                    break;

            }

            if (gs.IsSun)//阳卦顺行
            {
                for (int i = 0; i < 3; i++)
                {
                    GuaYao gy = new GuaYao(GuaYaoValues[i]);
                    gy.Index = gs.IsDownGua ? i % 3 : i % 3 + 3; //上下卦的爻位
                    gy.SkyLoc = new SkyLoc(SkyIndex, LocIndex);

                    LocIndex = LocIndex + 2 > 11 ? 0 : LocIndex + 2;

                    ls.Add(gy);

                }
            }
            else//阴卦逆行
            {
                for (int i = 0; i < GuaYaoValues.Length; i++)
                {
                    GuaYao gy = new GuaYao(GuaYaoValues[i]);
                    gy.Index = gs.IsDownGua ? i % 3 : i % 3 + 3; //上下卦的爻位
                    gy.SkyLoc = new SkyLoc(SkyIndex, LocIndex);

                    LocIndex = LocIndex - 2 < 0 ? 11 : LocIndex - 2;//逆

                    ls.Add(gy);

                }
            }
            return ls;




        }

        /// <summary>
        /// 六爻卦加载六神 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="DaySky"></param>
        private static void LoadSixGod(GuaClass g, SkyClass DaySky)
        {
            //==============加载六神============================
            if (g == null) throw new ArgumentNullException(nameof(g));
            if (DaySky == null) throw new ArgumentNullException(nameof(DaySky));

            var ls = SixGodClass.GetSixGod(DaySky).ToList();
            for (int i = 0; i < ls.Count; i++)
            {
                g.Yaos[i].SixGod = ls[i];
            }

            //==============加载六神============================
        }

        /// <summary>
        /// 因为变卦需要按主卦的卦宫来生成正确的伏神六亲，所以需要使用此方法来加载
        /// </summary>
        /// <returns></returns>
        public SixRelativeClass? GetHideRelativeByGuaSelf(GuaSubClass GuaSelf)
        {
            if (this.HideRelative == null) return null;
            FiveAttrRule far = FiveAttr.GetBothAttrRule(GuaSelf.FiveAttr.Name, this.HideRelative.SkyLoc.Loc.FiveAttr.Name);

            SixRelativeClass src = SixRelativeClass.GetSixRelative(far);
            src.SkyLoc = this.HideRelative.SkyLoc;
            return src;
        }



        /// <summary>
        /// 因为变卦需要按主卦的卦宫来生成正确的六亲，所以需要使用此方法来加载
        /// </summary>
        /// <returns></returns>
        public SixRelativeClass GetSixRelativeByGuaSelf(GuaSubClass GuaSelf)
        {

            FiveAttrRule far = FiveAttr.GetBothAttrRule(GuaSelf.FiveAttr.Name, this.SkyLoc.Loc.FiveAttr.Name);
            SixRelativeClass src = SixRelativeClass.GetSixRelative(far);
            return src;

        }


        /// <summary>
        /// 核心方法：根据本宫纯卦五行与地支纳音生克关系，全自动装配主卦初爻至上爻的“六亲”属性。
        /// </summary>
        /// <remarks>
        /// <b>装卦生克数理逻辑：</b><br/>
        /// 方法会依次轮询下卦（贞卦）和上卦（悔卦）共六个爻位的纳支（<see cref="LocClass"/>）。<br/>
        /// 调用核心交叉规则 <see cref="FiveAttr.GetBothAttrRule(string,string)"/>，以母本卦宫的五行属性（我）为基准点，与各爻位纳支五行（他）进行“生我者父母、我生者子孙、克我者官鬼、我克者妻财、比和者兄弟”的五行断立，最终写入 <see cref="SixRelative"/> 列表。
        /// </remarks>
        private static void LoadSixRelative(GuaClass g)
        {
            //================================加载六亲================================
            if (g == null || g.GuaSelf == null) throw new Exception("六爻卦或卦宫不存在！ ");
            List<SixRelativeClass> srcs = new List<SixRelativeClass>();
            for (int i = 0; i < 3; i++)
            {
                LocClass lc = g.DownGua.Yaos[i].SkyLoc.Loc;
                FiveAttrRule far = FiveAttr.GetBothAttrRule(g.GuaSelf.FiveAttr.Name, lc.FiveAttr.Name);
                SixRelativeClass src = SixRelativeClass.GetSixRelative(far);
                src.SkyLoc = g.DownGua.Yaos[i].SkyLoc; //保存干支（伏神时可以使用）
                g.DownGua.Yaos[i].SixRelative = src;
            }
            for (int i = 0; i < 3; i++)
            {
                LocClass lc = g.UpGua.Yaos[i].SkyLoc.Loc;
                FiveAttrRule far = FiveAttr.GetBothAttrRule(g.GuaSelf.FiveAttr.Name, lc.FiveAttr.Name);
                SixRelativeClass src = SixRelativeClass.GetSixRelative(far);
                src.SkyLoc = g.UpGua.Yaos[i].SkyLoc; //保存干支（伏神时可以使用）
                g.UpGua.Yaos[i].SixRelative = src;
            }

            //================================加载六亲================================
            //================================加载伏神================================
            List<GuaYao> ls = g.DownGua.Yaos.Union(g.UpGua.Yaos).ToList();

            var SR = ls.Select(x => x.SixRelative.Index).Distinct().ToList();
            bool IsHadHide = (SR.Count < SixRelativeClass.SixRelativeNames.Length); //小于所有六亲长度则有伏神
            if (!IsHadHide) return;
            //bool TorF = false;
            //for (int i = 0; i < SixRelativeClass.SixRelativeNames.Length; i++)
            //{
            //    for (int j = 0; j < 6; j++)
            //    {
            //        SixRelativeClass src = ls[j].SixRelative;
            //        TorF = i == src.Index;

            //        if (TorF == true) break;//找到
            //    }
            //    if (TorF == false) break;//找不到,表示有伏神

            //}
            //if (TorF == false) return;//表示没有伏神，退出




            GuaClass gc = g.GuaSelf.ToGuaClass();//转为六爻卦
            gc.LoadAllYaos();//必须先加载卦宫的爻数据

            List<int> hsrys = new List<int>();//六亲爻位
            string sHad = "";
            for (int i = 0; i < 6; i++)
            {
                SixRelativeClass src = ls[i].SixRelative;
                sHad += src.Index;
            }
            for (int i = 0; i < 6; i++)
            {
                SixRelativeClass src = gc.Yaos[i].SixRelative;

                if (sHad.IndexOf(src.Index.ToString()) == -1)
                {
                    //src.YaoPosIndex = i;

                    ls[i].HideRelative = src;//保存这个六亲作为伏神
                                             //  hsrys.Add(i);//保存位置

                }
            }

            //================================加载伏神================================

        }


        /// <summary>
        /// 加载六爻卦的世应和卦宫及卦类别名
        /// </summary>
        /// <param name="g">六爻卦类</param>
        /// <exception cref="Exception"></exception>
        private static void LoadGuaSelfandHereThere(GuaClass g)
        {


            if (g.DownGua == null || g.UpGua == null) throw new Exception("六爻卦不能为null！");
            //=======================归藏定卦宫法==================================
            int[] Yaos = { 0, 0, 0 };
            Yaos[0] = (g.DownGua.Yaos[0].Value % 2) + (g.UpGua.Yaos[0].Value % 2);//下爻
            Yaos[1] = (g.DownGua.Yaos[1].Value % 2) + (g.UpGua.Yaos[1].Value % 2);//中爻
            Yaos[2] = (g.DownGua.Yaos[2].Value % 2) + (g.UpGua.Yaos[2].Value % 2);//上爻
            for (int i = 0; i < 3; i++)
            {
                Yaos[i] = Yaos[i] % 2;
            }

            string sn = GuaSubClass.GuaSubYaoValues.Where(x => x.Key != "黄" && x.Value.SequenceEqual(Yaos)).Select(x => x.Key).FirstOrDefault();
            //=======================归藏定卦宫法==================================
            // string sn = GuaSubClass.GuaSubYaoValues.Where(x => x.Value == Yaos).Select(x => x.Key).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(sn)) throw new Exception("没找到相关卦名");

            GuaSubClass gsc = new GuaSubClass(sn);//结合后，看是什么卦(归藏定卦宫法）
            List<GuaYao> ls = g.DownGua.Yaos.Union(g.UpGua.Yaos).ToList();

            if (gsc.Name.Equals("乾"))
            {//乾卦，世在3，卦宫是外卦
                ls[2].IsHereYao = true;//  = 2;
                ls[5].IsThereYao = true;// 5;
                g.GuaSelf = g.UpGua;
            }
            else if (gsc.Name.Equals("兑"))
            {//兑卦，世在2，卦宫是外卦
                ls[1].IsHereYao = true; //  1;
                ls[4].IsThereYao = true;// 4;
                g.GuaSelf = g.UpGua;
            }
            else if (gsc.Name.Equals("震"))
            {//震卦，世在初，卦宫是外卦
                ls[0].IsHereYao = true;// 0;
                ls[3].IsThereYao = true;// 3;
                g.GuaSelf = g.UpGua;
            }
            else if (gsc.Name.Equals("巽"))
            {//巽卦，世在4，卦宫是内卦全反
                ls[3].IsHereYao = true;// 3;
                ls[0].IsThereYao = true;// 0;
                g.GuaSelf = g.DownGua.GetXorGua();//取反卦
            }
            else if (gsc.Name.Equals("艮"))
            {//艮卦，世在5，卦宫是内卦全反
                ls[4].IsHereYao = true; // 4;
                ls[1].IsThereYao = true;// 1;
                g.GuaSelf = g.DownGua.GetXorGua();//取反卦
            }
            else if (gsc.Name.Equals("坤"))
            {//坤卦，世在6，卦宫是本身（纯卦)
                ls[5].IsHereYao = true;// 5;
                ls[2].IsThereYao = true;// 2;
                g.GuaSelf = g.DownGua;
                string san = "纯卦";
                if (!g.GuaAlias.Contains(san)) g.GuaAlias.Add(san);

            }
            else if (gsc.Name.Equals("离"))
            {//离卦(游魂卦)，世在4，卦宫是内卦全反
                ls[3].IsHereYao = true;// 3;
                ls[0].IsThereYao = true;// 0;
                g.GuaSelf = g.DownGua.GetXorGua();//取反卦
                string san = "游魂卦";
                if (!g.GuaAlias.Contains(san)) g.GuaAlias.Add(san);

            }
            else if (gsc.Name.Equals("坎"))
            {//离卦(归魂卦)，世在3，卦宫是内卦
                ls[2].IsHereYao = true;// 2;
                ls[5].IsThereYao = true;// 5;
                g.GuaSelf = g.DownGua;

                string san = "归魂卦";
                if (!g.GuaAlias.Contains(san)) g.GuaAlias.Add(san);
            }

            //=======================设置其他的卦类别名====================

            bool TorF = false; String sValue = "";
            List<LocClass> lcs1 = g.DownGua.Yaos.Select(x => x.SkyLoc.Loc).ToList();
            List<LocClass> lcs2 = g.UpGua.Yaos.Select(x => x.SkyLoc.Loc).ToList();
            for (int i = 0; i < 3; i++)
            {
                sValue = FiveAttr.LocCombine(lcs1[i].Name, lcs2[i].Name);
                TorF = sValue.Length > 0;
                if (TorF == false) break;//不是六合卦
            }
            if (TorF)
            {
                string san = "六合卦";
                if (!g.GuaAlias.Contains(san)) g.GuaAlias.Add(san);


            }

            for (int i = 0; i < 3; i++)
            {
                sValue = FiveAttr.BothConflict(lcs1[i].Name, lcs2[i].Name);
                TorF = sValue.Length > 0;
                if (TorF == false) break;//不是六冲卦
            }
            if (TorF)
            {
                string san = "六冲卦";
                if (!g.GuaAlias.Contains(san)) g.GuaAlias.Add(san);


            }

            //=======================设置其他的卦类别名====================
        }



        /// <summary>
        /// 根据卦爻值获得卦爻形式
        /// </summary>
        /// <param name="GuaYaoValue"></param>
        /// <param name="IsAddName"></param>
        /// <returns></returns>
        public static string GetYaoFace(int GuaYaoValue, bool IsAddName = false)
        {
            string s = Faces[Values.IndexOf(GuaYaoValue)];
            if (IsAddName) s += Names[Values.IndexOf(GuaYaoValue)];
            return s;
        }

        /// <summary>
        /// 把爻显示转成HTML
        /// </summary>
        /// <param name="GuaYaoValue"></param>
        /// <param name="Heightpx"></param>
        /// <param name="Style"></param>
        /// <param name="markStyle"></param>
        /// <returns></returns>
        public static string GetYaoFaceToHTML(int GuaYaoValue, int Heightpx = 24, string Style = "", string markStyle = "")
        {

            string sYin = @$"
                        <div class=""yin"" style='height:{Heightpx}px;{Style}'>
        <span></span>
        <span></span>

    </div>

";
            string sYang = $"<div class=\"yang\" style='height:{Heightpx}px;{Style}'></div>";
            string st = GuaYaoValue % 2 == 0 ? sYin : sYang;
            string sm = $"<span class=\"mark fw-bold \" style='font-size:{Heightpx * 1.5f}px; {markStyle}'>&nbsp;</span>"; ;
            if (GuaYaoValue == 3) sm = $"<span class=\"mark fw-bold\" style='font-size:{Heightpx * 1.5f}px;{markStyle}' >o</span>";
            if (GuaYaoValue == 2) sm = $"<span class=\"mark fw-bold\" style='font-size:{Heightpx * 1.5f}px;{markStyle}'>x</span>";

            st += sm;

            return st;
        }




        public static implicit operator int(GuaYao a)
        {
            // 如果对象为 null，可以根据业务返回 0 或抛出异常
            if (a == null) return 0;

            // 直接返回其 Value 属性
            return a.Value;
        }

    }


}
