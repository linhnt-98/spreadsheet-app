using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW5
{
    /// <summary>
    /// Node for constant values.
    /// </summary>
    internal class ConstantNode : Node
    {
        /// <summary>
        /// Gets or sets value of the node.
        /// </summary>
        public double Value
        {
            get;
            set;
        }
    }
}
