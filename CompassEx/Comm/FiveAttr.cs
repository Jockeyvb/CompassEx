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
using CompassEx.Gua;
using Newtonsoft.Json;
using System;

namespace CompassEx.Comm
{

    /// <summary>
    /// 五行生克关系规则
    /// </summary>
    public enum FiveAttrRule : uint
    {
        None = 0,
        /// <summary>
        /// 我克为妻财
        /// </summary>
        MeCan = 1,

        /// <summary>
        /// 我生为子孙
        /// </summary>
        MeBirth = 2
            ,
        /// <summary>
        /// 同我为兄弟
        /// </summary>
        SameMe = 3,
        /// <summary>
        /// 克我为官鬼
        /// </summary>
        CanMe = 4,
        /// <summary>
        /// 生我为父母
        /// </summary>
        BirthMe = 5

    }


    /// <summary>
    ///  五行类
    /// </summary>
    public class FiveAttr : IEquatable<FiveAttr>
    {



        /// <summary>
        /// 五行名称数组
        /// </summary>
        public readonly static string[] FiveAttrNames = { "金", "木", "水", "火", "土" };

        /// <summary>
        /// 相克索引序列
        /// </summary>
        private readonly static string FiveAttrBothCans = "423014";

        /// <summary>
        /// 相生索引序列
        /// </summary>
        private readonly static string FiveAttrBothbirths = "402134";


        /// <summary>
        /// 五行名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 五行类所在索引
        /// </summary>
        public int Index { get { return Array.IndexOf(FiveAttrNames, this.Name); } }

        /// <summary>
        /// 构造五行
        /// </summary>
        /// <param name="Name">五行名称,参考：【<see cref="FiveAttrNames"/>】</param>
        /// <exception cref="IndexOutOfRangeException">若传入的不是五行的名称，则抛出异常</exception>
        public FiveAttr(string Name) : this(FiveAttrNames.IndexOf(Name))
        {

        }



        /// <summary>
        /// 构造五行
        /// </summary>
        /// <param name="iFiveAttrIndex">五行所在的索引,参考：【<see cref="FiveAttrNames"/>】</param>
        /// <exception cref="IndexOutOfRangeException">若传入的小于0 或大于5，则抛出异常</exception>
        [JsonConstructor]
        public FiveAttr([JsonProperty(nameof(Index))] int iFiveAttrIndex)
        {
            if (iFiveAttrIndex < 0 || iFiveAttrIndex >= FiveAttrNames.Length) throw new IndexOutOfRangeException();
            this.Name = FiveAttrNames[iFiveAttrIndex];
        }


        /// <summary>
        /// 五行之间获得的关系(金、木、水、火、土）
        /// </summary>
        /// <param name="WithFiveAttr">相关的五行</param>
        /// <returns></returns>
        public FiveAttrRule WhatRule(FiveAttr WithFiveAttr)
        {


            if (this.Index == WithFiveAttr.Index)
            {
                return FiveAttrRule.SameMe;

            }

            //--------我克 和克我-------------------------- -


            string s = this.Index.ToString() + WithFiveAttr.Index.ToString();//加起来看看能不能找到旁边的
            int i = FiveAttrBothCans.IndexOf(s);
            if (i > -1)
            {
                return FiveAttrRule.MeCan;//我克

            }
            s = WithFiveAttr.Index.ToString() + this.Index.ToString();//加起来看看能不能找到旁边的
            i = FiveAttrBothCans.IndexOf(s);
            if (i > -1)
            {
                return FiveAttrRule.CanMe;//克我

            }
            //----------------------我克和克我---------------------------

            //----------------------我生和生我---------------------------


            s = this.Index.ToString() + WithFiveAttr.Index.ToString();//加起来看看能不能找到旁边的
            i = FiveAttrBothbirths.IndexOf(s);
            if (i > -1)
            {
                return FiveAttrRule.MeBirth;//我生

            }
            s = WithFiveAttr.Index.ToString() + this.Index.ToString();//加起来看看能不能找到旁边的
            i = FiveAttrBothbirths.IndexOf(s);
            if (i > -1)
            {
                return FiveAttrRule.BirthMe;//生我

            }


            return FiveAttrRule.None;

        }




