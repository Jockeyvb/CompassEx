
using CompassEx;
using CompassEx.Comm;
using CompassEx.Gua;
using Newtonsoft.Json;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
namespace CompassExTest.Pages;

public partial class TestPage : INotifyPropertyChanged
{
    private double _compassRotation;
    private string _headingText = "请点击罗盘调整方位";


    private readonly CompassSkiaRenderer _renderer = new();

    //// 实例化我们的绘制器
    //public CompassDrawable CompassPainter { get; } = new CompassDrawable();

    //public double CompassRotation
    //{
    //    get => _compassRotation;
    //    set
    //    {
    //        _compassRotation = value;
    //        OnPropertyChanged();

    //        // 核心：当角度改变时，同步更新给绘制器，并通知 Canvas 重绘
    //        CompassPainter.Rotation = _compassRotation;
    //        CompassCanvas?.Invalidate();
    //    }
    //}

    public string HeadingText
    {
        get => _headingText;
        set { _headingText = value; OnPropertyChanged(); }
    }

    public TestPage()
    {
        InitializeComponent();
        BindingContext = this;
        Entry_TextChanged(null, null);


        //foreach (string sn in GuaSubClass.BeforeGuaSubNames)
        //{
        //    var sg = GuaSubClass.GetGuaSub(sn);
        //    Debug.WriteLine(sg.GuaSubName + "，先天：" + sg.CBeforRangeDegree.Start.ToString() + "-" + sg.CBeforRangeDegree.End.ToString() + "，后天：" + sg.CAfterRangeDegree.Start.ToString() + "-" + sg.CAfterRangeDegree.End.ToString());
        //}
        //foreach (string sn in GuaClass.GuaFullNames)
        //{
        //    Debug.WriteLine(sn);
        //}


        //foreach (string sn in GuaSubClass.AfterGuaSubNames)
        //{
        //    if (sn != "黄")
        //    {
        //        var sg = GuaSubClass.GetGuaSub(sn);
        //        var fn = GuaFlip.GetGuaFlipNineStarDC(sg, GuaFlipMethod.Dragon);
        //        Dictionary<string, NineStar> dc = new Dictionary<string, NineStar>();
        //        var nj = NaJia<NaJiaYGResult>.CreateYG(sg);
        //        foreach (var kv in fn)
        //        {
        //            dc.Add(kv.Key.Name, kv.Value);
        //        }

        //        Debug.WriteLine("\n主卦" + GuaFlipMethod.Dragon.ToString() + "：" + sn + "\n" + JsonConvert.SerializeObject(dc) + "\n，主卦纳甲：" + JsonConvert.SerializeObject(nj));


        //    }

        //}
        //==============================罗盘的天机出卦==============================
        //var g = new CGuaClass("晋");
        //var tj = new TianJiGua(g);
        //var ls = tj.GetOutGuas();
        //string st = string.Join(",", tj.OutGuaSubs.Select(tg => tg.Value.Name + tg.Value.AfterQuantity));

        //Debug.WriteLine($"晋卦的卦宫是：【{g.GuaSelf.Name}】，天机出卦的后天洛数是：" + st + "六爻卦出卦：");
        //foreach (var kv in ls)
        //{
        //    Debug.Write("," + kv.Value.GuaFullName);
        //}
        //==============================罗盘的天机出卦==============================





        // List<(string, string, string)> ls = [("J", "1981-09-16", "男"), ("英", "1981-09-14", "女"), ("韵", "2007-02-13", "女"), ("恒", "2010-09-20", "男"), ("o", "1979-09-20", "男"), ("诗", "2004-06-01", "女"), ("炽", "1958-08-01", "男"), ("兴", "1956-10-25", "女")];
        List<(string, string, string)> ls = [("繁", "1985-10-30", "男")];
        foreach (var l in ls)
        {
            FateGua fg = new FateGua(DateTime.Parse(l.Item2), l.Item3, new CGuaClass("乾", 2));

            Debug.WriteLine("\n" + l.Item1 + "：" + JsonConvert.SerializeObject(fg.Infos));
            Debug.WriteLine("\nIsOutGua：" + fg.IsOutGua + ",IsFateGuaOut：" + fg.IsFateGuaOut + ",IsNaJiaOut：" + fg.IsNaJiaOut);
        }


        ////==============================罗盘的天机出卦(带临爻）==============================
        //var g = new CGuaClass("乾", 2);
        //var tj = new TianJiGua(g);
        //Debug.WriteLine("\nGetOutGuas:" + string.Join(",", tj.GetOutGuas().Select(g => g.Value.GuaFullName)));

        //Debug.WriteLine("\nYaoTypes:" + JsonConvert.SerializeObject(tj.YaoTypes));
        //Debug.WriteLine("\nPlaceYaoTypes:" + JsonConvert.SerializeObject(tj.PlaceYaoTypes));


        ////==============================罗盘的天机出卦(带临爻）==============================
        //var dc = FlipGua.GetFlipGuaNineStarDC(new CHill("壬"), FlipGuaMethod.Dragon);

        //string json = JsonConvert.SerializeObject(dc.Select(kv => "【" + kv.Key.ToString() + "】" + "=》:" + JsonConvert.SerializeObject(kv.Value)));
        //Debug.WriteLine("\n GetFlipGuaNineStarDC:" + json);

        FiveGhostWealth fgw = FlipGua.GetFiveGhostWealth(new CHill("壬"));

        string json = fgw.ToString();
        Debug.WriteLine("\n GetFiveGhostWealth:" + json);


    }

