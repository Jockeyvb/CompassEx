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

using CompassEx.Assist;
using CompassEx.Comm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;



namespace CompassEx.Gua
{


    /// <summary>
    /// 占卦类型
    /// </summary>
    public enum GuaDoType
    {
        /// <summary>
        /// 默认为无
        /// </summary>
        [Description("无")]
        None = 0,

        /// <summary>
        /// 先天八卦
        /// </summary>
        [Description("六爻铜钱占卦"), Category("{level: 1 }")]
        SixYaos = 1,
        /// <summary>
        /// 后天八卦
        /// </summary>
        [Description("梅花先天数占卦"), Category("{level:2 }")]
        FlowerGuaBefore = 2,

        /// <summary>
        /// 后天八卦
        /// </summary>
        [Description("手动排盘"), Category("{level:3 }")]
        ManualSixYaos = 4,

    }

    /// <summary>
    /// 表示占卦主控制类，用于整合起卦、四柱参数配置以及六爻卦的动态推演与加载。
    /// </summary>
    public class GuaDo
    {



        /// <summary>
        /// 获取当前六爻卦依据日干支及预设旬空规则推导出的处于“日空亡”状态的地支对象列表。
        /// </summary>
        /// <value>一个包含空亡地支的集合（类型为 <see cref="LocClass"/> 列表）。</value>     
        public List<LocClass> DayLostLocs { get => (List<LocClass>)FSLT?.DaySL.LostLocs; }

        /// <summary>
        /// 获取或设置当前占出的主卦对象（本卦）。
        /// </summary>
        /// <value>一个 <see cref="GuaClass"/> 实例，若未加载则为 <c>null</c>。</value>
        [JsonProperty]
        public GuaClass? Gua
        {
            get; private set;

        }

        [JsonIgnore]
        public GuaClass? ChangedGua
        {
            get
            {

                var g = Gua?.GetChangeGua();

                g?.LoadAllYaos(this.FSLT.DaySL);
                return g;
            }
        }


        /// <summary>
        /// 占卦类型
        /// </summary>
        [JsonProperty]
        public GuaDoType GuaDoType { get; private set; }


        /// <summary>
        /// 梅花数字卦的两个数字(只有数字梅花占卦才有值)
        /// </summary>
        [JsonProperty]
        public int[] FlowerGuaNumber { get; private set; }

        /// <summary>
        /// 获取或设置当前占卦所对应的四柱干支信息类型。
        /// </summary>
        /// <value>一个 <see cref="FourSkyLocType"/> 实例，封装了年月日时的干支数据。</value>
        [JsonProperty]
        public FourSkyLocType FSLT { get; private set; }



        /// <summary>
        /// 反序列化后，需要加载方法
        /// </summary>
        /// <param name="context"></param>
        [System.Runtime.Serialization.OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (this.GuaDoType.HasFlag(GuaDoType.FlowerGuaBefore))
            {
                LoadFlowerGuaByBefore(this.FlowerGuaNumber);
            }
            else
            {
                LoadSixYaosGua(this.Gua.Yaos.Select(x => x.Value).ToArray(), this.GuaDoType.HasFlag(GuaDoType.ManualSixYaos));
            }



        }
        [JsonConstructor]
        private GuaDo()
        {

        }

        /// <summary>
        /// 初始化 <see cref="GuaDo"/> 类的新实例，通过指定的阳历时间自动转换为四柱参数来加载占卦信息。
        /// </summary>
        /// <param name="dt">用于起卦的目标阳历时间（<see cref="DateTime"/>）。</param>
        public GuaDo(DateTime dt) : this(dt.ToFourSkyLocType()!) { }


        /// <summary>
        /// 初始化 <see cref="GuaDo"/> 类的新实例，通过指定的四柱干支对象加载占卦信息。
        /// </summary>

        public GuaDo([JsonProperty(nameof(FSLT))] FourSkyLocType FSLT)
        {
            this.FSLT = FSLT;
        }


