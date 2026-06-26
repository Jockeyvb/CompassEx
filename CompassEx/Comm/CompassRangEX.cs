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

namespace CompassEx.Comm
{
    public class CompassRangEX
    {
        public double Start { get; set; }
        public double End { get; set; }

        public CompassRangEX(double start, double end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// 是否在范围内，已考虑了跨越界限的情况（例如360度） 
        /// </summary>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public bool IsInRange(double dValue)
        {


            dValue = (dValue % 360 + 360) % 360;
            if (this.End > this.Start)//如果结束值大于开始值，正常范围
            {
                return dValue >= this.Start && dValue < this.End;
            }
            else//如果结束值小于开始值，说明范围跨越了某个界限（例如360度），需要特殊处理
            {
                return dValue >= this.Start || dValue < this.End;
            }


        }

        /// <summary>
        /// 在圆中取得距离值 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double AngleRangeValue(double start, double end)
        {
            // 差值模360
            double diff = (end - start) % 360;
            // 修正负数模
            if (diff < 0) diff += 360;
            // 取短弧
            return diff > 180 ? 360 - diff : diff;
        }
        /// <summary>
        /// 在圆中取得距离值 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public double AngleRangeValue()
        {
            return AngleRangeValue(this.Start, this.End);
        }

    }
}
