using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Queue
{
	public static class VkGetQueueCheckpointDataNVSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkGetQueueCheckpointDataNV(IntPtr queue, ref UInt32 pCheckpointDataCount, [In, Out] VkCheckpointDataNV[] pCheckpointData);

        public static void GetQueueCheckpointDataNV(VkQueueInfo info, out MgCheckpointDataNV[] pCheckpointData)
        {
            var pCheckpointDataCount = 0U;

            vkGetQueueCheckpointDataNV(info.Handle, ref pCheckpointDataCount, null);

            pCheckpointData = new MgCheckpointDataNV[pCheckpointDataCount];
            if (pCheckpointDataCount > 0)
            {
                var bCheckpointData = new VkCheckpointDataNV[pCheckpointDataCount];

                vkGetQueueCheckpointDataNV(info.Handle, ref pCheckpointDataCount, bCheckpointData);

                for (var i = 0U; i < pCheckpointDataCount; i += 1)
                {
                    var current = bCheckpointData[i];

                    pCheckpointData[i] = new MgCheckpointDataNV
                    {
                        Stage = current.stage,
                        CheckpointMarker = current.pCheckpointMarker,
                    };
                }
            }
        }
    }
}
