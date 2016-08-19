using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkDrawIndexedIndirectCommand
	{
		public UInt32 indexCount { get; set; }
		public UInt32 instanceCount { get; set; }
		public UInt32 firstIndex { get; set; }
		public Int32 vertexOffset { get; set; }
		public UInt32 firstInstance { get; set; }
	}
}
