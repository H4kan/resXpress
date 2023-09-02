using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Xpress.Logic.FileSystem;

namespace ResXpress.Providers
{
    public class MessageProvider
    {
        private IVsStatusbar _bar;
        
        public MessageProvider(IVsStatusbar bar)
        {
            _bar = bar;
        }

        public void ShowInfoMessage(InfoMessage message)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (message.Status == InfoStatus.Success)
            {
                _bar.SetColorText(message.Text, (uint)COLORINDEX.CI_WHITE, (uint)COLORINDEX.CI_GREEN);
            }
            else
            {
                _bar.SetColorText(message.Text, (uint)COLORINDEX.CI_WHITE, (uint)COLORINDEX.CI_RED);
            }


        }
    }
}
