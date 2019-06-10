using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateIndirectCommandsLayoutNVXSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateIndirectCommandsLayoutNVX(IntPtr device, ref VkIndirectCommandsLayoutCreateInfoNVX pCreateInfo, IntPtr pAllocator, ref UInt64 pIndirectCommandsLayout);

        public static MgResult CreateIndirectCommandsLayoutNVX(VkDeviceInfo info, MgIndirectCommandsLayoutCreateInfoNVX pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgIndirectCommandsLayoutNVX pIndirectCommandsLayout)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(pAllocator);

            var pTokens = IntPtr.Zero;

            try
            {
                var tokenCount = (UInt32)pCreateInfo.Tokens.Length;

                pTokens = VkInteropsUtility.AllocateHGlobalStructArray(pCreateInfo.Tokens);

                var createInfo = new VkIndirectCommandsLayoutCreateInfoNVX
                {
                    sType = VkStructureType.StructureTypeIndirectCommandsLayoutCreateInfoNvx,
                    pNext = IntPtr.Zero,
                    pipelineBindPoint = pCreateInfo.PipelineBindPoint,
                    flags = pCreateInfo.Flags,
                    tokenCount = tokenCount,
                    pTokens = pTokens,
                };

                ulong bHandle = 0UL;
                var result = vkCreateIndirectCommandsLayoutNVX(info.Handle, ref createInfo, allocatorPtr, ref bHandle);

                pIndirectCommandsLayout = new VkIndirectCommandsLayoutNVX(bHandle);
                return result;
            }
            finally
            {
                if (pTokens != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pTokens);
                }
            }
        }
    }
}
