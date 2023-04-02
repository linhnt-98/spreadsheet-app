using System;
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
    /// <summary>
    /// Main Form.
    /// </summary>
    public partial class Form1 : Form
    {

        /// <summary>
        /// Initialize a spreedsheet instance of size 50*26.
        /// </summary>
        private Spreadsheet spreadSheet = new Spreadsheet(50, 26);

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        /// <summary>
        /// Initialize data grid.
        /// </summary>
        private void InitializeDataGrid(object sender, EventArgs e)
        {

            // Clear grid.
            this.dataGridView1.Rows.Clear();
            this.dataGridView1.Columns.Clear();

            this.spreadSheet.CellPropertyChanged += this.CellPropertyChanged;

            // Creating the columns of letters.
            string[] alphabetList =
            {
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
            };

            this.dataGridView1.RowHeadersWidth = 50;
            this.dataGridView1.ColumnCount = 26;
            this.dataGridView1.RowCount = 50;

            for (int i = 0; i < 26; i++)
            {
                this.dataGridView1.Columns[i].Name = alphabetList[i];
            }

            for (int i = 1; i < 51; i++)
            {
                this.dataGridView1.Rows[i - 1].HeaderCell.Value = i.ToString();
            }
        }

        /// <summary>
        /// Updates the cell value when the propertly is changed.
        /// </summary>
        /// <param name="sender">control for loading the form.</param>
        /// <param name="e">event data.</param>
        private void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value" && sender is Cell cell)
            {
                this.dataGridView1.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value = cell.Value;
            }
        }

        /// <summary>
        /// When editing cell, show text first instead of the value.
        /// </summary>
        /// <param name="sender">For loading form.</param>
        /// <param name="e">The event data.</param>
        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            Cell editingCell = this.spreadSheet.GetCell(e.RowIndex, e.ColumnIndex);

            this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = editingCell.Text;
        }

        /// <summary>
        /// When done editing cell, show the value instead of text.
        /// </summary>
        /// <param name="sender">For loading form.</param>
        /// <param name="e">The event data.</param>
        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Cell editingCell = this.spreadSheet.GetCell(e.RowIndex, e.ColumnIndex);

            var cell = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (cell.Value != null)
            {
                editingCell.Text = cell.Value.ToString();
            }
            else
            {
                editingCell.Text = string.Empty;
            }

            this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = editingCell.Value;
        }

    }
}
