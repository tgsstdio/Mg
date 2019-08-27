using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class InvalidateMappedMemoryRanges
	{
		public static void Validate(MgMappedMemoryRange[] pMemoryRanges)
		{
            if (pMemoryRanges == null)
                throw new ArgumentNullException(nameof(pMemoryRanges));
        }
	}
}
