using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Diagnostics;

namespace PatchBuilder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Patch Build Version: ");
            string buildver = Console.ReadLine();

            string curdir = Directory.GetCurrentDirectory();

            string updatelist = "C:\\Patch\\" + buildver + "\\updatelist.csv";
            string verfile = "C:\\Patch\\" + buildver + "\\ver.txt";

            if (!Directory.Exists("C:\\Patch\\" + buildver))
            {
                Directory.CreateDirectory("C:\\Patch\\" + buildver);
            }

            string[] allfiles = Directory.GetFiles(curdir, "*.*", SearchOption.AllDirectories);

            using (StreamWriter ver = File.CreateText(verfile))
            {
                ver.WriteLine(buildver);
            }

            using (StreamWriter updater = File.CreateText(updatelist))
            {
                foreach (var files in allfiles)
                {
                    string md5 = CalculateMD5(files);

                    string pattern = curdir + "\\";
                    string replace = null;

                    string fname = files.Replace(pattern, replace);

                    string[] split = fname.Split('\\');
                    string firstPart = string.Join("\\", split.Take(split.Length - 1));
                    string lastpart = split.Last();

                    updater.Write(fname + ",Client\\" + firstPart + "," + lastpart + "," + buildver + "," + md5 + "\n");

                    // Console.Write(fname + "," + firstPart + "," + lastpart + "," + buildver + "\n");
                }
            }

            Process.Start(@"C:\Patch\" + buildver + "\\");
        }

        private static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}