using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkFreeCommandBuffersSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkFreeCommandBuffers(IntPtr device, UInt64 commandPool, UInt32 commandBufferCount, [In] IntPtr[] pCommandBuffers);

        public static void FreeCommandBuffers(VkDeviceInfo info, IMgCommandPool commandPool, IMgCommandBuffer[] pCommandBuffers)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var bCommandPool = (VkCommandPool)commandPool;
            Debug.Assert(bCommandPool != null);

            var commandBufferCount = pCommandBuffers != null ? (uint)pCommandBuffers.Length : 0U;

            if (commandBufferCount > 0)
            {
                var bufferHandles = new IntPtr[commandBufferCount];
                for (var i = 0; i < commandBufferCount; ++i)
                {
                    var bCommandBuffer = (VkCommandBuffer)pCommandBuffers[i];
                    bufferHandles[i] = bCommandBuffer.Info.Handle;
                }

                vkFreeCommandBuffers(info.Handle, bCommandPool.Handle, commandBufferCount, bufferHandles);
            }
        }
    }
}
