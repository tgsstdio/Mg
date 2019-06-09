using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public class BindImageMemory2
	{
		public static void Validate(MgBindImageMemoryInfo[] pBindInfos)
		{
            if (pBindInfos == null)
                throw new ArgumentNullException(nameof(pBindInfos));
        }
	}
}
