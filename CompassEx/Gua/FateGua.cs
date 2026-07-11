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
using System.Linq;
using tyme.solar;


namespace CompassEx.Gua
{
    /// <summary>
    /// 是命卦的结构类型，包含了命卦的六爻纯卦和一些相关属性，例如：命卦的五行属性，命卦的运等<br/>
    /// 1、涉及余胜的命卦计算方法，命卦的入卦和出卦的计算方法，命卦的七爻飞爻卦的计算方法，以及命卦与罗盘24山（向）的关系等内容。
    /// 余胜唐所著的《玄空大卦些子法真诀》与《三元直指》中的天机出卦法：余胜唐在《三元直指》书中177页中，明确指出天机出卦法是纳甲用法（三元命卦无关）且并没涉及玄空大卦中的五行之论（虽然书中不全论大卦，亦应注或提示），如不查他其他著作完全不知道要如何使用且《玄空大卦些子法真诀》中的天机出卦法只说【命卦】又不指出命卦具体是三元命卦还是纳甲之卦，使用方法是以向卦为论，那么玄空大卦中是以所向的成卦之卦宫为论？还是以后天八卦为论？还是纳甲卦为论？真让人摸不着头脑。著书者应当严明解说；学习风水者应细仔研究考究为用，不可单论。<br/>
    /// 2、在刘贲所作《玄空大卦透析》书中456页第十九章中提到相似，甚至比余写的更为清晰指出以三元64卦以卦气为论，命卦则是纳甲（包括京房与杨公之纳甲法）用户可以参考其计算方法与判断方法。<br/>
    /// 关于京房纳甲与杨公纳甲资料，请自行查阅区别与使用<br />
    /// <b><font color="red">仅参供参考，请谨慎看待使用</font></b>
    /// </summary>
    public class FateGua
    {

        /// <summary>
        /// 个人本命三元后天三爻命卦
        /// </summary>
        /// <value>存储由出生年月日、性别推演得出的本命三爻卦实例，外部只读</value>
        public GuaSubClass FateGuaSub { get; private set; } = null;

        /// <summary>
        /// 天机卦系统 后天入卦集合（共5个入卦三爻卦）
        /// </summary>
        /// <value>Key：卦标识，Value：对应后天三爻卦实例，用于入卦得福判定</value>
        public Dictionary<string, GuaSubClass> InGuaSubs { get; private set; } = null;

        /// <summary>
        /// 天机卦系统 后天出卦集合（共3个出卦三爻卦）
        /// </summary>
        /// <value>Key：卦标识，Value：对应后天三爻卦实例，用于出卦凶性判定</value>
        public Dictionary<string, GuaSubClass> OutGuaSubs { get; private set; } = null;

        /// <summary>
        /// 京房易七世飞爻卦列表
        /// </summary>
        /// <value>由初爻依次变爻生成，共8卦（本卦+七世变卦），用于定位卦位、游魂/归魂判定、得福流年取值</value>
        public List<GuaClass> GuaList { get; private set; } = null;

        /// <summary>
        /// 命卦是否判定为出卦状态
        /// </summary>
        /// <value>true=出卦（天机外安、主不吉），false=入卦（天机内安、主吉）</value>
        public bool IsOutGua { get; private set; } = false;

        /// <summary>
        /// 命卦推演吉凶消息容器
        /// </summary>
        /// <value>存储推演过程中的提示、吉断、异常错误信息，用于前端展示与日志输出</value>
        public GoodBadInfos Infos { get; private set; } = new GoodBadInfos();

        /// <summary>
        /// 本命出生年干支（命庚）
        /// </summary>
        /// <value>由公历生日换算农历六十甲子年干支，用于纳甲出卦、得福人判定</value>
        public SkyLoc FateYearSkyLoc { get; private set; }

        /// <summary>
        /// 公历出生时间
        /// </summary>
        /// <value>用于立春分界判断、真实命年校正、干支换算</value>
        public DateTime BirthTime { get; private set; }

