using System;
namespace Magnesium.Toolkit.Validation.PhysicalDevice
{
	public static class GetPhysicalDevicePresentRectanglesKHR
	{
		public static void Validate(IMgSurfaceKHR surface, MgRect2D[] pRects)
		{
            if (surface == null)
                throw new ArgumentNullException(nameof(surface));
        }
	}
}
