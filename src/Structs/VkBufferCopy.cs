using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkBufferCopy
	{
		public UInt64 srcOffset { get; set; }
		public UInt64 dstOffset { get; set; }
		public UInt64 size { get; set; }
	}
}
