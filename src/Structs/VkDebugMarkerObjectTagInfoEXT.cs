using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkDebugMarkerObjectTagInfoEXT
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public VkDebugReportObjectTypeExt objectType { get; set; }
		public UInt64 @object { get; set; }
		public UInt64 tagName { get; set; }
		public UIntPtr tagSize { get; set; }
		public IntPtr pTag { get; set; }
	}
}
