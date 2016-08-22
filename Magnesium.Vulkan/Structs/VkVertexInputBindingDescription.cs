using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkVertexInputBindingDescription
	{
		public UInt32 binding { get; set; }
		public UInt32 stride { get; set; }
		public VkVertexInputRate inputRate { get; set; }
	}
}
