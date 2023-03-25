namespace HW5
{
    /// <summary>
    /// Node for operators.
    /// </summary>
    internal class OperatorNode : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNode"/> class.
        /// </summary>
        /// <param name="c">Some char.</param>
        public OperatorNode(char c)
        {
            this.Operator = c;
            this.Left = new Node();
            this.Right = new Node();
        }

        /// <summary>
        /// Gets or sets the operator symbol.
        /// </summary>
        public char Operator { get; set; }

        /// <summary>
        /// Gets or sets the left child node.
        /// </summary>
        public Node Left { get; set; }

        /// <summary>
        /// Gets or sets the right child node.
        /// </summary>
        public Node Right { get; set; }
    }
}
