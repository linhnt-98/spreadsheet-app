using SpreadsheetEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Operator node for division.
    /// </summary>
    internal class DivisionNode : OperatorNode
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DivisionNode"/> class.
        /// constructs nothing.
        /// </summary>
        public DivisionNode()
        {
        }

        /// <summary>
        /// Gets division operator.
        /// </summary>
        public static char Operator => '/';

        /// <summary>
        /// Gets precedence.
        /// </summary>
        public static ushort Precedence => 3;

        /// <summary>
        /// Gets associativity.
        /// </summary>
        public static AssocEnum Associativity => AssocEnum.Left;

        /// <summary>
        /// Evaluates left and right children for division.
        /// </summary>
        /// <returns>Double</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() / this.Right.Evaluate();
        }
    }
}
