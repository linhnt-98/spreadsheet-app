using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Spreadsheet class, container of 2D arrray of cells, and create all cells in spreadsheet.
    /// </summary>
    public class Spreadsheet
    {
        // Cell location.
        private Cell[,] spreadsheetArray;

        /// <summary>
        /// Dictionary containing a string(key) and a hashset of strings.
        /// </summary>
        private Dictionary<string, HashSet<string>> spreadsheetDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        public Spreadsheet(int rowIndex, int columnIndex)
        {
            this.spreadsheetArray = new Cell[rowIndex, columnIndex];
            this.spreadsheetDictionary = new Dictionary<string, HashSet<string>>();
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
                CreateCell evaluatedCell = sender as CreateCell;
                this.RemoveDependancy(evaluatedCell.CellTag);

                if (evaluatedCell.Text != string.Empty && evaluatedCell.Text[0] == '=')
                {
                    ExpressionTree exp = new ExpressionTree(evaluatedCell.Text.Substring(1));
                    this.MakeDependancy(evaluatedCell.CellTag, exp.GetKeys());
                }

                this.Eval(sender as Cell);
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
        /// gets a cell from its expression.
        /// </summary>
        /// <param name="expression">an expression.</param>
        /// <returns>cell.</returns>
        public Cell GetCell(string expression)
        {
            char columnChar = expression[0];

            Cell evaluatedCell;

            if (char.IsLetter(columnChar) == false)
            {
                return null;
            }

            if (int.TryParse(expression.Substring(1), out int numeral) == false)
            {
                return null;
            }

            try
            {
                evaluatedCell = this.GetCell(numeral - 1, columnChar - 'A');
            }
            catch
            {
                return null;
            }

            return evaluatedCell;
        }

        /// <summary>
        /// Sets text in the location of the cell.
        /// </summary>
        /// <param name="newCell">createcell object.</param>
        /// <param name="cell">a cell.</param>
        public void SetCellText(CreateCell newCell, Cell cell)
        {
            newCell.SetValue(newCell.Text);
            this.CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
        }

        /// <summary>
        /// Calls GetCell and changes the text using its indexes.
        /// </summary>
        /// <param name="rowIndex">row index.</param>
        /// <param name="columnIndex">column index.</param>
        /// <param name="newText">the new text.</param>
        public void SetCellText(int rowIndex, int columnIndex, string newText)
        {
            this.GetCell(rowIndex, columnIndex).Text = newText;
        }

        /// <summary>
        /// Class to create a cell.
        /// </summary>
        public class CreateCell : Cell
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CreateCell"/> class.
            /// Constructs a cell using the given indexes.
            /// </summary>
            /// <param name="rowIndex">row index.</param>
            /// <param name="columnIndex">column index.</param>
            public CreateCell(int rowIndex, int columnIndex)
                : base(rowIndex, columnIndex)
            {
            }

            /// <summary>
            /// sets the value of the cell.
            /// </summary>
            /// <param name="newValue">the new value.</param>
            public void SetValue(string newValue)
            {
                this.value = newValue;
            }
        }

        /// <summary>
        /// Adds or updates a cell to the spreadsheets dictionary/hashset.
        /// </summary>
        /// <param name="cellName">name of the cell.</param>
        /// <param name="variablesUsed">an array containing all used variables.</param>
        private void MakeDependancy(string cellName, string[] variablesUsed)
        {
            for (int i = 0; i < variablesUsed.Length; i++)
            {
                if (this.spreadsheetDictionary.ContainsKey(variablesUsed[i]) == false)
                {
                    this.spreadsheetDictionary[variablesUsed[i]] = new HashSet<string>();
                }

                this.spreadsheetDictionary[variablesUsed[i]].Add(cellName);
            }
        }

        /// <summary>
        /// Sets a cell to be empty.
        /// </summary>
        /// <param name="newCell">createcell object.</param>
        /// <param name="cell">a cell.</param>
        private void SetCellEmpty(CreateCell newCell, Cell cell)
        {
            newCell.SetValue(string.Empty);
            this.CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
        }

        /// <summary>
        /// Evaluates an expression inside a cell.
        /// </summary>
        /// <param name="cell">a cell.</param>
        private void Eval(Cell cell)
        {
            CreateCell newCell = cell as CreateCell;

            if (string.IsNullOrEmpty(newCell.Text))
            {
                this.SetCellEmpty(newCell, cell);
            }
            else if (newCell.Text[0] == '=' && newCell.Text.Length >= 3)
            {
                this.SetExp(newCell, cell);
            }
            else
            {
                this.SetCellText(newCell, cell);
            }

            if (this.spreadsheetDictionary.ContainsKey(newCell.CellTag))
            {
                foreach (var dependentCell in this.spreadsheetDictionary[newCell.CellTag])
                {
                    this.Eval(this.GetCell(dependentCell));
                }
            }
        }

        /// <summary>
        /// Sets an expressions variables to certain values.
        /// </summary>
        /// <param name="newCell">createcell object.</param>
        /// <param name="cell">a cell.</param>
        private void SetExp(CreateCell newCell, Cell cell)
        {
            // make new instance of expressiontree after the = symbol
            ExpressionTree exptree = new ExpressionTree(newCell.Text.Substring(1));

            // create array of variables(from the keys in our expressiontrees dictionary)
            string[] variables = exptree.GetKeys();

            foreach (string variableName in variables)
            {
                Cell variableCell = this.GetCell(variableName);

                if (string.IsNullOrEmpty(variableCell.Value))
                {
                    // if the cell is empty, return NaN when setting something equal. 
                    exptree.SetVariable(variableCell.CellTag, double.NaN);
                }
                else if (!double.TryParse(variableCell.Value, out double value))
                {
                    // The numerical value parsed if double. TryParse on the value string succeeds
                    // 0 otherwise
                    exptree.SetVariable(variableName, 0);
                }
                else
                {
                    // if cell is filled with valid double, set the cell to that double.
                    exptree.SetVariable(variableName, value);
                }
            }

            newCell.SetValue(exptree.Evaluate().ToString());
            this.CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
        }

        /// <summary>
        /// Removes a cell from the spreadsheets dictionary/hashset.
        /// </summary>
        /// <param name="cellName">name of the cell.</param>
        private void RemoveDependancy(string cellName)
        {
            List<string> dependenciesList = new List<string>();

            foreach (string key in this.spreadsheetDictionary.Keys)
            {
                if (this.spreadsheetDictionary[key].Contains(cellName))
                {
                    dependenciesList.Add(key);
                }
            }

            foreach (string key in dependenciesList)
            {
                HashSet<string> hashset = this.spreadsheetDictionary[key];
                if (hashset.Contains(cellName))
                {
                    hashset.Remove(cellName);
                }
            }
        }
    }
}
