using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spreadsheet_Jason_Nguyen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeDataGrid();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void InitializeDataGrid()
        {

            //Add Columns
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            for (char c = 'A'; c <= 'Z'; c++)
            {
                dataGridView1.Columns.Add("col" + Char.ToString(c), Char.ToString(c)); 
            }

            
            //Add Rows
            for (int i = 1; i <= 50; i++)
            {
                dataGridView1.Rows.Add(1) ;
                dataGridView1.Rows[i - 1].HeaderCell.Value = i.ToString();
            }

        }
    }
}
