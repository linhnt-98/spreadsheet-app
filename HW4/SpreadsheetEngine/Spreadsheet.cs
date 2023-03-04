using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    public class Spreadsheet
    {
        // Cell location.
        private Cell[,] spreadsheetArray;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        public Spreadsheet(int rowIndex, int columnIndex)
        {
            this.spreadsheetArray = new Cell[rowIndex, columnIndex];
            for (int i = 0; i < rowIndex; i++)
            {
                for (int j = 0; j < columnIndex; j++)
                {
                    this.spreadsheetArray[i, j] = new CreateCell(i, j);
                    this.spreadsheetArray[i, j].PropertyChanged += this.OnPropertyChanged;
                }
            }
        }

        /// <summary>
        /// CellPropertyChanged Event.
        /// </summary>
        public event PropertyChangedEventHandler CellPropertyChanged;

        /// <summary>
        /// Gets length of columns.
        /// </summary>
        public int ColumnCount
        {
            get { return this.spreadsheetArray.GetLength(1); }
        }

        /// <summary>
        /// Gets length of rows.
        /// </summary>
        public int RowCount
        {
            get { return this.spreadsheetArray.GetLength(0); }
        }

        /// <summary>
        /// Ways to set text in cells.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The PropertyChangedEventArgs e.</param>
        public void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                CreateCell cell = sender as CreateCell;

                // If the cell is empty, then set the original cell as empty as well.
                if ((cell.Text == string.Empty) || (cell.Text == null))
                {
                    cell.SetValue(string.Empty);
                    this.CellPropertyChanged(sender, new PropertyChangedEventArgs("Value"));
                }

                // Else if it starts with '=', and given a valid cell location, then copy from that location.
                else if (cell.Text[0] == '=' && cell.Text.Length >= 3)
                {
                    string columnLetter = cell.Text.Substring(1);
                    string rowNumber = columnLetter.Substring(1);
                    char[] alphabetList =
                    {
                        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                    };
                    int column = 0;
                    int row = int.Parse(rowNumber) - 1;

                    for (; column < 26; column++)
                    {
                        if (alphabetList[column] == columnLetter[0])
                        {
                            break;
                        }
                    }

                    cell.SetValue(this.GetCell(row, column).Text);
                    this.CellPropertyChanged(sender, new PropertyChangedEventArgs("Value"));
                }

                // Else, put whatever text in the current cell.
                else
                {
                    cell.SetValue(cell.Text);
                    this.CellPropertyChanged(sender, new PropertyChangedEventArgs("Value"));
                }
            }
        }

        /// <summary>
        /// Get the location of the cell.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <returns>The Cell.</returns>
        public Cell GetCell(int rowIndex, int columnIndex)
        {
            return this.spreadsheetArray[rowIndex, columnIndex];
        }

        /// <summary>
        /// Sets text in the location of the cell.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="newText">The new text.</param>
        public void SetCellText(int rowIndex, int columnIndex, string newText)
        {
            this.GetCell(rowIndex, columnIndex).Text = newText;
        }

        /// <summary>
        /// Class to create a cell.
        /// </summary>
        private class CreateCell : Cell
        {
            // CreateCell constructor.
            public CreateCell(int rowIndex, int columnIndex)
                : base(rowIndex, columnIndex)
            {
            }

            // Set a value in that cell.
            public void SetValue(string newValue)
            {
                this.value = newValue;
            }
        }
    }
}
