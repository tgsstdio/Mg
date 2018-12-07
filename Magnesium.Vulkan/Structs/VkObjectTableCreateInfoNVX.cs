using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkObjectTableCreateInfoNVX
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 objectCount { get; set; }
		public IntPtr pObjectEntryTypes { get; set; }
		public IntPtr pObjectEntryCounts { get; set; }
		public IntPtr pObjectEntryUsageFlags { get; set; }
		public UInt32 maxUniformBuffersPerDescriptor { get; set; }
		public UInt32 maxStorageBuffersPerDescriptor { get; set; }
		public UInt32 maxStorageImagesPerDescriptor { get; set; }
		public UInt32 maxSampledImagesPerDescriptor { get; set; }
		public UInt32 maxPipelineLayouts { get; set; }
	}
}
