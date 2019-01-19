using System;

namespace Magnesium
{

    public class MgObjectTableCreateInfoNVX
	{
        public MgObjectTableCreateInfoEntryNVX[] Entries { get; set; }
		public UInt32 MaxUniformBuffersPerDescriptor { get; set; }
		public UInt32 MaxStorageBuffersPerDescriptor { get; set; }
		public UInt32 MaxStorageImagesPerDescriptor { get; set; }
		public UInt32 MaxSampledImagesPerDescriptor { get; set; }
		public UInt32 MaxPipelineLayouts { get; set; }
	}
}
