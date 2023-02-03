using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HW2
{
    /// <summary>
    /// This method will compare 3 different approaches that 
    /// if they return the same result of finding unique integers 
    /// in 10000 random numbers from 0 to 20000
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            RunDistinctIntegers();
        }

        private static string RunDistinctIntegers() // this is your method 
        {
            string text = "";
            //Creat list.
            List<int> listIntegers = new List<int>();
            var rand = new Random();
            for (int i = 1; i <= 10000; i++) // 10000 integers
            {
                listIntegers.Add(rand.Next(0, 20001)); //getting random number from 0 to 20000
            }


            //Hashset
            HashSet<int> hashIntegers = new HashSet<int>();
            for (int j = 0; j < listIntegers.Count; j++)
            {
                hashIntegers.Add(listIntegers[j]);
            }

            //O(1) storage
            int count = 1;
            bool IsDuplicate = false;
            for (int i = 1; i < listIntegers.Count; i++)
            {
                IsDuplicate = false;
                for (int j = 0; j < i; j++)
                {
                    if (listIntegers[i] == listIntegers[j])
                    {
                        IsDuplicate = true;
                        break;
                    }
                }
                if (!IsDuplicate)
                {
                    count++;
                }
            }

            //Sort
            listIntegers.Sort();
            int sortedCount = 1;
            for (int i = 1; i < listIntegers.Count; i++)
            {
                if (listIntegers[i] != listIntegers[i - 1])
                {
                    sortedCount++;
                }
            }

            // 3 different approaches' result
            text = "1. HashSet method: " + hashIntegers.Count.ToString() + " unique numbers.";
            text += Environment.NewLine;
            text += "According to learn.microsoft.com, retrieving the value of this property is an O(1) operation.";
            text += Environment.NewLine;
            text += "2. O(1) storage method: " + count + " unique numbers.";
            text += Environment.NewLine;
            text += "3. Sorted method: " + sortedCount + " unique numbers.";


            return text; //print result
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = RunDistinctIntegers();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
