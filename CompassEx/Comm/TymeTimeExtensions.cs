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
using tyme.solar;

namespace CompassEx.Comm
{

    public static class TymeTimeExtensions
    {



        /// <summary>SolarTime 转  DateTime</summary>
        public static DateTime ToDateTime(this SolarTime st)
        {

            return new DateTime(st.Year, st.Month, st.Day, st.Hour, st.Minute, st.Second);
        }

        /// <summary>SolarDay 转 UTC DateTime</summary>
        public static DateTime ToDateTime(this SolarDay sd)
        {

            return new DateTime(sd.Year, sd.Month, sd.Day);
        }


    }
}
