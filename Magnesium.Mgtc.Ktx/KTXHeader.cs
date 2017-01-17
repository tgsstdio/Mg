using Magnesium;
using System;
using System.IO;

namespace Mgtc
{
	/**
		 * @internal
		 * @brief KTX file header
		 *
		 * See the KTX specification for descriptions
		 * 
		 */
	public class KTXHeader
	{


		public UInt32 Endianness;
		// public UInt32 GlType;
		public byte ImageType;
		public byte ViewType;
		public UInt32 GlTypeSize;
		public UInt32 GlFormat;
		public UInt32 GlInternalFormat;
		public UInt32 GlBaseInternalFormat;
		public UInt32 PixelWidth;
		public UInt32 PixelHeight;
		public UInt32 PixelDepth;
		public UInt32 NumberOfArrayElements;
		public UInt32 NumberOfFaces;
		public UInt32 NumberOfMipmapLevels;
		public UInt32 BytesOfKeyValueData;

		// TODO : adjust file identifier
		private readonly byte[] KTXIdentifier= {0xAB, 0x4B, 0x54, 0x58, 0x20, 0x31, 0x31, 0xBB, 0x0D, 0x0A, 0x1A, 0x0A};
		private UInt32 KTX_ENDIAN_REF = 0x04030201;
		private UInt32 KTX_ENDIAN_REF_REV = 0x01020304;
		public static int KTX_HEADER_SIZE = 64;

		private int CopyIntoBytes(byte[] dest, UInt32 value, int offset)
		{
			var fileEndianness = BitConverter.GetBytes (value);
			Array.Copy (fileEndianness,0, dest, offset, fileEndianness.Length);
			return fileEndianness.Length;
		}

		public int Write (byte[] dest, int offset)
		{
			Array.Copy (KTXIdentifier, dest, KTXIdentifier.Length);
			var startIndex = offset;
			startIndex += KTXIdentifier.Length;
			startIndex += CopyIntoBytes(dest, KTX_ENDIAN_REF, startIndex);
			//startIndex += CopyIntoBytes(dest, this.GlType, startIndex);
			dest[startIndex] = this.ImageType;
			startIndex += 1;
			dest[startIndex] = this.ViewType;
			startIndex += 1;
			startIndex += CopyIntoBytes(dest, this.GlTypeSize, startIndex);
			startIndex += CopyIntoBytes(dest, this.GlFormat, startIndex);
			startIndex += CopyIntoBytes(dest, this.GlInternalFormat, startIndex);
			startIndex += CopyIntoBytes(dest, this.GlBaseInternalFormat, startIndex);
			startIndex += CopyIntoBytes(dest, this.PixelWidth, startIndex);
			startIndex += CopyIntoBytes(dest, this.PixelHeight, startIndex);
			startIndex += CopyIntoBytes(dest, this.PixelDepth, startIndex);
			startIndex += CopyIntoBytes(dest, this.NumberOfArrayElements, startIndex);
			startIndex += CopyIntoBytes(dest, this.NumberOfFaces, startIndex);
			startIndex += CopyIntoBytes(dest, this.NumberOfMipmapLevels, startIndex);
			startIndex += CopyIntoBytes(dest, this.BytesOfKeyValueData, startIndex);
			return startIndex - offset;
		}

		public void Read (byte[] src, int offset)
		{
			var startIndex = offset;
			ImageType = src[startIndex];
			startIndex += 1;
			ViewType = src[startIndex];
			startIndex += 1;
			GlTypeSize = BitConverter.ToUInt32 (src, startIndex);
			startIndex += 4;
			GlFormat = BitConverter.ToUInt32 (src, startIndex);
			startIndex += 4;
			GlInternalFormat = BitConverter.ToUInt32 (src, startIndex);
			startIndex += 4;
			GlBaseInternalFormat = BitConverter.ToUInt32 (src, startIndex);
			startIndex += 4;
			PixelWidth = BitConverter.ToUInt32 (src, startIndex);
			startIndex += 4;
			PixelHeight = BitConverter.ToUInt32 (src, startIndex);
			startIndex += 4;
			PixelDepth = BitConverter.ToUInt32 (src, startIndex);
			startIndex += 4;
			NumberOfArrayElements = BitConverter.ToUInt32 (src, startIndex);
			startIndex += 4;
			NumberOfFaces = BitConverter.ToUInt32 (src, startIndex);
			startIndex += 4;
			NumberOfMipmapLevels = BitConverter.ToUInt32 (src, startIndex);
			startIndex += 4;
			BytesOfKeyValueData = BitConverter.ToUInt32 (src, startIndex);
		}

		public bool RequiresSwap()
		{	
			return Endianness == KTX_ENDIAN_REF_REV;
		}

