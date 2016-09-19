using System;
using Magnesium.OpenGL;
using OpenTK.Graphics.ES20;

namespace Magnesium.OpenGL.AndroidGL
{
	public class Es20GLImageFormatEntrypoint : IGLImageFormatEntrypoint
	{
		public GLInternalImageFormat GetGLFormat (MgFormat format, bool supportsSRgb)
		{
			//			InternalFormat = PixelInternalFormat.Rgba;
			//			GLFormat = PixelFormat.Rgba;
			//			GLType = PixelType.UnsignedByte;

			switch (format) {
			case MgFormat.R8G8B8_UINT:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) All.Rgb8Oes, 
					GLFormat = (int) PixelFormat.Rgb, // TODO: double check
					GLType = (int) All.UnsignedByte,
				};
			case MgFormat.R8G8B8A8_UINT:
				//case SurfaceFormat.Color:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) All.Rgba8Oes,
					GLFormat = (int) PixelFormat.Rgba,
					GLType = (int) All.UnsignedByte,
				};
			case MgFormat.R8G8B8A8_SRGB:
				//case SurfaceFormat.ColorSRgb:
				if (!supportsSRgb)
					//goto case SurfaceFormat.Color;
					goto case MgFormat.R8G8B8A8_UINT;
				return new GLInternalImageFormat
				{
					InternalFormat = (int) 0x8C40, // PixelInternalFormat.Srgb;
					GLFormat = (int) All.Rgba,
					GLType = (int) All.UnsignedByte,
				};
			case MgFormat.B5G6R5_UNORM_PACK16:
				//case SurfaceFormat.Bgr565:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) All.Rgb,
					GLFormat = (int) All.Rgb,
					GLType = (int) All.UnsignedShort565,
				};

			case MgFormat.B4G4R4A4_UNORM_PACK16:
				//case SurfaceFormat.Bgra4444:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) All.Rgba,
					GLFormat = (int) All.Rgba,
					GLType = (int) All.UnsignedShort4444,
				};
			case MgFormat.B5G5R5A1_UNORM_PACK16:
				return new GLInternalImageFormat
				{				
					//case SurfaceFormat.Bgra5551:
					InternalFormat = (int) All.Rgba,
					GLFormat = (int) All.Rgba,
					GLType = (int) All.UnsignedShort5551,
				};
			case MgFormat.R8_UINT:
				return new GLInternalImageFormat
				{						
					//case SurfaceFormat.Alpha8:
					InternalFormat = (int) All.Luminance,
					GLFormat = (int) All.Luminance,
					GLType = (int) All.UnsignedByte,
				};

				case MgFormat.BC1_RGB_UNORM_BLOCK:
				//case SurfaceFormat.Dxt1:
				// 0x83F0 is the RGB version, 0x83F1 is the RGBA version (1-bit alpha)
				// XNA uses the RGB version.
				return new GLInternalImageFormat
				{
					InternalFormat = 0x83F0,
					GLFormat = (int) All.CompressedTextureFormats,
					GLType = (int) All.UnsignedByte, // DEFAULT
				};
				case MgFormat.BC1_RGB_SRGB_BLOCK:
				//case SurfaceFormat.Dxt1SRgb:
				if (!supportsSRgb)
				goto case MgFormat.BC1_RGB_UNORM_BLOCK;
				//goto case SurfaceFormat.Dxt1;
				return new GLInternalImageFormat
				{
					InternalFormat = 0x8C4C,
					GLFormat = (int) All.CompressedTextureFormats,
					GLType = (int) All.UnsignedByte, // DEFAULT
				};
				case MgFormat.BC1_RGBA_UNORM_BLOCK:
				//case SurfaceFormat.Dxt1a:
				// 0x83F0 is the RGB version, 0x83F1 is the RGBA version (1-bit alpha)
				return new GLInternalImageFormat
				{
					InternalFormat = 0x83F1,
					GLFormat = (int) All.CompressedTextureFormats,
					GLType = (int) All.UnsignedByte, // DEFAULT
				};
				case MgFormat.BC2_UNORM_BLOCK:
				// case SurfaceFormat.Dxt3:
				return new GLInternalImageFormat
				{
					InternalFormat = 0x83F2,
					GLFormat = (int) All.CompressedTextureFormats,
					GLType = (int) All.UnsignedByte, // DEFAULT
				};
				case MgFormat.BC2_SRGB_BLOCK:
				//case SurfaceFormat.Dxt3SRgb:
				if (!supportsSRgb)
				//goto case SurfaceFormat.Dxt3;
				goto case MgFormat.BC2_UNORM_BLOCK;

				return new GLInternalImageFormat
				{				
					InternalFormat = 0x8C4E,
					GLFormat = (int) All.CompressedTextureFormats,
					GLType = (int) All.UnsignedByte, // DEFAULT
				};			
				case MgFormat.BC3_UNORM_BLOCK:
				//case SurfaceFormat.Dxt5:
				return new GLInternalImageFormat
				{
					InternalFormat = 0x83F3,
					GLFormat = (int) All.CompressedTextureFormats,
					GLType = (int) All.UnsignedByte, // DEFAULT
				};
				case MgFormat.BC3_SRGB_BLOCK:
				//case SurfaceFormat.Dxt5SRgb:
				if (!supportsSRgb)						
				//goto case SurfaceFormat.Dxt5;
				goto case MgFormat.BC3_UNORM_BLOCK;

				return new GLInternalImageFormat
				{				
					InternalFormat = 0x8C4F,
					GLFormat = (int) All.CompressedTextureFormats,
					GLType = (int) All.UnsignedByte, // DEFAULT
				};
				// 				TODO : find compatible VKFormat
				//				case SurfaceFormat.RgbaAtcExplicitAlpha:
				//					return new GLImageFormat
				//					{				
				//						InternalFormat = (PixelInternalFormat)(int) All.AtcRgbaExplicitAlphaAmd,
				//						GLFormat = (PixelFormat)(int) All.CompressedTextureFormats,
				//						GLType = PixelType.UnsignedByte, // DEFAUL
				//					};
				//				case SurfaceFormat.RgbaAtcInterpolatedAlpha:
				//					return new GLImageFormat
				//					{				
				//						InternalFormat = (PixelInternalFormat)(int) All.AtcRgbaInterpolatedAlphaAmd,
				//						GLFormat = (PixelFormat)(int) All.CompressedTextureFormats,
				//						GLType = PixelType.UnsignedByte, // DEFAULT
				//					};
				//
				//				case SurfaceFormat.RgbEtc1:
				//					return new GLImageFormat
				//					{
				//						InternalFormat = (PixelInternalFormat)0x8D64, // GL_ETC1_RGB8_OES
				//						GLFormat = (PixelFormat)(int) All.CompressedTextureFormats,
				//						GLType = PixelType.UnsignedByte, // DEFAULT
				//					};

