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

namespace CompassEx.Guo
{
    public class FiveAttr
    {

        public enum FiveAttrRule : uint
        {
            MeCan = (0), MeBirth = (1)
                , SameMe = (2), CanMe = (3), BirthMe = 4

            //生我
        }

        //  public int RuleIndex = 0;

        //private void FiveAttrRule(int i)
        //{
        //    this.RuleIndex = i;
        //}


        public static String[] FiveAttrNames = { "金", "木", "水", "火", "土" };



        /**
        * 根据名称获得两个地支之间的五行的关系：我克、我生、我同、克我、生我的关系。
        * 0表示我克，1表示我生，2表示我同，3表示克我，4表示生我
*/
        public static FiveAttrRule GetBothLocRule(String MeLocName, String LocName)
        {
            int iMe = Array.IndexOf(LocClass.LocNames, MeLocName);
            int il = Array.IndexOf(LocClass.LocNames, LocName);
            FiveAttrRule far = GetBothLocRule(iMe, il);


            return far;

        }



        /**
        * 根据索引获得两个地支之间的五行的关系：我克、我生、我同、克我、生我的关系。
        * 0表示我克，1表示我生，2表示我同，3表示克我，4表示生我
*/
        public static FiveAttrRule GetBothLocRule(int iMeLoc1, int iLoc2)
        {
            FiveAttrRule far = FiveAttrRule.MeCan;

            LocClass Mel = LocClass.GetLocClass(iMeLoc1);
            LocClass l2 = LocClass.GetLocClass(iLoc2);
            if (Mel.FiveAttrName.Equals(l2.FiveAttrName))
            {
                far = FiveAttrRule.SameMe;//同我,兄弟
            }
            else
            {
                int iMePos = Array.IndexOf(FiveAttrNames, Mel.FiveAttrName);//我的五行位置
                int iPos = Array.IndexOf(FiveAttrNames, l2.FiveAttrName);//对方的五行位置
                                                                         //----------------------我克和克我---------------------------
                String sFiveAttrBothCan = FiveAttrBothCan();
                sFiveAttrBothCan += sFiveAttrBothCan;//增加一次，看作连续数做对比

                String s = iMePos.ToString() + iPos.ToString();//加起来看看能不能找到旁边的
                int i = sFiveAttrBothCan.IndexOf(s);
                if (i > -1)
                {
                    far = FiveAttrRule.MeCan;//我克
                    return far;
                }
                s = iPos.ToString() + iMePos.ToString();//加起来看看能不能找到旁边的
                i = sFiveAttrBothCan.IndexOf(s);
                if (i > -1)
                {
                    far = FiveAttrRule.CanMe;//克我
                    return far;
                }
                //----------------------我克和克我---------------------------

                //----------------------我生和生我---------------------------
                String sFiveAttrBothBirth = FiveAttrBothbirth();
                sFiveAttrBothBirth += sFiveAttrBothBirth;//增加一次，看作连续数做对比

                s = iMePos.ToString() + iPos.ToString();//加起来看看能不能找到旁边的
                i = sFiveAttrBothBirth.IndexOf(s);
                if (i > -1)
                {
                    far = FiveAttrRule.MeBirth;//我生
                    return far;
                }
                s = iPos.ToString() + iMePos.ToString();//加起来看看能不能找到旁边的
                i = sFiveAttrBothBirth.IndexOf(s);
                if (i > -1)
                {
                    far = FiveAttrRule.BirthMe;//生我
                    return far;
                }
                //----------------------我生和生我---------------------------


            }

            return far;

        }


        /**
        * 根据属性名称获得五行之间的五行的关系：我克、我生、我同、克我、生我的关系。
        * 0表示我克，1表示我生，2表示我同，3表示克我，4表示生我
*/
        public static FiveAttrRule GetBothAttrRule(String MeAttrName, String OtherAttrName)
        {
            int iMe = Array.IndexOf(FiveAttrNames, MeAttrName);
            int il = Array.IndexOf(FiveAttrNames, OtherAttrName);
            FiveAttrRule far = GetBothAttrRule(iMe, il);

            return far;

        }

