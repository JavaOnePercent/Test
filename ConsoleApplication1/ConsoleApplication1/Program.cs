using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImapX;
using ImapX.Enums;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Weather();

        }

        private static string CheckSubject(string Subject)
        {
            Subject = Subject.ToLower();
            Subject = Regex.Replace(Subject, "[-.?!)(,: }{1234567890]", "");
            if ((Subject == "weather") || (Subject == "forecast") || (Subject == "forecast/daily"))
            {

            }
            else
            {
                Subject = "false";
            }
            return Subject;
        }

        public static void SendMail(string smtpServer, string from, string password, string mailto, string caption, string message, string attachFile = null)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new System.Net.Mail.MailAddress(from);
                mail.To.Add(new System.Net.Mail.MailAddress(mailto));
                mail.Subject = caption;
                mail.Body = message;
                if (!string.IsNullOrEmpty(attachFile))
                    mail.Attachments.Add(new System.Net.Mail.Attachment(attachFile));
                SmtpClient client = new SmtpClient();
                client.Host = smtpServer;
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(from.Split('@')[0], password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
                mail.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Mail.Send: " + e.Message);
            }
        }

        private static void Weather()
        {
            string City = "";
            string[] Body = { };
            WeatherTxt weatherTxt = new WeatherTxt();

            weatherTxt.DeleteBodyTxt();

            ImapClient client = new ImapClient("imap.gmail.com", true);

            if (client.Connect())
            {
                Console.WriteLine("connected");
                if (client.Login("nublikaskaweather@gmail.com", "123456789dD"))
                {
                    Console.WriteLine("logined");
                    client.Folders.Inbox.Messages.Download("ALL", MessageFetchMode.Full, 5);
                    foreach (Message message in client.Folders.Inbox.Messages)
                    {               
                        if (message.Seen != true)
                        {
                            Console.WriteLine("пытаюсь удалить txt и string");
                            weatherTxt.SetNewBody("");
                            weatherTxt.DeleteBodyTxt();
                            Console.WriteLine("удалил");
                            message.Seen = true;
                            if (CheckSubject(message.Subject) != "false")
                            {
                                Console.WriteLine(CheckSubject(message.Subject));
                                Console.WriteLine(message.From.Address.ToString());
                                City = Regex.Replace(message.Body.Text, "[-.?!)(,: }{1234567890]", "");
                                GetWeather(CheckSubject(message.Subject), City, ref weatherTxt);
                                SendMail("smtp.gmail.com",
                                         "nublikaskaweather@gmail.com",
                                         "123456789dD",
                                         message.From.Address.ToString(),
                                         CheckSubject(message.Subject),
                                         weatherTxt.getBody(),
                                         @"C:\Users\Владимир\Documents\Visual Studio 2015\Projects\ConsoleApplication1\ConsoleApplication1\bin\Debug\weather.txt"
                                         );
                                Console.WriteLine("сообщение отправлено");
                                Console.WriteLine(weatherTxt.getBody() + "\n--------------------------------------------------------------------------------");

                            }

                        }
                    }
                    int a = Console.Read();
                }
            }
        }

        private static void GetWeather(string method, string city, ref WeatherTxt weatherTxt)
        {
            //WeatherTxt weatherTxt = new WeatherTxt();
            string appId = File.ReadAllText(@"appId.txt");
            string url = "http://api.openweathermap.org/data/2.5/" + method + "?q=" + city + "&mode=xml&units=metric" + appId;
            string data = @"C:\Users\Владимир\Documents\Visual Studio 2015\Projects\ConsoleApplication1\ConsoleApplication1\bin\Debug\WeatherXml\" + Regex.Replace(System.DateTime.Now.ToLongTimeString(), "[:]", ".") + ".xml";
            Console.WriteLine(data);

            XDocument weather = new XDocument();
            weather = XDocument.Load(url);
            weather.Save(data);

            if (method == "weather")
            {
                foreach (XElement TemElement in weather.Element("current").Elements("temperature"))
                {
                    XAttribute valueAttribute = TemElement.Attribute("value");
                    weatherTxt.SetBody(valueAttribute.Value);                   
                }
                weatherTxt.BodyToTxt(weatherTxt.getBody());
            }
            else if (method == "forecast")
            {
                foreach (XElement TimeElement in weather.Element("weatherdata").Element("forecast").Elements("time"))
                {
                    XAttribute valueAttribute = TimeElement.Element("temperature").Attribute("value");
                    weatherTxt.SetBody(valueAttribute.Value);
                }
                weatherTxt.BodyToTxt(weatherTxt.getBody());
            }

            else if (method == "forecast/daily")
            {
                foreach (XElement TimeElement in weather.Element("weatherdata").Element("forecast").Elements("time"))
                {
                    XAttribute valueAttribute = TimeElement.Element("temperature").Attribute("day");
                    weatherTxt.SetBody(valueAttribute.Value);
                }
                weatherTxt.BodyToTxt(weatherTxt.getBody());
            }
        }
    }

}
