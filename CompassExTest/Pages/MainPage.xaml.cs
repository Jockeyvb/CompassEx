using CompassExTest.Models;
using CompassExTest.PageModels;

namespace CompassExTest.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}