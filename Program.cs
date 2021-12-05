using TextCopy;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace PaimonImporter
{
    class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string path = "AppData\\LocalLow\\miHoYo\\Genshin Impact\\";
            string fullPath = Path.Combine(dir, path);
            string file = Path.Combine(fullPath, "output_log.txt");

            if (!File.Exists(file))
            {
                Console.WriteLine("Cannot find file. Please open Genshin Impact, navigate to the wish history and restart this program.");
                Console.ReadLine();
                return 1;
            }

            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                string fileContents;
                using (StreamReader reader = new(stream))
                {
                    fileContents = reader.ReadToEnd();
                }

                Match match = Regex.Match(fileContents, "OnGetWebViewPageFinish.*log");
                if (!match.Success)
                {
                    Console.WriteLine("Cannot find the wish history url! Make sure to open the wish history first and run this again.");
                    Console.ReadLine();
                    return 1;
                }

                string url = Regex.Replace(match.Value, "OnGetWebViewPageFinish:", "");
                Console.WriteLine("The URL you can submit to paimon.moe is: ");
                Console.WriteLine(url);
                ClipboardService.SetText(url);
                Console.WriteLine("\n");
                Console.WriteLine("The URL is copied to your clipboard, but is also displayed for your convenience.");
                Console.WriteLine("Happy importing!");
                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
            }

            return 0;
        }
    }
}
