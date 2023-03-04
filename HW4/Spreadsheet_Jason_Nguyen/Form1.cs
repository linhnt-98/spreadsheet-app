﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpreadsheetEngine;

namespace Spreadsheet_Jason_Nguyen
{
    public partial class Form1 : Form
    {

        private Spreadsheet spreadSheet = new Spreadsheet(50, 26);

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

            //Clear grid
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            this.spreadSheet.CellPropertyChanged += this.CellPropertyChanged;


            //Add Columns
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

        /// <summary>
        /// Updates the cell value when the propertly is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value" && sender is Cell cell)
            {
                this.dataGridView1.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value = cell.Value;
            }
        }

        /// <summary>
        /// Creates a Demo of the cell engine, when button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        private void button1_Click_1(object sender, EventArgs e)
        {
            Random rand = new Random();
            for (int i = 0; i < 50; i++)
            {
                int rowIndex = rand.Next(0, 49);
                int columnIndex = rand.Next(2, 25);

                this.spreadSheet.SetCellText(rowIndex, columnIndex, "Hello World");
            }

            for (int i = 0; i < 50; i++)
            {
                this.spreadSheet.SetCellText(i, 1, "this is cell B" + (i + 1).ToString());
            }

            for (int i = 0; i < 50; i++)
            {
                this.spreadSheet.SetCellText(i, 0, "=B" + (i + 1).ToString());
            }
        }
    }
}
