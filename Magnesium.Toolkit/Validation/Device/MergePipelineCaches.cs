using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public class MergePipelineCaches
	{
		public static void Validate(IMgPipelineCache dstCache, IMgPipelineCache[] pSrcCaches)
		{
            if (pSrcCaches == null)
                throw new ArgumentNullException(nameof(pSrcCaches));
        }
	}
}
