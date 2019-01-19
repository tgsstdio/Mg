using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDebugUtilsLabelEXT
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public IntPtr pLabelName { get; set; }
		public MgVec4f color { get; set; }
	}
}
