using CommunityToolkit.Maui.Core.Extensions;
using CompassEx.Data.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CompassExTest.Pages;

public partial class DataTest : ContentPage, INotifyPropertyChanged
{
    public ObservableCollection<TblGoodDay> TblGoodDayList { get; set; } = new();
    public DataTest()
    {

        InitializeComponent();

        TblGoodDayList = TblGoodDay.GetTblGoodDayCol().ToObservableCollection();
        MyCollectionView.ItemsSource = TblGoodDayList;
        this.BindingContext = this;



    }

}