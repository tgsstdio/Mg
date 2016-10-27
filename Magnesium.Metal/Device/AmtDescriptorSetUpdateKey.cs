using System;
namespace Magnesium.Metal
{
	public class AmtDescriptorSetUpdateKey
	{
		public MgShaderStageFlagBits Stage { get; set; }
		//publicb  DescriptorType { get; set;}
		public uint VertexOffset { get; set; }
		public uint VertexSamplerIndex { get; set; }
		public uint ComputeOffset { get; set; }
		public uint ComputeSamplerIndex { get; set; }
		public uint FragmentOffset { get; set; }
		public uint FragmentSamplerIndex { get; set; }

		public void SetCombinedSampler(MgShaderStageFlagBits mask, uint offset, uint samplerIndex)
		{
			switch (mask)
			{
				case MgShaderStageFlagBits.COMPUTE_BIT:
					ComputeOffset = offset;
					ComputeSamplerIndex = samplerIndex;
					break;
				case MgShaderStageFlagBits.FRAGMENT_BIT:
					FragmentOffset = offset;
					FragmentSamplerIndex = samplerIndex;
					break;
				case MgShaderStageFlagBits.VERTEX_BIT:
					VertexOffset = offset;
					VertexSamplerIndex = samplerIndex;
					break;
				default:
					throw new NotSupportedException();
			}
			//DescriptorType = MgDescriptorType.COMBINED_IMAGE_SAMPLER;
			Stage |= mask;
		}

		public void SetBuffer(MgShaderStageFlagBits mask, uint offset)
		{
			switch (mask)
			{
				case MgShaderStageFlagBits.COMPUTE_BIT:
					ComputeOffset = offset;
					break;
				case MgShaderStageFlagBits.FRAGMENT_BIT:
					FragmentOffset = offset;
					break;
				case MgShaderStageFlagBits.VERTEX_BIT:
					VertexOffset = offset;
					break;
				default:
					throw new NotSupportedException();
			}
			//DescriptorType = MgDescriptorType.;
			Stage |= mask;
		}
	}
}