        /**
        * 根据索引获得两个五行的关系：我克、我生、我同、克我、生我的关系。
        * 0表示我克，1表示我生，2表示我同，3表示克我，4表示生我
*/
        public static FiveAttrRule GetBothAttrRule(int iMeAttr, int iOtherAttr)
        {
            FiveAttrRule far = FiveAttrRule.MeCan;

            if (iMeAttr == iOtherAttr)
            {
                far = FiveAttrRule.SameMe;//同我,兄弟
            }
            else
            {

                //----------------------我克和克我---------------------------
                String sFiveAttrBothCan = FiveAttrBothCan();
                sFiveAttrBothCan += sFiveAttrBothCan;//增加一次，看作连续数做对比

                String s = iMeAttr.ToString() + iOtherAttr.ToString();//加起来看看能不能找到旁边的
                int i = sFiveAttrBothCan.IndexOf(s);
                if (i > -1)
                {
                    far = FiveAttrRule.MeCan;//我克
                    return far;
                }
                s = iOtherAttr.ToString() + iMeAttr.ToString();//加起来看看能不能找到旁边的
                i = sFiveAttrBothCan.IndexOf(s);
                if (i > -1)
                {
                    far = FiveAttrRule.CanMe;//克我
                    return far;
                }
                //----------------------我克和克我---------------------------

                //----------------------我生和生我---------------------------
                String sFiveAttrBothBirth = FiveAttrBothbirth();
                sFiveAttrBothBirth += sFiveAttrBothBirth;//增加一次，看作连续数做对比

                s = iMeAttr + iOtherAttr.ToString();//加起来看看能不能找到旁边的
                i = sFiveAttrBothBirth.IndexOf(s);
                if (i > -1)
                {
                    far = FiveAttrRule.MeBirth;//我生
                    return far;
                }
                s = iOtherAttr.ToString() + iMeAttr;//加起来看看能不能找到旁边的
                i = sFiveAttrBothBirth.IndexOf(s);
                if (i > -1)
                {
                    far = FiveAttrRule.BirthMe;//生我
                    return far;
                }
                //----------------------我生和生我---------------------------


            }

            return far;

        }


        /**
        * 五行相克的顺序，可以一个字符取出后转为int后，FiveAttrNames[i],这样读出
*/
        public static String FiveAttrBothCan()
        {
            String s = "42301";
            return s;
        }



        /**
        * 五行相生的顺序，可以一个字符取出后转为int后，FiveAttrNames[i],这样读出
*/
        public static String FiveAttrBothbirth()
        {
            String s = "40213";
            return s;
        }



        /**
        * 地支相害
*/
        public static String BothHarm(String sLoc1, String sLoc2)
        {
            String[] S = { sLoc1, sLoc2 };
            int[] r = { -1, -1 };
            foreach (string s in S)
            {
                var i = Array.IndexOf(LocClass.LocNames, s);
                var j = 0;
                if (i > -1)
                {
                    r[j] = i;
                    j++;
                }

            }

            if (r.Length == 0) return "";
            int[] it = { 0, 7 };//子与未害 
            if (it[0] == r[0] && it[1] == r[1]) return "子与未害";

            it[0] = 1;//丑与午害 
            it[1] = 6;
            if (it[0] == r[0] && it[1] == r[1]) return "丑与午害";

            it[0] = 2;//寅与巳害 
            it[1] = 5;
            if (it[0] == r[0] && it[1] == r[1]) return "寅与巳害";

            it[0] = 3;//卯与辰害
            it[1] = 4;
            if (it[0] == r[0] && it[1] == r[1]) return "卯与辰害";


            it[0] = 8;//申与亥害
            it[1] = 11;
            if (it[0] == r[0] && it[1] == r[1]) return "申与亥害";


            it[0] = 9;//酉与戌害
            it[1] = 10;
            if (it[0] == r[0] && it[1] == r[1]) return "酉与戌害";



            return "";
        }

