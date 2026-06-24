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

using CompassEx.Guo;
using System;
using System.Collections.Generic;

namespace CompassEx.Comm
{

    public class Compass24Hill
    {



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


        /// <summary>
        /// 24山中五行是否是为阳
        /// </summary>
        public bool IsSun
        {
            get
            {
                int iPos = CompassEx.Compass24Hills.IndexOf(this.Name);


                if (iPos == 0 || iPos > 3 && iPos < 7 || iPos > 9 && iPos < 13 || iPos > 15 && iPos < 19 || iPos > 21) //阳
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        public Compass24Hill(string sName) { this.Name = sName; }
    }

}
