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
        /// initializes a instance of the UndoRedoClass for storing/getting undo or redo commands.
        /// </summary>
        private UndoRedoClass undoRedo = new UndoRedoClass();

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

            Cell currentcell = sender as Cell;

            if (e.PropertyName == "Value" && sender is Cell cell)
            {
                this.dataGridView1.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value = cell.Value;
            }

            if (e.PropertyName == "BGColor")
            {
                this.dataGridView1.Rows[currentcell.RowIndex].Cells[currentcell.ColumnIndex].Style.BackColor = Color.FromArgb((int)currentcell.BGColor);
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

            List<IUndoRedoCommand> undoRedoList = new List<IUndoRedoCommand>();

            undoRedoList.Add(new UndoRedoText(editingCell.Text, editingCell.CellTag));

            if (this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != null)
            {
                editingCell.Text = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            else
            {
                editingCell.Text = string.Empty;
            }

            DoCommand tempCmd = new DoCommand(undoRedoList, "change cell text");

            this.undoRedo.AddUndo(tempCmd);

            this.RefreshButtons();

            this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = editingCell.Value;
        }

        private void ChangeColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedColor = 0;

            ColorDialog colorDialog = new ColorDialog();

            List<IUndoRedoCommand> undoRedoList = new List<IUndoRedoCommand>();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                selectedColor = colorDialog.Color.ToArgb();

                foreach (DataGridViewCell cell in this.dataGridView1.SelectedCells)
                {
                    Cell spreadsheetCell = this.spreadSheet.GetCell(cell.RowIndex, cell.ColumnIndex);

                    UndoRedoColor undoRedoC = new UndoRedoColor((int)spreadsheetCell.BGColor, spreadsheetCell.CellTag);

                    undoRedoList.Add(undoRedoC);

                    spreadsheetCell.BGColor = (uint)selectedColor;
                }

                DoCommand tempCmd = new DoCommand(undoRedoList, "change bg color");

                this.undoRedo.AddUndo(tempCmd);

                this.RefreshButtons();
            }
        }

        /// <summary>
        /// Refreshes the first toolStrip Column which has the Undo or Redo buttons.
        /// </summary>
        private void RefreshButtons()
        {
            ToolStripItemCollection toolStripItems = this.menuStrip1.Items;

            // Gets the Edit Column of the toolstrip, [1] has the Cell column.
            ToolStripMenuItem toolStripEditColumn = toolStripItems[0] as ToolStripMenuItem;

            toolStripEditColumn.DropDownItems[0].Enabled = this.undoRedo.BoolUndoable;
            toolStripEditColumn.DropDownItems[0].Text = "Undo " + this.undoRedo.UndoStackNextCommand;

            toolStripEditColumn.DropDownItems[1].Enabled = this.undoRedo.BoolRedoable;
            toolStripEditColumn.DropDownItems[1].Text = "Redo " + this.undoRedo.RedoStackNextCommand;
        }

        /// <summary>
        /// Upon clicking the Undo button, calls the Undo command from the undoRedo class, and refreshes the buttons.
        /// </summary>
        /// <param name="sender">control for loading the form.</param>
        /// <param name="e">event data.</param>
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.undoRedo.Undo(this.spreadSheet);
            this.RefreshButtons();
        }

        /// <summary>
        /// Upon clicking the Undo button, calls the Redo command from the undoRedo class, and refreshes the buttons.
        /// </summary>
        /// <param name="sender">control for loading the form.</param>
        /// <param name="e">event data.</param>
        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.undoRedo.Redo(this.spreadSheet);
            this.RefreshButtons();
        }

        /// <summary>
        /// Opens a new SaveFileDialog, sets the settings, and then saves the xml.
        /// </summary>
        /// <param name="sender">control for loading the form.</param>
        /// <param name="e">event data.</param>
        
        
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML files (*.xml)|*.xml";
            saveFileDialog.Title = "Save SpreadSheet";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream ofStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);
                this.spreadSheet.Save(ofStream);
                ofStream.Dispose();
            }
        }
    }
}
