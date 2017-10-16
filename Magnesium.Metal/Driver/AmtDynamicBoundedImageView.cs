using System;
using Metal;

namespace Magnesium.Metal.Internals
{
	class AmtDynamicBoundedImageView : IAmtImageView, IAmtSwapchainImageView
	{
		private bool mIsDisposed = false;
		public void DestroyImageView(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;


			mIsDisposed = true;
		}

		private IMTLTexture mTexture;
		public IMTLTexture GetTexture()
		{
			return mTexture;
		}

		public void SetTexture(IMTLTexture texture)
		{
			mTexture = texture;
		}
	}
}