using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkDedicatedAllocationImageCreateInfoNV
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkBool32 dedicatedAllocation { get; set; }
	}
}
