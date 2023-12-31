﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SpreadsheetEngine
{

    /// <summary>
    /// Cell class.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        // Fields.
        private int rowIndex;
        private int columnIndex;

        /// <summary>
        /// To use part of text Property.
        /// </summary>
        private protected string text = string.Empty;

        /// <summary>
        /// To use part of value property.
        /// </summary>
        private protected string value = string.Empty;

        /// <summary>
        /// string value of cell.
        /// </summary>
        private protected string cellTag;

        /// <summary>
        /// the (int) color of a cell. Default -1 which makes it white.
        /// </summary>
        private protected int cellColor = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        public Cell(int rowIndex, int columnIndex)
        {
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.cellTag += Convert.ToChar('A' + columnIndex);
            this.cellTag += (rowIndex + 1).ToString();
        }

        /// <summary>
        /// Property Changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Gets rowIndex.
        /// </summary>
        public int RowIndex
        {
            get { return this.rowIndex; }
        }

        /// <summary>
        /// Gets columnIndex.
        /// </summary>
        public int ColumnIndex
        {
            get { return this.columnIndex; }
        }

        /// <summary>
        /// Gets or sets and sets text .
        /// </summary>
        public string Text
        {
            // Returns the protected field.
            get
            {
                return this.text;
            }

            set
            {
                // If text being set is same as current text, then ignore.
                if (this.text == value)
                {
                    return;
                }

                // Else, set text.
                this.text = value;

                // Invoking the property change event.
                this.PropertyChanged(this, new PropertyChangedEventArgs("Text"));
            }
        }

        /// <summary>
        /// Gets value.
        /// </summary>
        public string Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Gets the cell's tag.
        /// </summary>
        public string CellTag
        {
            get { return this.cellTag; }
        }

        /// <summary>
        /// Gets or sets the color of a cell.
        /// </summary>
        public uint BGColor
        {
            get
            {
                return (uint)this.cellColor;
            }

            set
            {
                if (this.cellColor == value)
                {
                    return;
                }
                else
                {
                    this.cellColor = (int)value;

                    this.PropertyChanged(this, new PropertyChangedEventArgs("BGColor"));
                }
            }
        }
    }
}
