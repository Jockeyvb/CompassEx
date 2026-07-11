using Mopups.Pages;
using Mopups.Services;

namespace CompassExTest.Pages.Controls
{
    public partial class HudMessagePopup : PopupPage
    {
        public HudMessagePopup(string message)
        {

            InitializeComponent();
            TxtMessage.Text = string.IsNullOrWhiteSpace(message) ? "錯誤" : message;
            StartAutoCloseTimer();
        }

        private async void StartAutoCloseTimer()
        {
            try
            {
                await Task.Delay(2000);
                if (MopupService.Instance.PopupStack.Contains(this))
                {
                    await MopupService.Instance.PopAsync();
                }
            }
            catch { }
        }

        protected override bool OnBackgroundClicked() => false;
    }
}
