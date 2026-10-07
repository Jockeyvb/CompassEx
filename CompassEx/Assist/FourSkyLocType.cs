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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using tyme.lunar;
using tyme.sixtycycle;
using tyme.solar;
namespace CompassEx.Assist;


/// <summary>
/// 扩展
/// </summary>
public static class FSLTEx
{




    /// <summary>
    /// 根据四柱获得时间(反推)
    /// </summary>
    /// <param name="SkyLocs">四柱数据</param>
    /// <param name="iEndYear">从推年份起（逆向），默认为0时，则会默认当前年份</param>
    /// <returns></returns>

    public static DateTime FindTimeByFSLT(string[] SkyLocs, int iEndYear = 0)
    {
        var st = SkyLocs;



        DateTime dt = DateTime.MinValue;

        if (iEndYear == 0) iEndYear = DateTime.Now.Year;

        if (st.Length == 0) return DateTime.MinValue;

        if (st.Length == 2) st = ["", st[0], st[1], ""]; //只有月柱和日柱时，补充空年柱和空时柱

        SixtyCycleYear cy = null;
        SixtyCycleMonth cm = null;
        SixtyCycleDay cd = null;
        SixtyCycleHour ch = null;
        if (st.Length == 4) //四柱完整或月柱和日柱
        {
            while (iEndYear > 1900)
            {
                cy = new SixtyCycleYear(iEndYear);//年柱
                if (cy.SixtyCycle.GetName() == st[0] || string.IsNullOrWhiteSpace(st[0]))
                {
                    foreach (SixtyCycleMonth m in cy.Months)
                    {
                        string mName = m.GetName().Replace("月", "");

                        if (mName == st[1])
                        {
                            cm = m;
                            break;
                        }
                    }
                    if (cm != null)//月柱找到则找日柱
                    {
                        foreach (SixtyCycleDay d in cm.Days)
                        {
                            string sN = d.GetName().Replace("日", "");
                            if (sN == st[2])
                            {
                                cd = d;
                                break;
                            }
                        }
                    }
                    if (cd != null)
                    {
                        foreach (var h in cd.Hours)
                        {
                            string sN = h.GetName().Replace("时", "");
                            if (sN == st[3] || string.IsNullOrWhiteSpace(st[3]))
                            {
                                ch = h;
                                return h.SolarTime.ToDateTime();

                            }
                        }
                    }

                }

                iEndYear--;//如果月柱不匹配，则继续往前推年份
            }

        }


        return dt;
    }
    /// <summary>
    /// 转换成FourSkyLocType类
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    public static FourSkyLocType? ToFourSkyLocType(this DateTime d)
    {
        if (d.Equals(DateTime.MinValue)) return null;
        var ls = d.ToLunar();
        var sh = d.ToSolarTime();
        var sls = d.ToSkyLocs();

        FourSkyLocType fslt = new FourSkyLocType() { Lunar = ls, YearSL = new SkyLoc(sls.Year.Name), Date = d, MonthSL = new SkyLoc(sls.Month.Name), DaySL = new SkyLoc(sls.Day.Name), HourSL = new SkyLoc(sls.Hour.Name), FullName = ls.Hour.TofourSkyLocString(), YearCNName = ls.Year.Year.ToCNName(), MonthCNName = ls.Month.GetName().Replace("月", ""), DayCNName = ls.Day.GetName(), FullCNName = d.ToFullCNName() };
        SolarDay sod = d.ToSolarDay();
        var terms = SolarTerm.Names.Where(x => sod.Term.GetName() == x && sod == sod.Term.GetSolarDay()); //当天才附值 

        fslt.SeasonName = terms.FirstOrDefault(); //节气
        if (fslt.SeasonName != null && string.IsNullOrWhiteSpace(fslt.SeasonName) == false) fslt.SeasonTime = sh.SolarDay.TermDay.SolarTerm.GetTermTime().ToDateTime();//节气时间
        fslt.DayBuildName = ls.Day.Duty.GetName();//十二日建
        var sch = sh.GetSixtyCycleHour();
        var scd = sch.SixtyCycleDay;
        var scm = scd.SixtyCycleMonth;
        var scy = scm.SixtyCycleYear;

        fslt.SixtyCycle = (scy, scm, scd, sch);

        return fslt;
    }


}


public class FourSkyLocType
{


