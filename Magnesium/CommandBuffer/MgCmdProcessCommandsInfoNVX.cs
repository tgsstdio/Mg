using System;

namespace Magnesium
{
	public class MgCmdProcessCommandsInfoNVX
	{
		public IMgObjectTableNVX ObjectTable { get; set; }
		public IMgIndirectCommandsLayoutNVX IndirectCommandsLayout { get; set; }
		public MgIndirectCommandsTokenNVX[] IndirectCommandsTokens { get; set; }
		public UInt32 MaxSequencesCount { get; set; }
		public IMgCommandBuffer TargetCommandBuffer { get; set; }
		public IMgBuffer SequencesCountBuffer { get; set; }
		public UInt64 SequencesCountOffset { get; set; }
		public IMgBuffer SequencesIndexBuffer { get; set; }
		public UInt64 SequencesIndexOffset { get; set; }
	}
}
