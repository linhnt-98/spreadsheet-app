using System;
using System.Collections.Generic;
using System.Linq;

namespace HW5
{

    /// <summary>
    /// Expression tree class.
    /// </summary>
    public class ExpressionTree
    {

        // The root of tree.
        private Node root;


        private Dictionary<string, double> variables = new Dictionary<string, double>();

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
            this.variables[name] = value;
        }

        /// <summary>
        /// Compiles the node.
        /// </summary>
        /// <param name="s">String.</param>
        /// <returns>The node.</returns>
        private static Node Compile(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }
            char[] operators = { '+', '-', '*', '/' };
            foreach (char op in operators)
            {
                Node n = Compile(s, op);
                if (n != null)
                {
                    return n;
                }
            }

            if (double.TryParse(s, out double number))
            {
                return new ConstantNode()
                {
                    Value = number,
                };
            }
            else
            {
                return new VariableNode()
                {
                    Name = s,
                };
            }
        }

        /// <summary>
        /// Splits expression into trees.
        /// </summary>
        /// <param name="expression">Expression in form of string.</param>
        /// <param name="op">The last name to join.</param>
        /// <returns>The operator node.</returns>
        private static Node Compile(string expression, char op)
        {
            for (int expressionIndex = expression.Length - 1; expressionIndex >= 0; expressionIndex--)
            {
                if (op == expression[expressionIndex])
                {
                    OperatorNode operatorNode = new OperatorNode(expression[expressionIndex]);
                    operatorNode.Left = Compile(expression.Substring(0, expressionIndex));
                    operatorNode.Right = Compile(expression.Substring(expressionIndex + 1));
                    return operatorNode;
                }
            }

            return null;
        }

        /// <summary>
        /// Determines what type of node it is.
        /// </summary>
        /// <param name="node">A node.</param>
        /// <returns>The evaluated value.</returns>
        private double Evaluate(Node node)
        {
            ConstantNode constantNode = node as ConstantNode;
            if (constantNode != null)
            {
                return constantNode.Value;
            }
            else if (node == null)
            {
                return double.NaN;
            }

            // if its a variable.
            VariableNode variableNode = node as VariableNode;
            if (variableNode != null)
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
            OperatorNode operatorNode = node as OperatorNode;
            if (operatorNode != null)
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
                        throw new NotSupportedException("Operator " + operatorNode.Operator.ToString() + " not supported.");
                }
            }

            throw new NotSupportedException();
        }
    }
}
