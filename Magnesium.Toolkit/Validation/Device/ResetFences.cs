using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class ResetFences
	{
		public static void Validate(IMgFence[] pFences)
		{
            if (pFences == null)
                throw new ArgumentNullException(nameof(pFences));
        }
	}
}
