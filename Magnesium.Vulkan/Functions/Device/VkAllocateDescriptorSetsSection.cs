using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkAllocateDescriptorSetsSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkAllocateDescriptorSets(IntPtr device, ref VkDescriptorSetAllocateInfo pAllocateInfo, [In, Out] UInt64[] pDescriptorSets);

        public static MgResult AllocateDescriptorSets(VkDeviceInfo info, MgDescriptorSetAllocateInfo pAllocateInfo, out IMgDescriptorSet[] pDescriptorSets)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var bDescriptorPool = (VkDescriptorPool)pAllocateInfo.DescriptorPool;
            Debug.Assert(bDescriptorPool != null);

            var pSetLayouts = IntPtr.Zero;

            try
            {
                var descriptorSetCount = pAllocateInfo.DescriptorSetCount;
                if (descriptorSetCount > 0)
                {
                    pSetLayouts = VkInteropsUtility.ExtractUInt64HandleArray(pAllocateInfo.SetLayouts,
                     (dsl) =>
                     {
                         var bSetLayout = (VkDescriptorSetLayout)dsl;
                         Debug.Assert(bSetLayout != null);
                         return bSetLayout.Handle;
                     });
                }

                var allocateInfo = new VkDescriptorSetAllocateInfo
                {
                    sType = VkStructureType.StructureTypeDescriptorSetAllocateInfo,
                    pNext = IntPtr.Zero,
                    descriptorPool = bDescriptorPool.Handle,
                    descriptorSetCount = pAllocateInfo.DescriptorSetCount,
                    pSetLayouts = pSetLayouts,
                };

                var internalHandles = new ulong[pAllocateInfo.DescriptorSetCount];
                var result = vkAllocateDescriptorSets(info.Handle, ref allocateInfo, internalHandles);

                pDescriptorSets = new VkDescriptorSet[pAllocateInfo.DescriptorSetCount];
                for (var i = 0; i < pAllocateInfo.DescriptorSetCount; ++i)
                {
                    pDescriptorSets[i] = new VkDescriptorSet(internalHandles[i]);
                }
                return result;
            }
            finally
            {
                if (pSetLayouts != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pSetLayouts);
                }
            }
        }
    }
}
