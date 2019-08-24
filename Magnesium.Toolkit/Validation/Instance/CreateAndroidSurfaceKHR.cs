using System;
namespace Magnesium.Toolkit.Validation.Instance
{
	public class CreateAndroidSurfaceKHR
	{
		public static void Validate(MgAndroidSurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
