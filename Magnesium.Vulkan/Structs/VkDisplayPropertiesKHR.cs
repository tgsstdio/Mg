using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDisplayPropertiesKHR
	{
		public UInt64 display;
		[MarshalAs(UnmanagedType.LPStr)]
		public string displayName;
		public MgExtent2D physicalDimensions;
		public MgExtent2D physicalResolution;
		public VkSurfaceTransformFlagsKhr supportedTransforms;
		public VkBool32 planeReorderPossible;
		public VkBool32 persistentContent;
	}
}
