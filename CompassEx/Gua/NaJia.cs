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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompassEx.Gua
{
    /// <summary>
    /// 易学理气核心：纳甲推演体系分类枚举。
    /// </summary>
    /// <remarks>
    /// 本枚举定义了系统支持的两大传统纳甲数理体系，分别服务于六爻预测学与风水勘测学。
    /// </remarks>
    public enum NaJiaType
    {
        /// <summary>
        /// 京房纳甲体系。
        /// </summary>
        /// <remarks>
        /// 主要用于 64 卦别卦（六爻复卦）的装卦、配干支、安世应，常服务于六爻预测学及天机出卦法。
        /// </remarks>
        JF = 0,

        /// <summary>
        /// 杨公纳甲体系。
        /// </summary>
        /// <remarks>
        /// 主要用于后天八卦单卦（三爻卦）的干支归化，常服务于九星山法（来龙/坐山）与辅星水法（向上消砂纳水）。
        /// </remarks>
        YG = 1
    }

    /// <summary>
    /// 统一的纳甲推演结果行为契约接口（用于泛型架构的强类型约束）。
    /// </summary>
    public interface INaJiaResult
    {
        /// <summary>
        /// 获取当前纳甲结果所属的推演体系类型。
        /// </summary>
        /// <value>
        /// 一个 <see cref="NaJiaType"/> 枚举值，表示所属的纳甲数理体系。
        /// </value>
        NaJiaType Type { get; }
    }

    /// <summary>
    /// 京房纳甲推演结果结构体。包含完整的六爻复卦、六亲及干支序列。
    /// </summary>
    /// <remarks>
    /// 本结构体数据常用于六爻预测学的装卦排盘。在天机出卦法中，亦可用于判定人命庚之干支是否得福神、官贵或犯出卦、入卦。
    /// </remarks>
    public struct NaJiaJFResult : INaJiaResult, IEquatable<NaJiaJFResult>
    {
        /// <summary>
        /// 当前推演所依附的六爻复卦（64卦别卦）实例。
        /// </summary>
        [JsonIgnore]
        public GuaClass Gua;

        /// <summary>
        /// 获取京房纳甲体系标记。
        /// </summary>
        /// <value>
        /// 始终返回 <see cref="NaJiaType.JF"/>。
        /// </value>
        public NaJiaType Type => NaJiaType.JF;

        /// <summary>
        /// 自下而上（初爻至上爻），六爻逐爻严格对应的干支（山字/干支时空坐标）集合。
        /// </summary>
        public List<SkyLoc> SkyLocs;


        /// <summary>
        /// 获取当前结构的哈希值，用以支持基于哈希算法的集合检索（如 <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/>）。
        /// </summary>
        /// <returns>返回根据内部关键字段生成的哈希整数值。</returns>
        public override int GetHashCode()
        {

            return Gua?.Name?.GetHashCode() ?? 0;
        }

        public bool Equals(NaJiaJFResult other)
        {
            string? thisName = this.Gua?.Name;
            string? otherName = other.Gua?.Name;

            // 如果两个都是 null（即都没有卦象），在业务上它们也算作相等
            return string.Equals(thisName, otherName, StringComparison.Ordinal);
        }
        public override bool Equals(object obj)
        {
            // 先检查 obj 是否是 NaJiaJFResult 类型，如果是，直接转发给强类型的 Equals 比较，避免重复编写逻辑
            return obj is NaJiaJFResult other && Equals(other);
        }
        /// <summary>
        /// 重载等于运算符 (<c>==</c>)，支持语法级直观对比。
        /// </summary>
        public static bool operator ==(NaJiaJFResult left, NaJiaJFResult right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// 重载不等于运算符 (<c>!=</c>)，支持语法级直观对比。
        /// </summary>
        public static bool operator !=(NaJiaJFResult left, NaJiaJFResult right)
        {
            return !left.Equals(right);
        }

    }



    /// <summary>
    /// 杨公纳甲推演结果结构体。包含单卦、归化天干及对应的地支宫位序列。
    /// </summary>
    /// <remarks>
    /// 本结构体数据为风水勘测的底层核心：九星山法依此读取来龙或坐山卦的归化干支；辅星水法依此读取向卦的纳甲水流水口。
    /// </remarks>
    public struct NaJiaYGResult : INaJiaResult, IEquatable<NaJiaYGResult>
    {
        /// <summary>
        /// 当前推演所依附的后天八卦单卦（三爻卦）实例。
        /// </summary>
        [JsonIgnore]
        public GuaSubClass Gua;

        /// <summary>
        /// 获取杨公纳甲体系标记。
        /// </summary>
        /// <value>
        /// 始终返回 <see cref="NaJiaType.YG"/>。
        /// </value>
        public NaJiaType Type => NaJiaType.YG;

        /// <summary>
        /// 当前单卦所纳的专属天干实例。
        /// </summary>
        /// <remarks>
        /// 易学数理对应：乾纳甲、坤纳乙、艮纳丙、巽纳辛、震纳庚、兑纳丁、离纳壬、坎纳癸。
        /// </remarks>
        public SkyClass Sky;

        /// <summary>
        /// 当前单卦纳甲数理所包含归化的地支（方位/时辰刻度）实例集合。
        /// </summary>
        /// <remarks>
        /// 四正卦依三合局归化地支：震纳亥卯未、兑纳巳酉丑、离纳寅午戌、坎纳申子辰。四维卦在风水高级理气中通常包含自身方位的延伸。
        /// </remarks>
        public List<LocClass> Locs;

        public override string ToString()
        {
            string st = "【" + Gua?.Name + "】：[天干：" + Sky.ToString() + ",地支：" + string.Join(",", Locs.Select(lc => lc.ToString())) + "]";
            return st;
        }

        /// <summary>
        /// 获取当前结构的哈希值，用以支持基于哈希算法的集合检索（如 <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/>）。
        /// </summary>
        /// <returns>返回根据内部关键字段生成的哈希整数值。</returns>
        public override int GetHashCode()
        {

            return Gua?.Name?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            // 先检查 obj 是否是 NaJiaYGResult 类型，如果是，直接转发给强类型的 Equals 比较，避免重复编写逻辑
            return obj is NaJiaYGResult other && Equals(other);
        }

        public bool Equals(NaJiaYGResult other)
        {
            string? thisName = this.Gua?.Name;
            string? otherName = other.Gua?.Name;

            // 如果两个都是 null（即都没有卦象），在业务上它们也算作相等
            return string.Equals(thisName, otherName, StringComparison.Ordinal);
        }

        /// <summary>
        /// 重载等于运算符 (<c>==</c>)，支持语法级直观对比。
        /// </summary>
        public static bool operator ==(NaJiaYGResult left, NaJiaYGResult right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// 重载不等于运算符 (<c>!=</c>)，支持语法级直观对比。
        /// </summary>
        public static bool operator !=(NaJiaYGResult left, NaJiaYGResult right)
        {
            return !left.Equals(right);
        }
    }

    /// <summary>
    /// 泛型纳甲推演核心发动机（处理类）。
    /// </summary>
    /// <typeparam name="TResult">必须是实现了 <see cref="INaJiaResult"/> 接口的结构体类型。</typeparam>
    /// <remarks>
    /// 本类采用控制反转与静态工厂设计模式，将繁复的京房、杨公装卦推演逻辑封装于内部。
    /// 对外提供高内聚的统一出口方法 <see cref="Execute"/>，完美支持动态扩充与高效计算。
    /// </remarks>
    public class NaJia<TResult> where TResult : struct, INaJiaResult
    {
        /// <summary>
        /// 内部私有类：用于静态配置杨公纳甲天干地支原始映射项的轻量级实体。
        /// </summary>
        public class NaJiaYGItem
        {
            /// <summary>
            /// 获取或设置归化的天干名称（如 "甲"、"乙"）。
            /// </summary>
            public string? SkyName { get; set; }

            /// <summary>
            /// 获取或设置归化的地支名称集合（如 ["亥", "卯", "未"]）。
            /// </summary>
            public string[]? LocNames { get; set; }
        }

        /// <summary>
        /// 静态只读的杨公纳甲数理映射字典。
        /// </summary>
        /// <remarks>
        /// 严格遵循地理风水“净阴净阳”及“二十四山纳甲”数理基础规则：
        /// <list type="bullet">
        ///   <item><description><b>乾坤艮巽（四维卦）：</b> 纳纯干，地支序列初始为空（高级理气应用中可根据需要扩充其同名方位山字）。</description></item>
        ///   <item><description><b>震兑离坎（四正卦）：</b> 依纳干所属五行的三合局归化地支（震木局[ "亥", "卯", "未"]、兑金局["巳", "酉", "丑"]、离火局["寅", "午", "戌"]、坎水局["申", "子", "辰"]）。</description></item>
        /// </list>
        /// </remarks>
        public static readonly Dictionary<string, NaJiaYGItem> YGNaJiaDC = new Dictionary<string, NaJiaYGItem>
    {
        { "乾", new NaJiaYGItem { SkyName = "甲", LocNames = new string[] { } } },
        { "坤", new NaJiaYGItem { SkyName = "乙", LocNames = new string[] { } } },
        { "震", new NaJiaYGItem { SkyName = "庚", LocNames = new string[] { "亥", "卯", "未" } } },
        { "巽", new NaJiaYGItem { SkyName = "辛", LocNames = new string[] { } } },
        { "艮", new NaJiaYGItem { SkyName = "丙", LocNames = new string[] { } } },
        { "兑", new NaJiaYGItem { SkyName = "丁", LocNames = new string[] { "巳", "酉", "丑" } } },
        { "离", new NaJiaYGItem { SkyName = "壬", LocNames = new string[] { "寅", "午", "戌" } } },
        { "坎", new NaJiaYGItem { SkyName = "癸", LocNames = new string[] { "申", "子", "辰" } } }
    };

        /// <summary>
        /// 获取或设置当前实例的纳甲计算体系类型。
        /// </summary>
        private NaJiaType CalculationType { get; set; }

        /// <summary>
        /// 内部存储计算所需的周易原始卦象数据（支持复卦 <see cref="GuaClass"/> 或单卦 <see cref="GuaSubClass"/>）。
        /// </summary>
        private object _GuaData;

        /// <summary>
        /// 初始化 <see cref="NaJia{TResult}"/> 类的新实例（供京房纳甲静态工厂调用）。
        /// </summary>
        /// <param name="g">传入的六爻复卦别卦实例（<see cref="GuaClass"/>）。</param>
        /// <exception cref="ArgumentNullException">当传入的复卦数据为 <see langword="null"/> 时抛出。</exception>
        public NaJia(GuaClass g)
        {
            _GuaData = g ?? throw new ArgumentNullException(nameof(g), "[数理错误] 京房纳甲输入的复卦数据不能为 null。");
            CalculationType = NaJiaType.JF;
        }

        /// <summary>
        /// 初始化 <see cref="NaJia{TResult}"/> 类的新实例（供杨公纳甲静态工厂调用）。
        /// </summary>
        /// <param name="gs">传入的后天八卦单卦实例（<see cref="GuaSubClass"/>）。</param>
        /// <exception cref="ArgumentNullException">当传入的单卦数据为 <see langword="null"/> 时抛出。</exception>
        public NaJia(GuaSubClass gs)
        {
            _GuaData = gs ?? throw new ArgumentNullException(nameof(gs), "[数理错误] 杨公纳甲输入的单卦数据不能为 null。");
            CalculationType = NaJiaType.YG;
        }


        /// <summary>
        /// 根据正针（地盘）二十四山向，计算并获取对应的杨公纳甲特异性结果。
        /// </summary>
        /// <remarks>
        /// <para><b>【业务背景】</b> 本方法实现了杨公堪舆理气中的“纳甲”核心逻辑。通过检索输入的二十四山向名称，自动匹配并归纳出其所属的八卦本宫或干支纳甲归属（天干纳甲、地支纳甲）。</para>
        /// <para><b>【架构安全机制】</b> 本方法虽声明在泛型类中，但属于杨公派系的<b>特异性静态方法</b>。内部引入了运行时类型锁，强制限定只有当类泛型参数 <typeparamref name="TResult"/> 确为 <see cref="NaJiaYGResult"/> 时方可成功执行，从而在多派系并存的堪舆系统中确保数理逻辑的安全隔离。</para>
        /// </remarks>
        /// <param name="c">输入的正针二十四山向实例对象 (<see cref="CHill"/>)。</param>
        /// <returns>返回构建完成的杨公纳甲专属结果实例 (<see cref="NaJiaYGResult"/>)。</returns>
        /// <exception cref="NotSupportedException">当调用本方法的泛型类上下文（<typeparamref name="TResult"/>）不是指定的 <see cref="NaJiaYGResult"/> 时阻断抛出。</exception>
        /// <exception cref="ArgumentNullException">当传入的正针二十四山向参数 <paramref name="c"/> 为 <see langword="null"/> 时抛出。</exception>
        /// <exception cref="ArgumentException">当入参山向名称在杨公纳甲映射字典 <c>YGNaJiaDC</c> 中未能匹配到任何对应的归属卦象时抛出。</exception>
        public static NaJiaYGResult CreateYG(CHill c)
        {
            if (typeof(TResult) != typeof(NaJiaYGResult))
            {
                throw new NotSupportedException("[数理阻断] 该静态方法专属于杨公纳甲业务，当前泛型上下文无权调用。");
            }

            if (c == null) throw new ArgumentNullException(nameof(c));

            var kv = YGNaJiaDC.Where(kv => kv.Key.Contains(c.Name) || kv.Value.SkyName.Equals(c.Name) || kv.Value.LocNames?.IndexOf(c.Name) > -1);//找到纳甲所在的卦
            if (!kv.Any()) throw new ArgumentException("入参中未能找到相关纳甲之卦", nameof(c));
            GuaSubClass gsc = new GuaSubClass(kv.FirstOrDefault().Key);
            return CreateYG(gsc);
        }





        /// <summary>
        /// 执行全盘纳甲核心数理逻辑推演（统一的总出口计算方法）。
        /// </summary>
        /// <returns>
        /// 返回计算完成后对应的泛型纳甲结果结构体；若体系类型未知则返回 <see langword="null"/>。
        /// </returns>
        /// <remarks>
        /// 本方法内部通过类型强转解包原始数据。对于杨公纳甲，通过提取单卦名称（<see cref="GuaSubClass.Name"/>）检索静态数理字典，动态解包、转换并装配完整的干支模型实体。
        /// </remarks>
        public TResult Execute()
        {
            // 1. 京房纳甲数理分支
            if (CalculationType == NaJiaType.JF)
            {
                var result = new NaJiaJFResult();
                GuaClass g = (GuaClass)_GuaData;

                result.Gua = g;
                result.SkyLocs = g.SkyLocs; // 完美对接复卦原本自带的干支集合

                // TODO: 编写具体的京房 64 卦纳甲配世应、纳干支核心算法逻辑...

                return (TResult)(object)result;
            }

            // 2. 杨公纳甲数理分支
            if (CalculationType == NaJiaType.YG)
            {
                var result = new NaJiaYGResult();
                GuaSubClass gs = (GuaSubClass)_GuaData;

                // 基于当前单卦名称（如"震"、"坎"）获取数理映射配置项
                var v = YGNaJiaDC[gs.Name];

                result.Gua = gs;

                // 将字符串名称转换为强类型的实体实例（解包 object 限制，恢复强类型提示）
                result.Sky = new SkyClass(v.SkyName);

                // 使用 LINQ 投影将字符串地支序列批量实例化为 LocClass 集合
                result.Locs = v.LocNames != null
                    ? v.LocNames.Select(sn => new LocClass(sn)).ToList()
                    : new List<LocClass>();

                return (TResult)(object)result;
            }

            return default;
        }

        /// <summary>
        /// 京房纳甲的静态工厂创建方法。输入六爻复卦，直接推导并返回完整的京房纳甲结果。
        /// </summary>
        /// <param name="g">需要计算的六爻复卦别卦实例（<see cref="GuaClass"/>）。</param>
        /// <returns>
        /// 返回包含复卦与六爻干支集合的 <see cref="NaJiaJFResult"/> 结构体数据。
        /// </returns>
        public static NaJiaJFResult CreateJF(GuaClass g)
        {
            return (new NaJia<NaJiaJFResult>(g)).Execute();
        }

        /// <summary>
        /// 杨公纳甲的静态工厂创建方法。输入后天单卦，直接推导并返回完整的杨公纳甲结果。
        /// </summary>
        /// <param name="gs">需要计算的后天八卦单卦实例（<see cref="GuaSubClass"/>）。</param>
        /// <returns>
        /// 返回包含单卦、天干及地支宫位的 <see cref="NaJiaYGResult"/> 结构体数据。
        /// </returns>
        public static NaJiaYGResult CreateYG(GuaSubClass gs)
        {
            return (new NaJia<NaJiaYGResult>(gs)).Execute();
        }
    }

}

