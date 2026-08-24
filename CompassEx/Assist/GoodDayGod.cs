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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CompassEx.Assist
{
    /// <summary>
    /// 表示择日神煞吉凶类型的枚举。
    /// </summary>
    public enum GoodDayGodGoodType
    {
        /// <summary>
        /// 无特殊吉凶属性。
        /// </summary>
        [Description("无")]
        None = 0,

        /// <summary>
        /// 中性，无明确的大吉或大凶。
        /// </summary>
        [Description("中性")]
        Neutral = 1,

        /// <summary>
        /// 凶，代表不吉利或需要避忌的日子。
        /// </summary>
        [Description("凶")]
        Bad = 2,

        /// <summary>
        /// 吉，代表吉祥或适合进行特定活动的良辰。
        /// </summary>
        [Description("吉")]
        Good = 4,

        /// <summary>
        /// 重度，可与吉凶属性搭配使用以在前端展示时突出显示。
        /// </summary>
        [Description("重度")]
        Important = 1024,
        /// <summary>
        /// 特重，可与吉凶属性搭配使用以在前端展示时突出显示。
        /// </summary>
        [Description("特重")]
        Special = 2048


    }

    /// <summary>
    /// 表示择日神煞的类，用于管理和判断传统择日学中的各类吉凶神煞（如天赦、十灵、魁罡、十恶大败等）。
    /// </summary>
    public class GoodDayGod : IEquatable<GoodDayGod>
    {
        public static readonly string GuChenDaysInfo = "【孤辰】“三命会通：男命犯之，疏离六亲，他乡之客”：主男人漂泊、性格孤僻、难以得到家族或周围人的强力照拂，人际关系较冷清。";
        public static readonly Dictionary<string, string> GuChenDays = new Dictionary<string, string> { { "亥子丑", "寅" }, { "‌寅卯辰", "巳" }, { "‌巳午未", "申" }, { "‌‌申酉戌", "亥" } };


        public static readonly string GuaSuDaysInfo = "【寡宿】“三命会通：女命犯之，独房眠，夫缘薄”：主女性内心情感孤独、夫妻沟通有隔阂，或聚少离多、晚婚、独自操持。";
        public static readonly Dictionary<string, string> GuaSuDays = new Dictionary<string, string> { { "亥子丑", "戌" }, { "‌寅卯辰", "丑" }, { "‌巳午未", "辰" }, { "‌‌申酉戌", "未" } };


        public static readonly string FourLeaveDaysInfo = "【四离】古人认为，在节气正式交替的前夕（即四离日），旧的季节之气尚未完全退去，新的季节之气已经蓄势待发，阴阳之气处于一种极其紊乱、剧烈交替的“混乱过渡期”。因此，天地磁场不稳定，人体的气血也容易受影响。";

        public static readonly string FourEndDaysInfo = "【四绝】是指立春、立夏、立秋、立冬这四个“四立”（四季之首）节气的前一天。“绝”指的是“气候穷尽、五行绝气”。在五行生克中，当一个季节即将结束、另一个季节即将诞生时，前一个季节的五行之气在这一天衰减到了绝对的零界点（气绝）。古人认为“天地气绝”之时，万物生机受阻，因此诸事不宜。";

        /// <summary>
        /// 天医神煞的科普与说明文本。
        /// </summary>
        public static readonly string TianyiDaysInfo = "【天医】，意为“上天派来的医师”。在传统观念中，天医日是天地间生气最旺、专门针对祛病疗伤、调理身体最具有加持力的日子。";

        /// <summary>
        /// 重丧日的数据匹配规则数组（按月支与日干对应）。
        /// </summary>
        public static readonly string[] DoubleLoseDays = { "寅月甲", "卯月乙", "辰月戊", "已月丙", "午月丁", "未月己", "申月庚", "酉月辛", "戌月戊", "亥月壬", "子月癸", "丑月己" };

        /// <summary>
        /// 重丧日的神煞科普与说明文本。
        /// </summary>
        public static readonly string DoubleLoseDaysInfo = "【重丧】指的是在一场丧事前后，家中或者与死者紧密相连的亲属中，接二连三、在短时间内（近则百日内，远则一年左右）又有人相继去世的凶灾现象。故丧葬之用慎之。";

        /// <summary>
        /// 魁罡日的干支数组（庚辰、壬辰、庚戌、戊戌）。
        /// </summary>
        public static readonly string[] KuiGangDays = { "庚辰", "壬辰", "庚戌", "戊戌" };

        /// <summary>
        /// 魁罡日的神煞科普与说明文本。
        /// </summary>
        public static readonly string KuiGangDaysInfo = "【魁罡】是由北斗七星中的前四星（魁）和后四星（罡）引申出的神煞。在命理和择日中代表极刚、极阳、极有权威、果断。命带魁罡者通常性格刚烈、正直、聪明，但一生多坎坷、波折或暴发暴败。择日上一般忌动土、诉讼或女性逢之。若是壬辰日为福星贵人日，故此有壬骑龙背喜非常的说法。";

        /// <summary>
        /// 天赦日的干支数组（戊寅、甲午、戊申、甲子）。
        /// </summary>
        public static readonly string[] SkyPardonDays = { "戊寅", "甲午", "戊申", "甲子" };

        /// <summary>
        /// 天赦日的神煞科普与说明文本。
        /// </summary>
        public static readonly string SkyPardonDaysInfo = "【天赦】是传统择日学中威力最大、最灵验的极吉之日。相传在此日百事大吉，具有“赦免罪过、化解灾厄”的神奇功用。古人常用于祭祀、祈福、求嗣、解冤释结、结婚、修造、安葬等。";

        /// <summary>
        /// 四废日的季节干支组合数组。
        /// </summary>
        public static readonly string[] FourScrapDays = { "庚申,辛酉", "壬子,癸亥", "甲寅,乙卯", "丙午,丁巳" };

        /// <summary>
        /// 四废日的神煞科普与说明文本。
        /// </summary>
        public static readonly string FourScrapDaysInfo = "【四废】代表生发之气受阻、万物枯竭。在传统择日中，此日百事忌讳，主做事有始无终、劳而无功、诸事不顺，尤其忌求医、出师、嫁娶、动土。";

        /// <summary>
        /// 十恶大败日的年日对照规则数组。
        /// </summary>
        public static readonly string[] TenDefeatDays = { "庚戌=甲辰", "辛亥=乙巳", "壬寅=丙申", "癸巳=丁亥", "甲戌=庚辰", "甲辰=戊戌", "乙亥=辛巳", "乙未=己丑", "丙寅=壬申", "丁巳=癸亥" };

        /// <summary>
        /// 十恶大败日的神煞科普与说明文本。
        /// </summary>
        public static readonly string TenDefeatDaysInfo = "【十恶大败】是传统择日中的大凶日。所谓“十恶”指重罪，“大败”指精光、消减。古人认为这几天“仓库金银化为灰尘”，主钱财散尽、做事无成、花钱如流水。在择日学中，忌出兵、远行、求财、结婚、开市。";

        /// <summary>
        /// 十灵日的干支数组。
        /// </summary>
        public static readonly string[] TenSpiritDays = { "甲辰", "乙亥", "丙辰", "丁酉", "戊午", "庚戌", "庚寅", "辛亥", "壬寅", "癸未" };

        /// <summary>
        /// 十灵日的神煞科普与说明文本。
        /// </summary>
        public static readonly string TenSpiritDaysInfo = "【十灵】代表灵气所钟、头脑聪明、悟性极高、对玄学、艺术、医术或宗教有极强天分。在择日或命理中多主清贵、灵巧。";

        /// <summary>
        /// 获取与当前神煞吉凶相对应的颜色（吉为绿色，凶为红色，中性及其他为蓝色）。
        /// </summary>
        /// <value>返回一个 <see cref="Color"/> 对象。</value>
        public Color Color
        {
            get
            {
                Color c = Color.Black;
                if (this.GoodType.HasFlag(GoodDayGodGoodType.Bad))
                {
                    c = Color.Red;
                }
                else if (this.GoodType.HasFlag(GoodDayGodGoodType.Good))
                {
                    c = Color.Green;
                }
                else
                {
                    c = Color.Blue;
                }
                return c;
            }
        }

        /// <summary>
        /// 获取神煞的名称（例如："天赦"、"十灵"等）。
        /// </summary>
        /// <value>神煞名称字符串，可能为 <c>null</c>。</value>
        public string? Name { get; private set; }

        /// <summary>
        /// 获取神煞的吉凶属性标记。
        /// </summary>
        /// <value>返回 <see cref="GoodDayGodGoodType"/> 枚举值。</value>
        public GoodDayGodGoodType GoodType { get; private set; }

        /// <summary>
        /// 获取该神煞的详细说明文案。
        /// </summary>
        /// <value>神煞说明字符串，可能为 <c>null</c>。</value>
        public string? Info { get; private set; }

        /// <summary>
        /// 私有构造函数，用于内部根据四柱类型初始化实例。
        /// </summary>
        /// <param name="fslt">四柱类型对象 <see cref="FourSkyLocType"/>。</param>
        private GoodDayGod(FourSkyLocType fslt)
        {
        }

        /// <summary>
        /// 格式化输出神煞名称的风格内容。
        /// </summary>
        /// <param name="IsShortName">是否缩写（为 <c>true</c> 时仅截取名称的第二个字）。</param>
        /// <param name="HTMLStyle">是否套用带颜色的 HTML 标签格式。</param>
        /// <param name="AddNameStr">可以在名称后面增加字符（如：日）</param>
        /// <returns>返回格式化后的字符串。</returns>
        public string ToString(bool IsShortName = false, bool HTMLStyle = false, string AddNameStr = "")
        {
            string st = this.Name + AddNameStr;
            if (IsShortName && !string.IsNullOrEmpty(st) && st.Length >= 2)
                st = st.Substring(1, 1); // 取第二个字

            if (HTMLStyle)
            {
                st = $"<span style='{"color:" + this.Color.ToHex()}; {(this.GoodType.HasFlag(GoodDayGodGoodType.Important) ? "font-weight:bold;" : "")} ' >{st}</span>";
            }
            return st;
        }

        /// <summary>
        /// 返回当前神煞的名称字符串。
        /// </summary>
        /// <returns>神煞名称。</returns>
        public override string ToString()
        {
            return Name ?? string.Empty;
        }

        /// <summary>
        /// 根据公历时间获取当天所有的择日神煞列表。
        /// </summary>
        /// <param name="d">公历时间 <see cref="DateTime"/>。</param>
        /// <returns>包含匹配的 <see cref="GoodDayGod"/> 实例的列表。</returns>
        public static List<GoodDayGod> GetAllGods(DateTime d)
        {
            return GetAllGods(d.ToFourSkyLocType());
        }

        /// <summary>
        /// 根据四柱类型获取对应的所有择日神煞列表，并按吉凶比重降序排列。
        /// </summary>
        /// <param name="fslt">四柱类对象 <see cref="FourSkyLocType"/>。</param>
        /// <returns>包含匹配的 <see cref="GoodDayGod"/> 实例的列表。</returns>
        public static List<GoodDayGod> GetAllGods(FourSkyLocType fslt)
        {
            List<GoodDayGod> ls = new List<GoodDayGod>();
            if (IsSkyPardonDay(fslt))
            {
                GoodDayGod gdg = new GoodDayGod(fslt) { Name = "天赦", GoodType = GoodDayGodGoodType.Good | GoodDayGodGoodType.Important, Info = SkyPardonDaysInfo };
                ls.Add(gdg);
            }

            if (IsTenSpiritDay(fslt))
            {
                GoodDayGod gdg = new GoodDayGod(fslt) { Name = "十灵", GoodType = GoodDayGodGoodType.Good, Info = TenSpiritDaysInfo };
                ls.Add(gdg);
            }

            if (IsKuiGangDay(fslt))
            {
                GoodDayGod gdg = new GoodDayGod(fslt) { Name = "魁罡", GoodType = GoodDayGodGoodType.Neutral | GoodDayGodGoodType.Important, Info = KuiGangDaysInfo };
                ls.Add(gdg);
            }

            if (IsTenDefeatDay(fslt))
            {
                GoodDayGod gdg = new GoodDayGod(fslt) { Name = "十恶大败", GoodType = GoodDayGodGoodType.Bad | GoodDayGodGoodType.Important, Info = TenDefeatDaysInfo };
                ls.Add(gdg);
            }

            if (IsFourScrapDay(fslt))
            {
                GoodDayGod gdg = new GoodDayGod(fslt) { Name = "四废", GoodType = GoodDayGodGoodType.Bad, Info = FourScrapDaysInfo };
                ls.Add(gdg);
            }

            if (IsDoubleLoseDay(fslt))
            {
                GoodDayGod gdg = new GoodDayGod(fslt) { Name = "重丧", GoodType = GoodDayGodGoodType.Bad | GoodDayGodGoodType.Important | GoodDayGodGoodType.Special, Info = DoubleLoseDaysInfo };
                ls.Add(gdg);
            }

            if (IsTianyiDay(fslt))
            {
                GoodDayGod gdg = new GoodDayGod(fslt) { Name = "天医", GoodType = GoodDayGodGoodType.Good, Info = TianyiDaysInfo };
                ls.Add(gdg);
            }

            if (IsFourLeaveDay(fslt))
            {
                GoodDayGod gdg = new GoodDayGod(fslt) { Name = "四离", GoodType = GoodDayGodGoodType.Bad | GoodDayGodGoodType.Important | GoodDayGodGoodType.Special, Info = FourLeaveDaysInfo };
                ls.Add(gdg);
            }
            if (IsFourEndDay(fslt))
            {
                GoodDayGod gdg = new GoodDayGod(fslt) { Name = "四绝", GoodType = GoodDayGodGoodType.Bad | GoodDayGodGoodType.Important | GoodDayGodGoodType.Special, Info = FourEndDaysInfo };
                ls.Add(gdg);
            }
            if (IsGuChenDay(fslt))
            {
                GoodDayGod gdg = new GoodDayGod(fslt) { Name = "孤辰", GoodType = GoodDayGodGoodType.Bad, Info = GuChenDaysInfo };
                ls.Add(gdg);
            }
            if (IsGuaSuDay(fslt))
            {
                GoodDayGod gdg = new GoodDayGod(fslt) { Name = "寡宿", GoodType = GoodDayGodGoodType.Bad, Info = GuaSuDaysInfo };
                ls.Add(gdg);
            }


            ls = ls.OrderByDescending(x => x.GoodType).ToList(); // 按吉凶比重来排序

            return ls;
        }

        /// <summary>
        /// 孤辰日
        /// </summary>
        /// <param name="FSLT"></param>
        /// <returns></returns>
        public static bool IsGuChenDay(FourSkyLocType FSLT)
        {
            int iCount = GuChenDays.Count(x => x.Key.IndexOf(FSLT.YearSL.Loc.Name) > -1 && x.Value.Equals(FSLT.DaySL.Loc.Name));
            return iCount > 0;
        }
        /// <summary>
        /// 寡宿
        /// </summary>
        /// <param name="FSLT"></param>
        /// <returns></returns>
        public static bool IsGuaSuDay(FourSkyLocType FSLT)
        {
            int iCount = GuaSuDays.Count(x => x.Key.IndexOf(FSLT.YearSL.Loc.Name) > -1 && x.Value.Equals(FSLT.DaySL.Loc.Name));
            return iCount > 0;
        }


        /// <summary>
        /// 是否为四绝日
        /// </summary>
        /// <param name="FSLT"></param>
        /// <returns></returns>
        public static bool IsFourEndDay(FourSkyLocType FSLT)
        {
            var sd = FSLT.Date.AddDays(1).ToSolarTime();//向前加一日
            string sn = sd.SolarDay.Term.GetName();
            return sd.SolarDay.TermDay.DayIndex == 0 && (sn.IndexOf("立") > -1);//在索引0时表示当日是节气，两立两至则是四绝日


        }


        /// <summary>
        /// 是否为四离日
        /// </summary>
        /// <param name="FSLT"></param>
        /// <returns></returns>
        public static bool IsFourLeaveDay(FourSkyLocType FSLT)
        {
            var sd = FSLT.Date.AddDays(1).ToSolarTime();//向前加一日
            string sn = sd.SolarDay.Term.GetName();
            return sd.SolarDay.TermDay.DayIndex == 0 && (sn.IndexOf("至") > -1 || sn.IndexOf("分") > -1);//在索引0时表示当日是节气，两至两分则是四离日


        }

        /// <summary>
        /// 判断指定的四柱类型是否为天医日。
        /// </summary>
        /// <param name="FSLT">四柱类对象 <see cref="FourSkyLocType"/>。</param>
        /// <returns>如果是天医日则返回 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool IsTianyiDay(FourSkyLocType FSLT)
        {
            return FSLT.DayBuildName == "闭"; // 十二日建的闭日是天医日
        }

        /// <summary>
        /// 判断指定的四柱类型是否为重丧日。
        /// </summary>
        /// <param name="FSLT">四柱类对象 <see cref="FourSkyLocType"/>。</param>
        /// <returns>如果是重丧日则返回 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool IsDoubleLoseDay(FourSkyLocType FSLT)
        {
            foreach (string s in DoubleLoseDays)
            {
                string[] sd = s.Split('月');
                if (FSLT.MonthSL.Loc.Name.Equals(sd[0])) // 月支
                {
                    return FSLT.DaySL.Sky.Name.Equals(sd[1]); // 日干
                }
            }

            return false;
        }

        /// <summary>
        /// 判断指定的四柱类型是否为魁罡日。
        /// </summary>
        /// <param name="FSLT">四柱类对象 <see cref="FourSkyLocType"/>。</param>
        /// <returns>如果是魁罡日则返回 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool IsKuiGangDay(FourSkyLocType FSLT)
        {
            return KuiGangDays.IndexOf(FSLT.DaySL.Name) > -1;
        }

        /// <summary>
        /// 判断指定的四柱类型是否为十灵日。
        /// </summary>
        /// <param name="FSLT">四柱类对象 <see cref="FourSkyLocType"/>。</param>
        /// <returns>如果是十灵日则返回 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool IsTenSpiritDay(FourSkyLocType FSLT)
        {
            return TenSpiritDays.IndexOf(FSLT.DaySL.Name) > -1;
        }

        /// <summary>
        /// 判断指定的公历时间是否为十恶大败日。
        /// </summary>
        /// <param name="d">公历时间 <see cref="DateTime"/>。</param>
        /// <returns>如果是十恶大败日则返回 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool IsTenDefeatDay(DateTime d)
        {
            var FSLT = d.ToFourSkyLocType();
            return IsTenDefeatDay(FSLT);
        }

        /// <summary>
        /// 判断指定的四柱类型是否为十恶大败日。
        /// </summary>
        /// <param name="FSLT">四柱类对象 <see cref="FourSkyLocType"/>。</param>
        /// <returns>如果是十恶大败日则返回 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool IsTenDefeatDay(FourSkyLocType FSLT)
        {
            foreach (string s in TenDefeatDays)
            {
                string[] sd = s.Split('=');
                if (FSLT.YearSL.Name.Equals(sd[0])) // 年
                {
                    return FSLT.DaySL.Name.Equals(sd[1]);
                }
            }

            return false;
        }

        /// <summary>
        /// 判断指定的公历时间是否为四废日。
        /// </summary>
        /// <param name="d">公历时间 <see cref="DateTime"/>。</param>
        /// <returns>如果是四废日则返回 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool IsFourScrapDay(DateTime d)
        {
            var FSLT = d.ToFourSkyLocType();
            return IsFourScrapDay(FSLT);
        }

        /// <summary>
        /// 判断指定的四柱类型是否为四废日。
        /// </summary>
        /// <param name="FSLT">四柱类对象 <see cref="FourSkyLocType"/>。</param>
        /// <returns>如果是四废日则返回 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool IsFourScrapDay(FourSkyLocType FSLT)
        {
            int iPos = LocClass.LocNames.IndexOf(FSLT.MonthSL.Loc.Name);
            if (iPos >= 2 && iPos <= 4) // 春
            {
                return FourScrapDays[0].IndexOf(FSLT.DaySL.Name) > -1;
            }
            else if (iPos >= 5 && iPos <= 7) // 夏
            {
                return FourScrapDays[1].IndexOf(FSLT.DaySL.Name) > -1;
            }
            else if (iPos >= 8 && iPos <= 10) // 秋
            {
                return FourScrapDays[2].IndexOf(FSLT.DaySL.Name) > -1;
            }
            else // 冬
            {
                return FourScrapDays[3].IndexOf(FSLT.DaySL.Name) > -1;
            }
        }

        /// <summary>
        /// 判断指定的四柱类型是否为天赦日。
        /// </summary>
        /// <param name="FSLT">四柱类对象 <see cref="FourSkyLocType"/>。</param>
        /// <returns>如果是天赦日则返回 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool IsSkyPardonDay(FourSkyLocType FSLT)
        {
            int iPos = LocClass.LocNames.IndexOf(FSLT.MonthSL.Loc.Name);
            if (iPos >= 2 && iPos <= 4) // 春
            {
                return FSLT.DaySL.Name == SkyPardonDays[0];
            }
            else if (iPos >= 5 && iPos <= 7) // 夏
            {
                return FSLT.DaySL.Name == SkyPardonDays[1];
            }
            else if (iPos >= 8 && iPos <= 10) // 秋
            {
                return FSLT.DaySL.Name == SkyPardonDays[2];
            }
            else // 冬
            {
                return FSLT.DaySL.Name == SkyPardonDays[3];
            }
        }

        /// <summary>
        /// 判断指定的公历时间是否为天赦日。
        /// </summary>
        /// <param name="d">公历时间 <see cref="DateTime"/>。</param>
        /// <returns>如果是天赦日则返回 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool IsSkyPardonDay(DateTime d)
        {
            var FSLT = d.ToFourSkyLocType();
            return IsSkyPardonDay(FSLT);
        }


        #region 显式实现对比、运算符和Key 方法
        // 1. 一般的 Equals(object)，內部可以轉型並利用顯式介面來比對
        public override bool Equals(object obj)
        {
            return Equals(obj as GoodDayGod);
        }

        // 2. 顯式實作 IEquatable<LocClass>.Equals
        bool IEquatable<GoodDayGod>.Equals(GoodDayGod other)
        {
            // 檢查是否為 null
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            // 使用 string 的比較方式（考慮大小寫或 null 的防禦）
            return string.Equals(this.Name, other.Name, StringComparison.Ordinal);
        }

        // 3. 務必配合 Name 計算 HashCode
        public override int GetHashCode()
        {
            // 若 Name 可能為 null，可以用 HashCode.Combine 或字串自身的 GetHashCode
            return Name != null ? Name.GetHashCode() : 0;
        }

        // 4. (選用) 重載 == 與 != 運算子，建議透過介面轉型來呼叫
        public static bool operator ==(GoodDayGod left, GoodDayGod right)
        {
            if (left is null) return right is null;
            return ((IEquatable<GoodDayGod>)left).Equals(right);
        }

        public static bool operator !=(GoodDayGod left, GoodDayGod right)
        {
            return !(left == right);
        }


        #endregion
    }
}
