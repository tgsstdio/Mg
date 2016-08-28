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
		public UInt64 pWaitSemaphores { get; set; }
		public UInt32 swapchainCount { get; set; }
		public IntPtr pSwapchains { get; set; } // array
		public UInt32 pImageIndices { get; set; } 
		// Reused for results
		public IntPtr pResults { get; set; } // array
	}
}
