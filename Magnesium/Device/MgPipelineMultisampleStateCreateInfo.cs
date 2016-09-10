using System;

namespace Magnesium
{
    public class MgPipelineMultisampleStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgSampleCountFlagBits RasterizationSamples { get; set; }
		public bool SampleShadingEnable { get; set; }
		public float MinSampleShading { get; set; }
		public UInt32[] SampleMask { get; set; }
		public bool AlphaToCoverageEnable { get; set; }
		public bool AlphaToOneEnable { get; set; }
	}
}

