using System;
using System.IO;
using System.Diagnostics;

namespace Magnesium.Mtx
{
	public class MtxTextureManager : IMtxTextureLoader
	{
		private readonly IMgTextureGenerator mTextureOptimizer;
		private readonly IMgGraphicsConfiguration mGraphicsConfiguration;

		public MtxTextureManager(IMgTextureGenerator optimizer, IMgGraphicsConfiguration configuration)
		{
			mTextureOptimizer = optimizer;
			mGraphicsConfiguration = configuration;
		}

		public KTXTextureOutput Load(Stream fs)
		{
			var HEADER_SIZE = MtxHeader.HEADER_SIZE;
			var headerChunk = new byte[HEADER_SIZE];
			int count = fs.Read(headerChunk, 0, HEADER_SIZE);
			if (count != HEADER_SIZE)
			{
				throw new InvalidOperationException("Mtx : Invalid header size ");
			}

			MtxHeader header;
			var status = ReadHeader(headerChunk, out header);
			if (status != MtxError.Success)
			{
				throw new InvalidOperationException("Mtx: Header parsing error");
			}

			status = ReadKeyValueDataSection(fs, header.KeyValueData, (int)header.BytesOfKeyValueData);
			if (status != MtxError.Success)
			{
				throw new InvalidOperationException("Mtx: invalid key value data");
			}

			//KeyValueArrayData[] inputData = GenerateKeyValueArray (destHeader);

			return LoadTexture(fs, header);
		}

		static MtxError ReadKeyValueDataSection(Stream stream, byte[] buffer, int count)
		{
			if (count <= 0)
			{
				return MtxError.Success;
			}

			if (buffer == null)
			{
				/* skip key/value metadata */
				stream.Seek((long)count, SeekOrigin.Current);
				return MtxError.Success;
			}

			int kvdCount = stream.Read(buffer, 0, count);
			if (kvdCount != count)
			{
				return MtxError.InvalidOperation;
			}

			return MtxError.Success;
		}

		private MtxError ReadHeader(byte[] headerChunk, out MtxHeader header)
		{
			var errorCode = MtxError.Success;
			header = new MtxHeader();
			var result = header.Load(headerChunk);
			if (result != MtxError.Success)
			{
				return result;
			}

			header.KeyValueData = new byte[header.BytesOfKeyValueData];
			if (header.KeyValueData == null)
			{
				return MtxError.OutOfMemory;
			}
			return errorCode;
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

		KTXTextureOutput LoadTexture(
			Stream src,
			MtxHeader header)
		{
			// default out values
			var errorCode = MtxError.Success;

			bool isFirstTime = true;
			int previousLodSize = 0;
			byte[] data = null;

			var faces = new KTXFaceData[header.ArrayLayers];
			for (var i = 0; i < faces.Length; ++i)
			{
				faces[i] = new KTXFaceData((uint)i, header.MipLevels);
			}

			using (var dest = new MemoryStream())
			{
				for (var level = 0; level < header.MipLevels; level += 1)
				{
					UInt32 faceLodSize;
					if (!header.ExtractUInt32(src, out faceLodSize))
					{
						errorCode = MtxError.InvalidOperation;
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
							errorCode = MtxError.OutOfMemory;
							break;
						}
					}
					else
					{
						if (previousLodSize < faceLodSizeRounded)
						{
							/* subsequent levels cannot be larger than the first level */
							errorCode = MtxError.InvalidValue;
							break;
						}
					}

					var mipmap = new MtxMipmapData
					{
						Level = level,
						Data = data,
						Size = faceLodSize,
						SizeRounded = faceLodSizeRounded,
					};

					var loadError = ExtractFace(src, dest, header, mipmap, data, faces);
					if (loadError != MtxError.Success)
					{
						errorCode = loadError;
						break;
					}

				}

				var format = header.FormatType;

				// MgImageOptimizer takes 
				// 1. byte array 
				// 2. image source

				var output = new KTXTextureOutput();

				// one face at a time
				var face = faces[0];

				output.Source = new MgImageSource
				{
					Format = format,
					Height = header.ImageHeight,
					Width = header.ImageWidth,
					Mipmaps = face.Mipmaps,
					Size = (uint)dest.Length,
				};

				output.TextureInfo = mTextureOptimizer.Load(dest.ToArray(), output.Source, null, null);

				// QUEUE WAIT ON 
				var err = mGraphicsConfiguration.Queue.QueueWaitIdle();
				Debug.Assert(err == Result.SUCCESS);
				mGraphicsConfiguration.Device.FreeCommandBuffers(
					mGraphicsConfiguration.Partition.CommandPool,
					new[] { output.TextureInfo.Command });

				return output;
			}
		}

		MtxError ExtractFace(
			Stream src,
			Stream dest,
			MtxHeader header,
			MtxMipmapData mipmap,
			byte[] dataBuffer,
			KTXFaceData[] faces
			)
		{
			mipmap.PixelWidth = Math.Max(1, header.ImageWidth >> mipmap.Level);
			mipmap.PixelHeight = Math.Max(1, header.ImageHeight >> mipmap.Level);
			mipmap.PixelDepth = Math.Max(1, header.ImageDepth >> mipmap.Level);

			for (int face = 0; face < header.ArrayLayers; face += 1)
			{
				var bytesRead = src.Read(dataBuffer, 0, mipmap.SizeRounded);
				if (bytesRead != mipmap.SizeRounded)
				{
					return MtxError.UnexpectedEndOfFile;
				}

				var bytesToRead = (int) mipmap.Size;

				/* Perform endianness conversion on texture data */
				if (header.RequiresSwap())
				{
					header.SwapEndian(dataBuffer, bytesToRead);
				}

				//mipmap.NumberOfFaces = (int)header.NumberOfFaces;
				mipmap.ViewType = header.ViewType;
				mipmap.FaceIndex = face;

				//if (header.Instructions.TextureDimensions == 1)
				//{

				//}
				//else if (header.ViewType == MgImageViewType.TYPE_2D_ARRAY)
				//{
				//	if (header.ArrayLayers > 0)
				//	{
				//		mipmap.PixelHeight = header.NumberOfArrayElements;
				//	}
				//}
				//else if (header.ViewType == MgImageViewType.TYPE_CUBE)
				//{
				//	if (header.ArrayLayers > 0)
				//	{
				//		mipmap.PixelDepth = header.NumberOfArrayElements;
				//	}
				//}

				faces[face].Mipmaps[mipmap.Level] = new MgImageMipmap
				{
					// TODO : MAKE SURE IT DOES NOT "EXPLODE" LATER
					Offset = (uint)dest.Position,
					Width = mipmap.PixelWidth,
					Height = mipmap.PixelHeight,
					Size = mipmap.Size,
				};

				dest.Write(dataBuffer, 0, bytesToRead);

			}
			return MtxError.Success;
		}

	}
}

