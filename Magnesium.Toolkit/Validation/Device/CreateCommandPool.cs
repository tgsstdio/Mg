using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class CreateCommandPool
	{
		public static void Validate(MgCommandPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
