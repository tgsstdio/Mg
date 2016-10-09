using System;
using Metal;

namespace Magnesium.Metal
{
	internal static class AmtFormatExtensions
	{
		public static MTLVertexFormat GetVertexFormat(MgFormat format)
		{
			switch (format)
			{
				default:
					throw new NotSupportedException();
				case MgFormat.UNDEFINED:
					return MTLVertexFormat.Invalid;
				case MgFormat.R8G8_UINT:
					return MTLVertexFormat.UChar2;
				case MgFormat.R8G8B8_UINT:
				case MgFormat.B8G8R8_UINT:
					return MTLVertexFormat.UChar3;
				case MgFormat.R8G8B8A8_UINT:
				case MgFormat.B8G8R8A8_UINT:
					return MTLVertexFormat.UChar4;
				case MgFormat.R8G8_SINT:
					return MTLVertexFormat.Char2;
				case MgFormat.R8G8B8_SINT:
				case MgFormat.B8G8R8_SINT:
					return MTLVertexFormat.Char3;
				case MgFormat.R8G8B8A8_SINT:
				case MgFormat.B8G8R8A8_SINT:
					return MTLVertexFormat.Char4;
				case MgFormat.R8G8_UNORM:
					return MTLVertexFormat.UChar2Normalized;
				case MgFormat.R8G8B8_UNORM:
				case MgFormat.B8G8R8_UNORM:
					return MTLVertexFormat.UChar3Normalized;
				case MgFormat.R8G8B8A8_UNORM:
				case MgFormat.B8G8R8A8_UNORM:
					return MTLVertexFormat.UChar4Normalized;
				case MgFormat.R8G8_SNORM:
					return MTLVertexFormat.Char2Normalized;
				case MgFormat.R8G8B8_SNORM:
				case MgFormat.B8G8R8_SNORM:
					return MTLVertexFormat.Char3Normalized;
				case MgFormat.R8G8B8A8_SNORM:
				case MgFormat.B8G8R8A8_SNORM:
					return MTLVertexFormat.Char4Normalized;
				case MgFormat.R16G16_UINT:
					return MTLVertexFormat.UShort2;
				case MgFormat.R16G16B16_UINT:
					return MTLVertexFormat.UShort3;
				case MgFormat.R16G16B16A16_UINT:
					return MTLVertexFormat.UShort4;
				case MgFormat.R16G16_SINT:
					return MTLVertexFormat.Short2;
				case MgFormat.R16G16B16_SINT:
					return MTLVertexFormat.Short3;
				case MgFormat.R16G16B16A16_SINT:
					return MTLVertexFormat.Short4;
				case MgFormat.R16G16_UNORM:
					return MTLVertexFormat.UShort2Normalized;
				case MgFormat.R16G16B16_UNORM:
					return MTLVertexFormat.UShort3Normalized;
				case MgFormat.R16G16B16A16_UNORM:
					return MTLVertexFormat.UShort4Normalized;
				case MgFormat.R16G16_SNORM:
					return MTLVertexFormat.Short2Normalized;
				case MgFormat.R16G16B16_SNORM:
					return MTLVertexFormat.Short3Normalized;
				case MgFormat.R16G16B16A16_SNORM:
					return MTLVertexFormat.Short4Normalized;
				case MgFormat.R16G16_SFLOAT:
					return MTLVertexFormat.Half2;
				case MgFormat.R16G16B16_SFLOAT:
					return MTLVertexFormat.Half3;
				case MgFormat.R16G16B16A16_SFLOAT:
					return MTLVertexFormat.Half4;
				case MgFormat.R32_SFLOAT:
					return MTLVertexFormat.Float;
				case MgFormat.R32G32_SFLOAT:
					return MTLVertexFormat.Float2;
				case MgFormat.R32G32B32_SFLOAT:
					return MTLVertexFormat.Float3;
				case MgFormat.R32G32B32A32_SFLOAT:
					return MTLVertexFormat.Float4;
				case MgFormat.R32_SINT:
					return MTLVertexFormat.Int;
				case MgFormat.R32G32_SINT:
					return MTLVertexFormat.Int2;
				case MgFormat.R32G32B32_SINT:
					return MTLVertexFormat.Int3;
				case MgFormat.R32G32B32A32_SINT:
					return MTLVertexFormat.Int4;
				case MgFormat.R32_UINT:
					return MTLVertexFormat.UInt;
				case MgFormat.R32G32_UINT:
					return MTLVertexFormat.UInt2;
				case MgFormat.R32G32B32_UINT:
					return MTLVertexFormat.UInt3;
				case MgFormat.R32G32B32A32_UINT:
					return MTLVertexFormat.UInt4;
				case MgFormat.A2R10G10B10_SNORM_PACK32:
				case MgFormat.A2B10G10R10_SNORM_PACK32:
					return MTLVertexFormat.Int1010102Normalized;
				case MgFormat.A2R10G10B10_UNORM_PACK32:
				case MgFormat.A2B10G10R10_UNORM_PACK32:
					return MTLVertexFormat.UInt1010102Normalized;
			}
		}

