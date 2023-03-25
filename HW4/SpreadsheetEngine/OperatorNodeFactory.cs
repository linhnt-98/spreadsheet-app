using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    /// <summary>
    /// OperatorNodeFactory class.
    /// </summary>
    internal class OperatorNodeFactory
    {
        /// <summary>
        /// A dictionary to key char values to their types.
        /// </summary>
        private readonly Dictionary<char, Type> operators = new Dictionary<char, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNodeFactory"/> class.
        /// </summary>
        public OperatorNodeFactory()
        {
            this.LookForOperator((op, type) => this.operators.Add(op, type));
        }

        /// <summary>
        /// To pass the OnOperator Method as an argument.
        /// </summary>
        /// <param name="op">Operator.</param>
        /// <param name="type">Type.</param>
        private delegate void OnOperator(char op, Type type);

        /// <summary>
        /// To check if operator is a key in the operator dictionary.
        /// </summary>
        /// <param name="op">Operator.</param>
        /// <returns>Boolean.</returns>
        public bool IsAOperator(char op)
        {
            return this.operators.ContainsKey(op);
        }

        /// <summary>
        /// Returns the precedence of the given operator.
        /// </summary>
        /// <param name="op">operator.</param>
        /// <returns>ushort.</returns>
        public ushort Precedence(char op)
        {
            if (this.IsAOperator(op))
            {
                Type type = this.operators[op];
                PropertyInfo propertyInfo = type.GetProperty("Precedence");
                if (propertyInfo != null)
                {
                    object value = propertyInfo.GetValue(type);
                    if (value.GetType().Name == "UInt16")
                    {
                        return (ushort)value;
                    }
                }
            }

            // else return nothing
            return 0;
        }

        /// <summary>
        /// Returns the Associativity of the given operator.
        /// </summary>
        /// <param name="op">Operator.</param>
        /// <returns>int.</returns>
        public OperatorNode.AssocEnum Associativity(char op)
        {
            if (this.IsAOperator(op))
            {
                Type type = this.operators[op];
                PropertyInfo propertyInfo = type.GetProperty("Associativity");
                if (propertyInfo != null)
                {
                    object value = propertyInfo.GetValue(type);
                    if (value.GetType().Name == "Associative")
                    {
                        return (OperatorNode.AssocEnum)value;
                    }
                }
            }

            // default return left if nothing is found.
            return OperatorNode.AssocEnum.Left;
        }

        /// <summary>
        /// Creates an operator Node.
        /// </summary>
        /// <param name="op">operator.</param>
        /// <returns>Operator node.</returns>
        public OperatorNode CreateOperatorNode(char op)
        {
            if (this.IsAOperator(op))
            {
                object operatorNodeObject = Activator.CreateInstance(this.operators[op]);
                if (operatorNodeObject is OperatorNode node)
                {
                    return node;
                }
            }

            // else, throw exception.
            throw new Exception("No Operator");
        }

        /// <summary>
        /// Check if operator is available.
        /// </summary>
        /// <param name="onOperator">a delegate with operator op type.</param>
        private void LookForOperator(OnOperator onOperator)
        {
            Type operatorNodeType = typeof(OperatorNode);
            foreach (var assemply in AppDomain.CurrentDomain.GetAssemblies())
            {
                IEnumerable<Type> operatorTypes = assemply.GetTypes().Where(type => type.IsSubclassOf(operatorNodeType));

                foreach (var type in operatorTypes)
                {
                    PropertyInfo operatorField = type.GetProperty("Operator");
                    if (operatorField != null)
                    {
                        object value = operatorField.GetValue(type);
                        if (value is char operatorSymbol)
                        {
                            onOperator(operatorSymbol, type);
                        }
                    }
                }
            }
        }
    }
}