    /// <summary>
    /// 寻找下个农历日
    /// </summary>
    /// <returns></returns>
    public DateTime GetNextCNDay(int? AddYears = default, int LessDays = 0, TimeSpan TriggerTime = default)
    {
        int iy = AddYears == default ? DateTime.Now.Year - this.Lunar.y.Year : AddYears.Value;

        var h = new tyme.lunar.LunarHour(this.Lunar.y.Year + iy, Lunar.m.Month, Lunar.d.Day, Lunar.h.Hour, 0, 0);

        var WillSetTime = h.GetSolarTime().ToDateTime();

        WillSetTime = WillSetTime.AddDays(-LessDays);//提前
        WillSetTime = WillSetTime.Date.AddHours(TriggerTime.Hours).AddMinutes(TriggerTime.Minutes);


        if (WillSetTime < DateTime.Now) //如果本年时间已过自动设置为明年
        {

            WillSetTime = GetNextCNDay(iy + 1, -LessDays, TriggerTime);//增加一年
        }


        return WillSetTime;
    }

    /// <summary>
    /// 不外部调用
    /// </summary>
    internal FourSkyLocType()
    {

    }


    // 反序列化“后”执行的方法
    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {

        try
        {
            var a = this.Date.ToFourSkyLocType();
            this.ApplyBaseProperties(a);
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.ToString());
        }

        // 可以在此初始化默认值，此时 JSON 数据尚未赋值给属性
    }


    /// <summary>
    /// 年、月、日、时干支数组
    /// </summary>
    public SkyLoc[] SkyLocs { get => [this.YearSL, this.MonthSL, this.DaySL, this.HourSL]; }


    /// <summary>
    /// 星期1为0，星期日为6
    /// </summary>
    public int DayWeekIndex
    {
        get
        {
            int index = (int)Date.DayOfWeek - 1;
            if (index < 0) index = 6;
            return index;
        }
    }

    /// <summary>
    /// 返回农历对象
    /// </summary>
    [JsonIgnore]
    public (LunarYear y, LunarMonth m, LunarDay d, LunarHour h) Lunar { get; internal set; } = default!;

    /// <summary>
    /// tyme 库的干支类对象
    /// </summary>
    [JsonIgnore]
    public (SixtyCycleYear y, SixtyCycleMonth m, SixtyCycleDay d, SixtyCycleHour h) SixtyCycle { get; internal set; } = default!;


    /// <summary>
    /// 年干支
    /// </summary>
    public SkyLoc YearSL { get; set; }



    /// <summary>
    /// 月干支
    /// </summary>
    public SkyLoc MonthSL { get; set; }

    /// <summary>
    /// 日干支
    /// </summary>
    public SkyLoc DaySL { get; set; }



    /// <summary>
    /// 时干支
    /// </summary>
    public SkyLoc HourSL { get; set; }

    /// <summary>
    /// 四柱全名
    /// </summary>
    public string FullName { get; set; } = "";//四柱全名
    /// <summary>
    /// 农历月份名
    /// </summary>
    public string MonthCNName { get; set; } = "";//农历月份名
    /// <summary>
    /// 农历年份名
    /// </summary>
    public string YearCNName { get; set; } = "";//农历年份名
    /// <summary>
    /// 农历的日名
    /// </summary>
    public string DayCNName { get; set; } = "";//农历的日名
    /// <summary>
    /// 农历全称【一九八一年九月初二】
    /// </summary>
    public string FullCNName { get; set; } = "";//农历全称

    /// <summary>
    /// 获得农历格式【1981-九-二】
    /// </summary>
    public string YMDCNName { get { return $"{this.Lunar.y.Year}-{this.MonthCNName}-{this.DayCNName}"; } }

    /// <summary>
    /// 如果当天是交节，那么补上节气DateTime
    /// </summary>
    public DateTime SeasonTime { get; set; } = DateTime.MinValue;

    /// <summary>
    /// 如果当天是交节，那么补上节气名称
    /// </summary>
    public string SeasonName { get; set; } = "";//如果当天是交节，那么补上节气名称
    /// <summary>
    /// 公历日期对象
    /// </summary>
    public DateTime Date { get; set; } = DateTime.MinValue;//公历日期对象
    /// <summary>
    /// 十二日建
    /// </summary>
    public string DayBuildName { get; set; } = "";

    /// <summary>
    /// 获得当前日期年份在立春前或后，如果是立春前，则用前一年计算命卦
    /// </summary>
    public FactYearEnum FactYear { get => CommCls.GetFactYear(this.Date); }

    /// <summary>
    /// 所有择日神煞
    /// </summary>
    [JsonIgnore]
    public List<GoodDayGod> Gods { get { if (_Gods == null) _Gods = GoodDayGod.GetAllGods(this); return _Gods; } }
    private List<GoodDayGod> _Gods;



}

