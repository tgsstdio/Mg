using System;
using System.IO;
using System.Collections.Generic;
using Magnesium;

namespace MonoGame.Textures.Ktx
{
	public class KTXTextureManager : IKTXTextureLoader
	{
		private readonly IMgTextureGenerator mTextureOptimizer;

		public KTXTextureManager(IMgTextureGenerator optimizer)
		{
			mTextureOptimizer = optimizer;
		}

		public MgTextureInfo[] Load(Stream fs)
		{
			var HEADER_SIZE = KTXHeader.KTX_HEADER_SIZE;
			var headerChunk = new byte[HEADER_SIZE];
			int count = fs.Read (headerChunk, 0, HEADER_SIZE);
			if (count != HEADER_SIZE)
			{
				throw new InvalidOperationException("KTX : Invalid header size ");
			}

			KTXHeader header;
			var status = ReadHeader (headerChunk, out header);
			if (status != KTXError.Success)
			{
				throw new InvalidOperationException("KTX: Header parsing error");
			}

			status = ReadKeyValueDataSection (fs, header.KeyValueData, (int)header.BytesOfKeyValueData);
			if (status != KTXError.Success)
			{
				throw new InvalidOperationException("KTX: invalid key value data");
			}

			//KeyValueArrayData[] inputData = GenerateKeyValueArray (destHeader);

			return LoadTexture(fs, header);
		}

		static KeyValueArrayData[] GenerateKeyValueArray (KTXHeader destHeader)
		{
			var output = new List<KeyValueArrayData> ();

			int offset = 0;
			do
			{
				var keyValue = new KeyValueArrayData ();
				var keyValueByteSize = destHeader.GetEndian32 (destHeader.KeyValueData, ref offset);
				keyValue.Id = destHeader.GetEndian64 (destHeader.KeyValueData, ref offset);
				keyValue.Offsets = new ulong[destHeader.NumberOfMipmapLevels];
				for (int j = 0; j < keyValue.Offsets.Length; ++j)
				{
					keyValue.Offsets [j] = destHeader.GetEndian64 (destHeader.KeyValueData, ref offset);
				}
				output.Add (keyValue);
			} 
			while(offset < destHeader.KeyValueData.Length);

			return output.ToArray();
		}

		static bool IsGenerateMipmapMissing ()
		{
			return false;
		}

		static KTXError ReadKeyValueDataSection (Stream stream, byte[] buffer, int count)
		{
			if (count <= 0)
			{
				return KTXError.Success;
			}

			if (buffer == null)
			{
				/* skip key/value metadata */
				stream.Seek ((long)count, SeekOrigin.Current);
				return KTXError.Success;
			}

			int kvdCount = stream.Read (buffer, 0, count);
			if (kvdCount != count)
			{
				return KTXError.InvalidOperation;
			} 

			return KTXError.Success;
		}

		private KTXError ReadHeader (byte[] headerChunk, out KTXHeader header)
		{			
			KTXError errorCode = KTXError.Success;
			header = new KTXHeader ();
			header.Populate (headerChunk);
			if (header.Instructions.Result != KTXError.Success)
			{
				return header.Instructions.Result;
			}
			header.KeyValueData = new byte[header.BytesOfKeyValueData];
			if (header.KeyValueData == null)
			{
				return KTXError.OutOfMemory;
			}
			return errorCode;
		}

		private static void SetAutomaticMipmapCreation (KTXLoadInstructions texinfo)
		{
			// DISABLED ON PURPOSE
			//			// Prefer glGenerateMipmaps over GL_GENERATE_MIPMAP
			//			if (texinfo.GenerateMipmaps && IsGenerateMipmapMissing ())
			//			{
			//				GL.TexParameter ((TextureTarget)texinfo.GlTarget, TextureParameterName.GenerateMipmap, (int)All.True);
			//			}
		}

		class KTXFaceData
		{
			public KTXFaceData(uint faceIndex, uint noOfMipmaps)
			{
				FaceIndex = faceIndex;
				Mipmaps = new MgImageMipmap[noOfMipmaps];
			}

