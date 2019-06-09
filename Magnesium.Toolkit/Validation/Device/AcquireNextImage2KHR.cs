using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public class AcquireNextImage2KHR
	{
		public static void Validate(MgAcquireNextImageInfoKHR pAcquireInfo, ref UInt32 pImageIndex)
		{
            if (pAcquireInfo == null)
                throw new ArgumentNullException(nameof(pAcquireInfo));
        }
	}
}
