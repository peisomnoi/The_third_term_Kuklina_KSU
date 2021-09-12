using System;
using System.Xml;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateXML(@"F:\Export6\pers.xml");
            Console.WriteLine("Файл XLM сгенерирован " + @"F:\Export6\pers.xml");
            GetTax(@"F:\Export6\pers.xml");
        }
        public static bool GenerateXML(string FileNameXML)
        {
            Random rnd = new Random();
            string[] SecondNames = new string[5] {"Иванов", "Петров", "Сидоров", "Навальный", "Путин" };
            string[] FirstNames = new string[5] { "Иван", "Петр", "Василий", "Алексей", "Владимир" };
            XmlDocument doc = new XmlDocument();
            var xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(xmlDeclaration);
            var root = doc.CreateElement("Sheet");
            for (int i = 0; i < 50; i++)
            {
                var persNode = doc.CreateElement("Person");
                    AddChildNode("IName", FirstNames[rnd.Next(0, 4)], persNode, doc);
                    AddChildNode("FName", SecondNames[rnd.Next(0, 4)], persNode, doc);
                    AddChildNode("Income", (rnd.Next(5000, 500000)).ToString(), persNode, doc);
                root.AppendChild(persNode);
            }
            doc.AppendChild(root);
            doc.Save(FileNameXML);
            return true ;
        }
        public static bool GetTax(string FileNameXML)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(FileNameXML);
            var root = doc.DocumentElement;
            foreach(XmlNode node in root.ChildNodes)
            {
                double Income = Convert.ToDouble(node.SelectSingleNode("Income").InnerText);
                double Tax;
                string IName = node.SelectSingleNode("IName").InnerText;
                string FName = node.SelectSingleNode("FName").InnerText;
                if (Income < 20000)
                {
                    Tax = Income * 0.12;
                }else if (Income>= 20000 && Income < 40000){
                    Tax = (Income-19999) * 0.20 + 19999 * 0.12;
                }
                else if (Income >= 40000)
                {
                    Tax = (Income - 39999) * 0.35 + 20000 * 0.20 + 19999 * 0.12;
                }
                else
                {
                    Tax = 0;
                }
                Console.WriteLine( FName + " " + IName + " Доход " + Income.ToString() + " Налог " + Tax.ToString());
            }
            return true;
        }
        private static void AddChildNode(string childName, string childText, XmlElement parentNode, XmlDocument doc)
        {
            var child = doc.CreateElement(childName);
            child.InnerText = childText;
            parentNode.AppendChild(child);
        }
    }
}
