using System;
namespace Magnesium.Toolkit.Validation.PhysicalDevice
{
	public static class GetPhysicalDeviceSurfaceFormats2KHR
	{
		public static void Validate(MgPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo)
		{
            if (pSurfaceInfo == null)
                throw new ArgumentNullException(nameof(pSurfaceInfo));
        }
	}
}
