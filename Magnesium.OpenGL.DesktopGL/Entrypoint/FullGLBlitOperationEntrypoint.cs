using OpenTK.Graphics.OpenGL;
using System;

namespace Magnesium.OpenGL.DesktopGL
{
    public class FullGLBlitOperationEntrypoint : IGLBlitOperationEntrypoint
    {
        private readonly IGLErrorHandler mErrHandler;
        public FullGLBlitOperationEntrypoint(IGLErrorHandler errHandler)
        {
            mErrHandler = errHandler;
        }

        public void CopyBuffer(uint src, uint dst, IntPtr readOffset, IntPtr writeOffset, int size)
        {
            GL.CopyNamedBufferSubData(src, dst, readOffset, writeOffset, size);
            mErrHandler.CheckGLError();
        }

        private static uint GetUnpackSize(MgFormat surfaceFormat)
        {
            switch (surfaceFormat)
            {
                case MgFormat.BC1_RGB_UNORM_BLOCK:
                //case SurfaceFormat.Dxt1:
                case MgFormat.BC1_RGB_SRGB_BLOCK:
                //case SurfaceFormat.Dxt1SRgb:
                case MgFormat.BC1_RGBA_UNORM_BLOCK:
                    //case SurfaceFormat.Dxt1a:
                    //case SurfaceFormat.RgbPvrtc2Bpp:
                    //case SurfaceFormat.RgbaPvrtc2Bpp:			
                    //case SurfaceFormat.RgbEtc1:
                    // One texel in DXT1, PVRTC 2bpp and ETC1 is a minimum 4x4 block, which is 8 bytes
                    return 8;
                //			case MgFormat.BC2_UNORM_BLOCK:
                //				//case SurfaceFormat.Dxt3:
                //			case MgFormat.BC2_SRGB_BLOCK:
                //				//case SurfaceFormat.Dxt3SRgb:
                //			case MgFormat.BC3_UNORM_BLOCK:
                //				//case SurfaceFormat.Dxt5:
                //			case MgFormat.BC3_SRGB_BLOCK:
                //				//case SurfaceFormat.Dxt5SRgb:
                //				//case SurfaceFormat.RgbPvrtc4Bpp:
                //				//case SurfaceFormat.RgbaPvrtc4Bpp:
                //				//case SurfaceFormat.RgbaAtcExplicitAlpha:
                //				//case SurfaceFormat.RgbaAtcInterpolatedAlpha:
                //				// One texel in DXT3, DXT5 and PVRTC 4bpp is a minimum 4x4 block, which is 16 bytes
                //				return 16;
                case MgFormat.R8_UNORM:
                    //case SurfaceFormat.Alpha8:
                    return 1;
                case MgFormat.B5G6R5_UNORM_PACK16:
                //case SurfaceFormat.Bgr565:
                case MgFormat.B4G4R4A4_UNORM_PACK16:
                //case SurfaceFormat.Bgra4444:
                case MgFormat.B5G5R5A1_UNORM_PACK16:
                //case SurfaceFormat.Bgra5551:
                case MgFormat.R16_SFLOAT:
                //case SurfaceFormat.HalfSingle:
                case MgFormat.R16_UNORM:
                    //case SurfaceFormat.NormalizedByte2:
                    return 2;
                case MgFormat.R8G8B8A8_UINT:
                //case SurfaceFormat.Color:
                case MgFormat.R8G8B8A8_SRGB:
                //case SurfaceFormat.ColorSRgb:
                case MgFormat.R32_SFLOAT:
                //case SurfaceFormat.Single:
                case MgFormat.R16G16_UINT:
                //case SurfaceFormat.Rg32:
                case MgFormat.R16G16_SFLOAT:
                //case SurfaceFormat.HalfVector2:
                case MgFormat.R8G8B8A8_SNORM:
                //case SurfaceFormat.NormalizedByte4:
                case MgFormat.A2B10G10R10_UINT_PACK32:
                    //case SurfaceFormat.Rgba1010102:
                    //case SurfaceFormat.Bgra32:
                    //case SurfaceFormat.Bgra32SRgb:
                    //case SurfaceFormat.Bgr32:
                    //case SurfaceFormat.Bgr32SRgb:
                    return 4;
                case MgFormat.R16G16B16A16_SFLOAT:
                //case SurfaceFormat.HalfVector4:
                //case SurfaceFormat.Rgba64:
                case MgFormat.R32G32_SFLOAT:
                    //case SurfaceFormat.Vector2:
                    return 8;
                //			case MgFormat.R32G32B32A32_SFLOAT:				
                //				//case SurfaceFormat.Vector4:
                //				return 16;
                //			case MgFormat.R8G8B8_SRGB:
                //			case MgFormat.R8G8B8_SSCALED:
                //			case MgFormat.R8G8B8_UINT:
                //			case MgFormat.R8G8B8_UNORM:
                //			case MgFormat.R8G8B8_USCALED:
                //			case MgFormat.R8G8B8_SINT:
                //			case MgFormat.R8G8B8_SNORM:
                //				return 3;
                default:
                    throw new ArgumentException();
            }
        }

