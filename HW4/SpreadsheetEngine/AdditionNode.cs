using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Operator node that adds.
    /// </summary>
    internal class AdditionNode : OperatorNode
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionNode"/> class.
        /// </summary>
        public AdditionNode()
        {
        }

        /// <summary>
        /// Gets addition operator.
        /// </summary>
        public static new char Operator => '+';

        /// <summary>
        /// Gets precedence.
        /// </summary>
        public static ushort Precedence => 4;

        /// <summary>
        /// Gets associativity.
        /// </summary>
        public static AssocEnum Associativity => AssocEnum.Left;

        /// <summary>
        /// Evaluates left and right children to add.
        /// </summary>
        /// <returns>Double</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() + this.Right.Evaluate();
        }
    }
}
