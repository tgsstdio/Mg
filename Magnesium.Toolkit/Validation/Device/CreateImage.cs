using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public class CreateImage
	{
		public static void Validate(MgImageCreateInfo pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
