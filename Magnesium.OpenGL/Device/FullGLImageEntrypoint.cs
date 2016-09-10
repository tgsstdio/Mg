using System;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLDeviceImageEntrypoint : IGLDeviceImageEntrypoint
	{
		public FullGLDeviceImageEntrypoint ()
		{
			
		}

		public void DeleteImage (int textureId)
		{
			int[] ids = new int[1];
			ids[0] = textureId;
			GL.DeleteTextures(1, ids);
		}

		static SizedInternalFormat GetInternalFormat(MgFormat format)
		{
			// FROM https://www.opengl.org/wiki/Image_Format
			//			"": No type suffix means unsigned normalized integer format.
			//			"_SNORM": Signed normalized integer format.
			//			"F": Floating-point. Thus, GL_RGBA32F is a floating-point format where 
			//				each component is a 32-bit IEEE floating-point value.
			//			"I": Signed integral format. Thus GL_RGBA8I gives a signed integer
			//				format where each of the four components is an integer on the range [-128, 127].
			//			"UI": Unsigned integral format. The values go from [0, MAX_INT] for the integer size.

			switch (format)
			{
			// 8bit MATCHING : GL_R8UI, GL_R8I, GL_R8_SNORM, GL_R8
			case MgFormat.R8_UINT:
				return SizedInternalFormat.R8ui;
			case MgFormat.R8_SINT:
				return SizedInternalFormat.R8i;
			case MgFormat.R8_SNORM:
				return (SizedInternalFormat)All.R8Snorm;
			case MgFormat.R8_UNORM:
				return (SizedInternalFormat)All.R8;

				// 16 bit MATCHING : GL_R16F, GL_RG8UI, GL_R16UI, GL_RG8I, GL_R16I, 
				// GL_RG8_SNORM, GL_R16_SNORM, GL_RG8, GL_R16
			case MgFormat.R16_SFLOAT:
				return SizedInternalFormat.R16f;
			case MgFormat.R8G8_UINT:
				return SizedInternalFormat.Rg8ui;
			case MgFormat.R16_UINT:
				return SizedInternalFormat.R16ui;
			case MgFormat.R8G8_SINT:
				return SizedInternalFormat.Rg8i;
			case MgFormat.R16_SINT:
				return SizedInternalFormat.R16i;
			case MgFormat.R8G8_SNORM:
				return (SizedInternalFormat)All.Rg8Snorm;
			case MgFormat.R16_SNORM:
				return (SizedInternalFormat)All.R16Snorm;
			case MgFormat.R8G8_UNORM:
				return (SizedInternalFormat)All.Rg8;
			case MgFormat.R16_UNORM:
				return (SizedInternalFormat)All.R16;

				// 24bit MATCHING : GL_RGB8, GL_RGB8_SNORM, GL_SRGB8, GL_RGB8UI, GL_RGB8I
			case MgFormat.R8G8B8_UNORM:
				return (SizedInternalFormat)All.Rgb8;
			case MgFormat.R8G8B8_SNORM:
				return (SizedInternalFormat)All.Rgb8Snorm;
			case MgFormat.R8G8B8_SRGB:
				return (SizedInternalFormat)All.Srgb8;
			case MgFormat.R8G8B8_UINT:
				return (SizedInternalFormat)All.Rgb8ui; 
			case MgFormat.R8G8B8_SINT:
				return (SizedInternalFormat)All.Rgb8i;

				// 32bit MATCHING : GL_RG16F, GL_R11F_G11F_B10F, GL_R32F, GL_RGB10_A2UI,
				// GL_RGBA8UI, GL_RG16UI, GL_R32UI, GL_RGBA8I
				// GL_RG16I, GL_R32I, GL_RGBA8, GL_RG16, GL_RGBA8_SNORM, 
				// GL_RG16_SNORM, GL_SRGB8_ALPHA8, GL_RGB9_E5
			case MgFormat.R16G16_SFLOAT:
				return SizedInternalFormat.Rg16f;
			case MgFormat.B10G11R11_UFLOAT_PACK32:
				return (SizedInternalFormat)All.R11fG11fB10f;
			case MgFormat.R32_SFLOAT:
				return SizedInternalFormat.R32f;
			case MgFormat.A2B10G10R10_UINT_PACK32:
				return (SizedInternalFormat)All.Rgb10A2ui;
			case MgFormat.R8G8B8A8_UINT:
				return SizedInternalFormat.Rgba8ui;
			case MgFormat.R16G16_UINT:
				return SizedInternalFormat.Rg16ui;
			case MgFormat.R32_UINT:
				return SizedInternalFormat.R32ui;
			case MgFormat.R8G8B8A8_SINT:
				return SizedInternalFormat.Rgba8i;
			case MgFormat.R16G16_SINT:
				return (SizedInternalFormat)All.Rg16i;
			case MgFormat.R32_SINT:
				return (SizedInternalFormat)All.R32i;
			case MgFormat.A2B10G10R10_UNORM_PACK32:
				return (SizedInternalFormat)All.Rgb10A2;
			case MgFormat.R8G8B8A8_UNORM:
				return (SizedInternalFormat)All.Rgba8;
			case MgFormat.R16G16_UNORM:
				return (SizedInternalFormat)All.Rg16;
			case MgFormat.R8G8B8A8_SNORM:
				return (SizedInternalFormat)All.Rgba8Snorm;
			case MgFormat.R16G16_SNORM:
				return (SizedInternalFormat)All.Rg16Snorm;
			case MgFormat.R8G8B8A8_SRGB:
				return (SizedInternalFormat)All.Srgb8Alpha8;
			case MgFormat.E5B9G9R9_UFLOAT_PACK32:
				return (SizedInternalFormat)All.Rgb9E5;

				// 48-bit
				// MATCHING : GL_RGB16, GL_RGB16_SNORM, GL_RGB16F, GL_RGB16UI, GL_RGB16I
			case MgFormat.R16G16B16_UNORM:
				return (SizedInternalFormat)All.Rgb16;
			case MgFormat.R16G16B16_SNORM:
				return (SizedInternalFormat)All.Rgb16Snorm;
			case MgFormat.R16G16B16_SFLOAT:
				return (SizedInternalFormat)All.Rgb16f;
			case MgFormat.R16G16B16_UINT:
				return (SizedInternalFormat)All.Rgb16ui;
			case MgFormat.R16G16B16_SINT:
				return (SizedInternalFormat)All.Rgb16i;

				// 64-bit
				// MATCHING : GL_RGBA16F, GL_RG32F, GL_RGBA16UI, GL_RG32UI, GL_RGBA16I, GL_RG32I
				// GL_RGBA16, GL_RGBA16_SNORM
			case MgFormat.R16G16B16A16_SFLOAT:
				return SizedInternalFormat.Rgba32f;
			case MgFormat.R32G32_SFLOAT:
				return SizedInternalFormat.Rg32f;
			case MgFormat.R16G16B16A16_UINT:
				return SizedInternalFormat.Rgba16ui;
			case MgFormat.R32G32_UINT:
				return SizedInternalFormat.Rg32ui;
			case MgFormat.R16G16B16A16_SINT:
				return SizedInternalFormat.Rgba16i;
			case MgFormat.R32G32_SINT:
				return SizedInternalFormat.Rg32i;
			case MgFormat.R16G16B16A16_UNORM:
				return SizedInternalFormat.Rgba16;
			case MgFormat.R16G16B16A16_SNORM:
				return (SizedInternalFormat)All.Rgba16Snorm;

				// 96-bit
				// MATCHING : GL_RGB32F, GL_RGB32UI, GL_RGB32I
			case MgFormat.R32G32B32_SFLOAT:
				return (SizedInternalFormat)All.Rgb32f;
			case MgFormat.R32G32B32_UINT:
				return (SizedInternalFormat)All.Rgb32ui;
			case MgFormat.R32G32B32_SINT:
				return (SizedInternalFormat)All.Rgb32i;

				// 128-bit
				// MATCHING : GL_RGBA32F, GL_RGBA32UI
				// NOT MATCHING : , , GL_RGBA32I
			case MgFormat.R32G32B32A32_SFLOAT:
				return SizedInternalFormat.Rgba32f;
			case MgFormat.R32G32B32A32_UINT:
				return SizedInternalFormat.Rgba32ui;
			case MgFormat.R32G32B32A32_SINT:
				return SizedInternalFormat.Rgba32i;

				//			GL_S3TC_DXT1_RGB	GL_COMPRESSED_RGB_S3TC_DXT1_EXT, GL_COMPRESSED_SRGB_S3TC_DXT1_EXT
			case MgFormat.BC1_RGB_UNORM_BLOCK:
				return (SizedInternalFormat)All.CompressedRgbS3tcDxt1Ext;
			case MgFormat.BC1_RGB_SRGB_BLOCK:
				return (SizedInternalFormat)All.CompressedSrgbS3tcDxt1Ext;

				//			GL_S3TC_DXT1_RGBA	GL_COMPRESSED_RGBA_S3TC_DXT1_EXT, GL_COMPRESSED_SRGB_ALPHA_S3TC_DXT1_EXT
			case MgFormat.BC1_RGBA_UNORM_BLOCK:
				return (SizedInternalFormat)All.CompressedRgbaS3tcDxt1Ext;
			case MgFormat.BC1_RGBA_SRGB_BLOCK:
				return (SizedInternalFormat)All.CompressedSrgbAlphaS3tcDxt1Ext;			

				//			GL_S3TC_DXT3_RGBA	GL_COMPRESSED_RGBA_S3TC_DXT3_EXT, GL_COMPRESSED_SRGB_ALPHA_S3TC_DXT3_EXT

			case MgFormat.BC2_UNORM_BLOCK:
				return (SizedInternalFormat)All.CompressedRgbaS3tcDxt3Ext;
			case MgFormat.BC2_SRGB_BLOCK:
				return (SizedInternalFormat)All.CompressedSrgbAlphaS3tcDxt3Ext;

				//			GL_S3TC_DXT5_RGBA	GL_COMPRESSED_RGBA_S3TC_DXT5_EXT, GL_COMPRESSED_SRGB_ALPHA_S3TC_DXT5_EXT

			case MgFormat.BC3_UNORM_BLOCK:
				return (SizedInternalFormat)All.CompressedRgbaS3tcDxt5Ext;
			case MgFormat.BC3_SRGB_BLOCK:
				return (SizedInternalFormat)All.CompressedSrgbAlphaS3tcDxt5Ext;

			default:
				throw new NotSupportedException ();
			}

		}

		#region IGLTextureModule implementation

		public int CreateTextureStorage1D (int levels, MgFormat format, int width)
		{
			var internalFormat = GetInternalFormat(format);

			int[] textureId = new int[1];
			GL.CreateTextures (TextureTarget.Texture1D, 1, textureId);
			GL.Ext.TextureStorage1D (textureId [0], (ExtDirectStateAccess)All.Texture1D, levels, internalFormat, width);
			return textureId [0];
		}

		public int CreateTextureStorage2D (int levels, MgFormat format, int width, int height)
		{
			var internalFormat = GetInternalFormat(format);

			int[] textureId = new int[1];
			GL.CreateTextures (TextureTarget.Texture2D, 1, textureId);
			GL.Ext.TextureStorage2D (textureId[0], (ExtDirectStateAccess)All.Texture2D, levels, internalFormat, width, height);
			return textureId [0];
		}

		public int CreateTextureStorage3D (int levels, MgFormat format, int width, int height, int depth)
		{
			var internalFormat = GetInternalFormat(format);

			int[] textureId = new int[1];
			GL.CreateTextures (TextureTarget.Texture3D, 1, textureId);
			GL.Ext.TextureStorage3D (textureId [0], (ExtDirectStateAccess)All.Texture3D, levels, internalFormat, width, height, depth);
			return textureId [0];
		}

		#endregion
	}
}

