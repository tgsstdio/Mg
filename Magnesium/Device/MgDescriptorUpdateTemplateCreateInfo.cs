using System;

namespace Magnesium
{
	public class MgDescriptorUpdateTemplateCreateInfo
	{
		public UInt32 Flags { get; set; }
		///
		/// Descriptor update entries for the template
		///
		public MgDescriptorUpdateTemplateEntry[] DescriptorUpdateEntries { get; set; }
		public MgDescriptorUpdateTemplateType TemplateType { get; set; }
		public IMgDescriptorSetLayout DescriptorSetLayout { get; set; }
		public MgPipelineBindPoint PipelineBindPoint { get; set; }
		///
		/// If used for push descriptors, this is the only allowed layout
		///
		public IMgPipelineLayout PipelineLayout { get; set; }
		public UInt32 Set { get; set; }
	}
}
