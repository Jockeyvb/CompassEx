using CompassEx;
using CompassEx.Comm;
using CompassEx.Gua;
using SkiaSharp;
using System.Diagnostics;
namespace CompassExTest.Pages;

/// <summary>
/// SkiaSharp 4.148.0 专用罗盘绘制器
/// </summary>
public class CompassSkiaRenderer
{
    public SKCanvas Canvas;
    public SKSize CanvasSize;

    public double Rotation { get; set; } = 0;

    private float BaseRadius = 0;
    private const float BaseBorderWidth = 2;

    //private readonly SKTypeface TF = SKTypeface.FromFamilyName(
    //    "Microsoft YaHei",
    //    SKFontStyleWeight.Normal,
    //    SKFontStyleWidth.Normal,
    //    SKFontStyleSlant.Upright);


    private static SKTypeface TF;

    private SKTypeface TFSymbol;


    // 恢复 guaTextPaint
    private SKPaint guaTextPaint = new SKPaint
    {
        Color = SKColors.Black,
        IsAntialias = true
    };
    public struct DrawContextStru
    {
        public DrawContextStru() { }
        public float CenterSpace;
        public float InnerRadius;
        public float OuterRadius;
        public CompassRangEX CR = null;
        public SKPath Path = null;
        public SKColor ForceColor = SKColors.Black;
        public SKColor BGColor = SKColors.Transparent;
        public float AddTextSize = 0;
    }

    // 缓存所有卦扇区路径，用于绘制 + 点击命中检测
    public List<(string Name, SKPath Sector, CompassRangEX Range)> GuaSectorCache { get; private set; } = new();


    private SKTypeface _chineseTypeface;

    public CompassSkiaRenderer()
    {
        // 💡 因为复制过去后它会保持原有的文件夹层级，所以需要加上文件夹路径
        // 🚀 核心门道：利用 MAUI 的跨平台统一文件系统
        // 注意：这里只需传入纯文件名，不需要带上 "Resources/Fonts/" 路径前缀
        using Stream fontStream = FileSystem.OpenAppPackageFileAsync("NotoSansSymbols-Regular.ttf").GetAwaiter().GetResult();

        if (fontStream != null)
        {
            // 🎨 SkiaSharp 直接从内存流中安全反灌并装配字体
            TFSymbol = SKTypeface.FromStream(fontStream);
        }


        using Stream fs = FileSystem.OpenAppPackageFileAsync("SIMHEI.TTF").GetAwaiter().GetResult();

        if (fs != null)
        {
            // 🎨 SkiaSharp 直接从内存流中安全反灌并装配字体
            TF = SKTypeface.FromStream(fs);
        }





    }

    /// <summary>
    /// 入口绘制方法
    /// </summary>
    public void Render(SKCanvas canvas = null, SKSize? canvasSize = null)
    {


        if (canvasSize != null) this.CanvasSize = canvasSize.Value;
        if (canvas != null) this.Canvas = canvas;

        if (this.CanvasSize.Equals(SKSize.Empty) || this.Canvas == null) return;

        float centerX = this.CanvasSize.Width / 2f;
        float centerY = this.CanvasSize.Height / 2f;
        float radius = Math.Min(centerX, centerY) - 20;
        if (radius <= 0) return;

        BaseRadius = radius;
        this.Canvas.Clear(SKColors.White);
        this.Canvas.Save();

        // 全局旋转（罗盘）
        float visualRotation = 360 - (float)Rotation;
        this.Canvas.RotateDegrees(visualRotation, centerX, centerY);

        // 1. 最外圈
        using var circlePaint = new SKPaint
        {
            Color = SKColors.DarkSlateGray,
            StrokeWidth = 3,
            IsStroke = true,
            IsAntialias = true
        };
        this.Canvas.DrawCircle(centerX, centerY, radius, circlePaint);

        // 2. 刻度 + 数字
        DrawDegreeTicks(this.Canvas, centerX, centerY, radius);

        // 3. 四正方位文字
        DrawDirText(this.Canvas, centerX, centerY, radius);

        // 4. 八卦扇区
        GuaSectorCache.Clear();
        DrawAll(this.Canvas);

        this.Canvas.Restore();

        // 中心固定指针（不受旋转影响）
        using var lineRed = new SKPaint
        {
            Color = SKColors.Red.WithAlpha(80),
            StrokeWidth = 4,
            IsAntialias = true
        };
        this.Canvas.DrawLine(centerX, centerY + radius - 15, centerX, centerY - radius + 15, lineRed);

        using var dotRed = new SKPaint
        {
            Color = SKColors.Red,
            IsAntialias = true
        };
        this.Canvas.DrawCircle(centerX, centerY, 6, dotRed);
    }

