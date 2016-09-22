using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Magnesium
{
    public class MgPresentationLayer : IMgPresentationLayer
	{
		public MgPresentationLayer (IMgThreadPartition partition, IMgSwapchainCollection collection)
		{
			mPartition = partition;
			mCollection = collection;
		}

		private readonly IMgThreadPartition mPartition;			
		private readonly IMgSwapchainCollection mCollection;

		//public IMgCommandBuffer PostPresent { get; set; }

		//public IMgCommandBuffer PrePresent { get; set; }

		uint AcquireNextImage (IMgSemaphore presentComplete, ulong timeout)
		{
			uint nextImage;
			Result err = mPartition.Device.AcquireNextImageKHR (mCollection.Swapchain, timeout, presentComplete, null, out nextImage);
			Debug.Assert (err == Result.SUCCESS);
			return nextImage;
		}

		public void EndDraw (uint[] nextImage, IMgCommandBuffer prePresent, IMgSemaphore[] renderComplete)
		{
			Result err;

			var presentImages = new List<MgPresentInfoKHRImage>();
			foreach (var image in nextImage)
			{
				var currentBuffer = mCollection.Buffers[image];
				submitPrePresentBarrier(prePresent, currentBuffer.Image);

				presentImages.Add(new MgPresentInfoKHRImage
				{
					ImageIndex = image,
					Swapchain = mCollection.Swapchain,
				});
			}

			var presentInfo = new MgPresentInfoKHR {
				WaitSemaphores = renderComplete,
				Images = presentImages.ToArray(),
			};

			//err = swapChain.queuePresent(queue, currentBuffer, semaphores.renderComplete);
			err = mPartition.Queue.QueuePresentKHR (presentInfo);
			Debug.Assert (err == Result.SUCCESS, err + " != Result.SUCCESS");
			err = mPartition.Queue.QueueWaitIdle ();
			Debug.Assert (err == Result.SUCCESS, err + " != Result.SUCCESS");
        }

		public uint BeginDraw (IMgCommandBuffer postPresent, IMgSemaphore presentComplete, ulong timeout)
		{
			// Get next image in the swap chain (back/front buffer)
			//err = swapChain.acquireNextImage(semaphores.presentComplete, &currentBuffer);
			var nextImage = AcquireNextImage (presentComplete, timeout);
			var currentBuffer = mCollection.Buffers [nextImage];
			submitPostPresentBarrier (postPresent, currentBuffer.Image);
			return nextImage;
		}

		public uint BeginDraw (IMgCommandBuffer postPresent, IMgSemaphore presentComplete)
		{
			return BeginDraw (postPresent, presentComplete, ulong.MaxValue);
		}

		void submitPostPresentBarrier(IMgCommandBuffer postPresent, IMgImage image)
		{
			if (postPresent == null)
				throw new InvalidOperationException ();

			MgCommandBufferBeginInfo cmdBufInfo = new MgCommandBufferBeginInfo {

			};

			var vkRes = postPresent.BeginCommandBuffer(cmdBufInfo);
			Debug.Assert(vkRes == Result.SUCCESS, vkRes + " != Result.SUCCESS");

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
				new [] {postPresentBarrier});

			vkRes = postPresent.EndCommandBuffer();
			Debug.Assert(vkRes == Result.SUCCESS, vkRes + " != Result.SUCCESS");

            var submitInfo = new [] {
				new MgSubmitInfo
				{
					CommandBuffers = new []
					{
						postPresent
					},
				}
			};

			vkRes = mPartition.Queue.QueueSubmit(submitInfo, null);
			Debug.Assert(vkRes == Result.SUCCESS, vkRes + " != Result.SUCCESS");
        }

		void submitPrePresentBarrier(IMgCommandBuffer prePresent, IMgImage image)
		{
			if (prePresent == null)
				throw new InvalidOperationException ();

			MgCommandBufferBeginInfo cmdBufInfo = new MgCommandBufferBeginInfo {

			};

			var vkRes = prePresent.BeginCommandBuffer(cmdBufInfo);
			Debug.Assert(vkRes == Result.SUCCESS, vkRes + " != Result.SUCCESS");

            const uint VK_QUEUE_FAMILY_IGNORED = ~0U;

			var prePresentBarrier = new MgImageMemoryBarrier {
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
				new []{ prePresentBarrier} );

			vkRes = prePresent.EndCommandBuffer();
			Debug.Assert(vkRes == Result.SUCCESS, vkRes + " != Result.SUCCESS");

            var submitInfo = new []
			{
				new MgSubmitInfo
				{
					CommandBuffers = new []{prePresent}
				}
			};

			vkRes = mPartition.Queue.QueueSubmit(submitInfo, null);
			Debug.Assert(vkRes == Result.SUCCESS, vkRes + " != Result.SUCCESS");
        }
	}
}

