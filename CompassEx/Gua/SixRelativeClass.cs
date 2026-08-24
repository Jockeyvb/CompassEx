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

using CompassEx.Comm;
using System;

namespace CompassEx.Gua
{
    /// <summary>
    /// 表示六爻或八字预测学中“六亲”（父母、子孙、官鬼、妻财、兄弟）的装配与推演实体类。
    /// </summary>
    /// <remarks>
    /// 六亲是由全卦宫（或日元）的五行属性与各个爻位（或干支）的五行属性通过“生克制化”的河洛数理规则比对推演而来。
    /// 本类提供了通过字面名称快速创建六亲对象，以及通过标准五行生克规则（<see cref="FiveAttrRule"/>）进行实体动态装配的静态工厂方法。
    /// </remarks>
    public class SixRelativeClass
    {
        /// <summary>
        /// 六亲经典字面名称的全局静态只读序列。
        /// </summary>
        /// <value>
        /// 包含 5 个标准六亲名称：妻财(0)、子孙(1)、兄弟(2)、官鬼(3)、父母(4)。
        /// </value>
        public static readonly string[] SixRelativeNames = { "妻财", "子孙", "兄弟", "官鬼", "父母" };

        #region 属性



        /// <summary>
        /// 获取当前六亲实体的字面名称（如“父母”、“子孙”等）。
        /// </summary>
        /// <value>一个 <see cref="string"/> 字符串，表示当前爻位对应的六亲称谓。</value>
        public string? Name { get { return SixRelativeNames[this.Index]; } }

        /// <summary>
        /// 六亲索引
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// 六新属性
        /// </summary>
        public FiveAttrRule FiveAttrRule { get { return (FiveAttrRule)Index; } }


        /// <summary>
        /// 六神干支
        /// </summary>
        public SkyLoc SkyLoc { get; internal set; }


        #endregion


        #region 方法


        public SixRelativeClass(string SixRelativeName) : this((FiveAttrRule)SixRelativeNames.IndexOf(SixRelativeName))
        { }


        public SixRelativeClass(FiveAttrRule far)
        {
            int index = (int)far;
            if (index < 0 || index >= SixRelativeNames.Length)
                throw new IndexOutOfRangeException($"生克规则转换值【{index}】超出了六亲元数据数组的合法边界。");


            this.Index = index;


        }




        /// <summary>
        /// 根据标准的五行生克规则对象，动态推演并实例化对应的六亲类实体。
        /// </summary>
        /// <param name="far">包含当前爻位与主宫生克关系的 <see cref="FiveAttrRule"/> 规则对象或枚举。</param>
        /// <returns>返回经过生克数理映射后装配完毕的 <see cref="SixRelativeClass"/> 六亲实体对象。</returns>
        /// <exception cref="IndexOutOfRangeException">当传入的生克规则对象转换后的整型数值超出 <c>0 ~ 4</c> 的六亲数组物理边界时抛出此异常。</exception>
        /// <remarks>
        /// <para><b>⚠️ 类型强转契约说明：</b></para>
        /// <para>本方法内部依赖于将自定义五行生克规则 <paramref name="far"/> 显式强制转换为整型（<c>(int)far</c>）来直接作为 <see cref="SixRelatives"/> 数组的内存检索下标。</para>
        /// <para>为了防止系统在运行时发生非预期崩溃，必须确保 <see cref="FiveAttrRule"/> 内部枚举元素的排布顺位与本类的六亲元数据序列（妻财、子孙、兄弟、官鬼、父母）在易学数理上保持<b>绝对严格的严格对齐</b>。</para>
        /// </remarks>
        public static SixRelativeClass GetSixRelative(FiveAttrRule far)
        {
            int index = (int)far;
            if (index < 0 || index >= SixRelativeNames.Length)
                throw new IndexOutOfRangeException($"生克规则转换值【{index}】超出了六亲元数据数组的合法边界。");

            SixRelativeClass src = new SixRelativeClass(far);

            return src;
        }

        #endregion
    }

}
