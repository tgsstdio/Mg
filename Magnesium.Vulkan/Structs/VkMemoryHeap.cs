using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkMemoryHeap
	{
		public UInt64 size { get; set; }
		public VkMemoryHeapFlags flags { get; set; }
	}
}
