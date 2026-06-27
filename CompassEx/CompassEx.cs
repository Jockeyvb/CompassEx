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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
namespace CompassEx
{
    /// <summary>
    /// 罗盘的基本类
    /// </summary>

    public class CompassEx
    {
        /// <summary>
        /// 后天八卦的卦名罗盘上的度数(每卦45度）
        /// </summary>
        public const double GuoSubDegree = 45;
        /// <summary>
        /// 罗盘后天八卦的卦序列
        /// </summary>
        public static readonly String[] CompassAfterGuoSubNames = { "坎", "艮", "震", "巽", "离", "坤", "兑", "乾" };

        /// <summary>
        /// 先天八卦序列
        /// </summary>
        public static readonly string[] CompassBeforGuoSubNames = { "坤", "震", "离", "兑", "乾", "巽", "坎", "艮" };







        /// <summary>
        /// 罗盘64卦的度数，每卦占5.625度（360度/64卦）
        /// </summary>
        public const double CompassGuoDegree = 5.625;

        /// <summary>
        /// 每15度一个山
        /// </summary>
        public const double CHillDegree = 15;


        private CHill c24Hill;
        /// <summary>
        /// 根据当前罗盘的度数，获得对应的24山对象
        /// </summary>
        public CHill C24Hill { get => c24Hill; }

        private double degree;

        /// <summary>
        /// 创建一个罗盘对象，包含一个度数属性，表示当前罗盘指向的度数
        /// </summary>
        public double Degree
        {
            get => degree; set
            {
                degree = value;

                Init();


            }
        }

        /// <summary>
        /// 创建罗盘对象，传入一个度数参数，表示当前罗盘指向的度数，并根据该度数初始化对应的后天八卦、24山和先天64卦对象
        /// </summary>
        /// <param name="Degreen"></param>
        public CompassEx(double Degreen)
        {

            this.Degree = Degreen;

        }




        private GuoSubClass AfterGuoSub_;
        /// <summary>
        ///   根据当前罗盘的度数，获得对应的后天八卦对象
        /// </summary>
        public GuoSubClass AfterGuoSub { get => AfterGuoSub_; }



        private GuoClass beforGuo;

        /// <summary>
        /// 根据当前罗盘的度数，获得对应的先天64卦对象
        /// </summary>
        public GuoClass BeforGuo { get => beforGuo; }

        /// <summary>
        ///先天64卦对象(天盘），使用字典缓存所有64卦对象，避免每次计算时都创建新的对象，提高性能，需要优先初始化
        ///罗盘上先天排行从午为乾1、兑2、离3、震4（阳仪），巽5、坎6、艮7、坤8午（阴仪）
        ///罗盘上子坤（360度）至午乾（180度）从坤0度至天风姤180度，乾左为顺180度至地雷复360，内卦为卦宫外卦相荡而成
        /// </summary>
        [JsonIgnore]
        public static Dictionary<CompassRangEX, GuoClass> CBeforGuos;


        /// <summary>
        ///后天64卦对象(地盘），使用字典缓存所有64卦对象，避免每次计算时都创建新的对象，提高性能,需要优先初始化
        ///罗盘上后天排行从子为乾9、兑4、离3、震8（阳仪），巽2、坎7、艮6、坤1午（阴仪）
        ///罗盘上子乾（360度）至午坤（180度）从乾0度至雷地豫180度，坤左为顺180度至地雷复360，外卦为卦宫内卦相荡而成
        /// </summary>
        [JsonIgnore]
        public static Dictionary<CompassRangEX, GuoClass> CAfterGuos;





        /// <summary>
        /// 根据当前罗盘的度数，初始化对应属性
        /// </summary>
        private void Init()
        {
            this.AfterGuoSub_ = GetAfterGuoSub();//获得当前度数的罗盘的后天八卦对象
            this.c24Hill = Get24Hill();//获得当前度数的罗盘的后天八卦对象
            this.beforGuo = GetCBeforGuo();//获得当前度数的罗盘的先天64卦对象
        }



        /// <summary>
        /// 获得罗盘先天64卦的度数对象
        /// </summary>
        /// <param name="Name">卦名</param>
        /// <returns></returns>
        public static CompassRangEX GetCBeforGuoDegree(string GuoName)
        {
            foreach (var kv in CBeforGuos)
            {
                if (kv.Value.GuoName == GuoName)
                {
                    return kv.Key;
                }
            }
            return null;
        }



        /// <summary>
        /// 根据当前罗盘的度数，获得对应的先天64卦对象 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public GuoClass GetCBeforGuo()
        {
            foreach (var kv in CBeforGuos)
            {
                if (kv.Key.IsInRange(this.degree))
                {
                    return kv.Value;
                }
            }
            return null;
        }





        /// <summary>
        ///加载并初始化罗盘上的所有后天64卦（地盘）对象，按照顺时针方向排列，从坤卦开始，每5.625度一个卦逆时针，共64个卦
        /// </summary>
        /// <returns></returns>
        public static void LoadAllCAfterGuos()
        {


            CAfterGuos = C3Y.C3Y.GetAllCAfterGuos();

        }






