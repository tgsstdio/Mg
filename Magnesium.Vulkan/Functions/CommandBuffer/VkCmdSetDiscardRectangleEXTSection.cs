using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdSetDiscardRectangleEXTSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe void vkCmdSetDiscardRectangleEXT(IntPtr commandBuffer, UInt32 firstDiscardRectangle, UInt32 discardRectangleCount, MgRect2D* pDiscardRectangles);

        public static void CmdSetDiscardRectangleEXT(VkCommandBufferInfo info, UInt32 firstDiscardRectangle, MgRect2D[] discardRectangles)
        {
            unsafe
            {
                var pDiscardRectangles = GCHandle.Alloc(discardRectangles, GCHandleType.Pinned);

                try
                {
                    var discardRectangleCount = discardRectangles != null ? (uint)discardRectangles.Length : 0U;
                    var pinnedObject = pDiscardRectangles.AddrOfPinnedObject();
                    var bRects = (MgRect2D*)pinnedObject.ToPointer();

                    vkCmdSetDiscardRectangleEXT(info.Handle, firstDiscardRectangle, discardRectangleCount, bRects);
                }
                finally
                {
                    pDiscardRectangles.Free();
                }
            }
        }
    }
}
