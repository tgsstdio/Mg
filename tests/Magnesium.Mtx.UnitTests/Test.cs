using NUnit.Framework;
using System;
using System.Text;

namespace Magnesium.Mtx.UnitTests
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void TestCase()
		{
			// SHOULD BE <<MTX 100>>\r\n\z
			byte[] KTXIdentifier = { 0xAB, 0x4D, 0x54, 0x58, 0x20, 0x31, 0x31, 0xBB, 0x0D, 0x0A, 0x1A, 0x0A };

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
}
}
