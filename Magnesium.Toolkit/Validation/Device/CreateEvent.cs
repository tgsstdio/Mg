using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class CreateEvent
	{
		public static void Validate(MgEventCreateInfo pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