			public uint FaceIndex { get; private set; }
			public MgImageMipmap[] Mipmaps { get; set; }
		}        

		MgTextureInfo[] LoadTexture (
			Stream src,
			KTXHeader header)
		{
			// default out values
			KTXError errorCode = KTXError.Success;

			bool isFirstTime = true;
			int previousLodSize = 0;
			byte[] data = null;

			var textures = new List<MgTextureInfo>();

			var faces = new KTXFaceData[header.NumberOfFaces];
			for(var i = 0; i < faces.Length; ++i)
			{
				faces[i] = new KTXFaceData((uint)i, header.NumberOfMipmapLevels);
			}

			using (var dest = new MemoryStream())
			{
				for (int level = 0; level < header.NumberOfMipmapLevels; ++level)
				{
					UInt32 faceLodSize;
					if (!header.ExtractUInt32(src, out faceLodSize))
					{
						errorCode = KTXError.InvalidOperation;
						break;
					}

					int faceLodSizeRounded = ((int)faceLodSize + 3) & ~3;

					// array texture is this correct ?
					if (isFirstTime)
					{
						isFirstTime = false;
						previousLodSize = faceLodSizeRounded;

						/* allocate memory sufficient for the first level */
						data = new byte[faceLodSizeRounded];
						if (data == null)
						{
							errorCode = KTXError.OutOfMemory;
							break;
						}
					}
					else
					{
						if (previousLodSize < faceLodSizeRounded)
						{
							/* subsequent levels cannot be larger than the first level */
							errorCode = KTXError.InvalidValue;
							break;
						}
					}

					var mipmap = new KTXMipmapData();
					mipmap.Common.Level = level;
					mipmap.Common.Data = data;
					mipmap.Common.Size = faceLodSize;
					mipmap.SizeRounded = faceLodSizeRounded;

					var loadError = ExtractFace(src, dest, header, mipmap, data, faces);
					if (loadError != KTXError.Success)
					{
						errorCode = loadError;
						break;
					}

				}

				var format = DetermineFormat(header.GlType, header.GlFormat, header.GlInternalFormat);

				// MgImageOptimizer takes 
				// 1. byte array 
				// 2. image source

				foreach(var face in faces)
				{                      
					var source = new MgImageSource
					{
						Format = format,
						Height = header.PixelHeight,
						Width = header.PixelWidth,
						Mipmaps = face.Mipmaps,
						Size = (uint) dest.Length,
					};

					var texture =  mTextureOptimizer.Load(dest.ToArray(), source, null, null);

					textures.Add(texture);
				}

				return textures.ToArray();
			}		
		}

        private enum GLIntFmt
        {
            UNDEFINED = 0
            // ASTC SRGB ALPHA 8
            ,GL_COMPRESSED_SRGB8_ALPHA8_ASTC_4x4_KHR = 0x93D0
            ,GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x4_KHR = 0x93D1
            ,GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x5_KHR = 0x93D2
            ,GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x5_KHR = 0x93D3
            ,GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x6_KHR = 0x93D4
            ,GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x5_KHR = 0x93D5
            ,GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x6_KHR = 0x93D6
            ,GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x8_KHR = 0x93D7
            ,GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x5_KHR = 0x93D8
            ,GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x6_KHR = 0x93D9
            ,GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x8_KHR = 0x93DA
            ,GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x10_KHR = 0x93DB
            ,GL_COMPRESSED_SRGB8_ALPHA8_ASTC_12x10_KHR = 0x93DC
            ,GL_COMPRESSED_SRGB8_ALPHA8_ASTC_12x12_KHR = 0x93DD

