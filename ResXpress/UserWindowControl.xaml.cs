using ResXpress.Providers;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="UserWindowControl"/> class.
        /// </summary>
        public UserWindowControl()
        {
            this.InitializeComponent();
 
        }

        public void InitializeServices(SolutionPathProvider solutionPathProvider)
        {
            _solutionPathProvider = solutionPathProvider;
            _fileSystemService = new FileSystemService();
            this.InitializeStuff();
        }

        private void InitializeStuff()
        {
            this.fileComboBox.Items.Clear();
            var solPath = _solutionPathProvider.GetSolutionPath();
            if (solPath == null)
            {
                this.fileComboBox.Items.Add("Open project first");
                this.fileComboBox.SelectedIndex= 0;
                this.runBtn.IsEnabled = false;
            }
            else
            {
                var fileNames = this._fileSystemService.GetFileNames(solPath);
                foreach (var file in fileNames)
                {
                    this.fileComboBox.Items.Add(file);
                }
                this.runBtn.IsEnabled= true;
            }

     
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.inputBox.Text = "siema";
        }
    }
}