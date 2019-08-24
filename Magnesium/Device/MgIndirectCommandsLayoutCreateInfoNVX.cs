using System;

namespace Magnesium
{
	public class MgIndirectCommandsLayoutCreateInfoNVX
	{
		public MgPipelineBindPoint PipelineBindPoint { get; set; }
		public MgIndirectCommandsLayoutUsageFlagBitsNVX Flags { get; set; }
		public MgIndirectCommandsLayoutTokenNVX[] Tokens { get; set; }
	}
}
