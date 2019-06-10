using System;
namespace Magnesium.Toolkit.Validation.PhysicalDevice
{
	public class GetPhysicalDeviceSurfaceSupportKHR
	{
		public static void Validate(UInt32 queueFamilyIndex, IMgSurfaceKHR surface, ref Boolean pSupported)
		{
            if (surface == null)
                throw new ArgumentNullException(nameof(surface));
        }
	}
}
