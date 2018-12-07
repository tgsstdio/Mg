using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkVertexInputAttributeDescription
	{
		public UInt32 location { get; set; }
		public UInt32 binding { get; set; }
		public MgFormat format { get; set; }
		public UInt32 offset { get; set; }
	}
}
