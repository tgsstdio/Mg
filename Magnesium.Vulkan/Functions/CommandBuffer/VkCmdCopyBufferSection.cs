using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdCopyBufferSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe void vkCmdCopyBuffer(IntPtr commandBuffer, UInt64 srcBuffer, UInt64 dstBuffer, UInt32 regionCount, Magnesium.MgBufferCopy* pRegions);

        public static void CmdCopyBuffer(VkCommandBufferInfo info, IMgBuffer srcBuffer, IMgBuffer dstBuffer, MgBufferCopy[] pRegions)
        {
            var bBuffer_src = (VkBuffer)srcBuffer;
            Debug.Assert(bBuffer_src != null);

            var bBuffer_dst = (VkBuffer)dstBuffer;
            Debug.Assert(bBuffer_dst != null);

            var handle = GCHandle.Alloc(pRegions, GCHandleType.Pinned);

            try
            {
                unsafe
                {
                    var regionCount = (uint)pRegions.Length;
                    var region = handle.AddrOfPinnedObject();

                    MgBufferCopy* regions = (MgBufferCopy*)region.ToPointer();

                    vkCmdCopyBuffer(info.Handle, bBuffer_src.Handle, bBuffer_dst.Handle, regionCount, regions);
                }
            }
            finally
            {
                handle.Free();
            }
        }
    }
}
