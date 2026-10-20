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
/// 天干反复黄泉类
/// 论向首与水法/道路（来水、去水、放水、开门）的禁忌。
/// </summary>
public class RepeatedlyYellowWater
{



    //================================12山(正针）对应相关的天干反复黄泉================================
    private static readonly string[] CHillNames = ["壬", "癸", "艮", "甲", "乙", "巽", "丙", "丁", "坤", "庚", "辛", "乾"];
    private static readonly string[] Names = ["乾", "艮", "癸甲", "艮", "巽", "丙乙", "巽", "坤", "庚丁", "坤", "乾", "辛壬"];
    //================================12山对应相关的天干反复黄泉================================
    /// <summary>
    /// 天干反复黄泉名称
    /// </summary>
    //  public static readonly string[] RepeatedlyYellowWaterNames = ["乾", "艮", "癸", "甲", "巽", "丙", "乙", "坤", "庚", "丁", "辛", "壬"];


    public int Index { get; init; }

    /// <summary>
    /// 天干反复黄泉名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 天干反复黄泉角度，实例是中针二十四山的所在角度
    /// </summary>
    [JsonIgnore] public CompassRangEX CRangeDegree { get => this.Chill.CRangeDegree; }




    /// <summary>
    /// 对应的正针二十四山（最多可能有两个)
    /// </summary>
    CHill Chill { get; init; }

    public RepeatedlyYellowWater(CHill c)
    {
        if (c == null) throw new ArgumentNullException(nameof(c));
        if (c.hillType != HillType.CHill) throw new ArgumentException("天干反复黄泉实例必须是【地盘正针二十四山】");

        int iPos = CHillNames.IndexOf(c.Name);
        if (iPos == -1) throw new ArgumentException("输入的二十四山中没有天干反复黄泉");

        this.Index = iPos;
        this.Name = Names[iPos];
        this.Chill = c;

    }

    /// <summary>
    /// 获得所有天干反复黄泉角度字典
    /// </summary>
    /// <returns></returns>
    public static Dictionary<CompassRangEX, RepeatedlyYellowWater> GetAllRepeatedlyYellowWaters()
    {
        return CHillNames.Select(x => new RepeatedlyYellowWater(new CHill(x))).ToDictionary(x => x.CRangeDegree, x => x);
    }

}
