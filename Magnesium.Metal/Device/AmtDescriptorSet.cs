using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtDescriptorSet : IMgDescriptorSet, IEquatable<AmtDescriptorSet>
	{
		public int Key { get; private set; }

		// BUFFERS, (TEXTURE, SAMPLER)

		public AmtDescriptorSet(int i)
		{
			this.Key = i;
			Locator = new AmtDescriptorSetBindingLocator();
		}


		public AmtDescriptorSetBindingMap Compute { get; private set; }
		public AmtDescriptorSetBindingMap Vertex { get; private set; }
		public AmtDescriptorSetBindingMap Fragment { get; private set;}
		// TODO : tesselation

		public IAmtDescriptorSetBindingLocator Locator { get; private set; }

		public void Initialize(AmtDescriptorSetLayout layout)
		{
			// TODO : create a dictionary to find which location to bind
			//PipelineResources = layout.PipelineResources;

			var computeResources = new AmtPipelineLayoutStageResource(MgShaderStageFlagBits.COMPUTE_BIT, layout, Locator);
			Compute = new AmtDescriptorSetBindingMap(computeResources);

			var vertResources = new AmtPipelineLayoutStageResource(MgShaderStageFlagBits.VERTEX_BIT, layout, Locator);
			Vertex = new AmtDescriptorSetBindingMap(vertResources);

			var fragResources = new AmtPipelineLayoutStageResource(MgShaderStageFlagBits.FRAGMENT_BIT, layout, Locator);
			Fragment = new AmtDescriptorSetBindingMap(fragResources);
		}

		internal void Reset()
		{
			Compute.Reset();
			Vertex.Reset();
			Fragment.Reset();
		}

		public bool Equals(AmtDescriptorSet other)
		{
			return Key == other.Key;
		}

		public override int GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;
				// Suitable nullity checks etc, of course :)
				//hash = hash * 23 + Pool.GetHashCode();
				hash = hash * 23 + Key.GetHashCode();
				return hash;
			}
		}
	}
}
