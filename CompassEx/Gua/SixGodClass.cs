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
    /// 表示六爻预测学中“六神”（六兽）的动态装配与排盘实体类。
    /// </summary>
    /// <remarks>
    /// 本类完整实现了传统六爻预测中由日天干发动起六神的排盘法则。
    /// 通过将十天干映射到对应的六神初始起点，自下而上（初爻至上爻）依次排布青龙、朱雀、勾陈、腾蛇、白虎、玄武，常用于六爻装骨髓及神煞吉凶推演。
    /// </remarks>
    public class SixGodClass
    {
        /// <summary>
        /// 六神（六兽）经典字面名称的全局静态只读序列。
        /// </summary>
        /// <value>
        /// 包含 6 个标准六神名，严格按照顺时针五行流转顺序排列：青龙(0)、朱雀(1)、勾陈(2)、腾蛇(3)、白虎(4)、玄武(5)。
        /// </value>
        public readonly static string[] SixGods = { "青龙", "朱雀", "勾陈", "腾蛇", "白虎", "玄武" };

        // ★ 极致性能与逻辑防线：十天干起六神起始索引的静态查找数组（物理硬件级直达 O(1)）
        // 索引 0~9 分别对应：甲(0), 乙(0), 丙(1), 丁(1), 戊(2), 己(3), 庚(4), 辛(4), 壬(5), 癸(5)
        private static readonly int[] _skyToSixGodStartLookup = { 0, 0, 1, 1, 2, 3, 4, 4, 5, 5 };

        #region 属性

        /// <summary>
        /// 简称
        /// </summary>
        public string? ShortName { get => Name?.Substring(0, 1); }

        /// <summary>
        /// 获取当前爻位所配六神的字面名称（如“青龙”、“玄武”）。
        /// </summary>
        /// <value>一个 <see cref="string"/> 字符串，代表当前爻的神煞名称。</value>
        public string? Name { get; private set; }

        /// <summary>
        /// 获取当前卦局中，六神发动的源头起首索引。
        /// </summary>
        /// <value>一个 <see cref="int"/> 整数，有效范围为 <c>0 ~ 5</c>，代表日干在初爻发动的原始六神位置。</value>
        /// <remarks>
        /// <b>数理规范：</b>同一卦局中排出的 6 个六神对象，其 <see cref="StartIndex"/> 应当完全一致，用来标记本局的起神基准点。
        /// </remarks>
        public int StartIndex { get; private set; }

        #endregion


        #region 方法

        /// <summary>
        /// 根据传入的日天干对象，动态推算并装配当前六爻卦局的六神只读序列。
        /// </summary>
        /// <param name="DaySky">输入的当前排盘日天干 <see cref="SkyClass"/> 实体实例。</param>
        /// <returns>返回包含 6 个爻位六神实体的原生数组。</returns>
        public static SixGodClass[] GetSixGod(SkyClass DaySky)
        {
            return GetSixGod(DaySky.Name);
        }

        /// <summary>
        /// 根据传入的日天干字面名称，动态推算并装配当前六爻卦局的六神只读序列。
        /// </summary>
        /// <param name="DaySkyName">输入的日天干单字名称（如“甲”、“戊”等）。</param>
        /// <returns>若成功匹配到合法的十天干，则返回包含 6 个爻位六神实体的原生数组；若名称非法则返回 <c>null</c>。</returns>
        public static SixGodClass[]? GetSixGod(string DaySkyName)
        {
            int iPos = Array.IndexOf(SkyClass.SkyNames, DaySkyName);
            if (iPos == -1) return null;
            return GetSixGod(iPos);
        }

        /// <summary>
        /// 基于日天干的绝对顺位索引，通过极致性能的静态数组查找算法，排布自初爻至上爻的六神完备序列。
        /// </summary>
        /// <param name="iDaySkyIndex">日天干在十天干序列中的绝对索引位置，有效取值范围为 <c>0 ~ 9</c>。</param>
        /// <returns>返回一个长度严格为 6 的 <see cref="SixGodClass"/> 原生数组，按初爻到上爻的顺序排列。</returns>
        /// <exception cref="IndexOutOfRangeException">当传入的日天干索引超出 <c>0 ~ 9</c> 的合法天干边界时，内部查找矩阵会抛出越界异常。</exception>
        /// <remarks>
        /// <para><b>★ 算法升级与架构重构解析：</b></para>
        /// <para>1. <b>消灭数学分支：</b>废弃了历史版本中依靠整数除法（<c>iDaySkyIndex / 2</c>）与条件修正（<c>iPos++</c>）的模糊推导，改用确定性的静态预存查找数组 <c>_skyToSixGodStartLookup</c>，在 <c>O(1)</c> 复杂度内瞬间锁定本局六神起首位置。</para>
        /// <para>2. <b>修复数据血缘：</b>修正了原本在循环装配时错误将每个爻位独立递增值赋予 <see cref="StartIndex"/> 的逻辑偏误。当前版本中，全数组 6 个爻位对象的 <see cref="StartIndex"/> 均死死锚定在原始起首点上，而各爻位的实际六神名称（<see cref="Name"/>）则自下而上进行完备的环形顺流步进（若超过索引 5 则自动闭环归零）。</para>
        /// </remarks>
        public static SixGodClass[] GetSixGod(int iDaySkyIndex)
        {
            if (iDaySkyIndex < 0 || iDaySkyIndex >= SkyClass.SkyNames.Length)
                throw new IndexOutOfRangeException(nameof(iDaySkyIndex));

            // 1. 瞬间利用静态数组查找锁定初爻发动的原始六神位置
            int startPos = _skyToSixGodStartLookup[iDaySkyIndex];

            // 2. 初始化长度严格为 6 的原生数组，承载初爻至上爻
            SixGodClass[] sgcs = new SixGodClass[6];
            int currentPos = startPos;

            for (int i = 0; i < 6; i++)
            {
                SixGodClass sgc = new SixGodClass
                {
                    // 各爻位的实际六神名称按顺序环形步进
                    Name = SixGods[currentPos],
                    // 所有的 StartIndex 必须严格锁定在本局的统一发起源头上！
                    StartIndex = startPos
                };

                sgcs[i] = sgc;

                // 环形步进流转：超过玄武(5)后自动回到青龙(0)
                currentPos++;
                if (currentPos > 5) currentPos = 0;
            }

            return sgcs;
        }

        #endregion
    }

}
