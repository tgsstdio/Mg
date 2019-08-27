using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class CreateSwapchainKHR
	{
		public static void Validate(MgSwapchainCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
