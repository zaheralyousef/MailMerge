using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MailMerge
{
    class Program
    {
        static string sourceXMLFilePath = @"custom_data.xml";
        static string sourceMailTemplatePath = @"mail_template.txt";
        static void Main(string[] args)
        {
            // Read the file as one string.
            string mailTemplate = File.ReadAllText(sourceMailTemplatePath);

            //Load xml
            XDocument xdoc = XDocument.Load(sourceXMLFilePath);

            //Run query
            var orders = (from customData in xdoc.Descendants("Order")
                          select new
                          {
                              orderId = customData.Attribute("id").Value,
                              orderData = customData.Descendants()
                          }).ToList();

            //Loop through results
            foreach (var order in orders)
            {
                Console.WriteLine();
                Console.WriteLine("=======================");
                Console.WriteLine("Order:  " + order.orderId);
                Console.WriteLine("=======================");
                mailTemplate = mailTemplate.Replace("%salutation%", order.orderData.First(x => x.Name == "salutation").Value);
                
                foreach(var item in order.orderData)
                {
                    mailTemplate = mailTemplate.Replace('%' + item.Name.ToString() + '%', item.Value);
                }
                    
                Console.WriteLine(mailTemplate);
                
            }
        }

    }
}