    // 全域變數：控制縮放
    private float _gestureScale = 1.0f;

    // 全域變數：控制拖動平移（核心）
    private float _offsetX = 0f;
    private float _offsetY = 0f;






    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {

        SKCanvas canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.Transparent);

        // 1. 執行【畫布整體平移】
        // 手指往右拖多少，整個畫布的坐標系就往右移動多少像素
        canvas.Translate(_offsetX, _offsetY);

        // 2. 執行【畫布中心縮放】
        // 為了讓縮放以羅盤圓心為軸心放大，我們通常在平移後疊加中心點縮放
        float canvasCenterX = e.Info.Width / 2f;
        float canvasCenterY = e.Info.Height / 2f;
        canvas.Scale(_gestureScale, _gestureScale, canvasCenterX, canvasCenterY);




        _renderer.Render(canvas, e.Info.Size);
    }
    // 全域變數：記錄上一次觸控點的坐標（用於計算拖動差值）
    private SKPoint _lastTouchPoint;
    private bool _isDragging = false;
    // 全局字典：用于追踪当前屏幕上的所有手指（Key为手指ID，Value为当前物理坐标）
    private readonly System.Collections.Generic.Dictionary<long, SKPoint> _touchPoints = new();
    // 全局变量：用于计算双指初始距离
    private float _startPointerDistance = 0f;
    private float _baseScaleAtPinchStart = 1.0f;
    // 【新增控制变量】用于标记刚才是不是处于双指缩放状态，以便在松手时触发一次高清重绘
    private bool _wasPinchZooming = false;

    private async void OnCanvasTouch(object sender, SKTouchEventArgs e)
    {
        try
        {
            SKPoint currentPoint = e.Location;

            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    if (!_touchPoints.ContainsKey(e.Id))
                    {
                        _touchPoints.Add(e.Id, currentPoint);
                    }

                    if (_touchPoints.Count == 1)
                    {
                        _lastTouchPoint = currentPoint;
                        _isDragging = false;
                    }
                    else if (_touchPoints.Count == 2)
                    {
                        _isDragging = false;
                        _lastTouchPoint = SKPoint.Empty;
                        _wasPinchZooming = true;

                        var points = new System.Collections.Generic.List<SKPoint>(_touchPoints.Values);

                        // 🌟【核心改善点 1】：在刚按下双指时，由于此时 SkiaCompassView.Scale 还是 1.0，
                        // 这里的初始距离是干净的绝对物理距离，记录它。
                        _startPointerDistance = SKPoint.Distance(points[0], points[1]);
                        _baseScaleAtPinchStart = _gestureScale;
                    }
                    break;

                case SKTouchAction.Moved:
                    if (_touchPoints.ContainsKey(e.Id))
                    {
                        _touchPoints[e.Id] = currentPoint;
                    }

                    if (_touchPoints.Count == 1 && !_lastTouchPoint.IsEmpty)
                    {
                        // 【分支 A：单指拖拽平移】
                        float deltaX = currentPoint.X - _lastTouchPoint.X;
                        float deltaY = currentPoint.Y - _lastTouchPoint.Y;

                        if (Math.Abs(deltaX) > 5 || Math.Abs(deltaY) > 5)
                        {
                            _isDragging = true;
                            _offsetX += deltaX;
                            _offsetY += deltaY;
                            _lastTouchPoint = currentPoint;

                            SkiaCompassView.InvalidateSurface();



                        }
                    }
                    else if (_touchPoints.Count == 2 && _startPointerDistance > 0)
                    {
                        // 【分支 B：双指捏合缩放】
                        var points = new System.Collections.Generic.List<SKPoint>(_touchPoints.Values);

                        // 🌟【核心改善点 2】：这是解决瞬间复位的关键！
                        // 因为你在下面改了 SkiaCompassView.Scale，此时进来的 points 坐标会被成比例缩放。
                        // 我们必须利用当前的真实缩放值（SkiaCompassView.Scale），通过乘法逆运算，将其还原为纯净的、不受污染的物理屏幕坐标！
                        double currentVisualScale = SkiaCompassView.Scale;
                        SKPoint p1_raw = new SKPoint((float)(points[0].X * currentVisualScale), (float)(points[0].Y * currentVisualScale));
                        SKPoint p2_raw = new SKPoint((float)(points[1].X * currentVisualScale), (float)(points[1].Y * currentVisualScale));

                        // 🌟 用解毒后的物理坐标计算距离
                        float currentDistance = SKPoint.Distance(p1_raw, p2_raw);
                        if (currentDistance <= 0) return;

                        // 计算当前距离与初始距离的比例
                        float scaleFactor = currentDistance / _startPointerDistance;

                        // 1. 临时计算出目标缩放比例
                        float targetScale = _baseScaleAtPinchStart * scaleFactor;
                        targetScale = Math.Clamp(targetScale, 0.5f, 5.0f);

                        // 2. 直接操纵 MAUI 原生视图层进行硬件加速缩放
                        if (_gestureScale > 0)
                        {
                            SkiaCompassView.Scale = targetScale / _gestureScale;
                        }
                    }
                    break;

                case SKTouchAction.Released:






                    if (_wasPinchZooming && _touchPoints.Count == 2)
                    {
                        // 🌟【核心改善点 3】：松手瞬间算最终比例时，同样需要使用当前的 Scale 还原点坐标
                        var points = new System.Collections.Generic.List<SKPoint>(_touchPoints.Values);

                        double currentVisualScale = SkiaCompassView.Scale;
                        SKPoint p1_raw = new SKPoint((float)(points[0].X * currentVisualScale), (float)(points[0].Y * currentVisualScale));
                        SKPoint p2_raw = new SKPoint((float)(points[1].X * currentVisualScale), (float)(points[1].Y * currentVisualScale));

                        float currentDistance = SKPoint.Distance(p1_raw, p2_raw);
                        float scaleFactor = currentDistance / _startPointerDistance;

                        // 1. 正式把最终缩放结果同步给内部全局变量
                        _gestureScale = Math.Clamp(_baseScaleAtPinchStart * scaleFactor, 0.5f, 5.0f);

                        // 2. 还原 MAUI 原生视图的缩放
                        SkiaCompassView.Scale = 1.0;

                        // 3. 触发仅此一次的、最终的高清重新渲染
                        SkiaCompassView.InvalidateSurface();

                        _wasPinchZooming = false;
                    }

                    if (_touchPoints.Count == 1 && !_isDragging && !_lastTouchPoint.IsEmpty && !_wasPinchZooming)
                    {

                        HandleCompassRotateOnClick(currentPoint);
                    }

                    _touchPoints.Remove(e.Id);

                    if (_touchPoints.Count == 0)
                    {
                        _lastTouchPoint = SKPoint.Empty;
                        _isDragging = false;
                        _startPointerDistance = 0f;
                        _wasPinchZooming = false;
                    }
                    else if (_touchPoints.Count == 1)
                    {
                        // 如果从双指变回了单指，此时原生 Scale 已经恢复为 1 了，直接重置单指拖动锚点

                        var remainingId = new System.Collections.Generic.List<long>(_touchPoints.Keys)[0];
                        _lastTouchPoint = _touchPoints[remainingId];
                        _isDragging = false;

                    }

                    break;

                case SKTouchAction.Cancelled:
                    _touchPoints.Clear();
                    _lastTouchPoint = SKPoint.Empty;
                    _isDragging = false;
                    _startPointerDistance = 0f;
                    _wasPinchZooming = false;
                    SkiaCompassView.Scale = 1.0;
                    break;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"触控流错误: {ex.Message}");
        }
        finally
        {
            e.Handled = true;
        }
    }

    private void HandleCompassRotateOnClick(SKPoint p)
    {

        SKPoint touch = p;
        // 统一使用渲染器画布尺寸，不再硬编码取SkiaCompassView宽高
        float cx = SkiaCompassView.CanvasSize.Width / 2f;
        float cy = SkiaCompassView.CanvasSize.Height / 2f;

        // 计算点击对应的罗盘刻度
        double panelDegree = GetTouchPanelDegree(touch, cx, cy, _renderer.Rotation);
        // 检测命中卦象
        string? hitGua = _renderer.HitTestGua(touch, cx, cy, _gestureScale);
        string dirText = GetDirectionText(panelDegree);
        _isDragging = false;
        _lastTouchPoint = SKPoint.Empty;
        // 执行旋转
        _renderer.Rotation = panelDegree;
        SkiaCompassView.InvalidateSurface();
        // UI赋值切主线程，避免跨线程异常
        MainThread.BeginInvokeOnMainThread(() =>
        {
            string info = $"{panelDegree:F1}° 方位:{dirText}（先天）";
            if (!string.IsNullOrEmpty(hitGua))
            {
                info += $" 选中:{hitGua}";
            }
            HeadingText = info;

            CompassRangEX cr = new CompassRangEX(panelDegree, panelDegree);
            List<CompassObjType> ls = cr.GetCompassObjByDegree();
            string st = "";
            ls.ForEach(co =>
            {
                st += co.ObjTypeCNName + ",名称：" + co.Name + "，角度范围：" + co.CRDegree.Start.ToString("F1") + "-" + co.CRDegree.End.ToString("F1") + "°\n";
            });
            lblGetCompassObjByDegree.Text = st;



        });
    }


    /// <summary>
    /// 计算触摸点对应的罗盘面板刻度度数（抵消罗盘旋转）
    /// </summary>
    /// <param name="touchPt">画布触摸坐标</param>
    /// <param name="cx">画布中心X</param>
    /// <param name="cy">画布中心Y</param>
    /// <param name="rotateTotal">罗盘整体旋转 Rotation</param>
    /// <returns>0~360 罗盘刻度度数</returns>
    private double GetTouchPanelDegree(SKPoint touchPt, float cx, float cy, double rotateTotal)
    {
        // 1. 触摸点相对圆心的偏移量
        float dx = touchPt.X - cx;
        float dy = touchPt.Y - cy;

        // 防止用户极其精准地点中了圆心导致 dx 和 dy 均为 0，引发数学错误
        if (Math.Abs(dx) < 0.001f && Math.Abs(dy) < 0.001f)
            return rotateTotal;

        // 2. 计算点击位置的绝对屏幕角度（正北向上为0，顺时针 0 ~ 360）
        double rad = Math.Atan2(dx, -dy);
        double rawAngle = rad * 180 / Math.PI;
        if (rawAngle < 0) rawAngle += 360;

        // 3. 💥 核心修正：加上罗盘旋转角度
        // 盘面顺时针转，等于点击的刻度逆时针移，所以盘面上的真实刻度需要累加当前转过去的度数
        double target = rawAngle + rotateTotal;

        // 4. 规范到 0 ~ 360 范围
        target %= 360;
        if (target < 0) target += 360;

        return target;
    }


    protected override void OnParentSet()
    {
#if WINDOWS
 InnerSquareGrid.WidthRequest = 1000;
        SkiaCompassView.HandlerChanged += (sender, e) =>
        {
            if (SkiaCompassView.Handler?.PlatformView is Microsoft.UI.Xaml.FrameworkElement winView)
            {
                // 直接訂閱 Windows (WinUI 3) 原生的滑鼠滾輪事件！
                winView.PointerWheelChanged += WinView_PointerWheelChanged;
            }
        };
#endif

        base.OnParentSet();
    }
