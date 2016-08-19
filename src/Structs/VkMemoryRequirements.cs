using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkMemoryRequirements
	{
		public UInt64 size { get; set; }
		public UInt64 alignment { get; set; }
		public UInt32 memoryTypeBits { get; set; }
	}
}