    #region 刻度绘制
    private void DrawDegreeTicks(SKCanvas canvas, float cx, float cy, float r)
    {
        using var tickNormal = new SKPaint { Color = SKColors.DimGray, StrokeWidth = 2, IsAntialias = true };
        using var tickBold = new SKPaint { Color = SKColors.Red, StrokeWidth = 2, IsAntialias = true };

        using var textNormalPaint = new SKPaint { Color = SKColors.DimGray, IsAntialias = true };
        using var textBoldPaint = new SKPaint { Color = SKColors.Red, IsAntialias = true };

        float minSize = Math.Min(CanvasSize.Width, CanvasSize.Height);

        float RoundH = minSize / 15f;


        using var fontNormal = new SKFont(TF, RoundH * .2f);
        using var fontBold = new SKFont(TF, RoundH * 0.2f) { Embolden = true };

        for (int angle = 0; angle < 360; angle += 5)
        {
            canvas.Save();
            canvas.RotateDegrees(angle, cx, cy);

            bool isMajor = angle % 45 == 0;
            float lineLen = isMajor ? 15 : 10;

            var tickPaint = isMajor ? tickBold : tickNormal;
            var txtPaint = isMajor ? textBoldPaint : textNormalPaint;
            var font = isMajor ? fontBold : fontNormal;

            // 刻度线
            canvas.DrawLine(cx, cy - r, cx, cy - r + lineLen, tickPaint);

            // 角度文字
            string txt = angle.ToString();
            using var blob = SKTextBlob.Create(txt, font);
            SKRect bounds = blob.Bounds;
            float tx = cx - bounds.Left - bounds.Width / 2 + 2.5f;
            float ty = cy - r - (bounds.Top - bounds.Height) + 5f;

            canvas.DrawText(blob, tx, ty, txtPaint);

            canvas.Restore();
        }
    }
    #endregion

    #region 四正方位文字
    private void DrawDirText(SKCanvas canvas, float cx, float cy, float r)
    {
        using var dirTextPaint = new SKPaint
        {
            Color = SKColors.Crimson,
            IsAntialias = true
        };




        float minSize = Math.Min(CanvasSize.Width, CanvasSize.Height);

        float RoundH = minSize / 15f;
        float textOffset = RoundH / 2;
        float dirR = r + textOffset;
        using var dirFont = new SKFont(TF, RoundH * 0.5f) { Embolden = true };
        DrawSingleDirText(canvas, cx, cy, dirR, 0, "北", dirTextPaint, dirFont);
        DrawSingleDirText(canvas, cx, cy, dirR, 90, "东", dirTextPaint, dirFont);
        DrawSingleDirText(canvas, cx, cy, dirR, 180, "南", dirTextPaint, dirFont);
        DrawSingleDirText(canvas, cx, cy, dirR, 270, "西", dirTextPaint, dirFont);
    }

    private void DrawSingleDirText(SKCanvas canvas, float cx, float cy, float r, float deg, string text, SKPaint paint, SKFont font)
    {
        SKPoint pt = GetArcPoint(cx, cy, r, deg);
        using var blob = SKTextBlob.Create(text, font);
        SKRect bounds = blob.Bounds;
        float x = pt.X - bounds.Width / 2;
        float y = pt.Y + bounds.Height / 2;
        canvas.DrawText(blob, x, y, paint);
    }
    #endregion

