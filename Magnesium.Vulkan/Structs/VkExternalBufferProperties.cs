using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkExternalBufferProperties
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkExternalMemoryProperties externalMemoryProperties { get; set; }
	}
}
