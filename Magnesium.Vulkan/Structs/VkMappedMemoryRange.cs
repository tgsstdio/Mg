using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkMappedMemoryRange
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt64 memory { get; set; }
		public UInt64 offset { get; set; }
		public UInt64 size { get; set; }
	}
}
