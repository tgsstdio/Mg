using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdResolveImageSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe void vkCmdResolveImage(IntPtr commandBuffer, UInt64 srcImage, MgImageLayout srcImageLayout, UInt64 dstImage, MgImageLayout dstImageLayout, UInt32 regionCount, MgImageResolve* pRegions);

        public static void CmdResolveImage(VkCommandBufferInfo info, IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageResolve[] pRegions)
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
                    var regionAddress = handle.AddrOfPinnedObject();

                    var pinnedArray = (MgImageResolve*)regionAddress.ToPointer();

                    vkCmdResolveImage(info.Handle, bSrcImage.Handle, srcImageLayout, bDstImage.Handle, dstImageLayout, regionCount, pinnedArray);
                }
            }
            finally
            {
                handle.Free();
            }
        }
    }
}
