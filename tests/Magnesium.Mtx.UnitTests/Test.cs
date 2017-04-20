using NUnit.Framework;
using System;
using System.Text;

namespace Magnesium.Mtx.UnitTests
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void CheckIdentifier()
		{
			// SHOULD BE <<MTX 100>>\r\n\z\n
			byte[] KTXIdentifier = Magnesium.Mtx.MtxHeader.FileIdentifier;

			var charOffset = 0;
			var internalValue = KTXIdentifier[charOffset];
			Assert.AreEqual(0xAB, internalValue);

			charOffset += 1;
			internalValue = KTXIdentifier[charOffset];
			var actual = ExtractASCII(internalValue);
			Assert.AreEqual('M', actual);

			charOffset += 1;
			internalValue = KTXIdentifier[charOffset];
			actual = ExtractASCII(internalValue);
			Assert.AreEqual('T', actual);

			charOffset += 1;
			internalValue = KTXIdentifier[charOffset];
			actual = ExtractASCII(internalValue);
			Assert.AreEqual('X', actual);

			charOffset += 1;
			internalValue = KTXIdentifier[charOffset];
			actual = ExtractASCII(internalValue);
			Assert.AreEqual(' ', actual);

			charOffset += 1;
			internalValue = KTXIdentifier[charOffset];
			actual = ExtractASCII(internalValue);
			Assert.AreEqual('1', actual);

			charOffset += 1;
			internalValue = KTXIdentifier[charOffset];
			actual = ExtractASCII(internalValue);
			Assert.AreEqual('0', actual);

			charOffset += 1;
			internalValue = KTXIdentifier[charOffset];
			actual = ExtractASCII(internalValue);
			Assert.AreEqual('0', actual);

			charOffset += 1;
			internalValue = KTXIdentifier[charOffset];
			Assert.AreEqual(0xBB, internalValue);

			charOffset += 1;
			internalValue = KTXIdentifier[charOffset];
			Assert.AreEqual(0x0D, internalValue);

			charOffset += 1;
			internalValue = KTXIdentifier[charOffset];
			Assert.AreEqual(0x0A, internalValue);

			charOffset += 1;
			internalValue = KTXIdentifier[charOffset];
			Assert.AreEqual(0x1A, internalValue);

			charOffset += 1;
			internalValue = KTXIdentifier[charOffset];
			Assert.AreEqual(0x0A, internalValue);

		}

		static char ExtractASCII(byte value)
		{
			return Encoding.ASCII.GetChars(new byte[] { value }, 0, 1)[0];
		}

		[Test()]
		public void ExtractData()
		{
			var expected = new MtxHeader
			{
				Endianness = Mtx.MtxHeader.ENDIAN_REF,
				ImageType = MgImageType.TYPE_2D,
				ViewType = MgImageViewType.TYPE_2D_ARRAY,
				FormatType = MgFormat.R8G8B8_UINT,
				PixelSize = 3,
				ImageWidth = 1024,
				ImageHeight = 512,
				ImageDepth = 1,
				ArrayLayers = 2,
				MipLevels = 10,
				BytesOfKeyValueData = 0,
			};

			var buf = new byte[MtxHeader.HEADER_SIZE];
			expected.Save(buf, 0);

			var actual = new MtxHeader();
			actual.Load(buf);

			Assert.AreEqual(expected.Endianness, actual.Endianness);
			Assert.AreEqual(expected.ImageType, actual.ImageType);
			Assert.AreEqual(expected.ViewType, actual.ViewType);
			Assert.AreEqual(expected.FormatType, actual.FormatType);
			Assert.AreEqual(expected.PixelSize, actual.PixelSize);
			Assert.AreEqual(expected.ImageWidth, actual.ImageWidth);
			Assert.AreEqual(expected.ImageHeight, actual.ImageHeight);
			Assert.AreEqual(expected.ImageDepth, actual.ImageDepth);
			Assert.AreEqual(expected.ArrayLayers, expected.ArrayLayers);
			Assert.AreEqual(expected.MipLevels, expected.MipLevels);
		}
	}
}
