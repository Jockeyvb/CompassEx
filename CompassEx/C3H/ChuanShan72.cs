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

namespace CompassEx.C3H;

/// <summary>
/// 穿山七十二龙
/// </summary>
public class ChuanShan72
{

    /// <summary>
    /// 单龙为5度
    /// </summary>
    public const double SingleDegree = 5;


    public const double StartDegree = 347.5; //甲子龙

    /// <summary>
    /// 穿山60甲子排序保存
    /// </summary>
    private static readonly List<string> ChuanShan72Names = GetChuanShan72Names();

    private static List<string> GetChuanShan72Names()
    {
        List<string> ls = new();

        foreach (var slName in LocClass.LocNames)
        {
            ls = ls.Concat(SkyLoc.SkyLoc60Name.Where(x => x.IndexOf(slName) > -1)).ToList();
            ls.Add("");//每5龙中增加一个空龙

        }
        return ls;
    }

    public int Index { get; init; }

    /// <summary>
    /// 穿山七十二龙名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 穿山七十二龙角度，实例是中针二十四山的所在角度
    /// </summary>
    [JsonIgnore]
    public CompassRangEX CRangeDegree
    {
        get
        {

            double dStart = StartDegree + Index * SingleDegree;
            if (dStart > 360) dStart = dStart % 360;
            CompassRangEX cr = new CompassRangEX(dStart, dStart + SingleDegree);
            return cr;

        }
    }

    public ChuanShan72(string name) : this(ChuanShan72Names.IndexOf(name))
    {
        if (string.IsNullOrWhiteSpace(name)) throw new Exception("穿山72龙名称不能为空！");
    }

    [JsonConstructor]
    public ChuanShan72([JsonProperty(nameof(Index))] int index)
    {
        if (index < 0 || index >= ChuanShan72Names.Count) throw new ArgumentOutOfRangeException(nameof(index));


        this.Index = index;
        this.Name = ChuanShan72Names[index];


    }

    /// <summary>
    /// 获得所有穿山七十二龙角度字典
    /// </summary>
    /// <returns></returns>
    public static Dictionary<CompassRangEX, ChuanShan72> GetAllChuanShan72s()
    {
        return ChuanShan72Names.Select((x, index) => new ChuanShan72(index)).ToDictionary(k => k.CRangeDegree, v => v);
    }

}
