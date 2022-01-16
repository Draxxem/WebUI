using System;
using System.IO;

namespace WebUi
{
    public class Common
    {
        public Common()
        {

        }

        public string ReadFile(string dir, string fileName)
        {
            

            return File.ReadAllText($"{dir}/{fileName}");
        }
    }
}