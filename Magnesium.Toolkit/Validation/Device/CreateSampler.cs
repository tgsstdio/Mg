using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public class CreateSampler
	{
		public static void Validate(MgSamplerCreateInfo pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
