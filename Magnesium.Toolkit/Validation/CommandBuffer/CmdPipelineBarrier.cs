using System;
namespace Magnesium.Toolkit.Validation.CommandBuffer
{
	public class CmdPipelineBarrier
	{
		public static void Validate(MgPipelineStageFlagBits srcStageMask, MgPipelineStageFlagBits dstStageMask, MgDependencyFlagBits dependencyFlags, MgMemoryBarrier[] pMemoryBarriers, MgBufferMemoryBarrier[] pBufferMemoryBarriers, MgImageMemoryBarrier[] pImageMemoryBarriers)
		{
			// TODO: add validation
		}
	}
}
