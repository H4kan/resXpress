using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xpress.Logic.Orchestration
{
    public class RWEvent
    {
        public string TargetFilePath { get; set; }

        public IEnumerable<RWRecord> Records { get; set; }

    }

    public class RWRecord
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public int Position { get; set; }

        public bool IsNew => Position == -1;

    }
}
