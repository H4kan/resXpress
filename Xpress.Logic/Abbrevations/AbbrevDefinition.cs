using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xpress.Logic.Abbrevations
{
    public static class AbbrevDefinition
    {
        private static Dictionary<string, IEnumerable<string>> _allAbrevations = new Dictionary<string, IEnumerable<string>>();

       static AbbrevDefinition()
       {
            _allAbrevations.Add("NB", new List<string> { "NO" });
            _allAbrevations.Add("SV", new List<string> { "SE" });
        }

        public static string NonAbbrevated(string name)
        {
            if (_allAbrevations.Any(x => x.Key == name)) return name;

            var nonAbbrev = _allAbrevations.FirstOrDefault(x => x.Value.Contains(name));
            if (nonAbbrev.Key != null)
            {
                return nonAbbrev.Key;
            }
            else return name;
        }

    }
}
