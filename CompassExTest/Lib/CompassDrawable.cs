using CompassEx.Comm;
using CompassEx.Guo;

namespace CompassExTest.Pages;

public class CompassDrawable : IDrawable
{
    // 允许外部传入当前的旋转角度，以便重绘
    public double Rotation { get; set; } = 0;

    float BaseRadius = 0;//基础中心点半径
    float BaseBorderWidth = 2;//边框默认宽度（间距）

    public struct DrawContextStru

    {


        public DrawContextStru() { }

        public float InnerRadius;
        public float OuterRadius;
        public Color ForceColor = Colors.Black;
        public Color BGColor = Colors.Transparent;
        public CompassRangEX CR = null;
        public PathF Path = null;
    }
    public void DrawAfterSubGuo(ICanvas canvas, RectF dirtyRect)
    {
        float fw = 12;//默认文字字体大小
        float centerX = dirtyRect.Width / 2;
        float centerY = dirtyRect.Height / 2;
        float radius = BaseRadius + fw + BaseBorderWidth;

        canvas.StrokeColor = Colors.Red;
        canvas.DrawCircle(centerX, centerY, radius);
        foreach (string sn in GuoSubClass.BeforeGuoSubNames)
        {

            var CBGuo = CompassEx.Guo.GuoSubClass.GetGuoSub(sn, false);
            DrawContextStru DCS = new DrawContextStru { InnerRadius = radius + 40, OuterRadius = radius + 80, CR = CBGuo.CBeforRangeDegree };
            DrawVerticalAutoFitLabel(canvas, dirtyRect, ref DCS, CBGuo.GuoSubName);
            if (DCS.Path != null)
            {
                // canvas.DrawPath(DCS.Path);
            }
        }

    }

    public void DrawVerticalAutoFitLabel(ICanvas canvas, RectF dirtyRect, ref DrawContextStru DCS, string sText)
    {

        if (string.IsNullOrWhiteSpace(sText)) return;

        float centerX = dirtyRect.Width / 2;
        float centerY = dirtyRect.Height / 2;

        float innerRadius = DCS.InnerRadius;
        float outerRadius = DCS.OuterRadius;
        float totalHeight = outerRadius - innerRadius;
        float autoFontSize = totalHeight * 0.8f / sText.Length;

        canvas.FontSize = autoFontSize;
        canvas.FontColor = Colors.DarkSlateGray;
        canvas.Font = Microsoft.Maui.Graphics.Font.DefaultBold;

        // 1. 计算扇形中点角度
        float midAngle = (float)DCS.CR.Start + ((float)DCS.CR.AngleRangeValue() / 2);
        float midRadius = innerRadius + (totalHeight * 0.5f);

        // 2. 算出文字所在圆环坐标（位置不变，保证卦位精准）
        double radians = midAngle * Math.PI / 180.0;
        float wordX = centerX + (float)(midRadius * Math.Sin(radians));
        float wordY = centerY - (float)(midRadius * Math.Cos(radians));

        float boxWidth = autoFontSize * 1.5f;
        float boxHeight = autoFontSize * 1.2f;






        canvas.SaveState();
        // 关键：以180度正南为文字正向基准，旋转文字让阅读方向朝外
        float textRotate = midAngle - 180;
        canvas.Rotate(textRotate, wordX, wordY);
        DCS.Path = CreateRingSectorPath(centerX, centerY, innerRadius, outerRadius, (float)DCS.CR.Start, (float)DCS.CR.End);
        // 以文字自身中心点绘制，无需偏移
        canvas.DrawString(
            sText[0].ToString(),
            wordX - (boxWidth / 2),
            wordY - (boxHeight / 2),
            boxWidth,
            boxHeight,
            HorizontalAlignment.Center,
            VerticalAlignment.Center
        );
        canvas.DrawPath(DCS.Path);
        canvas.RestoreState();
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        float centerX = dirtyRect.Width / 2;
        float centerY = dirtyRect.Height / 2;
        float radius = Math.Min(centerX, centerY) - 20;
        if (radius <= 0) return;

        canvas.Antialias = true;

        // 最外圈大圆
        canvas.StrokeColor = Colors.DarkSlateGray;
        canvas.StrokeSize = 3;
        canvas.DrawCircle(centerX, centerY, radius);

        // ========== 全局旋转：刻度+四正文字+八卦全部一起转 ==========
        canvas.SaveState();
        float visualRotation = 360 - (float)Rotation;
        canvas.Rotate(visualRotation, centerX, centerY);

        // 绘制5度刻度
        canvas.FontSize = 12;
        canvas.FontColor = Colors.DimGray;
        canvas.Font = Microsoft.Maui.Graphics.Font.Default;
        for (int angle = 0; angle < 360; angle += 5)
        {
            canvas.SaveState();
            canvas.Rotate((float)angle, centerX, centerY);

            float lineWidth = 10;
            canvas.FontColor = Colors.DimGray;
            canvas.FontSize = 10;
            canvas.StrokeColor = Colors.DimGray;

            if (angle % 45 == 0)
            {
                canvas.FontColor = Colors.Red;
                lineWidth = 15;
                canvas.StrokeColor = Colors.Red;
                canvas.FontSize = 12;
                canvas.Font = Microsoft.Maui.Graphics.Font.DefaultBold;
            }

            canvas.StrokeSize = 2;
            canvas.DrawLine(centerX, centerY - radius, centerX, centerY - radius + lineWidth);

            string angleText = angle.ToString();
            canvas.DrawString(
                angleText,
                centerX - 20,
                centerY - radius + 15,
                40,
                20,
                HorizontalAlignment.Center,
                VerticalAlignment.Center);

            canvas.Font = Microsoft.Maui.Graphics.Font.Default;
            canvas.RestoreState();
        }

        // ========== 修复：三角函数精准计算东南西北坐标，不再硬写死偏移 ==========
        canvas.FontSize = 18;
        canvas.FontColor = Colors.Crimson;
        float textOffset = (float)(18 * 2.5f); // 文字距离外圈刻度向内偏移距离
        float dirRadius = radius - textOffset;
        float boxW = 30;
        float boxH = 30;

        // 正北 0°
        double radBei = 0 * Math.PI / 180.0;
        float xBei = centerX + (float)(dirRadius * Math.Sin(radBei));
        float yBei = centerY - (float)(dirRadius * Math.Cos(radBei));
        canvas.DrawString("北", xBei - boxW / 2, yBei - boxH / 2, boxW, boxH, HorizontalAlignment.Center, VerticalAlignment.Center);

        // 正东 90°
        double radDong = 90 * Math.PI / 180.0;
        float xDong = centerX + (float)(dirRadius * Math.Sin(radDong));
        float yDong = centerY - (float)(dirRadius * Math.Cos(radDong));
        canvas.DrawString("东", xDong - boxW / 2, yDong - boxH / 2, boxW, boxH, HorizontalAlignment.Center, VerticalAlignment.Center);

        // 正南 180°
        double radNan = 180 * Math.PI / 180.0;
        float xNan = centerX + (float)(dirRadius * Math.Sin(radNan));
        float yNan = centerY - (float)(dirRadius * Math.Cos(radNan));
        canvas.DrawString("南", xNan - boxW / 2, yNan - boxH / 2, boxW, boxH, HorizontalAlignment.Center, VerticalAlignment.Center);

        // 正西 270°
        double radXi = 270 * Math.PI / 180.0;
        float xXi = centerX + (float)(dirRadius * Math.Sin(radXi));
        float yXi = centerY - (float)(dirRadius * Math.Cos(radXi));
        canvas.DrawString("西", xXi - boxW / 2, yXi - boxH / 2, boxW, boxH, HorizontalAlignment.Center, VerticalAlignment.Center);

        // 八卦绘制（全局旋转内部，同步转动）
        DrawAfterSubGuo(canvas, dirtyRect);

        // 配对全局SaveState，必须保留
        canvas.RestoreState();

        // 固定不动的中心指针（不受旋转影响）
        canvas.StrokeColor = Colors.Red.WithAlpha(0.3f);
        canvas.StrokeSize = 4;
        canvas.DrawLine(centerX, centerY + radius - 15, centerX, centerY - radius + 15);

        canvas.FillColor = Colors.Red;
        canvas.FillCircle(centerX, centerY, 6);

    }








