using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdCopyImageSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe void vkCmdCopyImage(IntPtr commandBuffer, UInt64 srcImage, MgImageLayout srcImageLayout, UInt64 dstImage, MgImageLayout dstImageLayout, UInt32 regionCount, Magnesium.MgImageCopy* pRegions);

        public static void CmdCopyImage(VkCommandBufferInfo info, IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageCopy[] pRegions)
        {
            var bSrcImage = (VkImage)srcImage;
            Debug.Assert(bSrcImage != null);

            var bDstImage = (VkImage)dstImage;
            Debug.Assert(bDstImage != null);


            var handle = GCHandle.Alloc(pRegions, GCHandleType.Pinned);

            try
            {
                unsafe
                {
                    var regionCount = (uint)pRegions.Length;
                    var region = handle.AddrOfPinnedObject();

                    MgImageCopy* regions = (MgImageCopy*)region.ToPointer();
                    vkCmdCopyImage(info.Handle, bSrcImage.Handle, srcImageLayout, bDstImage.Handle, dstImageLayout, regionCount, regions);
                }
            }
            finally
            {
                handle.Free();
            }
        }
    }
}
