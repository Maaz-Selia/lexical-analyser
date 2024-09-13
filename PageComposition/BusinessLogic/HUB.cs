using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public enum Format { Fill, FillSoft, FillAdjust, LineMoment, FillSet };
    public class HUB
    {
        #region Field Variables
        public Format format;
        public int wrap = 0;
        public int wrapSoft = 0;
        public int columnMoment = 0;
        public List<String> words;
        public List<String> definiteWords = new List<String>();
        #endregion

        #region Constructors
        public HUB()
        {
            words = new List<String>();
        }

        public HUB(Format format, int wrap, int wrapSoft, int columnMoment, List<String> words)
        {
            this.format = format;
            this.wrap = wrap;
            this.wrapSoft = wrapSoft;
            this.columnMoment = columnMoment;
            this.words = words;
        }
        #endregion

        #region Methods
        public void VerifyWords()
        {
            VowelChecker vc = new VowelChecker();

            foreach (String s in words)
            {
                if (!string.IsNullOrEmpty(s) && s.All(Char.IsLetter) && s.ToLower() == s)
                {
                    if (s.Length <= 3 && vc.ContainsHowManyVowel(s) >= 1 && vc.VowelAreInAlphaOrder(s))
                    {
                        definiteWords.Add(s);
                    }
                    else if (s.Length > 3 && vc.ContainsHowManyVowel(s) >= 2 && vc.VowelAreInAlphaOrder(s))
                    {
                        definiteWords.Add(s);
                    }
                    else { }
                }
            }
        }
        #endregion
    }
}
