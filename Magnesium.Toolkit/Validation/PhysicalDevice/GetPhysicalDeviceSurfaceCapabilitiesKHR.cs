using System;
namespace Magnesium.Toolkit.Validation.PhysicalDevice
{
	public class GetPhysicalDeviceSurfaceCapabilitiesKHR
	{
		public static void Validate(IMgSurfaceKHR surface)
		{
            if (surface == null)
                throw new ArgumentNullException(nameof(surface));
        }
	}
}
