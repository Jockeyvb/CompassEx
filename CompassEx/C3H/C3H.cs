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

namespace CompassEx
{

    /// <summary>
    /// 三合罗盘类。
    /// </summary>
    /// <remarks>
    /// 本类继承自 <see cref="CompassEx"/>，主要用于处理三合罗盘体系（地盘正针、人盘中针、天盘缝针等三盘派系）的度数映射与罗盘层级计算。
    /// </remarks>
    public class C3HEx : CompassEx
    {
        /// <summary>
        /// 初始化 <see cref="C3HEx"/> 类的新实例。
        /// </summary>
        /// <param name="Degreen">当前罗盘的初始度数（角度值）。</param>
        /// <remarks>
        /// 构造函数通过调用基类 <see cref="CompassEx"/> 的构造函数来完成基本度数的初始化。
        /// </remarks>
        public C3HEx(double Degreen) : base(Degreen)
        {
        }
    }
}
