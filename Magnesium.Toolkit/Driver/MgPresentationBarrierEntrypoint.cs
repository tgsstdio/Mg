using System;
using System.Diagnostics;

namespace Magnesium.Toolkit
{
	public class MgPresentationBarrierEntrypoint : IMgPresentationBarrierEntrypoint
	{
		public void SubmitPrePresentBarrier(IMgCommandBuffer prePresent, IMgImage image)
		{
			if (prePresent == null)
				throw new InvalidOperationException();

			MgCommandBufferBeginInfo cmdBufInfo = new MgCommandBufferBeginInfo
			{

			};

			var vkRes = prePresent.BeginCommandBuffer(cmdBufInfo);
			Debug.Assert(vkRes == MgResult.SUCCESS, vkRes + " != MgResult.SUCCESS");

			const uint VK_QUEUE_FAMILY_IGNORED = ~0U;

			var prePresentBarrier = new MgImageMemoryBarrier
			{
				Image = image,
				SrcAccessMask = MgAccessFlagBits.COLOR_ATTACHMENT_WRITE_BIT,
				DstAccessMask = 0,
				OldLayout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
				NewLayout = MgImageLayout.PRESENT_SRC_KHR,
				SrcQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
				DstQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
				SubresourceRange = new MgImageSubresourceRange
				{
					AspectMask = MgImageAspectFlagBits.COLOR_BIT,
					BaseArrayLayer = 0,
					LayerCount = 1,
					BaseMipLevel = 0,
					LevelCount = 1,
				},
			};

			const int VK_FLAGS_NONE = 0;
			prePresent.CmdPipelineBarrier(
				MgPipelineStageFlagBits.ALL_COMMANDS_BIT,
				MgPipelineStageFlagBits.TOP_OF_PIPE_BIT,
				VK_FLAGS_NONE,
				null, // No memory barriers,
				null, // No buffer barriers,
				new[] { prePresentBarrier });

			vkRes = prePresent.EndCommandBuffer();
			Debug.Assert(vkRes == MgResult.SUCCESS, vkRes + " != MgResult.SUCCESS");


		}

		public void SubmitPostPresentBarrier(IMgCommandBuffer postPresent, IMgImage image)
		{
			if (postPresent == null)
				throw new InvalidOperationException();

			MgCommandBufferBeginInfo cmdBufInfo = new MgCommandBufferBeginInfo
			{

			};

			var vkRes = postPresent.BeginCommandBuffer(cmdBufInfo);
			Debug.Assert(vkRes == MgResult.SUCCESS, vkRes + " != MgResult.SUCCESS");

			const uint VK_QUEUE_FAMILY_IGNORED = ~0U;

			var postPresentBarrier = new MgImageMemoryBarrier
			{
				Image = image,
				SrcAccessMask = 0,
				DstAccessMask = MgAccessFlagBits.COLOR_ATTACHMENT_WRITE_BIT,
				OldLayout = MgImageLayout.PRESENT_SRC_KHR,
				NewLayout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
				SrcQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
				DstQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
				SubresourceRange = new MgImageSubresourceRange
				{
					AspectMask = MgImageAspectFlagBits.COLOR_BIT,
					BaseArrayLayer = 0,
					LayerCount = 1,
					BaseMipLevel = 0,
					LevelCount = 1,
				},
			};

			postPresent.CmdPipelineBarrier(
				MgPipelineStageFlagBits.ALL_COMMANDS_BIT,
				MgPipelineStageFlagBits.TOP_OF_PIPE_BIT,
				0,
				null, // No memory barriers,
				null, // No buffer barriers,
				new[] { postPresentBarrier });

			vkRes = postPresent.EndCommandBuffer();
			Debug.Assert(vkRes == MgResult.SUCCESS, vkRes + " != MgResult.SUCCESS");
		}
	}
}
