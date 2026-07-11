using CommunityToolkit.Maui.Core.Extensions;
using CompassEx.Data.Models;
using Mopups.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CompassExTest.Pages;

public partial class DataTest : ContentPage, INotifyPropertyChanged
{
    public ObservableCollection<TblGoodDay.ViewModel> TblGoodDayList { get; set; } = new();
    public DataTest()
    {

        InitializeComponent();
        this.BindingContext = this;






    }

    private void Button_Clicked(object sender, EventArgs e)
    {

        // 1. 拿出第一個物件
        var item = TblGoodDayList[0];

        // 2. 修改屬性
        item.Info = "test..............";

    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        string sWhere = " and ( Month=@m1  or Month=@m2)   ";
        var rrt = TblGoodDay.GetTblGoodDayCol(sWhere, [("m1", "五")], orderBy: " Order by Month asc",
            pageIndex: 1, pageSize: 30
        ).Result;
        if (rrt.RESULT == 0)
        {
            // 💡 建立剛才寫好的熱門 Mopups HUD 實例
            var hud = new CompassExTest.Pages.Controls.HudMessagePopup(rrt.Message);

            // 💡 異步調用全域圖層服務，這在 WinUI 桌面端百分之百能立馬彈出
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await MopupService.Instance.PushAsync(hud);
            });

        }
        else
        {
            TblGoodDayList = rrt.ReturnObj.Select(g => g.GetVM()).ToObservableCollection();
            MyCollectionView.ItemsSource = TblGoodDayList;
        }
    }
}