using System;

namespace Magnesium
{
    public class MgPipelineDynamicStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgDynamicState[] DynamicStates { get; set; }
	}
}

