using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class WaitForFences
	{
		public static void Validate(IMgFence[] pFences, Boolean waitAll, UInt64 timeout)
		{
            if (pFences == null)
                throw new ArgumentNullException(nameof(pFences));
        }
	}
}
