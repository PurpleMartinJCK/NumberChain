using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberChain
{
    class Number
    {
        public ulong value;
        public string wordsAnd;
        public int valueAnd;
        public string wordsPlain;
        public int valuePlain;

        public string[] illions;
        public string[] tensTypes;
        public string[] onesTeens;

        public Number(ulong value, string[] illions, string[] tensTypes, string[] onesTeens)
        {
            this.value = value;
            this.illions = illions;
            this.tensTypes = tensTypes;
            this.onesTeens = onesTeens;
            wordsAnd = FindWords(this.value, true);
            wordsPlain = FindWords(this.value, false);
        }

        public Number(ulong value, Number givenNum, string[] illions, string[] tensTypes, string[] onesTeens)
        {
            this.value = value;
            this.illions = illions;
            this.tensTypes = tensTypes;
            this.onesTeens = onesTeens;
            this.wordsAnd = FindWords(this.value, true, givenNum);
            this.wordsPlain = FindWords(this.value, false, givenNum);
        }

        public void LoadValueTables()
        {
            illions = LoadCSV("..\\..\\Illions.csv");
            tensTypes = LoadCSV("..\\..\\TensTypes.csv");
            onesTeens = LoadCSV("..\\..\\OnesTeens.csv");
        }
        /// <summary>
        /// This loads a comma seperated value file into the program.
        /// </summary>
        /// <param name="filePath">The path of the file to be loaded.</param>
        /// <returns>Returns in a string array.</returns>
        public string[] LoadCSV(string filePath)
        {
            StreamReader sr = new StreamReader(filePath);
            var lines = new List<string>();
            int Row = 0;
            while (!sr.EndOfStream)
            {
                string Line = sr.ReadLine();
                lines.Add(Line);
                Row++;
            }

            return (lines.ToArray());

        }

        /// <summary>
        /// This splits up and builds the word for a number from scratch. Hopefully I'll have a (probably dynamic programming)
        /// algorithm to do it with the other constructor but hey, this is good backup.
        /// </summary>
        /// <param name="tempVal">The value to calculate from.</param>
        /// <param name="withAnd">Whether or not this iteration uses the word and near the end.</param>
        /// <returns>This returns the string of the final word of the number. Gonna split this off from the one that counts the letters.</returns>
        public string FindWords(ulong tempVal, bool withAnd)
        {
            string concatString = "";

            // The largest of the 'illions' in value.
            ushort illionsMag = (ushort)(Math.Floor(Math.Log10(tempVal)) / 3);
            // The number of digits in value.
            ushort digitNum = (ushort)Math.Floor(Math.Log10(tempVal) + 1);
            // The number of relevent digits in the largest 'illions' section. So if it's twelve million, this will return two because twelve has
            // two digits. If it was one hundred and seven billion ninety two million and five (in numbers, not words), this will return 3 as there are three sig figs in one hundred and seven.
            ushort illionsPre = (ushort)(((digitNum) % 3));
            if (illionsPre == 0)
                illionsPre = 3;

            double illionsVal;
            // powVal here is the first illions it needs to find. We know what to use as all the illions are at a multiple of three, plus one.
            ushort powVal = (ushort)(digitNum - illionsPre);
            // This is an important loop. This splits up the number by 'illion' and finds
            // the value for each section through recursion.
            while (tempVal > 1000)
            {
                illionsVal = Math.Floor(((double)tempVal / Math.Pow(10, powVal)));
                tempVal -= Convert.ToUInt64(illionsVal * Math.Pow(10, powVal));
                powVal -= 3;

                concatString += IllionsString(illionsMag, (ushort)illionsVal, withAnd) + " ";

                illionsMag -= 1;
            }

            // 123, 456, 789
            //  23, 456, 789
            //   3, 456, 789

            concatString += DredsTensOnesVal((ushort)tempVal, withAnd, true);

            return (concatString);
        }

        /// <summary>
        /// This is cool, it takes a given Number and pulls it to bits, allowing it to save computation time.
        /// </summary>
        /// <param name="tempVal">The value to calculate from.</param>
        /// <param name="withAnd">Whether or not this iteration uses the word and near the end.</param>
        /// <param name="givenNum">The number to pull apart.</param>
        /// <returns>Hopefully returns the value more quickly.</returns>
        public string FindWords(ulong tempVal, bool withAnd, Number givenNum)
        {
            string concatString = "";

            // The largest of the 'illions' in value.
            ushort illionsMag = (ushort)(Math.Floor(Math.Log10(tempVal)) / 3);
            // The number of digits in value.
            ushort digitNum = (ushort)Math.Floor(Math.Log10(tempVal) + 1);
            // The number of relevent digits in the largest 'illions' section. So if it's twelve million, this will return two because twelve has
            // two digits. If it was one hundred and seven billion ninety two million and five (in numbers, not words), this will return 3 as there are three sig figs in one hundred and seven.
            ushort illionsPre = (ushort)(((digitNum) % 3));
            if (illionsPre == 0)
                illionsPre = 3;

            double illionsVal;
            // powVal here is the first illions it needs to find. We know what to use as all the illions are at a multiple of three, plus one.
            ushort powVal = (ushort)(digitNum - illionsPre);


            illionsVal = Math.Floor(((double)tempVal / Math.Pow(10, powVal)));
            tempVal -= Convert.ToUInt64(illionsVal * Math.Pow(10, powVal));
            powVal -= 3;

            concatString += IllionsString(illionsMag, (ushort)illionsVal, withAnd) + " ";

            illionsMag -= 1;


            // 123, 456, 789
            //  23, 456, 789
            //   3, 456, 789
            if (withAnd)
                concatString += " " + givenNum.wordsAnd;
            else
                concatString += " " + givenNum.wordsPlain;
            return (concatString);
        }

        /// <summary>
        /// This finds values lower than one thousand in words.
        /// </summary>
        /// <param name="tempVal">The value to be figured out.</param>
        /// <param name="withAnd">Whether or not we're using and.</param>
        /// <param name="lastAnd">If this is the last run from a larger section.</param>
        /// <returns>The string of the words.</returns>
        public string DredsTensOnesVal(ushort tempVal, bool withAnd, bool lastAnd)
        {
            string concatString = "";
            // This makes dredVal equal to the value of the hundreds column.
            ushort dredVal = (ushort)Math.Floor((double)tempVal / 100);

            // Now we take the hundreds from tempVal to make it just have our tens and ones.
            tempVal -= (ushort)(dredVal * 100);

            if (dredVal > 0)
            {
                concatString += onesTeens[dredVal] + " " + "Hundred ";
                if (withAnd && (tempVal > 0))
                {
                    concatString += "and ";
                    lastAnd = false;
                }
            }

            if (lastAnd && withAnd && (tempVal > 0) && value > 999)
                concatString += "and ";

            concatString += TensOnesVal(tempVal);

            return (concatString);
        }

        /// <summary>
        /// Finds the tens and/or ones, even if it's a teen value which we
        /// all know is special in engligh. Just look at fifteen and its
        /// lack of a 'v' the bugger.
        /// </summary>
        /// <param name="tempVal">The value to figure out.</param>
        /// <returns></returns>
        public string TensOnesVal(ulong tempVal)
        {
            string concatString = "";

            if (tempVal < 20)
                concatString = onesTeens[tempVal];
            else
            {
                concatString += tensTypes[(int)Math.Floor((double)tempVal / 10) - 2];
                if ((tempVal % 10) != 0)
                    concatString += " " + onesTeens[tempVal % 10];
            }

            return (concatString);
        }

        /// <summary>
        /// This uses the magnitude of the 'illions' (which tells if it's million, trillion, etc.) and the
        /// value at that magnitude to get the string.
        /// </summary>
        /// <param name="illionsMag">The magnitude of the 'illion'.</param>
        /// <param name="illionsVal">The values at that level (it'll be less than 1000 always).</param>
        /// <param name="withAnd">Whether it has and.</param>
        /// <returns>The full string.</returns>
        public string IllionsString(ushort illionsMag, ushort illionsVal, bool withAnd)
        {
            string concatString = "";

            if (illionsVal > 0)
            {
                concatString += DredsTensOnesVal(illionsVal, withAnd, false) + " " + illions[illionsMag];
            }

            return concatString;
        }
    }
}
