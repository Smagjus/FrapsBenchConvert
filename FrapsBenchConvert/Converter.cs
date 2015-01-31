using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrapsBenchConvert
{
    //Converter version #2
    class Converter
    {
        private string[] columns = {"Frame#", "Time (ms)", "Time (s)", "Delta", "FPS", "Seconds", "Frames per second"};
        private int[] frameNumber, fps, seconds,framesPerSecond;
        private float[] timeMs, timeS, delta;
        private string fileName;

        public Converter(string fileName)
        {
            this.fileName = fileName;
            var lineCount = File.ReadLines(fileName).Count();

            frameNumber = new int[lineCount];
            fps = new int[lineCount];
            seconds = new int[lineCount];
            framesPerSecond = new int[lineCount];
            timeMs = new float[lineCount];
            timeS = new float[lineCount];
            delta = new float[lineCount];
        }

        public void Convert()
        {
            ReadFile();
        }

        public void ReadFile()
        {
            string buffer;
            int lineNumber = 0;
            var sr = new StreamReader(fileName);
            sr.ReadLine();                          //skip headline/first line
            while(!sr.EndOfStream)
            {
                buffer = sr.ReadLine();
                var values = buffer.Split(',');
                frameNumber[lineNumber] = Int32.Parse(values[0]);
                timeMs[lineNumber] = float.Parse(values[1], CultureInfo.InvariantCulture);

                lineNumber++;
            }
        }


    }
}
