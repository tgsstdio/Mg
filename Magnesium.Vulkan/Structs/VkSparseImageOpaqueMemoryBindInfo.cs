using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkSparseImageOpaqueMemoryBindInfo
	{
		public UInt64 image { get; set; }
		public UInt32 bindCount { get; set; }
		public IntPtr pBinds { get; set; } // VkSparseMemoryBind
}
}
