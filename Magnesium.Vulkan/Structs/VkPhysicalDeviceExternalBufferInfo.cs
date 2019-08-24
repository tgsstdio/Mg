using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkPhysicalDeviceExternalBufferInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkBufferCreateFlags flags { get; set; }
		public MgBufferUsageFlagBits usage { get; set; }
		public MgExternalMemoryHandleTypeFlagBits handleType { get; set; }
	}
}
