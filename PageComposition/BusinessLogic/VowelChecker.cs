using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class VowelChecker
    {
        private char[] vowels = new[] { 'a', 'e', 'i', 'o', 'u'};

        #region Methods
        public int ContainsHowManyVowel(String word)
        {
            int vowelcount = 0;
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == 'a' || word[i] == 'e' || word[i] == 'i' || word[i] == 'o' || word[i] == 'u')
                {
                    vowelcount++;
                }
            }
            return vowelcount;
        }
        private string SortString(String input)
        {
            char[] characters = input.ToArray();
            Array.Sort(characters);
            return new string(characters);
        }
        public bool VowelAreInAlphaOrder(String word)
        {
            var vowels = new List<char> { 'a', 'e', 'i', 'o', 'u' };
            string vowelList = "";
            for(int i = 0; i < word.Length; i++)
            {
                if (word[i] == 'a' || word[i] == 'e' || word[i] == 'i' || word[i] == 'o' || word[i] == 'u')
                {
                    vowelList += word[i];
                }
            }
            var expectedResult = SortString(vowelList);
            bool isOrdered = vowelList.SequenceEqual(expectedResult);
            return isOrdered;
        }
        #endregion
    }
}
