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
using CompassEx.C3H;
using CompassEx.C3Y;
using CompassEx.Gua;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace CompassEx.Comm;


public interface ICompassObjsRegistry
{
    CompassObjType ObjType { get; }
    string CNName { get; }
    // 统一的查询接口，返回标准的结构体列表
    IEnumerable<CompassObjStru> Query(double start, double end);
    IEnumerable<CompassObjStru> GetAll();  // ← 新增
}


public class CompassObjsProvider<T> : ICompassObjsRegistry where T : class
{


    public CompassObjType ObjType { get; set; }
    public string CNName { get => ObjType.GetEnumDescription(); }
    public IReadOnlyDictionary<CompassRangEX, T> Dictionary { get; set; }

    public IEnumerable<CompassObjStru> GetAll()
    {
        foreach (var kv in Dictionary)
        {
            string? sn = kv.Value.GetType().GetProperty("Name")?.GetValue(kv.Value).ToString();
            yield return new CompassObjStru
            {
                ObjTypeCNName = CNName,
                CRDegree = kv.Key,
                ObjType = typeof(T),
                Obj = kv.Value,
                Name = sn ?? string.Empty,
                CObjType = ObjType
            };
        }
    }

    public IEnumerable<CompassObjStru> Query(double start, double end)
    {
        foreach (var kv in Dictionary)
        {
            if (kv.Key.IsInRange(start) || kv.Key.IsInRange(end - 0.01))
            {

                string? sn = kv.Value.GetType().GetProperty("Name")?.GetValue(kv.Value).ToString();

                // 通过统一的逻辑组装结构体
                yield return new CompassObjStru
                {
                    ObjTypeCNName = CNName,
                    CRDegree = kv.Key,
                    ObjType = typeof(T),
                    Obj = kv.Value,
                    Name = sn ?? string.Empty, // 假设对象都有 Name 属性
                    CObjType = ObjType
                };
            }
        }
    }
}


public static class CompassObjsManager
{
    // 统一注册所有的图层


    private static readonly List<ICompassObjsRegistry> _layers =

