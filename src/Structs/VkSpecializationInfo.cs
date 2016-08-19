using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkSpecializationInfo
	{
		public UInt32 mapEntryCount { get; set; }
		public VkSpecializationMapEntry pMapEntries { get; set; }
		public UIntPtr dataSize { get; set; }
		public IntPtr pData { get; set; }
	}
}
