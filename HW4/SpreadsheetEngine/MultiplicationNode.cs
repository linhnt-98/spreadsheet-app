using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Operator Node for multiplication.
    /// </summary>
    internal class MultiplicationNode : OperatorNode
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplicationNode"/> class.
        /// Constructs nothing.
        /// </summary>
        public MultiplicationNode()
        {
        }

        /// <summary>
        /// Gets mulitplication operator.
        /// </summary>
        public static new char Operator => '*';

        /// <summary>
        /// Gets precedence.
        /// </summary>
        public static ushort Precedence => 3;

        /// <summary>
        /// Gets associativity.
        /// </summary>
        public static AssocEnum Associativity => AssocEnum.Left;

        /// <summary>
        /// Evaluate left and right children to multiply.
        /// </summary>
        /// <returns>Double.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() * this.Right.Evaluate();
        }
    }
}
