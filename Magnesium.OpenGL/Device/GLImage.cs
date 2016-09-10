using System;

namespace Magnesium.OpenGL
{
	public class GLImage : IMgImage
	{
		private readonly int mTextureId;
		public int OriginalTextureId {
			get {
				return mTextureId;
			}
		}

//		private readonly SizedInternalFormat mInternalFormat;
//		public SizedInternalFormat InternalFormat
//		{
//			get
//			{ 
//				return mInternalFormat;
//			}
//		}

		private int mLevels;
		public int Levels
		{
			get {
				return mLevels;
			}
		}

		private readonly int mDepth;
		public int Depth
		{
			get {
				return mDepth;
			}
		}

		private readonly MgImageType mImageType;
		public MgImageType ImageType {
			get {
				return mImageType;
			}
		}

		private MgFormat mFormat;
		public MgFormat Format {
			get {
				return mFormat;
			}
		}

		private int mLayers;

		private IGLDeviceImageEntrypoint mEntrypoint;
		public GLImage (IGLDeviceImageEntrypoint entrypoint, int textureId, MgImageType imageType, MgFormat format, int width, int height, int depth, int levels, int arrayLayers)
		{
			mEntrypoint = entrypoint;
			mTextureId = textureId;
			mFormat = format;
			mImageType = imageType;
			//mInternalFormat = internalFormat;
			mWidth = width;
			mHeight = height;
			mDepth = depth;
			mLevels = levels;
			mLayers = arrayLayers;
			GenerateMipmapLevels ();
		}

