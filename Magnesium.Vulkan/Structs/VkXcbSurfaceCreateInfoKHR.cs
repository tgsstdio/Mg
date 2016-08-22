using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkXcbSurfaceCreateInfoKHR
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public IntPtr connection { get; set; }
		public IntPtr window { get; set; }
	}
}
