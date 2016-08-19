using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkDescriptorBufferInfo
	{
		public UInt64 buffer { get; set; }
		public UInt64 offset { get; set; }
		public UInt64 range { get; set; }
	}
}
