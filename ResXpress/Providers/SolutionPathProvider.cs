using EnvDTE;
using System;
using System.Collections.Generic;
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
                return _dte.Solution.FullName;
            }

            return null; // No solution open
        }
    }
}
