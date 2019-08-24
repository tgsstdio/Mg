using System;
namespace Magnesium.Toolkit.Validation.PhysicalDevice
{
	public class GetDisplayModeProperties2KHR
	{
		public static void Validate(IMgDisplayKHR display)
		{
            if (display == null)
                throw new ArgumentNullException(nameof(display));
        }
	}
}
