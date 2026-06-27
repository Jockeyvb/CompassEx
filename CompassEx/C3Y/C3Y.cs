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
using CompassEx.Guo;
using System.Collections.Generic;

namespace CompassEx.C3Y
{
    /// <summary>
    /// 三元罗盘类
    /// </summary>
    public class C3Y
    {




        /// <summary>
        ///加载并初始化罗盘上的所有后天64卦（地盘）对象，按照顺时针方向排列，从坤卦开始，每5.625度一个卦逆时针，共64个卦
        /// </summary>
        /// <returns></returns>
        public static Dictionary<CompassRangEX, GuoClass> GetAllCAfterGuos()
        {


            //罗盘上后天排行从子为乾9、兑4、离3、震8（阳仪），巽2、坎7、艮6、坤1午（阴仪）
            //实际是按先天卦排名索引，乾1、兑2、离3、震4（阳仪），巽5、坎6、艮7、坤8午（阴仪）
            //并把乾(360度）置上卦为卦宫，下卦相荡而成
            double baseDegree = 360;//罗盘360-5.625=354.375，则354.375至360度为坤卦
            double dEnd = baseDegree;
            Dictionary<CompassRangEX, GuoClass> dc = new Dictionary<CompassRangEX, GuoClass>();
            //--------------------阳仪32卦------------------
            for (int i = 0; i < 4; i++) //至震
            {
                GuoSubClass gu = GuoSubClass.GetGuoSub(i, true); //上卦 

                for (int j = 0; j < 8; j++)//按1-8卦相荡(下卦）顺
                {
                    GuoSubClass gd = GuoSubClass.GetGuoSub(j, false); //下卦 
                    List<int> iYao = new List<int>();
                    iYao.AddRange(gd.Yaos);//相荡
                    iYao.AddRange(gu.Yaos);//卦宫

                    GuoClass g = GuoClass.GetGuoClass(iYao.ToArray()); //根据六爻数获得64卦对象

                    CompassRangEX rang = new CompassRangEX(dEnd - CompassEx.CompassGuoDegree, dEnd); //范围
                    dEnd = dEnd - CompassEx.CompassGuoDegree;
                    dc.Add(rang, g);  //范围对象作为key，卦对象作为value添加到字典中
                }

            }
            //--------------------阳仪32卦------------------

            //---------------------阴仪32卦----------------------------------

            for (int i = 7; i > 3; i--)
            {
                GuoSubClass gu = GuoSubClass.GetGuoSub(i, true); //上卦 
                for (int j = 7; j > -1; j--)//按8-1卦相荡(下卦）逆
                {
                    GuoSubClass gd = GuoSubClass.GetGuoSub(j, false); //下卦
                    List<int> iYao = new List<int>();
                    iYao.AddRange(gd.Yaos);
                    iYao.AddRange(gu.Yaos);
                    GuoClass g = GuoClass.GetGuoClass(iYao.ToArray()); //根据六爻数获得64卦对象
                    CompassRangEX rang = new CompassRangEX(dEnd - CompassEx.CompassGuoDegree, dEnd); //范围
                    dEnd = dEnd - CompassEx.CompassGuoDegree;
                    dc.Add(rang, g);  //范围对象作为key，卦对象作为value添加到字典中
                }
            }

            //---------------------阴仪32卦----------------------------------

            return dc;

        }






        /// <summary>
        ///加载罗盘上的所有先天64卦（天盘）对象，按照顺时针方向排列，从坤卦开始，每5.625度一个卦逆时针，共64个卦
        /// </summary>
        /// <returns></returns>
        public static Dictionary<CompassRangEX, GuoClass> GetAllBeforGuos()
        {
            double baseDegree = 360;//罗盘360-5.625=354.375，则354.375至360度为坤卦
            double dEnd = baseDegree;
            Dictionary<CompassRangEX, GuoClass> dc = new Dictionary<CompassRangEX, GuoClass>();
            //--------------------//阴从右边道相通------------------
            for (int i = 7; i > 3; i--)
            {
                GuoSubClass gd = GuoSubClass.GetGuoSub(i, true); //下卦（从坤右边转相荡至巽）

                for (int j = 7; j > -1; j--)
                {
                    GuoSubClass gu = GuoSubClass.GetGuoSub(j, false); //上卦（从坤右边转相荡至乾）
                    List<int> iYao = new List<int>();
                    iYao.AddRange(gd.Yaos);
                    iYao.AddRange(gu.Yaos);
                    GuoClass g = GuoClass.GetGuoClass(iYao.ToArray()); //根据六爻数获得64卦对象

                    CompassRangEX rang = new CompassRangEX(dEnd - CompassEx.CompassGuoDegree, dEnd); //范围
                    dEnd = dEnd - CompassEx.CompassGuoDegree;
                    dc.Add(rang, g);  //范围对象作为key，卦对象作为value添加到字典中
                }

            }
            //--------------------//阴从右边道相通------------------

            //---------------------阳从左边团团转----------------------------------

            for (int i = 0; i < 4; i++)
            {
                GuoSubClass gd = GuoSubClass.GetGuoSub(i, true); //下卦（从乾左边转相荡至震）
                for (int j = 0; j < 8; j++)
                {
                    GuoSubClass gu = GuoSubClass.GetGuoSub(j, false); //上卦（从乾左边转相荡至坤）
                    List<int> iYao = new List<int>();
                    iYao.AddRange(gd.Yaos);
                    iYao.AddRange(gu.Yaos);
                    GuoClass g = GuoClass.GetGuoClass(iYao.ToArray()); //根据六爻数获得64卦对象
                    CompassRangEX rang = new CompassRangEX(dEnd - CompassEx.CompassGuoDegree, dEnd); //范围
                    dEnd = dEnd - CompassEx.CompassGuoDegree;
                    dc.Add(rang, g);  //范围对象作为key，卦对象作为value添加到字典中
                }
            }

            //---------------------阳从左边团团转----------------------------------

            return dc;

        }


    }
}
