using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class CreateDescriptorSetLayout
	{
		public static void Validate(MgDescriptorSetLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
