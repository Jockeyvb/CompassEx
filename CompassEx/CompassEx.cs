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
namespace CompassEx
{
    /// <summary>
    /// 罗盘的基本类
    /// </summary>

    public class CompassEx
    {
        #region 字段

        /// <summary>
        /// 先后天八卦的单卦度数。
        /// </summary>
        /// <value>默认值为 45 度（360度 / 8卦）。</value>
        public const double GuaSubDegree = 45;

        /// <summary>
        /// 罗盘后天八卦的卦名序列。
        /// </summary>
        /// <value>包含 8 个后天卦名："坎", "艮", "震", "巽", "离", "坤", "兑", "乾"。</value>
        public static readonly String[] CompassAfterGuaSubNames = { "坎", "艮", "震", "巽", "离", "坤", "兑", "乾" };

        /// <summary>
        /// 罗盘先天八卦的卦名序列。
        /// </summary>
        /// <value>包含 8 个先天卦名："坤", "震", "离", "兑", "乾", "巽", "坎", "艮"。</value>
        public static readonly string[] CompassBeforGuaSubNames = { "坤", "震", "离", "兑", "乾", "巽", "坎", "艮" };

        /// <summary>
        /// 先天 64 卦对象字典缓存（天盘）。
        /// </summary>
        /// <value>
        /// 键为 <see cref="CompassRangEX"/> 范围，值为对应的 <see cref="GuaClass"/> 卦象对象。
        /// </value>
        /// <remarks>
        /// <para><b>性能说明：</b>使用字典缓存所有 64 卦对象，避免每次计算时重复创建新对象，提高执行性能。系统启动时需要优先初始化此字典。</para>
        /// <para><b>排列规则：</b>罗盘上先天排行从午为乾1、兑2、离3、震4（阳仪），巽5、坎6、艮7、坤8（阴仪）。</para>
        /// <para><b>度数映射：</b>罗盘上子坤（360度）至午乾（180度）。从坤 0 度至天风姤 180 度，乾左为顺 180 度至地雷复 360 度。内卦为卦宫，外卦相荡而成。</para>
        /// </remarks>
        [JsonIgnore]
        public static Dictionary<CompassRangEX, GuaClass> CBeforeGuas;

        /// <summary>
        /// 后天 64 卦对象字典缓存（地盘）。
        /// </summary>
        /// <value>
        /// 键为 <see cref="CompassRangEX"/> 范围，值为对应的 <see cref="GuaClass"/> 卦象对象。
        /// </value>
        /// <remarks>
        /// <para><b>性能说明：</b>使用字典缓存所有 64 卦对象，避免每次计算时重复创建新对象，提高执行性能。系统启动时需要优先初始化此字典。</para>
        /// <para><b>排列规则：</b>罗盘上后天排行从子为乾9、兑4、离3、震8（阳仪），巽2、坎7、艮6、坤1（阴仪）。</para>
        /// <para><b>度数映射：</b>罗盘上子乾（360度）至午坤（180度）。从乾 0 度至雷地豫 180 度，坤左为顺 180 度至地雷复 360 度。外卦为卦宫，内卦相荡而成。</para>
        /// </remarks>
        [JsonIgnore]
        public static Dictionary<CompassRangEX, GuaClass> CAfterGuas;

        /// <summary>
        /// 罗盘 64 卦的单卦度数。
        /// </summary>
        /// <value>默认值为 5.625 度（360度 / 64卦）。</value>
        public const double CompassGuaDegree = 5.625;

        /// <summary>
        /// 罗盘二十四山的单山度数。
        /// </summary>
        /// <value>默认值为 15 度（360度 / 24山）。</value>
        public const double CHillDegree = 15;

        #endregion


        #region 属性

        private CHill c24Hill;

        /// <summary>
        /// 获取当前罗盘度数对应的二十四山对象。
        /// </summary>
        /// <value>
        /// 返回一个 <see cref="CHill"/> 对象，表示当前度数所落入的二十四山方位。
        /// </value>
        public CHill C24Hill { get => c24Hill; }


        private double degree;

