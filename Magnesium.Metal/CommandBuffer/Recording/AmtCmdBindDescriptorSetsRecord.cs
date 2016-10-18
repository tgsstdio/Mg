using System;

namespace Magnesium.Metal
{
	public class AmtCmdBindDescriptorSetsRecord
	{
		public nuint VertexIndexOffset { get; set; }
		public AmtCmdBindDescriptorSetsStageRecord VertexStage { get; set; }
		public AmtCmdBindDescriptorSetsStageRecord FragmentStage { get; set; }
	}
}