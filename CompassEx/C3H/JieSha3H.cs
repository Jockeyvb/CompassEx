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
/// 劫煞取用
/// </summary>
public class JieSha3H
{


    //        public static readonly String[] C24HillNames = { "壬", "子", "癸", "丑", "艮", "寅", "甲", "卯", "乙", "辰", "巽", "巳", "丙", "午", "丁", "未", "坤", "申", "庚", "酉", "辛", "戌", "乾", "亥" };

    private static readonly string[] Names = ["申", "巳", "巳", "艮", "丁", "未", "丙", "午", "甲", "未", "癸", "酉", "辛", "酉", "寅", "癸", "乙", "癸", "午", "寅", "丑", "丑", "卯", "乙"];

    /// <summary>
    /// 劫煞取用名称
    /// </summary>
    //  public static readonly string[] JieSha3HNames = ["乾", "艮", "癸", "甲", "巽", "丙", "乙", "坤", "庚", "丁", "辛", "壬"];


    public int Index { get; init; }

    /// <summary>
    /// 劫煞取用名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 劫煞取用角度，实例是中针二十四山的所在角度
    /// </summary>
    [JsonIgnore] public CompassRangEX CRangeDegree { get => this.Chill.CRangeDegree; }




    /// <summary>
    /// 对应的正针二十四山 
    /// </summary>
    CHill Chill { get; init; }

    public JieSha3H(CHill c)
    {
        if (c == null) throw new ArgumentNullException(nameof(c));
        if (c.hillType != HillType.CHill) throw new ArgumentException("劫煞取用实例必须是【地盘正针二十四山】");

        int iPos = CHill.C24HillNames.IndexOf(c.Name);
        if (iPos == -1) throw new ArgumentException("输入的二十四山中没有劫煞取用");

        this.Index = iPos;
        this.Name = Names[iPos];
        this.Chill = c;

    }

    /// <summary>
    /// 获得所有劫煞取用角度字典
    /// </summary>
    /// <returns></returns>
    public static Dictionary<CompassRangEX, JieSha3H> GetAllJieSha3Hs()
    {
        return CHill.C24HillNames.Select(x => new JieSha3H(new CHill(x))).ToDictionary(x => x.CRangeDegree, x => x);
    }

}
