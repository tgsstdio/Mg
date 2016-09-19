using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDispatchIndirectCommand
	{
		public UInt32 x { get; set; }
		public UInt32 y { get; set; }
		public UInt32 z { get; set; }
	}
}
