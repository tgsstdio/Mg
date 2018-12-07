using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkBufferViewCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public UInt64 buffer { get; set; }
		public MgFormat format { get; set; }
		public UInt64 offset { get; set; }
		public UInt64 range { get; set; }
	}
}
