using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkPipelineShaderStageCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public VkShaderStageFlags stage { get; set; }
		public UInt64 module { get; set; }
		public string pName { get; set; }
		public VkSpecializationInfo pSpecializationInfo { get; set; }
	}
}
