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

namespace CompassEx.C3Y;




/// <summary>
/// 罗盘六十四卦卦爻类，包括：天盘六十四卦(先天卦序)和 地盘六十四卦(后天卦序)
/// </summary>
public class CGuaYao
{




    /// <summary>
    /// 单爻度数范围
    /// </summary>
    private const double SingleDegree = C3YEx.CompassGuaDegree / 6;


    /// <summary>
    /// 起始卦度数
    /// </summary>
    private double StartDegree
    {
        get
        {
            return this.CGua.CRangeDegree.Start;

        }
    }

    /// <summary>
    /// 起始度数是否由上爻开始
    /// </summary>
    public bool IsStartUp { get => this.CGua.CGuaIndex % 2 == 1; }


    /// <summary>
    /// 六爻卦
    /// </summary>
    public CGuaClass CGua { get; init; }

    /// <summary>
    /// 罗盘六十四卦卦爻所在索引0-5
    /// </summary>
    public int Index { get; init; }

    /// <summary>
    /// 罗盘六十四卦卦爻名称（初爻,二爻.....上爻)
    /// </summary>
    public string Name { get => GuaYao.SixYaoNames[this.Index]; }

    /// <summary>
    /// 缩写
    /// </summary>
    public string ShortName { get => this.Index == 0 ? "刀" : this.Name.Substring(0, 1); }

    /// <summary>
    /// 六十四卦角度，实例是中针二十四山的所在角度
    /// </summary>
    [JsonIgnore]
    public CompassRangEX CRangeDegree
    {
        get
        {
            int iPos = this.IsStartUp ? Index : 5 - Index;//如果有上爻开始则反转度数

            double dStart = StartDegree + iPos * SingleDegree;
            dStart = dStart % 360;
            double dEnd = dStart + SingleDegree;


            CompassRangEX cr = new CompassRangEX(dStart, dEnd);
            return cr;

        }
    }


    /// <summary>
    /// 罗盘卦爻构造函数
    /// </summary>
    /// <param name="cg">罗盘卦象类型</param>
    /// <param name="YaoIndex">要创建的卦爻索引0-5</param>
    /// <exception cref="ArgumentNullException"></exception>
    [JsonConstructor]
    public CGuaYao([JsonProperty(nameof(CGua))] CGuaClass cg, [JsonProperty(nameof(Index))] int YaoIndex)
    {
        if (cg == null) throw new ArgumentNullException(nameof(cg));

        this.CGua = cg;
        this.Index = YaoIndex;

    }

    /// <summary>
    /// 获得所有六十四卦角度字典(384爻)
    /// </summary>
    /// <returns></returns>
    public static Dictionary<CompassRangEX, CGuaYao> GetAllCGuaYaos(CGuaType gt)
    {
        if (gt == CGuaType.None) throw new ArgumentNullException(nameof(gt));

        List<CGuaClass> gs = GuaClass.FullNames.Select(x => new CGuaClass(x, gt)).ToList();
        List<CGuaYao> ys = new();
        foreach (CGuaClass c in gs)
        {
            for (int i = 0; i < 6; i++)
            {
                CGuaYao y = new CGuaYao(c, i);
                ys.Add(y);
            }

        }

        return ys.ToDictionary(x => x.CRangeDegree, y => y);

    }

}
