using Microsoft.VisualStudio.Shell;
using ResXpress.Providers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Xpress.Logic.FileSystem;

namespace ResXpress
{
    /// <summary>
    /// Interaction logic for UserWindowControl.
    /// </summary>
    public partial class UserWindowControl : UserControl
    {
        private FileSystemService _fileSystemService;
        private SolutionPathProvider _solutionPathProvider;
        private MessageProvider _messageProvider;

        private bool readyToRun = false;
        private string solPath = string.Empty;

        private IEnumerable<string> languages;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserWindowControl"/> class.
        /// </summary>
        public UserWindowControl()
        {
            this.InitializeComponent();
 
        }

        public void InitializeServices(
            SolutionPathProvider solutionPathProvider,
            MessageProvider messageProvider)
        {
            _solutionPathProvider = solutionPathProvider;
            _messageProvider = messageProvider;
            _fileSystemService = new FileSystemService();
            ThreadHelper.ThrowIfNotOnUIThread();
            this.InitializeStuff();
        }

        private void InitializeStuff()
        {
            this.runBtn.IsEnabled = false;
            this.fileComboBox.Items.Clear();
            ThreadHelper.ThrowIfNotOnUIThread();
            solPath = _solutionPathProvider.GetSolutionPath();
            if (solPath == null)
            {
                this.fileComboBox.Items.Add("Open project first");
                this.fileComboBox.SelectedIndex= 0;
            }
            else
            {
                var fileNames = this._fileSystemService.GetFileNames(solPath, out languages);
                this.readyToRun = fileNames.Count() > 0;
                if (!this.readyToRun)
                {
                    this.fileComboBox.Items.Add("Project doesn't have resx files");
                    this.fileComboBox.SelectedIndex = 0;
                }
                foreach (var file in fileNames)
                {
                    this.fileComboBox.Items.Add(file);
                }
                if (this.fileComboBox.Items.Count == 1)
                {
                    this.fileComboBox.SelectedIndex = 0;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.runBtn.IsEnabled = false;
            var message = this._fileSystemService.ProcessFileChange(
                this.inputBox.Text, solPath, this.fileComboBox.Text, this.languages);
            ThreadHelper.ThrowIfNotOnUIThread();
            this._messageProvider.ShowInfoMessage(message);
            if (message.Status == InfoStatus.Success)
            {
                Window.GetWindow(this).Close();
            }
            
        }

        private void fileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (readyToRun)
            this.runBtn.IsEnabled = true;
        }
    }
}