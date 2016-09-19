using OpenTK.Graphics.OpenGL;
using System;

namespace Magnesium.OpenGL.DesktopGL
{
	public class DesktopGLImageFormatEntrypoint : IGLImageFormatEntrypoint
	{
		public GLInternalImageFormat GetGLFormat (MgFormat format, bool supportsSRgb)
		{
//			InternalFormat = (int) PixelInternalFormat.Rgba;
//			GLFormat = (int) PixelFormat.Rgba;
//			GLType = (int) PixelType.UnsignedByte;

			switch (format) {
			case MgFormat.R8G8B8_UINT:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) PixelInternalFormat.Rgb8ui,
					GLFormat = (int) PixelFormat.RgbInteger,
					GLType = (int) PixelType.UnsignedByte,
				};
			case MgFormat.R8G8B8A8_UINT:
				//case SurfaceFormat.Color:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) PixelInternalFormat.Rgba8ui,
					GLFormat = (int) PixelFormat.RgbaInteger,
					GLType = (int) PixelType.UnsignedByte,
				};
			case MgFormat.R8G8B8A8_SRGB:
				//case SurfaceFormat.ColorSRgb:
				if (!supportsSRgb)
					//goto case SurfaceFormat.Color;
					goto case MgFormat.R8G8B8A8_UINT;
				return new GLInternalImageFormat
				{
					InternalFormat = (int) PixelInternalFormat.Srgb, //(PixelInternalFormat) 0x8C40; // (int) PixelInternalFormat.Srgb;
					GLFormat = (int) PixelFormat.Rgba,
					GLType = (int) PixelType.UnsignedByte,
				};
			case MgFormat.B5G6R5_UNORM_PACK16:
				//case SurfaceFormat.Bgr565:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) PixelInternalFormat.Rgb,
					GLFormat = (int) PixelFormat.Rgb,
					GLType = (int) PixelType.UnsignedShort565,
				};

			case MgFormat.B4G4R4A4_UNORM_PACK16:
				//case SurfaceFormat.Bgra4444:
				return new GLInternalImageFormat
				{
					#if IOS || ANDROID
					InternalFormat = (int) PixelInternalFormat.Rgba,
					#else
					InternalFormat = (int) PixelInternalFormat.Rgba4,
					#endif
					GLFormat = (int) PixelFormat.Rgba,
					GLType = (int) PixelType.UnsignedShort4444,
				};
			case MgFormat.B5G5R5A1_UNORM_PACK16:
				return new GLInternalImageFormat
				{				
					//case SurfaceFormat.Bgra5551:
					InternalFormat = (int) PixelInternalFormat.Rgba,
					GLFormat = (int) PixelFormat.Rgba,
					GLType = (int) PixelType.UnsignedShort5551,
				};
			case MgFormat.R8_UINT:
				return new GLInternalImageFormat
				{						
					//case SurfaceFormat.Alpha8:
					InternalFormat = (int) PixelInternalFormat.Luminance,
					GLFormat = (int) PixelFormat.Luminance,
					GLType = (int) PixelType.UnsignedByte,
				};
			
			#if !IOS && !ANDROID && !ANGLE
			case MgFormat.BC1_RGB_UNORM_BLOCK:
				return new GLInternalImageFormat
				{				
					//case SurfaceFormat.Dxt1:
					InternalFormat = (int) PixelInternalFormat.CompressedRgbS3tcDxt1Ext,
					GLFormat = (int)All.CompressedTextureFormats,
					GLType = (int) PixelType.UnsignedByte, // DEFAULT
				};
			case MgFormat.BC1_RGB_SRGB_BLOCK:
				// case SurfaceFormat.Dxt1SRgb:
				if (!supportsSRgb)
					//goto case SurfaceFormat.Dxt1;
					goto case MgFormat.BC1_RGB_SRGB_BLOCK;
				return new GLInternalImageFormat
				{				
					InternalFormat = (int) PixelInternalFormat.CompressedSrgbS3tcDxt1Ext,
					GLFormat = (int) All.CompressedTextureFormats,
					GLType = (int) PixelType.UnsignedByte, // DEFAULT
				};
			case MgFormat.BC1_RGBA_UNORM_BLOCK:
				//case SurfaceFormat.Dxt1a:
				return new GLInternalImageFormat
				{	
					InternalFormat = (int) PixelInternalFormat.CompressedRgbaS3tcDxt1Ext,
					GLFormat = (int)All.CompressedTextureFormats,
					GLType = (int) PixelType.UnsignedByte, // DEFAULT
				};
			case MgFormat.BC2_UNORM_BLOCK:
				//case SurfaceFormat.Dxt3:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) PixelInternalFormat.CompressedRgbaS3tcDxt3Ext,
					GLFormat = (int)All.CompressedTextureFormats,
					GLType = (int) PixelType.UnsignedByte, // DEFAULT
				};
			case MgFormat.BC2_SRGB_BLOCK:
				//case SurfaceFormat.Dxt3SRgb:
				if (!supportsSRgb)
					goto case MgFormat.BC2_UNORM_BLOCK;
				//	goto case SurfaceFormat.Dxt3;
				return new GLInternalImageFormat
				{
					InternalFormat = (int) PixelInternalFormat.CompressedSrgbAlphaS3tcDxt3Ext,
					GLFormat = (int) All.CompressedTextureFormats,
					GLType = (int) PixelType.UnsignedByte, // DEFAULT
				};

			case MgFormat.BC3_UNORM_BLOCK:
				//case SurfaceFormat.Dxt5:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) PixelInternalFormat.CompressedRgbaS3tcDxt5Ext,
					GLFormat = (int) All.CompressedTextureFormats,
					GLType = (int) PixelType.UnsignedByte, // DEFAULT
				};
			case MgFormat.BC3_SRGB_BLOCK:
				//case SurfaceFormat.Dxt5SRgb:
				if (!supportsSRgb)
					goto case MgFormat.BC3_UNORM_BLOCK;
				//goto case SurfaceFormat.Dxt5;
				return new GLInternalImageFormat
				{
					InternalFormat = (int) PixelInternalFormat.CompressedSrgbAlphaS3tcDxt5Ext,
					GLFormat = (int) All.CompressedTextureFormats,
					GLType = (int) PixelType.UnsignedByte, // DEFAULT
				};
			case MgFormat.R32_SFLOAT:
				// case SurfaceFormat.Single:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) PixelInternalFormat.R32f,
					GLFormat = (int) PixelFormat.Red,
					GLType = (int) PixelType.Float,
				};
			case MgFormat.R16G16_SFLOAT:
				// case SurfaceFormat.HalfVector2:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) PixelInternalFormat.Rg16f,
					GLFormat = (int) PixelFormat.Rg,
					GLType = (int) PixelType.HalfFloat,
				};
				// HdrBlendable implemented as HalfVector4 (see http://blogs.msdn.com/b/shawnhar/archive/2010/07/09/surfaceformat-hdrblendable.aspx)			
			case MgFormat.R16G16B16A16_SFLOAT:
				//case SurfaceFormat.HdrBlendable:
				//case SurfaceFormat.HalfVector4:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) PixelInternalFormat.Rgba16f,
					GLFormat = (int) PixelFormat.Rgba,
					GLType = (int) PixelType.HalfFloat,
				};
			case MgFormat.R16_SFLOAT:
				//case SurfaceFormat.HalfSingle:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) PixelInternalFormat.R16f,
					GLFormat = (int) PixelFormat.Red,
					GLType = (int) PixelType.HalfFloat,
				};
			case MgFormat.R32G32_SFLOAT:
				//case SurfaceFormat.Vector2:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) PixelInternalFormat.Rg32f,
					GLFormat = (int) PixelFormat.Rg,
					GLType = (int) PixelType.Float,
				};
			case MgFormat.R32G32B32A32_SFLOAT:
				//case SurfaceFormat.Vector4:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) PixelInternalFormat.Rgba32f,
					GLFormat = (int) PixelFormat.Rgba,
					GLType = (int) PixelType.Float,
				};
			case MgFormat.R8G8_SNORM:
				//case SurfaceFormat.NormalizedByte2:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) PixelInternalFormat.Rg8i,
					GLFormat = (int) PixelFormat.Rg,
					GLType = (int) PixelType.Byte,
				};
			case MgFormat.R8G8B8A8_SNORM:
				//case SurfaceFormat.NormalizedByte4:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) PixelInternalFormat.Rgba8i,
					GLFormat = (int) PixelFormat.Rgba,
					GLType = (int) PixelType.Byte,
				};
			case MgFormat.R16G16_UINT:
				//case SurfaceFormat.Rg32:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) PixelInternalFormat.Rg16ui,
					GLFormat = (int) PixelFormat.RgInteger,
					GLType = (int) PixelType.UnsignedShort,
				};
			case MgFormat.R16G16B16A16_UINT:
				//case SurfaceFormat.Rgba64:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) PixelInternalFormat.Rgba16ui,
					GLFormat = (int) PixelFormat.RgbaInteger,
					GLType = (int) PixelType.UnsignedShort,
				};
			case MgFormat.A2B10G10R10_UINT_PACK32:
				//case SurfaceFormat.Rgba1010102:
				return new GLInternalImageFormat
				{
					InternalFormat = (int) PixelInternalFormat.Rgb10A2ui,
					GLFormat = (int) PixelFormat.Rgba,
					GLType = (int) PixelType.UnsignedInt1010102,
				};
			#endif

