using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkBindSparseInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 waitSemaphoreCount { get; set; }
		public UInt64 pWaitSemaphores { get; set; }
		public UInt32 bufferBindCount { get; set; }
		public VkSparseBufferMemoryBindInfo pBufferBinds { get; set; }
		public UInt32 imageOpaqueBindCount { get; set; }
		public VkSparseImageOpaqueMemoryBindInfo pImageOpaqueBinds { get; set; }
		public UInt32 imageBindCount { get; set; }
		public VkSparseImageMemoryBindInfo pImageBinds { get; set; }
		public UInt32 signalSemaphoreCount { get; set; }
		public UInt64 pSignalSemaphores { get; set; }
	}
}
