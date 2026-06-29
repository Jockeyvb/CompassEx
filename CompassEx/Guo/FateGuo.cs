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
using System.Linq;
using tyme.solar;
using static CompassEx.Comm.Defined;


namespace CompassEx.Guo
{
    /// <summary>
    /// 是命卦的结构类型，包含了命卦的六爻纯卦和一些相关属性，例如：命卦的五行属性，命卦的运等<br/>
    /// 1、涉及余胜的命卦计算方法，命卦的入卦和出卦的计算方法，命卦的七爻飞爻卦的计算方法，以及命卦与罗盘24山（向）的关系等内容。
    /// 余胜唐所著的《玄空大卦些子法真诀》与《三元直指》中的天机出卦法：余胜唐在《三元直指》书中177页中，明确指出天机出卦法是纳甲用法（三元命卦无关）且并没涉及玄空大卦中的五行之论（虽然书中不全论大卦，亦应注或提示），如不查他其他著作完全不知道要如何使用且《玄空大卦些子法真诀》中的天机出卦法只说【命卦】又不指出命卦具体是三元命卦还是纳甲之卦，使用方法是以向卦为论，那么玄空大卦中是以所向的成卦之卦宫为论？还是以后天八卦为论？还是纳甲卦为论？真让人摸不着头脑。著书者应当严明解说；学习风水者应细仔研究考究为用，不可单论。<br/>
    /// 2、在刘贲所作《玄空大卦透析》书中456页第十九章中提到相似，甚至比余写的更为清晰指出以三元64卦以卦气为论，命卦则是纳甲（包括京房与杨公之纳甲法）用户可以参考其计算方法与判断方法。<br/>
    /// 关于京房纳甲与杨公纳甲资料，请自行查阅区别与使用<br />
    /// <b><font color="red">仅参供参考，请谨慎看待使用</font></b>
    /// </summary>
    public class FateGuo
    {



        /// <summary>
        /// 所属命卦
        /// </summary>
        public GuoSubClass GuoSub { get; private set; } = null;

        /// <summary>
        /// 入卦（三爻卦）后天，入卦数为5个
        /// </summary>
        public Dictionary<string, GuoSubClass> InGuoSubs { get; private set; } = null;
        /// <summary>
        /// 出卦（三爻卦）后天，出卦数为3个
        /// </summary>
        public Dictionary<string, GuoSubClass> OutGuoSubs { get; private set; } = null;

        /// <summary>
        /// 7世飞爻卦(京房易卦法)（由初爻开始往上变，以后最卦接着变出共变7次，共8个卦）
        /// </summary>
        public List<GuoClass> GuoList { get; private set; } = null;

        /// <summary>
        /// 是否出卦
        /// </summary>
        public bool IsOutGuo { get; private set; } = false;
        /// <summary>
        /// 相关信息，例如：如果无法计算命卦，则会在这里记录错误信息
        /// </summary>
        public string Message { get; private set; } = "";







        /// <summary>
        /// 构造函数，输入出生日期和性别，计算命卦，并获得相关的入卦和出卦等信息（包括计算立春岁数）
        /// </summary>
        /// <param name="d">命卦日期</param>
        /// <param name="Sex">性别</param>
        /// <param name="ToGuo8">罗盘中的24山（向）所属的卦宫位置（后天）</param>
        public FateGuo(DateTime d, string Sex, GuoClass ToGuo8)
        {
            try
            {

                if (d <= DateTime.MinValue) throw new Exception("请输入正确的出生日期。");
                if (Sex != "男" && Sex != "女") throw new Exception("性别只能是【男】或【女】");
                if (ToGuo8 == null) throw new Exception("请输入正确的罗盘24山（向）所属的卦宫位置（后天）。");

                this.GuoSub = GetFateGuo(d, Sex);
                if (this.GuoSub == null) throw new Exception("无法正确计算命卦。");


                TianJiOutGuo tjg = new TianJiOutGuo(GuoClass.GetGuoClass(this.GuoSub.GuoSubName));

                this.IsOutGuo = tjg.IsOutGuo(ToGuo8.UpGuo); //是否出卦
                this.InGuoSubs = tjg.InGuoSubs;
                this.OutGuoSubs = tjg.OutGuoSubs;
                //  this.GuoList = GuoClass.GetGuoClass(this.GuoSub.GuoSubName).Get7HereYaoGuo();//获得命卦的7世飞爻卦


                //this.IsOutGuo = gscOuts.ContainsKey(ToGuo8.GuoSubName); //如果向在出卦中，则说明属于出卦

                //if (this.IsOutGuo)
                //{
                //    this.Message = "命卦属于出卦，天机安存外";
                //}
                //else
                //{
                //    this.Message = "命卦属于入卦，天机安存内";
                //}

                this.Message = "三元命卦辅助参考，择日应该与命庚卦为用";

            }
            catch (Exception ex)
            {
                this.GuoSub = null;
                this.Message = ex.Message;
            }


        }





        /// <summary>
        /// 获得当前日期的三元命卦（三爻卦）后天
        /// </summary>
        /// <param name="d">出生日期（公历），将自动计算立春前后的真实年份</param>
        /// <param name="Sex">性别</param>
        /// <returns></returns>
        public static GuoSubClass GetFateGuo(DateTime d, string Sex)
        {
            int iy = d.Year;
            SolarTerm term = SolarTerm.FromName(d.Year, "立春");

            FactYearEnum IsFact = d >= term.GetSolarDay().ToDateTime() ? FactYearEnum.SpringAfter : FactYearEnum.SpringBefore;//先判断是否为当年的立春后，如果是立春前，则用前一年计算命卦
            if (IsFact == FactYearEnum.SpringBefore) iy--;//如果是立春前，则用前一年计算命卦

            char[] sdy = iy.ToString().ToArray();
            int ir = 0;
            //=====================================第一次计算=========================  .
            for (int i = 0; i < sdy.Length; i++)//拆开单个数字计算
            {
                ir += int.Parse(sdy[i].ToString());
            }
            //=====================================第一次计算=========================
            sdy = ir.ToString().ToArray();
            while (sdy.Length > 1)
            {
                ir = 0;
                //=====================================加至个位数=========================  .
                for (int i = 0; i < sdy.Length; i++)//拆开单个数字计算
                {
                    ir += int.Parse(sdy[i].ToString());
                }
                //=====================================加至个位数=========================
                sdy = ir.ToString().ToArray();
            }

            if (Sex == "男")
            {

                ir = 11 - ir; //男用11去减个位数
                sdy = ir.ToString().ToArray();
                while (sdy.Length > 1)
                {
                    ir = 0;
                    //=====================================加至个位数=========================  .
                    for (int i = 0; i < sdy.Length; i++)//拆开单个数字计算
                    {
                        ir += int.Parse(sdy[i].ToString());
                    }
                    //=====================================加至个位数=========================
                    sdy = ir.ToString().ToArray();
                }


                if (ir == 0) ir = 1;//如果等0则为1（坎）
                if (ir == 5) ir = 2;//寄坤
            }
            else //女 
            {
                ir += 4;
                if (ir > 9)
                {
                    ir = ir - 9;
                }
                if (ir == 0) ir = 9;//女命，如果等0，则为9（离）
                if (ir == 5) ir = 8;//寄坤
            }


            GuoSubClass gsc = GuoSubClass.GetAfterGuoSub(ir - 1);
            return gsc;

        }



    }


}
