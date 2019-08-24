using System;
namespace Magnesium.Toolkit.Validation.PhysicalDevice
{
	public class GetPhysicalDeviceExternalSemaphoreProperties
	{
		public static void Validate(MgPhysicalDeviceExternalSemaphoreInfo pExternalSemaphoreInfo)
		{
            if (pExternalSemaphoreInfo == null)
                throw new ArgumentNullException(nameof(pExternalSemaphoreInfo));
        }
	}
}
