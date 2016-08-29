using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkPresentInfoKHR
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 waitSemaphoreCount { get; set; }
		public IntPtr pWaitSemaphores { get; set; } // UInt64[]
		public UInt32 swapchainCount { get; set; }
		public IntPtr pSwapchains { get; set; } // array
		public IntPtr pImageIndices { get; set; } // UInt32[]
		public IntPtr pResults { get; set; } // array
	}
}
