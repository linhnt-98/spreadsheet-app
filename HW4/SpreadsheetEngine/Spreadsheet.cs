using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

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

            if (e.PropertyName == "BGColor")
            {
                this.CellPropertyChanged(sender, new PropertyChangedEventArgs("BGColor"));
            }
        }

        /// <summary>
        /// Saves all modified cells into an XML file.
        /// </summary>
        /// <param name="outFile">output filestream.</param>
        public void Save(Stream outFile)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            XmlWriter writeXml = XmlWriter.Create(outFile, settings);

            writeXml.WriteStartElement("spreadsheet"); // <spreadsheet>

            foreach (Cell currentCell in this.spreadsheetArray)
            {
                // When saving, only write data from cells that have one or more non-default properties. This
                // means that if a cell hasn’t been changed in any way then you don’t need to write data for it to
                // the file.
                if (currentCell.Text != string.Empty || currentCell.Value != string.Empty || currentCell.BGColor != 4294967295)
                {
                    writeXml.WriteStartElement("cell");
                    writeXml.WriteAttributeString("celltag", currentCell.CellTag);
                    writeXml.WriteElementString("bgcolor", currentCell.BGColor.ToString("x8"));
                    writeXml.WriteElementString("text", currentCell.Text.ToString());
                    writeXml.WriteEndElement();

                    // Spreadsheet is saved/loaded into XML as shown.
                    /*
                    <cell celltag="A1">
                        <bgcolor> ffffffff </bgcolor>
                        <text> 22 </text >
                    </cell>
                    */
                }
            }

            writeXml.WriteEndElement(); // </spreadsheet>
            writeXml.Close();
        }

        /// <summary>
        /// Loads found xml file information (bgcolor and text) into the specified tagged cell.
        /// </summary>
        /// <param name="infile">input filestream.</param>
        public void Load(Stream infile)
        {
            XDocument inFile = XDocument.Load(infile);

            // check every start element.
            foreach (XElement label in inFile.Root.Elements("cell"))
            {
                // gets the cell at "celltag".
                Cell currentCell = this.GetCell(label.Attribute("celltag").Value);

                if (label.Element("bgcolor") != null)
                {
                    // sets the current cell's BGcolor to the uint value of the saved xml hexnumber.
                    currentCell.BGColor = uint.Parse(label.Element("bgcolor").Value, System.Globalization.NumberStyles.HexNumber);
                }

                if (label.Element("text") != null)
                {
                    // sets the current cell's text to the text of the saved xml text.
                    currentCell.Text = label.Element("text").Value.ToString();
                }
            }

            // Spreadsheet is saved/loaded into XML as shown.
            /*
            <cell celltag="A1">
                <bgcolor> ffffffff </bgcolor>
                <text> 22 </text >
            </cell>
            */
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
            bool error = false;
            CreateCell newCell = cell as CreateCell;

            if (string.IsNullOrEmpty(newCell.Text))
            {
                this.SetCellEmpty(newCell, cell);
            }
            else if (newCell.Text[0] == '=' && newCell.Text.Length >= 3)
            {
                // pass our error boolean in by reference, so it gets modified in here.
                this.SetExp(newCell, cell, ref error);

                // if error was set to true by the SetExp function, kill this void function.
                if (error == true)
                {
                    return;
                }
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
        private void SetExp(CreateCell newCell, Cell cell, ref bool error)
        {
            // makes new instance of expressiontree, after the =
            ExpressionTree exptree = new ExpressionTree(newCell.Text.Substring(1));

            // create array of variables(from the keys in our expressiontrees dictionary)
            string[] variables = exptree.GetKeys();

            foreach (string variableName in variables)
            {
                if (this.GetCell(variableName) == null)
                {
                    // The cell formula could reference something that doesn’t exist on the spreadsheet. This could
                    // mean it’s a cell name that’s just beyond the range of what our spreadsheet supports, such as
                    // “Z12345”. It could also just be a bad cell name, such as “Ba”. Set the cell value to an error
                    // message as opposed to treating the non-existent cell’s value as 0.
                    error = true;
                    newCell.SetValue("!(bad reference)");
                    this.CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
                    return;
                }

                Cell variableCell = this.GetCell(variableName);

                if (string.IsNullOrEmpty(variableCell.Value))
                {
                    // if the cell is empty, setting something equal will return 0.
                    exptree.SetVariable(variableCell.CellTag, 0);
                }
                else if (!double.TryParse(variableCell.Value, out double value))
                {
                    // Since a cell’s “Value” property is a string, and you will need a double when setting variable
                    // values during formula evaluation, consider the numerical(double) value of a cell to be:
                    // The numerical value parsed if double.TryParse on the value string succeeds
                    // 0 otherwise
                    exptree.SetVariable(variableName, 0);
                }
                else
                {
                    // if cell is filled with valid double (from that out statement ^), set the cell to that double.
                    exptree.SetVariable(variableName, value);
                }

                if (variableName == newCell.CellTag)
                {
                    // if our variablename is the same as the reference celltag, set error values.
                    error = true;
                    newCell.SetValue("!(self reference)");
                    this.CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
                    return;
                }
            }

            foreach (string variableName in variables)
            {
                if (variableName == newCell.CellTag)
                {
                    // if our variablename is the same as the reference celltag, set error values.
                    error = true;
                    newCell.SetValue("!(circular reference)");
                    this.CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
                    return;
                }

                if (!this.spreadsheetDictionary.ContainsKey(newCell.CellTag))
                {
                    continue;
                }

                string currentCell = newCell.CellTag;

               
                // loop through the dependantcells of the current cell.
                foreach (string dependentCell in this.spreadsheetDictionary[currentCell])
                {
                    if (variableName == dependentCell)
                    {
                        // if our variablename is the same as the dependantCell, set error values.
                        error = true;
                        newCell.SetValue("!(circular reference)");
                        this.CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
                        return;
                    }

                    if (this.spreadsheetDictionary.ContainsKey(dependentCell) == false)
                    {
                        continue;
                    }

                    currentCell = dependentCell;
                }
            }

            // if it makes it down here, everything worked correctly.
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
