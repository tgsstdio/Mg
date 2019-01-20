using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkExternalImageFormatPropertiesNV
	{
		public VkImageFormatProperties imageFormatProperties { get; set; }
		public UInt32 externalMemoryFeatures { get; set; }
		public UInt32 exportFromImportedHandleTypes { get; set; }
		public UInt32 compatibleHandleTypes { get; set; }
	}
}
