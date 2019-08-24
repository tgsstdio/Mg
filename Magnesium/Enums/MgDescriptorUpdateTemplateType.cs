using System;

namespace Magnesium
{
	public enum MgDescriptorUpdateTemplateType : UInt32
	{
		/// <summary> 
		/// Create descriptor update template for descriptor set updates
		/// </summary> 
		DESCRIPTOR_SET = 0,
		/// <summary> 
		/// Create descriptor update template for pushed descriptor updates
		/// </summary> 
		PUSH_DESCRIPTORS_KHR = 1,
		DESCRIPTOR_SET_KHR = DESCRIPTOR_SET,
	}
}
