using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkAcquireNextImageInfoKHR
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public UInt64 swapchain;
		public UInt64 timeout;
		public UInt64 semaphore;
		public UInt64 fence;
		public UInt32 deviceMask;
	}
}
