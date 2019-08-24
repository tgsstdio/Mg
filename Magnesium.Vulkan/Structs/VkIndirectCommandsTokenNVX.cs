using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkIndirectCommandsTokenNVX
	{
		public MgIndirectCommandsTokenTypeNVX tokenType { get; set; }
		// buffer containing tableEntries and additional data for indirectCommands
		public UInt64 buffer { get; set; }
		// offset from the base address of the buffer
		public UInt64 offset { get; set; }
	}
}
