using System;
using Metal;
using Foundation;

namespace Magnesium.Metal
{
	public class AmtImageView : IAmtImageView
	{
		private readonly IMTLTexture mImageView;
		public AmtImageView(MgImageViewCreateInfo pCreateInfo)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));


			if (pCreateInfo.Image == null)
				throw new ArgumentNullException(nameof(pCreateInfo.Image));

			var bImage = (AmtImage)pCreateInfo.Image;

			mImageView = bImage.OriginalTexture.CreateTextureView(
				AmtFormatExtensions.GetPixelFormat(pCreateInfo.Format),
				TranslateTextureType(pCreateInfo.ViewType),
				GenerateLevelRange(pCreateInfo.SubresourceRange),
				GenerateSliceRange(pCreateInfo.SubresourceRange));
		}

		public AmtImageView(IMTLTexture texture)
		{
			mImageView = texture;
		}

		MTLTextureType TranslateTextureType(MgImageViewType viewType)
		{
			switch (viewType)
			{
				default:
					throw new NotSupportedException();
				case MgImageViewType.TYPE_1D:
					return MTLTextureType.k1D;
				case MgImageViewType.TYPE_2D:
					return MTLTextureType.k2D;
				case MgImageViewType.TYPE_2D_ARRAY:
					return MTLTextureType.k2DArray;
				case MgImageViewType.TYPE_3D:
					return MTLTextureType.k3D;
				case MgImageViewType.TYPE_CUBE:
					return MTLTextureType.kCube;
				case MgImageViewType.TYPE_CUBE_ARRAY:
					return MTLTextureType.kCubeArray;
			}
		}

		static NSRange GenerateSliceRange(MgImageSubresourceRange subresourceRange)
		{
			return new NSRange
			{
				Location = (nint)subresourceRange.BaseArrayLayer,
				Length = (nint)subresourceRange.LayerCount,
			};
		}

		static NSRange GenerateLevelRange(MgImageSubresourceRange subresourceRange)
		{
			return new NSRange
			{
				Location = (nint) subresourceRange.BaseMipLevel,
				Length = (nint) subresourceRange.LevelCount,
			};
		}

		public void DestroyImageView(IMgDevice device, IMgAllocationCallbacks allocator)
		{
			
		}

		public IMTLTexture GetTexture()
		{
			return mImageView;
		}
	}
}