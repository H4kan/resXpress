using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xpress.Logic.Decoders;

namespace Xpress.Logic.FileSystem
{
    public class FileSystemService
    {

        public IEnumerable<string> GetFileNames(string path, out IEnumerable<string> languages)
        {
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


            return new InfoMessage() { 
                Text = "All records imported successfully", 
                Status = InfoStatus.Success };
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