        /// <summary>
        /// 返回五行名称
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }



        /// <summary>
        ///    /// <summary>
        ///  根据名称获得两个地支之间的五行的关系：我克、我生、我同、克我、生我的关系。<br/>
        /// 0表示我克，1表示我生，2表示我同，3表示克我，4表示生我
        /// </summary>
        /// </summary>
        /// <param name="iMeLoc1">地支1索引</param>
        /// <param name="iLoc2">地支2索引</param>
        /// <returns></returns>
        public static FiveAttrRule GetBothLocRule(int iMeLoc1, int iLoc2)
        {


            LocClass Mel = LocClass.GetLocClass(iMeLoc1);
            LocClass l2 = LocClass.GetLocClass(iLoc2);
            return Mel.FiveAttr.WhatRule(l2.FiveAttr);

        }



        /// <summary>
        /// 根据属性名称获得五行之间的五行的关系：我克、我生、我同、克我、生我的关系。<br/>
        /// 0表示我克，1表示我生，2表示我同，3表示克我，4表示生我
        /// </summary>
        /// <param name="MeAttrName">五行名称1</param>
        /// <param name="OtherAttrName">五行名称2</param>
        /// <returns></returns>
        public static FiveAttrRule GetBothAttrRule(string MeAttrName, string OtherAttrName)
        {
            int iMe = Array.IndexOf(FiveAttrNames, MeAttrName);
            int il = Array.IndexOf(FiveAttrNames, OtherAttrName);
            FiveAttrRule far = GetBothAttrRule(iMe, il);

            return far;

        }

        /// <summary>
        /// 根据属性名称获得五行之间的五行的关系：我克、我生、我同、克我、生我的关系。<br/>
        /// 0表示我克，1表示我生，2表示我同，3表示克我，4表示生我</summary>
        /// <param name="iMeAttr">五行名称1索引</param>
        /// <param name="iOtherAttr">五行名称1索引</param>
        /// <returns></returns>
        public static FiveAttrRule GetBothAttrRule(int iMeAttr, int iOtherAttr)
        {
            FiveAttr me = new FiveAttr(iMeAttr);
            FiveAttr WithAttr = new FiveAttr(iOtherAttr);

            return me.WhatRule(WithAttr);

        }

        /// <summary>
        /// 判定并获取两个指定地支之间的“地支相害”关系描述（支持无序入参）。
        /// </summary>
        /// <param name="sLoc1">参与比对的第一项地支名称（如“子”、“丑”等）。</param>
        /// <param name="sLoc2">参与比对的第二项地支名称（如“未”、“午”等）。</param>
        /// <returns>若两地支构成相害，则返回对应的关系字符串（如“子与未害”）；若不构成相害或入参无效，则返回空字符串（<c>""</c>）。</returns>
        /// <remarks>
        /// <para><b>算法推演原理与重构说明：</b></para>
        /// <para>本方法已重构为<b>无序判定算法</b>。地支相害（又称六害）是由六合受冲推演而来。</para>
        /// <para>系统在获取到两地支在 <c>LocClass.LocNames</c> 中的索引后会执行升序排序。由此消除了参数传入先后顺序的严格限制，无论调用者传入 <c>("子", "未")</c> 还是 <c>("未", "子")</c> 均能精准识别，大幅提升了接口的健壮性。</para>
        /// </remarks>
        public static string BothHarm(string sLoc1, string sLoc2)
        {
            String[] S = { sLoc1, sLoc2 };
            int[] r = { -1, -1 };

            var j = 0; // ★ 已修正：从 foreach 内部移至外部，防止循环时 j 永远被重置为 0
            foreach (string s in S)
            {
                var i = Array.IndexOf(LocClass.LocNames, s);
                if (i > -1)
                {
                    r[j] = i;
                    j++;
                }
            }

            if (r[0] == -1 || r[1] == -1) return "";

            // ★ 核心重构：将提取出的地支索引进行升序排序，使算法支持无序传入
            Array.Sort(r);

            int[] it = { 0, 7 }; // 子与未害 
            if (it[0] == r[0] && it[1] == r[1]) return "子与未害";

            it[0] = 1; // 丑与午害 
            it[1] = 6;
            if (it[0] == r[0] && it[1] == r[1]) return "丑与午害";

            it[0] = 2; // 寅与巳害 
            it[1] = 5;
            if (it[0] == r[0] && it[1] == r[1]) return "寅与巳害";

            it[0] = 3; // 卯与辰害
            it[1] = 4;
            if (it[0] == r[0] && it[1] == r[1]) return "卯与辰害";

            it[0] = 8; // 申与亥害
            it[1] = 11;
            if (it[0] == r[0] && it[1] == r[1]) return "申与亥害";

            it[0] = 9; // 酉与戌害
            it[1] = 10;
            if (it[0] == r[0] && it[1] == r[1]) return "酉与戌害";

            return "";
        }


