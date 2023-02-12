using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HW3
{
    public class FibonacciTextReader : TextReader
    {
        Dictionary<string, BigInteger> FibNumbers = new Dictionary<string, BigInteger>();

        private int maxLine;
        private int lineCounter = 1;

        public FibonacciTextReader(int _maxLine)
        {
            maxLine = _maxLine;
        }

        public BigInteger CalcFib(int n)
        {
            var cacheFound = FibNumbers.TryGetValue(n.ToString(), out BigInteger valueAtN);
            if (cacheFound)
            {
                return valueAtN;
            }


            if (n == 1)
            {
                return 0;
            }
            else if (n == 2)
            {
                return 1;
            }
            else if (n >= 3)
            {
                BigInteger n_2 = CalcFib(n - 2);
                SaveToCache((n - 2).ToString(), n_2);

                BigInteger n_1 = CalcFib(n - 1);
                SaveToCache((n - 1).ToString(), n_1);

                BigInteger result = n_2 + n_1;
                SaveToCache(n.ToString(), result);

                return result;
            }
            else
            {
                return -1;
            }
        }

        // Attempt to push data to 

        public override string ReadLine()
        {
            var lineValue = CalcFib(lineCounter);
            lineCounter++;
            return lineValue.ToString();
        }

        public override string ReadToEnd()
        {
            // some logic to calculate all fib numbers until maxLine (nth)
            // e.g. if maxLine is 3
            //string[] array = { "0", "1", "1" };
            List<string> array = new List<string>();

            while (lineCounter <= maxLine)
            {
                var lineValue = ReadLine();
                array.Add(lineValue);
            }

            lineCounter = 1;

            return String.Join(Environment.NewLine, array);
        }

        private void SaveToCache(string key, BigInteger value)
        {
            try
            {
                // "Attempt" to push to dictionary
                FibNumbers.Add(key, value);
            }
            catch
            {
                // "key" already exists - ignore
            }
        }

    }
}