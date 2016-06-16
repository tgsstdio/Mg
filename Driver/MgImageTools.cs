using System;
using Magnesium;

namespace Magnesium
{
	public class MgImageTools : IMgImageTools
	{
		// Fixed sub resource on first mip level and layer
		public void SetImageLayout(
			IMgCommandBuffer cmdbuffer,
			IMgImage image,
			MgImageAspectFlagBits aspectMask,
			MgImageLayout oldImageLayout,
			MgImageLayout newImageLayout)
		{
			var subresourceRange = new MgImageSubresourceRange{
				AspectMask = aspectMask,
				BaseMipLevel = 0,
				LevelCount = 1,
				LayerCount = 1,
			};
			PushImageMemoryBarrier(cmdbuffer, image, aspectMask, oldImageLayout, newImageLayout, subresourceRange);
		}

		public void SetImageLayout (IMgCommandBuffer cmdbuffer, IMgImage image, MgImageAspectFlagBits aspectMask, MgImageLayout oldImageLayout, MgImageLayout newImageLayout, uint mipLevel, uint mipLevelCount)
		{
			throw new NotImplementedException ();
		}

		// Create an image memory barrier for changing the layout of
		// an image and put it into an active command buffer
		// See chapter 11.4 "Image Layout" for details
		private void PushImageMemoryBarrier(
			IMgCommandBuffer cmdbuffer, 
			IMgImage image, 
			MgImageAspectFlagBits aspectMask, 
			MgImageLayout oldImageLayout, 
			MgImageLayout newImageLayout,
			MgImageSubresourceRange subresourceRange)
		{
			const uint VK_QUEUE_FAMILY_IGNORED = ~0U;// 0xffffffff;

			// Create an image barrier object
			MgImageMemoryBarrier imageMemoryBarrier = new MgImageMemoryBarrier{
				OldLayout = oldImageLayout,
				NewLayout = newImageLayout,
				Image = image,
				SubresourceRange = subresourceRange,
				// Some default values
				SrcQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
				DstQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED,
			};

			// Source layouts (old)

			// Undefined layout
			// Only allowed as initial layout!
			// Make sure any writes to the image have been finished
			if (oldImageLayout == MgImageLayout.PREINITIALIZED)
			{
				imageMemoryBarrier.SrcAccessMask = (MgAccessFlagBits.HOST_WRITE_BIT | MgAccessFlagBits.TRANSFER_WRITE_BIT);
			}

			// Old layout is color attachment
			// Make sure any writes to the color buffer have been finished
			if (oldImageLayout == MgImageLayout.COLOR_ATTACHMENT_OPTIMAL) 
			{
				imageMemoryBarrier.SrcAccessMask = MgAccessFlagBits.COLOR_ATTACHMENT_WRITE_BIT;
			}

			// Old layout is depth/stencil attachment
			// Make sure any writes to the depth/stencil buffer have been finished
			if (oldImageLayout == MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL)
			{
				imageMemoryBarrier.SrcAccessMask = MgAccessFlagBits.DEPTH_STENCIL_ATTACHMENT_WRITE_BIT;
			}

			// Old layout is transfer source
			// Make sure any reads from the image have been finished
			if (oldImageLayout == MgImageLayout.TRANSFER_SRC_OPTIMAL)
			{
				imageMemoryBarrier.SrcAccessMask = MgAccessFlagBits.TRANSFER_READ_BIT;
			}

			// Old layout is shader read (sampler, input attachment)
			// Make sure any shader reads from the image have been finished
			if (oldImageLayout == MgImageLayout.SHADER_READ_ONLY_OPTIMAL)
			{
				imageMemoryBarrier.SrcAccessMask = MgAccessFlagBits.SHADER_READ_BIT;
			}

			// Target layouts (new)

			// New layout is transfer destination (copy, blit)
			// Make sure any copyies to the image have been finished
			if (newImageLayout == MgImageLayout.TRANSFER_DST_OPTIMAL)
			{
				imageMemoryBarrier.DstAccessMask = MgAccessFlagBits.TRANSFER_WRITE_BIT;
			}

			// New layout is transfer source (copy, blit)
			// Make sure any reads from and writes to the image have been finished
			if (newImageLayout == MgImageLayout.TRANSFER_SRC_OPTIMAL)
			{
				imageMemoryBarrier.SrcAccessMask = imageMemoryBarrier.SrcAccessMask | MgAccessFlagBits.TRANSFER_READ_BIT;
				imageMemoryBarrier.DstAccessMask =  MgAccessFlagBits.TRANSFER_READ_BIT;
			}

			// New layout is color attachment
			// Make sure any writes to the color buffer have been finished
			if (newImageLayout == MgImageLayout.COLOR_ATTACHMENT_OPTIMAL)
			{
				imageMemoryBarrier.DstAccessMask =  MgAccessFlagBits.COLOR_ATTACHMENT_WRITE_BIT;
				imageMemoryBarrier.SrcAccessMask = MgAccessFlagBits.TRANSFER_READ_BIT;
			}

			// New layout is depth attachment
			// Make sure any writes to depth/stencil buffer have been finished
			if (newImageLayout == MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL) 
			{
				imageMemoryBarrier.DstAccessMask = imageMemoryBarrier.DstAccessMask | MgAccessFlagBits.DEPTH_STENCIL_ATTACHMENT_WRITE_BIT;
			}

			// New layout is shader read (sampler, input attachment)
			// Make sure any writes to the image have been finished
			if (newImageLayout == MgImageLayout.SHADER_READ_ONLY_OPTIMAL)
			{
				imageMemoryBarrier.SrcAccessMask =  MgAccessFlagBits.HOST_WRITE_BIT | MgAccessFlagBits.TRANSFER_WRITE_BIT;
				imageMemoryBarrier.DstAccessMask = MgAccessFlagBits.SHADER_READ_BIT;
			}

			// Put barrier on top
			MgPipelineStageFlagBits srcStageFlags = MgPipelineStageFlagBits.TOP_OF_PIPE_BIT;
			MgPipelineStageFlagBits destStageFlags = MgPipelineStageFlagBits.TOP_OF_PIPE_BIT;

			// Put barrier inside setup command buffer
			cmdbuffer.CmdPipelineBarrier(
				srcStageFlags, 
				destStageFlags,
				0, 
				null,
				null,
				new []{imageMemoryBarrier});
		}
	}
}

