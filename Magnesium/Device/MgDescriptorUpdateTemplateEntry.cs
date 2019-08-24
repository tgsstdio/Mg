using System;

namespace Magnesium
{
	public class MgDescriptorUpdateTemplateEntry
	{
		///
		/// Binding within the destination descriptor set to write
		///
		public UInt32 DstBinding { get; set; }
		///
		/// Array element within the destination binding to write
		///
		public UInt32 DstArrayElement { get; set; }
		///
		/// Number of descriptors to write
		///
		public UInt32 DescriptorCount { get; set; }
		///
		/// Descriptor type to write
		///
		public MgDescriptorType DescriptorType { get; set; }
		///
		/// Offset into pData where the descriptors to update are stored
		///
		public UIntPtr Offset { get; set; }
		///
		/// Stride between two descriptors in pData when writing more than one descriptor
		///
		public UIntPtr Stride { get; set; }
	}
}
