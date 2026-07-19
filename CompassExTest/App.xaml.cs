using CompassEx.Data;

namespace CompassExTest
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Comm.InitializeDatabase();
            MainPage = new NavigationPage(new Pages.MainContainerPage());
        }

        //protected override Window CreateWindow(IActivationState? activationState)
        //{
        //    return new Window(new AppShell());
        //}
    }
}