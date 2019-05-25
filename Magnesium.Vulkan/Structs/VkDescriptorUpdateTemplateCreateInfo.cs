using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkDescriptorUpdateTemplateCreateInfo
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public UInt32 flags;
		// Number of descriptor update entries to use for the update template
		public UInt32 descriptorUpdateEntryCount;
		// Descriptor update entries for the template
		public IntPtr pDescriptorUpdateEntries;
		public MgDescriptorUpdateTemplateType templateType;
		public UInt64 descriptorSetLayout;
		public MgPipelineBindPoint pipelineBindPoint;
		// If used for push descriptors, this is the only allowed layout
		public UInt64 pipelineLayout;
		public UInt32 set;
	}
}
