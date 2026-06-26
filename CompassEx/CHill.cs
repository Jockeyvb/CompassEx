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
using System;
using System.Collections.Generic;

namespace CompassEx
{
    /// <summary>
    /// 表示罗盘的24山及其相关属性和方法。
    /// </summary>
    public class CHill
    {

        /// <summary>
        /// 罗盘的24山名称，按照顺时针方向排列，从壬山开始，每15度一个山，共24个山(正针)
        /// </summary>
        public static readonly String[] C24Hills = { "壬", "子", "癸", "丑", "艮", "寅", "甲", "卯", "乙", "辰", "巽", "巳", "丙", "午", "丁", "未", "坤", "申", "庚", "酉", "辛", "戌", "乾", "亥" };

        /// <summary>
        /// 属阳的二十四山
        /// </summary>
        public static readonly string[] CSun24Hills = { "壬", "丙", "甲", "庚", "巽", "巳", "亥", "乾", "坤", "申", "艮", "寅" };

        /// <summary>
        /// 是否为阳
        /// </summary>
        public bool IsSun { get { return CSun24Hills.IndexOf(this.Name) > -1; } }

        /// <summary>
        /// 罗盘24山的名称，例如：壬、子等
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 罗盘24山的度数范围，例如：壬山的度数范围为337.5-352.5度
        /// </summary>
        public CompassRangEX CRangeDegree { get { return CompassEx.Get24HillDegree(this.Name); } }

        /// <summary>
        /// 获得正针本24山中对应的后天八卦对象
        /// </summary>
        /// <returns></returns>
        public GuoSubClass GetAfterGuo()
        {
            CompassEx ce = new CompassEx(this.CRangeDegree.Start);
            return ce.GetAfterGuoSub();
        }


        /// <summary>
        /// 获得正针本24山中中包含所有的先天64卦
        /// </summary>
        /// <returns></returns>       
        public Dictionary<CompassRangEX, GuoClass> GetCBeforGuos()
        {
            Dictionary<CompassRangEX, GuoClass> dc = new Dictionary<CompassRangEX, GuoClass>();
            CompassRangEX CRE = this.CRangeDegree;
            foreach (var kv in CompassEx.CBeforGuos)
            {
                if (CRE.IsInRange(kv.Key.Start) || CRE.IsInRange(kv.Key.End - 0.01)) //因为1山对应的64卦不整齐，所以要在结束度数上-0.01度来获得精确64卦对象
                {
                    dc.Add(kv.Key, kv.Value);
                }
            }
            return dc;
        }



        public CHill(string sName) { this.Name = sName; }
    }

}