        /// <summary>
        /// 罗盘坐向对应的六爻向卦
        /// </summary>
        /// <value>风水罗盘24山对应六爻卦，为出入卦、纳甲推演的基准卦体</value>
        public GuaClass ToGua { get; private set; }

        /// <summary>
        /// 天机出卦法推演实例
        /// </summary>
        /// <value>承载向卦天机出入卦、飞爻卦列表等核心推演数据</value>
        public TianJiGua TJGua { get; private set; }

        /// <summary>
        /// 入卦得福对应命庚干支集合
        /// </summary>
        /// <value>符合本卦入卦得福的出生年天干，对应得福人群</value>
        public string[] InGuaGoodSL { get; private set; }

        /// <summary>
        /// 入卦得福对应流年干支集合
        /// </summary>
        /// <value>本卦体系下对应的得福年份，用于择日择年参考</value>
        public string[] InGuaGoodYear { get; private set; }

        /// <summary>
        /// 初始化命卦推演实例，自动完成本命卦、出入卦、纳甲吉凶、得福干支年份全量计算
        /// </summary>
        /// <param name="d">公历出生时间，用于校正立春真实命年（确定精确的生肖与三元命年分界线）</param>
        /// <param name="Sex">性别，仅支持【男/女】，男女命卦计算对应的数学公式与飞星轨迹不同</param>
        /// <param name="ToGua">罗盘六爻向卦实例，不可为空（用于比对房屋坐向与个人命卦的吉凶关系）</param>
        /// <exception cref="Exception">出生日期非法、性别非法、向卦为空、命卦计算异常、出卦数据异常时抛出</exception>
        public FateGua(DateTime d, string Sex, GuaClass ToGua)
        {
            try
            {
                // -----------------------------------------------------------------
                // 1. 输入参数合法性校验
                // -----------------------------------------------------------------
                if (d <= DateTime.MinValue) throw new Exception("请输入正确的出生日期。");
                if (Sex != "男" && Sex != "女") throw new Exception("性别只能是【男】或【女】");
                if (ToGua == null) throw new Exception("请输入正确的向卦。");
                this.ToGua = ToGua;

                // -----------------------------------------------------------------
                // 2. 核心本命卦推演
                // -----------------------------------------------------------------
                // 调用静态方法，根据公历出生时间和性别，计算出用户的后天三元本命卦（即八宅明镜之命宫）
                this.FateGuaSub = GetFateGua(d, Sex);
                if (this.FateGuaSub == null) throw new Exception("无法正确计算命卦。");
                this.BirthTime = d;

                // -----------------------------------------------------------------
                // 3. 天机出卦法理推演
                // -----------------------------------------------------------------
                // 实例化天机出卦法，获取当前向卦在罗盘立向上的“入卦”与“出卦”具体范围
                TianJiGua tjg = new TianJiGua(this.ToGua);//获得天机出卦法信息(命卦计算）

                this.TJGua = tjg;
                this.InGuaSubs = tjg.InGuaSubs;   // 获取理气相合、未出卦的吉祥干支方位
                this.OutGuaSubs = tjg.OutGuaSubs; // 获取理气不合、犯出卦的凶祸干支方位

                // -----------------------------------------------------------------
                // 4. 出卦状态深度校验（交叉判定）
                // -----------------------------------------------------------------
                // 步骤A：优先验证用户的“本命卦”与当前房屋坐向之间是否产生“同宫出卦”或“两界出卦”
                this.IsOutGua = IsOutByFateGua(); //天机出卦法中（命卦）是否出卦

                // 步骤B：若天机法判定未出卦（即属于吉或平），则进一步引入“纳甲法”进行细化演算
                // 校验命卦与向卦的纳甲五行生克，若纳甲理气不和导致出卦，则生成对应的吉凶坏消息
                if (this.IsOutGua == false) //如果已经出卦出无须再计算
                {
                    this.IsOutGua = IsOutByNaJia(); //计算向卦与命卦中是否出卦并生成好坏消息
                }

                // -----------------------------------------------------------------
                // 5. 组装输出文本信息
                // -----------------------------------------------------------------
                // 格式化输出完整的理气盘口结论。注：在风水玄学择日（选时辰）时，应当结合本命对应的“命庚卦”联合使用
                Infos.Info.Info = $"向卦：【{this.ToGua.Name}】，卦宫：【{this.ToGua.GuaSelf.Name}】,三元命卦：【{this.FateGuaSub.AfterQuantity + this.FateGuaSub.AfterGuaSubColor + this.FateGuaSub.Name}】，三元命卦辅助参考，择日应该与命庚卦为用";

            }
            catch (Exception ex)
            {
                // 异常捕获机制：一旦推演过程出错，清空命卦实例，并将底层错误堆栈日志写入坏消息队列中
                this.FateGuaSub = null;
                this.Infos.BadInfos.Add(new InfoType(false) { Info = ex.ToString() });
            }
        }

