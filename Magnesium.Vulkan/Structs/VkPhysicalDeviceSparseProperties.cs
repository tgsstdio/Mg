using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkPhysicalDeviceSparseProperties
	{
		public VkBool32 residencyStandard2DBlockShape { get; set; }
		public VkBool32 residencyStandard2DMultisampleBlockShape { get; set; }
		public VkBool32 residencyStandard3DBlockShape { get; set; }
		public VkBool32 residencyAlignedMipSize { get; set; }
		public VkBool32 residencyNonResidentStrict { get; set; }
	}
}
