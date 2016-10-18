using System;
namespace Magnesium.Metal
{
	public class AmtDescriptorSetBindingMap
	{
		public AmtDescriptorSetBufferBinding[] Buffers { get; private set; }
		public AmtDescriptorSetSamplerBinding[] SamplerStates { get; private set; }
		public AmtDescriptorSetTextureBinding[] Textures { get; private set; }

		public AmtDescriptorSetBindingMap(AmtPipelineLayoutStageResource resources)
		{
			Buffers = new AmtDescriptorSetBufferBinding[resources.VertexBuffers.Length];
			for (var i = 0; i < Buffers.Length; ++i)
			{
				Buffers[i] = new AmtDescriptorSetBufferBinding
				{
				};
			}

			SamplerStates = new AmtDescriptorSetSamplerBinding[resources.Samplers.Length];
			for (var i = 0; i < SamplerStates.Length; ++i)
			{
				SamplerStates[i] = new AmtDescriptorSetSamplerBinding
				{
				};
			}

			Textures = new AmtDescriptorSetTextureBinding[resources.Textures.Length];
			for (var i = 0; i < Textures.Length; ++i)
			{
				Textures[i] = new AmtDescriptorSetTextureBinding
				{
				};
			}
		}

		public void Reset()
		{
			for (var i = 0; i < Buffers.Length; ++i)
			{
				Buffers[i].Buffer = null;
			}

			for (var i = 0; i < SamplerStates.Length; ++i)
			{
				SamplerStates[i].Sampler = null;
			}

			for (var i = 0; i < Textures.Length; ++i)
			{
				Textures[i].Texture = null;
			}
		}
	}
}
