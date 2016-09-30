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
		}

		public AmtDescriptorSetBindingMap Compute { get; private set; }
		public AmtDescriptorSetBindingMap Vertex { get; private set; }
		public AmtDescriptorSetBindingMap Fragment { get; private set;}
		// TODO : tesselation

		public void Initialize(AmtDescriptorSetLayout layout)
		{
			var computeResources = new AmtPipelineLayoutStageResource(MgShaderStageFlagBits.COMPUTE_BIT, layout);
			Compute = new AmtDescriptorSetBindingMap(computeResources);

			var vertResources = new AmtPipelineLayoutStageResource(MgShaderStageFlagBits.VERTEX_BIT, layout);
			Vertex = new AmtDescriptorSetBindingMap(vertResources);

			var fragResources = new AmtPipelineLayoutStageResource(MgShaderStageFlagBits.FRAGMENT_BIT, layout);
			Fragment = new AmtDescriptorSetBindingMap(fragResources);
		}

		internal void Reset()
		{
			throw new NotImplementedException();
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
