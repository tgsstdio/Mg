using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public class CreateComputePipelines
	{
		public static void Validate(IMgPipelineCache pipelineCache, MgComputePipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfos == null)
                throw new ArgumentNullException(nameof(pCreateInfos));
        }
	}
}
