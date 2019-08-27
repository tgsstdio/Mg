using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class CreateSharedSwapchainsKHR
	{
		public static void Validate(MgSwapchainCreateInfoKHR[] pCreateInfos, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfos == null)
                throw new ArgumentNullException(nameof(pCreateInfos));
        }
	}
}
