using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDebugMarkerObjectNameInfoEXT
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkDebugReportObjectTypeExt objectType { get; set; }
		public UInt64 @object { get; set; }
		public string pObjectName { get; set; }
	}
}
