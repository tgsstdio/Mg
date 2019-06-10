using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.CommandBuffer
{
	public class VkBeginCommandBufferSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkBeginCommandBuffer(IntPtr commandBuffer, ref VkCommandBufferBeginInfo pBeginInfo);

        public static MgResult BeginCommandBuffer(VkCommandBufferInfo info, MgCommandBufferBeginInfo pBeginInfo)
        {
            IntPtr inheritanceInfo = IntPtr.Zero;
            try
            {
                var param_0 = new VkCommandBufferBeginInfo();
                param_0.sType = VkStructureType.StructureTypeCommandBufferBeginInfo;
                param_0.pNext = IntPtr.Zero;
                param_0.flags = (VkCommandBufferUsageFlags)pBeginInfo.Flags;

                if (pBeginInfo.InheritanceInfo != null)
                {
                    var ihData = new VkCommandBufferInheritanceInfo();
                    ihData.sType = VkStructureType.StructureTypeCommandBufferInheritanceInfo;
                    ihData.pNext = IntPtr.Zero;

                    {
                        UInt64 internalPtr = 0UL;
                        var container = pBeginInfo.InheritanceInfo.RenderPass;
                        if (container != null)
                        {
                            var rp = (VkRenderPass)container;
                            Debug.Assert(rp != null);
                            internalPtr = rp.Handle;
                        }
                        ihData.renderPass = internalPtr;
                    }

                    ihData.subpass = pBeginInfo.InheritanceInfo.Subpass;

                    {
                        UInt64 internalPtr = 0UL;
                        var container = pBeginInfo.InheritanceInfo.Framebuffer;
                        if (container != null)
                        {
                            var fb = (VkFramebuffer)container;
                            Debug.Assert(fb != null);
                            internalPtr = fb.Handle;
                        }
                        ihData.framebuffer = internalPtr;
                    }

                    ihData.occlusionQueryEnable = new VkBool32 { Value = pBeginInfo.InheritanceInfo.OcclusionQueryEnable ? 1U : 0U };
                    ihData.queryFlags = (VkQueryControlFlags)pBeginInfo.InheritanceInfo.QueryFlags;
                    ihData.pipelineStatistics = (VkQueryPipelineStatisticFlags)pBeginInfo.InheritanceInfo.PipelineStatistics;

                    // Copy data
                    inheritanceInfo = Marshal.AllocHGlobal(Marshal.SizeOf(ihData));
                    Marshal.StructureToPtr(ihData, inheritanceInfo, false);
                }

                param_0.pInheritanceInfo = inheritanceInfo;

                return vkBeginCommandBuffer(info.Handle, ref param_0);
            }
            finally
            {
                if (inheritanceInfo != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(inheritanceInfo);
                }
            }
        }
    }
}
