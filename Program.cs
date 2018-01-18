using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Use alt + F2 for the analyser



namespace NumberChain
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch timerWatch = new Stopwatch();
            timerWatch.Start();

            string[] illions = LoadCSV("..\\..\\Illions.csv");
            string[] tensTypes = LoadCSV("..\\..\\TensTypes.csv");
            string[] onesTeens = LoadCSV("..\\..\\OnesTeens.csv");

            //ulong testNum = 412123456789229;
            //testNum = 123456789;
            //long time1 = timerWatch.ElapsedMilliseconds;
            //ulong testNumFirst = IllionsRounder(testNum);
            //long time2 = timerWatch.ElapsedMilliseconds;
            //Console.WriteLine(DateTime.Now);
            //long time3 = timerWatch.ElapsedMilliseconds;
            //Number firstNum = new Number(testNumFirst, );
            //long time4 = timerWatch.ElapsedMilliseconds;
            //IllionsRounder(testNum);
            //Number secondNum = new Number(testNum, firstNum);
            //long time5 = timerWatch.ElapsedMilliseconds;

            //int testNumDigits = (int)Math.Floor(Math.Log10(testNum));
            //Console.WriteLine(testNumDigits);

            //Number numTwo = new Number(testNum);
            //Number numThree = new Number(23456789);
            //Number numFour = new Number(3456789);
            //int testDiv = testNumDigits / 3;

            //// 42 , 123 , 456 , 789 , 229
            //// 412 , 123 , 456 , 789 , 229
            //Console.WriteLine(testNum + ": " + IllionsRounder(testNum));
            //Console.WriteLine("42123456789229: " + IllionsRounder(42123456789229));
            //ulong[] testArray = new ulong[5000];
            int timesRun = 10000000;

            Console.WriteLine("test" + new Number(0, illions, tensTypes, onesTeens).wordsPlain);

            long time6 = timerWatch.ElapsedMilliseconds;
            //      TestMethodOne(timesRun, illions, tensTypes, onesTeens);
            long time7 = timerWatch.ElapsedMilliseconds;
            Console.WriteLine("First set done.");
            //       System.Threading.Thread.Sleep(2000);
            long time8 = timerWatch.ElapsedMilliseconds;
            //  TestMethodTwo(timesRun, illions, tensTypes, onesTeens);
            long time9 = timerWatch.ElapsedMilliseconds;
            //Console.WriteLine("Dreds Test: " + numTwo.DredsTensOnesVal(216, true, false));
            //Console.WriteLine("Numtwo Test: " + numTwo.wordsAnd);
            //Console.WriteLine("Numthree Test: " + numThree.wordsAnd);
            //Console.WriteLine("Numfour Test: " + numFour.wordsAnd);

            long time10 = timerWatch.ElapsedMilliseconds;
            TestMethodThree(2, illions, tensTypes, onesTeens);
            long time11 = timerWatch.ElapsedMilliseconds;


            //Console.WriteLine(testDiv);
            Console.WriteLine("TimeTest1: " + (time7 - time6));
            Console.WriteLine("TimeTest2: " + (time9 - time8));
            Console.WriteLine("TimeTest3: " + (time11 - time10));
            Console.ReadLine();
        }

        static ulong IllionsRounder(ulong roundVal)
        {

            // The number of digits in value.
            ushort digitNum = (ushort)Math.Floor(Math.Log10(roundVal) + 1);
            // The number of relevent digits in the largest 'illions' section. So if it's twelve million, this will return two because twelve has
            // two digits. If it was one hundred and seven billion ninety two million and five (in numbers, not words), this will return 3 as there are three sig figs in one hundred and seven.
            ushort illionsPre = (ushort)(((digitNum) % 3));
            if (illionsPre == 0)
                illionsPre = 3;
            ushort powVal = (ushort)(digitNum - illionsPre);
            return (ulong)(roundVal % Math.Pow(10, powVal));
        }

        static void TestMethodOne(int timesRun, string[] illions, string[] tensTypes, string[] onesTeens)
        {
            Number[] testNums = new Number[timesRun];
            for (int i = 0; i < testNums.Length; i++)
            {
                testNums[i] = new Number((ulong)(i + 1000000), illions, tensTypes, onesTeens);
            }
        }
        static void TestMethodTwo(int timesRun, string[] illions, string[] tensTypes, string[] onesTeens)
        {
            Number[] testNums = new Number[timesRun];
            for (int i = 0; i < testNums.Length; i++)
            {
                if (i >= 1000)
                    testNums[i] = new Number((ulong)(i + 1000000), testNums[IllionsRounder((ulong)(i))], illions, tensTypes, onesTeens);
                else
                    testNums[i] = new Number((ulong)(i + 1000000), illions, tensTypes, onesTeens);
            }
        }

        static void TestMethodThree(int illionsMag, string[] illions, string[] tensTypes, string[] onesTeens)
        {
            Number[][] testNums = new Number[illionsMag + 1][];
            testNums[0] = new Number[1000];

            for (int j = 0; j < testNums[0].Length; j++)
            {
                testNums[0][j] = new Number((ulong)(j), illions, tensTypes, onesTeens);
            }

            for (int i = 1; i <= illionsMag; i++)
            {
                ulong count = 0;
                ulong illionMult = (ulong)Math.Pow(10, i * 3);
                if (i == 2)
                    testNums[i] = new Number[illionMult * 90];
                else
                testNums[i] = new Number[illionMult * 900];
                for (int j = 0; j < testNums[i].Length && count < (ulong)testNums[i].Length; j++)
                {
                    for (int k = 0; k < i; k++)
                    {
                        for (int p = 0; p < testNums[k].Length; p++)
                        {
                            if (count == 1000)
                                Console.WriteLine("Hey");
                            testNums[i][count] = new Number(testNums[k][p].value + illionMult * ((ulong)j + 1), testNums[k][p], illions, tensTypes, onesTeens);
                            count++;
                        }
                    }
                }
            }
        }

        static string[] LoadCSV(string filePath)
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
    }
}