        new()
    {




        new CompassObjsProvider<GuaSubClass> { ObjType = CompassObjType.BeforeGuaSub, Dictionary = CompassEx.BeforeGuaSubs },
        new CompassObjsProvider<GuaSubClass> { ObjType = CompassObjType.AfterGuaSub, Dictionary = CompassEx.AfterGuaSubs },
         new CompassObjsProvider<CHill>  { ObjType = CompassObjType.C3YCHill , Dictionary = CompassEx.C3YCHills },

        new CompassObjsProvider<CHill>  { ObjType = CompassObjType.CHill , Dictionary = CompassEx.CHills },
        new CompassObjsProvider<CHill>  { ObjType = CompassObjType.RHill , Dictionary = CompassEx.RCHills },
        new CompassObjsProvider<CHill>  { ObjType = CompassObjType.SHill , Dictionary = CompassEx.SCHills },
        new CompassObjsProvider<GuaClass>  { ObjType = CompassObjType.BeforeGua , Dictionary = C3YEx.CBeforeGuas },
        new CompassObjsProvider<GuaClass>  { ObjType = CompassObjType.AfterGua  , Dictionary = C3YEx.CAfterGuas },
        new CompassObjsProvider<GuaClass>  { ObjType = CompassObjType.BeforeGuaQi  , Dictionary = C3YEx.CBeforeGuas },
         new CompassObjsProvider<GuaClass>  { ObjType = CompassObjType.BeforeGuaFate   , Dictionary = C3YEx.CBeforeGuas },

           new CompassObjsProvider<GuaClass>  { ObjType = CompassObjType.AfterGuaQi    , Dictionary = C3YEx.CAfterGuas },
           new CompassObjsProvider<GuaClass>  { ObjType = CompassObjType.AfterGuaFate     , Dictionary = C3YEx.CAfterGuas },
              new CompassObjsProvider<RepeatedlyYellowWater>  { ObjType = CompassObjType.RepeatedlyYellowWater      , Dictionary = C3HEx.RepeatedlyYellowWaters },

              new CompassObjsProvider<EightShaYellowWater>  { ObjType = CompassObjType.EightShaYellowWater       , Dictionary = C3HEx.EightShaYellowWaters },

                   new CompassObjsProvider<NineStarFourWall>  { ObjType = CompassObjType.NineStarFourWall       , Dictionary = C3HEx.NineStarFourWalls },
               new CompassObjsProvider<JieSha3H>  { ObjType = CompassObjType.JieSha3H       , Dictionary = C3HEx.JieSha3Hs },
                 new CompassObjsProvider<ChuanShan72>  { ObjType = CompassObjType.ChuanShan72       , Dictionary = C3HEx.ChuanShan72s },
                    new CompassObjsProvider<AverageTouDi60>  { ObjType = CompassObjType.AverageTouDi60       , Dictionary = C3HEx.AverageTouDi60s },

                       new CompassObjsProvider<EightRouterYellowWater>  { ObjType = CompassObjType.EightRouterYellowWater       , Dictionary = C3HEx.EightRouterYellowWaters },
                        new CompassObjsProvider<Term24>  { ObjType = CompassObjType.SunTerm24       , Dictionary = C3HEx.SunTerm24s },
                          new CompassObjsProvider<Term24>  { ObjType = CompassObjType.MoonTerm24       , Dictionary = C3HEx.MoonTerm24s },


                 new CompassObjsProvider<SkyLocStar28>  { ObjType = CompassObjType.SkyLocStar28        , Dictionary = C3HEx.SkyLocStar28s },
                 new CompassObjsProvider<SkyLocNaYin>  { ObjType = CompassObjType.AverageTouDi60SkyLocNaYin        , Dictionary = C3HEx.AverageTouDi60SkyLocNaYins },

                  new CompassObjsProvider<FenJin120>  { ObjType = CompassObjType.CFenJin120        , Dictionary = C3HEx.CFenJin120s },
                 new CompassObjsProvider<FenJin120>  { ObjType = CompassObjType.RFenJin120        , Dictionary = C3HEx.RFenJin120s },
                 new CompassObjsProvider<FenJin120>  { ObjType = CompassObjType.SFenJin120        , Dictionary = C3HEx.SFenJin120s },
                 new CompassObjsProvider<SkyStar24>  { ObjType = CompassObjType.SkyStar24        , Dictionary = C3HEx.SkyStar24s },

                  new CompassObjsProvider<SkyStar24>  { ObjType = CompassObjType.RSkyStar24        , Dictionary = C3HEx.RSkyStar24s },




                   new CompassObjsProvider<CGuaYao>  { ObjType = CompassObjType.CGuaBeforeYao        , Dictionary = C3YEx.CGuaBeforeYaos },
                  new CompassObjsProvider<CGuaYao>  { ObjType = CompassObjType.CGuaAfterYao        , Dictionary = C3YEx.CGuaAfterYaos },

                   new CompassObjsProvider<CGuaYao>  { ObjType = CompassObjType.CGuaBeforeYaoSymbol   , Dictionary = C3YEx.CGuaBeforeYaos },

                    new CompassObjsProvider<CGuaYao>  { ObjType = CompassObjType.CGuaAfterYaoSymbol  , Dictionary = C3YEx.CGuaAfterYaos },

                     new CompassObjsProvider<CGuaYao>  { ObjType = CompassObjType.CGuaBeforeYaoSixRelative   , Dictionary = C3YEx.CGuaBeforeYaos },

                     new CompassObjsProvider<CGuaYao>  { ObjType = CompassObjType.CGuaAfterYaoSixRelative  , Dictionary = C3YEx.CGuaAfterYaos },



    };


    public static List<CompassObjStru> GetCompassObjByType(CompassObjType ot)

    {
        var ls = new List<CompassObjStru>();

        foreach (var layer in _layers)
        {
            // 如果请求的类型匹配（通过位掩码判断）
            if (ot.HasFlag(layer.ObjType))
            {
                ls.AddRange(layer.GetAll());
            }
        }

        return ls;
    }

    // 终极优雅的聚合查询方法（再也不用写一堆 if 了！）
    public static List<CompassObjStru> GetCompassObjByDegree(double start, double end, CompassObjType ot = CompassObjType.All)
    {
        var ls = new List<CompassObjStru>();

        foreach (var layer in _layers)
        {
            // 如果请求的类型匹配（通过位掩码判断）
            if (ot.HasFlag(layer.ObjType))
            {
                ls.AddRange(layer.Query(start, end));
            }
        }

        return ls;
    }
}