    #region 八卦扇区绘制
    private void DrawAll(SKCanvas canvas)
    {
        float cx = CanvasSize.Width / 2f;
        float cy = CanvasSize.Height / 2f;

        using var sectorFill = new SKPaint
        {
            Color = SKColors.LightGray.WithAlpha(60),
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };

        using var sectorStroke = new SKPaint
        {
            Color = SKColors.Gray,
            StrokeWidth = 1,
            IsStroke = true,
            IsAntialias = true
        };
        float minSize = Math.Min(CanvasSize.Width, CanvasSize.Height);

        float RoundH = minSize / 15f;

        float LastR = RoundH;//最后半径
        float ysplace = RoundH;//外圈相距

        using var PathPaint = new SKPaint
        {
            Color = SKColors.Red,
            StrokeWidth = 1,
            IsStroke = true,
            IsAntialias = true
        };


        var PathFillP = PathPaint.Clone();


        PathFillP.Style = SKPaintStyle.Fill;

        //===============================画先天八卦===========================
        foreach (string sn in GuaSubClass.BeforeGuaSubNames)
        {


            var cbGua = GuaSubClass.GetGuaSub(sn, false);
            if (cbGua?.CBeforRangeDegree == null) continue;

            float start = (float)cbGua.CBeforRangeDegree.Start;
            float end = (float)cbGua.CBeforRangeDegree.End;

            var dcs = new DrawContextStru
            {
                CR = cbGua.CBeforRangeDegree,
                InnerRadius = LastR,

            };
            dcs.OuterRadius = dcs.InnerRadius + ysplace;

            // 生成路径（核心修复）
            SKPath sectorPath = CreateRingSectorPath(cx, cy, dcs.InnerRadius, dcs.OuterRadius, start, end);
            dcs.Path = sectorPath;
            GuaSectorCache.Add((cbGua.Name, sectorPath, cbGua.CBeforRangeDegree));

            Debug.WriteLine(cbGua.Symbol);


            canvas.DrawPath(sectorPath, PathPaint);

            // 文字
            DrawLabel(canvas, dcs, cbGua.Symbol, true);


        }
        //===============================画先天八卦===========================

        LastR = LastR + ysplace;

        //===============================画后天八卦===========================
        foreach (string sn in GuaSubClass.BeforeGuaSubNames)
        {


            var cbGua = GuaSubClass.GetGuaSub(sn, false);
            if (cbGua?.CAfterRangeDegree == null) continue;

            float start = (float)cbGua.CAfterRangeDegree.Start;
            float end = (float)cbGua.CAfterRangeDegree.End;

            var dcs = new DrawContextStru
            {
                CR = cbGua.CAfterRangeDegree,
                InnerRadius = LastR,
                AddTextSize = 10,
                BGColor = SKColors.Red.WithAlpha(10)

            };
            dcs.OuterRadius = dcs.InnerRadius + ysplace;

            // 生成路径（核心修复）
            SKPath sectorPath = CreateRingSectorPath(cx, cy, dcs.InnerRadius, dcs.OuterRadius, start, end);
            dcs.Path = sectorPath;
            GuaSectorCache.Add((cbGua.Name, sectorPath, cbGua.CAfterRangeDegree));


            canvas.DrawPath(sectorPath, PathPaint);//画边
            if (dcs.BGColor != SKColors.Transparent)
            {

                PathFillP.Color = dcs.BGColor;
                canvas.DrawPath(sectorPath, PathFillP);//填充
            }
            // 文字
            DrawLabel(canvas, dcs, cbGua.Name + "　" + cbGua.AfterGuaSubCNQuantity, false);


        }
        //===============================画后天八卦===========================


        LastR = LastR + ysplace;

        //===============================画正针二十四山===========================
        foreach (string sn in CHill.C24HillNames)
        {
            var CH = new CHill(sn);

            var CR = CH.CRangeDegree;
            if (CH == null) continue;

            float start = (float)CR.Start;
            float end = (float)CR.End;

            var dcs = new DrawContextStru
            {
                CR = CR,
                InnerRadius = LastR,
                AddTextSize = -10,
                ForceColor = CH.IsSun == true ? SKColors.Red : SKColors.Black
            };
            dcs.OuterRadius = dcs.InnerRadius + ysplace;

            // 生成路径（核心修复）
            SKPath sectorPath = CreateRingSectorPath(cx, cy, dcs.InnerRadius, dcs.OuterRadius, start, end);
            dcs.Path = sectorPath;
            GuaSectorCache.Add((sn, sectorPath, CR));


            canvas.DrawPath(sectorPath, PathPaint);//画边

            //PathFillP.Color = SKColors.Red.WithAlpha(10);
            //canvas.DrawPath(sectorPath, PathFillP);//填充

            // 文字
            DrawLabel(canvas, dcs, sn, false);


        }
        //===============================画正针二十四山===========================

        LastR = LastR + ysplace;
        //===============================画地盘六十四卦(后天)===========================
        foreach (var dc in CompassEx.CompassEx.CAfterGuas)
        {
            var CR = dc.Key;
            var G = dc.Value;


            if (G == null) continue;

            float start = (float)CR.Start;
            float end = (float)CR.End;

            var dcs = new DrawContextStru
            {
                CR = CR,
                InnerRadius = LastR,
                AddTextSize = G.Name.Length > 1 ? -10 : -20,

            };
            dcs.OuterRadius = dcs.InnerRadius + ysplace;


            if (G.GuaFate == "一")
            {
                dcs.ForceColor = SKColors.Red;

            }
            else if (G.GuaFate == "九")
            {
                dcs.ForceColor = SKColors.Blue;
            }


            // 生成路径（核心修复）
            SKPath sectorPath = CreateRingSectorPath(cx, cy, dcs.InnerRadius, dcs.OuterRadius, start, end);
            dcs.Path = sectorPath;
            GuaSectorCache.Add((G.Name, sectorPath, CR));


            canvas.DrawPath(sectorPath, PathPaint);//画边

            //PathFillP.Color = SKColors.Red.WithAlpha(10);
            //canvas.DrawPath(sectorPath, PathFillP);//填充

            // 文字
            DrawLabel(canvas, dcs, G.Name, false);


        }






        LastR = LastR + ysplace;

        //===============================画天盘六十四卦(先天)===========================
        foreach (var dc in CompassEx.CompassEx.CBeforeGuas)
        {
            var CR = dc.Key;
            var G = dc.Value;


            if (G == null) continue;

            float start = (float)CR.Start;
            float end = (float)CR.End;

            var dcs = new DrawContextStru
            {
                CR = CR,
                InnerRadius = LastR,
                AddTextSize = G.Name.Length > 1 ? -10 : -20,

            };
            if (G.GuaFate == "一")
            {
                dcs.ForceColor = SKColors.Red;

            }
            else if (G.GuaFate == "九")
            {
                dcs.ForceColor = SKColors.Blue;
            }

            dcs.OuterRadius = dcs.InnerRadius + ysplace;

            // 生成路径（核心修复）
            SKPath sectorPath = CreateRingSectorPath(cx, cy, dcs.InnerRadius, dcs.OuterRadius, start, end);
            dcs.Path = sectorPath;
            GuaSectorCache.Add((G.Name, sectorPath, CR));


            canvas.DrawPath(sectorPath, PathPaint);//画边

            //PathFillP.Color = SKColors.Red.WithAlpha(10);
            //canvas.DrawPath(sectorPath, PathFillP);//填充

            // 文字
            DrawLabel(canvas, dcs, G.Name, false);


        }
        //===============================画天盘六十四卦(先天)===========================


        //===============================画地盘六十四卦(后天)===========================

        LastR = LastR + ysplace;

        ysplace = RoundH / 2f;
        //===============================画卦气===========================
        foreach (var dc in CompassEx.CompassEx.CBeforeGuas)
        {
            var CR = dc.Key;
            var G = dc.Value;


            if (G == null) continue;

            float start = (float)CR.Start;
            float end = (float)CR.End;
            GuaQi gq = G.GuaQi;
            var dcs = new DrawContextStru
            {
                CR = CR,
                InnerRadius = LastR,
                AddTextSize = 6

            };
            dcs.OuterRadius = dcs.InnerRadius + ysplace;
            string sFA = gq.FiveAttr.Name;
            if (sFA == "金")
            {
                dcs.ForceColor = SKColors.Goldenrod;
            }
            else if (sFA == "木")
            {
                dcs.ForceColor = SKColors.DarkGreen;
            }
            else if (sFA == "水")
            {
                dcs.ForceColor = SKColors.Black;
            }
            else if (sFA == "火")
            {
                dcs.ForceColor = SKColors.Red;
            }



            // 生成路径（核心修复）
            SKPath sectorPath = CreateRingSectorPath(cx, cy, dcs.InnerRadius, dcs.OuterRadius, start, end);
            dcs.Path = sectorPath;



            canvas.DrawPath(sectorPath, PathPaint);//画边

            //PathFillP.Color = SKColors.Red.WithAlpha(10);
            //canvas.DrawPath(sectorPath, PathFillP);//填充

            // 文字

            string st = gq.GuaQiNumber.ToString() + gq.FiveAttr.ToString();
            DrawLabel(canvas, dcs, st, false);


        }

        //===============================画卦气===========================
    }

