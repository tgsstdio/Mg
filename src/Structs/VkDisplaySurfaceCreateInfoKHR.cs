using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkDisplaySurfaceCreateInfoKHR
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public UInt64 displayMode { get; set; }
		public UInt32 planeIndex { get; set; }
		public UInt32 planeStackIndex { get; set; }
		public VkSurfaceTransformFlagsKhr transform { get; set; }
		public float globalAlpha { get; set; }
		public VkDisplayPlaneAlphaFlagsKhr alphaMode { get; set; }
		public VkExtent2D imageExtent { get; set; }
	}
}
