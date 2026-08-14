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

using CommLib;
using CompassEx.Comm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CompassEx.Assist
{
    /// <summary>
    /// 表示时辰神煞（时家神煞）的类，用于处理择吉中时辰的吉凶、名称索引及颜色标识。
    /// </summary>
    public class HourGod
    {
        /// <summary>
        /// 所有时辰神煞名称的静态只读数组，定义了神煞的顺序与吉凶分界线。
        /// </summary>
        public static readonly string[] HourGodNames = { "金匮", "日建", "天乙", "日合", "喜神", "玉堂", "日马", "司命", "天官", "宝光", "青龙", "福星", "明堂", "日禄", "日刑", "天牢", "玄武", "日破", "路空", "天刑", "旬空", "朱雀", "不遇", "日害", "白虎", "勾陈" };

        /// <summary>
        /// 获取当前神煞的名称。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 获取当前神煞在 <see cref="HourGodNames"/> 数组中的索引位置。
        /// </summary>
        public int Index { get { return HourGodNames.IndexOf(this.Name); } }

        /// <summary>
        /// 获取一个值，该值指示当前神煞是否为吉神（索引小于 14 为吉）。
        /// </summary>
        public bool IsGood { get { return this.Index < 14; } }

        /// <summary>
        /// 获取当前神煞对应的显示颜色（吉神通常为红色，凶神为黑色）。
        /// </summary>
        public Color Color { get { return this.IsGood ? Color.Red : Color.Black; } }

        /// <summary>
        /// 初始化 <see cref="HourGod"/> 类的新实例，通过指定的名称查找并创建。
        /// </summary>
        /// <param name="name">神煞名称字符串。</param>
        public HourGod(string name) : this(HourGodNames.IndexOf(name))
        {
        }

        /// <summary>
        /// 初始化 <see cref="HourGod"/> 类的新实例，通过在 <see cref="HourGodNames"/> 中的索引位置创建。
        /// </summary>
        /// <param name="index">神煞在数组中的索引位置。</param>
        /// <exception cref="ArgumentOutOfRangeException">当索引小于 0 或超出数组最大索引范围时抛出。</exception>
        public HourGod(int index)
        {
            if (index < 0 || index >= HourGodNames.Length) throw new ArgumentOutOfRangeException("index");

            this.Name = HourGodNames[index];
        }


        /// <summary>
        /// 返回当前神煞的名称字符串。
        /// </summary>
        /// <returns>返回 <see cref="Name"/> 属性的值。</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// 将当前神煞的名称包装为带有对应颜色样式的 HTML 字符串。
        /// </summary>
        /// <returns>返回带有颜色标签的 HTML 字符串（例如：&lt;font color='#RRGGBB'&gt;名称&lt;/font&gt;）。</returns>
        public string ToHtmlString()
        {
            string st = "<font color='#" + Color.ToHex() + "'>" + this.Name + "</font>";
            return st;
        }
    }
}