    private void DrawLabel(SKCanvas canvas, DrawContextStru dcs, string text, bool IsSymbol = false)
    {
        if (string.IsNullOrWhiteSpace(text)) return;

        float cx = CanvasSize.Width / 2f;
        float cy = CanvasSize.Height / 2f;
        float rIn = dcs.InnerRadius;
        float rOut = dcs.OuterRadius;
        float startDeg = (float)dcs.CR.Start;
        float endDeg = (float)dcs.CR.End;

        float totalH = rOut - rIn;
        float midAngle = startDeg + ((float)dcs.CR.AngleRangeValue() / 2f);
        float midR = rIn + totalH * 0.5f;

        float fontSize = (float)(totalH * 0.7f / Math.Max(1, text.Length));
        SKTypeface tmpTF = TF;

        if (IsSymbol)
        {
            midR /= 1.5f;
            fontSize /= 1.4f;
            tmpTF = TFSymbol;
        }
        fontSize += dcs.AddTextSize;
        SKPoint wordPt = GetArcPoint(cx, cy, midR, midAngle);
        using var font = new SKFont(tmpTF, fontSize) { Embolden = true };

        float rotate = midAngle - 180;

        canvas.Save();
        canvas.RotateDegrees(rotate, wordPt.X, wordPt.Y);

        string showTxt = text;
        using var tmpP = guaTextPaint.Clone();
        tmpP.Color = dcs.ForceColor;
        // ==========================================
        // 💡 核心改动：如果是符号，且现在还是方块，先用兼容写法
        // ==========================================
        if (IsSymbol)
        {
            // 符号在没解决字体问题前，bounds 获取到的是豆腐块的错误尺寸。
            // 我们临时强制使用基于 fontSize 的估算对齐，防止它乱飞。
            // 等你换了支持八卦的字体（如 Noto Sans）后，可以换回下面的通用公式。
            float drawX = wordPt.X - (fontSize / 2.2f);
            float drawY = wordPt.Y + fontSize;

            canvas.DrawText(showTxt, drawX, drawY, font, tmpP);
        }
        else
        {
            // 普通中文文字：完美适用几何中心对齐公式
            using var blob = SKTextBlob.Create(showTxt, font);
            SKRect bounds = blob.Bounds;

            float drawX = wordPt.X - bounds.Left - bounds.Width / 2f;
            float drawY = wordPt.Y - bounds.Top - bounds.Height / 2f;

            canvas.DrawText(blob, drawX, drawY, tmpP);
        }

        canvas.Restore();
    }
    #endregion

