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

using CommLib;
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


    private SixRelativeClass? _SixRelative;
    /// <summary>
    /// 六亲的
    /// </summary>
    [JsonIgnore]
    public SixRelativeClass? SixRelative
    {
        get
        {
            var sr = this.CGua.Yaos[this.Index].SixRelative;

            if (_SixRelative == null)
            {
                if (sr == null) this.CGua.LoadAllYaos();

                _SixRelative = this.CGua.Yaos[this.Index].SixRelative;
            }
            return _SixRelative;
        }
    }


    /// <summary>
    /// 角度偏移量（双爻动（抽双爻）为吉时不为0）
    /// </summary>
    public float GoodSymbolAngleOffset
    {
        get
        {

            float fo = 0;
            if (this.IsBothDoingGood == false) return 0; //不是双爻动不需要角度偏移
            if (this.CGua.CGuaYaos.Any(x => x.IsDoingGood)) return 0;//如果本卦中有卦爻是单爻动则不需要角度偏移

            fo = this.IsStartUp ? (float)SingleDegree / 2 : -(float)(SingleDegree / 2);
            //上爻开始（阴卦）则偏移为正，阳卦开始的则为负数

            return fo;
        }

    }


    GuaFitType? _DoingGuaFitType;
    /// <summary>
    /// 单爻动变后的卦合类型
    /// </summary>
    public GuaFitType DoingGuaFitType
    {
        get
        {

            if (_DoingGuaFitType.HasValue == false) GetIsDoingGood();

            return _DoingGuaFitType.GetValueOrDefault();
        }

    }

    private FiveAttrRule? _DoingGuaFiveAttrRule;
    /// <summary>
    /// 单爻动时变出的卦五行关系
    /// </summary>
    public FiveAttrRule DoingGuaFiveAttrRule
    {
        get
        {

            if (_DoingGuaFiveAttrRule.HasValue == false) GetIsDoingGood();
            return _DoingGuaFiveAttrRule.GetValueOrDefault();
        }
    }



    private FiveAttrRule? _BothDoingGuaFiveAttrRule;
    /// <summary>
    /// 单爻动时变出的卦五行关系
    /// </summary>
    public FiveAttrRule BothDoingGuaFiveAttrRule
    {
        get
        {

            if (_BothDoingGuaFiveAttrRule.HasValue == false) GetIsDoingGood(true);
            return _BothDoingGuaFiveAttrRule.GetValueOrDefault();
        }
    }


    private GuaFitType? _BothDoingGuaFitType;

    /// <summary>
    /// 双爻动变后的卦合类型（则本爻累加1爻）
    /// </summary>
    public GuaFitType BothDoingGuaFitType
    {
        get
        {

            if (_BothDoingGuaFitType.HasValue == false) GetIsDoingGood(true);

            return _BothDoingGuaFitType.GetValueOrDefault();
        }
    }



    /// <summary>
    /// 单爻可用点的符号(双爻动吉可用则需要加入罗盘上的偏移值调整位置，应是阳卦角度应该加0.46875度，阴卦角度-0.46875 位置上，在中间)
    /// 有GoodSymbolAngleOffset角度偏移则是双爻动
    /// </summary>
    public string GoodSymbol
    {
        get => this.IsDoingGood || GoodSymbolAngleOffset != 0 ? "●" : "";

    }


    private bool? _IsDoingGood;
    /// <summary>
    /// 获得单爻动的吉凶可用
    /// </summary>
    public bool IsDoingGood
    {
        get
        {
            if (_IsDoingGood.HasValue == false) _IsDoingGood = GetIsDoingGood();
            return _IsDoingGood.GetValueOrDefault();
        }

    }



    private bool? _IsBothDoingGood;
    /// <summary>
    /// 获得双爻动的吉凶可用
    /// </summary>
    public bool IsBothDoingGood
    {
        get
        {
            if (_IsBothDoingGood.HasValue == false)
            {
                _IsBothDoingGood = this.Index == 5 ? false : GetIsDoingGood(true);
            }
            return _IsBothDoingGood.GetValueOrDefault();
        }

    }

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
    public bool IsStartUp { get => !this.CGua.IsSunRouter; }


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










    //========抽爻换象吉爻规则参考钟义明《玄空大卦秘诀破译》与刘贲的《玄空大卦透析》===========
    //抽爻换象中变出的卦以吉论的吉爻的条件其中之一：不合者以凶论（不可用）
    //1、上卦与下卦先天洛数（卦气）能合五、十、十五（且卦五行不能克）
    //2、上卦与下卦先天洛数（卦气）能合生成，即16、27、38、49。（且卦五行不能克）
    //3、上卦与下卦合雌雄正配亦吉（不考虑五行相克）
    //抽爻换象中变出的卦上下卦：如卦（非爻）五行相克，则应人伦发凶（不可用）：
    //1、受克者大凶
    //2、克人者亦凶
    //=======抽爻换象吉爻规则参考钟义明《玄空大卦秘诀破译》与刘贲的《玄空大卦透析》===========
    //经考究对比，市面上的罗盘有些是错误的，例如：有某某罗盘：中孚卦，上爻为为红点（则抽爻为吉），变出的卦为：水泽节，上卦坎卦气为7，下卦兑卦气为4，不相合，也不符合雌雄正配，虽五行相生，亦为不可用，学卦理者应该独立思考其合理性。
    //有些卦，应该考虑卦理之余，应考虑24山，八卦分界线（空亡），因为有些卦如：【艮】三爻抽爻虽然为吉度但近空亡亦需慎重考虑使用,。


    /// <summary>
    /// 获得抽爻换象后是否为吉爻
    /// </summary>
    /// <param name="IsBoth">是否获得双爻动的吉凶</param>
    /// <returns></returns>
    public bool GetIsDoingGood(bool IsBoth = false)
    {


        if (IsBoth && this.Index == 5) return false;

        GuaClass G = this.CGua.DeepJSONClone(); //不要改变本卦使用复制卦
        if (!IsBoth || this.Index == 5)//如果是上爻只检测单爻
        {

            G.SetYaoDoing([this.Index], false);//设置单爻动

        }
        else
        {
            G.SetYaoDoing([this.Index, this.Index + 1], false);//设置双爻动（只能是相邻的爻动）
        }

        GuaClass CG = G.GetChangeGua();//获得变卦

        var far = CG.GuaQiFit;//本卦的上、下卦，卦气关系
        var gr = CG.UpWithDownGuaRule;
        if (IsBoth)
        {
            this._BothDoingGuaFitType = far;
            this._BothDoingGuaFiveAttrRule = gr;
        }
        else
        {
            this._DoingGuaFitType = far;
            this._DoingGuaFiveAttrRule = gr;
        }




        if (far == GuaFitType.None) return false;//不合卦理则为凶

        if (far.HasFlag(GuaFitType.CiXiong)) return true;//若卦理合雌雄则不需要判断五行相克


        if (gr != FiveAttrRule.CanMe && gr != FiveAttrRule.MeCan) return true;//不是我克或克我则为吉

        return false;



    }


}
