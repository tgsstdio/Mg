using System;

namespace Magnesium
{
    public class MgPipelineInputAssemblyStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgPrimitiveTopology Topology { get; set; }
		public bool PrimitiveRestartEnable { get; set; }
	}
}

