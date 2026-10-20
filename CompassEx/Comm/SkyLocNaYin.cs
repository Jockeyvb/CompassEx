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

using CompassEx.C3H;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompassEx.Comm;

public class SkyLocNaYin
{







    /// <summary>
    /// 一个纳音对应两个干支
    /// </summary>
    private readonly string[] NaYinNames = new string[30] {
            "海中金", "炉中火", "大林木", "路旁土", "剑锋金",
            "山头火", "涧下水", "城头土", "白腊金", "杨柳木",
            "泉中水", "屋上土", "霹雳火", "松柏木", "长流水",
            "砂中金", "山下火", "平地木", "壁上土", "金箔金",
            "覆灯火", "天河水", "大驿土", "钗钏金", "桑松木",
            "大溪水", "沙中土", "天上火", "石榴木", "大海水" };


    /// <summary>
    /// 盲派纳音（用于推算四柱的纳音文本，比如：1981年（辛酉年），庭江坎兑够倔（摘）兔，庭（辛酉年）江为甲寅（初一），
    /// 坎卦（上半年为坎卦（6个月）的月大月小，阳为月大，初爻正月小、二月大、三月小、四月小、五月大、六月小；兑（下半年）七月小、八月大、九月大、十月小、十一月大、十二月大。
    /// 够表示十二月大有年三十日，如果为缺则为十二月小无年三十日（主要用于排除当年有闰月两个卦的12爻不够用）
    /// 倔（摘）为：立春为癸丑日。
    /// 免则卯：立春为卯时。
    /// </summary>
    private readonly string[] NaYinNames1 = new string[30] {
            "猪肉金", "炉野火", "园林木", "露头土", "剑锋金",
            "磐急火", "简沟水", "城离土", "白腊金", "杨柳木",
            "井泉水", "屋瓦土", "雷电火", "松柏木", "长流水",
            "沙中金", "矛莹火", "涉雨木", "室壁土", "渡泊金",
            "福灯火", "河汗水", "易夬土", "娄串金", "深摘木",
            "江溪水", "盘泥土", "日月火", "花庭木", "沧海水" };

    /// <summary>
    /// 所在索引
    /// </summary>
    public int Index { get; init; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; init; }




    /// <summary>
    /// 所属五行
    /// </summary>
    public FiveAttr FiveAttr { get => new FiveAttr(this.Name.Substring(2, 1)); }



    public SkyLocNaYin(SkyLoc sl) : this(sl.Index / 2)
    {

    }


    [JsonConstructor]
    public SkyLocNaYin([JsonProperty(nameof(Index))] int index)
    {
        if (index < 0 || index >= NaYinNames.Length) throw new ArgumentOutOfRangeException(nameof(index));
        this.Index = index;
        this.Name = NaYinNames[index];
    }


}
