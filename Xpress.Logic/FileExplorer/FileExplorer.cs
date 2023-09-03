using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xpress.Logic.Orchestration;

namespace Xpress.Logic.FileExplorer
{
    public class Explorer
    {
        private Dictionary<string, List<string>> _allFiles;

        public void OpenAllFiles(IEnumerable<string> filePaths)
        {
            _allFiles = new Dictionary<string, List<string>>();
            foreach (string filePath in filePaths)
            {
                _allFiles.Add(filePath, File.ReadAllLines(filePath).ToList());
            }
        }

        public int SearchForPositionByKey(string key, string filePath)
        {
            var fileData = _allFiles[filePath];

            int i = 0;
            while (i < fileData.Count)
            {
                if (fileData[i].TrimStart(' ').StartsWith($"<data name=\"{key}\""))
                {
                    return i + 1;
                }
                i++;
            }

            return -1;
        }

        public int SearchForPositionByValue(string value, string filePath)
        {
            var fileData = _allFiles[filePath];

            int i = 0;
            while (i < fileData.Count)
            {
                if (fileData[i].TrimStart(' ').StartsWith($"<value>{value}</value>"))
                {
                    return i;
                }
                i++;
            }

            return -1;
        }

        public void DisposeAllFiles()
        {
            _allFiles.Clear();
        }

        public void PerformRWEvents(IEnumerable<RWEvent> events)
        {
            foreach(var rwEvent in events)
            {
                var targetFile = _allFiles[rwEvent.TargetFilePath];

                foreach(var rwRecord in rwEvent.Records)
                {
                    if (rwRecord.IsNew)
                    {
                        AddRecord(rwRecord.Key, rwRecord.Value, targetFile);
                    }
                    else
                    {
                        ChangeRecord(rwRecord.Position, rwRecord.Value, targetFile);
                    }
                }

                File.WriteAllLines(rwEvent.TargetFilePath, targetFile);
            }
        }

        private void ChangeRecord(int position, string value, List<string> fileData)
        {
            fileData[position] = $"  <value>{value}</value>";
        }

        private void AddRecord(string key, string value, List<string> fileData)
        {
            var firstLine = $"<data name=\"{key}\" xml:space=\"preserve\">";
            var secondLine = $"  <value>{value}</value>";
            var thirdLine = "</data>";

            var rootLine = fileData[fileData.Count - 1];
            fileData.RemoveAt(fileData.Count - 1);

            fileData.Add(firstLine);
            fileData.Add(secondLine);
            fileData.Add(thirdLine);
            fileData.Add(rootLine);
        }

        public string GetKeyAtPositon(int position, string filepath)
        {
            var targetLine = _allFiles[filepath][position];

            return String.Concat(targetLine.SkipWhile(c => c != '"').Skip(1).TakeWhile(c => c !=  '"'));
        }

    }
}