//				#if IOS || ANDROID
//				// 				TODO : find compatible VKFormat
//			case SurfaceFormat.RgbPvrtc2Bpp:
//				return new GLImageFormat
//				{
//					InternalFormat = (PixelInternalFormat)(int) All.CompressedRgbPvrtc2Bppv1Img,
//					GLFormat = (PixelFormat)(int) All.CompressedTextureFormats,
//					//						GLType = PixelType.UnsignedByte, // DEFAULT
//				};
//			case SurfaceFormat.RgbPvrtc4Bpp:
//				return new GLImageFormat
//				{
//					InternalFormat = (PixelInternalFormat)(int) All.CompressedRgbPvrtc4Bppv1Img,
//					GLFormat = (PixelFormat)(int) All.CompressedTextureFormats,
//					//						GLType = PixelType.UnsignedByte, // DEFAULT
//				};
//				break;
//			case SurfaceFormat.RgbaPvrtc2Bpp:
//				return new GLImageFormat
//				{				
//					InternalFormat = (PixelInternalFormat)(int) All.CompressedRgbaPvrtc2Bppv1Img,
//					GLFormat = (PixelFormat)(int) All.CompressedTextureFormats,
//					//						GLType = PixelType.UnsignedByte, // DEFAULT
//				};
//			case SurfaceFormat.RgbaPvrtc4Bpp:
//				return new GLImageFormat
//				{				
//					InternalFormat = (PixelInternalFormat)(int) All.CompressedRgbaPvrtc4Bppv1Img,
//					GLFormat = (PixelFormat)(int) All.CompressedTextureFormats,
//					//						GLType = PixelType.UnsignedByte, // DEFAULT
//				};
//				#endif
			default:
				throw new NotSupportedException();
			}
		}
	}
}

