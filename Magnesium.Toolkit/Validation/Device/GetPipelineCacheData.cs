using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class GetPipelineCacheData
	{
		public static void Validate(IMgPipelineCache pipelineCache)
		{
            if (pipelineCache == null)
                throw new ArgumentNullException(nameof(pipelineCache));
        }
	}
}