        /// <summary>
        /// 根据公历生日、性别，计算校正立春后的【三元后天三爻本命卦】
        /// </summary>
        /// <remarks>
        /// 本算法严格遵循传统三元九运紫白飞星命卦口诀进行数学建模：
        /// <list type="number">
        /// <item>
        /// <description><b>岁首分界：</b> 严格以立春精确交节时间为准。立春前出生者自动视作上一年出生，年份减1。</description>
        /// </item>
        /// <item>
        /// <description><b>年数求一（九进制约简）：</b> 将公历年份四位数字逐位累加，直到缩减为 1~9 的个位基数（等价于 Mod 9 算法）。</description>
        /// </item>
        /// <item>
        /// <description><b>男命公式：</b> 运星逆行，使用 <c>11 - 基数</c>。若余数为5寄宫于【坤二宫】，为0归【坎一宫】。</description>
        /// </item>
        /// <item>
        /// <description><b>女命公式：</b> 运星顺行，使用 <c>基数 + 4</c>（即加9取余）。若余数为5寄宫于【艮八宫】，为0归【离九宫】。</description>
        /// </item>
        /// <item>
        /// <description><b>后天映射：</b> 最终数字 1~9 严格映射后天八卦卦宫位置返回。</description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="d">公历出生时间，自动识别立春前后校正命年</param>
        /// <param name="Sex">性别【男/女】，区分不同命卦公式</param>
        /// <returns>校正并计算完成后的三元后天三爻命卦实例 <see cref="GuaSubClass"/></returns>
        public static GuaSubClass GetFateGua(DateTime d, string Sex)
        {
            int iy = d.Year;
            // 获取当前公历年份的“立春”节气精确交节时刻（包含具体时分秒）
            SolarTerm term = SolarTerm.FromName(d.Year, "立春");

            // 判断出生时刻是否已经跨过立春点。民俗命理以立春为岁首，未过立春则视作上一年出生
            FactYearEnum IsFact = d >= term.GetSolarDay().ToDateTime() ? FactYearEnum.SpringAfter : FactYearEnum.SpringBefore;//先判断是否为当年的立春后，如果是立春前，则用前一年计算命卦
            if (IsFact == FactYearEnum.SpringBefore) iy--;//如果是立春前，则用前一年计算命卦

            // 将最终确定的推演年份转化为字符数组，准备进行“年数求一”的累加操作
            char[] sdy = iy.ToString().ToArray();
            int ir = 0;

            //=====================================第一次计算=========================  .
            // 第一次求和：将四位年份的每一位数字拆开相加（例如：1998年 -> 1+9+9+8 = 27）
            for (int i = 0; i < sdy.Length; i++)//拆开单个数字计算
            {
                ir += int.Parse(sdy[i].ToString());
            }
            //=====================================第一次计算=========================

            sdy = ir.ToString().ToArray();
            // 循环连续求和：如果相加结果仍为两位数，则继续拆开数字相加，直至缩减为 1~9 的个位基数（例如：27 -> 2+7 = 9）
            while (sdy.Length > 1)
            {
                ir = 0;
                //=====================================加至个位数=========================  .
                for (int i = 0; i < sdy.Length; i++)//拆开单个数字计算
                {
                    ir += int.Parse(sdy[i].ToString());
                }
                //=====================================加至个位数=========================
                sdy = ir.ToString().ToArray();
            }

            // 根据“男逆女顺”原则，分别代入不同的九宫飞星飞泊公式进行宫位定向
            if (Sex == "男")
            {
                // 男命元旦盘逆行口诀。数学简式：用 11 减去上面求出的年份个位基数
                ir = 11 - ir; //男用11去减个位数
                sdy = ir.ToString().ToArray();

                // 对男命计算结果进行再次归一化判定（防止减法过后出现两位数，确保落入九宫数区间）
                while (sdy.Length > 1)
                {
                    ir = 0;
                    //=====================================加至个位数=========================  .
                    for (int i = 0; i < sdy.Length; i++)//拆开单个数字计算
                    {
                        ir += int.Parse(sdy[i].ToString());
                    }
                    //=====================================加至个位数=========================
                    sdy = ir.ToString().ToArray();
                }

                // 男命边界及寄宫特例处理
                if (ir == 0) ir = 1;//如果等0则为1（坎一宫）
                if (ir == 5) ir = 2;//五黄入中宫无对应卦，男命依照风水惯例寄宫于【坤二宫】
            }
            else //女 
            {
                // 女命元旦盘顺行口诀。数学简式：年份个位基数加 4 
                ir += 4;
                if (ir > 9)
                {
                    ir = ir - 9; // 超过九宫范围则扣减9，作九宫循环
                }

                // 女命边界及寄宫特例处理
                if (ir == 0) ir = 9;//女命，如果等0，则为9（离九宫）
                if (ir == 5) ir = 8;//五黄入中宫无对应卦，女命依照风水惯例寄宫于【艮八宫】
            }

            // 转换为底层八卦数组索引（九宫数 - 1），从映射表里获取最终对应的后天三爻命卦实例
            GuaSubClass gsc = GuaSubClass.GetAfterGuaSub(ir - 1);
            return gsc;
        }


