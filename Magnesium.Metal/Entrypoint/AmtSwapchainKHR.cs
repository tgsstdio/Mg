using System;
using System.Diagnostics;
using System.Threading;
using MetalKit;
namespace Magnesium.Metal
{
	public class AmtSwapchainKHR : IMgSwapchainKHR
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
				mImages[i].Inflight = new ManualResetEvent(true);
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
				if (info.Inflight != null)
				{
					info.Inflight.Dispose();
					info.Inflight = null;
				}
			}

			mIsDisposed = true;
		}

		internal uint GetAvailableImageIndex()
		{
			return 0;
		}

		public void RefreshImageView(uint index)
		{
			Debug.Assert(!mIsDisposed);

			var drawable = mView.CurrentDrawable;
			if (drawable == null)
			{
				throw new InvalidOperationException("METAL : Swapchain failed");
			}

			mImages[index].Drawable = drawable;
			mImages[index].Inflight.Reset();
			mColorView.SetTexture(drawable.Texture);
		}
	}
}
