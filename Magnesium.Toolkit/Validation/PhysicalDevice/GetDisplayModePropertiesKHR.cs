using System;
namespace Magnesium.Toolkit.Validation.PhysicalDevice
{
	public static class GetDisplayModePropertiesKHR
	{
		public static void Validate(IMgDisplayKHR display)
		{
            if (display == null)
                throw new ArgumentNullException(nameof(display));
        }
	}
}