		private static uint GetSize(MgFormat surfaceFormat)
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
			case MgFormat.BC2_UNORM_BLOCK:
				//case SurfaceFormat.Dxt3:
			case MgFormat.BC2_SRGB_BLOCK:
				//case SurfaceFormat.Dxt3SRgb:
			case MgFormat.BC3_UNORM_BLOCK:
				//case SurfaceFormat.Dxt5:
			case MgFormat.BC3_SRGB_BLOCK:
				//case SurfaceFormat.Dxt5SRgb:
				//case SurfaceFormat.RgbPvrtc4Bpp:
				//case SurfaceFormat.RgbaPvrtc4Bpp:
				//case SurfaceFormat.RgbaAtcExplicitAlpha:
				//case SurfaceFormat.RgbaAtcInterpolatedAlpha:
				// One texel in DXT3, DXT5 and PVRTC 4bpp is a minimum 4x4 block, which is 16 bytes
				return 16;
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
			case MgFormat.R32G32B32A32_SFLOAT:				
				//case SurfaceFormat.Vector4:
				return 16;
			case MgFormat.R8G8B8_SRGB:
			case MgFormat.R8G8B8_SSCALED:
			case MgFormat.R8G8B8_UINT:
			case MgFormat.R8G8B8_UNORM:
			case MgFormat.R8G8B8_USCALED:
			case MgFormat.R8G8B8_SINT:
			case MgFormat.R8G8B8_SNORM:
				return 3;
			default:
				throw new ArgumentException();
			}
		}

		private GLImageArraySubresource[] mArraySubresources;
		public GLImageArraySubresource[] ArrayLayers { 
			get
			{
				return mArraySubresources;
			}
		}

		private void GenerateMipmapLevels()
		{
			ulong imageSize = 0;
			ulong offset = 0;

			uint width = (uint) mWidth;
			uint height = (uint) mHeight;
			uint depth = (uint) mDepth;

			// 
			uint pixelSize = GetSize (mFormat);

			mArraySubresources = new GLImageArraySubresource[mLayers];

			for (uint index = 0; index < mLayers; ++index)
			{
				var arrayItem = new GLImageArraySubresource (index, new GLImageLevelSubresource[mLevels]);

				for (uint level = 0; level < mLevels; ++level)
				{		
					switch (mFormat)
					{
					// FIXME : 
					//				//case SurfaceFormat.RgbPvrtc2Bpp:
					//				case SurfaceFormat.RgbaPvrtc2Bpp:
					//					imageSize = (Math.Max(this.width, 16) * Math.Max(this.height, 8) * 2 + 7) / 8;
					//					break;
					//				case SurfaceFormat.RgbPvrtc4Bpp:
					//				case SurfaceFormat.RgbaPvrtc4Bpp:
					//					imageSize = (Math.Max(this.width, 8) * Math.Max(this.height, 8) * 4 + 7) / 8;
					//					break;
					case MgFormat.BC1_RGB_UNORM_BLOCK:
						//case SurfaceFormat.Dxt1:
					case MgFormat.BC1_RGBA_UNORM_BLOCK:
						//case SurfaceFormat.Dxt1a:
					case MgFormat.BC1_RGB_SRGB_BLOCK:
						//case SurfaceFormat.Dxt1SRgb:
					case MgFormat.BC2_UNORM_BLOCK:
						//case SurfaceFormat.Dxt3:
					case MgFormat.BC2_SRGB_BLOCK:				
						//case SurfaceFormat.Dxt3SRgb:
					case MgFormat.BC3_UNORM_BLOCK:
						//case SurfaceFormat.Dxt5:
					case MgFormat.BC3_SRGB_BLOCK:
						//case SurfaceFormat.Dxt5SRgb:
						//case SurfaceFormat.RgbEtc1:
						//case SurfaceFormat.RgbaAtcExplicitAlpha:
						//case SurfaceFormat.RgbaAtcInterpolatedAlpha:

						// TODO : include depth SOMEHOW
						imageSize = ((width + 3) / 4) * ((height + 3) / 4) * pixelSize;
						break;
					default:
						imageSize = pixelSize * width * height * depth;
						break;
					//return Result.ERROR_FEATURE_NOT_PRESENT;
					}

					var current = new MgSubresourceLayout {
						Offset = offset,
						Size = imageSize,
						ArrayPitch = RoundPixelUp(imageSize),
						RowPitch = (mImageType == MgImageType.TYPE_1D) ? 0U : RoundPixelUp(width),
						DepthPitch = (mImageType == MgImageType.TYPE_1D || mImageType == MgImageType.TYPE_2D) ? 0U : RoundPixelUp(depth),
					};

					var mipLevelData = new GLImageLevelSubresource ();
					mipLevelData.Width = (int) width;
					mipLevelData.Height = (int) height;
					mipLevelData.Depth = (int)depth;
					mipLevelData.SubresourceLayout = current;

					arrayItem.Levels [level] = mipLevelData;

					// for next array item
					offset += current.ArrayPitch;

					if (width > 1)
						width /= 2;

					if (height > 1)
						height /= 2;

					if (depth > 1)
						depth /= 2;
				}
				mArraySubresources [index] = arrayItem;
			}

		}

		private ulong RoundPixelUp(ulong value)
		{
			// round up to 4
			return value;
		}


		private readonly int mHeight;
		public int Height {
			get {
				return mHeight;
			}
		}

		private readonly int mWidth;
		public int Width {
			get
			{
				return mWidth;
			}
		}

		#region IMgImage implementation
		private IntPtr mMemory = IntPtr.Zero;
		public IntPtr Handle {
			get {
				return mMemory;
			}
		}
		public Result BindImageMemory (IMgDevice device, IMgDeviceMemory memory, ulong memoryOffset)
		{
			var deviceMemory = memory as IGLDeviceMemory;

			if (deviceMemory != null)
			{
				mMemory = IntPtr.Add (deviceMemory.Handle, (int)memoryOffset);
			}
			return Result.SUCCESS;
		}

		private bool mIsDisposed = false;
		public void DestroyImage (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			mEntrypoint.DeleteImage (mTextureId);
			//GL.DeleteTexture (mTextureId);

			mIsDisposed = true;
		}

		#endregion
	}
}

