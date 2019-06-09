using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public class AllocateCommandBuffers
	{
		public static void Validate(MgCommandBufferAllocateInfo pAllocateInfo, IMgCommandBuffer[] pCommandBuffers)
		{
            if (pAllocateInfo == null)
                throw new ArgumentNullException(nameof(pAllocateInfo));

            if (pCommandBuffers == null)
                throw new ArgumentNullException(nameof(pCommandBuffers));

            if (pAllocateInfo.CommandBufferCount != pCommandBuffers.Length)
                throw new ArgumentOutOfRangeException(nameof(pAllocateInfo.CommandBufferCount) + " !=  " + nameof(pCommandBuffers.Length));
        }
	}
}
