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







    public static class ArrayExtensions
    {
        public static int IndexOf<T>(this T[] array, T value)
        {
            return Array.IndexOf(array, value);
        }
    }

    public static class Comm
    {

        /// <summary>
        /// 初始化一些基本数据
        /// </summary>
        public static void AllInit()
        {
            CompassEx.LoadAllCAfterGuos();
            CompassEx.LoadAllBeforGuos();
        }

        /// <summary>
        /// 从基类为继承类重设相关属性
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <typeparam name="TBase"></typeparam>
        /// <param name="child"></param>
        /// <param name="baseTemplate"></param>
        public static void ApplyBaseProperties<TChild, TBase>(this TChild child, TBase baseTemplate)
            where TChild : TBase
        {
            // 用反射复制（慎用，性能差且容易出错）
            foreach (var prop in typeof(TBase).GetProperties())
            {
                if (prop.CanWrite)
                {
                    prop.SetValue(child, prop.GetValue(baseTemplate));
                }
            }

            foreach (var f in typeof(TBase).GetFields())
            {

                f.SetValue(child, f.GetValue(baseTemplate));

            }
        }

    }
}
