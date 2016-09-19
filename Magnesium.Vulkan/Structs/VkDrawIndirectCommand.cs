using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDrawIndirectCommand
	{
		public UInt32 vertexCount { get; set; }
		public UInt32 instanceCount { get; set; }
		public UInt32 firstVertex { get; set; }
		public UInt32 firstInstance { get; set; }
	}
}
