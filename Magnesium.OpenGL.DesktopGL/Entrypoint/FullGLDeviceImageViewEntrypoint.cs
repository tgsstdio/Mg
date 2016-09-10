using OpenTK.Graphics.OpenGL;
using System;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLDeviceImageViewEntrypoint : IGLDeviceImageViewEntrypoint
	{
		private static TextureTarget GetGLTextureTarget(MgImageViewType viewType)
		{
			switch (viewType)
			{
			case MgImageViewType.TYPE_1D:
				return TextureTarget.Texture1D;
			case MgImageViewType.TYPE_1D_ARRAY:
				return TextureTarget.Texture1DArray;
			case MgImageViewType.TYPE_2D:
				return TextureTarget.Texture2D;
			case MgImageViewType.TYPE_2D_ARRAY:
				return TextureTarget.Texture2DArray;
			case MgImageViewType.TYPE_3D:
				return TextureTarget.Texture3D;
			default:
				throw new NotSupportedException ();
			}
		}

		IGLErrorHandler mErrHandler;
		IGLImageFormatEntrypoint mImgFormat;

		public FullGLDeviceImageViewEntrypoint (IGLErrorHandler errHandler, IGLImageFormatEntrypoint imgFormat)
		{
			mErrHandler = errHandler;
			mImgFormat = imgFormat;
		}

		public void DeleteImageView (int texture)
		{
			int[] ids = new int[1];
			ids[0] = texture;
			GL.DeleteTextures(1, ids);
		}

		#region IGLImageViewFactory implementation
		public int CreateImageView (GLImage image, MgImageViewCreateInfo pCreateInfo)
		{
			var internalFormat = mImgFormat.GetGLFormat (pCreateInfo.Format, true);

			var textureTarget = GetGLTextureTarget (pCreateInfo.ViewType);

			int textureId = GL.GenTexture();
			mErrHandler.LogGLError ("GL.CreateTextures (AFTER)");

			GL.TextureView​(
				textureId,
				textureTarget,
				image.OriginalTextureId,
				(PixelInternalFormat) internalFormat.InternalFormat,
				(int) pCreateInfo.SubresourceRange.BaseMipLevel,
				(int) pCreateInfo.SubresourceRange.LevelCount,
				(int) pCreateInfo.SubresourceRange.BaseArrayLayer​,
				(int) pCreateInfo.SubresourceRange.LayerCount​);

			mErrHandler.LogGLError ("GL.TextureView (AFTER)");

			return textureId;
		}
		#endregion
		
	}
}