        /**
        * 地支相刑
*/
        public static String BothTorture(String sLoc1, String sLoc2)
        {
            String[] S = { sLoc1, sLoc2 };
            int[] r = { -1, -1 };
            foreach (string s in S)
            {
                var i = Array.IndexOf(LocClass.LocNames, s);
                var j = 0;
                if (i > -1)
                {
                    r[j] = i;
                    j++;
                }

            }

            int[] it = { 2, 5 };//寅刑巳 
            if (it[0] == r[0] && it[1] == r[1]) return "恃势之刑";

            it[0] = 5;// 巳刑申 
            it[1] = 8;
            if (it[0] == r[0] && it[1] == r[1]) return "恃势之刑";
            it[0] = 2;// 申刑寅 
            it[1] = 8;
            if (it[0] == r[0] && it[1] == r[1]) return "恃势之刑";



            it[0] = 1;//未刑丑
            it[1] = 7;
            if (it[0] == r[0] && it[1] == r[1]) return "无恩之刑";
            it[0] = 1;// 丑刑戌 
            it[1] = 10;
            if (it[0] == r[0] && it[1] == r[1]) return "无恩之刑";
            it[0] = 7;//戌刑未 
            it[1] = 10;
            if (it[0] == r[0] && it[1] == r[1]) return "无恩之刑";


            it[0] = 0;//子刑卯 
            it[1] = 3;
            if (it[0] == r[0] && it[1] == r[1]) return "无礼之刑";


            //辰午酉亥自刑

            if (sLoc1.Equals(sLoc2))
            {
                if (sLoc1.Equals("辰") || sLoc1.Equals("午") || sLoc1.Equals("酉") || sLoc1.Equals("亥")) return "自刑";
            }


            return "";
        }

        /**
        * 地支相冲
*/
        public static String BothConflict(String sLoc1, String sLoc2)
        {
            int iLoc1 = Array.IndexOf(LocClass.LocNames, sLoc1);
            int iLoc2 = Array.IndexOf(LocClass.LocNames, sLoc2);


            if (Math.Abs(iLoc1 - iLoc2) == 6) return "相冲";


            return "";
        }





        /**
        * 地支六合
*/
        public static String LocCombine(String sLoc1, String sLoc2)
        {

            String[] S = { sLoc1, sLoc2 };
            int[] r = { -1, -1 };
            foreach (string s in S)
            {
                var i = Array.IndexOf(LocClass.LocNames, s);
                var j = 0;
                if (i > -1)
                {
                    r[j] = i;
                    j++;
                }

            }
            int[] it = { 0, 1 };//子丑合化土
            if (it[0] == r[0] && it[1] == r[1]) return "土";

            it[0] = 4;
            it[1] = 9;//辰酉合化金
            if (it[0] == r[0] && it[1] == r[1]) return "金";

            it[0] = 5;
            it[1] = 8;//巳申合化水
            if (it[0] == r[0] && it[1] == r[1]) return "水";

            it[0] = 2;
            it[1] = 11;//寅亥合化水
            if (it[0] == r[0] && it[1] == r[1]) return "木";

            it[0] = 3;
            it[1] = 10;//卯戌合化火
            if (it[0] == r[0] && it[1] == r[1]) return "火";

            it[0] = 6;
            it[1] = 7;//卯戌合化火
            if (it[0] == r[0] && it[1] == r[1]) return "日月";

            return "";
        }


        /**
        * 天干五合
*/
        public static String SkyCombine(String sSky1, String sSky2)
        {


            String[] S = { sSky1, sSky1 };
            int[] r = { -1, -1 };
            foreach (string s in S)
            {
                var i = Array.IndexOf(LocClass.LocNames, s);
                var j = 0;
                if (i > -1)
                {
                    r[j] = i;
                    j++;
                }

            }


            int[] it = { 0, 5 };//甲己合化土
            if (it[0] == r[0] && it[1] == r[1]) return "土";

            it[0] = 1;
            it[1] = 6;//乙庚合化金
            if (it[0] == r[0] && it[1] == r[1]) return "金";

            it[0] = 2;
            it[1] = 7;//乙庚合化金
            if (it[0] == r[0] && it[1] == r[1]) return "水";

            it[0] = 3;
            it[1] = 8;//丁壬合化木
            if (it[0] == r[0] && it[1] == r[1]) return "木";

            it[0] = 4;
            it[1] = 9;//丁壬合化木
            if (it[0] == r[0] && it[1] == r[1]) return "火";

            return "";
        }
    }
}
