using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResXpress.Providers
{
    public class SolutionPathProvider
    {
        private readonly DTE _dte;

        public SolutionPathProvider(DTE dte)
        {
            _dte = dte;
        }

        public string GetSolutionPath()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            if (_dte?.Solution != null && _dte.Solution.IsOpen)
            {
                var path = _dte.Solution.FullName;
                if (!Directory.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                }
                return path;

            }

            return null; // No solution open
        }
    }
}
