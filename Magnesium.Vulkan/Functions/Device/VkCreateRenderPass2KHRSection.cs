using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateRenderPass2KHRSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateRenderPass2KHR(IntPtr device, ref VkRenderPassCreateInfo2KHR pCreateInfo, IntPtr pAllocator, ref UInt64 pRenderPass);

        public static MgResult CreateRenderPass2KHR(VkDeviceInfo info, MgRenderPassCreateInfo2KHR pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgRenderPass pRenderPass)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkInteropsUtility.GetAllocatorHandle(pAllocator);

            var allocatedItems = new List<IntPtr>();

            try
            {
                var pAttachments = IntPtr.Zero;
                var attachmentCount = pCreateInfo.Attachments != null
                    ? (uint)pCreateInfo.Attachments.Length
                    : 0U;
                if (attachmentCount > 0)
                {
                    pAttachments = VkInteropsUtility.AllocateHGlobalArray(
                        pCreateInfo.Attachments,
                        (attachment) =>
                        {
                            return new VkAttachmentDescription2KHR
                            {
                                sType = VkStructureType.StructureTypeAttachmentDescription2Khr,
                                // TODO: extension here
                                pNext = IntPtr.Zero,
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
                    allocatedItems.Add(pAttachments);
                }

                var subpassCount = pCreateInfo.Subpasses != null
                    ? (uint)pCreateInfo.Subpasses.Length
                    : 0U;
                var pSubpasses = IntPtr.Zero;
                if (subpassCount > 0)
                {
                    pSubpasses = VkInteropsUtility.AllocateHGlobalArray
                        <MgSubpassDescription2KHR, VkSubpassDescription2KHR>
                        (
                            pCreateInfo.Subpasses,
                            (src) =>
                            {
                                var inputAttachmentCount =
                                    src.InputAttachments != null
                                    ? (uint)src.InputAttachments.Length
                                    : 0U;

                                var pInputAttachments = IntPtr.Zero;
                                if (inputAttachmentCount > 0)
                                {
                                    pInputAttachments = VkInteropsUtility.AllocateHGlobalArray
                                        <MgAttachmentReference2KHR, VkAttachmentReference2KHR>
                                        (
                                            src.InputAttachments,
                                            (input) =>
                                            {
                                                return new VkAttachmentReference2KHR
                                                {
                                                    sType = VkStructureType.StructureTypeAttachmentReference2Khr,
                                                    // TODO: extension here
                                                    pNext = IntPtr.Zero,
                                                    attachment = input.Attachment,
                                                    layout = input.Layout,
                                                    aspectMask = input.AspectMask,
                                                };
                                            }
                                        );
                                    allocatedItems.Add(pInputAttachments);
                                }

                                var colorAttachmentCount =
                                    src.ColorAndResolveAttachments != null
                                    ? (uint)src.ColorAndResolveAttachments.Length
                                    : 0U;
                                var pColorAttachments = IntPtr.Zero;
                                var pResolveAttachments = IntPtr.Zero;

                                if (colorAttachmentCount > 0)
                                {
                                    pColorAttachments = VkInteropsUtility.AllocateHGlobalArray
                                        <MgColorAndResolveAttachmentInfo, VkAttachmentReference2KHR>
                                        (
                                            src.ColorAndResolveAttachments,
                                            (cnr) =>
                                            {
                                                return new VkAttachmentReference2KHR
                                                {
                                                    sType = VkStructureType.StructureTypeAttachmentReference2Khr,
                                                    // TODO: extension here
                                                    pNext = IntPtr.Zero,
                                                    attachment = cnr.Color.Attachment,
                                                    layout = cnr.Color.Layout,
                                                    aspectMask = cnr.Color.AspectMask,
                                                };
                                            }
                                        );
                                    allocatedItems.Add(pColorAttachments);

                                    pResolveAttachments = VkInteropsUtility.AllocateHGlobalArray
                                        <MgColorAndResolveAttachmentInfo, VkAttachmentReference2KHR>
                                        (
                                            src.ColorAndResolveAttachments,
                                            (cnr) =>
                                            {
                                                return new VkAttachmentReference2KHR
                                                {
                                                    sType = VkStructureType.StructureTypeAttachmentReference2Khr,
                                                    // TODO: extension here
                                                    pNext = IntPtr.Zero,
                                                    attachment = cnr.Resolve.Attachment,
                                                    layout = cnr.Resolve.Layout,
                                                    aspectMask = cnr.Resolve.AspectMask,
                                                };
                                            }
                                        );
                                    allocatedItems.Add(pResolveAttachments);
                                }

                                var preserveAttachmentCount = src.PreserveAttachments != null
                                    ? (uint)src.PreserveAttachments.Length
                                    : 0U;
                                var pPreserveAttachments = IntPtr.Zero;

                                if (preserveAttachmentCount > 0)
                                {
                                    pPreserveAttachments = VkInteropsUtility.AllocateUInt32Array(src.PreserveAttachments);
                                    allocatedItems.Add(pPreserveAttachments);
                                }

                                return new VkSubpassDescription2KHR
                                {
                                    sType = VkStructureType.StructureTypeSubpassDescription2Khr,
                                    // TODO: extension here
                                    pNext = IntPtr.Zero,
                                    flags = src.Flags,
                                    pipelineBindPoint = src.PipelineBindPoint,
                                    viewMask = src.ViewMask,
                                    inputAttachmentCount = inputAttachmentCount,
                                    pInputAttachments = pInputAttachments,
                                    colorAttachmentCount = colorAttachmentCount,
                                    pColorAttachments = pColorAttachments,
                                    pResolveAttachments = pResolveAttachments,
                                    pDepthStencilAttachment = ExtractDepthStencilAttachment2KHR(
                                        allocatedItems,
                                        src.DepthStencilAttachment),
                                    preserveAttachmentCount = preserveAttachmentCount,
                                    pPreserveAttachments = pPreserveAttachments,
                                };
                            }
                        );
                }

                var dependencyCount = pCreateInfo.Dependencies != null ?
                    (uint)pCreateInfo.Dependencies.Length : 0U;
                var pDependencies = IntPtr.Zero;
                if (dependencyCount > 0)
                {
                    pDependencies = VkInteropsUtility.AllocateHGlobalArray(
                        pCreateInfo.Dependencies,
                         (src) => {
                             return new VkSubpassDependency2KHR
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
                    allocatedItems.Add(pDependencies);
                }

                var correlatedViewMaskCount = pCreateInfo.CorrelatedViewMasks != null
                    ? (uint)pCreateInfo.CorrelatedViewMasks.Length
                    : 0U;

                var pCorrelatedViewMasks = IntPtr.Zero;

                if (correlatedViewMaskCount > 0)
                {
                    pCorrelatedViewMasks = VkInteropsUtility.AllocateUInt32Array(pCreateInfo.CorrelatedViewMasks);
                    allocatedItems.Add(pCorrelatedViewMasks);
                }

                var createInfo = new VkRenderPassCreateInfo2KHR
                {
                    sType = VkStructureType.StructureTypeRenderPassCreateInfo2Khr,
                    pNext = IntPtr.Zero,
                    flags = pCreateInfo.Flags,
                    attachmentCount = attachmentCount,
                    pAttachments = pAttachments,
                    subpassCount = subpassCount,
                    pSubpasses = pSubpasses,
                    dependencyCount = dependencyCount,
                    pDependencies = pDependencies,
                    correlatedViewMaskCount = correlatedViewMaskCount,
                    pCorrelatedViewMasks = pCorrelatedViewMasks,
                };

                ulong internalHandle = 0;
                var result = vkCreateRenderPass2KHR(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
                pRenderPass = new VkRenderPass(internalHandle);
                return result;
            }
            finally
            {
                foreach (var ptr in allocatedItems)
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        private static IntPtr ExtractDepthStencilAttachment2KHR(List<IntPtr> allocatedItems, MgAttachmentReference2KHR src)
        {
            var result = IntPtr.Zero;

            if (src != null)
            {
                result = VkInteropsUtility.AllocateHGlobalStructArray(
                    new[]
                    {
                        new VkAttachmentReference2KHR
                        {
                            sType = VkStructureType.StructureTypeAttachmentReference2Khr,
                            // TODO: extension here
                            pNext = IntPtr.Zero,
                            attachment = src.Attachment,
                            layout = src.Layout,
                            aspectMask = src.AspectMask,

                        }
                    }
                );
                allocatedItems.Add(result);
            }

            return result;
        }
    }
}
