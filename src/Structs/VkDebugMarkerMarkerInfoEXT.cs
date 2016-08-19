using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkDebugMarkerMarkerInfoEXT
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public string pMarkerName { get; set; }
		public float color { get; set; }
	}
}
