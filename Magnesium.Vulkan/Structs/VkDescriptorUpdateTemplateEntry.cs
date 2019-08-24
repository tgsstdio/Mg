using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDescriptorUpdateTemplateEntry
	{
		// Binding within the destination descriptor set to write
		public UInt32 dstBinding;
		// Array element within the destination binding to write
		public UInt32 dstArrayElement;
		// Number of descriptors to write
		public UInt32 descriptorCount;
		// Descriptor type to write
		public MgDescriptorType descriptorType;
		// Offset into pData where the descriptors to update are stored
		public UIntPtr offset;
		// Stride between two descriptors in pData when writing more than one descriptor
		public UIntPtr stride;
	}
}
