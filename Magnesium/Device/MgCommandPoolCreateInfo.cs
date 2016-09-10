using System;

namespace Magnesium
{
    public class MgCommandPoolCreateInfo
	{
		public MgCommandPoolCreateFlagBits Flags { get; set; }
		public UInt32 QueueFamilyIndex { get; set; }
	}

}

