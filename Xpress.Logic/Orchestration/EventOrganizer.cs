using System.Collections;
using System.Collections.Generic;
using Xpress.Logic.Decoders;

namespace Xpress.Logic.Orchestration
{
    public class EventOrganizer
    {
        private string solutionPath;
        private string partialPath;

        public EventOrganizer(string solPath, string partialPath) {
            this.solutionPath = solPath;
            this.partialPath = partialPath;
        }

        public IEnumerable<RWEvent> OrchestrateEvents(UserRequest request)
        {

            //ScoutFiles(result);

            //return result;

            return new List<RWEvent>();
        }


        // this will assign positions to records
        private IEnumerable<RWEvent> ScoutFiles(UserRequest request)
        {
            


            return new List<RWEvent>();
        }

        private string GetFilePath(Language language)
        {
            return solutionPath + partialPath.Replace("/", "\\") + "." + language.Name.ToLower() + ".resx";
        }

    }
}
