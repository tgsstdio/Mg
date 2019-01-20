using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
    [StructLayout(LayoutKind.Sequential)]
	internal struct VkCheckpointDataNV
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public MgPipelineStageFlagBits stage { get; set; }
		public IntPtr pCheckpointMarker { get; set; }
	}
}
