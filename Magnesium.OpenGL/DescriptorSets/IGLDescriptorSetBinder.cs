namespace Magnesium.OpenGL.Internals
{
	public interface IGLDescriptorSetBinder
	{
		void Clear();
		void Bind(MgPipelineBindPoint pipelineBindPoint, IMgPipelineLayout layout, uint firstSet,
		          uint descriptorSetCount, IMgDescriptorSet[] pDescriptorSets, uint[] pDynamicOffsets);
        bool IsInvalid { get; }
		IGLPipelineLayout BoundPipelineLayout { get;  }
		uint[] BoundDynamicOffsets { get; }
		IGLDescriptorSet BoundDescriptorSet { get; }
	}
}