            // ASTC RBGA
            ,GL_COMPRESSED_RGBA_ASTC_4x4_KHR = 0x93B0
            ,GL_COMPRESSED_RGBA_ASTC_5x4_KHR = 0x93B1
            ,GL_COMPRESSED_RGBA_ASTC_5x5_KHR = 0x93B2
            ,GL_COMPRESSED_RGBA_ASTC_6x5_KHR = 0x93B3
            ,GL_COMPRESSED_RGBA_ASTC_6x6_KHR = 0x93B4
            ,GL_COMPRESSED_RGBA_ASTC_8x5_KHR = 0x93B5
            ,GL_COMPRESSED_RGBA_ASTC_8x6_KHR = 0x93B6
            ,GL_COMPRESSED_RGBA_ASTC_8x8_KHR = 0x93B7
            ,GL_COMPRESSED_RGBA_ASTC_10x5_KHR = 0x93B8
            ,GL_COMPRESSED_RGBA_ASTC_10x6_KHR = 0x93B9
            ,GL_COMPRESSED_RGBA_ASTC_10x8_KHR = 0x93BA
            ,GL_COMPRESSED_RGBA_ASTC_10x10_KHR = 0x93BB
            ,GL_COMPRESSED_RGBA_ASTC_12x10_KHR = 0x93BC
            ,GL_COMPRESSED_RGBA_ASTC_12x12_KHR = 0x93BD

            // DXT
            ,GL_COMPRESSED_RGB_S3TC_DXT1_EXT = 0x83F0
            ,GL_COMPRESSED_RGBA_S3TC_DXT1_EXT = 0x83F1
            ,GL_COMPRESSED_RGBA_S3TC_DXT3_EXT = 0x83F2
            ,GL_COMPRESSED_RGBA_S3TC_DXT5_EXT = 0x83F3

            // DXT SRGB
            ,GL_COMPRESSED_SRGB_S3TC_DXT1_EXT = 0x8C4C
            ,GL_COMPRESSED_SRGB_ALPHA_S3TC_DXT1_EXT = 0x8C4D
            ,GL_COMPRESSED_SRGB_ALPHA_S3TC_DXT3_EXT = 0x8C4E
            ,GL_COMPRESSED_SRGB_ALPHA_S3TC_DXT5_EXT = 0x8C4F

            // ARB_texture_compression_rgtc
            ,GL_COMPRESSED_RED_RGTC1 = 0x8DBB
            ,GL_COMPRESSED_SIGNED_RED_RGTC1 = 0x8DBC
            ,GL_COMPRESSED_RG_RGTC2 = 0x8DBD
            ,GL_COMPRESSED_SIGNED_RG_RGTC2 = 0x8DBE

            // GL_ARB_texture_compression_bptc
            ,GL_COMPRESSED_RGBA_BPTC_UNORM_ARB = 0x8E8C
            ,GL_COMPRESSED_SRGB_ALPHA_BPTC_UNORM_ARB = 0x8E8D
            ,GL_COMPRESSED_RGB_BPTC_SIGNED_FLOAT_ARB = 0x8E8E
            ,GL_COMPRESSED_RGB_BPTC_UNSIGNED_FLOAT_ARB = 0x8E8F
        };

