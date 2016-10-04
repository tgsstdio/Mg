using System;
using System.Threading;
using CoreAnimation;
using Metal;
using MetalKit;

namespace Magnesium.Metal
{
	public class AmtPresentationLayer : IMgPresentationLayer
	{
		private readonly MTKView mView;

		public AmtPresentationLayer(MTKView view)
		{
			mView = view;

			mLayers = new AmtLayerInfo[1];
			mLayers[0] = new AmtLayerInfo
			{
				Inflight = new Semaphore(1, 1),
			};
		}

		private class AmtLayerInfo
		{
			public ICAMetalDrawable Drawable { get; set;}
			public Semaphore Inflight { get; set;}
		}

		private AmtLayerInfo[] mLayers;

		private ICAMetalDrawable mDrawable;
		private IMTLCommandBuffer mPresentation;
		public uint BeginDraw(IMgCommandBuffer postPresent, IMgSemaphore presentComplete)
		{
			var currentIndex = 0U;

			mLayers[currentIndex].Inflight.WaitOne();
			mLayers[currentIndex].Drawable = mView.CurrentDrawable;
			return currentIndex;
		}

		public uint BeginDraw(IMgCommandBuffer postPresent, IMgSemaphore presentComplete, ulong timeout)
		{
			var currentIndex = 0U;

			mLayers[currentIndex].Inflight.WaitOne();

			mLayers[currentIndex].Drawable = mView.CurrentDrawable;
			return currentIndex;
		}

		public void EndDraw(uint[] nextImage, IMgCommandBuffer prePresent, IMgSemaphore[] renderComplete)
		{

		}
	}
}
