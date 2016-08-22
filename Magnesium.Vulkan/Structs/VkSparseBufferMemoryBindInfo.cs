using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkSparseBufferMemoryBindInfo
	{
		public UInt64 buffer { get; set; }
		public UInt32 bindCount { get; set; }
		public VkSparseMemoryBind pBinds { get; set; }
	}
}
