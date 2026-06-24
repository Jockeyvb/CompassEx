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
        public static string[] SkyNames = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };//天干
        public string SkyName;//地支名称
        public int Index;//天干位置
        public string FiveAttrName;//五行属性名
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
            SkyClass sc = new SkyClass();
            if (iSkyIndex < 2)
            {
                sc.FiveAttrName = "木";
            }
            else if (iSkyIndex == 2 || iSkyIndex == 3)
            {
                sc.FiveAttrName = "火";
            }
            else if (iSkyIndex == 4 || iSkyIndex == 5)
            {
                sc.FiveAttrName = "土";
            }
            else if (iSkyIndex == 6 || iSkyIndex == 7)
            {
                sc.FiveAttrName = "金";
            }
            else
            {
                sc.FiveAttrName = "土";
            }
            sc.Index = iSkyIndex;
            sc.SkyName = SkyNames[iSkyIndex];
            return sc;
        }


    }
}
