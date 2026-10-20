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
/// 一百二十分金（包括地盘、人盘、天盘24山）
/// </summary>
public class FenJin120
{

    /// <summary>
    /// 为3度范围
    /// </summary>
    private const double SingleDegree = 3;


    private static readonly Dictionary<HillType, double> StartDegreeDC = new() {

        { HillType.CHill , new CHill(0, HillType.CHill ).CRangeDegree.Start },
         { HillType.RHill  , new CHill(0, HillType.RHill ).CRangeDegree.Start },
         { HillType.SHill  , new CHill(0, HillType.SHill ).CRangeDegree.Start },


    };



    public HillType HillType { get; init; }


    /// <summary>
    /// 穿山60甲子排序保存
    /// </summary>
    private static readonly List<string> Names = Get120Names();


    private static List<string> Get120Names()
    {
        List<string> ls = new();

        for (var i = 0; i < CHill.C24HillNames.Length; i++)
        {
            string ln = CHill.C24HillNames[i];

            if (LocClass.LocNames.IndexOf(ln) == -1)//天干或四维
            {
                int k = i - 1 < 0 ? 23 : i - 1;
                ln = CHill.C24HillNames[k]; //天干或四维向前找地支的干支
            }


            var en = SkyLoc.SkyLoc60.Where(x => x.Loc.Name == ln).OrderBy(s => s.Sky.Index).Select(x => x.Name);//按天干排序
            ls.AddRange(en);
        }

        return ls;
    }

    private static readonly string[] GoodSkyNames = ["丙", "庚", "辛", "丁"];

    public int Index { get; init; }

    /// <summary>
    /// 平分透地六十龙名称
    /// </summary>
    public string Name { get; init; }


    public bool IsGood { get => GoodSkyNames.Any(x => x.Equals(this.Name.Substring(0, 1))); }

    /// <summary>
    /// 平分透地六十龙角度，实例是中针二十四山的所在角度
    /// </summary>
    public CompassRangEX CRangeDegree
    {
        get
        {

            double dStart = StartDegreeDC[this.HillType] + Index * SingleDegree;
            dStart = dStart % 360;
            CompassRangEX cr = new CompassRangEX(dStart, dStart + SingleDegree);
            return cr;

        }
    }

    /// <summary>
    /// 纳音
    /// </summary>
    [JsonIgnore]
    public SkyLocNaYin NaYin { get { return new SkyLocNaYin(this.SkyLoc); } }

    /// <summary>
    /// 干支
    /// </summary>
    [JsonIgnore]
    public SkyLoc SkyLoc { get => new SkyLoc(this.Name); }



    [JsonConstructor]
    public FenJin120([JsonProperty(nameof(Index))] int index, [JsonProperty(nameof(HillType))] HillType ht)
    {
        if (index < 0 || index >= Names.Count) throw new ArgumentOutOfRangeException(nameof(index));

        this.Index = index;
        this.Name = Names[index];
        this.HillType = ht;


    }


    /// <summary>
    /// 获得所有平分透地六十龙角度字典
    /// </summary>
    /// <returns></returns>
    public static Dictionary<CompassRangEX, FenJin120> GetAllFenJin120s(HillType ht)
    {
        return Names.Select((x, index) => new FenJin120(index, ht)).ToDictionary(k => k.CRangeDegree, v => v);
    }

}
