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

namespace CompassEx.Guo
{



    /// <summary>
    /// 纳甲类,包括：京房纳甲，杨公纳甲
    /// </summary>
    public class NaJia
    {

        /// <summary>
        /// 纳甲类型
        /// </summary>
        public enum NaJiaType
        {
            /// <summary>
            /// 京房纳甲
            /// </summary>
            JingFang = 0,
            /// <summary>
            /// 杨公纳甲
            /// </summary>
            YangGong = 1,
        }


        public NaJiaType Type { get; private set; }



    }
}
