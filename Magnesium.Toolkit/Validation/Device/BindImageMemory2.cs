using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class BindImageMemory2
	{
		public static void Validate(MgBindImageMemoryInfo[] pBindInfos)
		{
            if (pBindInfos == null)
                throw new ArgumentNullException(nameof(pBindInfos));
        }
	}
}
