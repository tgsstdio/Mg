using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Magnesium
{
    public class MgPresentationLayer : IMgPresentationLayer
	{
		public MgPresentationLayer 
		(
			IMgGraphicsConfiguration graphicsConfiguration, 
			IMgSwapchainCollection collection,
			IMgPresentationBarrierEntrypoint barrier
		)
		{
			mGraphicsConfiguration = graphicsConfiguration;
			mCollection = collection;
			mBarrier = barrier;
		}

		private readonly IMgGraphicsConfiguration mGraphicsConfiguration;
		private readonly IMgPresentationBarrierEntrypoint mBarrier;
		private readonly IMgSwapchainCollection mCollection;

		//public IMgCommandBuffer PostPresent { get; set; }

		//public IMgCommandBuffer PrePresent { get; set; }

		uint AcquireNextImage (IMgSemaphore presentComplete, ulong timeout)
		{
			uint nextImage;
			Result err = mGraphicsConfiguration.Device.AcquireNextImageKHR (mCollection.Swapchain, timeout, presentComplete, null, out nextImage);
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
				mBarrier.SubmitPrePresentBarrier(prePresent, currentBuffer.Image);

				var submitInfo = new[]
				{
					new MgSubmitInfo
					{
						CommandBuffers = new []{prePresent}
					}
				};

				var result = mGraphicsConfiguration.Queue.QueueSubmit(submitInfo, null);
				Debug.Assert(result == Result.SUCCESS, result + " != Result.SUCCESS");

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
			err = mGraphicsConfiguration.Queue.QueuePresentKHR (presentInfo);
			Debug.Assert (err == Result.SUCCESS, err + " != Result.SUCCESS");
			err = mGraphicsConfiguration.Queue.QueueWaitIdle ();
			Debug.Assert (err == Result.SUCCESS, err + " != Result.SUCCESS");
        }

		public uint BeginDraw (IMgCommandBuffer postPresent, IMgSemaphore presentComplete, ulong timeout)
		{
			// Get next image in the swap chain (back/front buffer)
			//err = swapChain.acquireNextImage(semaphores.presentComplete, &currentBuffer);
			var nextImage = AcquireNextImage (presentComplete, timeout);
			var currentBuffer = mCollection.Buffers [nextImage];

			mBarrier.SubmitPostPresentBarrier (postPresent, currentBuffer.Image);
			var submitInfo = new[] {
				new MgSubmitInfo
				{
					CommandBuffers = new []
					{
						postPresent
					},
				}
			};

			var result = mGraphicsConfiguration.Queue.QueueSubmit(submitInfo, null);
			Debug.Assert(result == Result.SUCCESS, result + " != Result.SUCCESS");

			return nextImage;
		}

		public uint BeginDraw (IMgCommandBuffer postPresent, IMgSemaphore presentComplete)
		{
			return BeginDraw (postPresent, presentComplete, ulong.MaxValue);
		}

	}
}