        /// <summary>
        /// 判定并获取两个指定地支之间的“地支相刑”关系描述（支持无序入参）。
        /// </summary>
        /// <param name="sLoc1">参与比对的第一项地支名称（如“子”、“卯”、“寅”等）。</param>
        /// <param name="sLoc2">参与比对的第二项地支名称（如“卯”、“子”、“巳”等）。</param>
        /// <returns>若两地支构成相刑，则返回对应的刑伤类别（如“恃势之刑”、“无恩之刑”、“无礼之刑”或“自刑”）；若不构成则返回空字符串（<c>""</c>）。</returns>
        /// <remarks>
        /// <para><b>算法推演原理与重构说明：</b></para>
        /// <para>本方法已重构为<b>无序判定算法</b>。包含三刑（寅巳申、丑戌未）、两刑（子卯）以及特殊地支（辰、午、酉、亥）的同支“自刑”判定。</para>
        /// <para>系统在获取到两地支在 <c>LocClass.LocNames</c> 中的索引后会执行升序排序，同时内部的比对矩阵也已同步调整为升序特征值。由此消除了参数传入先后顺序的严格限制，提升了系统的健壮性。</para>
        /// </remarks>
        public static string BothTorture(string sLoc1, string sLoc2)
        {
            String[] S = { sLoc1, sLoc2 };
            int[] r = { -1, -1 };

            var j = 0; // ★ 已修正：从 foreach 内部移至外部，防止循环时 j 永远被重置为 0
            foreach (string s in S)
            {
                var i = Array.IndexOf(LocClass.LocNames, s);
                if (i > -1)
                {
                    r[j] = i;
                    j++;
                }
            }

            // ★ 核心重构：将提取出的地支索引进行升序排序，使算法支持无序传入
            Array.Sort(r);

            // --- 恃势之刑 (寅2, 巳5, 申8) ---
            int[] it = { 2, 5 }; // 寅与巳
            if (it[0] == r[0] && it[1] == r[1]) return "恃势之刑";

            it[0] = 5; // 巳与申 
            it[1] = 8;
            if (it[0] == r[0] && it[1] == r[1]) return "恃势之刑";

            it[0] = 2; // 寅与申 (原为 8和2，排序后调整为 2和8)
            it[1] = 8;
            if (it[0] == r[0] && it[1] == r[1]) return "恃势之刑";

            // --- 无恩之刑 (丑1, 未7, 戌10) ---
            it[0] = 1; // 丑与未 (原为 1与7)
            it[1] = 7;
            if (it[0] == r[0] && it[1] == r[1]) return "无恩之刑";

            it[0] = 1; // 丑与戌 
            it[1] = 10;
            if (it[0] == r[0] && it[1] == r[1]) return "无恩之刑";

            it[0] = 7; // 未与戌 (原为 7与10)
            it[1] = 10;
            if (it[0] == r[0] && it[1] == r[1]) return "无恩之刑";

            // --- 无礼之刑 (子0, 卯3) ---
            it[0] = 0; // 子与卯 
            it[1] = 3;
            if (it[0] == r[0] && it[1] == r[1]) return "无礼之刑";

            // --- 辰午酉亥自刑 ---
            if (sLoc1.Equals(sLoc2))
            {
                if (sLoc1.Equals("辰") || sLoc1.Equals("午") || sLoc1.Equals("酉") || sLoc1.Equals("亥")) return "自刑";
            }

            return "";
        }


