using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Diagnostics;
using BusinessLogic;

namespace IO
{
    public class PageInputXml
    {
        static String delimStr = " ";
        static char[] delimChars = delimStr.ToCharArray();
        public static HUB LoadInput(String pathname)   // Load XML File and parse info.
        {
            #region Open XML File
            XmlDocument pagInputXml = new XmlDocument();   // Opening XML File
            try
            {
                using (TextReader reader = File.OpenText(pathname))
                {
                    pagInputXml.Load(reader);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
#endregion

            HUB LineIn = new HUB(); // Creating HUB Object

            #region Format
            XmlNodeList formatnl = pagInputXml.SelectNodes("//pageinput/format"); // Finding Format Node
            if (formatnl.Count == 1)
            {
                String formatstr = formatnl[0].FirstChild.Value;
                switch (formatstr)
                {
                    case "Fill":
                        {
                            LineIn.format = Format.Fill;
                            break;
                        }
                    case "FillSoft":
                        {
                            LineIn.format = Format.FillSoft;
                            break;
                        }
                    case "FillAdjust":
                        {
                            LineIn.format = Format.FillAdjust;
                            break;
                        }
                    case "LineMoment":
                        {
                            LineIn.format = Format.LineMoment;
                            break;
                        }
                    case "FillSet":
                        {
                            LineIn.format = Format.FillSet;
                            break;
                        }
                    default:
                        {
                            throw new Exception("Unknown format: " + formatstr);
                        }
                }
            }
            else
            {
                throw new Exception("Xml error");
            }
            #endregion
            #region Wrap
            XmlNodeList wrapnl = pagInputXml.SelectNodes("//pageinput/wrap"); // Finding Wrap node
            if (formatnl.Count == 1)
            {
                LineIn.wrap = Int32.Parse(wrapnl[0].InnerText);
            }
            else
            {
                throw new Exception("Xml error");
            }
            #endregion
            #region WrapSoft
            XmlNodeList wrapsoftnl = pagInputXml.SelectNodes("//pageinput/wrapsoft"); // Finding WrapSoft node
            if (wrapsoftnl.Count == 1)
            {
                LineIn.wrapSoft = Int32.Parse(wrapsoftnl[0].InnerText);
            }
            else
            {
                throw new Exception("Xml error");
            }
            #endregion
            #region Moment
            XmlNodeList columnmomentnl = pagInputXml.SelectNodes("//pageinput/columnmoment"); // Finding Moment node
            if (columnmomentnl.Count == 1)
            {
                LineIn.columnMoment = Int32.Parse(columnmomentnl[0].InnerText);
            }
            else
            {
                throw new Exception("Xml error");
            }
            #endregion
            #region Words
            XmlNodeList wordsnl = pagInputXml.SelectNodes("//pageinput/words"); // Finding Words node

            String text = wordsnl[0].InnerText;
            String[] textParts;
            textParts = text.Split(delimChars[0]);
            foreach(String s in textParts)
            {
                LineIn.words.Add(s);
            }
#endregion

            return LineIn;
        }
    }
}
