namespace SpreadsheetEngine
{
    /// <summary>
    /// Node class.
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        public Node()
        {
        }

        /// <summary>
        /// abstract method to be overloaded.
        /// </summary>
        /// <returns>Double.</returns>
        public abstract double Evaluate();
    }
}
