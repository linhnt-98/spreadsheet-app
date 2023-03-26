using System.Collections.Generic;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Varible string in node.
    /// </summary>
    internal class VariableNode : Node
    {
        /// <summary>
        /// Privated variable value.
        /// </summary>
        private readonly string varValue;

        /// <summary>
        /// Dictionary with string and double being called.
        /// </summary>
        private readonly Dictionary<string, double> varDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// </summary>
        /// <param name="newVarValue">Variable value same as input variable.</param>
        /// <param name="pairs">The dictionary passed in.</param>
        public VariableNode(string newVarValue, ref Dictionary<string, double> pairs)
        {
            this.varValue = newVarValue;
            this.varDictionary = pairs;
        }

        /// <summary>
        /// Gets or sets name of node.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Evaulate to return a keyed value.
        /// </summary>
        /// <returns>Double.</returns>
        public override double Evaluate()
        {
            double value = 0;
            if (this.varDictionary.ContainsKey(this.varValue))
            {
                {
                    value = this.varDictionary[this.varValue];
                }

                return value;
            }
            else
                return double.NaN;
        }
    }
}
