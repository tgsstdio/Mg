using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public class CreatePipelineCache
	{
		public static void Validate(MgPipelineCacheCreateInfo pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
