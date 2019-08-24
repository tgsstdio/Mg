using System;
namespace Magnesium.Toolkit.Validation.PhysicalDevice
{
	public class GetPhysicalDeviceSurfacePresentModesKHR
	{
		public static void Validate(IMgSurfaceKHR surface)
		{
            if (surface == null)
                throw new ArgumentNullException(nameof(surface));
        }
	}
}
