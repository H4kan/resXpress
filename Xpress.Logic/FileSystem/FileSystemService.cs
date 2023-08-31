using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Xpress.Logic.FileSystem
{
    public class FileSystemService
    {

        public IEnumerable<string> GetFileNames(string path)
        {
            var files = Directory.EnumerateFiles(path, $"*.resx", SearchOption.AllDirectories);

            var grouped = files.GroupBy(f => new { dir = Path.GetDirectoryName(f), name = GetSimpleFileName(f) });

            return grouped.Select(g => g.First()).Select(f => GetDisplayFileName(path, f));
        }

        public void ProcessFileChange(string inputText, string solPath, string fileGroup)
        {
            
        }

        private string GetDisplayFileName(string solPath, string path)
        {
            return (String.Concat(Path.GetDirectoryName(path).Skip(solPath.Length)) + "\\" + GetSimpleFileName(path)).Replace("\\", "/");
        }

        private string GetSimpleFileName(string path)
        {
            return String.Concat(Path.GetFileNameWithoutExtension(path).TakeWhile(c => c != '.'));
        }

    }
}