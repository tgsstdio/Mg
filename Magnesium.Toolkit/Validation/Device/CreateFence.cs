using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public class CreateFence
	{
		public static void Validate(MgFenceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
