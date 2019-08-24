using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkComputePipelineCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public MgPipelineCreateFlagBits flags { get; set; }
		public VkPipelineShaderStageCreateInfo stage { get; set; }
		public UInt64 layout { get; set; }
		public UInt64 basePipelineHandle { get; set; }
		public Int32 basePipelineIndex { get; set; }
	}
}
