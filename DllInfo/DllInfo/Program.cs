using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace DllInfo
{
    class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage GetBuildType <assemblyName>");

                return 2;
            }

            return BuildFind.GetBuildType(args[0]);
        }

        static int CheckEnviroment(string[] args)
        { 
            string inputPath = string.Empty;

            if (args.Length == 1 && Directory.Exists(args[0]))
            {
                inputPath = args[0];
            }
            else
            {
                Console.WriteLine("Input path does not exist. Check input arguments.");

                return 2;
            }

            string[] searchExtensions = { ".dll", ".exe", ".lib" };

            var files = Directory
                        .GetFiles(inputPath, "*.*", SearchOption.TopDirectoryOnly)
                        .Where(file => searchExtensions.Any(ext => file.ToLower().Contains(ext)))
                        .ToList();

            uint releaseCount = 0;

            uint debugCount = 0;

            StreamWriter sw = new StreamWriter("output.txt", false);

            foreach (var file in files)
            {
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(file);

                sw.Write(file);
                sw.Write("\t\t");

                if (fileVersionInfo.IsDebug)
                {
                    sw.Write("Debug");
                    debugCount++;
                }
                else
                {
                    sw.Write("Release");
                    releaseCount++;
                }

                sw.Write(Environment.NewLine);
            }

            sw.Write(Environment.NewLine);
            sw.WriteLine("Debug: " + debugCount.ToString());
            sw.WriteLine("Release: " + releaseCount.ToString());
            sw.Flush();
            sw.Close();
            sw.Dispose();

            Console.WriteLine("Output successfully written to: " + new FileInfo("output.txt").FullName);

            return 0;
        }
    }
}