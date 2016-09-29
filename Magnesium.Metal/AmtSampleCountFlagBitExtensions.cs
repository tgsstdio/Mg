using System;
namespace Magnesium.Metal
{
	internal static class AmtSampleCountFlagBitExtensions
	{
		internal static nuint TranslateSampleCount(MgSampleCountFlagBits count)
		{
			switch (count)
			{
				case MgSampleCountFlagBits.COUNT_1_BIT:
					return 1;
				case MgSampleCountFlagBits.COUNT_4_BIT:
					return 4;
				case MgSampleCountFlagBits.COUNT_2_BIT:
					return 2;
				case MgSampleCountFlagBits.COUNT_8_BIT:
					return 8;
				case MgSampleCountFlagBits.COUNT_16_BIT:
					return 16;
				case MgSampleCountFlagBits.COUNT_32_BIT:
					return 32;
				case MgSampleCountFlagBits.COUNT_64_BIT:
					return 64;
				default:
					throw new NotSupportedException();
			}
		}
	}
}
