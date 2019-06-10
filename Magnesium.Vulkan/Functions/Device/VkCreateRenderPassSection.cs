using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateRenderPassSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateRenderPass(IntPtr device, ref VkRenderPassCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pRenderPass);

        public static MgResult CreateRenderPass(VkDeviceInfo info, MgRenderPassCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgRenderPass pRenderPass)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

            var attachedItems = new List<IntPtr>();

            try
            {
                var pAttachments = IntPtr.Zero;
                uint attachmentCount = pCreateInfo.Attachments != null ? (uint)pCreateInfo.Attachments.Length : 0U;
                if (attachmentCount > 0)
                {
                    pAttachments = VkInteropsUtility.AllocateHGlobalArray(
                        pCreateInfo.Attachments,
                        (attachment) =>
                        {
                            return new VkAttachmentDescription
                            {
                                flags = attachment.Flags,
                                format = attachment.Format,
                                samples = attachment.Samples,
                                loadOp = (VkAttachmentLoadOp)attachment.LoadOp,
                                storeOp = (VkAttachmentStoreOp)attachment.StoreOp,
                                stencilLoadOp = (VkAttachmentLoadOp)attachment.StencilLoadOp,
                                stencilStoreOp = (VkAttachmentStoreOp)attachment.StencilStoreOp,
                                initialLayout = attachment.InitialLayout,
                                finalLayout = attachment.FinalLayout
                            };
                        });
                    attachedItems.Add(pAttachments);
                }

                var attReferenceSize = Marshal.SizeOf(typeof(VkAttachmentReference));

                uint subpassCount = pCreateInfo.Subpasses != null ? (uint)pCreateInfo.Subpasses.Length : 0U;
                var pSubpasses = IntPtr.Zero;
                if (subpassCount > 0)
                {
                    var subPassDescriptionSize = Marshal.SizeOf(typeof(VkSubpassDescription));
                    pSubpasses = Marshal.AllocHGlobal((int)(subPassDescriptionSize * subpassCount));
                    attachedItems.Add(pSubpasses);

                    var subpassOffset = 0;
                    foreach (var currentSubpass in pCreateInfo.Subpasses)
                    {
                        var depthStencil = IntPtr.Zero;
                        if (currentSubpass.DepthStencilAttachment != null)
                        {
                            depthStencil = Marshal.AllocHGlobal(attReferenceSize);
                            var attachment = new VkAttachmentReference
                            {
                                attachment = currentSubpass.DepthStencilAttachment.Attachment,
                                layout = currentSubpass.DepthStencilAttachment.Layout,
                            };
                            Marshal.StructureToPtr(attachment, depthStencil, false);
                            attachedItems.Add(depthStencil);
                        }

                        var pInputAttachments = IntPtr.Zero;
                        var inputAttachmentCount =
                            currentSubpass.InputAttachments != null
                            ? (uint)currentSubpass.InputAttachments.Length
                            : 0;

                        if (inputAttachmentCount > 0)
                        {
                            pInputAttachments = VkInteropsUtility.AllocateHGlobalArray(
                                currentSubpass.InputAttachments,
                                (input) =>
                                {
                                    return new VkAttachmentReference
                                    {
                                        attachment = input.Attachment,
                                        layout = input.Layout,
                                    };
                                });
                            attachedItems.Add(pInputAttachments);
                        }

                        var colorAttachmentCount =
                            currentSubpass.ColorAttachments != null
                            ? (uint)currentSubpass.ColorAttachments.Length
                            : 0;

                        var pColorAttachments = IntPtr.Zero;
                        var pResolveAttachments = IntPtr.Zero;

                        if (colorAttachmentCount > 0)
                        {
                            pColorAttachments = VkInteropsUtility.AllocateHGlobalArray(
                                currentSubpass.ColorAttachments,
                                (color) =>
                                {
                                    return new VkAttachmentReference
                                    {
                                        attachment = color.Attachment,
                                        layout = color.Layout,
                                    };
                                });
                            attachedItems.Add(pColorAttachments);

                            if (currentSubpass.ResolveAttachments != null)
                            {
                                pResolveAttachments = VkInteropsUtility.AllocateHGlobalArray(
                                    currentSubpass.ResolveAttachments,
                                    (resolve) =>
                                    {
                                        return new VkAttachmentReference
                                        {
                                            attachment = resolve.Attachment,
                                            layout = resolve.Layout,
                                        };
                                    });
                                attachedItems.Add(pResolveAttachments);
                            }
                        }

                        var preserveAttachmentCount =
                            currentSubpass.PreserveAttachments != null
                            ? (uint)currentSubpass.PreserveAttachments.Length
                            : 0U;
                        var pPreserveAttachments = IntPtr.Zero;

                        if (preserveAttachmentCount > 0)
                        {
                            pPreserveAttachments = VkInteropsUtility.AllocateUInt32Array(currentSubpass.PreserveAttachments);
                            attachedItems.Add(pPreserveAttachments);
                        }

                        var description = new VkSubpassDescription
                        {
                            flags = currentSubpass.Flags,
                            pipelineBindPoint = currentSubpass.PipelineBindPoint,
                            inputAttachmentCount = inputAttachmentCount,
                            pInputAttachments = pInputAttachments,// VkAttachmentReference
                            colorAttachmentCount = colorAttachmentCount,
                            pColorAttachments = pColorAttachments, // VkAttachmentReference
                            pResolveAttachments = pResolveAttachments,
                            pDepthStencilAttachment = depthStencil,
                            preserveAttachmentCount = preserveAttachmentCount,
                            pPreserveAttachments = pPreserveAttachments, // uint
                        };

                        var dest = IntPtr.Add(pSubpasses, subpassOffset);
                        Marshal.StructureToPtr(description, dest, false);
                        subpassOffset += subPassDescriptionSize;
                    }
                }

                uint dependencyCount = pCreateInfo.Dependencies != null ? (uint)pCreateInfo.Dependencies.Length : 0U;
                var pDependencies = IntPtr.Zero;
                if (dependencyCount > 0)
                {
                    pDependencies = VkInteropsUtility.AllocateHGlobalArray(
                        pCreateInfo.Dependencies,
                         (src) => {
                             return new VkSubpassDependency
                             {
                                 srcSubpass = src.SrcSubpass,
                                 dstSubpass = src.DstSubpass,
                                 srcStageMask = src.SrcStageMask,
                                 dstStageMask = src.DstStageMask,
                                 srcAccessMask = src.SrcAccessMask,
                                 dstAccessMask = src.DstAccessMask,
                                 dependencyFlags = src.DependencyFlags,
                             };
                         });
                    attachedItems.Add(pDependencies);
                }

                var createInfo = new VkRenderPassCreateInfo
                {
                    sType = VkStructureType.StructureTypeRenderPassCreateInfo,
                    pNext = IntPtr.Zero,
                    flags = pCreateInfo.Flags,
                    attachmentCount = attachmentCount,
                    pAttachments = pAttachments,
                    subpassCount = subpassCount,
                    pSubpasses = pSubpasses,
                    dependencyCount = dependencyCount,
                    pDependencies = pDependencies,
                };

                ulong internalHandle = 0;
                var result = vkCreateRenderPass(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
                pRenderPass = new VkRenderPass(internalHandle);
                return result;
            }
            finally
            {
                foreach (var ptr in attachedItems)
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }
    }
}
