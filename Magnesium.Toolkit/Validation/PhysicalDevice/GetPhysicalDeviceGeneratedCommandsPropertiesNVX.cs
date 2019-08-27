using System;
namespace Magnesium.Toolkit.Validation.PhysicalDevice
{
	public static class GetPhysicalDeviceGeneratedCommandsPropertiesNVX
	{
		public static void Validate(MgDeviceGeneratedCommandsFeaturesNVX pFeatures)
		{
            if (pFeatures == null)
                throw new ArgumentNullException(nameof(pFeatures));
        }
	}
}
