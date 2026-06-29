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
    /// 天干地支类
    /// </summary>
    public class SkyLoc
    {

        /// <summary>
        /// 天干地支名称
        /// </summary>
        public string SkyLocName { get { return Sky.SkyName + Loc.LocName; } }

        /// <summary>
        /// 天干
        /// </summary>
        public SkyClass Sky { get; private set; }

        /// <summary>
        /// 地支
        /// </summary>
        public LocClass Loc { get; private set; }

        /// <summary>
        /// 天干地支构造函数
        /// </summary>
        /// <param name="SkyLocName">天干地支组合<see cref="SkyClass.SkyNames"/><see cref="LocClass.LocNames"/></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public SkyLoc(string SkyLocName) : this(SkyClass.SkyNames.IndexOf(SkyLocName[0].ToString()), LocClass.LocNames.IndexOf(SkyLocName[1].ToString()))
        {


        }

        /// <summary>
        /// 天干地支构造函数
        /// </summary>
        /// <param name="SkyIndex">天干索引 <see cref="SkyClass.SkyNames"/></param>
        /// <param name="LocIndex">地支索引<see cref="LocClass.LocNames"/></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public SkyLoc(int SkyIndex, int LocIndex)
        {
            if (SkyIndex < 0 || SkyIndex > SkyClass.SkyNames.Length) throw new IndexOutOfRangeException(nameof(SkyIndex));

            if (LocIndex < 0 || LocIndex > LocClass.LocNames.Length) throw new IndexOutOfRangeException(nameof(LocIndex));

            this.Sky = new SkyClass(SkyIndex);

            this.Loc = new LocClass(LocIndex);

        }

    }
}
