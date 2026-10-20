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
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using tyme.culture.star.twentyeight;
using tyme.solar;

namespace CompassEx.C3H;

/// <summary>
/// 罗盘中的二十四节气类型
/// </summary>
public enum CTermType
{
    None = 0,
    [Description("太阳到山盘（理气/消长）（逆时针）")]

    Sun = 1,//太阳到山盘（理气/消长）(逆时针）

    [Description("太阴到山盘（天度直量）（顺时针)")]
    Moon = 2 //太阴到山盘/天度直量（顺时针)
}


/// <summary>
/// 二十节气类，包括：太阳到山盘（理气/消长）和 太阴到山盘/天度直量
/// </summary>
public class Term24
{


    /// <summary>
    /// 单个元素范围
    /// </summary>
    private const double SingleDegree = 360 / 24;


    private readonly Dictionary<CTermType, double> StartDegree = new() { { CTermType.Sun, 30 }, { CTermType.Moon, 0 } }; //冬至

    //合用tyme 历库，索引： 冬至、小寒、大寒、立春、雨水、惊蛰、春分、清明、谷雨、立夏、小满、芒种、夏至、小暑、大暑、立秋、处暑、白露、秋分、寒露、霜降、立冬、小雪、大雪



    /// <summary>
    /// 罗盘中的二十四节气类型
    /// </summary>
    public CTermType TermType { get; init; }

    /// <summary>
    /// 所在的索引
    /// </summary>
    public int Index { get; init; }

    /// <summary>
    /// 二十四节气名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 二十四节气角度，实例是中针二十四山的所在角度
    /// </summary>
    [JsonIgnore]
    public CompassRangEX CRangeDegree
    {
        get
        {

            double dStart = StartDegree[this.TermType] + Index * SingleDegree;
            if (dStart >= 360) dStart = dStart % 360;
            double dEnd = dStart + SingleDegree;


            CompassRangEX cr = new CompassRangEX(dStart, dEnd);
            return cr;

        }
    }

    public Term24(string name, CTermType termType) : this(SolarTerm.Names.IndexOf(name), termType)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new Exception("二十四节气名称不能为空！");
    }

    [JsonConstructor]
    public Term24([JsonProperty(nameof(Index))] int index, CTermType termType)
    {
        if (index < 0 || index >= SolarTerm.Names.Length) throw new ArgumentOutOfRangeException(nameof(index));
        if (termType == 0) throw new ArgumentException("termType不能为none");

        this.Index = termType == CTermType.Moon ? index : (SolarTerm.Names.Length - index) % 24;
        this.Name = SolarTerm.Names[index];
        this.TermType = termType;

    }

    /// <summary>
    /// 获得所有二十四节气角度字典
    /// </summary>
    /// <returns></returns>
    public static Dictionary<CompassRangEX, Term24> GetAllTerm24s(CTermType termType)
    {
        return SolarTerm.Names.Select((x, index) => new Term24(index, termType)).ToDictionary(k => k.CRangeDegree, v => v);
    }

}