    #region 工具方法
    private double DegToRad(double deg) => deg * Math.PI / 180.0;

    private SKPoint GetArcPoint(float cx, float cy, float r, double deg)
    {
        double rad = DegToRad(deg);
        float x = cx + (float)(r * Math.Sin(rad));
        float y = cy - (float)(r * Math.Cos(rad));   // 0° 在正北，顺时针
        return new SKPoint(x, y);
    }
    /// <summary>
    /// 环形扇形 - 高精度分段版（最终修复版）
    /// </summary>
    private SKPath CreateRingSectorPath(float cx, float cy, float rIn, float rOut,
                                    float startDeg, float endDeg)
    {
        var path = new SKPath { FillType = SKPathFillType.EvenOdd };

        float start = Normalize(startDeg);
        float end = Normalize(endDeg);

        float sweep = end - start;
        if (sweep <= 0) sweep += 360f;

        const int segments = 36;                 // 36 段已经很平滑，可调
        float step = sweep / segments;

        // 外弧：start → end（顺时针）
        path.MoveTo(GetArcPoint(cx, cy, rOut, start));
        for (int i = 1; i <= segments; i++)
        {
            float angle = start + i * step;
            path.LineTo(GetArcPoint(cx, cy, rOut, angle));
        }

        // 连接到内弧终点
        path.LineTo(GetArcPoint(cx, cy, rIn, end));

        // 内弧：end → start（逆时针）
        for (int i = 1; i <= segments; i++)
        {
            float angle = end - i * step;
            path.LineTo(GetArcPoint(cx, cy, rIn, angle));
        }

        // 显式闭合（更保险）
        path.LineTo(GetArcPoint(cx, cy, rOut, start));
        path.Close();

        return path;
    }

