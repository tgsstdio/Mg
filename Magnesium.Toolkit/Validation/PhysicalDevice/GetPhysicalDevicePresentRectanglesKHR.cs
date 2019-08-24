using System;
namespace Magnesium.Toolkit.Validation.PhysicalDevice
{
	public class GetPhysicalDevicePresentRectanglesKHR
	{
		public static void Validate(IMgSurfaceKHR surface, MgRect2D[] pRects)
		{
            if (surface == null)
                throw new ArgumentNullException(nameof(surface));
        }
	}
}
