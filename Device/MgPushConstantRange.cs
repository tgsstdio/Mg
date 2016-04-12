using System;

namespace Magnesium
{
    public class MgPushConstantRange
	{
		public MgShaderStageFlagBits StageFlags { get; set; }
		public UInt32 Offset { get; set; }
		public UInt32 Size { get; set; }
	}
}

