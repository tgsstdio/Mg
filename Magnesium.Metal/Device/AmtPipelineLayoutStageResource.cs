using System;
using System.Collections.Generic;

namespace Magnesium.Metal
{
	public class AmtPipelineLayoutStageResource
	{
		public AmtPipelineLayoutStageResource(
			MgShaderStageFlagBits mask, 
          	MgPipelineLayoutCreateInfo createInfo)
		{
			var vertBuffers = new SortedList<uint, AmtPipelineLayoutBufferBinding>();
			var textures = new SortedList<uint, AmtPipelineLayoutTextureBinding>();
			var samplers = new SortedList<uint, AmtPipelineLayoutSamplerBinding>();

			Stage = mask;
			foreach (var setLayout in createInfo.SetLayouts)
			{
				var bSetLayout = (AmtDescriptorSetLayout)setLayout;
				Initialise(mask, bSetLayout, null, vertBuffers, textures, samplers);
			}
			ConvertToArrays(vertBuffers, textures, samplers);
		}

		public AmtPipelineLayoutStageResource(
			MgShaderStageFlagBits mask,
			AmtDescriptorSetLayout setLayout,
			IAmtDescriptorSetBindingLocator locator)
		{
			var vertBuffers = new SortedList<uint, AmtPipelineLayoutBufferBinding>();
			var textures = new SortedList<uint, AmtPipelineLayoutTextureBinding>();
			var samplers = new SortedList<uint, AmtPipelineLayoutSamplerBinding>();

			Stage = mask;
			Initialise(mask, setLayout, locator, vertBuffers, textures, samplers);
			ConvertToArrays(vertBuffers, textures, samplers);
		}

		void ConvertToArrays(
			SortedList<uint, AmtPipelineLayoutBufferBinding> vertBuffers, 
			SortedList<uint, AmtPipelineLayoutTextureBinding> textures,
			SortedList<uint, AmtPipelineLayoutSamplerBinding> samplers)
		{
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

		void Initialise(
			MgShaderStageFlagBits mask,
			AmtDescriptorSetLayout setLayout,
			IAmtDescriptorSetBindingLocator locator,
			SortedList<uint, AmtPipelineLayoutBufferBinding> vertBuffers,
			SortedList<uint, AmtPipelineLayoutTextureBinding> textures, 
			SortedList<uint, AmtPipelineLayoutSamplerBinding> samplers
		)
		{
			for (var i = 0; i < setLayout.PipelineResources.Length; ++i)
			{
				var resource = setLayout.PipelineResources[i];

				AmtDescriptorSetUpdateKey item = null;
				if (locator != null)
				{
					if (!locator.TryGetValue(resource.Binding, out item))
					{
						item = locator.Add(resource.Binding);
					}
				}

				if ((resource.Stage & mask) == mask)
				{
					switch (resource.DescriptorType)
					{
						case MgDescriptorType.COMBINED_IMAGE_SAMPLER:
						case MgDescriptorType.SAMPLED_IMAGE:

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

							if (locator != null)
							{
								var nextTextureIndex = (uint)textures.Count;
								var nextSamplerIndex = (uint)samplers.Count;
								item.SetCombinedSampler(mask, nextTextureIndex, nextSamplerIndex);
							}

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

								if (locator != null)
								{
									var nextOffset = (uint)vertBuffers.Count;
									item.SetBuffer(mask, nextOffset);
								}

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

								if (locator != null)
								{
									var nextOffset = (uint)vertBuffers.Count;
									item.SetBuffer(mask, nextOffset);
								}

								vertBuffers.Add(vBuffer.Binding, vBuffer);
							}
							break;
					}
				}
			}
		}

		public MgShaderStageFlagBits Stage { get; private set; }
		public AmtPipelineLayoutBufferBinding[] VertexBuffers { get; private set;}
		public AmtPipelineLayoutTextureBinding[] Textures { get; private set;}
		public AmtPipelineLayoutSamplerBinding[] Samplers { get; private set; }
	}
}
