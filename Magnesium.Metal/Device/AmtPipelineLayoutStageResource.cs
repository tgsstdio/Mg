using System;
using System.Collections.Generic;

namespace Magnesium.Metal
{
	public class AmtPipelineLayoutStageResource
	{
		public AmtPipelineLayoutStageResource(MgShaderStageFlagBits mask, MgPipelineLayoutCreateInfo createInfo)
		{
			Stage = mask;
			Initialise(mask, createInfo);
		}

		void Initialise(MgShaderStageFlagBits mask, MgPipelineLayoutCreateInfo createInfo)
		{
			var vertBuffers = new SortedList<uint, AmtPipelineLayoutBufferBinding>();
			var textures = new SortedList<uint, AmtPipelineLayoutTextureBinding>();
			var samplers = new SortedList<uint, AmtPipelineLayoutSamplerBinding>();
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

								var texture = new AmtPipelineLayoutTextureBinding
								{
									Binding = resource.Binding,
									DescriptorType = resource.DescriptorType,
								};

								var sampler = new AmtPipelineLayoutSamplerBinding
								{
									Binding = resource.Binding,
									DescriptorType = resource.DescriptorType,
								};

								textures.Add(texture.Binding, texture);
								samplers.Add(sampler.Binding, sampler);
								break;
							case MgDescriptorType.STORAGE_BUFFER:
							case MgDescriptorType.STORAGE_BUFFER_DYNAMIC:
								{
									var sBuffer = new AmtPipelineLayoutBufferBinding
									{
										Binding = resource.Binding,
										Category = AmtPipelineLayoutBufferBindingCategory.StorageBuffer,
										DescriptorCount = resource.DescriptorCount,
									};

									vertBuffers.Add(sBuffer.Binding, sBuffer);
								}
								break;

							case MgDescriptorType.UNIFORM_BUFFER:
							case MgDescriptorType.UNIFORM_BUFFER_DYNAMIC:
								{
									var vBuffer = new AmtPipelineLayoutBufferBinding
									{
										Binding = resource.Binding,
										Category = AmtPipelineLayoutBufferBindingCategory.UniformBuffer,
										DescriptorCount = resource.DescriptorCount,
									};

									vertBuffers.Add(vBuffer.Binding, vBuffer);
								}
								break;
						}
					}
				}
			}

			VertexBuffers = new AmtPipelineLayoutBufferBinding[vertBuffers.Count];
			if (VertexBuffers.Length > 0)
				vertBuffers.Values.CopyTo(VertexBuffers, 0);

			Textures = new AmtPipelineLayoutTextureBinding[textures.Count];
			if (Textures.Length > 0)
				textures.Values.CopyTo(Textures, 0);

			Samplers = new AmtPipelineLayoutSamplerBinding[samplers.Count];
			if (Samplers.Length > 0)
				samplers.Values.CopyTo(Samplers, 0);
		}

		public MgShaderStageFlagBits Stage { get; private set; }
		public AmtPipelineLayoutBufferBinding[] VertexBuffers { get; private set;}
		public AmtPipelineLayoutTextureBinding[] Textures { get; private set;}
		public AmtPipelineLayoutSamplerBinding[] Samplers { get; private set; }
	}
}
