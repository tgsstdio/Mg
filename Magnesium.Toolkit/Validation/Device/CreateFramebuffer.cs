using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class CreateFramebuffer
	{
		public static void Validate(MgFramebufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
