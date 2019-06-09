using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public class CreateSemaphore
	{
		public static void Validate(MgSemaphoreCreateInfo pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
