using System;
using Metal;
using Foundation;

namespace Magnesium.Metal
{
	public class AmtImageView : IMgImageView
	{
		public IMTLTexture Image { get; private set;}
		public AmtImageView(MgImageViewCreateInfo pCreateInfo)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));


			if (pCreateInfo.Image == null)
				throw new ArgumentNullException(nameof(pCreateInfo.Image));

			var bImage = (AmtImage)pCreateInfo.Image;

			Image = bImage.OriginalTexture.CreateTextureView(
				AmtFormatExtensions.GetPixelFormat(pCreateInfo.Format),
				TranslateTextureType(pCreateInfo.ViewType),
				GenerateLevelRange(pCreateInfo.SubresourceRange),
				GenerateSliceRange(this, pCreateInfo.SubresourceRange));
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

		static NSRange GenerateSliceRange(AmtImageView instance, MgImageSubresourceRange subresourceRange)
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
	}
}