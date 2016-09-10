using System;

namespace Magnesium
{
    public class MgPipelineViewportStateCreateInfo
	{
		public UInt32 Flags { get; set; }
		public MgViewport[] Viewports { get; set; }
		public MgRect2D[] Scissors { get; set; }
	}
}

