using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Class that inherits from our DoUndoRedoCommand interface, used for storing
    /// the old cellText values.
    /// </summary>
    public class UndoRedoText : IUndoRedoCommand
    {
        /// <summary>
        /// string cellText, which contains the text of the cell.
        /// </summary>
        private string cellText;

        /// <summary>
        /// string cellName, which contains the "name" or "tag" of the cell.
        /// </summary>
        private string cellName;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedoText"/> class.
        /// Constructor for the UndoRedoText class, which takes  and stores the inputted name and color of the cell.
        /// </summary>
        /// <param name="oldCellText">(string)Text.</param>
        /// <param name="oldCellName">Name of cell.</param>
        public UndoRedoText(string oldCellText, string oldCellName)
        {
            this.cellText = oldCellText;
            this.cellName = oldCellName;
        }

        /// <summary>
        /// inherited abstract interface method, saves and returns the old text of a cell.
        /// </summary>
        /// <param name="spreadSheet">spreadSheet Instance.</param>
        /// <returns>(UndoRedoColor)the old cell color.</returns>
        public IUndoRedoCommand Exec(Spreadsheet spreadSheet)
        {
            Cell cell = spreadSheet.GetCell(this.cellName);

            string old = cell.Text;

            cell.Text = this.cellText;

            UndoRedoText oldTextClass = new UndoRedoText(old, this.cellName);

            return oldTextClass;
        }
    }
}
