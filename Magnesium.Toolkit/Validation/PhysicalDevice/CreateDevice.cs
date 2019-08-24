using System;
namespace Magnesium.Toolkit.Validation.PhysicalDevice
{
	public class CreateDevice
	{
		public static void Validate(MgDeviceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
