using System;
using System.IO;

namespace WebUi
{
    public class Common
    {
        public Common()
        {

        }

        public string ReadFile(string fileName)
        {
            var currentDir = Path.GetDirectoryName(new Uri(typeof(Common).Assembly.CodeBase).LocalPath);

            return File.ReadAllText($"{currentDir}/{fileName}");
        }
    }
}