using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdCopyImageToBufferSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe void vkCmdCopyImageToBuffer(IntPtr commandBuffer, UInt64 srcImage, MgImageLayout srcImageLayout, UInt64 dstBuffer, UInt32 regionCount, MgBufferImageCopy* pRegions);

        public static void CmdCopyImageToBuffer(VkCommandBufferInfo info, IMgImage srcImage, MgImageLayout srcImageLayout, IMgBuffer dstBuffer, MgBufferImageCopy[] pRegions)
        {
            var bSrcImage = (VkImage)srcImage;
            Debug.Assert(bSrcImage != null);
            var bDstBuffer = (VkBuffer)dstBuffer;
            Debug.Assert(bDstBuffer != null);

            var handle = GCHandle.Alloc(pRegions, GCHandleType.Pinned);

            try
            {
                unsafe
                {
                    var regionCount = (uint)pRegions.Length;
                    var regionAddress = handle.AddrOfPinnedObject();

                    var pinnedArray = (MgBufferImageCopy*)regionAddress.ToPointer();

                    vkCmdCopyImageToBuffer(info.Handle, bSrcImage.Handle, srcImageLayout, bDstBuffer.Handle, regionCount, pinnedArray);
                }
            }
            finally
            {
                handle.Free();
            }
        }
    }
}
