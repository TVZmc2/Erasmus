using Microsoft.Phone.Shell;

namespace ErasmusAppTVZ.Helpers
{
    sealed class ProgressIndicatorHelper
    {
        /// <summary>
        /// Shows or hides ProgressIndicator
        /// </summary>
        /// <param name="value">Show or hide</param>
        /// <param name="text">Text to be displayed</param>
        public static void SetProgressBar(bool value, string text)
        {
            SystemTray.ProgressIndicator.Text = text;
            SystemTray.ProgressIndicator.IsIndeterminate = value;
            SystemTray.ProgressIndicator.IsVisible = value;
        }
    }
}