        /// <summary>
        ///加载罗盘上的所有先天64卦（天盘）对象，按照顺时针方向排列，从坤卦开始，每5.625度一个卦逆时针，共64个卦
        /// </summary>
        /// <returns></returns>
        public static void LoadAllBeforGuos()
        {
            CBeforGuos = C3Y.C3Y.GetAllBeforGuos();



        }







        /// <summary>
        ///  根据当前罗盘的度数，获得对应的24山对象
        /// </summary>
        /// <returns></returns>
        public CHill Get24Hill()
        {
            foreach (string sN in CHill.C24Hills)
            {
                CompassRangEX range = Get24HillDegree(sN);
                if (range.IsInRange(this.degree))
                {
                    CHill hill = new CHill(sN);

                    return hill;
                }
            }
            return null;
        }




        /// <summary>
        /// 根据当前罗盘的度数，获得对应的24山中的度数范围对象
        /// </summary>
        /// <returns></returns>
        public static CompassRangEX Get24HillDegree(string HillName)
        {
            double baseDegree = 337.5;//罗盘24山的度数起点为壬山的337.5度

            int GIndex = CHill.C24Hills.IndexOf(HillName);//获得罗盘后天八卦的索引位置
            double degree = baseDegree + GIndex * CHillDegree;//根据索引位置计算度数
            double fStart = degree;
            if (fStart > 360) fStart -= 360;//如果超过360度，调整到0-360范围内
            double fEnd = degree + CHillDegree;
            if (fEnd > 360) fEnd -= 360;//如果超过360度，调整到0-360范围内

            CompassRangEX range = new CompassRangEX(fStart, fEnd);


            return range;
        }





        /// <summary>
        /// 根据当前罗盘的度数，获得对应的先天八卦 
        /// </summary>
        /// <returns></returns>
        public GuoSubClass GetBeforGuoSub()
        {
            foreach (string sN in CompassAfterGuoSubNames)
            {
                CompassRangEX range = GetBeforGuoSubDegree(sN);
                if (range.IsInRange(this.degree))
                {

                    var g = GuoSubClass.GetGuoSub(sN, true);



                    return g;
                }
            }

            return null;

        }


        /// <summary>
        /// 获得先天八卦的度数范围（罗盘）
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static CompassRangEX GetBeforGuoSubDegree(GuoSubClass g)
        {
            return GetBeforGuoSubDegree(g.GuoSubName);
        }


        /// <summary>
        /// 获得先天八卦的度数范围（罗盘）
        /// </summary>
        /// <returns></returns>
        public static CompassRangEX GetBeforGuoSubDegree(string GuoSubName)
        {
            double baseDegree = 337.5;//先天八卦的度数起点为艮卦的337.5度

            int GIndex = CompassBeforGuoSubNames.IndexOf(GuoSubName);//获得罗盘先天八卦的索引位置
            double degree = baseDegree + GIndex * GuoSubDegree;//根据索引位置计算度数
            double fStart = degree;
            if (fStart > 360) fStart -= 360;//如果超过360度，调整到0-360范围内
            double fEnd = degree + GuoSubDegree;
            if (fEnd > 360) fEnd -= 360;//如果超过360度，调整到0-360范围内

            CompassRangEX range = new CompassRangEX(fStart, fEnd);


            return range;

        }


        /// <summary>
        /// 根据当前罗盘的度数，获得对应的后天八卦 
        /// </summary>
        /// <returns></returns>
        public GuoSubClass GetAfterGuoSub()
        {
            foreach (string sN in CompassAfterGuoSubNames)
            {
                CompassRangEX range = GetAfterGuoSubDegree(sN);
                if (range.IsInRange(this.degree))
                {

                    var g = GuoSubClass.GetGuoSub(sN, true);



                    return g;
                }
            }

            return null;

        }




        /// <summary>
        /// GetAfterGuoSubDegree
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static CompassRangEX GetAfterGuoSubDegree(GuoSubClass g)
        {
            return GetAfterGuoSubDegree(g.GuoSubName);
        }


        /// <summary>
        /// 获得后天八卦的度数范围（罗盘）
        /// </summary>
        /// <returns></returns>
        public static CompassRangEX GetAfterGuoSubDegree(string GuoSubName)
        {
            double baseDegree = 337.5;//后天八卦的度数起点为坎卦的337.5度

            int GIndex = CompassAfterGuoSubNames.IndexOf(GuoSubName);//获得罗盘后天八卦的索引位置
            double degree = baseDegree + GIndex * GuoSubDegree;//根据索引位置计算度数
            double fStart = degree;
            if (fStart > 360) fStart -= 360;//如果超过360度，调整到0-360范围内
            double fEnd = degree + GuoSubDegree;
            if (fEnd > 360) fEnd -= 360;//如果超过360度，调整到0-360范围内

            CompassRangEX range = new CompassRangEX(fStart, fEnd);


            return range;

        }


    }

}
