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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompassEx.Comm;

public class Star28 : IEquatable<Star28>
{
    /// <summary>
    /// 二十八星宿全称
    /// </summary>
    public readonly static string[] Names = new string[28] { "角木蛟", "亢金龙", "氐土貉", "房日兔", "心月狐", "尾火虎", "箕水豹", "斗木獬", "牛金牛", "女土蝠", "虚日鼠", "危月燕", "室火猪", "壁水獝", "奎木狼", "娄金狗", "胃土彘", "昴日鸡", "毕月鸟", "嘴火猴", "参水猿", "井木犴", "鬼金羊", "柳土獐", "星日马", "张月鹿", "翼火蛇", "轸水蚓" };
    /// <summary>
    /// 二十八星宿简称
    /// </summary>
    public readonly static string[] ShortNames = Names.Select(x => x.Substring(0, 1)).ToArray();

    /// <summary>
    /// 方位名称顺序
    /// </summary>
    public readonly static string[] DirectionNames = new string[4] { "东", "北", "西", "南" };

    private FiveAttr _FiveAttr = default!;
    /// <summary>
    /// 二十八宿五行
    /// </summary>
    public FiveAttr FiveAttr
    {
        get
        {
            if (_FiveAttr == null!)
            {

                if (this.Name.IndexOf("日") > -1 || this.Name.IndexOf("月") > -1)
                {
                    _FiveAttr = new FiveAttr("火");
                }
                else
                {
                    _FiveAttr = new FiveAttr(this.Name.Substring(1, 1));
                }
            }

            return _FiveAttr;

        }
    }
    /// <summary>
    /// 方位五行
    /// </summary>
    public FiveAttr DirectionFiveAttr { get => FiveAttr.GetDirectionFiveAttr(this.DirectionName); }
    /// <summary>
    /// 二十八宿方位全称
    /// </summary>
    public string DirectionFullName { get => this.DirectionName + "方七宿"; }
    /// <summary>
    /// 二十八宿方位名称
    /// </summary>
    public string DirectionName { get => DirectionNames[this.Index / 7]; }
    /// <summary>
    /// 二十八宿索引
    /// </summary>
    public int Index { get; init; }
    /// <summary>
    /// 二十八宿名称
    /// </summary>
    public string Name { get => Names[this.Index]; }
    /// <summary>
    /// 二十八宿简称
    /// </summary>
    public string ShortName { get => ShortNames[this.Index]; }



    public Star28(string shortName) : this(ShortNames.IndexOf(shortName))
    {

    }

    [JsonConstructor]
    public Star28([JsonProperty(nameof(Index))] int index)
    {
        if (index < 0 || index >= Names.Length) throw new ArgumentOutOfRangeException(nameof(index));

        this.Index = index;

    }


    public override string ToString()
    {

        return this.Name;
    }


    #region 显式实现对比、运算符和Key 方法
    // 1. 一般的 Equals(object)，內部可以轉型並利用顯式介面來比對
    public override bool Equals(object obj)
    {
        if (obj is FiveAttr other)
        {
            return ((IEquatable<Star28>)this).Equals(other);
        }
        return false;
    }

    // 2. 顯式實作 IEquatable<LocClass>.Equals
    bool IEquatable<Star28>.Equals(Star28 other)
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
    public static bool operator ==(Star28 left, Star28 right)
    {
        if (left is null) return right is null;
        return ((IEquatable<FiveAttr>)left).Equals(right);
    }

    public static bool operator !=(Star28 left, Star28 right)
    {
        return !(left == right);
    }


    #endregion
}