        /// <summary>
        /// 获取或设置当前罗盘指向的度数。
        /// </summary>
        /// <value>
        /// 一个 <see cref="double"/> 值，表示罗盘的当前角度（通常为 0 至 360 度）。
        /// </value>
        /// <remarks>
        /// <para><b>警告（副作用）：</b>当设置（<c>set</c>）此属性的值时，系统会自动调用内部的 <c>Init()</c> 方法。</para>
        /// <para>该方法会根据新传入的度数，重新计算并刷新与之关联的二十四山、后天八卦以及先天 64 卦等所有依赖属性。</para>
        /// </remarks>
        public double Degree
        {
            get => degree;
            set
            {
                degree = value;
                Init();
            }
        }


        private GuaSubClass AfterGuaSub_;

        /// <summary>
        /// 获取当前罗盘度数对应的后天八卦对象。
        /// </summary>
        /// <value>
        /// 返回一个 <see cref="GuaSubClass"/> 对象，表示当前度数所属的后天单卦方位。
        /// </value>
        public GuaSubClass AfterGuaSub { get => AfterGuaSub_; }


        private GuaClass beforGua;

        /// <summary>
        /// 获取当前罗盘度数对应的先天 64 卦对象。
        /// </summary>
        /// <value>
        /// 返回一个 <see cref="GuaClass"/> 对象，表示当前度数在天盘上对应的先天 64 卦象。
        /// </value>
        public GuaClass BeforGua { get => beforGua; }

        #endregion



        #region 构造函数

        /// <summary>
        /// 初始化 <see cref="CompassEx"/> 类的新实例，并根据传入的度数完成罗盘各方位对象的初始化。
        /// </summary>
        /// <param name="Degreen">当前罗盘指向的度数（通常为 0 至 360 度）。</param>
        /// <remarks>
        /// <para>实例化该对象时，传入的度数会直接赋值给 <see cref="Degree"/> 属性。</para>
        /// <para>由此会隐式触发内部的初始化流程，自动计算并填充对应的后天八卦（<see cref="AfterGuaSub"/>）、二十四山（<see cref="C24Hill"/>）以及先天 64 卦（<see cref="BeforGua"/>）等关联对象。</para>
        /// </remarks>
        public CompassEx(double Degreen)
        {
            this.Degree = Degreen;
        }

        #endregion


        #region 方法

        /// <summary>
        /// 根据当前罗盘的度数，隐式初始化所有相关的方位与卦象属性。
        /// </summary>
        /// <remarks>
        /// 该方法在私有字段更新时被内部触发，依次调用并刷新以下对象：
        /// <list type="bullet">
        /// <item><description>后天八卦对象：<see cref="AfterGuaSub_"/></description></item>
        /// <item><description>二十四山对象：<see cref="c24Hill"/></description></item>
        /// <item><description>先天 64 卦对象：<see cref="beforGua"/></description></item>
        /// </list>
        /// </remarks>
        private void Init()
        {
            this.AfterGuaSub_ = GetAfterGuaSub();
            this.c24Hill = Get24Hill();
            this.beforGua = GetCBeforeGua();
        }

