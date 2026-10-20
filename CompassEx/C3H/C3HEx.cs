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

using CompassEx.C3H;
using CompassEx.Comm;
using System.Collections.Generic;

namespace CompassEx
{

    /// <summary>
    /// 三合罗盘类。
    /// </summary>
    /// <remarks>
    /// 本类继承自 <see cref="CompassEx"/>，主要用于处理三合罗盘体系（地盘正针、人盘中针、天盘缝针等三盘派系）的度数映射与罗盘层级计算。
    /// </remarks>
    public class C3HEx : CompassEx
    {

        /// <summary>
        /// 天干反复黄角度字典缓存
        /// </summary>
        public static readonly IReadOnlyDictionary<CompassRangEX, RepeatedlyYellowWater> RepeatedlyYellowWaters = RepeatedlyYellowWater.GetAllRepeatedlyYellowWaters();

        /// <summary>
        /// 八煞黄泉角度字典缓存
        /// </summary>
        public static readonly IReadOnlyDictionary<CompassRangEX, EightShaYellowWater> EightShaYellowWaters = EightShaYellowWater.GetAllEightShaYellowWaters();

        /// <summary>
        /// 八路黄泉（白虎煞）
        /// </summary>
        public static readonly IReadOnlyDictionary<CompassRangEX, EightRouterYellowWater> EightRouterYellowWaters = EightRouterYellowWater.GetAllEightRouterYellowWaters();

        /// <summary>
        ///  二十四节气（太阳到山盘）
        /// </summary>
        public static readonly IReadOnlyDictionary<CompassRangEX, Term24> SunTerm24s = Term24.GetAllTerm24s(CTermType.Sun);

        /// <summary>
        /// 二十四节气（太阴到山盘）
        /// </summary>
        public static readonly IReadOnlyDictionary<CompassRangEX, Term24> MoonTerm24s = Term24.GetAllTerm24s(CTermType.Moon);


        /// <summary>
        /// 九星四垣角度字典缓存
        /// </summary>
        public static readonly IReadOnlyDictionary<CompassRangEX, NineStarFourWall> NineStarFourWalls = NineStarFourWall.GetAllNineStarFourWalls();
        /// <summary>
        /// 劫煞取用角度字典缓存
        /// </summary>
        public static readonly IReadOnlyDictionary<CompassRangEX, JieSha3H> JieSha3Hs = JieSha3H.GetAllJieSha3Hs();






        /// <summary>
        /// 穿山七十二龙角度字典缓存
        /// </summary>
        public static readonly IReadOnlyDictionary<CompassRangEX, ChuanShan72> ChuanShan72s = ChuanShan72.GetAllChuanShan72s();

        /// <summary>
        /// 平分透地60龙角度字典缓存
        /// </summary>
        public static readonly IReadOnlyDictionary<CompassRangEX, AverageTouDi60> AverageTouDi60s = AverageTouDi60.GetAllAverageTouDi60s();

        /// <summary>
        /// 平分透地60龙纳音角度字典缓存
        /// </summary>
        public static readonly IReadOnlyDictionary<CompassRangEX, SkyLocNaYin> AverageTouDi60SkyLocNaYins = AverageTouDi60.GetAllAverageTouDi60SkyLocNaYins();



        /// <summary>
        /// 管山禽持世宿
        /// </summary>
        public static readonly IReadOnlyDictionary<CompassRangEX, SkyLocStar28> SkyLocStar28s = SkyLocStar28.GetAllSkyLocStar28s();


        /// <summary>
        /// 地盘120分金
        /// </summary>
        public static readonly IReadOnlyDictionary<CompassRangEX, FenJin120> CFenJin120s = FenJin120.GetAllFenJin120s(HillType.CHill);


        /// <summary>
        /// 人盘120分金
        /// </summary>
        public static readonly IReadOnlyDictionary<CompassRangEX, FenJin120> RFenJin120s = FenJin120.GetAllFenJin120s(HillType.RHill);



        /// <summary>
        /// 天盘120分金
        /// </summary>
        public static readonly IReadOnlyDictionary<CompassRangEX, FenJin120> SFenJin120s = FenJin120.GetAllFenJin120s(HillType.SHill);




        /// <summary>
        /// 二十四天星(地盘）
        /// </summary>
        public static readonly IReadOnlyDictionary<CompassRangEX, SkyStar24> SkyStar24s = SkyStar24.GetAllSkyStar24s(HillType.CHill);


        /// <summary>
        /// 二十四天星(人盘）
        /// </summary>
        public static readonly IReadOnlyDictionary<CompassRangEX, SkyStar24> RSkyStar24s = SkyStar24.GetAllSkyStar24s(HillType.RHill);








        /// <summary>
        /// 初始化 <see cref="C3HEx"/> 类的新实例。
        /// </summary>
        /// <param name="Degreen">当前罗盘的初始度数（角度值）。</param>
        /// <remarks>
        /// 构造函数通过调用基类 <see cref="CompassEx"/> 的构造函数来完成基本度数的初始化。
        /// </remarks>
        public C3HEx(double Degreen) : base(Degreen)
        {
        }
    }
}
