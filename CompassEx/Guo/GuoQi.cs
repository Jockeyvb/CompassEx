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

namespace CompassEx.Guo
{


    /// <summary>
    ///64卦（洛数）、先天五行数
    /// </summary>
    public class GuoQi
    {
        /// <summary>
        /// 引用卦
        /// </summary>
        public GuoSubClass GuoSub { get; private set; }
        /// <summary>
        /// 卦气五行字典
        /// </summary>
        public readonly Dictionary<int, FiveAttr> QuoQiFiveAttr = new Dictionary<int, FiveAttr> { { 1, new FiveAttr("水") }, { 6, new FiveAttr("水") }, { 2, new FiveAttr("火") }, { 7, new FiveAttr("火") }, { 3, new FiveAttr("木") }, { 8, new FiveAttr("木") }, { 4, new FiveAttr("金") }, { 9, new FiveAttr("金") }, { 5, new FiveAttr("土") }, { 10, new FiveAttr("土") }, { 15, new FiveAttr("土") } };
        /// <summary>
        /// 卦气五行名称
        /// </summary>
        public FiveAttr FiveAttr { get { return this.QuoQiFiveAttr[this.GuoQiNumber]; } }


        /// <summary>
        ///  64卦气（洛数）先天五行数
        /// </summary>
        public int GuoQiNumber
        {
            get
            {

                foreach (string sn in GuoSubClass.BeforeGuoSubNames)
                {
                    var gs = GuoSubClass.GetGuoSub(sn);
                    var CR = gs.CAfterRangeDegree;
                    if (this.GuoSub.CBeforRangeDegree.IsInRange(CR.Start)) //本卦的所在的先天卦位
                        return gs.AfterQuantity + 1; //先天卦位的后天数为卦气（洛数）
                }
                throw new Exception("未找到卦气");
            }

        }

        public NaJia JiFanNaJai()
        {
            return null;
        }


        /// <summary>
        /// 卦气结构函数
        /// </summary>
        /// <param name="GuoName">六爻卦名</param>
        public GuoQi(string GuoName) : this(new GuoClass(GuoName))
        {

        }


        /// <summary>
        /// 卦气结构函数
        /// </summary>
        /// <param name="gs">三爻卦</param>
        /// <exception cref="NullReferenceException"></exception>
        public GuoQi(GuoSubClass gs)
        {
            if (gs == null) throw new NullReferenceException(nameof(gs));
            GuoSub = gs;
        }

        /// <summary>
        /// 六爻卦的卦气以上卦计算
        /// </summary>
        /// <param name="g"></param>
        public GuoQi(GuoClass g) : this(g.UpGuo)
        {

        }

    }

}
