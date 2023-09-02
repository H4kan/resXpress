using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xpress.Logic.Decoders
{
    public class InputDecoder
    {
        private IEnumerable<Language> allLanguages;

        public InputDecoder(IEnumerable<string> languages) {

            allLanguages = languages.Select(l => new Language()
            {
                Name = l.ToUpper()
            });

        }


        public UserRequest Decode(string inputText)
        {
            var splittedLines = String.Concat(inputText.Where(c => c != '\r')).Split('\n');
            var request = new UserRequest()
            {
                Records = new List<Record>() { },
                Languages = this.allLanguages
            };

            var unusedLanguages = new List<Language>();
            unusedLanguages.AddRange(this.allLanguages);
            request.Records.Add(new Record()
            {
                Key = null,
                Values = new List<LocalizedRecord> { },
            });

            foreach (var line in splittedLines)
            {
                if (line.Length == 0)
                {
                    request.Records.Add(new Record()
                    {
                        Key = null,
                        Values = new List<LocalizedRecord> { },
                    });
                    unusedLanguages.Clear();
                    unusedLanguages.AddRange(this.allLanguages);
                    continue;
                }
                var lineLang = this.allLanguages.FirstOrDefault(lang => line.StartsWith(lang.Name));
                if (lineLang != null)
                {
                    if (unusedLanguages.Any(l => l.Name == lineLang.Name))
                    {
                        request.Records.Last().Values.Add(new LocalizedRecord()
                        {
                            Language = lineLang,
                            Value = String.Concat(line.SkipWhile(c => c != ':').SkipWhile(c => !Char.IsLetterOrDigit(c))),
                        });
                        unusedLanguages.RemoveAll(l => l.Name == lineLang.Name);
                    }
                    else
                    {
                        request.Records.Add(new Record()
                        {
                            Key = null,
                            Values = new List<LocalizedRecord> {
                                new LocalizedRecord()
                                {
                                    Language = lineLang,
                                    Value = String.Concat(line.SkipWhile(c => c != ':').SkipWhile(c => !Char.IsLetterOrDigit(c))),
                                }
                            },
                        });
                        unusedLanguages.Clear();
                        unusedLanguages.AddRange(this.allLanguages);
                        unusedLanguages.RemoveAll(l => l.Name == lineLang.Name);
                    }
                }
                
            }

            request.Records = request.Records.Where(r => r.Values.Count > 0).ToList();
            

            return request;
        }


    }
}
