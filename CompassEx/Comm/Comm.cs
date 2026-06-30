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
    /// 类库核心公共辅助工具类，提供全局初始化入口及基于反射的对象属性浅拷贝扩展。
    /// </summary>
    public static class Comm
    {
        /// <summary>
        /// 全自动初始化罗盘引擎中赖以运行的所有底层基本数据。
        /// </summary>
        /// <remarks>
        /// <b>初始化加载名堂：</b><br/>
        /// 该方法在系统启动或反序列化时应被优先调用。它会通过内部流水线依次唤醒并充填两套最核心的周天度数数据：
        /// <list type="bullet">
        /// <item><description>调用 <see cref="CompassEx.LoadAllCAfterGuos"/>：全量装载后天六十四卦罗盘圈层分度范围数据。</description></item>
        /// <item><description>调用 <see cref="CompassEx.LoadAllCBeforGuos"/>：全量装载伏羲先天六十四卦方圆图周天物理刻度数据。</description></item>
        /// </list>
        /// </remarks>
        public static void AllInit()
        {
            CompassEx.LoadAllCAfterGuos();
            CompassEx.LoadAllCBeforGuos();
        }

        /// <summary>
        /// 扩展方法：利用运行时反射（Reflection）机制，强行从父级基类模板中将所有属性与字段的值“浅拷贝”反灌给当前继承类实例。
        /// </summary>
        /// <typeparam name="TChild">继承类（派生类）的具体类型，必须隐式继承自 <typeparamref name="TBase"/>。</typeparam>
        /// <typeparam name="TBase">基类（模板类）的具体类型。</typeparam>
        /// <param name="child">正在接受赋值的当前继承类（子类）目标对象实例。</param>
        /// <param name="baseTemplate">作为数据源的基类（父类）实体模板对象。</param>
        /// <remarks>
        /// <b>🛠️ 内部反射拷贝门道与避坑提示：</b>
        /// <list type="number">
        /// <item><description><b>属性遍历</b>：方法首先通过 <c>typeof(TBase).GetProperties()</c> 捕获父类公开属性，并通过 <c>prop.CanWrite</c> 安全拦截，防止对只读属性或受保护的 Get 块执行非法注入。</description></item>
        /// <item><description><b>字段反灌</b>：随后通过 <c>typeof(TBase).GetFields()</c> 递归提取底层物理字段并完成原子值覆写。</description></item>
        /// <item><description><b>⚠️ 性能与安全警告</b>：由于使用了运行时动态反射，该操作会带来较大的 CPU 耗能，<b>在高频循环或深层装卦排盘时应克制使用</b>。另外，它仅支持第一层浅拷贝，若字段中含有 <c>List&lt;T&gt;</c> 等引用类型，两端实例会共享同一份内存地址，修改时存在联动篡改的隐患，在跨变卦深度计算时需高度注意。</description></item>
        /// </list>
        /// 该方法常用于在 JSON 反序列化后期生命周期中，快速将多字段的静态基类参数直接克隆给派生实体类。
        /// </remarks>
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

    /// <summary>
    /// 针对一维数组的高性能公共流式扩展方法封装类。
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// 扩展方法：在给定的强类型一维数组中，高效正向检索特定元素并返回其首次出现的零基（Zero-based）索引。
        /// </summary>
        /// <typeparam name="T">数组内部元素的泛型类型。</typeparam>
        /// <param name="array">当前正在执行检索的目标一维数组实体。</param>
        /// <param name="value">期望在数组中匹配定位的目标对象或数值。</param>
        /// <returns>返回匹配项在数组中的绝对索引位置（范围在 <c>0</c> 到 <c>Length - 1</c> 之间）；若全盘未匹配成功，则返回标准未找到标识 <c>-1</c>。</returns>
        /// <remarks>
        /// 该方法是对原生静态函数 <see cref="Array.IndexOf{T}(T[], T)"/> 的流式桥接封装，允许类库在内部对静态数组数据（如干支集、卦序表）直接像调用 List 一样使用流畅的 <c>array.IndexOf(value)</c> 语法，从而大幅减少语法噪音。
        /// </remarks>
        public static int IndexOf<T>(this T[] array, T value)
        {
            return Array.IndexOf(array, value);
        }
    }
}

