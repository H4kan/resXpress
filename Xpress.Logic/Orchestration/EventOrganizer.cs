using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xpress.Logic.Decoders;
using Xpress.Logic.FileExplorer;

namespace Xpress.Logic.Orchestration
{
    public class EventOrganizer
    {
        private string solutionPath;
        private string partialPath;

        private Explorer _fileExplorer;

        public EventOrganizer(Explorer fileExplorer, string solPath, string partialPath) {
            this.solutionPath = solPath;
            this.partialPath = partialPath;
            _fileExplorer = fileExplorer;
        }

        public IEnumerable<RWEvent> OrchestrateEvents(UserRequest request)
        {
            var recordsHandler = new Dictionary<string, List<RWRecord>>();
            foreach(var lng in request.Languages)
            {
                recordsHandler.Add(lng.Name, new List<RWRecord>());
            }

            this._fileExplorer.OpenAllFiles(request.Languages.Select(l => GetFilePath(l)));

            var currentRecordLRecords = new List<(RWRecord, Language)>();

            foreach(var record in request.Records)
            {
                currentRecordLRecords.Clear();
                foreach(var lRecord in record.Values)
                {
                    var rwRecord = new RWRecord();
                    currentRecordLRecords.Add((rwRecord, lRecord.Language));
                    if (record.Key != null)
                    {
                        rwRecord.Key = record.Key;
                        rwRecord.Position = GetKeyRecordPosition(record.Key, GetFilePath(lRecord.Language));
                    }
                    else
                    {
                        rwRecord.Position = GetValueRecordPosition(lRecord.Value, GetFilePath(lRecord.Language));
                        if (rwRecord.Position != -1)
                        {
                            rwRecord.Key = GetKey(rwRecord.Position, GetFilePath(lRecord.Language));
                        }
                    }
                    rwRecord.Value = lRecord.Value;
                    recordsHandler[lRecord.Language.Name].Add(rwRecord);
                    
                }

                TransferKeys(currentRecordLRecords);
            }

            var events = request.Languages.Select(lng => new RWEvent()
            {
                Records = recordsHandler[lng.Name],
                TargetFilePath = GetFilePath(lng)
            });


            return events;
        }


        private string GetFilePath(Language language)
        {
            return solutionPath + partialPath.Replace("/", "\\") + "." + language.Name.ToLower() + ".resx";
        }

        // -1 if new
        private int GetValueRecordPosition(string record, string filePath)
        {
            return _fileExplorer.SearchForPositionByValue(record, filePath);
        }


        // -1 if new
        private int GetKeyRecordPosition(string key, string filePath)
        {
            return _fileExplorer.SearchForPositionByKey(key, filePath);
        }


        public string GetKey(int position, string filePath)
        {
            return _fileExplorer.GetKeyAtPositon(position - 1, filePath);
        }

        public void TransferKeys(IEnumerable<(RWRecord, Language)> relatedRecords)
        {
            var anyKey = relatedRecords.Select(r => r.Item1).FirstOrDefault(r => r.Key != null);
            if (anyKey != null)
            {
                foreach(var record in relatedRecords.Where(r => r.Item1.Key == null))
                {
                    record.Item1.Key = anyKey.Key;
                    record.Item1.Position = GetKeyRecordPosition(record.Item1.Key, GetFilePath(record.Item2));
                }
            }
        }
    }
}
