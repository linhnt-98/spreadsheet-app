namespace SpreadsheetEngine
{
    /// <summary>
    /// Node for constant values.
    /// </summary>
    internal class ConstantNode : Node
    {
        /// <summary>
        /// Privated double value.
        /// </summary>
        private readonly double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantNode"/> class.
        /// </summary>
        /// <param name="value">A constant.</param>
        public ConstantNode(double value)
        {
            this.value = value;
        }

        /// <summary>
        /// Evaluates just returns the value.
        /// </summary>
        /// <returns>Double.</returns>
        public override double Evaluate()
        {
            return this.value;
        }
    }
}
