using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public class CreateDescriptorPool
	{
		public static void Validate(MgDescriptorPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