        /// <summary>
        /// 判定并获取两个指定地支之间的“地支相冲”关系。
        /// </summary>
        /// <param name="sLoc1">参与比对的第一项地支名称。</param>
        /// <param name="sLoc2">参与比对的第二项地支名称。</param>
        /// <returns>若两地支在十二地支顺位中绝对间隔为 6（即对冲），则返回字符串 <c>"相冲"</c>；否则返回空字符串（<c>""</c>）。</returns>
        public static bool BothConflict(string sLoc1, string sLoc2)
        {
            int iLoc1 = Array.IndexOf(LocClass.LocNames, sLoc1);
            int iLoc2 = Array.IndexOf(LocClass.LocNames, sLoc2);

            if (Math.Abs(iLoc1 - iLoc2) == 6) return true;

            return false;
        }

        /// <summary>
        /// 判定并获取两个指定地支之间的“地支六合”及合化后的五行或能量属性（支持无序入参）。
        /// </summary>
        /// <param name="sLoc1">参与比对的第一项地支名称（如“子”、“丑”等）。</param>
        /// <param name="sLoc2">参与比对的第二项地支名称（如“未”、“午”等）。</param>
        /// <returns>若两地支构成六合，则返回合化后的五行或能量特征字符串（如“土”、“金”、“水”、“木”、“火”或“日月”）；若不构成六合则返回空字符串（<c>""</c>）。</returns>
        /// <remarks>
        /// <para><b>算法推演原理与重构说明：</b></para>
        /// <para>本方法已重构为<b>无序判定算法</b>。系统在获取到两个地支在 <c>LocClass.LocNames</c> 中的整型索引后，会自动对其进行从小到大的升序排列。</para>
        /// <para>通过该机制，消除了历史版本中对地支先后传入顺序的严格依赖，无论传入 <c>("子", "丑")</c> 还是 <c>("丑", "子")</c> 均能精准识别，大幅提升了接口的健壮性。</para>
        /// </remarks>
        public static String LocCombine(string sLoc1, string sLoc2)
        {
            String[] S = { sLoc1, sLoc2 };
            int[] r = { -1, -1 };

            var j = 0; // ★ 已修正：从 foreach 内部移至外部，防止循环时 j 永远被重置为 0
            foreach (string s in S)
            {
                var i = Array.IndexOf(LocClass.LocNames, s);
                if (i > -1)
                {
                    r[j] = i;
                    j++;
                }
            }

            // ★ 核心重构：将提取出的地支索引进行升序排序，使算法支持无序传入
            Array.Sort(r);

            int[] it = { 0, 1 }; // 子丑合化土（排序后必然是 0 在前，1 在后）
            if (it[0] == r[0] && it[1] == r[1]) return "土";

            it[0] = 4;
            it[1] = 9; // 辰酉合化金
            if (it[0] == r[0] && it[1] == r[1]) return "金";

            it[0] = 5;
            it[1] = 8; // 巳申合化水
            if (it[0] == r[0] && it[1] == r[1]) return "水";

            it[0] = 2;
            it[1] = 11; // 寅亥合化木
            if (it[0] == r[0] && it[1] == r[1]) return "木";

            it[0] = 3;
            it[1] = 10; // 卯戌合化火
            if (it[0] == r[0] && it[1] == r[1]) return "火";

            it[0] = 6;
            it[1] = 7; // 午未合化日月
            if (it[0] == r[0] && it[1] == r[1]) return "日月";

            return "";
        }


