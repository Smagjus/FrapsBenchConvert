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
        private int[] frameNumber, seconds,framesPerSecond;
        private float[] timeMs, timeS, delta, fps;
        private string fileName;
        private int lineCount;

        public Converter(string fileName)
        {
            this.fileName = fileName;
            lineCount = File.ReadLines(fileName).Count();

            frameNumber = new int[lineCount];
            fps = new float[lineCount];
            seconds = new int[lineCount];
            framesPerSecond = new int[lineCount];
            timeMs = new float[lineCount];
            timeS = new float[lineCount];
            delta = new float[lineCount];
        }

        public void Convert()
        {
            ReadFile();
            FillArrays();
        }

        private void FillArrays()
        {
            //float second = 0;
            int secCount = 0;
            int lastFrame = 0;
            int thisFrame = 0;

            for (int i = 0; i < lineCount; i++)
            {
                timeS[i] = timeMs[i]/1000;
                if (i != lineCount -1)
                    delta[i] = timeMs[i] - timeMs[i + 1];
                else
                    delta[i] = delta[i-1];
                fps[i] = 1000/delta[i];

                if (timeMs[i] > (secCount+1)*1000)
                {
                    seconds[secCount] = secCount + 1;
                    thisFrame = frameNumber[i];
                    framesPerSecond[secCount] = thisFrame - lastFrame;
                    lastFrame = thisFrame;
                    secCount++;
                }
            }

            Array.Resize(ref seconds, secCount);
            Array.Resize(ref framesPerSecond, secCount);
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
