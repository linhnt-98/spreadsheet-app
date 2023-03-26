namespace SpreadsheetEngine
{
    /// <summary>
    /// Node for operators.
    /// </summary>
    public abstract class OperatorNode : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNode"/> class.
        /// </summary>
        public OperatorNode()
        {
        }

        /// <summary>
        /// sets associtivity of right or left.
        /// </summary>
        public enum AssocEnum
        {
            /// <summary>
            /// sets associtivity of right.
            /// </summary>
            Right,

            /// <summary>
            /// sets associtivity of left.
            /// </summary>
            Left,
        }

        /// <summary>
        /// Gets or sets the left child node.
        /// </summary>
        public Node Left
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the right child node.
        /// </summary>
        public Node Right
        {
            get;
            set;
        }
    }
}
