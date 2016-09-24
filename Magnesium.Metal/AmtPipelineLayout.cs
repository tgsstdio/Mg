using System;

namespace Magnesium.Metal
{
	class AmtPipelineLayout : IMgPipelineLayout
	{
		public AmtPipelineStageResourceLayout FragmentStage { get; private set; }

		public AmtPipelineStageResourceLayout VertexStage { get; private set; }

		public AmtPipelineStageResourceLayout ComputeStage { get; private set; }

		public AmtPipelineLayout(MgPipelineLayoutCreateInfo createInfo)
		{
			if (createInfo != null)
			{
				throw new ArgumentNullException(nameof(createInfo));
			}

			FragmentStage = new AmtPipelineStageResourceLayout(MgShaderStageFlagBits.FRAGMENT_BIT, createInfo);
			VertexStage = new AmtPipelineStageResourceLayout(MgShaderStageFlagBits.VERTEX_BIT, createInfo);
			ComputeStage = new AmtPipelineStageResourceLayout(MgShaderStageFlagBits.COMPUTE_BIT, createInfo);
		}

		public void DestroyPipelineLayout(IMgDevice device, IMgAllocationCallbacks allocator)
		{

		}
	}
}