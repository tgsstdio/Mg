using System;
namespace Magnesium.Toolkit.Validation.PhysicalDevice
{
	public class GetPhysicalDeviceExternalBufferProperties
	{
		public static void Validate(MgPhysicalDeviceExternalBufferInfo pExternalBufferInfo)
		{
            if (pExternalBufferInfo == null)
                throw new ArgumentNullException(nameof(pExternalBufferInfo));
        }
	}
}
