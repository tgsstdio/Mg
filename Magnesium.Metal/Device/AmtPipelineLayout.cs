using System;

namespace Magnesium.Metal
{
	public class AmtPipelineLayout : IMgPipelineLayout
	{
		public AmtPipelineLayoutStageResource FragmentStage { get; private set; }

		public AmtPipelineLayoutStageResource VertexStage { get; private set; }

		public AmtPipelineLayoutStageResource ComputeStage { get; private set; }

		public AmtPipelineLayout(MgPipelineLayoutCreateInfo createInfo)
		{
			if (createInfo == null)
			{
				throw new ArgumentNullException(nameof(createInfo));
			}

			FragmentStage = new AmtPipelineLayoutStageResource(MgShaderStageFlagBits.FRAGMENT_BIT, createInfo);
			VertexStage = new AmtPipelineLayoutStageResource(MgShaderStageFlagBits.VERTEX_BIT, createInfo);
			ComputeStage = new AmtPipelineLayoutStageResource(MgShaderStageFlagBits.COMPUTE_BIT, createInfo);
		}

		public void DestroyPipelineLayout(IMgDevice device, IMgAllocationCallbacks allocator)
		{

		}
	}
}