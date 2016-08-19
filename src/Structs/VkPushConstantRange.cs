using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkPushConstantRange
	{
		public VkShaderStageFlags stageFlags { get; set; }
		public UInt32 offset { get; set; }
		public UInt32 size { get; set; }
	}
}
