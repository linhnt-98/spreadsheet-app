using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace HW3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.loadFromFilesToolStripMenuItem.Click += new System.EventHandler(this.loadFromFilesToolStripMenuItem_Click);
            this.loadFibonacciNumbersfirst50ToolStripMenuItem.Click += new System.EventHandler(this.loadFibonacciNumbersfirst50ToolStripMenuItem_Click);
            this.loadFibonacciNumbersfirst100ToolStripMenuItem.Click += new System.EventHandler(this.loadFibonacciNumbersfirst100ToolStripMenuItem_Click);
            this.saveToFileToolStripMenuItem.Click += new System.EventHandler(this.saveToFileToolStripMenuItem_Click);

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoadText(TextReader sr)
        {
            this.textBox1.Text = sr.ReadToEnd();
        }

        private void loadFromFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // This opens the dialog box.
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            // This line opens the selected file.
            var file = openFileDialog.OpenFile();

            // This loads text in the text box.
            StreamReader reader = new StreamReader(file);
            this.LoadText(reader);
        }

        /// <summary>
        /// Prints the fibonacci up to the 50th sequence.
        /// </summary>
        private void loadFibonacciNumbersfirst50ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FibonacciTextReader fibTextReader = new FibonacciTextReader(50);
            this.LoadText(fibTextReader);
        }

        /// <summary>
        /// Prints the fibonacci up to the 100th sequence.
        /// </summary>
        private void loadFibonacciNumbersfirst100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FibonacciTextReader fibTextReader = new FibonacciTextReader(100);
            this.LoadText(fibTextReader);
        }

        /// <summary>
        /// Saves text from form into a file.
        /// </summary>
        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream stream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog(); // Opens a dialog box for the user to choose where to save the file.

            if (saveFileDialog1.ShowDialog() == DialogResult.OK) // This is true when the user clicks save. Does nothing if cancels.
            {
                stream = saveFileDialog1.OpenFile();
                if (stream != null)
                {
                    string fileContents = this.textBox1.Text; // Stores text from the textbox into a string

                    StreamWriter streamWriter = new StreamWriter(stream);
                    streamWriter.Write(fileContents); // Writes that string and store it into a file, and finalize.
                    streamWriter.Flush();
                    stream.Close();
                }
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }

}