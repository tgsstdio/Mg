using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkSubmitInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 waitSemaphoreCount { get; set; }
		public UInt64 pWaitSemaphores { get; set; }
		public VkPipelineStageFlags pWaitDstStageMask { get; set; }
		public UInt32 commandBufferCount { get; set; }
		public IntPtr pCommandBuffers { get; set; }
		public UInt32 signalSemaphoreCount { get; set; }
		public UInt64 pSignalSemaphores { get; set; }
	}
}
