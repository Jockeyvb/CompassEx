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

    /// <summary>
    /// 针对元贞历法核心库（Tyme）时间与日期对象的 C# 官方标准 <see cref="DateTime"/> 转换扩展工具类。
    /// </summary>
    /// <remarks>
    /// 本静态类通过提供高效的链式扩展方法（Extension Methods），实现了将三方历法库中的公历时间（<see cref="SolarTime"/>）与公历日期（<see cref="SolarDay"/>）快速转换为 .NET 原生的时间结构体，常用于前后端排盘结果的数据对接与串联。
    /// </remarks>
    public static class TymeTimeExtensions
    {

        /// <summary>
        /// 【扩展方法】将 .NET 原生的 <see cref="DateTime"/> 快速转化为 Tyme 历法库专用的 <see cref="SolarTime"/> 高精度阳历时间对象。
        /// </summary>
        /// <param name="d">需要进行类型转换的 .NET 标准系统日期时间实例（通常为北京时间或现场真太阳时）。</param>
        /// <returns>
        /// 返回一个高精度的 <see cref="SolarTime"/> 实体，内部完美继承传入时间的年、月、日、时、分、秒等时空刻度数值。
        /// </returns>
        /// <remarks>
        /// 本方法作为系统高频使用的核心桥接管道，常用于将电脑当前系统时间（如 <see cref="DateTime.Now"/>）或从数据库读取的勘测时间，
        /// 无缝转换为历法库模型，以便后续进行高精度的立春换年柱、节气换月柱等核心易学理气数理推演。
        /// </remarks>
        public static SolarTime ToSolarTime(this DateTime d)
        {
            // 降维拆解 .NET 原生时间组件，通过构造函数直接实例化 Tyme 阳历时间对象
            return new SolarTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second);
        }


        /// <summary>
        /// 将 Tyme 历法库的公历时间对象转换为 .NET 标准的 <see cref="DateTime"/> 实例。
        /// </summary>
        /// <param name="st">被扩展的当前 <see cref="SolarTime"/> 公历时间对象实例。</param>
        /// <returns>返回一个包含完整年、月、日、时、分、秒信息的 <see cref="DateTime"/> 结构体对象。</returns>
        /// <remarks>
        /// <b>转换细节：</b>该方法通过显式提取输入对象的具体时分秒数值进行直接映射，生成的 <see cref="DateTime"/> 实例默认其 <see cref="DateTime.Kind"/> 属性为 <see cref="DateTimeKind.Unspecified"/>。
        /// </remarks> // 👈 這裡之前漏掉了閉合標籤，現已補上
        public static DateTime ToDateTime(this SolarTime st)
        {
            return new DateTime(st.Year, st.Month, st.Day, st.Hour, st.Minute, st.Second);
        }

        /// <summary>
        /// 将 Tyme 历法库的公历日期对象转换为 .NET 标准的 <see cref="DateTime"/> 实例（零点时刻）。
        /// </summary>
        /// <param name="sd">被扩展的当前 <see cref="SolarDay"/> 公历日期对象实例。</param>
        /// <returns>返回一个代表该日期当天凌晨零点整（<c>00:00:00</c>）的 <see cref="DateTime"/> 结构体对象。</returns>
        public static DateTime ToDateTime(this SolarDay sd)
        {
            return new DateTime(sd.Year, sd.Month, sd.Day);
        }
    }


}
