using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class CreateRenderPass2KHR
	{
		public static void Validate(MgRenderPassCreateInfo2KHR pCreateInfo, IMgAllocationCallbacks pAllocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
