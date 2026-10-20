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
/// 平分透地六十龙
/// </summary>
public class AverageTouDi60
{

    /// <summary>
    /// 单龙为5度
    /// </summary>
    public const double SingleDegree = 6;


    public const double StartDegree = 337.5; //甲子龙




    /// <summary>
    /// 穿山60甲子排序保存
    /// </summary>
    private static readonly List<string> TouDi60Names = SkyLoc.SkyLoc60.OrderBy(x => x.Loc.Index).Select(x => x.Name).ToList();




    public int Index { get; init; }

    /// <summary>
    /// 平分透地六十龙名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 平分透地六十龙角度，实例是中针二十四山的所在角度
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

    /// <summary>
    /// 纳音
    /// </summary>
    [JsonIgnore] public SkyLocNaYin NaYin { get { return this.SkyLoc.NaYin; } }

    /// <summary>
    /// 干支
    /// </summary>
    [JsonIgnore] public SkyLoc SkyLoc { get => new SkyLoc(this.Name); }

    public AverageTouDi60(string name) : this(TouDi60Names.IndexOf(name))
    {
        if (string.IsNullOrWhiteSpace(name)) throw new Exception("平分透地六十龙名称不能为空！");
    }

    [JsonConstructor]
    public AverageTouDi60([JsonProperty(nameof(Index))] int index)
    {
        if (index < 0 || index >= TouDi60Names.Count) throw new ArgumentOutOfRangeException(nameof(index));


        this.Index = index;
        this.Name = TouDi60Names[index];



    }

    /// <summary>
    /// 获得所有平分透地六十龙的纳音角度字典
    /// </summary>
    /// <returns></returns>
    public static Dictionary<CompassRangEX, SkyLocNaYin> GetAllAverageTouDi60SkyLocNaYins()
    {
        return C3HEx.AverageTouDi60s.ToDictionary(r => r.Key, a => a.Value.NaYin);
    }

    /// <summary>
    /// 获得所有平分透地六十龙角度字典
    /// </summary>
    /// <returns></returns>
    public static Dictionary<CompassRangEX, AverageTouDi60> GetAllAverageTouDi60s()
    {
        return SkyLoc.SkyLoc60Name.Select((x, index) => new AverageTouDi60(index)).ToDictionary(k => k.CRangeDegree, v => v);
    }

}
