using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xpress.Logic.Decoders;
using Xpress.Logic.Orchestration;

namespace Xpress.Logic.FileSystem
{
    public class FileSystemService
    {

        public FileExplorer.Explorer _fileExplorer = new FileExplorer.Explorer();

        public IEnumerable<string> GetFileNames(string path, out IEnumerable<string> languages)
        {
            if (!Directory.Exists(path))
            {
                path = Path.GetDirectoryName(path);
            }
            var files = Directory.EnumerateFiles(path, $"*.resx", SearchOption.AllDirectories);

            var grouped = files.GroupBy(f => new { dir = Path.GetDirectoryName(f), name = GetSimpleFileName(f) });

            languages = files.Select(f => this.GetLanguage(f)).Distinct();

            return grouped.Select(g => g.First()).Select(f => GetDisplayFileName(path, f));
        }

        public InfoMessage ProcessFileChange(string inputText, string solPath, string fileGroup, IEnumerable<string> languages)
        {
            var decorder = new InputDecoder(languages);
            var request = new UserRequest();
            try
            {
                request = decorder.Decode(inputText);
            }
            catch
            {
                return new InfoMessage() { 
                    Text = "Critical error occured while importing records", 
                    Status = InfoStatus.Failure };
            }
            var eventOrganizer = new EventOrganizer(_fileExplorer, solPath, fileGroup);

            IEnumerable<RWEvent> rwEvents = new List<RWEvent>();
            try
            {
                rwEvents = eventOrganizer.OrchestrateEvents(request);
            }
            catch
            {
                return new InfoMessage()
                {
                    Text = "Critical error occured while importing records",
                    Status = InfoStatus.Failure
                };
            }

            var resultMsg = new InfoMessage();
            var notApplicable = rwEvents.SelectMany(r => r.Records).Where(r => r.Key == null).Count();

            rwEvents = FilterOutUnApplicable(rwEvents);
            _fileExplorer.PerformRWEvents(rwEvents);
            _fileExplorer.DisposeAllFiles();



            resultMsg.Text = notApplicable > 0 ? "Some records couldn't be imported" : "All records imported successfully";
            resultMsg.Status = notApplicable > 0 ? InfoStatus.PartialFailure : InfoStatus.Success;


            return resultMsg;
        }

        public IEnumerable<RWEvent> FilterOutUnApplicable(IEnumerable<RWEvent> rwEvents)
        {
            foreach (var rwEvent in rwEvents)
            {
                rwEvent.Records = rwEvent.Records.Where(x => x.Key != null);
            }
            return rwEvents.Where(r => r.Records.Count() > 0);
        }

        private string GetDisplayFileName(string solPath, string path)
        {
            return (String.Concat(Path.GetDirectoryName(path).Skip(solPath.Length)) + "\\" + GetSimpleFileName(path)).Replace("\\", "/");
        }

        private string GetSimpleFileName(string path)
        {
            return String.Concat(Path.GetFileNameWithoutExtension(path).TakeWhile(c => c != '.'));
        }

        private string GetLanguage(string path)
        {
            return String.Concat(Path.GetFileNameWithoutExtension(path).SkipWhile(c => c != '.').Skip(1));
        }

    }
}