#if WINDOWS
/// <summary>
/// 處理 Windows 平台底層原生的滑鼠滾輪事件
/// </summary>
private void WinView_PointerWheelChanged(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
{
    // 1. 獲取目前指針（滑鼠）相對於 SkiaCompassView 控制項的屬性
    var currentPoint = e.GetCurrentPoint(SkiaCompassView.Handler?.PlatformView as Microsoft.UI.Xaml.UIElement);
    var properties = currentPoint.Properties;

    // 2. 獲取滾輪滾動的物理增量（在 Windows 中，向前滾為正數，向後滾為負數）
    int wheelDelta = properties.MouseWheelDelta;

    if (wheelDelta != 0)
    {
        // 3. 根據滾輪方向計算縮放係數（向前滾放大 10%，向後滾縮小 10%）
        float scaleFactor = wheelDelta > 0 ? 1.1f : 0.9f;

        // 4. 更新全域的手勢縮放變數
        float newScale = _gestureScale * scaleFactor;

        // 5. 實施安全邊界限制（防止羅盤被無限放大或縮小到看不見）
        _gestureScale = Math.Clamp(newScale, 0.5f, 5.0f);

        // 6. 強行通知 SkiaSharp 利用 GPU 進行高精度重繪
        SkiaCompassView.InvalidateSurface();

        // 7. 標記此事件已被當前控制項徹底處理
        // 這非常關鍵！它可以防止滾輪動作穿透到外層的 ScrollView，從而避免「縮放羅盤時頁面也跟著上下滾動」的糟糕體驗
        e.Handled = true;
    }
}
#endif




    //private void OnCompassTapped(object sender, TappedEventArgs e)
    //{
    //    var visualElement = sender as VisualElement;
    //    if (visualElement == null) return;

    //    Point? relativePosition = e.GetPosition(visualElement);
    //    if (relativePosition == null) return;

    //    double touchX = relativePosition.Value.X;
    //    double touchY = relativePosition.Value.Y;

    //    double centerX = visualElement.Width / 2;
    //    double centerY = visualElement.Height / 2;

    //    double dx = touchX - centerX;
    //    double dy = centerY - touchY;

    //    double radians = Math.Atan2(dy, dx);
    //    double degrees = 360 - radians * (180 / Math.PI);

    //    // 计算罗盘方位角
    //    double heading = 90 - degrees;
    //    if (heading < 0) heading += 360;
    //    if (heading >= 360) heading -= 360;
    //    float visualRotation = 360 - (float)Rotation;
    //    _renderer.Rotation = visualRotation;
    //}

    private string GetDirectionText(double heading)
    {
        foreach (string sn in GuaSubClass.BeforeGuaSubNames)
        {
            GuaSubClass gs = GuaSubClass.GetGuaSub(sn, false);
            if (gs.CBeforRangeDegree.IsInRange(heading)) return sn;
        }
        return "";
    }

    #region Property Changed
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        string sv = tDegree.Text;
        if (Microsoft.VisualBasic.Information.IsNumeric(sv) == false) return;

        double dDegree = double.Parse(sv);
        _renderer.Rotation = dDegree;
        SkiaCompassView.InvalidateSurface();
        HeadingText = $"{tDegree.Text:F1}° {GetDirectionText(dDegree)}";

    }
#if WINDOWS
    // 告诉 C# 怎么调用 Windows 系统的窗口管理控制
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    // 告诉 C# 怎么判断当前窗口是不是最大化状态
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern bool IsZoomed(IntPtr hWnd);
#endif
}
