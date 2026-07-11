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

namespace CompassEx.C3Y
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
        /// 允许 null（无临爻）；数组长度限制 0～2，最多存储2个爻序号
        /// </value>
        public int[]? PlaceYaos { get; private set; }

        /// <summary>
        /// 初始化三元罗盘六爻卦实例，并绑定该卦对应的临爻位
        /// </summary>
        /// <param name="GuaNameOrAttrName">卦标识，支持两种传入格式：
        /// <list type="bullet">
        /// <item><term>卦名</term><description>如：乾、坤、震、巽等标准六十四卦名称</description></item>
        /// <item><term>属性字段名</term><description>底层映射实体属性名，用于反射匹配卦信息</description></item>
        /// </list>
        /// </param>
        /// <param name="PlaceYaos">可变参数，当前卦所临爻位序号；可传 null / 不传值 / 1个爻 / 2个爻；传入超过2个爻将抛出 <see cref="ArgumentOutOfRangeException"/></param>
        /// <exception cref="ArgumentOutOfRangeException">当传入临爻数组长度大于2时触发，提示单个卦最多仅能关联2个爻位</exception>
        public CGuaClass(string GuaNameOrAttrName, params int[]? PlaceYaos) : base(GuaNameOrAttrName)
        {
            // 校验临爻数量，限制最多2爻
            if (PlaceYaos != null && PlaceYaos.Length > 2)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(PlaceYaos),
                    "所临爻不能超过2个爻"
                );
            }
            this.PlaceYaos = PlaceYaos;
        }
    }
}
