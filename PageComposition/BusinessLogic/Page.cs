using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class Page
    {
        public void ListToText(List<String> list)
        {
            using(StreamWriter sw = new StreamWriter("page.txt"))
            {
                foreach(string s in list)
                {
                    sw.Write(s + "\n");
                }
            }
        }
    }
}
