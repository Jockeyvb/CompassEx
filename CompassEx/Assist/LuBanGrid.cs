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
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CommLib
{


    /// <summary>
    /// 鲁班尺大格（分格）类。
    /// </summary>
    /// <remarks>
    /// 该类用于计算和表达传统堪舆学中“鲁班尺”的 8 个大格（如财、病、离、义等）。
    /// 鲁班尺主要用于阳宅建筑、门户、家具等尺寸的吉凶量度。
    /// 每个大格内部包含 4 个小格，大格的物理固定循环尺寸常数定义为 53.625 毫米。
    /// </remarks>
    public class LuBanGrid
    {
        /// <summary>
        /// 鲁班尺 8 大格名称的静态只读数组。
        /// </summary>
        public static string[] LuBanGridName = { "财", "病", "离", "义", "官", "劫", "害", "吉" }; //鲁班大格

        /// <summary>
        /// 鲁班尺 8 大格对应的传统吉凶颜色标记数组（<see cref="Color.Red"/> 代表吉，<see cref="Color.Black"/> 代表凶）。
        /// </summary>
        public static Color[] LuBanGridColor = { Color.Red, Color.Black, Color.Black, Color.Red, Color.Red, Color.Black, Color.Black, Color.Red };

        /// <summary>
        /// 当前大格实例对应的吉凶颜色（红吉，黑凶）。
        /// </summary>
        public Color GridColor;

        /// <summary>
        /// 当前大格在 8 个循环大格中的绝对索引值（取值范围：0 - 7）。
        /// </summary>
        public int Index;

        /// <summary>
        /// 当前大格在鲁班尺上的理论起始刻度值（单位：毫米）。
        /// </summary>
        public double StartValue;

        /// <summary>
        /// 当前大格在鲁班尺上的理论结束刻度值（单位：毫米）。
        /// </summary>
        public double EndValue;

        /// <summary>
        /// 当前大格的名称（如“财”、“病”等）。
        /// </summary>
        public string GridName;

        /// <summary>
        /// 鲁班尺每个大格的固定循环尺寸常数（定义为 53.625 毫米）。
        /// </summary>
        public const double GridFixed = 53.625;

        /// <summary>
        /// 触发或传入的实际实现刻度值（若创建时未显式指定有效的刻度，则默认返回 <see cref="StartValue"/>）。
        /// </summary>
        public double Value = -1;

        /// <summary>
        /// 当前物理刻度值精确落在哪一个具体的鲁班小格对象（<see cref="LuBanSubGrid"/>）上。
        /// </summary>
        public LuBanSubGrid SG;

        /// <summary>
        /// 当前大格内部所包含的全部 4 个连续小格的集合列表。
        /// </summary>
        public List<LuBanSubGrid> Child = new List<LuBanSubGrid>();

        /// <summary>
        /// 内部私有字段：缓存比本值更大的最近一个吉利刻度值。
        /// </summary>
        private double rightGoodValue;

        /// <summary>
        /// 获取比当前刻度值更大的最近一个吉利（红色）刻度边界值。
        /// </summary>
        /// <value>
        /// 如果当前值非法返回 -1；若当前大格本身就是吉（红色），则无需寻找，直接返回 -1；
        /// 否则向右（向后）遍历尺面大格周期，寻找到第一个吉利大格的起始刻度加 1 毫米。
        /// </value>
        public double RightGoodValue
        {
            get
            {
                // 健壮性检查：若刻度值尚未有效设置，直接返回 -1
                if (this.Value < 0) return -1;
                // 如果当前大格本身就是吉利的颜色，直接返回 -1
                if (this.GridColor == Color.Red) return -1;//如果是吉则不需要寻

                double EndValue = this.EndValue;
                // 从当前大格的下一个大格开始向右递增遍历
                for (int i = this.Index + 1; i < LuBanGridName.Length; i++)
                {
                    // 按步长累加并向下取整，推算右侧大格的边界，再减 1 毫米修正
                    EndValue = Math.Floor(EndValue + GridFixed) - 1;
                    // 一旦遇到标红的吉利大格，则对其位置进行逆向实例化以获取精确的 StartValue
                    if (LuBanGridColor[i] == Color.Red)
                    {
                        LuBanGrid LBG = new LuBanGrid(EndValue);
                        rightGoodValue = LBG.StartValue + 1; // 锁定右侧吉利位置
                        break;
                    }
                }
                return rightGoodValue;
            }
        }

        /// <summary>
        /// 内部私有字段：缓存比本值更小的最近一个吉利刻度值。
        /// </summary>
        private double leftGoodValue;

        /// <summary>
        /// 获取比当前刻度值更小的最近一个吉利（红色）刻度边界值。
        /// </summary>
        /// <value>
        /// 如果当前值非法返回 -1；若当前大格本身就是吉（红色），则无需寻找，直接返回 -1；
        /// 否则向左（向前）遍历尺面大格周期，寻找到第一个吉利大格的结束刻度。
        /// </value>
        public double LeftGoodValue
        {
            get
            {
                // 健壮性检查：若刻度值尚未有效设置，直接返回 -1
                if (this.Value < 0) return -1;
                // 如果当前大格本身就是吉利的颜色，直接返回 -1
                if (this.GridColor == Color.Red) return -1;//如果是吉则不需要寻

                double EndValue = this.EndValue;
                // 从当前大格的上一个大格开始向左递减遍历
                for (int i = this.Index - 1; i >= 0; i--)
                {
                    // 按步长递减并向下取整，推算左侧大格的边界位置
                    EndValue = Math.Floor(EndValue - GridFixed);
                    // 一旦遇到标红的吉利大格，则对其位置进行逆向实例化以获取精确的 EndValue
                    if (LuBanGridColor[i] == Color.Red)
                    {
                        LuBanGrid LBG = new LuBanGrid(EndValue);
                        leftGoodValue = LBG.EndValue; // 锁定左侧吉利位置
                        break;
                    }
                }
                return leftGoodValue;
            }
        }

        /// <summary>
        /// 根据累计的大格总步数（次数）统一计算并配置当前大格实例的所有相关吉凶属性、关联小格。
        /// </summary>
        /// <param name="Times">累计的大格总步数（未取余的原始计数）。</param>
        public void SetGrid(int Times)
        {
            // 1. 根据总步数对 8 取余，计算出当前大格在周期内的标准索引 (0-7)
            int index = Times % LuBanGridName.Length;
            this.Index = index;

            // 2. 映射大格的名称
            this.GridName = LuBanGrid.LuBanGridName[index];

            // 3. 计算当前大格在尺面上的理论起始刻度（毫米）并向下取整
            this.StartValue = Math.Floor(Times * GridFixed);

            // 4. 计算当前大格在尺面上的理论结束刻度（毫米）并向下取整
            this.EndValue = Math.Floor((Times + 1) * GridFixed);

            // 5. 健壮性处理：如果 Value 还没有被赋予有效的正数刻度，则默认将其置为当前格的起始刻度
            this.Value = this.Value < 0 ? StartValue : this.Value;

            // 6. 绑定大格的传统吉凶颜色属性
            this.GridColor = LuBanGridColor[index];

            // 7. 依据当前大格的实现刻度，构建并关联精确位置处的单个小格对象
            this.SG = new LuBanSubGrid(this.Value);
            this.SG.Parent = this;

            // 8. 获取并装载当前大格范围内完整的一组（共4个）小格对象到 Child 列表中
            this.Child = LuBanSubGrid.GetGroup(this.Value, this);

            //for (int i = index * 4; i < (index + 1) * 4; i++)
            //{
            //    LuBanSubGrid LBSG = new LuBanSubGrid(LuBanSubGrid.LuBanSubGridName [ i] );
            //    LBSG.Parent = this;
            //    this.Child.Add(LBSG);
            //}
        }

        /// <summary>
        /// 根据大格名称（如“财”）实例化一个鲁班大格对象。
        /// </summary>
        /// <param name="GridName">要匹配的大格名称字符串。</param>
        public LuBanGrid(String GridName)
        {
            // 查找该大格名称在 8 大格数组中第一次出现的索引位置
            int index = Array.IndexOf(LuBanGridName, GridName);

            // 健壮性检查：如果传入了错误的名称，查找结果为 -1，则直接终止构造
            if (index < 0) return;

            // 调用内部核心方法初始化属性
            SetGrid(index);
        }

        /// <summary>
        /// 根据指定的鲁班尺物理实际刻度值实例化一个大格对象。
        /// </summary>
        /// <param name="Value">输入的长度刻度值（单位：毫米）。</param>
        public LuBanGrid(double Value)
        {
            // 健壮性检查：传入负数刻度属非法输入，直接拦截
            if (Value < 0) return;

            // 1. 将刻度值向下取整
            Value = Math.Floor(Value);

            // 2. 局部变量声明（留存原代码的分析变量 a）
            var a = Value / GridFixed;

            // 3. 计算当前刻度累计属于第几个大格（总步数）
            double d = Math.Floor(Value / GridFixed);

            //if (Value % GridFixed == 0 && Value > 0)//如果刚刚好，也算本格
            //{
            //    d -= 1;
            //}

            // 4. 记录当前的实际实现值
            this.Value = Value;

            // 5. 传入累计的总大格步数，去配置当前大格的各项关键属性
            SetGrid((int)d);
        }
    }

    /// <summary>
    /// 鲁班尺小格（分格）类。
    /// </summary>
    /// <remarks>
    /// 该类用于计算和表达传统堪舆学中“鲁班尺”的 32 个小格（如财德、宝库等）。
    /// 每 4 个小格组合为一个大格（由 <see cref="LuBanGrid"/> 表达）。
    /// 鲁班尺每小格的物理固定尺寸约为 13.40625 毫米。
    /// </remarks>
    public class LuBanSubGrid
    {
        /// <summary>
        /// 鲁班尺 32 小格名称的静态只读数组。
        /// </summary>
        public static string[] LuBanSubGridName = { "财德", "宝库", "六合", "迎福", "退财", "公事", "牢执", "孤寡", "长库", "劫财", "官鬼", "失脱", "添丁", "益利", "贵子", "大吉", "顺科", "横财", "进益", "富贵", "死别", "退口", "离乡", "失财", "灾至", "死绝", "病临", "口舌", "财至", "登科", "进宝", "兴旺" }; //

        /// <summary>
        /// 鲁班尺 32 小格各自对应的吉凶详细断语及释义说明文字。
        /// </summary>
        private static string[] LuBanSubGridInfos = { "指在财，德善，功德方面有表现。", "比喻可得或储藏珍贵物品。", "合和美满。六合为天地四方。", "迎接福。福为幸福，利益。", "损财，破财之意。", "多指因公家的事如贪污受贿及案件官司等。", "指牢狱之灾。", "指有孤独寡居的行为。", "古有监狱之说。", "破耗及耗损财。", "指有官煞引起之事。", "物品失落、人离散之意。", "古时生男孩叫添丁", "增加了财资利禄。", "日后能显贵的子嗣。", "吉祥吉利。", "顺利通过考试而获中。", "意外之财。", "收益进益。", "有财有势。", "即永别。", "指有孝服之事。", "背井离乡。", "财物损失或丢失。", "灾殃祸患到。", "死得干干净净。", "疾病来临。", "争执争吵。", "即财到。", "考试被录取。", "招财进宝。", "兴盛旺盛。" };

        /// <summary>
        /// 当前小格所属的父级大格对象。
        /// </summary>
        public LuBanGrid Parent;

        /// <summary>
        /// 当前小格在 32 个循环小格中的绝对索引值（取值范围：0 - 31）。
        /// </summary>
        public int Index;

        /// <summary>
        /// 当前小格在鲁班尺上的理论起始刻度值（单位：毫米）。
        /// </summary>
        public double StartValue;

        /// <summary>
        /// 当前小格在鲁班尺上的理论结束刻度值（单位：毫米）。
        /// </summary>
        public double EndValue;

        /// <summary>
        /// 触发或传入的实际实现刻度值（若创建时未显式指定有效的刻度，则默认返回 <see cref="StartValue"/>）。
        /// </summary>
        public double Value = -1;

        /// <summary>
        /// 当前小格的详细吉凶断语及释义说明信息。
        /// </summary>
        public string LuBanSubGridInfo = "";

        /// <summary>
        /// 当前小格的名称（如“财德”、“宝库”等）。
        /// </summary>
        public string SubGridName;

        /// <summary>
        /// 鲁班尺每小格的固定尺寸常数（这里定义为 13.40625 毫米）。
        /// </summary>
        public const double SubGridFixed = 13.40625;

        /// <summary>
        /// 根据累计的小格总步数（次数）计算并设置当前小格的各项属性。
        /// </summary>
        /// <param name="Times">累计的小格总步数（未取余的原始计数）。</param>
        private void SetSubGrid(int Times)
        {
            // 1. 根据总步数对 32 取余，计算出当前小格在周期内的标准索引 (0-31)
            this.Index = Times % LuBanSubGrid.LuBanSubGridName.Length;

            // 2. 根据索引从静态数组中获取对应的小格吉凶名称
            this.SubGridName = LuBanSubGrid.LuBanSubGridName[this.Index];

            // 3. 计算并向下取整当前小格的起始毫米刻度值
            this.StartValue = Math.Floor(Times * SubGridFixed);

            // 4. 计算并向下取整当前小格的结束毫米刻度值
            this.EndValue = Math.Floor((Times + 1) * SubGridFixed);

            // 5. 健壮性处理：如果 Value 还没有被赋予有效的正数刻度，则默认将其置为当前格的起始刻度
            this.Value = this.Value < 0 ? StartValue : this.Value;

            // 6. 自动抽取并映射当前小格的详细吉凶白话文释义
            this.LuBanSubGridInfo = LuBanSubGridInfos[this.Index];
        }

        /// <summary>
        /// 依据指定的刻度值，获取其所在的大格所包含的全部 4 个小格（作为完整的一组返回）。
        /// </summary>
        /// <param name="value">输入的物理刻度值。</param>
        /// <param name="parent">所属的父级大格对象。</param>
        /// <returns>返回包含同组 4 个 <see cref="LuBanSubGrid"/> 对象的列表集合。</returns>
        public static List<LuBanSubGrid> GetGroup(double value, LuBanGrid parent)
        {
            List<LuBanSubGrid> li = new List<LuBanSubGrid>();

            // 1. 先用当前的刻度值临时构建一个锚点小格对象
            LuBanSubGrid LBSG = new LuBanSubGrid(value);

            // 2. 核心算法：推算当前小格所在大格（4个小格一组）的第一小格的绝对次数索引
            double t = LBSG.Index - (LBSG.Index % 4) + 1; //取本组的第一个格

            // 3. 核心算法：逆向推算当前组第一小格的基础起始刻度位置
            double st = LBSG.EndValue - (((LBSG.Index % 4)) * SubGridFixed);

            int j = 0;
            // 4. 循环 4 次，依次构建出该大格包含的 4 个连续小格
            for (double i = t; i < t + 4; i++)
            {
                // 依据基准刻度 + 偏移量创建全新的小格对象
                var sg = new LuBanSubGrid(st + (j * SubGridFixed));
                sg.Parent = parent;

                // 压入返回列表
                li.Add(sg);
                j++;
            }

            return li;
        }

        /// <summary>
        /// 根据小格名称实例化一个鲁班小格对象。
        /// </summary>
        /// <param name="SubGridName">要匹配的小格名称（例如“大吉”）。</param>
        public LuBanSubGrid(String SubGridName)
        {
            // 查找该名称在 32 数组中第一次出现的索引位置
            int index = Array.IndexOf(LuBanSubGridName, SubGridName);

            // 健壮性检查：如果传入了错误的名称，查找结果为 -1，则直接终止构造
            if (index < 0) return;

            // 调用内部核心方法初始化属性
            SetSubGrid(index);
        }

        /// <summary>
        /// 根据指定的鲁班尺物理刻度值实例化一个小格对象。
        /// </summary>
        /// <param name="Value">输入的长度刻度值（单位：毫米）。</param>
        public LuBanSubGrid(double Value)
        {
            // 健壮性检查：传入负数刻度属非法输入，直接拦截
            if (Value < 0) return;

            // 1. 将刻度值向下取整
            Value = Math.Floor(Value);

            // 2. 计算当前刻度累计属于第几个小格（总步数）
            double d = Math.Floor(Value / SubGridFixed);

            // if (Value % SubGridFixed == 0 && Value > 0) //如果刚刚好，也算本格
            //{
            //    d -= 1;
            //}

            // 3. 记录当前的实际实现值
            this.Value = Value;
            //  d = d % LuBanSubGrid.LuBanSubGridName.Length;

            // 4. 传入累计的总步数，去配置当前格的各项关键属性
            SetSubGrid((int)d);
        }
    }



}
