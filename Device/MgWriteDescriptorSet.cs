using System;

namespace Magnesium
{
    // Device
    public class MgWriteDescriptorSet
	{
		public MgDescriptorSet DstSet { get; set; }
		public UInt32 DstBinding { get; set; }
		public UInt32 DstArrayElement { get; set; }
		public UInt32 DescriptorCount { get; set; }
		public MgDescriptorType DescriptorType { get; set; }
		public MgDescriptorImageInfo[] ImageInfo { get; set; }
		public MgDescriptorBufferInfo[] BufferInfo { get; set; }
		public MgBufferView[] TexelBufferView { get; set; }
	}
}

