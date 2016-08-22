using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkSparseMemoryBind
	{
		public UInt64 resourceOffset { get; set; }
		public UInt64 size { get; set; }
		public UInt64 memory { get; set; }
		public UInt64 memoryOffset { get; set; }
		public VkSparseMemoryBindFlags flags { get; set; }
	}
}
