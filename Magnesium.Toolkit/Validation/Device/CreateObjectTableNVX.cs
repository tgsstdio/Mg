using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class CreateObjectTableNVX
	{
		public static void Validate(MgObjectTableCreateInfoNVX pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));

            if (pCreateInfo.Entries == null)
                throw new ArgumentNullException(nameof(pCreateInfo.Entries));
        }
	}
}