        /// <summary>
        /// 判定并获取两个指定天干之间的“天干五合”及合化后的五行属性（支持无序入参）。
        /// </summary>
        /// <param name="sSky1">参与比对的第一项天干名称（如“甲”、“乙”等）。</param>
        /// <param name="sSky2">参与比对的第二项天干名称（如“己”、“庚”等）。</param>
        /// <returns>若两入参构成天干五合，则返回对应的合化五行字符串（如“土”、“金”、“水”、“木”、“火”）；若不构成五合或输入无效则返回空字符串（<c>""</c>）。</returns>
        /// <remarks>
        /// <para><b>算法推演原理与重构说明：</b></para>
        /// <para>本方法已重构为<b>无序判定算法</b>。系统在获取到两个天干在 <c>LocClass.LocNames</c> 中的整型索引后，会自动对其进行从小到大的升序排列。</para>
        /// <para>通过该机制，消除了历史版本中对天干先后传入顺序的严格依赖，大幅提升了外部接口调用的安全性和健壮性。</para>
        /// </remarks>
        public static String SkyCombine(string sSky1, string sSky2)
        {
            String[] S = { sSky1, sSky2 };
            int[] r = { -1, -1 };

            var j = 0;
            foreach (string s in S)
            {
                var i = Array.IndexOf(LocClass.LocNames, s);
                if (i > -1)
                {
                    r[j] = i;
                    j++;
                }
            }

            // ★ 核心重构：将提取出的索引进行升序排序，使算法支持无序传入
            Array.Sort(r);

            int[] it = { 0, 5 }; // 甲己合化土（排序后必然是 0 在前，5 在后）
            if (it[0] == r[0] && it[1] == r[1]) return "土";

            it[0] = 1;
            it[1] = 6; // 乙庚合化金
            if (it[0] == r[0] && it[1] == r[1]) return "金";

            it[0] = 2;
            it[1] = 7; // 丙辛合化水
            if (it[0] == r[0] && it[1] == r[1]) return "水";

            it[0] = 3;
            it[1] = 8; // 丁壬合化木
            if (it[0] == r[0] && it[1] == r[1]) return "木";

            it[0] = 4;
            it[1] = 9; // 戊癸合化火
            if (it[0] == r[0] && it[1] == r[1]) return "火";

            return "";
        }

        /// <summary>
        /// 获得方位五行
        /// </summary>
        /// <param name="DirectionName">方位名称（东、南、西、北、中）</param>
        /// <returns></returns>
        public static FiveAttr GetDirectionFiveAttr(string DirectionName)
        {
            FiveAttr fa = null!;
            if (DirectionName.Equals("东"))
            {
                fa = new("木");
            }
            else if (DirectionName.Equals("南"))
            {
                fa = new("火");
            }
            else if (DirectionName.Equals("西"))
            {
                fa = new("金");
            }
            else if (DirectionName.Equals("北"))
            {
                fa = new("水");
            }
            else if (DirectionName.Equals("中"))
            {
                fa = new("土");
            }
            return fa;
        }





        /// <summary>
        /// 转成HTML
        /// </summary>
        /// <returns></returns>
        public string ToHTML()
        {
            string st = $"<span style='color:{this.ToColor().ToHex()}'>{this.Name}</span>";
            return st;
        }



        #region 显式实现对比、运算符和Key 方法
        // 1. 一般的 Equals(object)，內部可以轉型並利用顯式介面來比對
        public override bool Equals(object obj)
        {
            if (obj is FiveAttr other)
            {
                return ((IEquatable<FiveAttr>)this).Equals(other);
            }
            return false;
        }

        // 2. 顯式實作 IEquatable<LocClass>.Equals
        bool IEquatable<FiveAttr>.Equals(FiveAttr other)
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
        public static bool operator ==(FiveAttr left, FiveAttr right)
        {
            if (left is null) return right is null;
            return ((IEquatable<FiveAttr>)left).Equals(right);
        }

        public static bool operator !=(FiveAttr left, FiveAttr right)
        {
            return !(left == right);
        }


        #endregion

    }
}
