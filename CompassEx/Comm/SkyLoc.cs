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
    /// 表示天干地支组合（如“甲子”、“乙丑”等六十甲子单元）的实体类。
    /// </summary>
    /// <remarks>
    /// 本类作为干支历法推排的核心单元，通过将一个独立的天干实例（<see cref="SkyClass"/>）与一个独立的地支实例（<see cref="LocClass"/>）进行复合封装，提供了干支合称输出、跨类索引校验以及干支对象的动态初始化功能。
    /// </remarks>
    public class SkyLoc : IEquatable<SkyLoc>
    {
        #region 属性

        /// <summary>
        /// 60甲子
        /// </summary>
        public static SkyLoc[] SkyLoc60
        {
            get
            {
                if (field == null) field = Get60SkyLocs();

                return field;

            }
        } = default!;


        public static string[] SkyLoc60Name
        {
            get
            {
                if (field == null) field = Get60SkyLocNames();

                return field;

            }
        } = default!;

        /// <summary>
        /// 获取当前天干地支组合的完整字面名称（如“甲子”、“丙寅”等）。
        /// </summary>
        /// <value>
        /// 一个由两个汉字组成的 <see cref="string"/> 字符串，动态拼接自 <see cref="SkyClass.Name"/> 与 <see cref="LocClass.Name"/>。
        /// </value>
        public string Name { get { return Sky.Name + Loc.Name; } }

        /// <summary>
        /// 获取当前干支组合中的天干实例对象。
        /// </summary>
        /// <value>
        /// 包含完整阴阳、五行及索引元数据的 <see cref="SkyClass"/> 实例。
        /// </value>
        public SkyClass Sky { get; private set; }

        /// <summary>
        /// 获取当前干支组合中的地支实例对象。
        /// </summary>
        /// <value>
        /// 包含完整时辰、阴阳、五行及索引元数据的 <see cref="LocClass"/> 实例。
        /// </value>
        public LocClass Loc { get; private set; }

        #endregion


        #region 构造函数

        /// <summary>
        /// 基于指定的双字干支组合名称（如“甲子”）初始化 <see cref="SkyLoc"/> 类的新实例。
        /// </summary>
        /// <param name="SkyLocName">由天干和地支按顺序组成的双字字符串（例如：“甲子”）。</param>
        /// <exception cref="IndexOutOfRangeException">当输入的字符串长度不足、或拆分出的单字无法在全局干支元数据中找到对应索引时抛出此异常。</exception>
        /// <remarks>
        /// <para><b>解构原理：</b></para>
        /// <para>方法内部会自动提取 <paramref name="SkyLocName"/> 的第 1 个字符（索引 0）作为天干字面量，提取第 2 个字符（索引 1）作为地支字面量。</para>
        /// <para>随后，通过分别检索它们在 <see cref="SkyClass.SkyNames"/> 和 <see cref="LocClass.LocNames"/> 中的位置，隐式链式传递给核心的双索引构造函数完成装配。</para>
        /// </remarks>
        public SkyLoc(string SkyLocName) : this(SkyClass.SkyNames.IndexOf(SkyLocName[0].ToString()), LocClass.LocNames.IndexOf(SkyLocName[1].ToString()))
        {
        }

        /// <summary>
        /// 基于指定的天干序列索引与地支序列索引初始化 <see cref="SkyLoc"/> 类的新实例。
        /// </summary>
        /// <param name="SkyIndex">天干在十天干序列中的绝对索引位置，有效取值范围为 <c>0 ~ 9</c>。可参考：<see cref="SkyClass.SkyNames"/>。</param>
        /// <param name="LocIndex">地支在十二地支序列中的绝对索引位置，有效取值范围为 <c>0 ~ 11</c>。可参考：<see cref="LocClass.LocNames"/>。</param>
        /// <exception cref="IndexOutOfRangeException">当传入的天干索引或地支索引超出其各自对应的合法物理数组边界时抛出此异常。</exception>
        /// <remarks>
        /// <b>★ 边界安全重构说明：</b>
        /// 已修正历史版本中对边界检查使用大于号（<c>&gt;</c>）导致的拦截穿透漏洞（例如：当 <paramref name="SkyIndex"/> 传入 <c>10</c> 时原逻辑无法拦截，会在下一步初始化时引发系统崩溃）。现已全面收紧为严谨的元素范围闭区间防御。
        /// </remarks>
        public SkyLoc(int SkyIndex, int LocIndex)
        {
            // ★ 核心修正：收紧边界检查，将原先的 > 替换为 >=，防止索引等于 Length 时的穿透崩溃
            if (SkyIndex < 0 || SkyIndex >= SkyClass.SkyNames.Length)
                throw new IndexOutOfRangeException(nameof(SkyIndex));

            if (LocIndex < 0 || LocIndex >= LocClass.LocNames.Length)
                throw new IndexOutOfRangeException(nameof(LocIndex));

            this.Sky = new SkyClass(SkyIndex);
            this.Loc = new LocClass(LocIndex);
        }

        #endregion

        #region 方法
        /// <summary>
        /// 根据给定的年份干支（五虎遁月法），推算该月的天干地支。
        /// </summary>
        /// <remarks>
        /// 五虎遁口诀：“甲己之年丙作首，乙庚之岁戊为头，丙辛必定寻庚上，丁壬壬位顺行流，戊癸何方发，甲寅上好求。”
        /// 该方法根据年干或月干的对应关系，结合地支位置计算出正确的月干，并组合成完整的干支。
        /// </remarks>
        /// <returns>返回推算后的月份完整天干地支对应的 <see cref="SkyLoc"/> 对象。</returns>
        /// <summary>
        /// 根据给定的年份干支（五虎遁月法），推算该月的天干地支。
        /// </summary>
        /// <param name="YearS">年份的天干对象（<see cref="SkyClass"/>），用于获取当前年份的天干名称。</param>
        /// <param name="MonthL">月份的地支对象（<see cref="LocClass"/>），用于获取当前月份的地支名称。</param>
        /// <returns>返回推算后的月份完整天干地支对应的 <see cref="SkyLoc"/> 对象。</returns>
        public static SkyLoc FiveTiger(SkyClass YearS, LocClass MonthL)
        {
            int ilIndex = 0, isIndex = 0;
            string sName = "";
            string sSkyName = YearS.Name;
            string sMonthLocName = MonthL.Name;
            ilIndex = Array.IndexOf(LocClass.LocNames, sMonthLocName);//获得地支所在的位置

            isIndex = Array.IndexOf(SkyClass.SkyNames, sSkyName);//获得第一个月的天干位置
            ilIndex -= 2;//因为一月从寅起所以要减回2就索引位置
            if (ilIndex == -1) ilIndex = +11; else if (ilIndex == -2) ilIndex = +10;//这里分别为丑和子的索引指定
            string sSky = "";
            int iIndex = isIndex;
            if (isIndex > 4) iIndex = isIndex - 5; //根据规则如果日天干大于4，则减回5，甲和己都起甲，己的排第5（从0开始)则以甲为标准
            iIndex = iIndex * 2 + 2 + ilIndex;
            iIndex = iIndex % 10;

            sSky = SkyClass.SkyNames[iIndex];
            sName = sSky + sMonthLocName;
            return new SkyLoc(sName);
        }
        /// <summary>
        /// 根据日的天干地支与指定的小时数（五鼠遁时法），推算该时辰的天干地支。
        /// </summary>
        /// <remarks>
        /// 五鼠遁口诀：“甲己还加甲，乙庚丙作初，丙辛从戊起，丁壬庚子居，戊癸何方发，壬子是真途。”
        /// 此重载方法直接接收 <see cref="SkyLoc"/> 对象。
        /// </remarks>
        /// <param name="DaySL">包含日的天干地支信息的 <see cref="SkyLoc"/> 对象。</param>
        /// <param name="Hour">当前的小时数（0-23之间的整数），用于判断对应的时辰（如：1点为丑时）。</param>
        /// <returns>返回推算后的时辰完整天干地支对应的 <see cref="SkyLoc"/> 对象。</returns>
        public static SkyLoc FiveMouse(SkyLoc DaySL, int Hour)
        {
            return FiveMouse(DaySL.Name, Hour);
        }

        /// <summary>
        /// 根据日的天干地支字符串与指定的小时数（五鼠遁时法），推算该时辰的天干地支。
        /// </summary>
        /// <remarks>
        /// 五鼠遁口诀：“甲己还加甲，乙庚丙作初，丙辛从戊起，丁壬庚子居，戊癸何方发，壬子是真途。”
        /// 此方法通过解析日干支字符串和小时，计算出时干并与时支组合。
        /// </remarks>
        /// <param name="DaySkyLocName">日的天干地支字符串（例如：“甲子”）。</param>
        /// <param name="Hour">当前的小时数（0-23之间的整数），用于判断对应的时辰（如：1点为丑时）。</param>
        /// <returns>返回推算后的时辰完整天干地支对应的 <see cref="SkyLoc"/> 对象。</returns>
        public static SkyLoc FiveMouse(string DaySkyLocName, int Hour)
        {
            int ilIndex = 0, isIndex = 0;
            int iTime = Hour;

            if (iTime >= 23 || iTime < 1)//子
                ilIndex = 0;
            else if (iTime >= 1 && iTime < 3)//丑
                ilIndex = 1;
            else if (iTime >= 3 && iTime < 5)//寅
                ilIndex = 2;
            else if (iTime >= 5 && iTime < 7)//卯
                ilIndex = 3;
            else if (iTime >= 7 && iTime < 9)//辰
                ilIndex = 4;
            else if (iTime >= 9 && iTime < 11)//巳
                ilIndex = 5;
            else if (iTime >= 11 && iTime < 13)//午
                ilIndex = 6;
            else if (iTime >= 13 && iTime < 15)//未
                ilIndex = 7;
            else if (iTime >= 15 && iTime < 17)//申
                ilIndex = 8;
            else if (iTime >= 17 && iTime < 19)//酉
                ilIndex = 9;
            else if (iTime >= 19 && iTime < 21)//戌
                ilIndex = 10;
            else if (iTime >= 21 && iTime < 23)//亥
                ilIndex = 11;

            string sLocName = LocClass.LocNames[ilIndex]; //取出地支
            string sSkyName = DaySkyLocName.Substring(0, 1);//取日之天干

            isIndex = Array.IndexOf(SkyClass.SkyNames, sSkyName);//获得日的天干位置

            string sSky = "";
            int iIndex = isIndex;
            if (isIndex > 4) iIndex = isIndex - 5; //根据规则如果日天干大于4，则减回5，甲和己都起甲，己的排第5（从0开始)则以甲为标准

            iIndex = iIndex * 2;//根据五鼠遁的规则＊2就可以

            iIndex += ilIndex;//加上地支所在的位置
            if (iIndex > 9) iIndex -= 10;
            sSky = SkyClass.SkyNames[iIndex];

            return new SkyLoc(sSky + sLocName);
        }


        /// <summary>
        /// 获取经典正统六十甲子干支组合的字符串序列（只读原生数组）。
        /// </summary>
        /// <returns>返回一个长度为 60 的 <see cref="string"/> 原生数组，按历法流转顺序包含“甲子”到“癸亥”。</returns>
        /// <remarks>
        /// <para><b>历法推演原理与算法重构：</b></para>
        /// <para>本方法已重构为现代高精度的<b>单层模运算算法</b>。消除了历史版本中双重嵌套循环（<c>int j = i</c>）导致的干支横向错配与漏项缺陷。</para>
        /// <para>由于十天干与十二地支的最小公倍数为 60，本算法通过单层 <c>0 ~ 59</c> 闭环流转，对天干基数 10 和地支基数 12 进行动态取模（<c>%</c>），确保干支双轨道同步顺时针推进，完美符合《黄帝内经》及传统干支历法规范。</para>
        /// </remarks>
        private static string[] Get60SkyLocNames()
        {
            string[] ls = new string[60];

            // 使用单层循环与取模，精准排出天干地支同步流转的六十甲子
            for (int k = 0; k < 60; k++)
            {
                int skyIndex = k % 10;
                int locIndex = k % 12;
                ls[k] = SkyClass.SkyNames[skyIndex] + LocClass.LocNames[locIndex];
            }

            return ls;
        }

        /// <summary>
        /// 获取包含完整天干地支对象绑定的六十甲子实体单元序列（只读原生数组）。
        /// </summary>
        /// <returns>返回一个包含 60 个 <see cref="SkyLoc"/> 复合实体的原生数组，按历法顺序排列。</returns>
        /// <remarks>
        /// <b>性能优化说明：</b>本方法通过单层取模算法动态实例化 60 个干支单元。由于六十甲子属于体系内的核心只读静态元数据，重构后直接返回原生数组，避免了动态列表（<c>List</c>）扩容带来的二次内存拷贝与垃圾回收（GC）开销。
        /// </remarks>
        private static SkyLoc[] Get60SkyLocs()
        {
            SkyLoc[] ls = new SkyLoc[60];

            // 同步利用最小公倍数模算法，构建高内聚的干支复合实体
            for (int k = 0; k < 60; k++)
            {
                int skyIndex = k % 10;
                int locIndex = k % 12;
                ls[k] = new SkyLoc(skyIndex, locIndex);
            }

            return ls;
        }

        public override string ToString()
        {
            return this.Name;
        }





        #region 显式实现对比、运算符和Key 方法
        // 1. 一般的 Equals(object)，內部可以轉型並利用顯式介面來比對
        public override bool Equals(object obj)
        {
            return Equals(obj as SkyLoc);
        }

        // 2. 顯式實作 IEquatable<LocClass>.Equals
        bool IEquatable<SkyLoc>.Equals(SkyLoc other)
        {
            // 檢查是否為 null
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            // 使用 string 的比較方式（考慮大小寫或 null 的防禦）
            return string.Equals(this.Name, other.Name, StringComparison.Ordinal);
        }

        // 3. 務必配合 Name 計算 HashCode
        public override int GetHashCode()
        {
            // 若 Name 可能為 null，可以用 HashCode.Combine 或字串自身的 GetHashCode
            return Name != null ? Name.GetHashCode() : 0;
        }

        // 4. (選用) 重載 == 與 != 運算子，建議透過介面轉型來呼叫
        public static bool operator ==(SkyLoc left, SkyLoc right)
        {
            if (left is null) return right is null;
            return ((IEquatable<SkyLoc>)left).Equals(right);
        }

        public static bool operator !=(SkyLoc left, SkyLoc right)
        {
            return !(left == right);
        }


        #endregion



        #endregion



    }

}
