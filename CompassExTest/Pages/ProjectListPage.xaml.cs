using CommunityToolkit.Maui.Core.Extensions;
using CompassEx.Data.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CompassExTest.Pages
{
    public partial class ProjectListPage : INotifyPropertyChanged
    {
        public ObservableCollection<TblGoodDay> TblGoodDayList { get; set; } = new();
        public ProjectListPage()
        {

            InitializeComponent();


            // 3. 【必須最後一步】控制項都準備好了，數據也好了，一鍵通車！


        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            TblGoodDayList = TblGoodDay.GetTblGoodDayCol(pageIndex: 2).ToObservableCollection();
            MyCollectionView.ItemsSource = TblGoodDayList;

        }


    }
}