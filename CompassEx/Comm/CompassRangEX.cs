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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CompassEx.Comm;


/// <summary>
/// 表示一个罗盘度数的角度范围。
/// </summary>
/// <remarks>
/// 该类用于处理 $0^\circ$ 到 $360^\circ$ 之间的角度区间判定。
/// 它支持正常的递增区间（例如 $10^\circ \sim 50^\circ$），
/// 同时也原生支持跨越 $360^\circ$ 边界的循环区间（例如 $350^\circ \sim 20^\circ$）。
/// </remarks>
public class CompassRangEX : IEquatable
{

    /// <summary>
    /// 获取或设置当前角度范围所对应的罗盘对象类型。（主要用于在字典中有多个相同的角度范围时区分不同的对象类型）
    /// </summary>
    public CompassObjType COT { get; set; }


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
    public CompassRangEX(double start, double end) : this(start, end, CompassObjType.None)
    {

    }

    /// <summary>
    /// 初始化 <see cref="CompassRangEX"/> 类的新实例，并指定其对应的罗盘对象类型。
    /// </summary>
    /// <param name="start">区间的起始角度（度）。</param>
    /// <param name="end">区间的结束角度（度）。</param>
    /// <param name="cot">对应的罗盘对象类型(主要用于不同对象的区分，字典可用)。</param>
    [JsonConstructor]
    public CompassRangEX([JsonProperty(nameof(Start))] double start, [JsonProperty(nameof(End))] double end, [JsonProperty(nameof(COT))] CompassObjType cot)
    {
        this.Start = start;
        this.End = end;
        this.COT = cot;
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


    /// <remarks>
    /// <para><b>一、 空间反向包含检索原理：</b></para>
    /// <para>
    /// 该方法属于反向地理坐标拾取逻辑。它通过遍历各层级的基础元数据（如八卦、二十四山、六十四卦），
    /// 调用其度数范围对象的 <c>IsInRange</c> 方法，验证当前类所代表的度数范围起点（<c>this.Start</c>）与终点（<c>this.End</c>）
    /// 是否被该山头、卦位或八卦方位的几何区间<b>完全包裹并包含</b>。
    /// </para>
    /// <para><b>二 ==========================================</b></para>
    /// <para><b>⚠️ 浮点数边界容差设计（扣度原理）：</b></para>
    /// <para>
    /// 由于计算机浮点数（<c>double</c>）运算存在微小的精度截断误差，且罗盘上相邻两个方位实体之间的临界度数是完全重合、相互粘连的
    /// （例如地盘正针子山的结束边界为 <c>7.5</c> 度，而癸山的起始边界同样为 <c>7.5</c> 度）。
    /// </para>
    /// <para>
    /// 为了在进行全区间覆盖判定时，防止临界值因精度溢出而导致系统误判，或错误地多抓取到相邻的下一个方位对象，
    /// 本算法在校验结束边界时，对传入的结束度数执行了减去 <c>0.1</c> 度的安全容差修正（即 <c>this.End - 0.01</c>）。
    /// 这样能确保检索结果在空间物理排布上的唯一性与精准度。
    /// </para>
    /// <para><b>三 ==========================================</b></para>
    /// <para><b>三元玄空大卦术语规范备注：</b></para>
    /// <para>
    /// 遵循三元玄空大卦派“体用分离”与宋代邵雍《伏羲先天六十四卦方圆图》的排布法则，罗盘大卦圈层其本底均由先天八卦理气数理推演而来。
    /// 在本方法的映射体系中：
    /// </para>
    /// <list type="bullet">
    /// <item><description><see cref="CompassObjType.BeforeGua"/> 对应并检索的是<b>天盘六十四卦（先天大卦圆图）</b>，在罗盘上主理气纳水与抽爻换象。</description></item>
    /// <item><description><see cref="CompassObjType.AfterGua"/> 对应并检索的是<b>地盘六十四卦（先天大卦方图）</b>，在罗盘上常用于对照辨析交媾理气。</description></item>
    /// </list>
    /// </remarks>
    public List<CompassObjStru> GetCompassObjByDegree(CompassObjType ot = CompassObjType.All)
    {
        return CompassObjsManager.GetCompassObjByDegree(this.Start, this.End, ot);
    }


    public static double MidAngle(double start, double end)
    {
        if (end <= start) end += 360;
        return ((start + end) / 2.0) % 360;
    }


    private const double Epsilon = 1e-9;

    private static bool DoubleEquals(double a, double b)
        => Math.Abs(a - b) < Epsilon;



    public static bool operator ==(CompassRangEX left, CompassRangEX right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(CompassRangEX left, CompassRangEX right) => !(left == right);


    public override bool Equals(object obj)
    {
        if (obj is not CompassRangEX other) return false;
        return DoubleEquals(this.Start, other.Start)
            && DoubleEquals(this.End, other.End)
            && this.COT == other.COT;
    }

    public override int GetHashCode()
    {
        // 2. 经典且高效的哈希组合算法（基于质数 17 和 23）
        // 完美解决不支持 HashCode.Combine 的问题，确保 Start 和 End 共同决定唯一性
        unchecked // 即使数值溢出也安全运行
        {
            int hash = 17;
            hash = hash * 23 + this.Start.GetHashCode();
            hash = hash * 23 + this.End.GetHashCode();
            hash = hash * 23 + this.COT.GetHashCode();
            return hash;
        }
    }
}

