using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xpress.Logic.FileSystem
{
    public class InfoMessage
    {
        public string Text { get; set; }
        public int ErrorCount { get; set; } 

        public InfoStatus Status { get; set; }
    }

    public enum InfoStatus
    {
        Success,
        Failure,
        PartialFailure
    }
}
