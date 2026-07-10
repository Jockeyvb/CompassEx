namespace CompassExTest.Pages;

public partial class MainContainerPage : ContentPage
{

    public ContentView MainCV;
    public MainContainerPage()
    {
        InitializeComponent();


        // ⚡ 初始化自救：窗口最大化逻辑（保持不变）
        this.HandlerChanged += (s, e) => TryMaximizeWindow();
        this.ParentChanged += (s, e) => TryMaximizeWindow();
        // 默认启动时，上面显示 Dashboard (TestPage)





    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Menu_Clicked(null, new TappedEventArgs("Dashboard"));

    }

    // ⚡ 底部菜单点击事件：直接换顶部的“演员”，菜单本身雷打不动留在底部
    private void Menu_Clicked(object sender, TappedEventArgs e)
    {
        // 1. 直接从事件参数 e.Parameter 中取出绑定的字符串
        if (e.Parameter is string pageType)
        {
            switch (pageType)
            {
                case "Dashboard":
                    cv.Content = new TestPage(); // ContentView 流派
                    break;
                case "Projects":
                    cv.Content = new ProjectListPage();
                    break;
                case "DataTest":
                    cv.Content = new DataTest().Content;
                    break;
            }
            MainCV = cv;
        }
    }

    // ⚡ 全局主题切换：一拨动，整个软件所有地方一起变色
    private void Theme_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
    {
        Application.Current.UserAppTheme = e.NewIndex == 0 ? AppTheme.Light : AppTheme.Dark;
    }

    private void TryMaximizeWindow()
    {
#if WINDOWS
        var mauiWindow = this.Window ?? Application.Current?.Windows.FirstOrDefault();
        if (mauiWindow?.Handler?.PlatformView is Microsoft.Maui.MauiWinUIWindow nativeWindow)
        {
            IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
            if (windowHandle != IntPtr.Zero && !IsZoomed(windowHandle))
            {
                ShowWindow(windowHandle, 3);
            }
        }
#endif
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