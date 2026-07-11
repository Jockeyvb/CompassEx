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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CompassEx.Comm
{


    /// <summary>
    /// 消息/提示文本样式结构体，包含内容、文字颜色、背景色、字号、字重
    /// </summary>
    public class InfoType
    {
        /// <summary>展示文本内容</summary>
        public string Info { get; set; } = "";

        /// <summary>文字颜色</summary>
        public Color TextColor { get; set; } = Color.Black;

        /// <summary>背景填充颜色</summary>
        public Color BGcolor { get; set; } = Color.White;

        /// <summary>字体大小，单位px</summary>
        public float FontSize { get; set; } = 12f;

        /// <summary>字体粗细，如 bold / normal / lighter / bolder</summary>
        public string FontWeight { get; set; } = "normal";

        public InfoType()
        {
        }

        public InfoType(bool IsGood)
        {
            FontWeight = "bold";
            if (IsGood)
            {
                TextColor = Color.Green;

            }
            else
            {
                TextColor = Color.Red;
            }
        }

        /// <summary>
        /// 转换为完整 HTML 内联 style 字符串
        /// </summary>
        /// <returns>例如：font-weight:bold;font-size:14px;color:#ffffff;background:#222222;</returns>
        public string ToHtmlStyle()
        {
            var sb = new System.Text.StringBuilder();

            // 字重
            if (!string.IsNullOrWhiteSpace(FontWeight))
                sb.Append($"font-weight:{FontWeight};");

            // 字号
            sb.Append($"font-size:{FontSize}px;");

            // 文字颜色
            sb.Append($"color:{ColorToRgba(TextColor)};");

            // 背景色
            sb.Append($"background:{ColorToRgba(BGcolor)};");

            return sb.ToString();
        }

        /// <summary>
        /// 生成完整带样式的 HTML div 标签
        /// </summary>
        public string ToFullHtml(bool IsDiv = false)
        {
            string style = ToHtmlStyle();
            string text = System.Net.WebUtility.HtmlEncode(Info);
            string s = IsDiv ? $"<div style=\"{style}\">{text}</div>" : $"<span style=\"{style}\">{text}</span>";
            return s;
        }

        /// <summary>
        /// Color 转 CSS rgba 格式（兼容透明）
        /// </summary>
        private static string ColorToRgba(Color c)
        {
            byte r = (byte)(c.R * 255);
            byte g = (byte)(c.G * 255);
            byte b = (byte)(c.B * 255);
            double a = Math.Round((double)c.A, 2);
            return $"rgba({r},{g},{b},{a})";
        }

        public override string ToString()
        {
            return Info;
        }
    }


    /// <summary>
    /// 相关好与坏的信息
    /// </summary>
    public class GoodBadInfos
    {
        /// <summary>
        /// 好的信息
        /// </summary>
        public List<InfoType> GoodInfos { get; set; } = new List<InfoType>();



        /// <summary>
        /// 坏的信息
        /// </summary>
        public List<InfoType> BadInfos { get; set; } = new List<InfoType>();



        /// <summary>
        /// 普通信息
        /// </summary>
        public InfoType Info { get; set; } = new InfoType();

        public GoodBadInfos()
        {

        }

        public override string ToString()
        {
            return Info.ToString() + "\n" + GoodInfos.Select(gi => gi.ToString()) + "\n" + BadInfos.Select(bi => bi.ToString()) + "\n";
        }

        public string ToHTML(bool IsDiv = true)
        {
            return Info.ToFullHtml(IsDiv) + "\n" + GoodInfos.Select(gi => gi.ToFullHtml(IsDiv)) + "\n" + BadInfos.Select(bi => bi.ToFullHtml(IsDiv)) + "\n";
        }

    }

}
