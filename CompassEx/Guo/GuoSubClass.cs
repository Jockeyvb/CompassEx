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

        /// <summary>
        /// 先天八卦的属性
        /// </summary>
        public readonly static String[] BeforeGuoSubAttrNames = { "天", "泽", "火", "雷", "风", "水", "山", "地" };//先天八卦的属性
        /// <summary>
        /// 先天八卦的伦理关系
        /// </summary>
        public readonly static String[] BeforeGuoSubReluNames = { "父", "少女", "中女", "长男", "长女", "中男", "少男", "母" };//先天八卦的伦理关系
        /// <summary>
        /// 先天八卦的卦名
        /// </summary>
        public readonly static String[] BeforeGuoSubNames = { "乾", "兑", "离", "震", "巽", "坎", "艮", "坤" };//先天八卦的卦名
        /// <summary>
        /// 先天八卦的卦数
        /// </summary>
        public readonly static String[] BeforeGuoSubNumerics = { "一", "二", "三", "四", "五", "六", "七", "八" };//先天八卦的卦数
        /// <summary>
        /// 后天八卦的属性
        /// </summary>
        public readonly static String[] AfterGuoSubAttrNames = { "水", "地", "雷", "风", "黄", "天", "泽", "山", "火" };//后天八卦的属性
        /// <summary>
        /// 后天八卦的卦名
        /// </summary>
        public readonly static String[] AfterGuoSubNames = { "坎", "坤", "震", "巽", "黄", "乾", "兑", "艮", "离" };//后天八卦的卦名




        public readonly static string TiJiSymbol = "☯️";
        /// <summary>
        /// 按先天卦序的卦象符号
        /// </summary>
        public readonly static String[] Symbols = {

"\u2630" ,	//乾 (天)
"\u2631"    ,//兑 (泽)
"\u2632"    ,//离 (火)
"\u2633"    ,//震 (雷)
"\u2634"    ,//巽 (风)
"\u2635"    ,//坎 (水)
"\u2636"    ,//艮 (山)
"\u2637"    ,//坤 (地)

        
        };//后天八卦的卦名


        /// <summary>
        /// 是否为阳卦（后天卦前4位为阳)
        /// </summary>
        public bool IsSun { get { return "乾坎艮震".IndexOf(this.GuoSubName) > -1; } }

        /// <summary>
        /// 是否为下卦
        /// </summary>
        public bool IsDownGuo { get; private set; } = false;
        /// <summary>
        ///  卦气（洛数）先天五行数
        /// </summary>
        public GuoQi GuoQi
        {
            get
            {
                return new GuoQi(this);
            }

        }



        /// <summary>
        /// 后天八卦的卦数
        /// </summary>
        public readonly static String[] AfterGuoSubNumerics = { "一", "二", "三", "四", "五", "六", "七", "八", "九" };//后天八卦的卦数
        /// <summary>
        /// 后天八卦的伦理关系
        /// </summary>
        public readonly static String[] AfterGuoSubReluNames = { "中男", "母", "长男", "长女", "黄中", "父", "少女", "少男", "中女" };//
        /// <summary>
        /// 后天八卦的颜色
        /// </summary>
        public readonly static String[] AfterGuoSubColors = { "白", "黑", "碧", "绿", "黄", "白", "赤", "白", "紫" };//后天八卦的颜色
        /// <summary>
        /// 后天八卦的颜色类(Color)
        /// </summary>
        public readonly static Color[] AfterGuoSubColorClasses = { Color.White, Color.Black, Color.DarkGreen, Color.Green, Color.BurlyWood, Color.White, Color.Red, Color.White, Color.Purple };//后天八卦的颜色


        /// <summary>
        /// 三爻卦构造函数<br />
        /// 使用此方法创建的类默认为下卦或结构复卦（六爻卦）时请使用<see cref="GuoSubClass.GetGuoSub(String , bool )"/>方法,明确指定属于上卦还是下卦
        /// </summary>
        /// <param name="GuoName">卦名</param>    
        /// <exception cref="IndexOutOfRangeException"></exception>

        public GuoSubClass(string GuoName) : this(BeforeGuoSubNames.IndexOf(GuoName))
        {

        }

        /// <summary>
        /// 三爻卦构造函数<br />
        /// 使用此方法创建的类默认为下卦
        /// 创建复卦（六爻卦）时请使用<see cref=" GuoSubClass.GetGuoSub(String , bool )"/>方法,明确指定属于上卦还是下卦 
        /// <see cref=" GuoSubClass.GetGuoSub(String , bool )"/>
        /// </summary>
        /// <param name="iBeforGuoIndex">先天卦索引</param>    
        /// <exception cref="IndexOutOfRangeException"></exception>
        public GuoSubClass(int iBeforGuoIndex)
        {
            if (iBeforGuoIndex < 0 || iBeforGuoIndex > BeforeGuoSubNames.Length) throw new IndexOutOfRangeException();
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




        /// <summary>
        /// 爻数组
        /// </summary>
        public int[] Yaos { get; set; } = { 0, 0, 0 }; //爻数组
        /// <summary>
        /// 爻的纳甲天干
        /// </summary>        
        public String SkyName { get; private set; } //爻的纳甲天干

        public SkyClass Sky { get { return new SkyClass(this.LocIndex); } }




        /// <summary>
        /// 地支地爻位列表，同下而上
        /// </summary>
        public List<LocClass> Locs { get; private set; } //地支地爻位列表，由下而上

        /// <summary>
        /// 地支开始的位置
        /// </summary>
        public int LocIndex { get; private set; } //地支开始的位置
        /// <summary>
        /// 卦的伦理关系
        /// </summary>
        public String GuoSubReluName { get; private set; } //卦的伦理关系
        /// <summary>
        /// 后天八卦的颜色属性
        /// </summary>
        public String AfterGuoSubColor { get; private set; } //后天八卦的颜色属性



        /// <summary>
        /// 后天八卦数
        /// </summary>
        public int AfterQuantity { get { return AfterGuoSubNumerics.IndexOf(this.AfterGuoSubCNQuantity); } }//后天八卦数

        /// <summary>
        /// 先天八卦数
        /// </summary>
        public int BeforeQuanity { get { return BeforeGuoSubNumerics.IndexOf(this.BeforeGuoSubCNQuantity); } }//先天八卦数
        /// <summary>
        /// 卦名，例如：乾
        /// </summary>
        public String GuoSubName { get; private set; } //卦名，例如：乾
        /// <summary>
        /// 卦的属性名,例如：天
        /// </summary>
        public String GuoSubAttrName { get; private set; } //卦的属性名,例如：天
        /// <summary>
        /// 卦的五行属性
        /// </summary>
        public FiveAttr FiveAttr { get; private set; } //卦的属性





        /// <summary>
        /// 先天八卦数中文
        /// </summary>
        public string BeforeGuoSubCNQuantity { get { return BeforeGuoSubNumerics[this.BeforeGuoSubIndex]; } }

        /// <summary>
        /// 后天八卦数中文
        /// </summary>
        public string AfterGuoSubCNQuantity { get { return AfterGuoSubNumerics[this.AfterGuoSubIndex]; } }

        /// <summary>
        /// 先天八卦索引位置
        /// </summary>
        public int BeforeGuoSubIndex { get { return BeforeGuoSubNames.IndexOf(this.GuoSubName); } }

        /// <summary>
        /// 后天八卦索引位置
        /// </summary>
        public int AfterGuoSubIndex { get { return AfterGuoSubNames.IndexOf(this.GuoSubName); } }
        /// <summary>
        /// 卦象符号
        /// </summary>
        public string Symbol { get { return Symbols[this.BeforeGuoSubIndex]; } }

        /// <summary>
        /// 根据本卦从罗盘获得后天八卦中的度数范围对象 
        /// </summary>
        public CompassRangEX CAfterRangeDegree { get { return CompassEx.GetAfterGuoSubDegree(this.GuoSubName); } }

        /// <summary>
        /// 根据本卦从罗盘获得先天天八卦中的度数范围对象 
        /// </summary>
        public CompassRangEX CBeforRangeDegree { get { return CompassEx.GetBeforGuoSubDegree(this.GuoSubName); } }

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
            foreach (var kv in CompassEx.CBeforGuos)
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
