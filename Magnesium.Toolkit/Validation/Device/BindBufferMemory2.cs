using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class BindBufferMemory2
	{
		public static void Validate(MgBindBufferMemoryInfo[] pBindInfos)
		{
            if (pBindInfos == null)
                throw new ArgumentNullException(nameof(pBindInfos));
        }
	}
}
