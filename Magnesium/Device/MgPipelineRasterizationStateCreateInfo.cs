using System;

namespace Magnesium
{
    public class MgPipelineRasterizationStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public bool DepthClampEnable { get; set; }
		public bool RasterizerDiscardEnable { get; set; }
		public MgPolygonMode PolygonMode { get; set; }
		public MgCullModeFlagBits CullMode { get; set; }
		public MgFrontFace FrontFace { get; set; }
		public bool DepthBiasEnable { get; set; }
		public float DepthBiasConstantFactor { get; set; }
		public float DepthBiasClamp { get; set; }
		public float DepthBiasSlopeFactor { get; set; }
		public float LineWidth { get; set; }
	}
}

