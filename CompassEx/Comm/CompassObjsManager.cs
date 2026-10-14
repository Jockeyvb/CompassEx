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

using CompassEx.Gua;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

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
    public string CNName { get; set; }
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
    private static readonly List<ICompassObjsRegistry> _layers = new()
    {
        new CompassObjsProvider<GuaSubClass> { ObjType = CompassObjType.BeforeGuaSub, CNName = "先天八卦", Dictionary = CompassEx.BeforeGuaSubs },
        new CompassObjsProvider<GuaSubClass> { ObjType = CompassObjType.AfterGuaSub, CNName = "后天八卦", Dictionary = CompassEx.AfterGuaSubs },
        new CompassObjsProvider<CHill>  { ObjType = CompassObjType.CHill, CNName = "地盘二十四山", Dictionary = CompassEx.CHills },
        new CompassObjsProvider<CHill>  { ObjType = CompassObjType.RHill, CNName = "人盘二十四山", Dictionary = CompassEx.RCHills },
        new CompassObjsProvider<CHill>  { ObjType = CompassObjType.SHill, CNName = "天盘二十四山", Dictionary = CompassEx.SCHills },
        new CompassObjsProvider<GuaClass>  { ObjType = CompassObjType.BeforeGua, CNName = "天盘六十四卦", Dictionary = C3YEx.CBeforeGuas },
        new CompassObjsProvider<GuaClass>  { ObjType = CompassObjType.AfterGua , CNName = "地盘六十四卦", Dictionary = C3YEx.CAfterGuas },

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
