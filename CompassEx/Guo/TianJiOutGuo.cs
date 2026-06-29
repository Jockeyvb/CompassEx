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

using System;
using System.Collections.Generic;

namespace CompassEx.Guo
{



    /// <summary>
    /// 天机出卦法（余胜唐、刘贲）有记载，一般用于收山出煞、命卦判断 等
    /// </summary>
    public class TianJiOutGuo
    {
        /// <summary>
        /// 7世飞爻卦(京房易卦法)（由初爻开始往上变，以后最卦接着变出共变7次，共8个卦）
        /// </summary>
        public List<GuoClass> GuoList { get; private set; } = null;

        /// <summary>
        /// 入卦集合（八卦）
        /// </summary>
        public Dictionary<string, GuoSubClass> InGuoSubs { get; private set; }

        /// <summary>
        /// 出卦集合（八卦）
        /// </summary>
        public Dictionary<string, GuoSubClass> OutGuoSubs { get; private set; }


        /// <summary>
        /// 判断是否出卦,玄空大卦中主要使用其卦气来用于收山出煞之用(只用于罗盘64卦上）<br />
        /// 三元命卦出卦则需要按纳甲来判断请使用【 <see cref="FateGuo.IsOutGuo()"/>  】
        /// </summary>
        /// <param name="CompareGuo">64卦中对比之卦</param>
        /// <returns></returns>
        public bool IsOutGuo(GuoSubClass CompareGuo)
        {
            return OutGuoSubs.ContainsKey(CompareGuo.GuoSubName);
        }


        /// <summary>
        /// 天机出卦法是根据六爻卦之卦宫(六纯卦）进行八卦爻变<br/>
        /// 七世飞爻得出五卦（三爻卦）为入卦，三个卦为出卦（三爻卦），并可查询本卦卦宫序列卦集合<br/>
        /// 一般三元罗盘上的<b><font color="red">向卦(六爻卦)</font></b>为入参结构本类
        /// </summary>
        /// <param name="ToGuo"><b><font color="red">向卦(六爻卦)</font></b></param>
        /// <exception cref="ArgumentNullException">不能传入null</exception>
        public TianJiOutGuo(GuoClass ToGuo)
        {
            if (ToGuo == null) throw new ArgumentNullException("入参卦不能为null");
            GuoClass GuoSelf = GuoClass.GetGuoClass(ToGuo.GuoSelf.GuoSubName);//取卦宫

            GuoList = GuoSelf.Get7HereYaoGuo(); //以卦宫来列出飞爻卦
            Dictionary<string, GuoSubClass> gscIns = new Dictionary<string, GuoSubClass>();
            //===========================获得入卦（三爻卦）后天===========================
            foreach (GuoClass gc in GuoList)
            {
                if (gscIns.ContainsKey(gc.DownGuo.GuoSubName) == false)
                {
                    gscIns.Add(gc.DownGuo.GuoSubName, gc.DownGuo);
                }
                if (gscIns.ContainsKey(gc.UpGuo.GuoSubName) == false)
                {
                    gscIns.Add(gc.UpGuo.GuoSubName, gc.UpGuo);
                }
            }
            this.InGuoSubs = gscIns; //命卦的入卦（三爻卦）后天

            //===========================获得入卦（三爻卦）后天===========================

            //===========================获得出卦（三爻卦）后天===========================
            Dictionary<string, GuoSubClass> gscOuts = new Dictionary<string, GuoSubClass>();
            foreach (string sN in GuoSubClass.BeforeGuoSubNames)
            {
                if (gscIns.ContainsKey(sN) == false && gscOuts.ContainsKey(sN) == false)
                {
                    gscOuts.Add(sN, GuoSubClass.GetGuoSub(sN, false));
                }
            }
            this.OutGuoSubs = gscOuts; //命卦的出卦（三爻卦）后天

            //===========================获得出卦（三爻卦）后天===========================


        }

    }
}
