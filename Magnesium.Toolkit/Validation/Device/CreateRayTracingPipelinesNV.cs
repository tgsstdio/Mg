using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public class CreateRayTracingPipelinesNV
	{
		public static void Validate(IMgPipelineCache pipelineCache, MgRayTracingPipelineCreateInfoNV[] pCreateInfos, IMgAllocationCallbacks pAllocator)
		{
            if (pCreateInfos == null)
                throw new ArgumentNullException(nameof(pCreateInfos));
        }
	}
}
