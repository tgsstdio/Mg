using System.Collections.Generic;

namespace Magnesium.OpenGL
{
	public interface IGLNextDescriptorPool : IMgDescriptorPool
	{
		uint MaxSets { get; }
		IDictionary<uint, IGLDescriptorSet> AllocatedSets { get; }

		IGLDescriptorPoolResource<GLTextureSlot> CombinedImageSamplers { get; }
		IGLDescriptorPoolResource<GLBufferDescriptor> UniformBuffers { get; }
		IGLDescriptorPoolResource<GLBufferDescriptor> StorageBuffers { get; }

		void ResetResource(GLDescriptorPoolResourceInfo resource);

		bool TryTake(out IGLDescriptorSet result);
	}
}