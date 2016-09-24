using System;
using System.Collections.Generic;

namespace Magnesium.Metal
{
	public class AmtPipelineStageResourceLayout
	{
		public AmtPipelineStageResourceLayout(MgShaderStageFlagBits mask, MgPipelineLayoutCreateInfo createInfo)
		{
			Stage = mask;
			Initialise(mask, createInfo);
		}

		void Initialise(MgShaderStageFlagBits mask, MgPipelineLayoutCreateInfo createInfo)
		{
			var vertBuffers = new SortedList<uint, AmtPipelineVertexBufferBinding>();
			var textures = new SortedList<uint, AmtPipelineTextureBinding>();
			var samplers = new SortedList<uint, AmtPipelineSamplerBinding>();
			foreach (var setLayout in createInfo.SetLayouts)
			{
				var bSetLayout = (AmtDescriptorSetLayout)setLayout;
				foreach (var resource in bSetLayout.PipelineResources)
				{
					if ((resource.Stage & mask) == mask)
					{
						switch (resource.DescriptorType)
						{
							case MgDescriptorType.COMBINED_IMAGE_SAMPLER:
							case MgDescriptorType.SAMPLER:

								var texture = new AmtPipelineTextureBinding
								{
									Binding = resource.Binding,
									DescriptorType = resource.DescriptorType,
								};

								var sampler = new AmtPipelineSamplerBinding
								{
									Binding = resource.Binding,
									DescriptorType = resource.DescriptorType,
								};

								textures.Add(texture.Binding, texture);
								samplers.Add(sampler.Binding, sampler);
								break;
							case MgDescriptorType.STORAGE_BUFFER:
							case MgDescriptorType.STORAGE_BUFFER_DYNAMIC:
							case MgDescriptorType.UNIFORM_BUFFER:
							case MgDescriptorType.UNIFORM_BUFFER_DYNAMIC:

								var vBuffer = new AmtPipelineVertexBufferBinding
								{
									Binding = resource.Binding,
									DescriptorType = resource.DescriptorType,
									DescriptorCount = resource.DescriptorCount,
								};

								vertBuffers.Add(vBuffer.Binding, vBuffer);

								break;
						}
					}
				}
			}

			VertexBuffers = new AmtPipelineVertexBufferBinding[vertBuffers.Count];
			if (VertexBuffers.Length > 0)
				vertBuffers.Values.CopyTo(VertexBuffers, 0);

			Textures = new AmtPipelineTextureBinding[textures.Count];
			if (Textures.Length > 0)
				textures.Values.CopyTo(Textures, 0);

			Samplers = new AmtPipelineSamplerBinding[samplers.Count];
			if (Samplers.Length > 0)
				samplers.Values.CopyTo(Samplers, 0);
		}

		public MgShaderStageFlagBits Stage { get; private set; }
		public AmtPipelineVertexBufferBinding[] VertexBuffers { get; private set;}
		public AmtPipelineTextureBinding[] Textures { get; private set;}
		public AmtPipelineSamplerBinding[] Samplers { get; private set; }
	}
}