//			#if !(IOS || ANDROID)
//				// 				TODO : find compatible VKFormat
//				case SurfaceFormat.RgbPvrtc2Bpp:
//					return new GLImageFormat
//					{
//						InternalFormat = (PixelInternalFormat)All.CompressedRgbPvrtc2Bppv1Img,
//						GLFormat = (int) All.CompressedTextureFormats,
////						GLType = PixelType.UnsignedByte, // DEFAULT
//					};
//				case SurfaceFormat.RgbPvrtc4Bpp:
//				return new GLImageFormat
//				{
//					InternalFormat = (PixelInternalFormat)All.CompressedRgbPvrtc4Bppv1Img,
//					GLFormat = (int) All.CompressedTextureFormats,
////						GLType = PixelType.UnsignedByte, // DEFAULT
//				};
//				break;
//				case SurfaceFormat.RgbaPvrtc2Bpp:
//				return new GLImageFormat
//				{				
//					InternalFormat = (PixelInternalFormat)All.CompressedRgbaPvrtc2Bppv1Img,
//					GLFormat = (int) All.CompressedTextureFormats,
////						GLType = PixelType.UnsignedByte, // DEFAULT
//				};
//				case SurfaceFormat.RgbaPvrtc4Bpp:
//				return new GLImageFormat
//				{				
//					InternalFormat = (PixelInternalFormat)All.CompressedRgbaPvrtc4Bppv1Img,
//					GLFormat = (int) All.CompressedTextureFormats,
////						GLType = PixelType.UnsignedByte, // DEFAULT
//				};
//			#endif
			default:
				throw new NotSupportedException();
			}
		}
	}
}

