using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkDisplayPropertiesKHR
	{
		public IntPtr display { get; set; }
		public string displayName { get; set; }
		public VkExtent2D physicalDimensions { get; set; }
		public VkExtent2D physicalResolution { get; set; }
		public VkSurfaceTransformFlagsKhr supportedTransforms { get; set; }
		public VkBool32 planeReorderPossible { get; set; }
		public VkBool32 persistentContent { get; set; }
	}
}
