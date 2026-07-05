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
using System;
using System.Collections.Generic;

namespace CompassEx.Gua
{

    /// <summary>
    /// 纳甲类型
    /// </summary>
    public enum NaJiaType
    {
        /// <summary>
        /// 京房纳甲(64卦、天机出卦法使用)
        /// </summary>
        JF = 0,
        /// <summary>
        /// 杨公纳甲(九星山法、辅星水法使用）
        /// </summary>
        YG = 1,
    }

    /// <summary>
    /// 京房纳甲结果类,包括：64卦，天干类，地支类
    /// 64卦类用于六爻预测学的纳甲推演（预测），天干类，地支类用于64卦装卦、排盘、起卦，天机出卦法也使用
    /// </summary>
    // 定义一个统一的纳甲结果接口（可选，用于类型约束）
    public interface INaJiaResult { NaJiaType Type { get; } }

    public struct NaJiaJFResult : INaJiaResult
    {
        /// <summary>
        /// 复卦（六爻卦）
        /// 可用于天机出卦法的纳甲判断人命庚之干支是否得福、官或出卦、入卦等。
        /// </summary>
        public GuaClass Gua;
        /// <summary>
        /// 京房纳甲类型
        /// </summary>
        public NaJiaType Type => NaJiaType.JF;
        /// <summary>
        /// 六爻对应的干支
        /// </summary>
        public List<SkyLoc> SkyLocs;
    }

    public struct NaJiaYGResult : INaJiaResult
    {
        /// <summary>
        /// 八卦（后天）
        /// 九星山法应该使用来龙卦，或坐卦卦
        /// 辅星水法应该使用向卦
        /// </summary>
        public GuaSubClass Gua;

        /// <summary>
        /// 杨公纳甲类型
        /// </summary>
        public NaJiaType Type => NaJiaType.YG;
        /// <summary>
        /// 所包含的
        /// </summary>
        public SkyLoc Sky;
        public List<LocClass> Locs;

    }

    // 将 NaJia 改为泛型类
    public class NaJia<TResult> where TResult : struct, INaJiaResult
    {
        /// <summary>
        /// 杨公纳甲翻卦序列
        /// </summary>
        private string[] NaJiaYGGuas = { "离", "巽", "坤", "兑",
                                         "乾", "艮", "坎", "震" };

        /// <summary>
        /// 杨公纳甲翻卦决字典
        /// </summary>

        private readonly Dictionary<string, int[]> NaJiaYGGuaDC = new Dictionary<string, int[]> {
            { "离", [0,7,3,6,2,5,1,4] } ,{"兑",[3,4,0,5,1,6,2,7] },{"乾",[4,3,7,2,6,1,5,0]},{"震",[7,0,4,1,5,2,6,3]}//边上边落双双起
            ,{"巽",[1,6,2,7,3,4,0,5] },{"坤",[2,5,1,4,0,7,3,6]},{"艮",[5,2,6,3,7,0,4,1] },{"坎",[6,1,5,0,4,3,7,2] } //中上中落双双起
        
        };




        public NaJiaType Type { get; private set; }

        // 内部存储计算所需的数据
        private object _GuaData;

        /// <summary>
        /// 京房纳甲
        /// </summary>
        /// <param name="g">复卦（六爻卦)</param>
        /// <exception cref="ArgumentNullException"></exception>
        private NaJia(GuaClass g)
        {
            if (g == null) throw new ArgumentNullException(nameof(g), "Gua data cannot be null.");


            Type = NaJiaType.JF;
            _GuaData = g;

        }

        /// <summary>
        /// 杨公纳甲构造函数，输入后天八卦的单卦对象，计算对应的纳甲结果。
        /// </summary>
        /// <param name="gs">后天八卦（三爻卦）</param>
        /// <exception cref="ArgumentNullException"></exception>
        private NaJia(GuaSubClass gs)
        {
            if (gs == null) throw new ArgumentNullException(nameof(gs), "Gua data cannot be null.");


            Type = NaJiaType.JF;
            _GuaData = gs;

        }



        // 统一的推演/计算方法，直接返回确定的泛型结果(总出口方法）
        public TResult? Execute()
        {
            if (Type == NaJiaType.JF)//京房纳甲
            {
                var R = new NaJiaJFResult();
                GuaClass g = (GuaClass)_GuaData;
                R.Gua = g;
                R.SkyLocs = g.SkyLocs; //使用原来复卦的干枝集合


                // 核心计算逻辑...
                return (TResult)(object)R;
            }
            if (Type == NaJiaType.YG) //杨公
            {
                var R = new NaJiaYGResult();



                // 核心计算逻辑...
                return (TResult)(object)R;
            }
            return null;
        }

        /// <summary>
        /// 京房纳甲的静态工厂方法，输入复卦对象，返回纳甲结果。
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static NaJiaJFResult? CreateJF(GuaClass g)
        {
            return (new NaJia<NaJiaJFResult>(g)).Execute();
        }
        /// <summary>
        /// 杨公纳甲的静态工厂方法，输入后天八卦的单卦对象，返回纳甲结果。
        /// </summary>
        /// <param name="gs"></param>
        /// <returns></returns>
        public static NaJiaYGResult? CreateYG(GuaSubClass gs)
        {
            return (new NaJia<NaJiaYGResult>(gs)).Execute();

        }
    }
}
