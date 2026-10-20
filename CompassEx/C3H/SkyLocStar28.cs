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
using System.Runtime.CompilerServices;
using System.Text;
using tyme.culture.star.twentyeight;

namespace CompassEx.C3H;

/// <summary>
/// 管山禽持世宿(与平分60透地龙相关）
/// </summary>
public class SkyLocStar28
{



    /// <summary>
    ///  管山禽持世宿(28宿索引)
    /// </summary>

    public int Index { get; init; }

    /// <summary>
    ///  管山禽持世宿名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    ///  管山禽持世宿角度，实例是中针二十四山的所在角度
    /// </summary>
    [JsonIgnore]
    public CompassRangEX CRangeDegree
    {
        get
        {


            return AverageTouDi60.CRangeDegree;

        }
    }

    public AverageTouDi60 AverageTouDi60 { get; init; }


    [JsonConstructor]
    public SkyLocStar28([JsonProperty(nameof(AverageTouDi60))] AverageTouDi60 AverageTouDi60)
    {

        if (AverageTouDi60 == null) throw new ArgumentNullException(nameof(AverageTouDi60));

        this.AverageTouDi60 = AverageTouDi60;
        this.Index = AverageTouDi60.SkyLoc.Index % 28;
        this.Name = TwentyEightStar.Names[Index];


    }

    /// <summary>
    /// 获得所有 管山禽持世宿角度字典
    /// </summary>
    /// <returns></returns>
    public static Dictionary<CompassRangEX, SkyLocStar28> GetAllSkyLocStar28s()
    {
        return C3HEx.AverageTouDi60s.ToDictionary(k => k.Key, v => new SkyLocStar28(v.Value));



    }

}