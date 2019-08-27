using System;
namespace Magnesium.Toolkit.Validation.PhysicalDevice
{
	public static class GetDisplayPlaneCapabilitiesKHR
	{
		public static void Validate(IMgDisplayModeKHR mode, UInt32 planeIndex)
		{
            if (mode == null)
                throw new ArgumentNullException(nameof(mode));
        }
	}
}
