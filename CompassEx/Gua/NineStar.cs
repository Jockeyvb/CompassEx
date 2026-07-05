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

namespace CompassEx.Gua
{


    /// <summary>
    /// 九星类型
    /// </summary>
    public enum NineStarType
    {
        /// <summary>
        /// 
        /// </summary>
        AfterGuaNineStar = 0
    }

    public class NineStar
    {



        /// <summary>
        /// 後天八卦九星（北斗九星）的完整中文名稱。
        /// </summary>
        /// <value>
        /// 包含 9 個元素的字符串數組，順序嚴格對應北斗九星：貪狼、巨門、祿存、文曲、廉貞、武曲、破軍、左輔、右弼。
        /// </value>
        /// <remarks>
        /// <para>本數組用於需要完整顯示星曜名稱的場景（如風水報告、羅盤盤面顯示）。</para>             
        /// </remarks>
        /// <seealso cref="NineStarNames"/>
        public readonly string[] NineStarFullNames = ["貪狼", "巨門", "祿存", "文曲", "廉貞", "武曲", "破軍", "左輔", "右弼"];

        /// <summary>
        /// 後天八卦九星（北斗九星）的單字簡稱。
        /// </summary>
        /// <value>
        /// 包含 9 個元素的字符串數組，順序與 <see cref="NineStarFullNames"/> 完全一致：貪、巨、祿、文、廉、武、破、輔、弼。
        /// </value>
        /// <remarks>
        /// 主要用於排版空間受限的場景，例如簡化版圖表、緊湊型排盤界面或終端日誌輸出。
        /// </remarks>
        /// <seealso cref="NineStarFullNames"/>
        public readonly string[] NineStarNames = ["貪", "巨", "祿", "文", "廉", "武", "破", "輔", "弼"];

        /// <summary>
        /// 翻卦九星完整名称序列（实质是八星）,默认是山法序列（水法不一样）
        /// </summary>
        public readonly string[] EightStarsFullName = ["辅弼", "贪狼", "巨门", "禄存", "文曲", "廉贞", "武曲", "破军"];
        /// <summary>
        /// 翻卦九星名称序列（实质是八星）,默认是山法序列（水法不一样）
        /// </summary>
        public readonly string[] EightStarsName = ["辅", "贪", "巨", "禄", "文", "廉", "武", "破"];
        /// <summary>
        /// 对应的翻卦八宅游年名称,默认是山法序列（水法不一样）
        /// </summary>
        public readonly string[] EightMansionsStarsName = ["伏位", "生气", "天医", "祸害", "六煞", "五鬼", "延年", "绝命"];

        /// <summary>
        /// 
        /// </summary>
        public int Index { set; private get; }

        /// <summary>
        /// 後天八卦九星（北斗九星）的简称。
        /// </summary>
        public string? NineStarName { set; private get; }
        /// <summary>
        /// 後天八卦九星（北斗九星）的完整中文名稱。
        /// </summary>
        public string? NineStarFullName { set; private get; }



    }
}
