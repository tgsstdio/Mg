using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDeviceGeneratedCommandsFeaturesNVX
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public VkBool32 computeBindingPointSupport;
	}
}
