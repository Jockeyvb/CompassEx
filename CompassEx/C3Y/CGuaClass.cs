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

using CompassEx.Gua;
using System;
using System.Linq;

namespace CompassEx
{
    /// <summary>
    /// 三元罗盘专用六爻卦实体类，继承基础卦类 <see cref="GuaClass"/>
    /// </summary>
    /// <remarks>
    /// 扩展基础卦结构，增加「卦所临爻位」数据存储，用于三元玄空罗盘纳甲、断卦时标记该卦对应的1～2个爻位；
    /// 约束规则：单个卦最多只能关联2个爻位，超出则抛出参数越界异常。
    /// </remarks>
    public class CGuaClass : GuaClass
    {
        /// <summary>
        /// 获取当前卦在三元盘中所临的爻位编号数组
        /// </summary>
        /// <value>
        /// 允许 null（无临爻）；数组长度限制 0～2，最多存储2个爻序号(只能是相邻两个爻）
        /// </value>
        public int[]? PlaceYaos { get; private set; }

        /// <summary>
        /// 初始化三元罗盘六爻卦实例，并绑定该卦对应的临爻位。
        /// </summary>
        /// <param name="GuaNameOrAttrName">
        /// 卦的唯一标识。支持以下两种格式之一：
        /// <list type="table">
        /// <listheader>
        /// <term>格式类型</term>
        /// <description>传入示例与说明</description>
        /// </listheader>
        /// <item>
        /// <term>标准卦名</term>
        /// <description>如 <c>"乾"</c>、<c>"坤"</c>、<c>"震"</c>、<c>"巽"</c> 等标准六十四卦汉字名称。</description>
        /// </item>
        /// <item>
        /// <term>属性字段名</term>
        /// <description>底层数据模型映射的属性名称，内部将通过反射机制自动匹配卦体信息。</description>
        /// </item>
        /// </list>
        /// </param>
        /// <param name="PlaceYaos">
        /// 一个可变长度的整数数组（<c>params</c>），代表当前卦所临的爻位序号。
        /// <para>该参数使用极其灵活，支持以下传参方式：</para>
        /// <list type="bullet">
        /// <item><description>不传任何参数、传入 <see langword="null"/> 或空数组：表示当前卦无临爻。</description></item>
        /// <item><description>传入 1 个整数：绑定单个临爻。</description></item>
        /// <item><description>传入 2 个整数：绑定双临爻。<b>注意：这两个爻位必须是连续相邻的。</b></description></item>
        /// </list>
        /// </param>
        /// <remarks>
        /// <para><b>⚠️ 业务建模约束：</b></para>
        /// <para>在三元罗盘的六爻卦逻辑中，单卦所临的爻位存在严格的空间/数理限制：</para>
        /// <list type="number">
        /// <item><description><b>数量上限：</b>单个卦最多只能同时关联 2 个爻位。</description></item>
        /// <item><description><b>位置关联：</b>当同时存在 2 个临爻时，它们在空间上必须连续（例如：初爻与二爻、三爻与四爻），不可跨爻绑定。</description></item>
        /// </list>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 当传入的 <paramref name="PlaceYaos"/> 数组长度大于 <c>2</c> 时抛出。提示单个卦最多仅能关联 2 个爻位。
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 当传入的 <paramref name="PlaceYaos"/> 数组长度等于 <c>2</c>，但两个爻位序号不连续（不相邻）时抛出。
        /// </exception>
        public CGuaClass(string GuaNameOrAttrName, params int[]? PlaceYaos) : base(GuaNameOrAttrName)
        {
            // 校验临爻数量，限制最多2爻
            if (PlaceYaos != null && PlaceYaos.Length > 2)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(PlaceYaos), "所临爻不能超过2个爻"
                );
            }

            // 校验相邻性
            if (PlaceYaos != null && PlaceYaos.Length == 2 && PlaceYaos[0] + 1 != PlaceYaos[1])
            {
                throw new ArgumentException("只能是相邻的两个爻", nameof(PlaceYaos));
            }

            this.PlaceYaos = PlaceYaos;
        }

    }
}
