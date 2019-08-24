using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public class CreateRenderPass
	{
		public static void Validate(MgRenderPassCreateInfo pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
