using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApplication1
{
    class WeatherTxt
    {
        private FileStream BodyTxt;
        private string BodyStr = "";

        public void SetBody(string newBodyStr)
        {
            BodyStr = BodyStr + "\n" + newBodyStr;
        }

        public void SetNewBody(string newBodyStr)
        {
            BodyStr = newBodyStr;
        }

        public string getBody()
        {
            return BodyStr;
        }

        public FileStream getBodyTxt()
        {
            return BodyTxt;
        }

        public void BodyToTxt(string newBodyStr)
        {
            BodyTxt = new FileStream(@"weather.txt", FileMode.Append);
            StreamWriter writer = new StreamWriter(BodyTxt);
            writer.WriteLine(BodyStr);
            writer.Close();
        }

        public void DeleteBodyTxt()
        {
            File.Delete(@"weather.txt");
        }
    }
}
