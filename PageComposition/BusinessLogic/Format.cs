using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class Fill
    {
        
        #region Field Variables
        protected Format format;
        protected int  wrap = 0;
        protected List<String> ToPrint;
        protected List<String> ToWork; 
        #endregion

        #region Construstors
        protected Fill()
        {
            ToWork = new List<string>();
        }
        public Fill(Format format, int wrap, List<String> words)
        {
            this.format = format;
            this.wrap = wrap;
            this.ToWork = words;
        } // Main Constructor
        #endregion

        #region Methods
        protected virtual bool IsOverflown(int currentLength, int lengthToAdd, int wrap)
        {
            if(currentLength + lengthToAdd > wrap)
            {
                return true;
            }
            else{ return false; }
        }
        protected virtual List<String> GoGo(int wrap, List<String> List)
        {
            List<String> result = new List<String>();
            int currentIndex = 1;
            string sendToPrint = List[0];
            int currentLength = sendToPrint.Length;
            do
            {
                if (currentLength == 0)
                {   
                        sendToPrint = List[currentIndex];
                        currentLength = sendToPrint.Length;
                        currentIndex++;
                }
                else
                {
                    if (currentIndex == List.Count)
                    {
                        result.Add(sendToPrint);
                        break;
                    }
                    else if (!IsOverflown(currentLength, List[currentIndex].Length + 1, wrap))
                    {
                        sendToPrint += " " + List[currentIndex];
                        currentLength = sendToPrint.Length;
                        currentIndex++;
                    }
                    else
                    {
                        result.Add(sendToPrint);
                        sendToPrint = "";
                        currentLength = 0;
                    }
                }
            } while(currentIndex <= List.Count);
            return result;
        }
        #endregion
        
        public virtual void Final()
        {
            ToPrint = GoGo(wrap, ToWork);
            File();
            //Console.WriteLine(ToString()); // DELETE
        }
        protected void File()
        {
            using (StreamWriter sw = new StreamWriter("page.txt"))
            {
                foreach (string s in ToPrint)
                {
                    sw.Write(s + "\n");
                }
            }
        }
    }

    public class FillSoft : Fill
    {
        #region Child-Specific Variable + Constructor
        private readonly int wrapSoft = 0;
        private int linesCompleted = 0;
        private List<String> backup = new List<string>();
        private List<String> current = new List<string>();      
        public FillSoft(Format format, int wrap, int wrapSoft, List<String> words)
        {
            this.format = format;
            this.wrap = wrap;
            this.wrapSoft = wrapSoft;
            this.ToWork = words;
        }
        #endregion

        #region Methods
        private string AreWeDone(List<string> list)
        {
            int half = list.Count / 2;
            if(half == linesCompleted)
            {
                return "YES";
            }
            else if(half > linesCompleted)
            {
                backup = current;
                return "NO";
            }
            else if(half < linesCompleted)
            {
                return "PRE";
            }
            else
            {
                return "";
            }
        }
        private int wrapFinder(List<String> list, int lines)
        {
            if(list.Count < lines)
            {
                return wrap;
            }
            else
            {
                return wrapSoft;
            }
        }
        private List<String> GoGoSoft(int wrap, int wrapSoft, List<String> List)
        {
            List<String> result = new List<string>();

            int currentIndex = 1;
            string sendToPrint = List[0];
            int currentLength = sendToPrint.Length;
            do
            {
                if (currentLength == 0)
                {
                    sendToPrint = List[currentIndex];
                    currentLength = sendToPrint.Length;
                    currentIndex++;
                }
                else
                {
                    if (currentIndex == List.Count)
                    {
                        result.Add(sendToPrint);
                        break;
                    }
                    else if (!IsOverflown(currentLength, List[currentIndex].Length + 1, wrapFinder(result, linesCompleted)))
                    {
                        sendToPrint += " " + List[currentIndex];
                        currentLength = sendToPrint.Length;
                        currentIndex++;
                    }
                    else
                    {
                        result.Add(sendToPrint);
                        sendToPrint = "";
                        currentLength = 0;
                    }
                }
            } while (currentIndex <= List.Count);
            return result;
        }
        #endregion

        public override void Final()
        {
            current = GoGo(wrapSoft, ToWork);
            while(AreWeDone(current) == "NO")
            {
                linesCompleted++;
                current = GoGoSoft(wrap, wrapSoft, ToWork);            
            }
            if(AreWeDone(current) == "YES")
            {
                ToPrint = current;
            }
            else if (AreWeDone(current) == "PRE")
            {
                ToPrint = backup;
            }
            File();
        }
    }

    public class FillAdjust : Fill
    {
        #region Child-Specific Variable + Constructor
        private List<String> AdjustToWork;
        public FillAdjust(Format format, int wrap, List<String> words)
        {
            this.format = format;
            this.wrap = wrap;
            this.ToWork = words;
        }
        #endregion

        #region Methods
        private bool Distance(string str, int wrap, out int result) // Definitely Works
        {
            string[] arr = str.Split(' ');
            result = (wrap - str.Length) + (arr.Length - 1);
            if(result > (0 + arr.Length - 1))
            {
                return true;
            }
            else if(result < (0 + arr.Length - 1))
            {
                return false;
            }
            else { return false; }
        }
        private List<int> SpaceCombo(string str, int dist) // Definitely Works
        {
            int linesDone = 0;
            string[] s = str.Split(' ');
            int spaceCount = s.Length - 1;

            #region Rounding Up
            List<int> spaceComboUp = new List<int>();
            float mainFloat = (float)(dist) / spaceCount;
            int main = 0;
            int t = (int)mainFloat;
            if (t == mainFloat)
            {
                main = t;
            }
            else
            {
                main = t + 1;
            }
            int last = dist - (main * (spaceCount - 1));
            for (int i = 0; i < spaceCount; i++)
            {
                if (i == spaceCount - 1)
                {
                    spaceComboUp.Add(last);
                }
                else
                {
                    spaceComboUp.Add(main);
                }
            }
            #endregion

            #region Rounding Down
            List<int> spaceComboDown = new List<int>();
            int main2 = dist / spaceCount;
            int last2 = dist - (main2 * (spaceCount - 1));
            for (int i = 0; i < spaceCount; i++)
            {
                if (i == spaceCount - 1)
                {
                    spaceComboDown.Add(last2);
                }
                else
                {
                    spaceComboDown.Add(main2);
                }
            }
            #endregion

            #region Return Block
            int upRange = spaceComboUp.Max() - spaceComboUp.Min();
            bool AllPos = true;
            foreach(int i in spaceComboUp)
            {
                if(i < 1)
                {
                    AllPos = false;
                }
            }

            int downRange = spaceComboDown.Max() - spaceComboDown.Min();
            if (upRange <= downRange && AllPos)
            {
                spaceComboUp.Sort();
                spaceComboUp.Reverse();
                linesDone++;
                return spaceComboUp;
            }
            else
            {
                spaceComboDown.Sort();
                spaceComboDown.Reverse();
                linesDone++;
                return spaceComboDown;
            }
            #endregion
        }
        private void Rank(string str, out List<String> outString, out List<int> outInt) // Probably Works
        {
            VowelChecker vc = new VowelChecker();
            List<String> keys = new List<string>();
            List<int> values = new List<int>();
            string[] temp = str.Split(' ');

            for (int i = 0; i < temp.Length - 1; i++)
            {
                int value = 0;
                string key = temp[i] + " " + temp[i + 1];
                value += vc.ContainsHowManyVowel(temp[i]) + vc.ContainsHowManyVowel(temp[i + 1]);
                keys.Add(key);
                values.Add(value);
            }
            outString = keys; 
            outInt = values;
        }
        private string strHelper(string s, int spaceCount) // Definitely Works
        {
            string[] arr = s.Split(' ');
            string str = arr[0];
            for(int i = 0; i < spaceCount; i++)
            {
                str += " ";
            }
            str += arr[1];
            return str;
        }
        private List<String> GoGoAdjust(List<String> list)
        {
            List<String> result = new List<string>();
            foreach(string s in list)
            {
                int distance = 0;
                bool Work = Distance(s, wrap, out distance);
                StringBuilder str = new StringBuilder(s);

                if (Work && s.Contains(' '))
                {
                    int spaceCounter = 0;
                    List<int> spaceCombo = SpaceCombo(s, distance);

                    List<String> keys; List<int> values;
                    Rank(s, out keys, out values);

                    do
                    {
                        int index = 0;
                        index = values.IndexOf(values.Max());

                        str.Replace(keys[index], strHelper(keys[index], spaceCombo[spaceCounter]));

                        values[index] = 0;
                        spaceCounter++;
                    } while (spaceCounter < spaceCombo.Count);

                    result.Add(str.ToString());
                }
                else
                {
                    result.Add(str.ToString());
                }
            }
            return result;
        }
        #endregion

        public override void Final()
        {
            AdjustToWork = GoGo(wrap, ToWork);
            ToPrint = GoGoAdjust(AdjustToWork);
            File();
        }
    }

    public class LineMoment : Fill
    {
        #region Child-Specific Variable + Constructor
        private readonly int moment = 0;
        public LineMoment(Format format, int wrap, int moment, List<String> words)
        {
            this.format = format;
            this.wrap = wrap;
            this.moment = moment;
            this.ToWork = words;
        }
        #endregion

        #region Methods
        private List<string> AllSpaces(string s)
        {
            List<String> l = new List<string>();
            l.Add(s);
            int index = s.IndexOf(' ');
            while (s.Length + 1 <= wrap)
            {
                s = s.Insert(index, " ");
                l.Add(s);
            }
            return l;
        }
        private int vOfL(char c)
        {
            int i = (int)c;
            i -= 96;
            return i;
        }
        private int LineScore(string s)
        {
            List<int> wordScores = new List<int>();
            int score = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ' ')
                {
                    wordScores.Add(score);
                    score = 0;
                }
                else if (i == s.Length - 1)
                {
                    score += vOfL(s[i]) * System.Math.Abs(moment - i);
                    wordScores.Add(score);
                }
                else
                {
                    score += vOfL(s[i]) * System.Math.Abs(moment - i);
                }
            }
            score = 0;
            foreach (int i in wordScores)
            {
                score += i;
            }
            return score;
        }
        private void GoGoScore()
        {
            foreach (string s in ToWork)
            {
                if (s.Contains(' '))
                {
                    List<String> w = AllSpaces(s);
                    List<int> i = new List<int>();
                    foreach (string s2 in w)
                    {
                        i.Add(LineScore(s2));
                    }
                    int index = i.IndexOf(i.Min());
                    string s3 = w[index];
                    ToPrint.Add(s3);
                }
                else
                {
                    ToPrint.Add(s);
                }
            }
        }
        #endregion

        public override void Final()
        {
            ToWork = GoGo(wrap, ToWork);
            ToPrint = new List<string>();
            GoGoScore();
            File();
        }
    }

    public class FillSet : Fill
    {
        #region Child-Specific Variable + Constructor
        private readonly int newWrap;
        public FillSet(Format format, int wrap, List<String> words)
        {
            this.format = format;
            this.wrap = wrap;
            this.newWrap = wrap + 1;
            this.ToWork = words;
        }
        #endregion

        #region Methods
        private List<String> AddSpace(List<String> list)
        {
            List<String> result = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                result.Add(" " + list[i]);
            }
            return result;
        }
        private List<String> RemovesSpace(List<String> list)
        {
            List<String> result = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                result.Add(list[i].Substring(1));
            }
            return result;
        }
        private IEnumerable<string> SortByLength(IEnumerable<string> e)
        {
            var sorted = from s in e
                         orderby s.Length descending
                         select s;
            return sorted;
        }
        private List<String> SortList(List<String> list)
        {
            List<String> result = new List<string>();
            foreach (string s in SortByLength(list))
            {
                result.Add(s);
            }
            return result;
        }
        private List<String> Binpack(List<string> list)
        {
            List<String> bins = new List<string> { "" };
            List<String> temp = SortList(list);
            foreach (string s in temp)
            {
                int binsIndex = 0;
                foreach (string s2 in bins)
                {
                    if (!IsOverflown(bins[binsIndex].Length, s.Length, newWrap))
                    {
                        bins[binsIndex] = bins[binsIndex] + s;
                        break;
                    }
                    else if (binsIndex == bins.Count - 1)
                    {
                        bins.Add(s);
                        break;
                    }
                    else { }
                    binsIndex++;
                }
            }
            return bins;
        }
        #endregion

        public override void Final()
        {
            List<String> spacedToWork = AddSpace(ToWork);
            List<String> spacedToPrint = Binpack(spacedToWork);
            ToPrint = RemovesSpace(spacedToPrint);
            File();
        }
    }
}