    private float Normalize(float deg)
    {
        deg %= 360f;
        if (deg < 0) deg += 360f;
        return deg;
    }


    #endregion

    #region 点击检测
    public string? HitTestGua(SKPoint touchPt, float canvasCenterX, float canvasCenterY, float Sacle = 1f)
    {
        // 1. 依然计算当前旋转的弧度
        float rad = (float)DegToRad(Rotation);

        // 触摸点相对圆心的偏移
        float dx = touchPt.X - canvasCenterX;
        float dy = touchPt.Y - canvasCenterY;

        // 2. 💥 核心修正：标准的二维逆矩阵变换（将触摸点反向旋转 rad 弧度还原）
        // cos(-rad) = cos(rad),  sin(-rad) = -sin(rad)
        float rx = (float)(dx * Math.Cos(rad) - dy * Math.Sin(rad));
        float ry = (float)(dx * Math.Sin(rad) + dy * Math.Cos(rad));

        // 3. 还原回未旋转时的原始画布坐标
        SKPoint rawTouch = new SKPoint(canvasCenterX + rx, canvasCenterY + ry);

        // 4. 此时的 rawTouch 坐标系已完美对齐你的静态 Sector 缓存
        foreach (var item in GuaSectorCache)
        {

            // 2. 創建 1.5 倍的二維等比例縮放矩陣
            SKMatrix scaleMatrix = SKMatrix.CreateScale(Sacle, Sacle);

            SKPath transformedPath = new SKPath();
            // 3. ★ 核心步驟：將 sourcePath 經過矩陣運算後，將結果寫入 destinationPath
            // 這樣做可以確保 sourcePath 完好无损，完全不需要手動遍歷點
            item.Sector.Transform(scaleMatrix, transformedPath);

            if (transformedPath.Contains(rawTouch.X, rawTouch.Y))
                return item.Name;

            //if (item.Sector.Contains(rawTouch.X, rawTouch.Y))
            //    return item.Name;
        }
        return null;
    }
    #endregion
}