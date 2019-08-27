using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class CreatePipelineLayout
	{
		public static void Validate(MgPipelineLayoutCreateInfo pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