        private MgFormat DetermineFormat(uint glType, uint glFormat, uint glInternalFormat)
		{
			const int IS_COMPRESSED = 0;
            if (glType == IS_COMPRESSED)
            {
                GLIntFmt internalFormat = (GLIntFmt) glInternalFormat;

                switch (internalFormat)
                {
                    default:
                        throw new NotSupportedException();

                    // DXT
                    case GLIntFmt.GL_COMPRESSED_RGB_S3TC_DXT1_EXT:
                        return MgFormat.BC1_RGB_UNORM_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_SRGB_S3TC_DXT1_EXT:
                        return MgFormat.BC1_RGB_SRGB_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_RGBA_S3TC_DXT1_EXT:
                        return MgFormat.BC1_RGBA_UNORM_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_SRGB_ALPHA_S3TC_DXT1_EXT:
                        return MgFormat.BC1_RGBA_SRGB_BLOCK;

                    case GLIntFmt.GL_COMPRESSED_RGBA_S3TC_DXT3_EXT:
                        return MgFormat.BC3_UNORM_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_SRGB_ALPHA_S3TC_DXT3_EXT:
                        return MgFormat.BC3_SRGB_BLOCK;

                    case GLIntFmt.GL_COMPRESSED_RGBA_S3TC_DXT5_EXT:
                        return MgFormat.BC5_SNORM_BLOCK;

                    // ARB_texture_compression_rgtc
                    case GLIntFmt.GL_COMPRESSED_RED_RGTC1:
                        return MgFormat.BC3_UNORM_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_SIGNED_RED_RGTC1:
                        return MgFormat.BC4_SNORM_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_SIGNED_RG_RGTC2:
                        return MgFormat.BC5_SNORM_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_RG_RGTC2:
                        return MgFormat.BC5_UNORM_BLOCK;

                    // GL_ARB_texture_compression_bptc
                    case GLIntFmt.GL_COMPRESSED_RGBA_BPTC_UNORM_ARB:
                        return MgFormat.BC7_UNORM_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_SRGB_ALPHA_BPTC_UNORM_ARB:
                        return MgFormat.BC7_SRGB_BLOCK;

                    case GLIntFmt.GL_COMPRESSED_RGB_BPTC_SIGNED_FLOAT_ARB:
                        return MgFormat.BC6H_SFLOAT_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_RGB_BPTC_UNSIGNED_FLOAT_ARB:
                        return MgFormat.BC6H_UFLOAT_BLOCK;

                    // ASTC SRGBA
                    case GLIntFmt.GL_COMPRESSED_SRGB8_ALPHA8_ASTC_4x4_KHR:
                        return MgFormat.ASTC_4x4_SRGB_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x4_KHR:
                        return MgFormat.ASTC_5x4_SRGB_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x5_KHR:
                        return MgFormat.ASTC_5x5_SRGB_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x5_KHR:
                        return MgFormat.ASTC_6x5_SRGB_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x6_KHR:
                        return MgFormat.ASTC_6x6_SRGB_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x5_KHR:
                        return MgFormat.ASTC_8x5_SRGB_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x6_KHR:
                        return MgFormat.ASTC_8x6_SRGB_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x8_KHR:
                        return MgFormat.ASTC_8x8_SRGB_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x5_KHR:
                        return MgFormat.ASTC_10x5_SRGB_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x6_KHR:
                        return MgFormat.ASTC_10x6_SRGB_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x8_KHR:
                        return MgFormat.ASTC_10x8_SRGB_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x10_KHR:
                        return MgFormat.ASTC_10x10_SRGB_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_SRGB8_ALPHA8_ASTC_12x10_KHR:
                        return MgFormat.ASTC_12x10_SRGB_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_SRGB8_ALPHA8_ASTC_12x12_KHR:
                        return MgFormat.ASTC_12x12_SRGB_BLOCK;

                    // ASTC RGBA
                    case GLIntFmt.GL_COMPRESSED_RGBA_ASTC_4x4_KHR:
                        return MgFormat.ASTC_4x4_UNORM_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_RGBA_ASTC_5x4_KHR:
                        return MgFormat.ASTC_5x4_SRGB_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_RGBA_ASTC_5x5_KHR:
                        return MgFormat.ASTC_5x5_UNORM_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_RGBA_ASTC_6x5_KHR:
                        return MgFormat.ASTC_6x5_UNORM_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_RGBA_ASTC_6x6_KHR:
                        return MgFormat.ASTC_6x6_UNORM_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_RGBA_ASTC_8x5_KHR:
                        return MgFormat.ASTC_8x5_UNORM_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_RGBA_ASTC_8x6_KHR:
                        return MgFormat.ASTC_8x6_UNORM_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_RGBA_ASTC_8x8_KHR:
                        return MgFormat.ASTC_8x8_UNORM_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_RGBA_ASTC_10x5_KHR:
                        return MgFormat.ASTC_10x5_UNORM_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_RGBA_ASTC_10x6_KHR:
                        return MgFormat.ASTC_10x6_UNORM_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_RGBA_ASTC_10x8_KHR:
                        return MgFormat.ASTC_10x8_UNORM_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_RGBA_ASTC_10x10_KHR:
                        return MgFormat.ASTC_10x10_UNORM_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_RGBA_ASTC_12x10_KHR:
                        return MgFormat.ASTC_12x10_UNORM_BLOCK;
                    case GLIntFmt.GL_COMPRESSED_RGBA_ASTC_12x12_KHR:
                        return MgFormat.ASTC_12x12_UNORM_BLOCK;

                }
			}
			else
			{
				// TAKEN FROM OpenGL 4.4 core spec
				const uint GL_UNSIGNED_BYTE = 0x1401;
				const uint GL_BYTE = 0x1400;

				const uint GL_SHORT = 0x1402;
				const uint GL_UNSIGNED_SHORT = 0x1403;
				const uint GL_HALF_FLOAT = 0x140B;
				const uint GL_FLOAT = 0x1406;
				const uint GL_FLOAT_32_UNSIGNED_INT_24_8_REV = 0x8DAD;

				const uint GL_INT = 0x1404;
				const uint GL_UNSIGNED_INT = 0x1405;
				const uint GL_UNSIGNED_INT_24_8 = 0x84FA;


                const uint GL_UNSIGNED_SHORT_4_4_4_4 = 0x8033;
				const uint GL_UNSIGNED_SHORT_5_5_5_1 = 0x8034;

				const uint GL_UNSIGNED_INT_8_8_8_8_REVERSED = 0x8367;
				const uint GL_UNSIGNED_INT_2_10_10_10_REVERSED = 0x8368;

				const uint GL_RGBA = 0x1908;
				const uint GL_RG = 0x8227;
                const uint GL_RG_INTEGER = 0x8228;
                const uint GL_RED = 0x1903;
                const uint GL_RED_INTEGER = 0x8D94;
                const int GL_RGB = 0x1907;
				const int GL_BGRA = 0x80E1;
				const uint GL_BGR = 0x80E0;
				const uint GL_DEPTH_COMPONENT = 0x1902;
				const uint GL_DEPTH_STENCIL = 0x84F9;

                const uint GL_STENCIL_INDEX = 0x1901;
                const uint GL_STENCIL_INDEX8 = 0x8D48;

                switch (glFormat)
                {



                    case GL_RGBA:
                        {
                            const uint GL_UNSIGNED_SHORT_1_5_5_5_REV = 0x8366;
                            const uint GL_UNSIGNED_INT_8_8_8_8 = 0x8035;
                            const uint GL_UNSIGNED_INT_5_9_9_9_REV = 0x8C3E;

                            switch (glType)
                            {
                                case GL_UNSIGNED_INT_8_8_8_8:
                                case GL_UNSIGNED_BYTE:
                                    return MgFormat.R8G8B8A8_UINT;
                                // NOT SURE ABOUT THESE
    
                                case GL_UNSIGNED_INT_8_8_8_8_REVERSED:
                                    return MgFormat.A8B8G8R8_UINT_PACK32;
                                case GL_UNSIGNED_INT_2_10_10_10_REVERSED:
                                    return MgFormat.A2B10G10R10_UINT_PACK32;

                                case GL_UNSIGNED_INT_5_9_9_9_REV:
                                    return MgFormat.E5B9G9R9_UFLOAT_PACK32;
                                case GL_BYTE:
                                    return MgFormat.R8G8B8A8_SINT;
                                case GL_UNSIGNED_SHORT:
                                    return MgFormat.R16G16B16A16_UINT;
                                case GL_SHORT:
                                    return MgFormat.R16G16B16A16_SINT;
                                case GL_HALF_FLOAT:
                                    return MgFormat.R16G16B16A16_SFLOAT;
                                case GL_FLOAT:
                                    return MgFormat.R32G32B32A32_SFLOAT;
                                case GL_INT:
                                    return MgFormat.R32G32B32A32_SINT;
                                case GL_UNSIGNED_INT:
                                    return MgFormat.R32G32B32A32_UINT;
                                case GL_UNSIGNED_SHORT_5_5_5_1:
                                    return MgFormat.R5G5B5A1_UNORM_PACK16;
                                case GL_UNSIGNED_SHORT_1_5_5_5_REV:
                                    return MgFormat.A1R5G5B5_UNORM_PACK16;
                                case GL_UNSIGNED_SHORT_4_4_4_4:
                                    return MgFormat.R4G4B4A4_UNORM_PACK16;
                                default:
                                    throw new NotSupportedException();
                            }
                        }
                    case GL_RGB:
                        {
                            const uint GL_UNSIGNED_INT_10F_11F_11F_REV = 0x8C3B;


                            switch (glType)
                            {
                                case GL_UNSIGNED_SHORT_4_4_4_4:
                                    return MgFormat.R4G4B4A4_UNORM_PACK16;
                                case GL_UNSIGNED_INT_10F_11F_11F_REV:
                                    return MgFormat.B10G11R11_UFLOAT_PACK32;
                                case GL_BYTE:
                                    return MgFormat.R8G8B8_SINT;
                                case GL_UNSIGNED_SHORT:
                                    return MgFormat.R16G16B16_UINT;
                                case GL_SHORT:
                                    return MgFormat.R16G16B16_SINT;
                                case GL_HALF_FLOAT:
                                    return MgFormat.R16G16B16_SFLOAT;
                                case GL_FLOAT:
                                    return MgFormat.R32G32B32_SFLOAT;
                                case GL_INT:
                                    return MgFormat.R32G32B32_SINT;
                                case GL_UNSIGNED_INT:
                                    return MgFormat.R32G32B32_UINT;
                                default:
                                    throw new NotSupportedException("glType is not supported for " + nameof(GL_RGB) + " : " + glType);
                            }
                        }
                    case GL_BGRA:
                        {
                            switch (glType)
                            {
                                case GL_UNSIGNED_BYTE:
                                    return MgFormat.B8G8R8A8_UINT;
                                case GL_BYTE:
                                    return MgFormat.B8G8R8A8_SINT;
                                case GL_UNSIGNED_SHORT_4_4_4_4:
                                    return MgFormat.B4G4R4A4_UNORM_PACK16;
                                case GL_UNSIGNED_SHORT_5_5_5_1:
                                    return MgFormat.B5G5R5A1_UNORM_PACK16;
                                case GL_UNSIGNED_INT_8_8_8_8_REVERSED:
                                    return MgFormat.A8B8G8R8_UINT_PACK32;
                                case GL_UNSIGNED_INT_2_10_10_10_REVERSED:
                                    return MgFormat.A2B10G10R10_UINT_PACK32;
                                default:
                                    throw new NotSupportedException("glType is not supported for " + nameof(GL_BGRA) + " : " + glType);

                            }
                        }
                    case GL_BGR:
                        {
                            const uint GL_UNSIGNED_SHORT_5_6_5_REV = 0x8364;

                            switch (glType)
                            {
                                case GL_BYTE:
                                    return MgFormat.B8G8R8A8_SINT;
                                case GL_UNSIGNED_BYTE:
                                    return MgFormat.B8G8R8A8_UINT;
                                case GL_UNSIGNED_SHORT_5_6_5_REV:
                                    return MgFormat.B5G6R5_UNORM_PACK16;
                                default:
                                    throw new NotSupportedException("glType is not supported for " + nameof(GL_BGR) + " : " + glType);
                            }
                        }
                    case GL_DEPTH_COMPONENT:
                        {
                            switch (glType)
                            {
                                case GL_UNSIGNED_SHORT:
                                    return MgFormat.D16_UNORM;
                                case GL_FLOAT:
                                    return MgFormat.D32_SFLOAT;
                                case GL_FLOAT_32_UNSIGNED_INT_24_8_REV:
                                    return MgFormat.X8_D24_UNORM_PACK32;
                                default:
                                    throw new NotSupportedException("glType is not supported for " + nameof(GL_DEPTH_COMPONENT) + " : " + glType);
                            }
                        }
                    case GL_DEPTH_STENCIL:
                        {
                            switch (glType)
                            {
                                case GL_UNSIGNED_INT_24_8:
                                    return MgFormat.D16_UNORM_S8_UINT;
                                case GL_FLOAT_32_UNSIGNED_INT_24_8_REV:
                                    return MgFormat.D24_UNORM_S8_UINT;
                                default:
                                    throw new NotSupportedException("glType is not supported for " + nameof(GL_DEPTH_STENCIL) + " : " + glType);
                            }
                        }
                    case GL_RED_INTEGER:
                        {
                            switch (glType)
                            {
                                case GL_UNSIGNED_BYTE:
                                    return MgFormat.R8_UINT;
                                case GL_BYTE:
                                    return MgFormat.R8_SINT;
                                case GL_UNSIGNED_SHORT:
                                    return MgFormat.R16_UINT;
                                case GL_SHORT:
                                    return MgFormat.R16_SINT;
                                case GL_INT:
                                    return MgFormat.R32_SINT;
                                case GL_UNSIGNED_INT:
                                    return MgFormat.R32_UINT;
                                default:
                                    throw new NotSupportedException("glType is not supported for " + nameof(GL_RED_INTEGER) + " : " + glType);
                            }
                        }
                    case GL_RED:
                        {
                            switch (glType)
                            {
                                case GL_UNSIGNED_BYTE:
                                    return MgFormat.R8_UNORM;
                                case GL_BYTE:
                                    return MgFormat.R8_SNORM;
                                case GL_UNSIGNED_SHORT:
                                    return MgFormat.R16_UNORM;
                                case GL_SHORT:
                                    return MgFormat.R16_SNORM;
                                case GL_HALF_FLOAT:
                                    return MgFormat.R16_SFLOAT;
                                case GL_FLOAT:
                                    return MgFormat.R32_SFLOAT;
                                case GL_INT:
                                    return MgFormat.R32_SINT;
                                case GL_UNSIGNED_INT:
                                    return MgFormat.R32_UINT;
                                default:
                                    throw new NotSupportedException("glType is not supported for GL_RED :" + glType);
                            }
                        }
                    case GL_RG_INTEGER:
                        {
                            switch (glType)
                            {
                                case GL_UNSIGNED_BYTE:
                                    return MgFormat.R8G8_UINT;
                                case GL_BYTE:
                                    return MgFormat.R8G8_SINT;
                                case GL_UNSIGNED_SHORT:
                                    return MgFormat.R16G16_UINT;
                                case GL_SHORT:
                                    return MgFormat.R16G16_SINT;
                                case GL_INT:
                                    return MgFormat.R32G32_SINT;
                                case GL_UNSIGNED_INT:
                                    return MgFormat.R32G32_UINT;
                                default:
                                    throw new NotSupportedException("glType is not supported for RG_INTEGER :" + glType);
                            }
                        }
                    case GL_RG:
                        {
                            switch (glType)
                            {
                                case GL_UNSIGNED_BYTE:
                                    return MgFormat.R8G8_UINT;
                                case GL_BYTE:
                                    return MgFormat.R8G8_SINT;
                                case GL_UNSIGNED_SHORT:
                                    return MgFormat.R16G16_UINT;
                                case GL_SHORT:
                                    return MgFormat.R16G16_SINT;
                                case GL_HALF_FLOAT:
                                    return MgFormat.R16G16_SFLOAT;
                                case GL_FLOAT:
                                    return MgFormat.R32G32_SFLOAT;
                                case GL_INT:
                                    return MgFormat.R32G32_SINT;
                                case GL_UNSIGNED_INT:
                                    return MgFormat.R32G32_UINT;
                                default:
                                    throw new NotSupportedException("glType is not supported for GL_RG :" + glType);
                            }
                        }
                    case GL_STENCIL_INDEX8:
                    case GL_STENCIL_INDEX:
                        {
                            switch (glType)
                            {
                                case GL_UNSIGNED_BYTE:
                                    return MgFormat.S8_UINT;
                                default:
                                    throw new NotSupportedException("glType is not supported for GL_STENCIL_INDEX/GL_STENCIL_INDEX8 :" + glType);
                            }
                        }            
                    default:
                        throw new NotSupportedException("glFormat is not supported :" + glType);
                }             
			}
		}

