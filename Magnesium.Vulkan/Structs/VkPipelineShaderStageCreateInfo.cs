using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkPipelineShaderStageCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public MgShaderStageFlagBits stage { get; set; }
		public UInt64 module { get; set; }
		public IntPtr pName { get; set; }
		public IntPtr pSpecializationInfo { get; set; }
	}
}
