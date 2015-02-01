using System;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;

namespace FrapsBenchConvert
{
    class Program
    {
        private const string FIND = "frametimes";

        private static void Main(string[] args)
        {
            //var con =
            //    new Converter(
            //        @"C:\Users\Smag\Documents\Visual Studio 2012\Projects\FrapsBenchConvert\FrapsBenchConvert\bin\Debug\PlanetSide2_x64 2015-01-24 22-01-27-19 frametimes.csv");
            //con.Convert();
            
            string[] files;
            if (args.Length > 0 && IsFile(args[0]))
                files = new[] {args[0]};
            else
                files = GetFiles(args[0]);

            foreach (var file in files)
                new Converter(file).Convert();
        }

        static bool IsFile(String FileName)
        {
            try
            {
                FileAttributes attr = File.GetAttributes(FileName);

                //detect whether its a directory or file
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    return false;
                else
                    return true;
            }
            catch (Exception e)
            {
                ErrorHandling.HandleException(e);
            }
        }

        static string[] GetFiles(string folder)
        {
            return Directory.GetFiles(folder, String.Format("*{0}*", FIND), System.IO.SearchOption.AllDirectories);
        }

        //static void ConvertFiles(string[] files)
        //{
        //    foreach (var file in files)
        //    {
        //        new Converter(file).Convert();
        //    }
        //}

        //static void Convert(string filename)
        //{
        //    var contents = File.ReadLines(filename).ToArray();
        //    double ms, msOld = 0, s, delta, fps;

        //    contents[0] = "Frame; Time (ms); Time (s); Delta; FPS";
        //    for(int i = 1; i < contents.Length; i++)
        //    {
        //        ms = double.Parse(contents[i].Split(',')[1], CultureInfo.InvariantCulture);
        //        ms = Math.Round(ms, 2);
        //        s = Math.Round(ms / 1000,2);
        //        delta = Math.Round(ms - msOld,2);
        //        fps = (delta != 0) ? Math.Round(1000/delta) : 0;
        //        msOld = ms;

        //        contents[i] = string.Format("{0};{1};{2};{3};{4}", i, ms, s, delta, fps);
        //    }
        //    Create(filename,contents);
        //}

        //static void Create(string filename, string[] contents)
        //{
        //    var path = filename.Replace("frametimes", "ft converted");
        //    var fs = File.Create(path);
        //    var bf = new StreamWriter(fs);
        //    foreach (var line in contents)
        //    {
        //        bf.WriteLine(line);
        //    }
        //    bf.Close();
        //    fs.Close();

        //    File.Delete(filename);
        //}
    }
}