        /// <summary>
        /// 【天机卦法】判定命卦是否为出卦
        /// </summary>
        /// <remarks>
        /// 比对逻辑：本命卦后天洛数 是否 匹配向卦对应的三个出卦三爻先天卦气数；
        /// 命中则为出卦（凶），未命中则为入卦（吉），自动写入吉凶消息。
        /// </remarks>
        /// <returns>true=出卦，false=入卦</returns>
        /// <exception cref="Exception">出卦集合无数据、数据异常时抛出</exception>
        private bool IsOutByFateGua()
        {
            if (this.OutGuaSubs.Any() == false) throw new Exception("出卦数据异常：" + nameof(OutGuaSubs));
            var ls = OutGuaSubs.Where(sg => this.ToGua.GuaQi.GuaQiNumber == sg.Value.AfterQuantity);
            if (ls.Any())
            {
                Infos.BadInfos.Add(new InfoType(false) { Info = "本命卦于向卦【" + this.ToGua.Name + "】中属于出卦" });
            }
            else
            {
                Infos.GoodInfos.Add(new InfoType(true) { Info = "本命卦于向卦【" + this.ToGua.Name + "】中属于入卦" });
            }
            return ls.Any();
        }

        /// <summary>
        /// 【京房纳甲法】二次判定是否出卦，并推演得福人、得福年份
        /// </summary>
        /// <remarks>
        /// 1. 提取本命出生年命庚干支；
        /// 2. 匹配出卦纳甲天干，命庚落在出卦天干则判定出卦；
        /// 3. 依据七世飞爻卦位、游魂归魂规则，自动计算入卦得福人群、得福流年；
        /// 4. 自动填充吉凶消息与对应干支年份数据。
        /// </remarks>
        /// <returns>true=纳甲出卦，false=入卦得福</returns>
        private bool IsOutByNaJia()
        {
            SolarTime st = this.BirthTime.ToSolarTime();
            string FateYearSLName = st.SolarDay.GetLunarDay().GetSixtyCycleDay().Year.GetName();//命庚干支
            this.FateYearSkyLoc = new SkyLoc(FateYearSLName);
            NaJiaJFResult JFR = NaJia<NaJiaJFResult>.CreateJF(ToGua);//向卦做京房纳甲

            //========先计算命庚天干是否存于出卦的纳甲里，如果存在则是出卦==========
            var outsnj = this.OutGuaSubs.Select(gs => NaJia<NaJiaJFResult>.CreateJF(new GuaClass(gs.Value.Name)));
            var outs = outsnj.Where(nj => nj.SkyLocs.Where(sl => sl.Sky.Name == FateYearSkyLoc.Sky.Name).Any());
            if (outs.Any())//如果存在出卦中的纳甲天干中，则是出卦
            {
                this.Infos.BadInfos.Add(new InfoType(false) { Info = "本命庚【" + FateYearSLName + "】于出卦：【" + outs.FirstOrDefault().Gua.Name + "】纳甲中【" + string.Join(",", outs.FirstOrDefault().SkyLocs.Select(sl => sl.Sky.Name).Distinct()) + "】，属于出卦" });
                return true;
            }
            //========先计算命庚天干是否存于出卦的纳甲里，如果存在则是出卦==========

            if (JFR.SkyLocs != null && JFR.SkyLocs.Any())
            {
                var rs = JFR.SkyLocs.Select(sl => sl.Sky.Name);//找到向卦的纳甲天干
                if (rs.Any())
                {
                    //==============找出卦宫的纲甲判断那年得福====================
                    int iPos = TJGua.GuaList.IndexOf(this.ToGua); //向卦在七世飞爻卦中的索引
                    if (iPos > -1)
                    {
                        if (iPos < TJGua.GuaList.Count() - 1) //第8个则是归魂卦，则不能判断得福年间
                        {
                            //获得卦宫的纳甲
                            var gselfNJ = NaJia<NaJiaJFResult>.CreateJF(this.ToGua.GuaSelf.ToGuaClass());
                            int iYao = iPos == 0 ? -1 : iPos - 1;//如果向卦在七世飞爻卦中的第一个卦则是为卦宫本身，则直接取宫的干支为得福年份-1表是卦宫,如乾卦，为甲年或壬年(只论年干)
                            if (iPos == 6) iYao = 3; //游魂卦等于变在第4爻
                            if (iYao == -1) //-1表是卦宫,如乾卦，为甲年或壬年(只论年干)
                            {
                                //=============判断得福人===============================
                                this.InGuaGoodSL = rs.Distinct().ToArray();
                                this.Infos.GoodInfos.Add(new InfoType(true) { Info = "命庚是【" + string.Join(",", this.InGuaGoodSL) + "】的人得福" });
                                //=============判断得福人===============================
                                this.InGuaGoodYear = gselfNJ.SkyLocs.Select(sl => sl.Sky.Name).Distinct().ToArray();
                                this.Infos.GoodInfos.Add(new InfoType(true) { Info = "【" + string.Join(",", InGuaGoodYear) + "】年得福" });

                            }
                            else//其余按干支论
                            {
                                //=============判断得福人===============================
                                this.InGuaGoodSL = [rs.ElementAt(iYao)];
                                this.Infos.GoodInfos.Add(new InfoType(true) { Info = "命庚是【" + string.Join(",", this.InGuaGoodSL) + "】的人得福" });
                                //=============判断得福人===============================

                                this.InGuaGoodYear = [gselfNJ.SkyLocs.ElementAt(iYao).SkyLocName];
                                this.Infos.GoodInfos.Add(new InfoType(true)
                                {
                                    Info = "【" + string.Join(",", InGuaGoodYear) + "】年得福"
                                });

                            }

                        }

                    }
                    //==============找出卦宫的纲甲判断那年得福====================

                }


            }
            else
            {
                return true;
            }

            return false;
        }


    }


}
