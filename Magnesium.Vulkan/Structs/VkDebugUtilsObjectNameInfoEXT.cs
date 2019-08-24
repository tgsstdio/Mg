using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDebugUtilsObjectNameInfoEXT
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public MgObjectType objectType { get; set; }
		public UInt64 objectHandle { get; set; }
		public IntPtr pObjectName { get; set; }
	}
}
