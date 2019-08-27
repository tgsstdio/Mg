using System;
namespace Magnesium.Toolkit.Validation.PhysicalDevice
{
	public static class GetDisplayModeProperties2KHR
	{
		public static void Validate(IMgDisplayKHR display)
		{
            if (display == null)
                throw new ArgumentNullException(nameof(display));
        }
	}
}
