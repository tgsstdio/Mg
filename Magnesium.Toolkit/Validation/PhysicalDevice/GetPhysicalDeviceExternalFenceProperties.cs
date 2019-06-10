using System;
namespace Magnesium.Toolkit.Validation.PhysicalDevice
{
	public class GetPhysicalDeviceExternalFenceProperties
	{
		public static void Validate(MgPhysicalDeviceExternalFenceInfo pExternalFenceInfo)
		{
            if (pExternalFenceInfo == null)
                throw new ArgumentNullException(nameof(pExternalFenceInfo));
        }
	}
}
