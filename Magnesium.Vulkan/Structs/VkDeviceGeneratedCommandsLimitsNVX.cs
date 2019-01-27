using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDeviceGeneratedCommandsLimitsNVX
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public UInt32 maxIndirectCommandsLayoutTokenCount;
		public UInt32 maxObjectEntryCounts;
		public UInt32 minSequenceCountBufferOffsetAlignment;
		public UInt32 minSequenceIndexBufferOffsetAlignment;
		public UInt32 minCommandsTokenBufferOffsetAlignment;
	}
}
