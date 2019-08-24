using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkMultisamplePropertiesEXT
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public MgExtent2D maxSampleLocationGridSize;
	}
}