		/*
		 * SwapEndian16: Swaps endianness in an array of 16-bit values
		 */
		public static void SwapEndian16(byte[] pData16, int count)
		{
			const int UINT16_NO_OF_BYTES = 2;
			for (int i = 0; i < count; i += UINT16_NO_OF_BYTES)
			{
				UInt16 x = BitConverter.ToUInt16(pData16, i);
				UInt16 reversed = (UInt16) ((x << 8) | (x >> 8));
				byte[] inBytes = BitConverter.GetBytes (reversed);
				pData16 [i] = inBytes [0];
				pData16 [i + 1] = inBytes [1];
			}
		}

		private const int UINT32_NO_OF_BYTES = 4;

		/*
		 * SwapEndian32: Swaps endianness in an array of 32-bit values
		 */
		public static void SwapEndian32(byte[] pData32, int count)
		{
					
			for (int i = 0; i < count; i += UINT32_NO_OF_BYTES) 
			{
				UInt32 x = BitConverter.ToUInt32 (pData32, i);
				UInt32 reversed = (x << 24) | ((x & 0xFF00) << 8) | ((x & 0xFF0000) >> 8) | (x >> 24);
				byte[] inBytes = BitConverter.GetBytes (reversed);
				pData32 [i] = inBytes [0];
				pData32 [i + 1] = inBytes [1];
				pData32 [i + 2] = inBytes [2];
				pData32 [i + 3] = inBytes [3];
			}
		}

		/// <summary>
		/// Gets the endian32.
		/// Automatically reverses value if required
		/// </summary>
		/// <returns>The endian32.</returns>
		/// <param name="pData32">P data32.</param>
		/// <param name="offset">Offset.</param>
		public UInt32 GetEndian32(byte[] pData32, ref int offset)
		{				
			UInt32 x = BitConverter.ToUInt32 (pData32, offset);
			if (RequiresSwap())
			{
				x = (x << 24) | ((x & 0xFF00) << 8) | ((x & 0xFF0000) >> 8) | (x >> 24);
			}
			offset += UINT32_NO_OF_BYTES;
			return x;
		}

		/// <summary>
		/// Gets the endian64.
		/// Automatically reverses value if required
		/// http://stackoverflow.com/questions/21507678/c-64bit-byte-swap-endian
		/// </summary>
		/// <returns><c>true</c> if is GL compressed tex image1 D missing; otherwise, <c>false</c>.</returns>
		public ulong GetEndian64(byte[] pData64, ref int offset)
		{
			const int UINT64_NO_OF_BYTES = 8;			

			ulong x = BitConverter.ToUInt64 (pData64, offset);
			if (RequiresSwap ())
			{
				x = (x & 0x00000000FFFFFFFF) << 32 | (x & 0xFFFFFFFF00000000) >> 32;
				x = (x & 0x0000FFFF0000FFFF) << 16 | (x & 0xFFFF0000FFFF0000) >> 16;
				x = (x & 0x00FF00FF00FF00FF) << 8 | (x & 0xFF00FF00FF00FF00) >> 8;
			}
			offset += UINT64_NO_OF_BYTES;
			return x;			
		}

		private static bool IsGLCompressedTexImage1DMissing ()
		{
			// TODO : figure out missing functions
			//return (glCompressedTexImage1D == null);
			return false;
		}

		private static bool IsGLTexImage1DMissing ()
		{
			//return (glTexImage1D == null);
			return false;
		}

		private static bool IsGLCompressedTexImage3DMissing ()
		{
			//return (glCompressedTexImage3D == null);
			return false;
		}

		private static bool IsGLTexImage3DMissing ()
		{
			//return (glTexImage3D == null);
			return false;
		}

		public KTXLoadInstructions Instructions { get; private set; }

