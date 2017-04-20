using System;

namespace Magnesium.Mtx
{
	public class MtxHeader
	{
		public UInt32 Endianness;

		// TODO : adjust file identifier
		// SHOULD BE <<MTX 100>>\r\n0x0A
		public static readonly byte[] FileIdentifier =
		{
			0xAB,
			0x4D, 0x54, 0x58,
			0x20,
			0x31, 0x30, 0x30,
			0xBB,
			0x0D, 0x0A, 0x1A,
			0x0A
		};
		public readonly static UInt32 HEADER_SIZE = 63;
		public readonly static UInt32 ENDIAN_REF = 0x04030201;
		public readonly static UInt32 ENDIAN_REF_REVERSED = 0x01020304;

		private int CopyIntoBytes(byte[] dest, UInt32 value, int offset)
		{
			var fileEndianness = BitConverter.GetBytes(value);
			Array.Copy(fileEndianness, 0, dest, offset, fileEndianness.Length);
			return fileEndianness.Length;
		}

		private bool RequiresSwap()
		{
			return Endianness == ENDIAN_REF_REVERSED;
		}

		private const int UINT32_NO_OF_BYTES = 4;

		public void Save(byte[] dst, int offset)
		{
			Array.Copy(FileIdentifier, dst, FileIdentifier.Length); // 13
			offset += FileIdentifier.Length;
			offset = WriteUInt32(dst, offset, ENDIAN_REF); // 4 

			offset = WriteByte(dst, offset, (byte)ImageType); // 6
			offset = WriteByte(dst, offset, (byte)ViewType); // 
			offset = WriteUInt32(dst, offset, (UInt32)FormatType); // 

			offset = WriteUInt32(dst, offset, ImageWidth); // 12
			offset = WriteUInt32(dst, offset, ImageHeight); // 
			offset = WriteUInt32(dst, offset, ImageDepth); // 

			offset = WriteUInt32(dst, offset, PixelSize); // 12
			offset = WriteUInt32(dst, offset, MipLevels); // 
			offset = WriteUInt32(dst, offset, ArrayLayers); // 

			offset = WriteUInt32(dst, offset, BytesOfKeyValueData); // 4
		}

		int WriteUInt32(byte[] dst, int startIndex, uint uintValue)
		{
			var temp = BitConverter.GetBytes(uintValue);
			Array.Copy(temp, 0, dst, startIndex, temp.Length);
			return startIndex + temp.Length;
		}

		int WriteByte(byte[] dst, int startIndex, byte byteValue)
		{
			dst[startIndex] = byteValue;
			return startIndex + 1;
		}

		public KTXError Load(byte[] headerChunk)
		{
			/* Compare identifier, is this a KTX file? */
			for (int i = 0; i < FileIdentifier.Length; ++i)
			{
				if (headerChunk[i] != FileIdentifier[i])
				{
					return KTXError.UnknownFileFormat;
				}
			}

			int headerOffset = FileIdentifier.Length;

			Endianness = BitConverter.ToUInt32(headerChunk, headerOffset);

			headerOffset += sizeof(UInt32);
			PopulateFields(headerChunk, headerOffset);

			return KTXError.Success;
		}

		public MgImageType ImageType;
		public MgImageViewType ViewType;
		public MgFormat FormatType;

		public UInt32 ImageWidth;
		public UInt32 ImageHeight;
		public UInt32 ImageDepth;
		public UInt32 PixelSize;
		public UInt32 MipLevels;
		public UInt32 ArrayLayers;

		public UInt32 BytesOfKeyValueData;

		private void PopulateFields(byte[] src, int offset)
		{
			var startIndex = offset;

			{
				byte rawImageType;
				startIndex = ReadByte(src, startIndex, out rawImageType);
				ImageType = (MgImageType)rawImageType;
			}

			{
				byte rawViewType;
				startIndex = ReadByte(src, startIndex, out rawViewType);
				ViewType = (MgImageViewType)rawViewType;
			}

			{
				uint rawFormatType;
				startIndex = ReadUInt32(src, startIndex, out rawFormatType);
				FormatType = (MgFormat)rawFormatType;
			}

			startIndex = ReadUInt32(src, startIndex, out ImageWidth);
			startIndex = ReadUInt32(src, startIndex, out ImageHeight);
			startIndex = ReadUInt32(src, startIndex, out ImageDepth);

			startIndex = ReadUInt32(src, startIndex, out PixelSize);
			startIndex = ReadUInt32(src, startIndex, out MipLevels);
			startIndex = ReadUInt32(src, startIndex, out ArrayLayers);

			startIndex = ReadUInt32(src, startIndex, out BytesOfKeyValueData);
		}

		private int ReadByte(byte[] src, int offset, out byte dest)
		{
			dest = src[offset];
			return offset + 1;
		}

		private int ReadUInt32(byte[] src, int offset, out UInt32 dest)
		{
			if (RequiresSwap())
			{
				var temp = new byte[UINT32_NO_OF_BYTES];
				Array.Copy(src, offset, temp, 0, UINT32_NO_OF_BYTES);
				Array.Reverse(temp, 0, UINT32_NO_OF_BYTES);
				dest = BitConverter.ToUInt32(temp, offset);
			}
			else
			{
				dest = BitConverter.ToUInt32(src, offset);
			}

			return offset + UINT32_NO_OF_BYTES;
		}

		public byte[] KeyValueData { get; set; }
	};
}

