using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class CreateImageView
	{
		public static void Validate(MgImageViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
