using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Weather
{
    class Program
    {
        static void Main(string[] args)
        {
            string city = "Yekaterinburg";
            string mode = "&mode=xml";
            string url = "http://api.openweathermap.org/data/2.5/weather?q=" + city + mode + "&units=metric&appid=01262a407ed6af1b08040793da605511";
            XDocument weather = XDocument.Load(url);

            foreach (XElement phoneElement in weather.Element("current").Elements("city"))
            {
                XAttribute idAttribute = phoneElement.Attribute("id");
                XAttribute cityAttribute = phoneElement.Attribute("name");

                if (idAttribute != null && cityAttribute != null)
                {
                    Console.WriteLine("id: " + idAttribute.Value);
                    Console.WriteLine("Город: " + cityAttribute.Value);
                }
            }

            foreach (XElement phoneElement in weather.Element("current").Elements("temperature"))
            {
                XAttribute valueAttribute = phoneElement.Attribute("value");

                if (valueAttribute != null)
                {
                    Console.WriteLine("value: " + valueAttribute.Value);
                }
            }
            Console.ReadKey();


        }
    }
}
