using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Operator node that does subtraction.
    /// </summary>
    internal class SubtractionNode : OperatorNode
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SubtractionNode"/> class.
        /// constructs nothing.
        /// </summary>
        public SubtractionNode()
        {
        }

        /// <summary>
        /// Gets subtraction operator.
        /// </summary>
        public static char Operator => '-';

        /// <summary>
        /// Gets precedance.
        /// </summary>
        public static ushort Precedence => 4;

        /// <summary>
        /// Gets Associativity.
        /// </summary>
        public static AssocEnum Associativity => AssocEnum.Left;

        /// <summary>
        /// Evaluates left and right children to subtract.
        /// </summary>
        /// <returns>Double.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() - this.Right.Evaluate();
        }
    }
}
