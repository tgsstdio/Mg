using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdClearAttachmentsSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe void vkCmdClearAttachments(IntPtr commandBuffer, UInt32 attachmentCount, Magnesium.MgClearAttachment* pAttachments, UInt32 rectCount, Magnesium.MgClearRect* pRects);

        public static void CmdClearAttachments(VkCommandBufferInfo info, MgClearAttachment[] pAttachments, MgClearRect[] pRects)
        {
            var attachmentHandle = GCHandle.Alloc(pAttachments, GCHandleType.Pinned);
            var rectsHandle = GCHandle.Alloc(pRects, GCHandleType.Pinned);

            try
            {
                unsafe
                {
                    var attachmentCount = (uint)pAttachments.Length;
                    var attachment = attachmentHandle.AddrOfPinnedObject();

                    var rectCount = (uint)pRects.Length;
                    var rects = rectsHandle.AddrOfPinnedObject();
                    vkCmdClearAttachments(info.Handle, attachmentCount, (Magnesium.MgClearAttachment*)attachment.ToPointer(), rectCount, (Magnesium.MgClearRect*)rects.ToPointer());
                }
            }
            finally
            {
                rectsHandle.Free();
                attachmentHandle.Free();
            }
        }
    }
}
