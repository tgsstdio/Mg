using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdBeginRenderPassSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdBeginRenderPass(IntPtr commandBuffer, ref VkRenderPassBeginInfo pRenderPassBegin, VkSubpassContents contents);

        public static void CmdBeginRenderPass(VkCommandBufferInfo info, MgRenderPassBeginInfo pRenderPassBegin, MgSubpassContents contents)
        {
            if (pRenderPassBegin == null)
            {
                throw new ArgumentNullException(nameof(pRenderPassBegin));
            }

            var bRenderPass = (VkRenderPass)pRenderPassBegin.RenderPass;
            Debug.Assert(bRenderPass != null);
            var bFrameBuffer = (VkFramebuffer)pRenderPassBegin.Framebuffer;
            Debug.Assert(bFrameBuffer != null);

            var clearValueCount = pRenderPassBegin.ClearValues != null ? (UInt32)pRenderPassBegin.ClearValues.Length : 0U;

            var clearValues = IntPtr.Zero;

            try
            {
                if (clearValueCount > 0)
                {
                    var stride = Marshal.SizeOf(typeof(MgClearValue));
                    clearValues = Marshal.AllocHGlobal((int)(stride * clearValueCount));

                    for (uint i = 0; i < clearValueCount; ++i)
                    {
                        IntPtr dest = IntPtr.Add(clearValues, (int)(i * stride));
                        Marshal.StructureToPtr(pRenderPassBegin.ClearValues[i], dest, false);
                    }
                }

                var beginInfo = new VkRenderPassBeginInfo
                {
                    sType = VkStructureType.StructureTypeRenderPassBeginInfo,
                    pNext = IntPtr.Zero,
                    renderPass = bRenderPass.Handle,
                    framebuffer = bFrameBuffer.Handle,
                    renderArea = pRenderPassBegin.RenderArea,
                    clearValueCount = clearValueCount,
                    pClearValues = clearValues,
                };

                vkCmdBeginRenderPass(info.Handle, ref beginInfo, (Magnesium.Vulkan.VkSubpassContents)contents);
            }
            finally
            {
                if (clearValues != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(clearValues);
                }
            }
        }
    }
}
