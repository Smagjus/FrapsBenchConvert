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
        private int[] frameNumber, seconds, framesPerSecond;
        private float[] timeMs, timeS, delta, fps;
        private string fileName;
        private int lineCount;

        public Converter(string fileName)
        {
            this.fileName = fileName;
            lineCount = File.ReadLines(fileName).Count() -1;

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
            CreateNewFile();
            DeleteOldFile();
        }

        private void CreateNewFile()
        {
            try
            {
                var path = fileName.Replace("frametimes", "ft converted");
                var stmWr = new StreamWriter(File.Create(path));
                var strBuffer = "";

                stmWr.WriteLine(GetHeadline());
                for (int i = 0; i < lineCount; i++)
                {
                    if(i < seconds.Length)
                        strBuffer = string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", frameNumber[i], timeMs[i], timeS[i], delta[i], fps[i],"", seconds[i], framesPerSecond[i]);
                    else
                        strBuffer = string.Format("{0};{1};{2};{3};{4}", frameNumber[i], timeMs[i], timeS[i], delta[i], fps[i]);
                    stmWr.WriteLine(strBuffer);
                }
                stmWr.Close();
            }
            catch (Exception e)
            {
                ErrorHandling.HandleException(e);
            }
        }

        private string GetHeadline()
        {
            string result = "";

            foreach (var name in columns)
            {
                result += name + ";";
            }
            return result;
        }

        private void FillArrays()
        {
            int secCount = 0;
            int lastFrame = 0;
            int thisFrame = 0;

            for (int i = 0; i < lineCount; i++)
            {
                timeS[i] = timeMs[i]/1000;
                if (i != lineCount -1)
                    delta[i] = timeMs[i + 1] - timeMs[i];
                else
                    delta[i] = delta[i-1];
                fps[i] = 1000/delta[i];

                timeS[i] = (float)Math.Round(timeS[i], 2);
                delta[i] = (float)Math.Round(delta[i], 2);
                fps[i] = (float)Math.Round(fps[i], 0);


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

        //Gets the values from the Fraps Frametime file and 
        //stores the values in an array
        private void ReadFile()
        {

            try
            {
                int lineNumber = 0;
                var sr = new StreamReader(fileName);
                sr.ReadLine();  //skip headline/first line
                while(!sr.EndOfStream)
                {
                    var buffer = sr.ReadLine();
                    var values = buffer.Split(',');
                    frameNumber[lineNumber] = Int32.Parse(values[0]);
                    timeMs[lineNumber] = (float) Math.Round(float.Parse(values[1], CultureInfo.InvariantCulture),2);

                    lineNumber++;
                }
                sr.Close();
            }
            catch (Exception e)
            {
                ErrorHandling.HandleException(e);
            }
        }

        private void DeleteOldFile()
        {
            try
            {
                File.Delete(fileName);
            }
            catch (Exception e)
            {
                ErrorHandling.HandleException(e);
            }
        }
    }
}
