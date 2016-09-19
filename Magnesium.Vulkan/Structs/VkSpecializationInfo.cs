using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkSpecializationInfo
	{
		public UInt32 mapEntryCount { get; set; }
		public IntPtr pMapEntries { get; set; } // VkSpecializationMapEntry
		public UIntPtr dataSize { get; set; }
		public IntPtr pData { get; set; }
	}
}
