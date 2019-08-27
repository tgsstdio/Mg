using System;
namespace Magnesium.Toolkit.Validation.PhysicalDevice
{
	public static class CreateDisplayModeKHR
	{
		public static void Validate(IMgDisplayKHR display, MgDisplayModeCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (display == null)
                throw new ArgumentNullException(nameof(display));

            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
    }
}
