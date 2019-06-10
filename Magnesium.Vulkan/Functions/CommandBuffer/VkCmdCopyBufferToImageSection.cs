using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdCopyBufferToImageSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe void vkCmdCopyBufferToImage(IntPtr commandBuffer, UInt64 srcBuffer, UInt64 dstImage, MgImageLayout dstImageLayout, UInt32 regionCount, MgBufferImageCopy* pRegions);

        public static void CmdCopyBufferToImage(VkCommandBufferInfo info, IMgBuffer srcBuffer, IMgImage dstImage, MgImageLayout dstImageLayout, MgBufferImageCopy[] pRegions)
        {
            var bSrcBuffer = (VkBuffer)srcBuffer;
            Debug.Assert(bSrcBuffer != null);
            var bDstImage = (VkImage)dstImage;
            Debug.Assert(bDstImage != null);

            var handle = GCHandle.Alloc(pRegions, GCHandleType.Pinned);

            try
            {
                unsafe
                {
                    var regionCount = (uint)pRegions.Length;
                    var region = handle.AddrOfPinnedObject();

                    var regions = (MgBufferImageCopy*)region.ToPointer();

                    vkCmdCopyBufferToImage(info.Handle, bSrcBuffer.Handle, bDstImage.Handle, dstImageLayout, regionCount, regions);
                }
            }
            finally
            {
                handle.Free();
            }
        }
    }
}
