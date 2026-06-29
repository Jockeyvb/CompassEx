using CompassEx.Guo;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
namespace CompassExTest.Pages;

public partial class TestPage : ContentPage, INotifyPropertyChanged
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


        foreach (string sn in GuoSubClass.BeforeGuoSubNames)
        {
            var sg = GuoSubClass.GetGuoSub(sn);
            Debug.WriteLine(sg.GuoSubName + "，先天：" + sg.CBeforRangeDegree.Start.ToString() + "-" + sg.CBeforRangeDegree.End.ToString() + "，后天：" + sg.CAfterRangeDegree.Start.ToString() + "-" + sg.CAfterRangeDegree.End.ToString());
        }
        //foreach (string sn in GuoClass.Symbols)
        //{
        //    Debug.WriteLine(sn);
        //}


    }

    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        SKCanvas canvas = e.Surface.Canvas;
        SKSize size = e.Info.Size;

        _renderer.Render(canvas, size);
    }

    private void OnCanvasTouch(object sender, SKTouchEventArgs e)
    {
        try
        {


            // 仅响应按下操作
            if (e.ActionType != SKTouchAction.Pressed)
                return;

            SKPoint touch = e.Location;
            // 统一使用渲染器画布尺寸，不再硬编码取SkiaCompassView宽高
            float cx = SkiaCompassView.CanvasSize.Width / 2f;
            float cy = SkiaCompassView.CanvasSize.Height / 2f;

            // 计算点击对应的罗盘刻度
            double panelDegree = GetTouchPanelDegree(touch, cx, cy, _renderer.Rotation);
            // 检测命中卦象
            string? hitGua = _renderer.HitTestGua(touch, cx, cy);
            string dirText = GetDirectionText(panelDegree);

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
            });
        }
        catch (Exception ex)
        {

        }
        finally
        {
            e.Handled = true;
        }
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

    protected override void OnAppearing()
    {
        var mauiWindow = this.Window ?? Application.Current?.MainPage?.Window;
        if (mauiWindow == null) return;
#if WINDOWS
    // 1. 获取 MAUI 的原生 Windows 窗口
    var handler = mauiWindow.Handler as Microsoft.Maui.Handlers.WindowHandler;
    if (handler?.PlatformView is Microsoft.UI.Xaml.Window nativeWindow)
    {
        // 2. 拿到窗口的绝对物理句柄 HWND
        IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
        
        // 3. 直接通过 Windows 底层用户界面 API 最大化窗口
        // 3 代表最大化 (SW_MAXIMIZE)，1 代表正常还原 (SW_SHOWNORMAL)
        if (IsZoomed(windowHandle))
        {
            ShowWindow(windowHandle, 1); // 如果已经最大化了，就还原
        }
        else
        {
            ShowWindow(windowHandle, 3); // 如果是普通窗口，就最大化
        }
    }
#endif

        base.OnAppearing();
    }

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
        foreach (string sn in GuoSubClass.BeforeGuoSubNames)
        {
            GuoSubClass gs = GuoSubClass.GetGuoSub(sn, false);
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