        /// <summary>
        /// 获取指定先天（天盘）64 卦卦名在罗盘上对应的度数范围对象。
        /// </summary>
        /// <param name="Name">要查询的先天 64 卦的完整卦名。</param>
        /// <returns>返回对应的 <see cref="CompassRangEX"/> 度数范围对象；若在缓存中未匹配到该卦名，则返回 <c>null</c>。</returns>
        public static CompassRangEX GetCBeforeGuaDegree(string Name)
        {
            foreach (var kv in CBeforeGuas)
            {
                if (kv.Value.Name == Name)
                {
                    return kv.Key;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取指定后天（地盘）64 卦卦名在罗盘上对应的度数范围对象。
        /// </summary>
        /// <param name="Name">要查询的后天 64 卦的完整卦名。</param>
        /// <returns>返回对应的 <see cref="CompassRangEX"/> 度数范围对象；若在缓存中未匹配到该卦名，则返回 <c>null</c>。</returns>
        public static CompassRangEX GetCAfterGuaDegree(string Name)
        {
            foreach (var kv in CAfterGuas)
            {
                if (kv.Value.Name == Name)
                {
                    return kv.Key;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据当前罗盘指向的度数，匹配并获取对应的先天 64 卦对象。
        /// </summary>
        /// <returns>返回当前度数所在的 <see cref="GuaClass"/> 先天 64 卦对象；若未找到匹配的范围则返回 <c>null</c>。</returns>
        public GuaClass GetCBeforeGua()
        {
            foreach (var kv in CBeforeGuas)
            {
                if (kv.Key.IsInRange(this.degree))
                {
                    return kv.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// 从数据源加载并初始化罗盘上的所有后天 64 卦（地盘）全局静态缓存字典。
        /// </summary>
        /// <remarks>
        /// <para><b>排列规则：</b>卦象按照顺时针方向在罗盘上排布，从坤卦开始，每个卦位占据 5.625 度（逆时针荡卦推演），共计 64 个卦象。</para>
        /// <para>该方法应在程序启动或系统初始化阶段优先执行。</para>
        /// </remarks>
        public static void LoadAllCAfterGuas()
        {
            CAfterGuas = C3Y.C3Y.GetAllCAfterGuas();
        }

        /// <summary>
        /// 从数据源加载并初始化罗盘上的所有先天 64 卦（天盘）全局静态缓存字典。
        /// </summary>
        /// <remarks>
        /// <para><b>排列规则：</b>卦象按照顺时针方向在罗盘上排布，从坤卦开始，每个卦位占据 5.625 度（逆时针荡卦推演），共计 64 个卦象。</para>
        /// <para>该方法应在程序启动或系统初始化阶段优先执行。</para>
        /// </remarks>
        public static void LoadAllCBeforeGuas()
        {
            CBeforeGuas = C3Y.C3Y.GetAllBeforGuas();
        }

        /// <summary>
        /// 根据当前罗盘指向的度数，计算并获取对应的二十四山对象。
        /// </summary>
        /// <returns>返回包装了当前山名的 <see cref="CHill"/> 对象；若均未匹配到对应范围则返回 <c>null</c>。</returns>
        public CHill Get24Hill()
        {
            foreach (string sN in CHill.C24HillNames)
            {
                CompassRangEX range = Get24HillDegree(sN);
                if (range.IsInRange(this.degree))
                {
                    CHill hill = new CHill(sN);
                    return hill;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据指定的山名，计算并获取该山在二十四山罗盘中所占据的绝对度数范围。
        /// </summary>
        /// <param name="HillName">指定的山名（如“壬”、“子”、“癸”等）。</param>
        /// <returns>返回表示该山起始与结束角度的 <see cref="CompassRangEX"/> 范围对象。</returns>
        /// <remarks>
        /// <para><b>推演原理：</b></para>
        /// <list type="bullet">
        /// <item><description>二十四山以<b>壬山</b>为度数起点，初始绝对方位设为 <c>337.5</c> 度。</description></item>
        /// <item><description>根据该山名在列表中的索引位置，按每山 <c>15</c> 度顺时针累加。</description></item>
        /// <item><description>当计算出的起始或结束度数超过 <c>360</c> 度时，系统会自动执行闭环修正（扣除 360 度）以限制在 <c>0 ~ 360</c> 度正常范围内。</description></item>
        /// </list>
        /// </remarks>
        public static CompassRangEX Get24HillDegree(string HillName)
        {
            double baseDegree = 337.5;

            int GIndex = CHill.C24HillNames.IndexOf(HillName);
            double degree = baseDegree + GIndex * CHillDegree;
            double fStart = degree;
            if (fStart > 360) fStart -= 360;
            double fEnd = degree + CHillDegree;
            if (fEnd > 360) fEnd -= 360;

            CompassRangEX range = new CompassRangEX(fStart, fEnd);
            return range;
        }

        /// <summary>
        /// 根据当前罗盘指向的度数，计算并获取对应的先天八卦单卦对象。
        /// </summary>
        /// <returns>返回对应的 <see cref="GuaSubClass"/> 先天八卦对象；若没有落在有效范围内则返回 <c>null</c>。</returns>
        public GuaSubClass GetBeforeGuaSub()
        {
            foreach (string sN in CompassAfterGuaSubNames)
            {
                CompassRangEX range = GetBeforGuaSubDegree(sN);
                if (range.IsInRange(this.degree))
                {
                    var g = GuaSubClass.GetGuaSub(sN, true);
                    return g;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取指定先天八卦对象在罗盘上对应的度数范围对象。
        /// </summary>
        /// <param name="g">包含目标卦名的 <see cref="GuaSubClass"/> 卦象对象实例。</param>
        /// <returns>返回对应的 <see cref="CompassRangEX"/> 度数范围对象。</returns>
        public static CompassRangEX GetBeforGuaSubDegree(GuaSubClass g)
        {
            return GetBeforGuaSubDegree(g.Name);
        }

        /// <summary>
        /// 根据指定的先天八卦卦名，计算并获取其在罗盘上所占据的绝对度数范围。
        /// </summary>
        /// <param name="Name">先天八卦的卦名（如“乾”、“坤”等）。</param>
        /// <returns>返回表示该卦起始与结束角度的 <see cref="CompassRangEX"/> 范围对象。</returns>
        /// <remarks>
        /// <para><b>推演原理：</b></para>
        /// <list type="bullet">
        /// <item><description>先天八卦以<b>艮卦</b>为度数起点，初始绝对方位设为 <c>337.5</c> 度。</description></item>
        /// <item><description>根据卦名在先天序列中的索引位置，按每卦 <c>45</c> 度顺时针累加。</description></item>
        /// <item><description>计算结果若超过 <c>360</c> 度，系统会自动执行闭环修正以保持在 <c>0 ~ 360</c> 度范围内。</description></item>
        /// </list>
        /// </remarks>
        public static CompassRangEX GetBeforGuaSubDegree(string Name)
        {
            double baseDegree = 337.5;

            int GIndex = CompassBeforGuaSubNames.IndexOf(Name);
            double degree = baseDegree + GIndex * GuaSubDegree;
            double fStart = degree;
            if (fStart > 360) fStart -= 360;
            double fEnd = degree + GuaSubDegree;
            if (fEnd > 360) fEnd -= 360;

            CompassRangEX range = new CompassRangEX(fStart, fEnd);
            return range;
        }

        /// <summary>
        /// 根据当前罗盘指向的度数，计算并获取对应的后天八卦单卦对象。
        /// </summary>
        /// <returns>返回对应的 <see cref="GuaSubClass"/> 后天八卦对象；若没有落在有效范围内则返回 <c>null</c>。</returns>
        public GuaSubClass GetAfterGuaSub()
        {
            foreach (string sN in CompassAfterGuaSubNames)
            {
                CompassRangEX range = GetAfterGuaSubDegree(sN);
                if (range.IsInRange(this.degree))
                {
                    var g = GuaSubClass.GetGuaSub(sN, true);
                    return g;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取指定后天八卦对象在罗盘上对应的度数范围对象。
        /// </summary>
        /// <param name="g">包含目标卦名的 <see cref="GuaSubClass"/> 卦象对象实例。</param>
        /// <returns>返回对应的 <see cref="CompassRangEX"/> 度数范围对象。</returns>
        public static CompassRangEX GetAfterGuaSubDegree(GuaSubClass g)
        {
            return GetAfterGuaSubDegree(g.Name);
        }

        /// <summary>
        /// 根据指定的后天八卦卦名，计算并获取其在罗盘上所占据的绝对度数范围。
        /// </summary>
        /// <param name="Name">后天八卦的卦名（如“坎”、“坤”等）。</param>
        /// <returns>返回表示该卦起始与结束角度的 <see cref="CompassRangEX"/> 范围对象。</returns>
        /// <remarks>
        /// <para><b>推演原理：</b></para>
        /// <list type="bullet">
        /// <item><description>后天八卦以<b>坎卦</b>为度数起点，初始绝对方位设为 <c>337.5</c> 度。</description></item>
        /// <item><description>根据卦名在后天序列中的索引位置，按每卦 <c>45</c> 度顺时针累加。</description></item>
        /// <item><description>计算结果若超过 <c>360</c> 度，系统会自动执行闭环修正以保持在 <c>0 ~ 360</c> 度范围内。</description></item>
        /// </list>
        /// </remarks>
        public static CompassRangEX GetAfterGuaSubDegree(string Name)
        {
            double baseDegree = 337.5;

            int GIndex = CompassAfterGuaSubNames.IndexOf(Name);
            double degree = baseDegree + GIndex * GuaSubDegree;
            double fStart = degree;
            if (fStart > 360) fStart -= 360;
            double fEnd = degree + GuaSubDegree;
            if (fEnd > 360) fEnd -= 360;

            CompassRangEX range = new CompassRangEX(fStart, fEnd);
            return range;
        }

        #endregion

    }

}
