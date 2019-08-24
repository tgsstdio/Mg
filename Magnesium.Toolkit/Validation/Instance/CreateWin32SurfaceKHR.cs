using System;
namespace Magnesium.Toolkit.Validation.Instance
{
	public class CreateWin32SurfaceKHR
	{
		public static void Validate(MgWin32SurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
