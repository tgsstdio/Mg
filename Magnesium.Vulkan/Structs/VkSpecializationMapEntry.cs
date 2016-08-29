using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkSpecializationMapEntry
	{
		public UInt32 constantID { get; set; }
		public UInt32 offset { get; set; }
		public UIntPtr size { get; set; }
	}
}
