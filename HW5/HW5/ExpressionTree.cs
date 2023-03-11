namespace HW5
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Expression tree class.
    /// </summary>
    public class ExpressionTree
    {
        // The root of tree.
        private Node root;

        private Dictionary<string, double> variables = new Dictionary<string, double>();
        private char[] operators = { '+', '-', '*', '/' };

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// </summary>
        /// <param name="expression">String input.</param>
        public ExpressionTree(string expression)
        {
            this.root = Compile(expression);
        }

        /// <summary>
        /// Use to return evaluated value.
        /// </summary>
        /// <returns>Evaluated double.</returns>
        public double Evaluate()
        {
            return this.Evaluate(this.root);
        }

        /// <summary>
        /// Joins a first name and a last name together into a single string.
        /// </summary>
        /// <param name="name">The name of the set variable.</param>
        /// <param name="value">The value of the set variable.</param>
        public void SetVariable(string name, double value)
        {
            variables[name] = value;
        }

        /// <summary>
        /// Compiles the node.
        /// </summary>
        /// <param name="s">String.</param>
        /// <returns>The node.</returns>
        private Node Compile(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (this.operators.Contains(s[i]))
                {
                    return Compile(s, i);
                }
            }

            if (double.TryParse(s, out double number))
            {
                return new ConstantNode() { Value = number, };
            }
            else
            {
                return new VariableNode() { Name = s, };
            }
        }

        /// <summary>
        /// Splits expression into trees.
        /// </summary>
        /// <param name="expression">Expression in form of string.</param>
        /// <param name="op">The last name to join.</param>
        /// <returns>The operator node.</returns>
        private Node Compile(string expression, int i)
        {
            OperatorNode operatorNode = new OperatorNode(expression[i]);
            operatorNode.Left = Compile(expression.Substring(0, i));
            operatorNode.Right = Compile(expression.Substring(i + 1));
            return operatorNode;
        }

        /// <summary>
        /// Determines what type of node it is.
        /// </summary>
        /// <param name="node">A node.</param>
        /// <returns>The evaluated value.</returns>
        private double Evaluate(Node node)
        {
            if (node is ConstantNode constantNode)
            {
                return constantNode.Value;
            }

            // if its a variable.
            if (node is VariableNode variableNode)
            {
                if (!this.variables.ContainsKey(variableNode.Name))
                {
                    // if the local dictionary doesnt have a key value pair
                    // returns not a number.
                    return double.NaN;
                }

                return this.variables[variableNode.Name];
            }

            // or an operator.
            if (node is OperatorNode operatorNode)
            {
                switch (operatorNode.Operator)
                {
                    case '+':
                        return this.Evaluate(operatorNode.Left) + this.Evaluate(operatorNode.Right);
                    case '-':
                        return this.Evaluate(operatorNode.Left) - this.Evaluate(operatorNode.Right);
                    case '*':
                        return this.Evaluate(operatorNode.Left) * this.Evaluate(operatorNode.Right);
                    case '/':
                        return this.Evaluate(operatorNode.Left) / this.Evaluate(operatorNode.Right);
                    default: // Throw execption if nothing else
                        throw new NotSupportedException(
                            "Operator " + operatorNode.Operator.ToString() + " not supported."
                        );
                }
            }

            throw new NotSupportedException();
        }
    }
}
