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
using CompassEx.Gua;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompassEx.C3H;


/// <summary>
/// 八煞黄泉，又名坐山八煞
/// </summary>
public class EightShaYellowWater
{

    //================================后天八卦山对应相关的八煞黄泉================================
    private static readonly string[] AfterGuaSubNames = ["坎", "艮", "震", "巽", "离", "坤", "兑", "乾"];
    private static readonly string[] Names = ["辰", "寅", "申", "酉", "亥", "卯", "巳", "午"];
    //================================后天八卦山对应相关的八煞黄泉================================

    public string Name { get; init; }

    public int Index { get; init; }

    public GuaSubClass AfterGuaSub { get; init; }

    /// <summary>
    /// 返回实际对应的后天卦度数
    /// </summary>
    public CompassRangEX CRangeDegree { get => this.AfterGuaSub.CAfterRangeDegree; }
    public EightShaYellowWater(GuaSubClass AfterGuaSub)
    {
        int index = AfterGuaSubNames.IndexOf(AfterGuaSub.Name);
        if (index < 0 || index >= AfterGuaSubNames.Length) throw new ArgumentOutOfRangeException(nameof(AfterGuaSub));
        this.Index = index;
        this.Name = Names[index];
        this.AfterGuaSub = AfterGuaSub;
    }


    /// <summary>
    /// 获得所有八煞黄泉的度数字典
    /// </summary>
    /// <returns></returns>
    public static Dictionary<CompassRangEX, EightShaYellowWater> GetAllEightShaYellowWaters()
    {
        return AfterGuaSubNames.Select(x => new EightShaYellowWater(new GuaSubClass(x))).ToDictionary(k => k.CRangeDegree, v => v);
    }



}
