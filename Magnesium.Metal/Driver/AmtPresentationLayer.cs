using System;
using System.Collections.Generic;
using System.Threading;
using CoreAnimation;
using Metal;
using MetalKit;

namespace Magnesium.Metal
{
	public class AmtPresentationLayer : IMgPresentationLayer
	{
		private readonly MTKView mView;
		private IMgSwapchainCollection mCollection;

		public AmtPresentationLayer(MTKView view, IMgSwapchainCollection swapchainCollection)
		{
			mView = view;

			mLayers = new AmtLayerInfo[1];
			mLayers[0] = new AmtLayerInfo
			{
				Inflight = new Semaphore(1, 1),
			};
			mCollection = swapchainCollection; 
		}

		private class AmtLayerInfo
		{
			public ICAMetalDrawable Drawable { get; set;}
			public Semaphore Inflight { get; set;}
		}

		private readonly AmtLayerInfo[] mLayers;

		public uint BeginDraw(IMgCommandBuffer postPresent, IMgSemaphore presentComplete)
		{
			var currentIndex = 0U;

			mLayers[currentIndex].Inflight.WaitOne();
			if (mLayers[currentIndex].Drawable != null)
			{
				mLayers[currentIndex].Drawable.Dispose();
			}

			mLayers[currentIndex].Drawable = mView.CurrentDrawable;
			return currentIndex;
		}

		public uint BeginDraw(IMgCommandBuffer postPresent, IMgSemaphore presentComplete, ulong timeout)
		{
			var currentIndex = 0U;

			mLayers[currentIndex].Inflight.WaitOne();
			if (mLayers[currentIndex].Drawable != null)
			{
				mLayers[currentIndex].Drawable.Dispose();
			}
			mLayers[currentIndex].Drawable = mView.CurrentDrawable;
			return currentIndex;
		}

		public void EndDraw(uint[] nextImage, IMgCommandBuffer prePresent, IMgSemaphore[] renderComplete)
		{
			Result err;

			var presentImages = new List<MgPresentInfoKHRImage>();
			foreach (var image in nextImage)
			{
				var currentBuffer = mLayers[image];
				//submitPrePresentBarrier(prePresent, currentBuffer.Image);

				presentImages.Add(new MgPresentInfoKHRImage
				{
					ImageIndex = image,
					Swapchain = mCollection.Swapchain,
				});
			}

			var presentInfo = new MgPresentInfoKHR
			{
				WaitSemaphores = renderComplete,
				Images = presentImages.ToArray(),
			};

			//err = swapChain.queuePresent(queue, currentBuffer, semaphores.renderComplete);
			err = mPartition.Queue.QueuePresentKHR(presentInfo);
			Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
			err = mPartition.Queue.QueueWaitIdle();
			Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
		}
	}
}
