using Microsoft.VisualStudio.Shell;
using ResXpress.Providers;
using System;
using System.Runtime.InteropServices;
using System.Windows.Controls;

namespace ResXpress
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("9c3b5ca4-7cac-4225-b099-1652691820eb")]
    public class UserWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserWindow"/> class.
        /// </summary>
        public UserWindow() : base(null)
        {
            this.Caption = "ResXpress";
            this.Content = new UserWindowControl();
        }
    }
}
