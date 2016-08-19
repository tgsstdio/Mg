using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkSparseImageMemoryBindInfo
	{
		public UInt64 image { get; set; }
		public UInt32 bindCount { get; set; }
		public VkSparseImageMemoryBind pBinds { get; set; }
	}
}
