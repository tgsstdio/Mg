using System;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace MonoGame.Textures.Ktx.DesktopGL
{
	/// <summary>
	/// ETC unpacker.
	/// Code translation from original C loader
	/// </summary>
	public class ETCUnpacker : IETCUnpacker
	{
		public IGLContextCapabilities Profile { get; private set; }
		public ETCUnpacker (IGLContextCapabilities profile)
		{
			Profile = profile;
		}

		public bool IsRequired (KTXLoadInstructions instructions, int glError)
		{
			const int GL_COMPRESSED_R11_EAC = 0x9270;
			var error = (ErrorCode)glError;

			return (error == ErrorCode.InvalidEnum || error == ErrorCode.InvalidValue)				
				&& instructions.IsCompressed && instructions.TextureDimensions == 2
				// GL_ETC1_RGB8_OES
				&& (instructions.GlInternalFormat == (int)OpenTK.Graphics.ES20.All.Etc1Rgb8Oes 
					// (glInternalFormat >= GL_COMPRESSED_R11_EAC && glInternalFormat <= GL_COMPRESSED_SRGB8_ALPHA8_ETC2_EAC)
					|| (instructions.GlInternalFormat >= GL_COMPRESSED_R11_EAC && instructions.GlInternalFormat <= (int)OpenTK.Graphics.ES30.CompressedInternalFormat.CompressedSrgb8Alpha8Etc2Eac));
		}

		public KTXError UnpackCompressedTexture (KTXLoadInstructions instructions, int level, int face, int pixelWidth, int pixelHeight, byte[] data)
		{
			byte[] unpacked;
			PixelFormat format;
			PixelInternalFormat internalFormat;
			PixelType type;
			bool formatSigned;
			KTXError errorCode = _ktxUnpackETC (data, (int)instructions.GlInternalFormat, (uint)pixelWidth, (uint)pixelHeight, out formatSigned, out unpacked, out format, out internalFormat, out type, Profile.R16Formats, Profile.SupportsSRGB);
			if (errorCode != KTXError.Success)
			{
				return errorCode;
			}	

			if ((Profile.SizedFormats & GLSizedFormats.NonLegacy) > 0)
			{
				if (internalFormat == PixelInternalFormat.Rgb8)
				{
					internalFormat = PixelInternalFormat.Rgb;
				}
				else
					if (internalFormat == PixelInternalFormat.Rgba8)
					{
						internalFormat = PixelInternalFormat.Rgba;
					}
			}
			GL.TexImage2D ((TextureTarget)(instructions.GlTarget + face), level, internalFormat, pixelWidth, pixelHeight, 0, format, type, unpacked);
			return KTXError.Success;
		}

		private enum eAlphaFormat
		{
			AfNone = 0,
			Af1Bit,
			Af8Bit,
			Af11Bit
		}

		/* Unpack an ETC1_RGB8_OES format compressed texture */
		private KTXError _ktxUnpackETC(
			byte[] srcETC,
			int srcFormat,
			uint activeWidth,
			uint activeHeight,
			out bool formatSigned,
			out byte[] dstImage,
			out PixelFormat format,
			out PixelInternalFormat internalFormat,
			out PixelType type,
			GLR16Formats r16Formats,
			bool supportsSRGB)
		{
			formatSigned = false;
			dstImage = null;
			format = (PixelFormat) 0;
			internalFormat = (PixelInternalFormat) 0;
			type = (PixelType)0;

			uint width, height;

			//uint x, y;

			/*const*/ byte[] src = srcETC;
			int srcOffset = 0;
			// AF_11BIT is used to compress R11 & RG11 though its not alpha data.
			eAlphaFormat alphaFormat = eAlphaFormat.AfNone;
			int dstChannels = 0;
			int dstChannelBytes = 0;

			switch (srcFormat)
			{
			case (int) OpenTK.Graphics.ES30.All.CompressedSignedR11Eac:  // GL_COMPRESSED_SIGNED_R11_EAC: // GL_COMPRESSED_SIGNED_R11_EAC
				if ((r16Formats & GLR16Formats.SNorm) > 0)
				{
					dstChannelBytes = sizeof(UInt16);
					dstChannels = 1;
					formatSigned = true;
					internalFormat = PixelInternalFormat.R16Snorm; //GL_R16_SNORM;
					format = PixelFormat.Red; //GL_RED;
					type = PixelType.Short; // GL_SHORT;
					alphaFormat = eAlphaFormat.Af11Bit;
				}
				else
				{
					return KTXError.UnsupportedTextureType; 
				}
				break;

			case (int) OpenTK.Graphics.ES30.All.CompressedR11Eac: // GL_COMPRESSED_R11_EAC:
				if ((r16Formats & GLR16Formats.Norm) > 0)
				{
					dstChannelBytes = sizeof(UInt16);
					dstChannels = 1;
					formatSigned = false;
					internalFormat = PixelInternalFormat.R16; //GL_R16;
					format = PixelFormat.Red; //GL_RED;
					type = PixelType.UnsignedShort; //GL_UNSIGNED_SHORT;
					alphaFormat = eAlphaFormat.Af11Bit;
				}
				else
				{
					return KTXError.UnsupportedTextureType; 
				}
				break;

			case (int) OpenTK.Graphics.ES30.All.CompressedSignedRg11Eac:// GL_COMPRESSED_SIGNED_RG11_EAC:
				if ((r16Formats & GLR16Formats.SNorm) > 0)
				{
					dstChannelBytes = sizeof(UInt16);
					dstChannels = 2;
					formatSigned = true;
					internalFormat = PixelInternalFormat.Rg16Snorm;//GL_RG16_SNORM;
					format = PixelFormat.Rg;// GL_RG;
					type = PixelType.Short; //GL_SHORT;
					alphaFormat = eAlphaFormat.Af11Bit;
				} 
				else
				{
					return KTXError.UnsupportedTextureType; 
				}
				break;

			case (int) OpenTK.Graphics.ES30.All.CompressedRg11Eac:  //GL_COMPRESSED_RG11_EAC:
				if ((r16Formats & GLR16Formats.Norm) > 0)
				{
					dstChannelBytes = sizeof(UInt16);
					dstChannels = 2;
					formatSigned = false;
					internalFormat = PixelInternalFormat.Rg16;//GL_RG16;
					format = PixelFormat.Rg; // GL_RG;
					type = PixelType.UnsignedShort; // GL_UNSIGNED_SHORT;
					alphaFormat = eAlphaFormat.Af11Bit;
				} 
				else
				{
					return KTXError.UnsupportedTextureType; 
				}
				break;

			case (int) OpenTK.Graphics.ES30.All.Rgb8Oes: // GL_ETC1_RGB8_OES:
			case (int) OpenTK.Graphics.ES30.All.CompressedRgb8Etc2: //GL_COMPRESSED_RGB8_ETC2:
				dstChannelBytes = sizeof(byte);
				dstChannels = 3;
				internalFormat = PixelInternalFormat.Rgb8; //GL_RGB8;
				format = PixelFormat.Rgb; // GL_RGB;
				type = PixelType.UnsignedByte; //GL_UNSIGNED_BYTE;
				break;

			case (int) OpenTK.Graphics.ES30.All.CompressedRgba8Etc2Eac://GL_COMPRESSED_RGBA8_ETC2_EAC:
				dstChannelBytes = sizeof(byte);
				dstChannels = 4;
				internalFormat = PixelInternalFormat.Rgba8; //GL_RGBA8;
				format = PixelFormat.Rgba;//GL_RGBA;
				type = PixelType.UnsignedByte; // GL_UNSIGNED_BYTE;
				alphaFormat = eAlphaFormat.Af8Bit;
				break;

			case (int) OpenTK.Graphics.ES30.All.CompressedRgb8PunchthroughAlpha1Etc2: //GL_COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2:
				dstChannelBytes = sizeof(byte);
				dstChannels = 4;
				internalFormat = PixelInternalFormat.Rgba8; //GL_RGBA8;
				format = PixelFormat.Rgba; //GL_RGBA;
				type = PixelType.UnsignedByte; //GL_UNSIGNED_BYTE;
				alphaFormat = eAlphaFormat.Af1Bit;
				break;

			case (int) OpenTK.Graphics.ES30.All.CompressedSrgb8Etc2://GL_COMPRESSED_SRGB8_ETC2:
				if (supportsSRGB)
				{
					dstChannelBytes = sizeof(byte);
					dstChannels = 3;
					internalFormat = PixelInternalFormat.Srgb8; // GL_SRGB8;
					format = PixelFormat.Rgb; // GL_RGB;
					type = PixelType.UnsignedByte; // GL_UNSIGNED_BYTE;
				}
				else
				{
					return KTXError.UnsupportedTextureType; 
				}
				break;

			case (int) OpenTK.Graphics.ES30.All.CompressedSrgb8Alpha8Etc2Eac://GL_COMPRESSED_SRGB8_ALPHA8_ETC2_EAC:
				if (supportsSRGB)
				{
					dstChannelBytes = sizeof(byte);
					dstChannels = 4;
					internalFormat = PixelInternalFormat.Srgb8Alpha8;// GL_SRGB8_ALPHA8;
					format = PixelFormat.Rgba; //GL_RGBA;
					type = PixelType.UnsignedByte; //GL_UNSIGNED_BYTE;
					alphaFormat = eAlphaFormat.Af8Bit;
				} 
				else
				{
					return KTXError.UnsupportedTextureType; 
				}
				break;

			case (int) OpenTK.Graphics.ES30.All.CompressedSrgb8PunchthroughAlpha1Etc2://	GL_COMPRESSED_SRGB8_PUNCHTHROUGH_ALPHA1_ETC2:
				if (supportsSRGB)
				{
					dstChannelBytes = sizeof(byte);
					dstChannels = 4;
					internalFormat = PixelInternalFormat.Srgb8Alpha8;// GL_SRGB8_ALPHA8;
					format = PixelFormat.Rgba; // GL_RGBA;
					type = PixelType.UnsignedByte; //GL_UNSIGNED_BYTE;
					alphaFormat = eAlphaFormat.Af1Bit;
				}
				else
				{
					return KTXError.UnsupportedTextureType; 
				}
				break;

			default:
				Debug.Assert(false); // Upper levels should be passing only one of the above srcFormats.
				break;
			}

			/* active_{width,height} show how many pixels contain active data,
			 * (the rest are just for making sure we have a 2*a x 4*b size).
			 */

			/* Compute the full width & height. */
			width = ((activeWidth+3)/4)*4;
			height = ((activeHeight+3)/4)*4;

			/* printf("Width = %d, Height = %d\n", width, height); */
			/* printf("active pixel area: top left %d x %d area.\n", activeWidth, activeHeight); */

			dstImage = new byte[dstChannels*dstChannelBytes*width*height];
			if (dstImage == null)
			{
				return KTXError.OutOfMemory;
			}

			if (alphaFormat != eAlphaFormat.AfNone)
			{
				setupAlphaTable ();
			}

			// NOTE: none of the decompress functions actually use the <height> parameter
			if (alphaFormat == eAlphaFormat.Af11Bit)
			{
				// One or two 11-bit alpha channels for R or RG.
				for (uint y=0; y < height/4; y++) {
					for (uint x=0; x < width/4; x++) {
						decompressBlockAlpha16bitC(formatSigned, src, srcOffset, dstImage, 0, width, height, 4*x, 4*y, dstChannels);
						srcOffset += 8;
						if (srcFormat == (int) OpenTK.Graphics.ES30.All.CompressedRg11Eac || srcFormat == (int) OpenTK.Graphics.ES30.All.CompressedSignedRg11Eac) 
						{
							decompressBlockAlpha16bitC(formatSigned, src, srcOffset, dstImage, dstChannelBytes, width, height, 4*x, 4*y, dstChannels);
							srcOffset += 8;
						}
					}
				}
			}
			else {
				UInt32 block_part1 = 0;
				UInt32 block_part2 = 0;

				for (uint y=0; y < height/4; y++)
				{
					for (uint x=0; x < width/4; x++) 
					{
						// Decode alpha channel for RGBA
						if (alphaFormat == eAlphaFormat.Af8Bit) {
							decompressBlockAlphaC(src, srcOffset, dstImage, 3, width, height, 4*x, 4*y, dstChannels);
							srcOffset += 8;
						}
						// Decode color dstChannels
						readBigEndian4byteWord(ref block_part1, src, srcOffset);
						srcOffset += 4;
						readBigEndian4byteWord(ref block_part2, src, srcOffset);
						srcOffset += 4;
						if (alphaFormat == eAlphaFormat.Af1Bit)
							decompressBlockETC21BitAlphaC(block_part1, block_part2, dstImage, null, width, height, 4*x, 4*y, dstChannels);
						else
							decompressBlockETC2c(block_part1, block_part2, dstImage, width, height, 4*x, 4*y, dstChannels);
					}
				}
			}

			/* Ok, now write out the active pixels to the destination image.
	 * (But only if the active pixels differ from the total pixels)
	 */

			if( !(height == activeHeight && width == activeWidth) ) {
				int dstPixelBytes = (int)(dstChannels * dstChannelBytes);
				long dstRowBytes = dstPixelBytes * width;
				long activeRowBytes = activeWidth * dstPixelBytes;
				byte[] newimg = new byte[dstPixelBytes * activeWidth * activeHeight];

				if (newimg == null) {
					dstImage = null;
					return KTXError.OutOfMemory;
				}

				/* Convert from total area to active area: */

				for (uint yy = 0; yy < activeHeight; yy++)
				{
					for (uint xx = 0; xx < activeWidth; xx++) 
					{
						for (int zz = 0; zz < dstPixelBytes; zz++)
						{
							newimg[ yy*activeRowBytes + xx*dstPixelBytes + zz ] = dstImage[ yy*dstRowBytes + xx*dstPixelBytes + zz];
						}
					}
				}

				dstImage = null;
				dstImage = newimg;
			}

			return KTXError.Success;
		}

		bool alphaTableInitialized = false;
		// alphaTable[256][8];
		int[,] alphaTable;
		/// int[,] alphaBase[16][4]
		int[,] alphaBase = {	
			{-15,-9,-6,-3},
			{-13,-10,-7,-3},
			{-13,-8,-5,-2},
			{-13,-6,-4,-2},
			{-12,-8,-6,-3},
			{-11,-9,-7,-3},
			{-11,-8,-7,-4},
			{-11,-8,-5,-3},
			{ -10,-8,-6,-2},
			{ -10,-8,-5,-2},
			{ -10,-8,-4,-2},
			{ -10,-7,-5,-2},
			{ -10,-7,-4,-3},
			{ -10,-3,-2, -1},
			{ -9,-8,-6,-4},
			{ -9,-7,-5,-3}
		};

		// Code used to create the valtab
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		private void setupAlphaTable() 
		{
			if(alphaTableInitialized)
				return;
			alphaTableInitialized = true;
			alphaTable = new int[256, 8];

			//read table used for alpha compression
			int buf;
			for(int i = 16; i<32; i++) 
			{
				for(int j=0; j<8; j++) 
				{
					buf=alphaBase[i - 16, 3-j % 4];
					if(j<4)
						alphaTable[i,j]=buf;
					else
						alphaTable[i,j]=(-buf-1);
				}
			}

			//beyond the first 16 values, the rest of the table is implicit.. so calculate that!
			for(int i=0; i<256; i++) 
			{
				//fill remaining slots in table with multiples of the first ones.
				int mul = i/16;
				int old = 16+i%16;
				for(int j = 0; j<8; j++) 
				{
					alphaTable[i,j]=alphaTable[old,j]*mul;
					//note: we don't do clamping here, though we could, because we'll be clamped afterwards anyway.
				}
			}
		}

		// Decompresses a block using one of the GL_COMPRESSED_R11_EAC or GL_COMPRESSED_SIGNED_R11_EAC-formats
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		void decompressBlockAlpha16bitC(bool formatSigned, byte[] data, int dataOffset, byte[] img, int imgOffset, uint width, uint height, uint ix, uint iy, int channels) 
		{
			int alpha = data[dataOffset];
			int table = data[dataOffset + 1];

			if(formatSigned) 
			{
				//if we have a signed format, the base value is given as a signed byte. We convert it to (0-255) here,
				//so more code can be shared with the unsigned mode.
				alpha = (sbyte) data[dataOffset];
				alpha = alpha+128;
			}

			int bit=0;
			int byteIndex =2;
			//extract an alpha value for each pixel.
			for(int x=0; x<4; x++) 
			{
				for(int y=0; y<4; y++) 
				{
					//Extract table index
					int index=0;
					for(int bitpos=0; bitpos<3; bitpos++) 
					{
						index|=getbit(data[byteIndex], 7-bit, 2-bitpos);
						bit++;
						if(bit>7) 
						{
							bit=0;
							byteIndex++;
						}
					}
					int windex = imgOffset + (int)(channels * (2 * (ix + x + (iy + y) * width)));
					#if !PGMOUT
					if(formatSigned)
					{
						//*(int16 *)&img[windex]
						Int16 signedValue = get16bits11signed(alpha,(table%16),(table/16),index);
						byte[] converted = BitConverter.GetBytes(signedValue);
						img[windex] = converted[0];
						img[windex + 1] = converted[1];
					}
					else
					{
						UInt16 unsignedValue = get16bits11bits(alpha,(table%16),(table/16),index);
						byte[] converted = BitConverter.GetBytes(unsignedValue);
						img[windex] = converted[0];
						img[windex + 1] = converted[1];
					}
					#else
					//make data compatible with the .pgm format. See the comment in compressBlockAlpha16() for details.
					uint16 uSixteen;
					if (formatSigned)
					{
					//the pgm-format only allows unsigned images,
					//so we add 2^15 to get a 16-bit value.
					uSixteen = get16bits11signed(alpha,(table%16),(table/16),index) + 256*128;
					}
					else
					{
					uSixteen = get16bits11bits(alpha,(table%16),(table/16),index);
					}
					//byte swap for pgm
					img[windex] = uSixteen/256;
					img[windex+1] = uSixteen%256;
					#endif

				}
			}			
		}

		// Decodes tha alpha component in a block coded with GL_COMPRESSED_RGBA8_ETC2_EAC.
		// Note that this decoding is slightly different from that of GL_COMPRESSED_R11_EAC.
		// However, a hardware decoder can share gates between the two formats as explained
		// in the specification under GL_COMPRESSED_R11_EAC.
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		void decompressBlockAlphaC(byte[] data, int dataOffset, byte[] img, int imgOffset, uint width, uint height, uint ix, uint iy, int channels) 
		{
			int alpha = data[dataOffset];
			int table = data[dataOffset + 1];

			int bit=0;
			int byteNum=2;
			//extract an alpha value for each pixel.
			for(int x=0; x<4; x++) 
			{
				for(int y=0; y<4; y++) 
				{
					//Extract table index
					int index=0;
					for(int bitpos=0; bitpos<3; bitpos++) 
					{
						index|=getbit(data[byteNum],7-bit,2-bitpos);
						bit++;
						if(bit>7) 
						{
							bit=0;
							byteNum++;
						}
					}
					long finalOffset = imgOffset + ((ix+x+(iy+y)*width)*channels);
					img[finalOffset]=clamp(alpha +alphaTable[table,index]);
				}
			}
		}

		static byte clamp(int n)
		{
			if ((uint) n <= 255) {
				return (byte)n;
			}
			return (byte)((n < 0) ? 0 : 255);
		}

		static void	readBigEndian4byteWord(ref UInt32 pBlock, byte[] s, int offset)
		{
			pBlock = (UInt32) ( (s[offset] << 24) | (s[offset + 1] << 16) | (s[offset + 2] << 8) | s[offset + 3]);
		}

		void decompressBlockAlpha(byte[] data, int dataOffset, byte[] img, int imgOffset, uint width, uint height, uint ix, uint iy) 
		{
			decompressBlockAlphaC(data, dataOffset, img, imgOffset, width, height, ix, iy, 1);
		}

		// Decompression function for ETC2_RGBA1 format.
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		void decompressBlockETC21BitAlphaC(uint blockPart1, uint blockPart2, byte[] img, byte[] alphaimg, uint width, uint height, uint startx, uint starty, int channelsRGB)
		{
			int diffbit;
			var color1 = new sbyte[3];
			var diff = new sbyte[3];
			sbyte red, green, blue;
			int channelsA;	
			int alphaOffset = 0;

			if(channelsRGB == 3)
			{
				// We will decode the alpha data to a separate memory area. 
				channelsA = 1;
			}
			else
			{
				// We will decode the RGB data and the alpha data to the same memory area, 
				// interleaved as RGBA. 
				channelsA = 4;
				alphaimg = img;
				alphaOffset = 3;
			}

			diffbit = (GETBITSHIGH(blockPart1, 1, 33));

			if( diffbit != 0 )
			{
				// We have diffbit = 1, meaning no transparent pixels. regular decompression.

				// Base color
				color1 [0] = (sbyte)GETBITSHIGH (blockPart1, 5, 63);
				color1 [1] = (sbyte)GETBITSHIGH (blockPart1, 5, 55);
				color1 [2] = (sbyte)GETBITSHIGH (blockPart1, 5, 47);

				// Diff color
				diff [0] = (sbyte)GETBITSHIGH (blockPart1, 3, 58);
				diff [1] = (sbyte)GETBITSHIGH (blockPart1, 3, 50);
				diff [2] = (sbyte)GETBITSHIGH (blockPart1, 3, 42);

				// Extend sign bit to entire byte. 
				diff [0] = (sbyte)(diff [0] << 5);
				diff [1] = (sbyte)(diff [1] << 5);
				diff [2] = (sbyte)(diff [2] << 5);
				diff [0] = (sbyte)(diff [0] >> 5);
				diff [1] = (sbyte)(diff [1] >> 5);
				diff [2] = (sbyte)(diff [2] >> 5);

				red = (sbyte)(color1 [0] + diff [0]);
				green = (sbyte)(color1 [1] + diff [1]);
				blue = (sbyte)(color1 [2] + diff [2]);

				if(red < 0 || red > 31)
				{
					uint block59_part1, block59_part2;
					unstuff59bits(blockPart1, blockPart2, out block59_part1, out block59_part2);
					decompressBlockTHUMB59Tc(block59_part1, block59_part2, img, width, height, startx, starty, channelsRGB);
				}
				else if (green < 0 || green > 31)
				{
					uint block58_part1, block58_part2;
					unstuff58bits(blockPart1, blockPart2, out block58_part1, out block58_part2);
					decompressBlockTHUMB58Hc(block58_part1, block58_part2, img, width, height, startx, starty, channelsRGB);
				}
				else if(blue < 0 || blue > 31)
				{
					uint block57_part1, block57_part2;

					unstuff57bits(blockPart1, blockPart2, out block57_part1, out block57_part2);
					decompressBlockPlanar57c(block57_part1, block57_part2, img, width, height, startx, starty, channelsRGB);
				}
				else
				{
					decompressBlockDifferentialWithAlphaC(blockPart1, blockPart2, img, alphaimg, alphaOffset, width, height, startx, starty, channelsRGB);
				}
				for(uint x=startx; x<startx+4; x++) 
				{
					for(uint y=starty; y<starty+4; y++) 
					{
						alphaimg[alphaOffset + channelsA*(x+y*width)]=255;
					}
				}
			}
			else
			{
				// We have diffbit = 0, transparent pixels. Only T-, H- or regular diff-mode possible.

				// Base color
				color1 [0] = (sbyte)GETBITSHIGH (blockPart1, 5, 63);
				color1 [1] = (sbyte)GETBITSHIGH (blockPart1, 5, 55);
				color1 [2] = (sbyte)GETBITSHIGH (blockPart1, 5, 47);

				// Diff color
				diff [0] = (sbyte)GETBITSHIGH (blockPart1, 3, 58);
				diff [1] = (sbyte)GETBITSHIGH (blockPart1, 3, 50);
				diff [2] = (sbyte)GETBITSHIGH (blockPart1, 3, 42);

				// Extend sign bit to entire byte. 
				diff [0] = (sbyte)(diff [0] << 5);
				diff [1] = (sbyte)(diff [1] << 5);
				diff [2] = (sbyte)(diff [2] << 5);
				diff [0] = (sbyte)(diff [0] >> 5);
				diff [1] = (sbyte)(diff [1] >> 5);
				diff [2] = (sbyte)(diff [2] >> 5);

				red = (sbyte)(color1 [0] + diff [0]);
				green = (sbyte)(color1 [1] + diff [1]);
				blue = (sbyte)(color1 [2] + diff [2]);
				if(red < 0 || red > 31)
				{
					uint block59_part1, block59_part2;
					unstuff59bits(blockPart1, blockPart2, out block59_part1, out block59_part2);
					decompressBlockTHUMB59TAlphaC(block59_part1, block59_part2, img, alphaimg, alphaOffset, width, height, startx, starty, channelsRGB);
				}
				else if(green < 0 || green > 31) 
				{
					uint block58_part1, block58_part2;
					unstuff58bits(blockPart1, blockPart2, out block58_part1, out block58_part2);
					decompressBlockTHUMB58HAlphaC(block58_part1, block58_part2, img, alphaimg, alphaOffset, width, height, startx, starty, channelsRGB);
				}
				else if(blue < 0 || blue > 31)
				{
					uint block57_part1, block57_part2;

					unstuff57bits(blockPart1, blockPart2, out block57_part1, out block57_part2);
					decompressBlockPlanar57c(block57_part1, block57_part2, img, width, height, startx, starty, channelsRGB);
					for(uint x=startx; x<startx+4; x++) 
					{
						for(uint y=starty; y<starty+4; y++) 
						{
							alphaimg[alphaOffset + channelsA*(x+y*width)]=255;
						}
					}
				}
				else
					decompressBlockDifferentialWithAlphaC(blockPart1, blockPart2, img, alphaimg, alphaOffset, width, height, startx, starty, channelsRGB);
			}
		}

		// Decompress an ETC2 RGB block
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		void decompressBlockETC2c(uint blockPart1, uint blockPart2, byte[] img, uint width, uint height, uint startx, uint starty, int channels)
		{
			int diffbit;
			sbyte[] color1 = new sbyte[3];
			sbyte[] diff = new sbyte[3];
			sbyte red, green, blue;

			diffbit = (int)GETBITSHIGH (blockPart1, 1, 33);

			if( diffbit != 0 )
			{
				// We have diffbit = 1;

				// Base color
				color1 [0] = (sbyte)GETBITSHIGH (blockPart1, 5, 63);
				color1 [1] = (sbyte)GETBITSHIGH (blockPart1, 5, 55);
				color1 [2] = (sbyte)GETBITSHIGH (blockPart1, 5, 47);

				// Diff color
				diff [0] = (sbyte)GETBITSHIGH (blockPart1, 3, 58);
				diff [1] = (sbyte)GETBITSHIGH (blockPart1, 3, 50);
				diff [2] = (sbyte)GETBITSHIGH (blockPart1, 3, 42);

				// Extend sign bit to entire byte. 
				diff [0] = (sbyte)(diff [0] << 5);
				diff [1] = (sbyte)(diff [1] << 5);
				diff [2] = (sbyte)(diff [2] << 5);
				diff [0] = (sbyte)(diff [0] >> 5);
				diff [1] = (sbyte)(diff [1] >> 5);
				diff [2] = (sbyte)(diff [2] >> 5);

				red = (sbyte)(color1 [0] + diff [0]);
				green = (sbyte)(color1 [1] + diff [1]);
				blue = (sbyte)(color1 [2] + diff [2]);

				if(red < 0 || red > 31)
				{
					UInt32 block59_part1, block59_part2;
					unstuff59bits(blockPart1, blockPart2, out block59_part1, out block59_part2);
					decompressBlockTHUMB59Tc(block59_part1, block59_part2, img, width, height, startx, starty, channels);
				}
				else if (green < 0 || green > 31)
				{
					UInt32 block58_part1, block58_part2;
					unstuff58bits(blockPart1, blockPart2, out block58_part1, out block58_part2);
					decompressBlockTHUMB58Hc(block58_part1, block58_part2, img, width, height, startx, starty, channels);
				}
				else if(blue < 0 || blue > 31)
				{
					UInt32 block57_part1, block57_part2;

					unstuff57bits(blockPart1, blockPart2, out block57_part1, out block57_part2);
					decompressBlockPlanar57c(block57_part1, block57_part2, img, width, height, startx, starty, channels);
				}
				else
				{
					decompressBlockDiffFlipC(blockPart1, blockPart2, img, width, height, startx, starty, channels);
				}
			}
			else
			{
				// We have diffbit = 0;
				decompressBlockDiffFlipC(blockPart1, blockPart2, img, width, height, startx, starty, channels);
			}
		}

		// bit number frompos is extracted from input, and moved to bit number topos in the return value.
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		byte getbit(byte input, int frompos, int topos) 
		{
			if(frompos>topos)
			{
				return (byte)(((1 << frompos) & input) >> (frompos - topos));
			}
			return (byte)(((1 << frompos) & input) << (topos - frompos));
		}

		// Does decompression and then immediately converts from 11 bit signed to a 16-bit format.
		// 
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		Int16 get16bits11signed(int baseNum, int table, int mul, int index) 
		{
			int elevenbase = baseNum-128;
			if(elevenbase==-128)
				elevenbase=-127;
			elevenbase*=8;
			//i want the positive value here
			int tabVal = -alphaBase[table,3-index%4]-1;
			//and the sign, please
			int sign = 1-(index/4);

			if(sign != 0)
				tabVal=tabVal+1;
			int elevenTabVal = tabVal*8;

			if(mul!=0)
				elevenTabVal*=mul;
			else
				elevenTabVal/=8;

			if(sign != 0)
				elevenTabVal=-elevenTabVal;

			//calculate sum
			int elevenbits = elevenbase+elevenTabVal;

			//clamp..
			if(elevenbits>=1024)
				elevenbits=1023;
			else if(elevenbits<-1023)
				elevenbits=-1023;
			//this is the value we would actually output.. 
			//but there aren't any good 11-bit file or uncompressed GL formats
			//so we extend to 15 bits signed.
			sign = (elevenbits<0) ? 1 : 0;
			elevenbits=Math.Abs(elevenbits);
			Int16 fifteenbits = (Int16)((elevenbits << 5) + (elevenbits >> 5));
			Int16 sixteenbits=fifteenbits;

			if(sign != 0)
				sixteenbits = (short)-sixteenbits;

			return sixteenbits;
		}

		// Does decompression and then immediately converts from 11 bit signed to a 16-bit format 
		// Calculates the 11 bit value represented by base, table, mul and index, and extends it to 16 bits.
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		UInt16 get16bits11bits(int baseNum, int table, int mul, int index) 
		{
			int elevenbase = baseNum*8+4;

			//i want the positive value here
			int tabVal = -alphaBase[table, 3-index%4]-1;
			//and the sign, please
			int sign = 1-(index/4);

			if(sign != 0)
				tabVal=tabVal+1;
			int elevenTabVal = tabVal*8;

			if(mul!=0)
				elevenTabVal*=mul;
			else
				elevenTabVal/=8;

			if(sign != 0)
				elevenTabVal=-elevenTabVal;

			//calculate sum
			int elevenbits = elevenbase+elevenTabVal;

			//clamp..
			if(elevenbits>=256*8)
				elevenbits=256*8-1;
			else if(elevenbits<0)
				elevenbits=0;
			//elevenbits now contains the 11 bit alpha value as defined in the spec.

			//extend to 16 bits before returning, since we don't have any good 11-bit file formats.
			UInt16 sixteenbits = (UInt16)((elevenbits << 5) + (elevenbits >> 6));

			return sixteenbits;
		}

		static byte GETBITSHIGH(uint source, int size, int startpos)
		{
			return (byte) (((source) >> (((startpos) - 32) - (size) + 1)) & ((1 << (size)) - 1));
		}

		// Decompress an ETC1 block (or ETC2 using individual or differential mode).
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		void decompressBlockDiffFlipC(uint blockPart1, uint blockPart2, byte[] img, uint width, uint height, uint startx, uint starty, int channels)
		{
			var avg_color = new byte[3];
			var enc_color1 = new byte[3]; 
			var enc_color2 = new byte[3];
			var diff = new sbyte[3];
			int table;
			int index,shift;
			int r,g,b;
			int diffbit;
			int flipbit;

			diffbit = (GETBITSHIGH(blockPart1, 1, 33));
			flipbit = (GETBITSHIGH(blockPart1, 1, 32));

			if( diffbit == 0 )
			{
				// We have diffbit = 0.

				// First decode left part of block.
				avg_color[0]= GETBITSHIGH(blockPart1, 4, 63);
				avg_color[1]= GETBITSHIGH(blockPart1, 4, 55);
				avg_color[2]= GETBITSHIGH(blockPart1, 4, 47);

				// Here, we should really multiply by 17 instead of 16. This can
				// be done by just copying the four lower bits to the upper ones
				// while keeping the lower bits.
				avg_color[0] |= (byte) (avg_color[0] <<4);
				avg_color[1] |= (byte) (avg_color[1] <<4);
				avg_color[2] |= (byte) (avg_color[2] <<4);

				table = GETBITSHIGH(blockPart1, 3, 39) << 1;

				uint pixel_indices_MSB, pixel_indices_LSB;

				pixel_indices_MSB = GETBITSui(blockPart2, 16, 31);
				pixel_indices_LSB = GETBITSui(blockPart2, 16, 15);

				if( (flipbit) == 0 )
				{
					// We should not flip
					shift = 0;
					for(uint x=startx; x<startx+2; x++)
					{
						for(uint y=starty; y<starty+4; y++)
						{
							index = (int)(((pixel_indices_MSB >> shift) & 1) << 1);
							index |= (int) ((pixel_indices_LSB >> shift) & 1);
							shift++;
							index=UNSCRAMBLE[index];

							r=RED_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[0]+COMPRESS_PARAMS[table,index],255));
							g=GREEN_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[1]+COMPRESS_PARAMS[table,index],255));
							b=BLUE_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[2]+COMPRESS_PARAMS[table,index],255));
						}
					}
				}
				else
				{
					// We should flip
					shift = 0;
					for(uint x=startx; x<startx+4; x++)
					{
						for(uint y=starty; y<starty+2; y++)
						{
							index = (int)(((pixel_indices_MSB >> shift) & 1) << 1);
							index |= (int) ((pixel_indices_LSB >> shift) & 1);
							shift++;
							index=UNSCRAMBLE[index];

							r=RED_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[0]+COMPRESS_PARAMS[table,index],255));
							g=GREEN_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[1]+COMPRESS_PARAMS[table,index],255));
							b=BLUE_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[2]+COMPRESS_PARAMS[table,index],255));
						}
						shift+=2;
					}
				}

				// Now decode other part of block. 
				avg_color[0]= GETBITSHIGH(blockPart1, 4, 59);
				avg_color[1]= GETBITSHIGH(blockPart1, 4, 51);
				avg_color[2]= GETBITSHIGH(blockPart1, 4, 43);

				// Here, we should really multiply by 17 instead of 16. This can
				// be done by just copying the four lower bits to the upper ones
				// while keeping the lower bits.
				avg_color[0] |= (byte) (avg_color[0] <<4);
				avg_color[1] |= (byte) (avg_color[1] <<4);
				avg_color[2] |= (byte) (avg_color[2] <<4);

				table = GETBITSHIGH(blockPart1, 3, 36) << 1;
				pixel_indices_MSB = GETBITSui(blockPart2, 16, 31);
				pixel_indices_LSB = GETBITSui(blockPart2, 16, 15);

				if( (flipbit) == 0 )
				{
					// We should not flip
					shift=8;
					for(uint x=startx+2; x<startx+4; x++)
					{
						for(uint y=starty; y<starty+4; y++)
						{
							index = (int)(((pixel_indices_MSB >> shift) & 1) << 1);
							index |= (int) ((pixel_indices_LSB >> shift) & 1);
							shift++;
							index=UNSCRAMBLE[index];

							r=RED_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[0]+COMPRESS_PARAMS[table,index],255));
							g=GREEN_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[1]+COMPRESS_PARAMS[table,index],255));
							b=BLUE_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[2]+COMPRESS_PARAMS[table,index],255));
						}
					}
				}
				else
				{
					// We should flip
					shift=2;
					for(uint x=startx; x<startx+4; x++)
					{
						for(uint y=starty+2; y<starty+4; y++)
						{
							index = (int)(((pixel_indices_MSB >> shift) & 1) << 1);
							index |= (int) ((pixel_indices_LSB >> shift) & 1);
							shift++;
							index=UNSCRAMBLE[index];

							r=RED_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[0]+COMPRESS_PARAMS[table,index],255));
							g=GREEN_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[1]+COMPRESS_PARAMS[table,index],255));
							b=BLUE_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[2]+COMPRESS_PARAMS[table,index],255));
						}
						shift += 2;
					}
				}
			}
			else
			{
				// We have diffbit = 1. 

				//      63 62 61 60 59 58 57 56 55 54 53 52 51 50 49 48 47 46 45 44 43 42 41 40 39 38 37 36 35 34  33  32 
				//      ---------------------------------------------------------------------------------------------------
				//     | base col1    | dcol 2 | base col1    | dcol 2 | base col 1   | dcol 2 | table  | table  |diff|flip|
				//     | R1' (5 bits) | dR2    | G1' (5 bits) | dG2    | B1' (5 bits) | dB2    | cw 1   | cw 2   |bit |bit |
				//      ---------------------------------------------------------------------------------------------------
				// 
				// 
				//     c) bit layout in bits 31 through 0 (in both cases)
				// 
				//      31 30 29 28 27 26 25 24 23 22 21 20 19 18 17 16 15 14 13 12 11 10  9  8  7  6  5  4  3   2   1  0
				//      --------------------------------------------------------------------------------------------------
				//     |       most significant pixel index bits       |         least significant pixel index bits       |  
				//     | p| o| n| m| l| k| j| i| h| g| f| e| d| c| b| a| p| o| n| m| l| k| j| i| h| g| f| e| d| c | b | a |
				//      --------------------------------------------------------------------------------------------------      

				// First decode left part of block.
				enc_color1[0]= GETBITSHIGH(blockPart1, 5, 63);
				enc_color1[1]= GETBITSHIGH(blockPart1, 5, 55);
				enc_color1[2]= GETBITSHIGH(blockPart1, 5, 47);

				// Expand from 5 to 8 bits
				avg_color [0] = (byte)((enc_color1 [0] << 3) | (enc_color1 [0] >> 2));
				avg_color [1] = (byte)((enc_color1 [1] << 3) | (enc_color1 [1] >> 2));
				avg_color [2] = (byte)((enc_color1 [2] << 3) | (enc_color1 [2] >> 2));

				table = GETBITSHIGH(blockPart1, 3, 39) << 1;

				uint pixel_indices_MSB, pixel_indices_LSB;

				pixel_indices_MSB = GETBITSui(blockPart2, 16, 31);
				pixel_indices_LSB = GETBITSui(blockPart2, 16, 15);

				if( (flipbit) == 0 )
				{
					// We should not flip
					shift = 0;
					for(uint x=startx; x<startx+2; x++)
					{
						for(uint y=starty; y<starty+4; y++)
						{
							index = (int)(((pixel_indices_MSB >> shift) & 1) << 1);
							index |= (int) ((pixel_indices_LSB >> shift) & 1);
							shift++;
							index=UNSCRAMBLE[index];

							r=RED_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[0]+COMPRESS_PARAMS[table,index],255));
							g=GREEN_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[1]+COMPRESS_PARAMS[table,index],255));
							b=BLUE_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[2]+COMPRESS_PARAMS[table,index],255));
						}
					}
				}
				else
				{
					// We should flip
					shift = 0;
					for(uint x=startx; x<startx+4; x++)
					{
						for(uint y=starty; y<starty+2; y++)
						{
							index = (int)(((pixel_indices_MSB >> shift) & 1) << 1);
							index |= (int) ((pixel_indices_LSB >> shift) & 1);
							shift++;
							index=UNSCRAMBLE[index];

							r=RED_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[0]+COMPRESS_PARAMS[table,index],255));
							g=GREEN_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[1]+COMPRESS_PARAMS[table,index],255));
							b=BLUE_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[2]+COMPRESS_PARAMS[table,index],255));
						}
						shift+=2;
					}
				}

				// Now decode right part of block. 
				diff [0] = (sbyte)GETBITSHIGH (blockPart1, 3, 58);
				diff [1] = (sbyte)GETBITSHIGH (blockPart1, 3, 50);
				diff [2] = (sbyte)GETBITSHIGH (blockPart1, 3, 42);

				// Extend sign bit to entire byte. 
				diff [0] = (sbyte)(diff [0] << 5);
				diff [1] = (sbyte)(diff [1] << 5);
				diff [2] = (sbyte)(diff [2] << 5);
				diff [0] = (sbyte)(diff [0] >> 5);
				diff [1] = (sbyte)(diff [1] >> 5);
				diff [2] = (sbyte)(diff [2] >> 5);

				//  Calculale second color
				enc_color2 [0] = (byte)(enc_color1 [0] + diff [0]);
				enc_color2 [1] = (byte)(enc_color1 [1] + diff [1]);
				enc_color2 [2] = (byte)(enc_color1 [2] + diff [2]);

				// Expand from 5 to 8 bits
				avg_color [0] = (byte)((enc_color2 [0] << 3) | (enc_color2 [0] >> 2));
				avg_color [1] = (byte)((enc_color2 [1] << 3) | (enc_color2 [1] >> 2));
				avg_color [2] = (byte)((enc_color2 [2] << 3) | (enc_color2 [2] >> 2));

				table = GETBITSHIGH(blockPart1, 3, 36) << 1;
				pixel_indices_MSB = GETBITSui(blockPart2, 16, 31);
				pixel_indices_LSB = GETBITSui(blockPart2, 16, 15);

				if( (flipbit) == 0 )
				{
					// We should not flip
					shift=8;
					for(uint x=startx+2; x<startx+4; x++)
					{
						for(uint y=starty; y<starty+4; y++)
						{
							index = (int)(((pixel_indices_MSB >> shift) & 1) << 1);
							index |= (int) ((pixel_indices_LSB >> shift) & 1);
							shift++;
							index=UNSCRAMBLE[index];

							r=RED_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[0]+COMPRESS_PARAMS[table,index],255));
							g=GREEN_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[1]+COMPRESS_PARAMS[table,index],255));
							b=BLUE_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[2]+COMPRESS_PARAMS[table,index],255));
						}
					}
				}
				else
				{
					// We should flip
					shift=2;
					for(uint x=startx; x<startx+4; x++)
					{
						for(uint y=starty+2; y<starty+4; y++)
						{
							index = (int)(((pixel_indices_MSB >> shift) & 1) << 1);
							index |= (int) ((pixel_indices_LSB >> shift) & 1);
							shift++;
							index=UNSCRAMBLE[index];

							r=RED_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[0]+COMPRESS_PARAMS[table,index],255));
							g=GREEN_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[1]+COMPRESS_PARAMS[table,index],255));
							b=BLUE_CHANNEL(img,width,x,y,channels, CLAMPi(0,avg_color[2]+COMPRESS_PARAMS[table,index],255));
						}
						shift += 2;
					}
				}
			}
		}

		// Decompress the planar mode.
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		void decompressBlockPlanar57c(uint compressed57_1, uint compressed57_2, byte[] img, uint width, uint height, uint startx, uint starty, int channels)
		{
			var colorO = new byte[3];
			var colorH = new byte[3];
			var colorV = new byte[3];

			colorO [0] = (byte)GETBITSHIGH (compressed57_1, 6, 63);
			colorO [1] = (byte)GETBITSHIGH (compressed57_1, 7, 57);
			colorO [2] = (byte)GETBITSHIGH (compressed57_1, 6, 50);
			colorH [0] = (byte)GETBITSHIGH (compressed57_1, 6, 44);
			colorH [1] = (byte)GETBITSHIGH (compressed57_1, 7, 38);
			colorH[2] = GETBITSbui(     compressed57_2, 6, 31);
			colorV[0] = GETBITSbui(     compressed57_2, 6, 25);
			colorV[1] = GETBITSbui(     compressed57_2, 7, 19);
			colorV[2] = GETBITSbui(     compressed57_2, 6, 12);

			colorO [0] = (byte)((colorO [0] << 2) | (colorO [0] >> 4));
			colorO [1] = (byte)((colorO [1] << 1) | (colorO [1] >> 6));
			colorO [2] = (byte)((colorO [2] << 2) | (colorO [2] >> 4));

			colorH [0] = (byte)((colorH [0] << 2) | (colorH [0] >> 4));
			colorH [1] = (byte)((colorH [1] << 1) | (colorH [1] >> 6));
			colorH [2] = (byte)((colorH [2] << 2) | (colorH [2] >> 4));

			colorV [0] = (byte)((colorV [0] << 2) | (colorV [0] >> 4));
			colorV [1] = (byte)((colorV [1] << 1) | (colorV [1] >> 6));
			colorV [2] = (byte)((colorV [2] << 2) | (colorV [2] >> 4));

			int xx, yy;

			for( xx=0; xx<4; xx++)
			{
				for( yy=0; yy<4; yy++)
				{
					img [channels * width * (starty + yy) + channels * (startx + xx) + 0] = CLAMPb (0, ((xx * (colorH [0] - colorO [0]) + yy * (colorV [0] - colorO [0]) + 4 * colorO [0] + 2) >> 2), 255);
					img [channels * width * (starty + yy) + channels * (startx + xx) + 1] = CLAMPb (0, ((xx * (colorH [1] - colorO [1]) + yy * (colorV [1] - colorO [1]) + 4 * colorO [1] + 2) >> 2), 255);
					img [channels * width * (starty + yy) + channels * (startx + xx) + 2] = CLAMPb (0, ((xx * (colorH [2] - colorO [2]) + yy * (colorV [2] - colorO [2]) + 4 * colorO [2] + 2) >> 2), 255);

					//Equivalent method
					/*img[channels*width*(starty+yy) + channels*(startx+xx) + 0] = (int)CLAMP(0, JAS_ROUND((xx*(colorH[0]-colorO[0])/4.0 + yy*(colorV[0]-colorO[0])/4.0 + colorO[0])), 255);
			img[channels*width*(starty+yy) + channels*(startx+xx) + 1] = (int)CLAMP(0, JAS_ROUND((xx*(colorH[1]-colorO[1])/4.0 + yy*(colorV[1]-colorO[1])/4.0 + colorO[1])), 255);
			img[channels*width*(starty+yy) + channels*(startx+xx) + 2] = (int)CLAMP(0, JAS_ROUND((xx*(colorH[2]-colorO[2])/4.0 + yy*(colorV[2]-colorO[2])/4.0 + colorO[2])), 255);*/

				}
			}
		}

		// Decompress an H-mode block 
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		void decompressBlockTHUMB58Hc(uint blockPart1, uint blockPart2, byte[] img, uint width, uint height, uint startx, uint starty, int channels)
		{
			uint col0, col1;
			var colors = new byte[2,3];
			var colorsRGB444 = new byte[2,3];
			var paint_colors = new byte[4,3];
			byte distance;
			var block_mask = new byte[4,4];

			// First decode left part of block.
			colorsRGB444 [0, R_COLOUR_COMPONENT] = (byte)GETBITSHIGH (blockPart1, 4, 57);
			colorsRGB444 [0, G_COLOUR_COMPONENT] = (byte)GETBITSHIGH (blockPart1, 4, 53);
			colorsRGB444 [0, B_COLOUR_COMPONENT] = (byte)GETBITSHIGH (blockPart1, 4, 49);

			colorsRGB444 [1, R_COLOUR_COMPONENT] = (byte)GETBITSHIGH (blockPart1, 4, 45);
			colorsRGB444 [1, G_COLOUR_COMPONENT] = (byte)GETBITSHIGH (blockPart1, 4, 41);
			colorsRGB444 [1, B_COLOUR_COMPONENT] = (byte)GETBITSHIGH (blockPart1, 4, 37);

			distance = 0;
			distance = (byte)((GETBITSHIGH (blockPart1, 2, 33)) << 1);

			col0 = GETBITSHIGH(blockPart1, 12, 57);
			col1 = GETBITSHIGH(blockPart1, 12, 45);

			if(col0 >= col1)
			{
				distance |= 1;
			}

			const int R_BITS58H = 4;
			const int G_BITS58H = 4;
			const int B_BITS58H = 4;

			// Extend the two colors to RGB888	
			decompressColor(R_BITS58H, G_BITS58H, B_BITS58H, colorsRGB444, colors);	

			calculatePaintColors58H (distance, (byte)BlockPattern.H, colors, paint_colors);

			// Choose one of the four paint colors for each texel
			for (int x = 0; x < BLOCKWIDTH; ++x) 
			{
				for (int y = 0; y < BLOCKHEIGHT; ++y) 
				{
					//block_mask[x][y] = GETBITS(block_part2,2,31-(y*4+x)*2);
					block_mask [x, y] = (byte)(GETBITSbui(blockPart2, 1, (y + x * 4) + 16) << 1);
					block_mask[x, y] |= GETBITSbui(blockPart2,1,(y+x*4));
					img[channels*((starty+y)*width+startx+x)+R_COLOUR_COMPONENT] =
						CLAMPb(0,paint_colors[block_mask[x,y],R_COLOUR_COMPONENT],255); // RED
					img[channels*((starty+y)*width+startx+x)+G_COLOUR_COMPONENT] =
						CLAMPb(0,paint_colors[block_mask[x,y],G_COLOUR_COMPONENT],255); // GREEN
					img[channels*((starty+y)*width+startx+x)+B_COLOUR_COMPONENT] =
						CLAMPb(0,paint_colors[block_mask[x,y],B_COLOUR_COMPONENT],255); // BLUE
				}
			}
		}

		// The format stores the bits for the three extra modes in a roundabout way to be able to
		// fit them without increasing the bit rate. This function converts them into something
		// that is easier to work with. 
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		void unstuff57bits(uint planarWord1, uint planarWord2, out uint planar57Word1, out uint planar57Word2)
		{
			// Get bits from twotimer configuration for 57 bits
			// 
			// Go to this bit layout:
			//
			//      63 62 61 60 59 58 57 56 55 54 53 52 51 50 49 48 47 46 45 44 43 42 41 40 39 38 37 36 35 34 33 32 
			//      -----------------------------------------------------------------------------------------------
			//     |R0               |G01G02              |B01B02  ;B03     |RH1           |RH2|GH                 |
			//      -----------------------------------------------------------------------------------------------
			//
			//      31 30 29 28 27 26 25 24 23 22 21 20 19 18 17 16 15 14 13 12 11 10  9  8  7  6  5  4  3  2  1  0
			//      -----------------------------------------------------------------------------------------------
			//     |BH               |RV               |GV                  |BV                | not used          |   
			//      -----------------------------------------------------------------------------------------------
			//
			//  From this:
			// 
			//      63 62 61 60 59 58 57 56 55 54 53 52 51 50 49 48 47 46 45 44 43 42 41 40 39 38 37 36 35 34 33 32 
			//      ------------------------------------------------------------------------------------------------
			//     |//|R0               |G01|/|G02              |B01|/ // //|B02  |//|B03     |RH1           |df|RH2|
			//      ------------------------------------------------------------------------------------------------
			//
			//      31 30 29 28 27 26 25 24 23 22 21 20 19 18 17 16 15 14 13 12 11 10  9  8  7  6  5  4  3  2  1  0
			//      -----------------------------------------------------------------------------------------------
			//     |GH                  |BH               |RV               |GV                   |BV              |
			//      -----------------------------------------------------------------------------------------------
			//
			//      63 62 61 60 59 58 57 56 55 54 53 52 51 50 49 48 47 46 45 44 43 42 41 40 39 38 37 36 35 34  33  32 
			//      ---------------------------------------------------------------------------------------------------
			//     | base col1    | dcol 2 | base col1    | dcol 2 | base col 1   | dcol 2 | table  | table  |diff|flip|
			//     | R1' (5 bits) | dR2    | G1' (5 bits) | dG2    | B1' (5 bits) | dB2    | cw 1   | cw 2   |bit |bit |
			//      ---------------------------------------------------------------------------------------------------

			byte RO, GO1, GO2, BO1, BO2, BO3, RH1, RH2, GH, BH, RV, GV, BV;

			RO = (byte)GETBITSHIGH (planarWord1, 6, 62);
			GO1 = (byte)GETBITSHIGH (planarWord1, 1, 56);
			GO2 = (byte)GETBITSHIGH (planarWord1, 6, 54);
			BO1 = (byte)GETBITSHIGH (planarWord1, 1, 48);
			BO2 = (byte)GETBITSHIGH (planarWord1, 2, 44);
			BO3 = (byte)GETBITSHIGH (planarWord1, 3, 41);
			RH1 = (byte)GETBITSHIGH (planarWord1, 5, 38);
			RH2 = (byte)GETBITSHIGH (planarWord1, 1, 32);
			GH  = GETBITSbui(     planarWord2, 7, 31);
			BH  = GETBITSbui(     planarWord2, 6, 24);
			RV  = GETBITSbui(     planarWord2, 6, 18);
			GV  = GETBITSbui(     planarWord2, 7, 12);
			BV  = GETBITSbui(     planarWord2, 6,  5);

			planar57Word1 = 0; planar57Word2 = 0;
			PUTBITSHIGHub(ref planar57Word1, RO,  6, 63);
			PUTBITSHIGHub(ref planar57Word1, GO1, 1, 57);
			PUTBITSHIGHub(ref planar57Word1, GO2, 6, 56);
			PUTBITSHIGHub(ref planar57Word1, BO1, 1, 50);
			PUTBITSHIGHub(ref planar57Word1, BO2, 2, 49);
			PUTBITSHIGHub(ref planar57Word1, BO3, 3, 47);
			PUTBITSHIGHub(ref planar57Word1, RH1, 5, 44);
			PUTBITSHIGHub(ref planar57Word1, RH2, 1, 39);
			PUTBITSHIGHub(ref planar57Word1, GH, 7, 38);
			PUTBITS(ref     planar57Word2, BH, 6, 31);
			PUTBITS(ref     planar57Word2, RV, 6, 25);
			PUTBITS(ref     planar57Word2, GV, 7, 19);
			PUTBITS(ref     planar57Word2, BV, 6, 12);
		}

		// The format stores the bits for the three extra modes in a roundabout way to be able to
		// fit them without increasing the bit rate. This function converts them into something
		// that is easier to work with. 
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		void unstuff58bits(uint thumbHWord1, uint thumbHWord2, out uint thumbH58Word1, out uint thumbH58Word2)
		{
			// Go to this layout:
			//
			//     |63 62 61 60 59 58|57 56 55 54 53 52 51|50 49|48 47 46 45 44 43 42 41 40 39 38 37 36 35 34 33|32   |
			//     |-------empty-----|part0---------------|part1|part2------------------------------------------|part3|
			//
			//  from this:
			// 
			//      63 62 61 60 59 58 57 56 55 54 53 52 51 50 49 48 47 46 45 44 43 42 41 40 39 38 37 36 35 34 33 32 
			//      --------------------------------------------------------------------------------------------------|
			//     |//|part0               |// // //|part1|//|part2                                          |df|part3|
			//      --------------------------------------------------------------------------------------------------|

			uint part0, part1, part2, part3;

			// move parts
			part0 = GETBITSHIGH( thumbHWord1, 7, 62);
			part1 = GETBITSHIGH( thumbHWord1, 2, 52);
			part2 = GETBITSHIGH( thumbHWord1,16, 49);
			part3 = GETBITSHIGH( thumbHWord1, 1, 32);
			thumbH58Word1 = 0;
			PUTBITSHIGHuui(ref thumbH58Word1, part0,  7, 57);
			PUTBITSHIGHuui(ref thumbH58Word1, part1,  2, 50);
			PUTBITSHIGHuui(ref thumbH58Word1, part2, 16, 48);
			PUTBITSHIGHuui(ref thumbH58Word1, part3,  1, 32);

			thumbH58Word2 = thumbHWord2;
		}

		// Decompress a T-mode block (simple packing)
		// Simple 59T packing:
		//|63 62 61 60 59|58 57 56 55|54 53 52 51|50 49 48 47|46 45 44 43|42 41 40 39|38 37 36 35|34 33 32|
		//|----empty-----|---red 0---|--green 0--|--blue 0---|---red 1---|--green 1--|--blue 1---|--dist--|
		//
		//|31 30 29 28 27 26 25 24 23 22 21 20 19 18 17 16 15 14 13 12 11 10 09 08 07 06 05 04 03 02 01 00|
		//|----------------------------------------index bits---------------------------------------------|
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		void decompressBlockTHUMB59Tc(uint blockPart1, uint blockPart2, byte[] img, uint width,uint height,uint startx,uint starty, int channels)
		{
			const int TABLE_BITS_59T = 3;
			const int R_BITS59T = 4;
			const int G_BITS59T = 4;
			const int B_BITS59T = 4;

			var colorsRGB444 = new byte[2,3];
			var colors = new byte[2,3];
			var paint_colors = new byte[4,3];
			byte distance;
			var block_mask = new byte[4,4];

			// First decode left part of block.
			colorsRGB444 [0, R_COLOUR_COMPONENT] = (byte)GETBITSHIGH (blockPart1, 4, 58);
			colorsRGB444 [0, G_COLOUR_COMPONENT] = (byte)GETBITSHIGH (blockPart1, 4, 54);
			colorsRGB444 [0, B_COLOUR_COMPONENT] = (byte)GETBITSHIGH (blockPart1, 4, 50);

			colorsRGB444 [1, R_COLOUR_COMPONENT] = (byte)GETBITSHIGH (blockPart1, 4, 46);
			colorsRGB444 [1, G_COLOUR_COMPONENT] = (byte)GETBITSHIGH (blockPart1, 4, 42);
			colorsRGB444 [1, B_COLOUR_COMPONENT] = (byte)GETBITSHIGH (blockPart1, 4, 38);

			distance = (byte)GETBITSHIGH (blockPart1, TABLE_BITS_59T, 34);

			// Extend the two colors to RGB888	
			decompressColor(R_BITS59T, G_BITS59T, B_BITS59T, colorsRGB444, colors);	
			calculatePaintColors59T (distance, (byte)BlockPattern.T, colors, paint_colors);

			// Choose one of the four paint colors for each texel
			for (int x = 0; x < BLOCKWIDTH; ++x) 
			{
				for (int y = 0; y < BLOCKHEIGHT; ++y) 
				{
					//block_mask[x][y] = GETBITS(block_part2,2,31-(y*4+x)*2);
					block_mask[x,y] = (byte)(GETBITSbui(blockPart2,1,(y+x*4)+16)<<1);
					block_mask[x,y] |= GETBITSbui(blockPart2,1,(y+x*4));
					img[channels*((starty+y)*width+startx+x)+R_COLOUR_COMPONENT] =
						CLAMPb(0,paint_colors[block_mask[x, y], R_COLOUR_COMPONENT],255); // RED
					img[channels*((starty+y)*width+startx+x)+G_COLOUR_COMPONENT] =
						CLAMPb(0,paint_colors[block_mask[x, y], G_COLOUR_COMPONENT],255); // GREEN
					img[channels*((starty+y)*width+startx+x)+ B_COLOUR_COMPONENT] =
						CLAMPb(0,paint_colors[block_mask[x, y], B_COLOUR_COMPONENT],255); // BLUE
				}
			}
		}


		// The format stores the bits for the three extra modes in a roundabout way to be able to
		// fit them without increasing the bit rate. This function converts them into something
		// that is easier to work with. 
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		void unstuff59bits(UInt32 thumbTWord1, UInt32 thumbTWord2, out UInt32 thumbT59Word1, out UInt32 thumbT59Word2)
		{
			// Get bits from twotimer configuration 59 bits. 
			// 
			// Go to this bit layout:
			//
			//     |63 62 61 60 59|58 57 56 55|54 53 52 51|50 49 48 47|46 45 44 43|42 41 40 39|38 37 36 35|34 33 32|
			//     |----empty-----|---red 0---|--green 0--|--blue 0---|---red 1---|--green 1--|--blue 1---|--dist--|
			//
			//     |31 30 29 28 27 26 25 24 23 22 21 20 19 18 17 16 15 14 13 12 11 10 09 08 07 06 05 04 03 02 01 00|
			//     |----------------------------------------index bits---------------------------------------------|
			//
			//
			//  From this:
			// 
			//      63 62 61 60 59 58 57 56 55 54 53 52 51 50 49 48 47 46 45 44 43 42 41 40 39 38 37 36 35 34 33 32 
			//      -----------------------------------------------------------------------------------------------
			//     |// // //|R0a  |//|R0b  |G0         |B0         |R1         |G1         |B1          |da  |df|db|
			//      -----------------------------------------------------------------------------------------------
			//
			//     |31 30 29 28 27 26 25 24 23 22 21 20 19 18 17 16 15 14 13 12 11 10 09 08 07 06 05 04 03 02 01 00|
			//     |----------------------------------------index bits---------------------------------------------|
			//
			//      63 62 61 60 59 58 57 56 55 54 53 52 51 50 49 48 47 46 45 44 43 42 41 40 39 38 37 36 35 34 33 32 
			//      -----------------------------------------------------------------------------------------------
			//     | base col1    | dcol 2 | base col1    | dcol 2 | base col 1   | dcol 2 | table  | table  |df|fp|
			//     | R1' (5 bits) | dR2    | G1' (5 bits) | dG2    | B1' (5 bits) | dB2    | cw 1   | cw 2   |bt|bt|
			//      ------------------------------------------------------------------------------------------------


			// Fix middle part
			thumbT59Word1 = thumbTWord1 >> 1;
			// Fix db (lowest bit of d)
			PUTBITSHIGHuui(ref thumbT59Word1, thumbTWord1,  1, 32);
			// Fix R0a (top two bits of R0)
			byte R0a = (byte)GETBITSHIGH (thumbTWord1, 2, 60);
			PUTBITSHIGHub(ref thumbT59Word1, R0a,  2, 58);

			// Zero top part (not needed)
			PUTBITSHIGHub(ref thumbT59Word1, 0,  5, 63);

			thumbT59Word2 = thumbTWord2;
		}


		// Decompress an H-mode block with alpha
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		void decompressBlockTHUMB58HAlphaC(uint blockPart1, uint blockPart2, byte[] img, byte[] alpha, int alphaOffset, uint width, uint height, uint startx, uint starty, int channelsRGB)
		{
			uint col0, col1;
			var colors = new byte[2,3];
			var colorsRGB444 = new byte[2, 3];
			var paint_colors = new byte[4, 3];
			byte distance;
			var block_mask = new byte[4, 4];
			int channelsA;
			int secondaryOffset = alphaOffset;

			if(channelsRGB == 3)
			{
				// We will decode the alpha data to a separate memory area. 
				channelsA = 1;
			}
			else
			{
				// We will decode the RGB data and the alpha data to the same memory area, 
				// interleaved as RGBA. 
				channelsA = 4;
				alpha = img;
				secondaryOffset += 3;
			}

			// First decode left part of block.
			colorsRGB444[0, R_COLOUR_COMPONENT]= GETBITSHIGH(blockPart1, 4, 57);
			colorsRGB444[0, G_COLOUR_COMPONENT]= GETBITSHIGH(blockPart1, 4, 53);
			colorsRGB444[0, B_COLOUR_COMPONENT]= GETBITSHIGH(blockPart1, 4, 49);

			colorsRGB444[1, R_COLOUR_COMPONENT]= GETBITSHIGH(blockPart1, 4, 45);
			colorsRGB444[1, G_COLOUR_COMPONENT]= GETBITSHIGH(blockPart1, 4, 41);
			colorsRGB444[1, B_COLOUR_COMPONENT]= GETBITSHIGH(blockPart1, 4, 37);

			distance = 0;
			distance = (byte)((GETBITSHIGH (blockPart1, 2, 33)) << 1);

			col0 = GETBITSHIGH(blockPart1, 12, 57);
			col1 = GETBITSHIGH(blockPart1, 12, 45);

			if(col0 >= col1)
			{
				distance |= 1;
			}

			const int R_BITS58H = 4;
			const int G_BITS58H = 4;
			const int B_BITS58H = 4;

			// Extend the two colors to RGB888	
			decompressColor(R_BITS58H, G_BITS58H, B_BITS58H, colorsRGB444, colors);	

			calculatePaintColors58H (distance, (byte)BlockPattern.H, colors, paint_colors);

			// Choose one of the four paint colors for each texel
			for (byte x = 0; x < BLOCKWIDTH; ++x) 
			{
				for (byte y = 0; y < BLOCKHEIGHT; ++y) 
				{
					//block_mask[x][y] = GETBITS(block_part2,2,31-(y*4+x)*2);
					block_mask [x, y] = (byte)(GETBITSbui (blockPart2, 1, (y + x * 4) + 16) << 1);
					block_mask[x, y] |= GETBITSbui(blockPart2,1,(y+x*4));
					img[channelsRGB*((starty+y)*width+startx+x)+R_COLOUR_COMPONENT] =
						CLAMPb(0,paint_colors[block_mask[x, y],R_COLOUR_COMPONENT],255); // RED
					img[channelsRGB*((starty+y)*width+startx+x)+G_COLOUR_COMPONENT] =
						CLAMPb(0,paint_colors[block_mask[x, y], G_COLOUR_COMPONENT],255); // GREEN
					img[channelsRGB*((starty+y)*width+startx+x)+B_COLOUR_COMPONENT] =
						CLAMPb(0,paint_colors[block_mask[x, y], B_COLOUR_COMPONENT],255); // BLUE

					if(block_mask[x,y]==2)  
					{
						alpha[channelsA*(x+startx+(y+starty)*width)]=0;
						img[channelsRGB*((starty+y)*width+startx+x)+R_COLOUR_COMPONENT] =0;
						img[channelsRGB*((starty+y)*width+startx+x)+G_COLOUR_COMPONENT] =0;
						img[channelsRGB*((starty+y)*width+startx+x)+B_COLOUR_COMPONENT] =0;
					}
					else
						alpha[channelsA*(x+startx+(y+starty)*width)]=255;
				}
			}
		}

		// Decompress an ETC2 block with punchthrough alpha
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		void decompressBlockDifferentialWithAlphaC(uint blockPart1, uint blockPart2, byte[] img, byte[] alpha, int alphaOffset, uint width, uint height, uint startx, uint starty, int channelsRGB)
		{

			var avg_color = new byte[3];
			var enc_color1 = new byte[3];
			var enc_color2 = new byte[3];
			var diff = new sbyte[3];
			int table;
			int index,shift;
			int r,g,b;
			int diffbit;
			int flipbit;
			int channelsA;
			int secondaryOffset = alphaOffset;

			if(channelsRGB == 3)
			{
				// We will decode the alpha data to a separate memory area. 
				channelsA = 1;
			}
			else
			{
				// We will decode the RGB data and the alpha data to the same memory area, 
				// interleaved as RGBA. 
				channelsA = 4;
				alpha = img;
				secondaryOffset += 3;
			}

			//the diffbit now encodes whether or not the entire alpha channel is 255.
			diffbit = (GETBITSHIGH(blockPart1, 1, 33));
			flipbit = (GETBITSHIGH(blockPart1, 1, 32));

			// First decode left part of block.
			enc_color1[0]= GETBITSHIGH(blockPart1, 5, 63);
			enc_color1[1]= GETBITSHIGH(blockPart1, 5, 55);
			enc_color1[2]= GETBITSHIGH(blockPart1, 5, 47);

			// Expand from 5 to 8 bits
			avg_color [0] = (byte)((enc_color1 [0] << 3) | (enc_color1 [0] >> 2));
			avg_color [1] = (byte)((enc_color1 [1] << 3) | (enc_color1 [1] >> 2));
			avg_color [2] = (byte)((enc_color1 [2] << 3) | (enc_color1 [2] >> 2));

			table = GETBITSHIGH(blockPart1, 3, 39) << 1;

			uint pixel_indices_MSB, pixel_indices_LSB;

			pixel_indices_MSB = GETBITSui(blockPart2, 16, 31);
			pixel_indices_LSB = GETBITSui(blockPart2, 16, 15);

			if( (flipbit) == 0 )
			{
				// We should not flip
				shift = 0;
				for(uint x=startx; x<startx+2; x++)
				{
					for(uint y=starty; y<starty+4; y++)
					{
						index = (int)(((pixel_indices_MSB >> shift) & 1) << 1);
						index |= (int) ((pixel_indices_LSB >> shift) & 1);
						shift++;
						index=UNSCRAMBLE[index];

						int mod = COMPRESS_PARAMS[table, index];
						if(diffbit==0&&(index==1||index==2)) 
						{
							mod=0;
						}

						r=RED_CHANNEL(img,width,x,y,channelsRGB, CLAMPi(0,avg_color[0]+mod,255));
						g=GREEN_CHANNEL(img,width,x,y,channelsRGB, CLAMPi(0,avg_color[1]+mod,255));
						b=BLUE_CHANNEL(img,width,x,y,channelsRGB, CLAMPi(0,avg_color[2]+mod,255));
						if(diffbit==0&&index==1) 
						{
							alpha[secondaryOffset + (y*width+x)*channelsA]=0;
							r=RED_CHANNEL(img,width,x,y,channelsRGB, 0);
							g=GREEN_CHANNEL(img,width,x,y,channelsRGB, 0);
							b=BLUE_CHANNEL(img,width,x,y,channelsRGB, 0);
						}
						else 
						{
							alpha[(y*width+x)*channelsA]=255;
						}

					}
				}
			}
			else
			{
				// We should flip
				shift = 0;
				for(uint x=startx; x<startx+4; x++)
				{
					for(uint y=starty; y<starty+2; y++)
					{
						index = (int)(((pixel_indices_MSB >> shift) & 1) << 1);
						index |= (int) ((pixel_indices_LSB >> shift) & 1);
						shift++;
						index=UNSCRAMBLE[index];
						int mod = COMPRESS_PARAMS[table, index];
						if(diffbit==0&&(index==1||index==2)) 
						{
							mod=0;
						}
						r=RED_CHANNEL(img,width,x,y,channelsRGB , CLAMPi(0,avg_color[0]+mod,255));
						g=GREEN_CHANNEL(img,width,x,y,channelsRGB, CLAMPi(0,avg_color[1]+mod,255));
						b=BLUE_CHANNEL(img,width,x,y,channelsRGB, CLAMPi(0,avg_color[2]+mod,255));
						if(diffbit==0&&index==1) 
						{
							alpha[secondaryOffset + (y*width+x)*channelsA]=0;
							r=RED_CHANNEL(img,width,x,y,channelsRGB, 0);
							g=GREEN_CHANNEL(img,width,x,y,channelsRGB, 0);
							b=BLUE_CHANNEL(img,width,x,y,channelsRGB, 0);
						}
						else 
						{
							alpha[secondaryOffset + (y*width+x)*channelsA]=255;
						}
					}
					shift+=2;
				}
			}
			// Now decode right part of block. 
			diff [0] = (sbyte)GETBITSHIGH (blockPart1, 3, 58);
			diff [1] = (sbyte)GETBITSHIGH (blockPart1, 3, 50);
			diff [2] = (sbyte)GETBITSHIGH (blockPart1, 3, 42);

			// Extend sign bit to entire byte. 
			diff [0] = (sbyte)(diff [0] << 5);
			diff [1] = (sbyte)(diff [1] << 5);
			diff [2] = (sbyte)(diff [2] << 5);
			diff [0] = (sbyte)(diff [0] >> 5);
			diff [1] = (sbyte)(diff [1] >> 5);
			diff [2] = (sbyte)(diff [2] >> 5);

			//  Calculate second color
			enc_color2 [0] = (byte)(enc_color1 [0] + diff [0]);
			enc_color2 [1] = (byte)(enc_color1 [1] + diff [1]);
			enc_color2 [2] = (byte)(enc_color1 [2] + diff [2]);

			// Expand from 5 to 8 bits
			avg_color [0] = (byte)((enc_color2 [0] << 3) | (enc_color2 [0] >> 2));
			avg_color [1] = (byte)((enc_color2 [1] << 3) | (enc_color2 [1] >> 2));
			avg_color [2] = (byte)((enc_color2 [2] << 3) | (enc_color2 [2] >> 2));

			table = GETBITSHIGH(blockPart1, 3, 36) << 1;
			pixel_indices_MSB = GETBITSui(blockPart2, 16, 31);
			pixel_indices_LSB = GETBITSui(blockPart2, 16, 15);

			if( (flipbit) == 0 )
			{
				// We should not flip
				shift=8;
				for(uint x=startx+2; x<startx+4; x++)
				{
					for(uint y=starty; y<starty+4; y++)
					{
						index = (int)(((pixel_indices_MSB >> shift) & 1) << 1);
						index |= (int) ((pixel_indices_LSB >> shift) & 1);
						shift++;
						index=UNSCRAMBLE[index];
						int mod = COMPRESS_PARAMS[table,index];
						if(diffbit==0&&(index==1||index==2)) 
						{
							mod=0;
						}

						r=RED_CHANNEL(img,width,x,y,channelsRGB, CLAMPi(0,avg_color[0]+mod,255));
						g=GREEN_CHANNEL(img,width,x,y,channelsRGB, CLAMPi(0,avg_color[1]+mod,255));
						b=BLUE_CHANNEL(img,width,x,y,channelsRGB, CLAMPi(0,avg_color[2]+mod,255));
						if(diffbit==0&&index==1) 
						{
							alpha[secondaryOffset + (y*width+x)*channelsA]=0;
							r=RED_CHANNEL(img,width,x,y,channelsRGB, 0);
							g=GREEN_CHANNEL(img,width,x,y,channelsRGB, 0);
							b=BLUE_CHANNEL(img,width,x,y,channelsRGB, 0);
						}
						else 
						{
							alpha[secondaryOffset + (y*width+x)*channelsA]=255;
						}
					}
				}
			}
			else
			{
				// We should flip
				shift=2;
				for(uint x=startx; x<startx+4; x++)
				{
					for(uint y=starty+2; y<starty+4; y++)
					{
						index = (int)(((pixel_indices_MSB >> shift) & 1) << 1);
						index |= (int) ((pixel_indices_LSB >> shift) & 1);
						shift++;
						index=UNSCRAMBLE[index];
						int mod = COMPRESS_PARAMS[table,index];
						if(diffbit==0&&(index==1||index==2)) 
						{
							mod=0;
						}

						r=RED_CHANNEL(img,width,x,y,channelsRGB, CLAMPi(0,avg_color[0]+mod,255));
						g=GREEN_CHANNEL(img,width,x,y,channelsRGB, CLAMPi(0,avg_color[1]+mod,255));
						b=BLUE_CHANNEL(img,width,x,y,channelsRGB, CLAMPi(0,avg_color[2]+mod,255));
						if(diffbit==0&&index==1) 
						{
							alpha[secondaryOffset + (y*width+x)*channelsA]=0;
							r=RED_CHANNEL(img,width,x,y,channelsRGB, 0);
							g=GREEN_CHANNEL(img,width,x,y,channelsRGB, 0);
							b=BLUE_CHANNEL(img,width,x,y,channelsRGB, 0);
						}
						else 
						{
							alpha[secondaryOffset + (y*width+x)*channelsA]=255;
						}
					}
					shift += 2;
				}
			}
		}


		// similar to regular decompression, but alpha channel is set to 0 if pixel index is 2, otherwise 255.
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		void decompressBlockTHUMB59TAlphaC(uint blockPart1, uint blockPart2, byte[] img, byte[] alpha, int alphaOffset, uint width, uint height, uint startx, uint starty, int channelsRGB)
		{

			var colorsRGB444 = new byte[2,3];
			var colors = new byte[2, 3];
			var paint_colors = new byte[4, 3];
			byte distance;
			var block_mask = new byte[4, 4];
			int channelsA;
			int secondaryOffset = alphaOffset;

			if(channelsRGB == 3)
			{
				// We will decode the alpha data to a separate memory area. 
				channelsA = 1;
			}
			else
			{
				// We will decode the RGB data and the alpha data to the same memory area, 
				// interleaved as RGBA. 
				channelsA = 4;
				alpha = img;
				secondaryOffset += 3;
			}

			// First decode left part of block.
			colorsRGB444[0,R_COLOUR_COMPONENT]= GETBITSHIGH(blockPart1, 4, 58);
			colorsRGB444[0,G_COLOUR_COMPONENT]= GETBITSHIGH(blockPart1, 4, 54);
			colorsRGB444[0,B_COLOUR_COMPONENT]= GETBITSHIGH(blockPart1, 4, 50);

			colorsRGB444[1,R_COLOUR_COMPONENT]= GETBITSHIGH(blockPart1, 4, 46);
			colorsRGB444[1,G_COLOUR_COMPONENT]= GETBITSHIGH(blockPart1, 4, 42);
			colorsRGB444[1,B_COLOUR_COMPONENT]= GETBITSHIGH(blockPart1, 4, 38);

			const int TABLE_BITS_59T = 3;
			distance   = GETBITSHIGH(blockPart1, TABLE_BITS_59T, 34);

			const byte R_BITS59T = 4;
			const byte G_BITS59T = 4;
			const byte B_BITS59T = 4;

			// Extend the two colors to RGB888	
			decompressColor(R_BITS59T, G_BITS59T, B_BITS59T, colorsRGB444, colors);	
			calculatePaintColors59T (distance, (byte)BlockPattern.T, colors, paint_colors);

			// Choose one of the four paint colors for each texel
			for (byte x = 0; x < BLOCKWIDTH; ++x) 
			{
				for (byte y = 0; y < BLOCKHEIGHT; ++y) 
				{
					//block_mask[x,y] = GETBITS(block_part2,2,31-(y*4+x)*2);
					block_mask [x, y] = (byte)(GETBITSbui (blockPart2, 1, (y + x * 4) + 16) << 1);
					block_mask[x,y] |= GETBITSbui(blockPart2,1,(y+x*4));
					img[channelsRGB*((starty+y)*width+startx+x)+ R_COLOUR_COMPONENT ] = 
						CLAMPb(0,paint_colors[block_mask[x,y],R_COLOUR_COMPONENT ],255); // RED
					img[channelsRGB*((starty+y)*width+startx+x)+G_COLOUR_COMPONENT] =
						CLAMPb(0,paint_colors[block_mask[x,y],G_COLOUR_COMPONENT ],255); // GREEN
					img[channelsRGB*((starty+y)*width+startx+x)+B_COLOUR_COMPONENT] =
						CLAMPb(0,paint_colors[block_mask[x,y],B_COLOUR_COMPONENT],255); // BLUE
					if(block_mask[x,y]==2)  
					{
						alpha[secondaryOffset + channelsA*(x+startx+(y+starty)*width)]=0;
						img[channelsRGB*((starty+y)*width+startx+x)+R_COLOUR_COMPONENT ] =0;
						img[channelsRGB*((starty+y)*width+startx+x)+G_COLOUR_COMPONENT] =0;
						img[channelsRGB*((starty+y)*width+startx+x)+B_COLOUR_COMPONENT] =0;
					}
					else
						alpha[secondaryOffset + channelsA*(x+startx+(y+starty)*width)]=255;
				}
			}
		}

		uint GETBITSui(uint source, int size, int startpos) 
		{
			return (uint)(((source) >> ((startpos) - (size) + 1)) & ((1 << (size)) - 1));
		}

		int[] UNSCRAMBLE = {2, 3, 1, 0};

		static int RED_CHANNEL(byte[] img, uint width, uint x, uint y, int channels, int value)
		{
			img [channels * (y * width + x) + 0] = (byte) value;
			return value;
		}

		static int GREEN_CHANNEL(byte[] img, uint width, uint x, uint y, int channels, int value)
		{
			img[channels*(y*width+x)+1] = (byte) value;
			return value;
		}

		static int BLUE_CHANNEL(byte[] img, uint width, uint x, uint y, int channels, int value)
		{
			img[channels*(y*width+x)+2] = (byte) value;
			return value;
		}

		static int CLAMPi(int ll, int x, int ul)
		{
			return (x < ll) ? ll : ((x > ul) ? ul : x);
		}

		int[,] COMPRESS_PARAMS = {{-8, -2,  2, 8}, {-8, -2,  2, 8}, {-17, -5, 5, 17}, {-17, -5, 5, 17}, {-29, -9, 9, 29}, {-29, -9, 9, 29}, {-42, -13, 13, 42}, {-42, -13, 13, 42}, {-60, -18, 18, 60}, {-60, -18, 18, 60}, {-80, -24, 24, 80}, {-80, -24, 24, 80}, {-106, -33, 33, 106}, {-106, -33, 33, 106}, {-183, -47, 47, 183}, {-183, -47, 47, 183}};

		static byte GETBITSbui(uint source, int size, int startpos) 
		{
			return (byte)(((source) >> ((startpos) - (size) + 1)) & ((1 << (size)) - 1));
		}

		static byte CLAMPb(int ll, int x, int ul)
		{
			return (byte)((x < ll) ? ll : ((x > ul) ? ul : x));
		}

		// Calculate the paint colors from the block colors 
		// using a distance d and one of the H- or T-patterns.
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		// byte[,] colors[2][3], byte[4][3] (possible_colors)
		void calculatePaintColors58H(byte d, byte p, byte[,] colors, byte[,] possibleColors) 
		{

			//			////////////////////////////////////////////
			//			
			//					C3      C1		C4----C1---C2
			//					|		|			  |
			//					|		|			  |
			//					|-------|			  |
			//					|		|			  |
			//					|		|			  |
			//					C4      C2			  C3
			//			
			//			////////////////////////////////////////////

			// C4
			possibleColors[3, R_COLOUR_COMPONENT] = CLAMPb(0,colors[1, R_COLOUR_COMPONENT] - TABLE_58H[d],255);
			possibleColors[3, G_COLOUR_COMPONENT] = CLAMPb(0,colors[1, G_COLOUR_COMPONENT] - TABLE_58H[d],255);
			possibleColors[3, B_COLOUR_COMPONENT] = CLAMPb(0,colors[1, B_COLOUR_COMPONENT] - TABLE_58H[d],255);

			if (p == (int) BlockPattern.H) 
			{ 
				// C1
				possibleColors[0, R_COLOUR_COMPONENT] = CLAMPb(0,colors[0, R_COLOUR_COMPONENT] + TABLE_58H[d],255);
				possibleColors[0, G_COLOUR_COMPONENT] = CLAMPb(0,colors[0, G_COLOUR_COMPONENT] + TABLE_58H[d],255);
				possibleColors[0, B_COLOUR_COMPONENT] = CLAMPb(0,colors[0, B_COLOUR_COMPONENT] + TABLE_58H[d],255);
				// C2
				possibleColors[1, R_COLOUR_COMPONENT] = CLAMPb(0,colors[0,R_COLOUR_COMPONENT] - TABLE_58H[d],255);
				possibleColors[1, G_COLOUR_COMPONENT] = CLAMPb(0,colors[0,G_COLOUR_COMPONENT] - TABLE_58H[d],255);
				possibleColors[1, B_COLOUR_COMPONENT] = CLAMPb(0,colors[0,B_COLOUR_COMPONENT] - TABLE_58H[d],255);
				// C3
				possibleColors[2, R_COLOUR_COMPONENT] = CLAMPb(0,colors[1, R_COLOUR_COMPONENT] + TABLE_58H[d],255);
				possibleColors[2, G_COLOUR_COMPONENT] = CLAMPb(0,colors[1, G_COLOUR_COMPONENT] + TABLE_58H[d],255);
				possibleColors[2, B_COLOUR_COMPONENT] = CLAMPb(0,colors[1, B_COLOUR_COMPONENT] + TABLE_58H[d],255);
			} 
			else 
			{
				throw new Exception("Invalid pattern. Terminating");
			}
		}

		//////////////////////////////////////////////
		//
		//		C3      C1		C4----C1---C2
		//		|		|			  |
		//		|		|			  |
		//		|-------|			  |
		//		|		|			  |
		//		|		|			  |
		//		C4      C2			  C3
		//
		//////////////////////////////////////////////
		// byte (colors)[2,3], uint8 (possible_colors)[4,3]
		void calculatePaintColors59T(byte d, byte p, byte[,] colors, byte[,] possibleColors) 
		{

			// C4
			possibleColors[3,R_COLOUR_COMPONENT] = CLAMPb(0,colors[1,R_COLOUR_COMPONENT] - TABLE_59T[d],255);
			possibleColors[3,G_COLOUR_COMPONENT] = CLAMPb(0,colors[1,G_COLOUR_COMPONENT] - TABLE_59T[d],255);
			possibleColors[3,B_COLOUR_COMPONENT] = CLAMPb(0,colors[1,B_COLOUR_COMPONENT] - TABLE_59T[d],255);

			if (p == (byte) BlockPattern.T) 
			{
				// C3
				possibleColors[0,R_COLOUR_COMPONENT] = colors[0,R_COLOUR_COMPONENT];
				possibleColors[0,G_COLOUR_COMPONENT] = colors[0,G_COLOUR_COMPONENT];
				possibleColors[0,B_COLOUR_COMPONENT] = colors[0,B_COLOUR_COMPONENT];
				// C2
				possibleColors[1,R_COLOUR_COMPONENT] = CLAMPb(0,colors[1,R_COLOUR_COMPONENT] + TABLE_59T[d],255);
				possibleColors[1,G_COLOUR_COMPONENT] = CLAMPb(0,colors[1,G_COLOUR_COMPONENT] + TABLE_59T[d],255);
				possibleColors[1,B_COLOUR_COMPONENT] = CLAMPb(0,colors[1,B_COLOUR_COMPONENT] + TABLE_59T[d],255);
				// C1
				possibleColors[2,R_COLOUR_COMPONENT] = colors[1,R_COLOUR_COMPONENT];
				possibleColors[2,G_COLOUR_COMPONENT] = colors[1,G_COLOUR_COMPONENT];
				possibleColors[2,B_COLOUR_COMPONENT] = colors[1,B_COLOUR_COMPONENT];

			} 
			else 
			{
				throw new Exception("Invalid pattern. Terminating");
			}
		}

		const int R_COLOUR_COMPONENT = 0;
		const int G_COLOUR_COMPONENT = 1;
		const int B_COLOUR_COMPONENT = 2;

		// The color bits are expanded to the full color
		// NO WARRANTY --- SEE STATEMENT IN TOP OF FILE (C) Ericsson AB 2013. All Rights Reserved.
		// byte[,] (colors_RGB444)[2][3]
		// byte[,] (colors)[2][3]
		void decompressColor(int rB, int gB, int bB, byte[,] colorsRGB444, byte[,] colors) 
		{
			// The color should be retrieved as:
			//
			// c = round(255/(r_bits^2-1))*comp_color
			//
			// This is similar to bit replication
			// 
			// Note -- this code only work for bit replication from 4 bits and up --- 3 bits needs
			// two copy operations.

			colors [0, R_COLOUR_COMPONENT] = (byte)((colorsRGB444 [0, R_COLOUR_COMPONENT] << (8 - rB)) | (colorsRGB444 [0, R_COLOUR_COMPONENT] >> (rB - (8 - rB))));
			colors [0, G_COLOUR_COMPONENT] = (byte)((colorsRGB444 [0, G_COLOUR_COMPONENT] << (8 - gB)) | (colorsRGB444 [0, G_COLOUR_COMPONENT] >> (gB - (8 - gB))));
			colors [0, B_COLOUR_COMPONENT] = (byte)((colorsRGB444 [0, B_COLOUR_COMPONENT] << (8 - bB)) | (colorsRGB444 [0, B_COLOUR_COMPONENT] >> (bB - (8 - bB))));
			colors [1, R_COLOUR_COMPONENT] = (byte)((colorsRGB444 [1, R_COLOUR_COMPONENT] << (8 - rB)) | (colorsRGB444 [1, R_COLOUR_COMPONENT] >> (rB - (8 - rB))));
			colors [1, G_COLOUR_COMPONENT] = (byte)((colorsRGB444 [1, G_COLOUR_COMPONENT] << (8 - gB)) | (colorsRGB444 [1, G_COLOUR_COMPONENT] >> (gB - (8 - gB))));
			colors [1, B_COLOUR_COMPONENT] = (byte)((colorsRGB444 [1, B_COLOUR_COMPONENT] << (8 - bB)) | (colorsRGB444 [1, B_COLOUR_COMPONENT] >> (bB - (8 - bB))));
		}

		void PUTBITS(ref uint dest, byte data, int size, int startpos)
		{
			int bitMask = MASK (size, startpos);
			int bitShift = SHIFT (size, startpos);

			dest = dest & ((uint) ~bitMask);
			dest |= (uint) ((data << bitShift) & bitMask);
		}

		void PUTBITSHIGHuui(ref UInt32 dest, UInt32 data, int size, int startpos) 
		{
			int mask  = MASKHIGH(size, startpos);
			int shift = SHIFTHIGH(size, startpos);

			dest =  dest & ((UInt32) (~mask));
			dest |= (UInt32) ((data << shift) & mask);
		}

		enum BlockPattern : int
		{
			H = 0, 
			T = 1
		};

		const int BLOCKWIDTH = 4;
		const int BLOCKHEIGHT = 4;

		static void PUTBITSHIGHub(ref UInt32 dest, byte data, int size, int startpos) 
		{
			int mask  = MASKHIGH(size, startpos);
			int shift = SHIFTHIGH(size, startpos);

			dest =  dest & ((UInt32) (~mask));
			dest |= (UInt32) ((data << shift) & mask);
		}

		private byte[] TABLE_58H = {3,6,11,16,23,32,41,64};  // 3-bit table for the 58 bit H-mode
		static int SHIFT(int size, int startpos)
		{
			return ((startpos)-(size)+1);
		}

		static int MASK(int size, int startpos)
		{
			return (((2<<(size-1))-1) << SHIFT(size,startpos));
		}

		private byte[] TABLE_59T = {3,6,11,16,23,32,41,64};  // 3-bit table for the 59 bit T-mode


		static int SHIFTHIGH(int size, int startpos)
		{
			return (((startpos)-32)-(size)+1);
		}

		static int MASKHIGH(int size, int startpos) 
		{
			return (((1<<(size))-1) << SHIFTHIGH(size,startpos));
		}

	}
}

