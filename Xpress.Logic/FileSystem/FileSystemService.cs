using System.Collections.Generic;

namespace Xpress.Logic.FileSystem
{
    public class FileSystemService
    {

        public IEnumerable<string> GetFileNames(string path)
        {
            return new List<string>()
            {
                "abc",
                "def",
                "fgh"
            };
        }
    }
}