    // 角度转弧度
    private double DegToRad(double deg)
    {
        return deg * Math.PI / 180.0;
    }

    // 根据圆心、半径、角度获取圆环坐标（和你文字三角函数完全统一）
    private PointF GetArcPoint(float cx, float cy, float r, double deg)
    {
        double rad = DegToRad(deg);
        float x = cx + (float)(r * Math.Sin(rad));
        float y = cy - (float)(r * Math.Cos(rad));
        return new PointF(x, y);
    }

    /// <summary>
    /// 生成环形扇形路径（八卦扇区）
    /// </summary>
    /// <param name="cx">圆心X</param>
    /// <param name="cy">圆心Y</param>
    /// <param name="rIn">内半径</param>
    /// <param name="rOut">外半径</param>
    /// <param name="startDeg">起始角度</param>
    /// <param name="endDeg">结束角度</param>
    public PathF CreateRingSectorPath(float cx, float cy, float rIn, float rOut, float startDeg, float endDeg)
    {
        PathF path = new PathF();

        // 1. 外圈外接矩形 左上角/右下角
        float outerLeft = cx - rOut;
        float outerTop = cy - rOut;
        float outerRight = cx + rOut;
        float outerBottom = cy + rOut;

        // 外圈起点
        PointF pOutStart = GetArcPoint(cx, cy, rOut, startDeg);
        path.MoveTo(pOutStart.X, pOutStart.Y);

        // 2. 外圈圆弧 使用7浮点参数重载AddArc
        path.AddArc(outerLeft, outerTop, outerRight, outerBottom, startDeg, endDeg, true);

        // 3. 连线到内圈终点
        PointF pInEnd = GetArcPoint(cx, cy, rIn, endDeg);
        path.LineTo(pInEnd.X, pInEnd.Y);

        // 4. 内圈外接矩形
        float innerLeft = cx - rIn;
        float innerTop = cy - rIn;
        float innerRight = cx + rIn;
        float innerBottom = cy + rIn;

        // 5. 反向绘制内圈圆弧闭合
        path.AddArc(innerLeft, innerTop, innerRight, innerBottom, endDeg, startDeg, false);

        // 闭合整个扇形区域
        path.Close();
        return path;
    }
}