		public static MTLPixelFormat GetPixelFormat(MgFormat format)
		{
			switch (format)
			{
				default:
					throw new NotSupportedException();
				case MgFormat.UNDEFINED:
					return MTLPixelFormat.Invalid;
				//A8Unorm,
				case MgFormat.R8_SRGB:
					return MTLPixelFormat.R8Unorm_sRGB;
				case MgFormat.R8_UNORM:
					return MTLPixelFormat.R8Unorm;
				case MgFormat.R8_SNORM:
					return MTLPixelFormat.R8Snorm;
				case MgFormat.R8_UINT:
					return MTLPixelFormat.R8Uint;
				case MgFormat.R8_SINT:
					return MTLPixelFormat.R8Sint;
				case MgFormat.R16_UNORM:
					return MTLPixelFormat.R16Unorm;
				case MgFormat.R16_SNORM:
					return MTLPixelFormat.R16Snorm;
				case MgFormat.R16_UINT:
					return MTLPixelFormat.R16Uint;
				case MgFormat.R16_SINT:
					return MTLPixelFormat.R16Sint;
				case MgFormat.R16_SFLOAT:
					return MTLPixelFormat.R16Float;
				case MgFormat.R8G8_UNORM:
					return MTLPixelFormat.RG8Unorm;
				case MgFormat.R8G8_SNORM:
					return MTLPixelFormat.RG8Snorm;
				case MgFormat.R8G8_UINT:
					return MTLPixelFormat.RG8Uint;
				case MgFormat.R8G8_SINT:
					return MTLPixelFormat.RG8Sint;
				case MgFormat.R32_UINT:
					return MTLPixelFormat.R32Uint;
				case MgFormat.R32_SINT:
					return MTLPixelFormat.R32Sint;
				case MgFormat.R32_SFLOAT:
					return MTLPixelFormat.R32Float;
				case MgFormat.R16G16_UNORM:
					return MTLPixelFormat.RG16Unorm;
				case MgFormat.R16G16_SNORM:
					return MTLPixelFormat.RG16Snorm;
				case MgFormat.R16G16_UINT:
					return MTLPixelFormat.RG16Uint;
				case MgFormat.R16G16_SINT:
					return MTLPixelFormat.RG16Sint;
				case MgFormat.R16G16_SFLOAT:
					return MTLPixelFormat.RG16Float;
				case MgFormat.R8G8B8A8_UNORM:
					return MTLPixelFormat.RGBA8Unorm;
				case MgFormat.R8G8B8A8_SRGB:
					return MTLPixelFormat.RGBA8Unorm_sRGB;
				case MgFormat.R8G8B8A8_SNORM:
					return MTLPixelFormat.RGBA8Snorm;
				case MgFormat.R8G8B8A8_UINT:
					return MTLPixelFormat.RGBA8Uint;
				case MgFormat.R8G8B8A8_SINT:
					return MTLPixelFormat.RGBA8Sint;
				case MgFormat.B8G8R8A8_UNORM:
					return MTLPixelFormat.BGRA8Unorm;
				case MgFormat.B8G8R8A8_SRGB:
					return MTLPixelFormat.BGRA8Unorm_sRGB;
				case MgFormat.A2R10G10B10_UNORM_PACK32:
					return MTLPixelFormat.RGB10A2Unorm;
				case MgFormat.A2R10G10B10_UINT_PACK32:
					return MTLPixelFormat.RGB10A2Uint;
				case MgFormat.B10G11R11_UFLOAT_PACK32:
					return MTLPixelFormat.RG11B10Float;
				case MgFormat.E5B9G9R9_UFLOAT_PACK32:
					return MTLPixelFormat.RGB9E5Float;
				case MgFormat.R32G32_UINT:
					return MTLPixelFormat.RG32Uint;
				case MgFormat.R32G32_SINT:
					return MTLPixelFormat.RG32Sint;
				case MgFormat.R32G32_SFLOAT:
					return MTLPixelFormat.RG32Float;
				case MgFormat.R16G16B16A16_UNORM:
					return MTLPixelFormat.RGBA16Unorm;
				case MgFormat.R16G16B16A16_SNORM:
					return MTLPixelFormat.RGBA16Snorm;
				case MgFormat.R16G16B16A16_UINT:
					return MTLPixelFormat.RGBA16Uint;
				case MgFormat.R16G16B16A16_SINT:
					return MTLPixelFormat.RGBA16Sint;
				case MgFormat.R16G16B16A16_SFLOAT:
					return MTLPixelFormat.RGBA16Float;
				case MgFormat.R32G32B32A32_UINT:
					return MTLPixelFormat.RGBA32Uint;
				case MgFormat.R32G32B32A32_SINT:
					return MTLPixelFormat.RGBA32Sint;
				case MgFormat.R32G32B32A32_SFLOAT:
					return MTLPixelFormat.RGBA32Float;
				case MgFormat.BC1_RGBA_UNORM_BLOCK:
					return MTLPixelFormat.BC1RGBA;
				case MgFormat.BC1_RGBA_SRGB_BLOCK:
					return MTLPixelFormat.BC1_RGBA_sRGB;
				case MgFormat.BC2_UNORM_BLOCK:
					return MTLPixelFormat.BC2RGBA;
				case MgFormat.BC2_SRGB_BLOCK:
					return MTLPixelFormat.BC2_RGBA_sRGB;
				case MgFormat.BC3_UNORM_BLOCK:
					return MTLPixelFormat.BC3RGBA;
				case MgFormat.BC3_SRGB_BLOCK:
					return MTLPixelFormat.BC3_RGBA_sRGB;
				case MgFormat.BC4_UNORM_BLOCK:
					return MTLPixelFormat.BC4_RUnorm;
				case MgFormat.BC4_SNORM_BLOCK:
					return MTLPixelFormat.BC4_RSnorm;
				case MgFormat.BC5_UNORM_BLOCK:
					return MTLPixelFormat.BC5_RGUnorm;
				case MgFormat.BC5_SNORM_BLOCK:
					return MTLPixelFormat.BC5_RGSnorm;
				case MgFormat.BC6H_SFLOAT_BLOCK:
					return MTLPixelFormat.BC6H_RGBFloat;
				case MgFormat.BC6H_UFLOAT_BLOCK:
					return MTLPixelFormat.BC6H_RGBUFloat;
				case MgFormat.BC7_UNORM_BLOCK:
					return MTLPixelFormat.BC7_RGBAUnorm;
				case MgFormat.BC7_SRGB_BLOCK:
					return MTLPixelFormat.BC7_RGBAUnorm_sRGB;
				//GBGR422 = 240uL,
				//BGRG422,
				case MgFormat.D32_SFLOAT:
					return MTLPixelFormat.Depth32Float;
				case MgFormat.S8_UINT:
					return MTLPixelFormat.Stencil8;
				case MgFormat.D24_UNORM_S8_UINT:
					return MTLPixelFormat.Depth24Unorm_Stencil8;
				case MgFormat.D32_SFLOAT_S8_UINT:
					return MTLPixelFormat.Depth32Float_Stencil8;
			}
		}

		public static bool IsStencilFormat(MgFormat format)
		{
			switch (format)
			{
				default:
					return false;
				case MgFormat.S8_UINT:
					//return MTLPixelFormat.Stencil8;
				case MgFormat.D24_UNORM_S8_UINT:
					//return MTLPixelFormat.Depth24Unorm_Stencil8;
				case MgFormat.D32_SFLOAT_S8_UINT:
					//return MTLPixelFormat.Depth32Float_Stencil8;
					return true;
			}
		}
	}
}
