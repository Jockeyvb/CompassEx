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

namespace CompassEx.Guo
{
    /// <summary>
    /// 六亲类
    /// </summary>
    public class SixRelativeClass
    {
        public static String[] SixRelatives = { "妻财", "子孙", "兄弟", "官鬼", "父母" };
        /// <summary>
        /// 六亲名称
        /// </summary>
        public String RelativeName;
        /// <summary>
        /// 六亲所在的位置
        /// </summary>
        public int RelativeIndex;





        /// <summary>
        /// 根据关系名称获得获得六亲类
        /// </summary>
        /// <param name="SixRelativeName"></param>
        /// <returns></returns>
        public static SixRelativeClass GetSixRelative(String SixRelativeName)
        {
            SixRelativeClass src = new SixRelativeClass();
            int iPos = Array.IndexOf(SixRelatives, SixRelativeName);
            src.RelativeName = SixRelatives[iPos];
            src.RelativeIndex = iPos;
            src.RelativeName = SixRelativeName;
            return src;
        }

        /**
		* 根据关系获得获得六亲类
		*/
        public static SixRelativeClass GetSixRelative(FiveAttrRule far)
        {
            SixRelativeClass src = new SixRelativeClass();
            src.RelativeName = SixRelatives[(int)far];
            src.RelativeIndex = (int)far;
            return src;
        }



    }
}
