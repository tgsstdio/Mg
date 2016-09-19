using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkPipelineRasterizationStateRasterizationOrderAMD
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkRasterizationOrderAMD rasterizationOrder { get; set; }
	}
}
