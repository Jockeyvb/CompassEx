using CommunityToolkit.Maui.Core.Extensions;
using CompassEx.Data.Models;
using Mopups.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CompassExTest.Pages;

public partial class DataTest : ContentPage, INotifyPropertyChanged
{
    public ObservableCollection<tbl_GoodDayVM> TblGoodDayList { get; set; }
    public DataTest()
    {

        InitializeComponent();







    }

    private void Button_Clicked(object sender, EventArgs e)
    {

        // 1. 拿出第一個物件
        var item = TblGoodDayList[0];
        var c = TblGoodDayList[1] as tbl_GoodDay;
        // 2. 修改屬性
        item.Info = "test..............";
        TblGoodDayList.RemoveAt(1);
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {

        // 1. 获取原始数据
        TblGoodDayList = tbl_GoodDay.List(l => l.Month == "五" || l.Month == "六", 1, 30).Select(x => x.ToViewModel()).ToObservableCollection();

        //// 2. 规范转换为 VM 集合
        //TblGoodDayList = new ObservableCollection<tbl_GoodDayVM>(
        //    rawData.Select(x => x.Totbl_GoodDayVM())
        //);
        if (!TblGoodDayList.Any())
        {
            // 💡 建立剛才寫好的熱門 Mopups HUD 實例
            var hud = new CompassExTest.Pages.Controls.HudMessagePopup("无法获得数据");

            // 💡 異步調用全域圖層服務，這在 WinUI 桌面端百分之百能立馬彈出
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await MopupService.Instance.PushAsync(hud);
            });

        }
        else
        {

            MyCollectionView.ItemsSource = TblGoodDayList;

        }

    }
}