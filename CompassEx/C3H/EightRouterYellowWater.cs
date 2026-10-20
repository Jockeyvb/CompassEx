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
/// 八路黄泉又名白虎煞
/// </summary>
public class EightRouterYellowWater
{



    //================================12山(正针）对应相关的八路黄泉================================

    private static readonly string[] Names = ["亥", "未丁", "未丁", "辰", "午", "亥", "未丁", "申", "丑", "未丁", "子", "辰", "午", "亥", "辰", "申", "丑", "未丁", "申", "辰", "子", "亥", "未丁", "申"];
    //================================12山对应相关的八路黄泉================================



    public int Index { get; init; }

    /// <summary>
    /// 八路黄泉名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 八路黄泉角度，实例是中针二十四山的所在角度
    /// </summary>
    [JsonIgnore] public CompassRangEX CRangeDegree { get => this.Chill.CRangeDegree; }




    /// <summary>
    /// 对应的正针二十四山（最多可能有两个)
    /// </summary>
    CHill Chill { get; init; }

    public EightRouterYellowWater(CHill c)
    {
        if (c == null) throw new ArgumentNullException(nameof(c));
        if (c.hillType != HillType.CHill) throw new ArgumentException("八路黄泉实例必须是【地盘正针二十四山】");

        int iPos = c.Index;
        if (iPos == -1) throw new ArgumentException("输入的二十四山中没有八路黄泉");

        this.Index = iPos;
        this.Name = Names[iPos];
        this.Chill = c;

    }

    /// <summary>
    /// 获得所有八路黄泉角度字典
    /// </summary>
    /// <returns></returns>
    public static Dictionary<CompassRangEX, EightRouterYellowWater> GetAllEightRouterYellowWaters()
    {
        return CHill.C24HillNames.Select(x => new EightRouterYellowWater(new CHill(x))).ToDictionary(x => x.CRangeDegree, x => x);
    }

}
