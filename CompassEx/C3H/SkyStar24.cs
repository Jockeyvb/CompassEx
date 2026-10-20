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
/// 二十四天星(包括地盘、人盘、天盘）
/// 目前发现有多种二十四天星的类型，为了兼容将兼容地盘、人盘、天盘度数生成的二十四天星
///  三元：根据地盘二十四山的二十四天星（亦有三合使用地盘）
///  三合：根据《罗经透解》古本 第158页，使用人盘二十四度数生成的二十四天星。
///  由于流传多个不同名称版本，作者以罗经透解》古本 第157页为准
/// </summary>
public class SkyStar24
{




    public static readonly string[] Names = new string[24] { "天辅", "天垒", "阴光", "天厨", "天市", "天培", "阴玑", "天命", "天官", "天罡", "太乙", "天屏", "太微", "阳权", "南极", "天常", "天铁", "天关", "天汉", "少微", "天乙", "天魁", "天厩", "天皇" };

    //在《罗经透解》古本中157面，...阴光癸(与市面通常：天道），...阴玑甲（流通：天苑），...阳权午（流通：天马），...南极丁（流通：天柱）,...天汉庚（流通：天潢、横）...天铁坤（流行：天钺）




    public int Index { get; init; }

    /// <summary>
    /// 二十四天星名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 二十四天星角度，实例是中针二十四山的所在角度
    /// </summary>
    [JsonIgnore] public CompassRangEX CRangeDegree { get => this.Chill.CRangeDegree; }




    /// <summary>
    /// 对应的正针二十四山 
    /// </summary>
    CHill Chill { get; init; }

    public SkyStar24(CHill c)
    {




        this.Index = c.Index;
        this.Name = Names[Index];
        this.Chill = c;

    }




    /// <summary>
    /// 获得所有二十四天星角度字典(包括地盘、人盘、天盘）
    /// </summary>
    /// <returns></returns>
    public static Dictionary<CompassRangEX, SkyStar24> GetAllSkyStar24s(HillType ht)
    {
        return Names.Select((x, index) => new SkyStar24(new CHill(index, ht))).ToDictionary(x => x.CRangeDegree, x => x);
    }

}
