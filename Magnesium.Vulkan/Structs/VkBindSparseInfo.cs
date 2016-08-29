using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkBindSparseInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 waitSemaphoreCount { get; set; }
		public IntPtr pWaitSemaphores { get; set; } // UInt64[]
		public UInt32 bufferBindCount { get; set; }
		public IntPtr pBufferBinds { get; set; } // VkSparseBufferMemoryBindInfo[]
		public UInt32 imageOpaqueBindCount { get; set; }
		public IntPtr pImageOpaqueBinds { get; set; } // VkSparseImageOpaqueMemoryBindInfo
		public UInt32 imageBindCount { get; set; }
		public IntPtr pImageBinds { get; set; } // VkSparseImageMemoryBindInfo[]
		public UInt32 signalSemaphoreCount { get; set; }
		public IntPtr pSignalSemaphores { get; set; } // UInt64[]
}
}
