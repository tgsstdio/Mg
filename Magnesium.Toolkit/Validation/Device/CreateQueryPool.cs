using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public class CreateQueryPool
	{
		public static void Validate(MgQueryPoolCreateInfo pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
