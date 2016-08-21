using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkDisplayPropertiesKHR
	{
		public IntPtr display;
		[MarshalAs(UnmanagedType.LPStr)]
		public string displayName;
		public MgExtent2D physicalDimensions;
		public MgExtent2D physicalResolution;
		public VkSurfaceTransformFlagsKhr supportedTransforms;
		public VkBool32 planeReorderPossible;
		public VkBool32 persistentContent;
	}
}
