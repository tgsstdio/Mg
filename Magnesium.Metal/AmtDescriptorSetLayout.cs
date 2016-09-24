using System;
using System.Collections.Generic;

namespace Magnesium.Metal
{
	public class AmtDescriptorSetLayout :IMgDescriptorSetLayout
	{
		public AmtDescriptorSetLayout(MgDescriptorSetLayoutCreateInfo createInfo)
		{
			if (createInfo == null)
			{
				throw new ArgumentNullException(nameof(createInfo));
			}

			var resources = new List<AmtPipelineResourceBinding>();

			if (createInfo.Bindings != null)
			{
				foreach (var binding in createInfo.Bindings)
				{
					var resource = new AmtPipelineResourceBinding
					{ 
						Binding = binding.Binding,
						Stage = binding.StageFlags,
						DescriptorCount = binding.DescriptorCount,
						DescriptorType = binding.DescriptorType
					};
					resources.Add(resource);
				}
			}
			PipelineResources = resources.ToArray();
		}

		internal AmtPipelineResourceBinding[] PipelineResources { get; private set; }

		public void DestroyDescriptorSetLayout(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			
		}
	}
}