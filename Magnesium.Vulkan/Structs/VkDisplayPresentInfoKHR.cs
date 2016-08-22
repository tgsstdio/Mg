using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkDisplayPresentInfoKHR
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public MgRect2D srcRect { get; set; }
		public MgRect2D dstRect { get; set; }
		public VkBool32 persistent { get; set; }
	}
}
