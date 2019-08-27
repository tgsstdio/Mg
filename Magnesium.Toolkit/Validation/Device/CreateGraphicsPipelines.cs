using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class CreateGraphicsPipelines
	{
		public static void Validate(IMgPipelineCache pipelineCache, MgGraphicsPipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfos == null)
                throw new ArgumentNullException(nameof(pCreateInfos));

            if (pCreateInfos.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pCreateInfos) + " == 0");
            }
        }
	}
}
