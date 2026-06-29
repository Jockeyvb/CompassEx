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

namespace CompassEx.Comm
{
    /// <summary>
    /// 天干类
    /// </summary>
    public class SkyClass
    {
        /// <summary>
        /// 天干名称列表
        /// </summary>
        public static string[] SkyNames = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };//天干

        /// <summary>
        /// 地支名称
        /// </summary>
        public string SkyName { get; private set; }//

        /// <summary>
        /// 天干索引
        /// </summary>
        public int Index { get; private set; }//

        /// <summary>
        /// 五行属性名
        /// </summary>
        public FiveAttr FiveAttr { get; private set; }//五行属性名

        /// <summary>
        /// 天干构造函数
        /// </summary>
        /// <param name="sSkyName">天干名称</param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public SkyClass(string sSkyName) : this(Array.IndexOf(SkyNames, sSkyName))
        {

        }

        /// <summary>
        ///  天干构造函数
        /// </summary>
        /// <param name="iSkyIndex">天干所在的索引，参考：【<see cref="SkyNames"/>】</param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public SkyClass(int iSkyIndex)
        {
            if (iSkyIndex < 0 || iSkyIndex > SkyNames.Length) throw new IndexOutOfRangeException();
            if (iSkyIndex < 2)
            {
                this.FiveAttr = new FiveAttr("木");
            }
            else if (iSkyIndex == 2 || iSkyIndex == 3)
            {
                this.FiveAttr = new FiveAttr("火");
            }
            else if (iSkyIndex == 4 || iSkyIndex == 5)
            {
                this.FiveAttr = new FiveAttr("土");
            }
            else if (iSkyIndex == 6 || iSkyIndex == 7)
            {
                this.FiveAttr = new FiveAttr("金");
            }
            else
            {
                this.FiveAttr = new FiveAttr("土");
            }
            this.Index = iSkyIndex;
            this.SkyName = SkyNames[iSkyIndex];
        }


        /// <summary>
        /// 根据天干的索引返回天干类
        /// </summary>
        /// <param name="sSkyName"></param>
        /// <returns></returns>
        public static SkyClass GetSkyClass(string sSkyName)
        {
            int iPos = Array.IndexOf(SkyNames, sSkyName);
            return GetSkyClass(iPos);
        }

        /// <summary>
        /// 根据天干的索引返回天干类
        /// </summary>
        /// <param name="iSkyIndex"></param>
        /// <returns></returns>
        public static SkyClass GetSkyClass(int iSkyIndex)
        {
            return new SkyClass(iSkyIndex);

        }


    }
}
