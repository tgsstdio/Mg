using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkCmdSetViewportSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe void vkCmdSetViewport(IntPtr commandBuffer, UInt32 firstViewport, UInt32 viewportCount, Magnesium.MgViewport* pViewports);

        public static void CmdSetViewport(VkCommandBufferInfo info, UInt32 firstViewport, MgViewport[] pViewports)
        {
            var viewportHandle = GCHandle.Alloc(pViewports, GCHandleType.Pinned);

            try
            {
                unsafe
                {
                    var viewportCount = (UInt32)pViewports.Length;
                    var pinnedObject = viewportHandle.AddrOfPinnedObject();

                    var viewports = (MgViewport*)pinnedObject.ToPointer();

                    vkCmdSetViewport(info.Handle, firstViewport, viewportCount, viewports);
                }
            }
            finally
            {
                viewportHandle.Free();
            }
        }
    }
}