		KTXError ExtractFace (
			Stream src, 
			Stream dest,           
			KTXHeader header,
			KTXMipmapData mipmap,
			byte[] dataBuffer,
			KTXFaceData[] faces
			)
		{
			mipmap.Common.PixelWidth = Math.Max (1, header.PixelWidth >> mipmap.Common.Level);
			mipmap.Common.PixelHeight = Math.Max (1, header.PixelHeight >> mipmap.Common.Level);
			mipmap.Common.PixelDepth = Math.Max (1, header.PixelDepth >> mipmap.Common.Level);

			for (int face = 0; face < header.NumberOfFaces; ++face)
			{
				var bytesRead = src.Read (dataBuffer, 0,  mipmap.SizeRounded);
				if (bytesRead != mipmap.SizeRounded)
				{
					return KTXError.UnexpectedEndOfFile;
				}

				var bytesToRead = (int)mipmap.Common.Size;

				/* Perform endianness conversion on texture data */
				if (header.RequiresSwap() && header.GlTypeSize == 2)
				{
					KTXHeader.SwapEndian16 (dataBuffer, bytesToRead);
				}
				else if (header.RequiresSwap() && header.GlTypeSize == 4)
				{
					KTXHeader.SwapEndian32 (dataBuffer, bytesToRead);
				}

				mipmap.Common.NumberOfFaces = (int)header.NumberOfFaces;
				mipmap.ViewType = header.Instructions.ViewType;
				mipmap.Common.IsCompressed = header.Instructions.IsCompressed;
				mipmap.Common.TextureDimensions = header.Instructions.TextureDimensions;
				mipmap.Common.FaceIndex = face;

				if (header.Instructions.TextureDimensions == 1)
				{					

				}
				else if (header.Instructions.TextureDimensions == 2)
				{
					if (header.NumberOfArrayElements > 0)
					{
						mipmap.Common.PixelHeight = header.NumberOfArrayElements;
					}
				}
				else if (header.Instructions.TextureDimensions == 3)
				{
					if (header.NumberOfArrayElements > 0)
					{
						mipmap.Common.PixelDepth = header.NumberOfArrayElements;
					}
				}

				faces[face].Mipmaps[mipmap.Common.Level] = new MgImageMipmap
				{
					// TODO : MAKE SURE IT DOES NOT "EXPLODE" LATER
					Offset = (uint)dest.Position,
					Width = mipmap.Common.PixelWidth,
					Height = mipmap.Common.PixelHeight,
					Size = mipmap.Common.Size,
				};

				dest.Write(dataBuffer, 0, bytesToRead);


	//            // Renderion is returning INVALID_VALUE. Oops!!
	//            if (mETCUnpacker.IsRequired (header.Instructions, mipmap.GLError))
				//{
				//	var result = mETCUnpacker.UnpackCompressedTexture (
				//		header.Instructions,
				//		mipmap.Common.Level,
				//		face,
				//		mipmap.Common.PixelWidth,
				//		mipmap.Common.PixelHeight,
				//		mipmap.Common.Data);
					
				//	if (result != KTXError.Success)
				//	{
				//		return result;
				//	}					

				//	mipmap.GLError = mPlatform.GetError ();
				//}
			}
			return KTXError.Success;
		}

	}
}

