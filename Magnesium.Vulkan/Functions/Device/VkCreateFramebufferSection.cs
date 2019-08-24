using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateFramebufferSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateFramebuffer(IntPtr device, ref VkFramebufferCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pFramebuffer);

        public static MgResult CreateFramebuffer(VkDeviceInfo info, MgFramebufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFramebuffer pFramebuffer)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkInteropsUtility.GetAllocatorHandle(allocator);

            var bRenderPass = (VkRenderPass)pCreateInfo.RenderPass;
            Debug.Assert(bRenderPass != null);

            var attachmentCount = 0U;
            var pAttachments = IntPtr.Zero;

            try
            {
                if (pCreateInfo.Attachments != null)
                {
                    attachmentCount = (uint)pCreateInfo.Attachments.Length;
                    if (attachmentCount > 0)
                    {
                        pAttachments = VkInteropsUtility.ExtractUInt64HandleArray(pCreateInfo.Attachments,
                            (a) =>
                            {
                                var bImageView = (VkImageView)a;
                                Debug.Assert(bImageView != null);
                                return bImageView.Handle;
                            });
                    }
                }

                var createInfo = new VkFramebufferCreateInfo
                {
                    sType = VkStructureType.StructureTypeFramebufferCreateInfo,
                    pNext = IntPtr.Zero,
                    flags = pCreateInfo.Flags,
                    renderPass = bRenderPass.Handle,
                    attachmentCount = attachmentCount,
                    pAttachments = pAttachments,
                    width = pCreateInfo.Width,
                    height = pCreateInfo.Height,
                    layers = pCreateInfo.Layers,
                };

                var internalHandle = 0UL;
                var result = vkCreateFramebuffer(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
                pFramebuffer = new VkFramebuffer(internalHandle);
                return result;
            }
            finally
            {
                if (pAttachments != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAttachments);
                }
            }
        }
    }
}
