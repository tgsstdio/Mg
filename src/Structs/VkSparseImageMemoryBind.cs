using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkSparseImageMemoryBind
	{
		public VkImageSubresource subresource { get; set; }
		public VkOffset3D offset { get; set; }
		public VkExtent3D extent { get; set; }
		public UInt64 memory { get; set; }
		public UInt64 memoryOffset { get; set; }
		public VkSparseMemoryBindFlags flags { get; set; }
	}
}
