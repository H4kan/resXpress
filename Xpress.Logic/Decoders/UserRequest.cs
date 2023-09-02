using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xpress.Logic.Decoders
{
    public class UserRequest
    {
        public List<Record> Records { get; set; }

        public IEnumerable<Language> Languages { get; set; }
    }


    public class Record
    {
        public string Key { get; set; }

        public List<LocalizedRecord> Values { get; set; }
    }

    public class LocalizedRecord
    {
        public Language Language { get; set; }

        public string Value { get; set; }
    }

    public class Language
    {
        public string Name { get; set; } 
    }
}
