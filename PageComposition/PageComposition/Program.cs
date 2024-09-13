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
using IO;

namespace PageComposition {
    class Program
    {
        static void Main(String[] args)
        {
            try
            {  // do not remove this try-catch statement, do not add any code called outside try-block





                string printTime = "Y"; // TO DELETE
                Stopwatch sw = new Stopwatch();
                if (printTime == "Y")
                { 
                    sw.Start();
                } // TO DELETE





                HUB lineInput = PageInputXml.LoadInput("input.xml");
                lineInput.VerifyWords();        
                switch (lineInput.format.ToString())
                {
                    case "Fill":
                        {
                            Fill newFormat = new Fill(lineInput.format, lineInput.wrap, lineInput.definiteWords);
                            newFormat.Final();
                            break;
                        }
                    case "FillSoft":
                        {
                            FillSoft newFormat = new FillSoft(lineInput.format, lineInput.wrap, lineInput.wrapSoft, lineInput.definiteWords);
                            newFormat.Final();
                            break;
                        }
                    case "FillAdjust":
                        {
                            FillAdjust newFormat = new FillAdjust(lineInput.format, lineInput.wrap, lineInput.definiteWords);
                            newFormat.Final();
                            break;
                        }
                    case "LineMoment":
                        {
                            LineMoment newFormat = new LineMoment(lineInput.format, lineInput.wrap, lineInput.columnMoment, lineInput.definiteWords);
                            newFormat.Final();
                            break;
                        }
                    case "FillSet":
                        {
                            FillSet newFormat = new FillSet(lineInput.format, lineInput.wrap, lineInput.definiteWords);
                            newFormat.Final();
                            break;
                        }
                    default:
                        {
                            throw new Exception("Unknown format: " + lineInput.format.ToString());
                        }

                }






                if (printTime == "Y") // TO DELETE
                {
                    sw.Stop();
                    if (sw.ElapsedMilliseconds < 500) { Console.WriteLine("WITHIN TIME !!!!! {0}", sw.ElapsedMilliseconds.ToString()); }
                    else { Console.WriteLine("TOOK TOO LONG !!!!! {0}", sw.ElapsedMilliseconds.ToString()); }
                    Console.ReadKey();
                } // TO DELETE




            }
            catch (Exception e)
            {
                // do not modify the code in this catch block except to comment two lines at end of block
                //Console.WriteLine("Unhandled exception: " + e.Message);
                // comment following two lines before final build and submission
                //Console.WriteLine("Press any key to exit program.");
                //Console.ReadKey();
            }
        }
    }
}
