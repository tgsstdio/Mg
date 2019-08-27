using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdBeginConditionalRenderingEXTSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdBeginConditionalRenderingEXT(IntPtr commandBuffer, ref VkConditionalRenderingBeginInfoEXT pConditionalRenderingBegin);

        public static void CmdBeginConditionalRenderingEXT(VkCommandBufferInfo info, MgConditionalRenderingBeginInfoEXT pConditionalRenderingBegin)
        {
            if (pConditionalRenderingBegin == null)
            {
                throw new ArgumentNullException(nameof(pConditionalRenderingBegin));
            }

            var bBuffer = (VkBuffer)pConditionalRenderingBegin.Buffer;
            Debug.Assert(bBuffer != null);

            var pBegin = new VkConditionalRenderingBeginInfoEXT
            {
                sType = VkStructureType.StructureTypeConditionalRenderingBeginInfoExt,
                pNext = IntPtr.Zero,
                buffer = bBuffer.Handle,
                offset = pConditionalRenderingBegin.Offset,
                flags = pConditionalRenderingBegin.Flags,
            };

            vkCmdBeginConditionalRenderingEXT(info.Handle, ref pBegin);
        }
    }
}
