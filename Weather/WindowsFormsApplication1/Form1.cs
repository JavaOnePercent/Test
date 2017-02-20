using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private string mode = "&mode=xml";
        private string units = "metric";
        private string city = "";
        private string valueStr = "";
        private string cityStr = "";
        private string idStr = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void cityChange_TextChanged(object sender, EventArgs e)
        {
           

        }

        private void button1_Click(object sender, EventArgs e)
        {
            city = cityChange.Text;
            string url = "http://api.openweathermap.org/data/2.5/weather?q=" + city + mode + "&units=" + units + "&appid=01262a407ed6af1b08040793da605511";
            XDocument weather = new XDocument();
            if (city != "")
            {
                weather = XDocument.Load(url);

                foreach (XElement TemElement in weather.Element("current").Elements("city"))
                {
                    XAttribute idAttribute = TemElement.Attribute("id");
                    XAttribute cityAttribute = TemElement.Attribute("name");
                    idStr = (string)idAttribute;
                    cityStr = (string)cityAttribute;
                    label1.Text = "id: " + idStr + "\ncity: " + cityStr;

                }

                foreach (XElement TemElement in weather.Element("current").Elements("temperature"))
                {
                    XAttribute valueAttribute = TemElement.Attribute("value");
                    valueStr = valueAttribute.Value;
                    if (comboBox1.SelectedItem.ToString() == "Фаренгейты")
                    {
                        //double valueDbl = Convert.ToDouble(valueStr); 
                        double valueDbl = Double.Parse(valueStr);                       
                        valueDbl = valueDbl * 1.8 + 32;
                        valueStr = Convert.ToString(valueDbl);

                    }
                    label1.Text += "\ntemperature: " + valueStr;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            units = comboBox1.SelectedItem.ToString();
            if (units == "Кельвины")
            {
                units = "";
            }
            else
            {
                units = "metric";
            }
        }
    }
}
