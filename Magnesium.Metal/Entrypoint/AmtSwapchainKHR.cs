using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using CoreAnimation;
using Metal;
using MetalKit;
namespace Magnesium.Metal
{
	public class AmtSwapchainKHR : IAmtSwapchainKHR
	{
		private readonly AmtSwapchainKHRImageInfo[] mImages;
		public AmtSwapchainKHRImageInfo[] Images
		{
			get
			{
				return mImages;
			}
		}
		private readonly MTKView mView;
		private readonly IAmtSwapchainImageView mColorView;

		private const int NO_OF_BUFFERS = 1;

		public AmtSwapchainKHR(MTKView view, IAmtSwapchainImageView color)
		{
			mImages = new AmtSwapchainKHRImageInfo[NO_OF_BUFFERS];
			for (var i = 0; i < NO_OF_BUFFERS; ++i)
			{
				mImages[i] = new AmtSwapchainKHRImageInfo
				{
					Drawable = null,
				};
				mImages[i].Inflight = new ManualResetEvent(false);
			}

			mView = view;
			mColorView = color;
		}

		private bool mIsDisposed = false;
		public void DestroySwapchainKHR(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;
			
			foreach (var info in mImages)
			{
				if (info.Drawable != null)
				{
					info.Drawable.Dispose();
					info.Drawable = null;
				}
			}

			mIsDisposed = true;
		}

		public bool AcquireNextImage(ulong timeout, out uint index)
		{
			Debug.Assert(!mIsDisposed);

			// TODO : make it variable
			var nextIndex = 0U;

			if (timeout == ulong.MaxValue)
			{
				mImages[nextIndex].Inflight.WaitOne();
			}
			else
			{
				var ticks = (long) timeout / 10000L;
				var timespan = TimeSpan.FromTicks(ticks);
				mImages[nextIndex].Inflight.WaitOne(timespan);
			}

			var drawable = mView.CurrentDrawable;
			if (drawable == null)
			{
				throw new InvalidOperationException("Swapchain failed");
			}

			mImages[nextIndex].Drawable = drawable;
			mImages[nextIndex].Inflight.Reset();
			index = nextIndex;
			mColorView.SetTexture(drawable.Texture);
			return true;
		}
	}
}
