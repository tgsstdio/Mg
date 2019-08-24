using System;

namespace Magnesium
{
	public class MgIndirectCommandsTokenNVX
	{
		public MgIndirectCommandsTokenTypeNVX TokenType { get; set; }
        /// <summary>
        /// buffer containing tableEntries and additional data for indirectCommands
        /// </summary>
        public IMgBuffer Buffer { get; set; }
        /// <summary>
        /// offset from the base address of the buffer
        /// </summary>
        public UInt64 Offset { get; set; }
	}
}
