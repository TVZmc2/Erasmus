using Microsoft.Phone.Shell;

namespace ErasmusAppTVZ.Helpers
{
    sealed class ProgressIndicatorHelper
    {
        public static void SetProgressBar(bool value, string text)
        {
            SystemTray.ProgressIndicator.Text = text;
            SystemTray.ProgressIndicator.IsIndeterminate = value;
            SystemTray.ProgressIndicator.IsVisible = value;
        }
    }
}
