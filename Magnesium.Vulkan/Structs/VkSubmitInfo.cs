using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkSubmitInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 waitSemaphoreCount { get; set; }
		public IntPtr pWaitSemaphores { get; set; } // UInt64[]
		public IntPtr pWaitDstStageMask { get; set; } // VkPipelineStageFlags[]
		public UInt32 commandBufferCount { get; set; }
		public IntPtr pCommandBuffers { get; set; } // IntPtr[]
		public UInt32 signalSemaphoreCount { get; set; }
		public IntPtr pSignalSemaphores { get; set; } // UInt64[]
}
}
