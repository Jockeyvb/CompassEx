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
    /// 六神类
    /// </summary>
    public class SixGodClass
    {
        public static String[] SixGods = { "青龙", "朱雀", "勾陈", "腾蛇", "白虎", "玄武" };

        public String GodName;//六神名称
        public int iStartIndex;//六神的所在索引
                               //private SQLiteDatabase db;//数据库



        /// <summary>
        /// 根据日的天干返回六神的数组
        /// </summary>
        /// <param name="DaySkyName"></param>
        /// <returns></returns>
        public static List<SixGodClass> GetSixGod(String DaySkyName)
        {
            int iPos = Array.IndexOf(SkyClass.SkyNames, DaySkyName);
            if (iPos == -1) return null;//找不到
            return GetSixGod(iPos);
        }


        /// <summary>
        /// 根据日的天干索引返回六神的数组
        /// </summary>
        /// <param name="iDaySkyIndex"></param>
        /// <returns></returns>
        public static List<SixGodClass> GetSixGod(int iDaySkyIndex)
        {
            List<SixGodClass> sgcs = new List<SixGodClass>();

            int iPos = iDaySkyIndex / 2;
            if (iDaySkyIndex > 4) iPos++;//呈蛇

            for (int i = 0; i < 6; i++)
            {
                SixGodClass sgc = new SixGodClass();

                sgc.GodName = SixGods[iPos];
                sgc.iStartIndex = iPos;

                sgcs.Add(sgc);
                iPos++;
                if (iPos > 5) iPos = 0;
            }
            return sgcs;
        }
    }
}
