using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public static class VkCmdProcessCommandsNVXSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkCmdProcessCommandsNVX(IntPtr commandBuffer, VkCmdProcessCommandsInfoNVX pProcessCommandsInfo);

        public static void CmdProcessCommandsNVX(VkCommandBufferInfo info, MgCmdProcessCommandsInfoNVX pProcessCommandsInfo)
        {
            var bObjectTable = (VkObjectTableNVX)pProcessCommandsInfo.ObjectTable;
            Debug.Assert(bObjectTable != null);
            var bLayout = (VkIndirectCommandsLayoutNVX)pProcessCommandsInfo.IndirectCommandsLayout;
            Debug.Assert(bLayout != null);
            var bTargetCommandBuffer = (VkCommandBuffer)pProcessCommandsInfo.TargetCommandBuffer;
            Debug.Assert(bTargetCommandBuffer != null);
            var bSequenceCountBuffer = (VkBuffer)pProcessCommandsInfo.SequencesCountBuffer;
            Debug.Assert(bSequenceCountBuffer != null);
            var bSequenceIndexBuffer = (VkBuffer)pProcessCommandsInfo.SequencesIndexBuffer;

            var indirectCommandsTokenCount = pProcessCommandsInfo.IndirectCommandsTokens != null
                ? (UInt32)pProcessCommandsInfo.IndirectCommandsTokens.Length : 0U;

            var indirectCommandTokens = IntPtr.Zero;

            try
            {
                indirectCommandTokens = VkInteropsUtility.AllocateHGlobalArray
                    (
                        pProcessCommandsInfo.IndirectCommandsTokens,
                        (token) =>
                        {
                            var bBuffer = (VkBuffer)token.Buffer;
                            Debug.Assert(bBuffer != null);

                            return new VkIndirectCommandsTokenNVX
                            {
                                tokenType = token.TokenType,
                                buffer = bBuffer.Handle,
                                offset = token.Offset,
                            };
                        }
                    );

                var processCommands = new VkCmdProcessCommandsInfoNVX
                {
                    sType = VkStructureType.StructureTypeCmdProcessCommandsInfoNvx,
                    pNext = IntPtr.Zero,
                    objectTable = bObjectTable.Handle,
                    indirectCommandsLayout = bLayout.Handle,
                    indirectCommandsTokenCount = indirectCommandsTokenCount,
                    pIndirectCommandsTokens = indirectCommandTokens,
                    maxSequencesCount = pProcessCommandsInfo.MaxSequencesCount,
                    targetCommandBuffer = bTargetCommandBuffer.Info.Handle,
                    sequencesCountBuffer = bSequenceCountBuffer.Handle,
                    sequencesCountOffset = pProcessCommandsInfo.SequencesCountOffset,
                    sequencesIndexBuffer = bSequenceIndexBuffer.Handle,
                    sequencesIndexOffset = pProcessCommandsInfo.SequencesIndexOffset,
                };

                vkCmdProcessCommandsNVX(
                    info.Handle,
                    processCommands);
            }
            finally
            {
                if (indirectCommandTokens != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(indirectCommandTokens);
                }
            }
        }
    }
}
