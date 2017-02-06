namespace Magnesium.OpenGL
{
	internal interface IGLDescriptorSetBinder
	{
		void Clear();
		void Bind(MgPipelineBindPoint pipelineBindPoint, IMgPipelineLayout layout, uint firstSet,
		          uint descriptorSetCount, IMgDescriptorSet[] pDescriptorSets, uint[] pDynamicOffsets);

		IGLPipelineLayout BoundPipelineLayout { get;  }
		uint[] BoundDynamicOffsets { get; }
		IGLDescriptorSet BoundDescriptorSet { get; }
	}
}