        /// <summary>
        /// 依据输入的 6 个爻值加载并初始化六爻主卦。
        /// </summary>
        /// <param name="YaoValues">包含 6 个爻值的整型数组（通常按初爻至上爻顺序排列）。（0为阴爻、1为阳爻，2为老阴（动爻），3为老阳（动爻）)</param>
        /// <param name="IsManual">是否手动排盘</param>
        /// <exception cref="ArgumentOutOfRangeException">当传入的 <paramref name="YaoValues"/> 数组长度不等于 6 时抛出此异常。</exception>
        public void LoadSixYaosGua(int[] YaoValues, bool IsManual = false)
        {
            if (YaoValues.Length != 6) throw new ArgumentOutOfRangeException(nameof(YaoValues));
            var gc = GuaClass.GetGuaClass(YaoValues);
            gc.LoadAllYaos(this.FSLT.DaySL); // 加载所有爻和所有干支类
            this.Gua = gc;
            this.GuaDoType = IsManual ? GuaDoType.ManualSixYaos : GuaDoType.SixYaos;

        }



        /// <summary>
        /// 先天梅花易数数字起卦
        /// </summary>
        /// <param name="YaoValues"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void LoadFlowerGuaByBefore(int[] YaoValues)
        {
            if (YaoValues.Length != 2) throw new ArgumentOutOfRangeException(nameof(YaoValues));
            LoadFlowerGuaByBefore(YaoValues[0], YaoValues[1]);
        }

        /// <summary>
        /// 执行经典的先天梅花易数数字起卦逻辑，推演并返回对应带有动爻的复卦。
        /// </summary>
        /// <param name="iv1">上卦计算基数（如：年支数 + 月数 + 日数之和，或上两字笔画数）。</param>
        /// <param name="iv2">下卦计算基数（如：年支数 + 月数 + 日数 + 时支数之和，或下两字笔画数）。</param>       
        /// <remarks>
        /// <b>梅花数理起卦公式说明：</b>
        /// <list type="number">
        /// <item><description><b>上卦公式</b>：<c>(iv1 % 8) - 1</c>。若余数为 0 则强行重设为 7（对应乾一、兑二……坤八的数组边界）。</description></item>
        /// <item><description><b>下卦公式</b>：<c>(iv2 % 8) - 1</c>。同样执行归零校验，并通过排除重复字提取出下卦物象，拼接成完整卦名以便反查。</description></item>
        /// <item><description><b>动爻公式（取变爻）</b>：<c>((iv1 + iv2) % 6) - 1</c>。相加取余锁定动爻。若余数为 0 则重设为 5（代表上爻/第六爻发动）。</description></item>
        /// </list>
        /// 算法最后会自动激活对应的动爻，并联动调用 <see cref="GuaClass.LoadAllYaos"/> 载入日干支及六亲、六神等外围断语属性，完成起卦闭环。
        /// </remarks>
        public void LoadFlowerGuaByBefore(int iv1, int iv2)
        {
            this.FlowerGuaNumber = [iv1, iv2];
            int iMod = (iv1 % 8) - 1; // 先天卦乾一，兑二...要减去1，因为卦位置从0开始
            if (iMod < 0) iMod = 7;   // 为负1，则是坤八数
            String sName = GuaSubClass.BeforeGuaSubAttrNames[iMod]; // 上卦名

            iMod = (iv2 % 8) - 1;
            if (iMod < 0) iMod = 7;   // 为负1，则是坤八数.
            sName += GuaSubClass.BeforeGuaSubAttrNames[iMod].Replace(sName, ""); // 下卦名



            int iYaoDoing = ((iv1 + iv2) % 6) - 1; // 相加取余则是动爻，要减去1，因为爻由0开始
            if (iYaoDoing < 0) iYaoDoing = 5;      // 为-1则是上爻动。

            GuaClass gc = new GuaClass(sName);

            gc.Yaos[iYaoDoing].IsDoing = true;
            gc.LoadAllYaos(this.FSLT.DaySL); // 加载所有爻和所有干支类
            this.Gua = gc;
            this.GuaDoType = GuaDoType.FlowerGuaBefore;
        }
    }
}
