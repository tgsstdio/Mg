using System;

namespace Magnesium
{
	public class MgIndirectCommandsLayoutTokenNVX
	{
		public MgIndirectCommandsTokenTypeNVX TokenType { get; set; }
        /// <summary>
        /// Binding unit for vertex attribute / descriptor set, offset for pushconstants
        /// </summary>
        public UInt32 BindingUnit { get; set; }
        /// <summary>
        /// Number of variable dynamic values for descriptor set / push constants
        /// </summary>
        public UInt32 DynamicCount { get; set; }
        /// <summary>
        /// Rate the which the array is advanced per element (must be power of 2, minimum 1)
        /// </summary>
        public UInt32 Divisor { get; set; }
	}
}
