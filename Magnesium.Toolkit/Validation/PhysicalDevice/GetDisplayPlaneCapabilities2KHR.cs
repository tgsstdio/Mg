using System;
namespace Magnesium.Toolkit.Validation.PhysicalDevice
{
	public static class GetDisplayPlaneCapabilities2KHR
	{
		public static void Validate(MgDisplayPlaneInfo2KHR pDisplayPlaneInfo)
		{
            if (pDisplayPlaneInfo == null)
                throw new ArgumentNullException(nameof(pDisplayPlaneInfo));

            if (pDisplayPlaneInfo.Mode == null)
                throw new ArgumentNullException(nameof(pDisplayPlaneInfo.Mode));
        }
	}
}
