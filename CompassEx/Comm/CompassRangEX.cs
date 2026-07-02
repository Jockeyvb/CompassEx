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

using CompassEx.Guo;
using System.Collections.Generic;

namespace CompassEx.Comm
{

    /// <summary>
    /// 表示一个罗盘度数的角度范围。
    /// </summary>
    /// <remarks>
    /// 该类用于处理 $0^\circ$ 到 $360^\circ$ 之间的角度区间判定。
    /// 它支持正常的递增区间（例如 $10^\circ \sim 50^\circ$），
    /// 同时也原生支持跨越 $360^\circ$ 边界的循环区间（例如 $350^\circ \sim 20^\circ$）。
    /// </remarks>
    public class CompassRangEX
    {
        /// <summary>
        /// 获取或设置范围的起始角度（度）。
        /// </summary>
        /// <value>
        /// 区间的起始边界值，通常在 $[0, 360)$ 范围内。
        /// </value>
        public double Start { get; set; }

        /// <summary>
        /// 获取或设置范围的结束角度（度）。
        /// </summary>
        /// <value>
        /// 区间的结束边界值，通常在 $[0, 360)$ 范围内。
        /// </value>
        public double End { get; set; }

        /// <summary>
        /// 初始化 <see cref="CompassRangEX"/> 类的新实例。
        /// </summary>
        /// <param name="start">区间的起始角度（度）。</param>
        /// <param name="end">区间的结束角度（度）。</param>
        /// <example>
        /// <code>
        /// // 创建一个普通的角度范围 (10度 到 50度)
        /// var normalRange = new CompassRangEX(10, 50);
        /// 
        /// // 创建一个跨越360度的角度范围 (350度 到 20度)
        /// var crossRange = new CompassRangEX(350, 20);
        /// </code>
        /// </example>
        public CompassRangEX(double start, double end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// 确定指定的角度是否包含在当前的罗盘范围内。
        /// </summary>
        /// <param name="dValue">需要判定的目标角度（度）。支持负数或大于360度的值，内部会自动执行标准化。</param>
        /// <returns>
        /// 如果 <paramref name="dValue"/> 在当前范围内，则返回 <see langword="true"/>；否则返回 <see langword="false"/>。
        /// </returns>
        /// <remarks>
        /// 判定规则采用前闭后开区间 $[Start, End)$。
        /// <para>如果 <see cref="End"/> &gt; <see cref="Start"/>：正常范围判定，需同时满足大于等于 Start 且小于 End。</para>
        /// <para>如果 <see cref="End"/> &lt;= <see cref="Start"/>：说明范围跨越了 $360^\circ$ 界限，满足大于等于 Start 或小于 End 即可。</para>
        /// </remarks>
        public bool IsInRange(double dValue)
        {
            // 将输入角度标准化到 [0, 360) 区间
            dValue = (dValue % 360 + 360) % 360;

            if (this.End > this.Start)
            {
                // 正常范围
                return dValue >= this.Start && dValue < this.End;
            }
            else
            {
                // 跨越 360 度的特殊范围
                return dValue >= this.Start || dValue < this.End;
            }
        }

        /// <summary>
        /// 计算圆周上两个角度之间的最短弧度差值（夹角）。
        /// </summary>
        /// <param name="start">起始角度（度）。</param>
        /// <param name="end">结束角度（度）。</param>
        /// <returns>两个角度之间的最短距离，其值永远在 $[0, 180]$ 之间。</returns>
        /// <example>
        /// <code>
        /// // 计算 10 度到 350 度的最短距离，结果为 20
        /// double distance = CompassRangEX.AngleRangeValue(10, 350);
        /// </code>
        /// </example>
        public static double AngleRangeValue(double start, double end)
        {
            // 差值模360
            double diff = (end - start) % 360;

            // 修正负数模，使其落在 [0, 360)
            if (diff < 0) diff += 360;

            // 如果大于180度，说明反向走更近，取短弧
            return diff > 180 ? 360 - diff : diff;
        }

        /// <summary>
        /// 计算当前实例中 <see cref="Start"/> 与 <see cref="End"/> 之间的最短弧度差值（夹角）。
        /// </summary>
        /// <returns>当前实例范围的绝对最短夹角大小，其值永远在 $[0, 180]$ 之间。</returns>
        /// <seealso cref="AngleRangeValue(double, double)"/>
        public double AngleRangeValue()
        {
            return AngleRangeValue(this.Start, this.End);
        }



        /// <summary>
        /// 要卖当前的罗盘范围内，获取所有相关的罗盘对象类型（如八卦、24山、64卦等）。
        /// </summary>
        /// <returns></returns>
        public List<CompassObjType> GetCompassObjByDegree()
        {
            List<CompassObjType> ls = new List<CompassObjType>();

            //===========================八卦===========================
            foreach (string sN in GuoSubClass.BeforeGuoSubNames)
            {
                GuoSubClass gs = new GuoSubClass(sN);
                //===========================先天八卦===========================
                if (gs != null && gs.CBeforRangeDegree.IsInRange(this.Start) && gs.CBeforRangeDegree.IsInRange(this.End - 0.1))
                {
                    ls.Add(new CompassObjType { ObjTypeCNName = "先天八卦", CRDegree = gs.CBeforRangeDegree, ObjType = gs.GetType(), Obj = gs, Name = gs.GuoSubName });
                }
                //===========================先天八卦===========================
                //===========================后天八卦===========================
                if (gs != null && gs.CAfterRangeDegree.IsInRange(this.Start) && gs.CAfterRangeDegree.IsInRange(this.End - 0.1))
                {
                    ls.Add(new CompassObjType { ObjTypeCNName = "后天八卦", CRDegree = gs.CAfterRangeDegree, ObjType = gs.GetType(), Obj = gs, Name = gs.GuoSubName });
                }
                //===========================后天八卦===========================

            }
            //===========================八卦===========================


            //===========================24山===========================

            foreach (string sN in CHill.C24Hills)
            {
                CHill c = new CHill(sN);
                //===========================正针24山===========================
                if (c != null && c.CRangeDegree.IsInRange(this.Start) && c.CRangeDegree.IsInRange(this.End - 0.1))
                {
                    ls.Add(new CompassObjType { ObjTypeCNName = "正针24山", CRDegree = c.CRangeDegree, ObjType = c.GetType(), Obj = c, Name = c.Name });
                }
                //===========================正针24山===========================

            }
            //===========================24山===========================



            //===========================64卦===========================
            foreach (string sN in GuoClass.GuoNames)
            {
                GuoClass g = new GuoClass(sN);
                //===========================先天(天盘）64卦===========================
                if (g != null && g.CBeforeRangeDegree.IsInRange(this.Start) && g.CBeforeRangeDegree.IsInRange(this.End - 0.1))
                {
                    ls.Add(new CompassObjType { ObjTypeCNName = "先天64卦", CRDegree = g.CBeforeRangeDegree, ObjType = g.GetType(), Obj = g, Name = g.GuoName });
                }
                //===========================先天(天盘）64卦===========================
                //===========================后天（地盘）64卦===========================
                if (g != null && g.CAfterRangeDegree.IsInRange(this.Start) && g.CAfterRangeDegree.IsInRange(this.End - 0.1))
                {
                    ls.Add(new CompassObjType { ObjTypeCNName = "后天64卦", CRDegree = g.CAfterRangeDegree, ObjType = g.GetType(), Obj = g, Name = g.GuoName });
                }
                //===========================后天（地盘）64卦===========================

            }
            //===========================64卦===========================




            return ls;
        }

    }
}
