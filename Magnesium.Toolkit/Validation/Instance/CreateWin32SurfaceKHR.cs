using System;
namespace Magnesium.Toolkit.Validation.Instance
{
	public static class CreateWin32SurfaceKHR
	{
		public static void Validate(MgWin32SurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
