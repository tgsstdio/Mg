using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class CreateIndirectCommandsLayoutNVX
	{
		public static void Validate(MgIndirectCommandsLayoutCreateInfoNVX pCreateInfo, IMgAllocationCallbacks pAllocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));

            if (pCreateInfo.Tokens == null)
                throw new ArgumentNullException(nameof(pCreateInfo.Tokens));
        }
	}
}
