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





            // 3 different approaches' result
            text = "1. HashSet method: " + hashIntegers.Count.ToString() + " unique numbers.";
            text += Environment.NewLine;





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