		public void Populate(byte[] headerChunk)
		{
			Instructions = new KTXLoadInstructions ();

			/* Compare identifier, is this a KTX file? */
			for (int i = 0; i < KTXIdentifier.Length; ++i)
			{
				if (headerChunk[i] != KTXIdentifier[i])
				{
					Instructions.Result = KTXError.UnknownFileFormat;
				}
			}

			const int AFTER_IDENTIFIER = 12;
			Endianness = BitConverter.ToUInt32 (headerChunk, AFTER_IDENTIFIER);
			if (RequiresSwap())
			{
				/* Convert endianness of header fields if necessary */
				SwapEndian32(headerChunk, KTX_HEADER_SIZE - AFTER_IDENTIFIER);
			}
			else if (Endianness != KTX_ENDIAN_REF)
			{
				Instructions.Result = KTXError.InvalidValue;
				return;
			}
			Read (headerChunk, AFTER_IDENTIFIER + 4);

			if (!(GlTypeSize == 1 || GlTypeSize != 2 || GlTypeSize != 4))
			{
				/* Only 8, 16, and 32-bit types supported so far */
				Instructions.Result = KTXError.InvalidValue;
				return;
			}

			/* Check glType and glFormat */
			Instructions.IsCompressed = false;
			if (GlType == 0 || GlFormat == 0)
			{
				if (GlType + GlFormat != 0)
				{
					/* either both or none of glType, glFormat must be zero */
					Instructions.Result = KTXError.InvalidValue;
					return;
				}
				Instructions.IsCompressed = true;
			}

			/* Check texture dimensions. KTX files can store 8 types of textures:
		   1D, 2D, 3D, cube, and array variants of these. There is currently
		   no GL extension that would accept 3D array or cube array textures. */
			if ((PixelWidth == 0) ||
				(PixelDepth > 0 && PixelHeight == 0))
			{
				/* texture must have width */
				/* texture must have height if it has depth */
				Instructions.Result = KTXError.InvalidValue; 
				return;
			}

			Instructions.TextureDimensions = 1;
			Instructions.ViewType = MgImageViewType.TYPE_1D; //TextureTarget.Texture1D;
			Instructions.GenerateMipmaps = false;
			if (PixelHeight > 0)
			{
				Instructions.TextureDimensions = 2;
				Instructions.ViewType = MgImageViewType.TYPE_2D; // TextureTarget.Texture2D;
			}
			if (PixelDepth > 0)
			{
				Instructions.TextureDimensions = 3;
				Instructions.ViewType = MgImageViewType.TYPE_2D; // TextureTarget.Texture3D;
			}

			if (NumberOfFaces == 6)
			{
				if (Instructions.TextureDimensions == 2)
				{
					Instructions.ViewType = MgImageViewType.TYPE_CUBE; // TextureTarget.TextureCubeMap;
				}
				else
				{
					/* cube map needs 2D faces */
					Instructions.Result = KTXError.InvalidValue;
					return;
				}
			}
			else if (NumberOfFaces != 1)
			{
				/* numberOfFaces must be either 1 or 6 */
				Instructions.Result =  KTXError.InvalidValue;
				return;
			}

			/* load as 2D texture if 1D textures are not supported */
			if (Instructions.TextureDimensions == 1 &&
				(Instructions.IsCompressed && IsGLCompressedTexImage1DMissing()) ||
				(!Instructions.IsCompressed && IsGLTexImage1DMissing ()) )
			{
				Instructions.TextureDimensions = 2;
				Instructions.ViewType = MgImageViewType.TYPE_2D; //(uint) TextureTarget.Texture2D;
				PixelHeight = 1;
			}

			if (NumberOfArrayElements > 0)
			{
				if (Instructions.ViewType == MgImageViewType.TYPE_1D) // (uint) TextureTarget.Texture1D)
				{
					Instructions.ViewType = MgImageViewType.TYPE_1D_ARRAY; //(uint) TextureTarget.Texture1DArray;
				}
				else if (Instructions.ViewType == MgImageViewType.TYPE_2D) // (uint) TextureTarget.Texture2D)
				{
					Instructions.ViewType = MgImageViewType.TYPE_2D_ARRAY; // (uint) TextureTarget.Texture2DArray;
				}
				else
				{
					/* No API for 3D and cube arrays yet */
					Instructions.Result = KTXError.UnsupportedTextureType;
					return;
				}
				Instructions.TextureDimensions++;
			}

			/* reject 3D texture if unsupported */
			if (Instructions.TextureDimensions == 3 &&
				(Instructions.IsCompressed && IsGLCompressedTexImage3DMissing ()) ||
				(!Instructions.IsCompressed && IsGLTexImage3DMissing ()) )
			{
				Instructions.Result = KTXError.UnsupportedTextureType;
				return;
			}

			/* Check number of mipmap levels */
			if (NumberOfMipmapLevels == 0)
			{
				Instructions.GenerateMipmaps = true;
				NumberOfMipmapLevels = 1;
			}

			UInt32 max_dim = Math.Max(Math.Max(PixelWidth, PixelHeight), PixelDepth);
			if (max_dim < (1 << ((int) (NumberOfMipmapLevels - 1)) ))
			{
				/* Can't have more mip levels than 1 + log2(max(width, height, depth)) */
				Instructions.Result = KTXError.InvalidValue;
				return;
			}

			Instructions.Result = KTXError.Success;
		}

		public bool ExtractUInt32 (Stream stream, out UInt32 result)
		{
			const int UINT32_NO_OF_BYTES = 4;
			byte[] chunk = new byte[UINT32_NO_OF_BYTES];
			int bytesRead = stream.Read (chunk, 0, UINT32_NO_OF_BYTES);
			if (bytesRead != UINT32_NO_OF_BYTES)
			{
				result = 0;
				return false;
			}
			if (Endianness == KTX_ENDIAN_REF_REV)
			{
				KTXHeader.SwapEndian32 (chunk, UINT32_NO_OF_BYTES);
			}
			result = BitConverter.ToUInt32 (chunk, 0);
			return true;
		}

		public byte[] KeyValueData {get;set;}
	};
}