        public void PerformOperation(GLCmdImageInstructionSet instructionSet)
        {
            const int DEFAULT_UNPACK_ALIGNMENT = 4;

            if (instructionSet == null)
                return;

            foreach (var inst in instructionSet.Images)
            {
                if (inst.Target == MgImageType.TYPE_1D)
                {
                    if (inst.PixelFormat == (int)All.CompressedTextureFormats)
                    {
                        GL.Ext.CompressedTextureSubImage1D(
                            inst.TextureId,
                            TextureTarget.Texture1D,
                            inst.Level,
                            inst.Slice,
                            inst.Width,
                            (PixelFormat)inst.InternalFormat,
                            inst.Size,
                            inst.Data
                        );

                        mErrHandler.LogGLError("CompressedTextureSubImage1D");

                    }
                    else
                    {
                        // Set pixel alignment to match texel size in bytes
                        GL.PixelStore(PixelStoreParameter.UnpackAlignment, GetUnpackSize(inst.Format));

                        mErrHandler.LogGLError("PixelStore.BEGIN");

                        GL.Ext.TextureSubImage1D(
                            inst.TextureId,
                            TextureTarget.Texture1D,
                            inst.Level,
                            0,
                            inst.Width,
                            (PixelFormat)inst.PixelFormat,
                            (PixelType)inst.PixelType,
                            inst.Data
                        );

                        mErrHandler.LogGLError("TextureSubImage1D");

                        GL.PixelStore(PixelStoreParameter.UnpackAlignment, DEFAULT_UNPACK_ALIGNMENT);

                        mErrHandler.LogGLError("PixelStore.END");
                    }
                }
                else if (inst.Target == MgImageType.TYPE_2D)
                {
                    if (inst.PixelFormat == (int)All.CompressedTextureFormats)
                    {
                        GL.Ext.CompressedTextureSubImage2D(
                            inst.TextureId,
                            TextureTarget.Texture2D,
                            inst.Level,
                            0,
                            inst.Slice,
                            inst.Width,
                            inst.Height,
                            (PixelFormat)inst.InternalFormat,
                            inst.Size,
                            inst.Data
                        );

                        mErrHandler.LogGLError("CompressedTextureSubImage2D");
                    }
                    else
                    {
                        // Set pixel alignment to match texel size in bytes
                        GL.PixelStore(PixelStoreParameter.UnpackAlignment, GetUnpackSize(inst.Format));

                        mErrHandler.LogGLError("PixelStore.BEGIN");

                        // SEEMS to behave like glTexImage2D
                        // https://www.opengl.org/sdk/docs/man/html/glTexImage2D.xhtml
                        GL.Ext.TextureSubImage2D(
                            inst.TextureId,
                            TextureTarget.Texture2D,
                            inst.Level,
                            0,
                            inst.Slice,
                            inst.Width,
                            inst.Height,
                            (PixelFormat)inst.PixelFormat,
                            (PixelType)inst.PixelType,
                            inst.Data
                        );

                        mErrHandler.LogGLError("TextureSubImage2D");

                        GL.PixelStore(PixelStoreParameter.UnpackAlignment, DEFAULT_UNPACK_ALIGNMENT);

                        mErrHandler.LogGLError("PixelStore.END");
                    }
                }
                else if (inst.Target == MgImageType.TYPE_3D)
                {
                    if (inst.PixelFormat == (int)All.CompressedTextureFormats)
                    {
                        GL.Ext.CompressedTextureSubImage3D(
                            inst.TextureId,
                            TextureTarget.Texture3D,
                            inst.Level,
                            0,
                            0,
                            inst.Slice,
                            inst.Width,
                            inst.Height,
                            inst.Depth,
                            (PixelFormat)inst.InternalFormat,
                            inst.Size,
                            inst.Data
                        );

                        mErrHandler.LogGLError("CompressedTextureSubImage3D");
                    }
                    else
                    {
                        // Set pixel alignment to match texel size in bytes
                        GL.PixelStore(PixelStoreParameter.UnpackAlignment, GetUnpackSize(inst.Format));
                        mErrHandler.LogGLError("PixelStore.BEGIN");

                        GL.Ext.TextureSubImage3D(
                            inst.TextureId,
                            TextureTarget.Texture3D,
                            inst.Level,
                            0,
                            0,
                            inst.Slice,
                            inst.Width,
                            inst.Height,
                            inst.Depth,
                            (PixelFormat)inst.PixelFormat,
                            (PixelType)inst.PixelType,
                            inst.Data
                        );

                        mErrHandler.LogGLError("TextureSubImage3D");

                        GL.PixelStore(PixelStoreParameter.UnpackAlignment, DEFAULT_UNPACK_ALIGNMENT);

                        mErrHandler.LogGLError("PixelStore.END");
                    }
                }
            }
        }

    }
}
