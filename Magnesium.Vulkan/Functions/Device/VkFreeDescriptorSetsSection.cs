using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkFreeDescriptorSetsSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkFreeDescriptorSets(IntPtr device, UInt64 descriptorPool, UInt32 descriptorSetCount, [In, Out] UInt64[] pDescriptorSets);

        public static MgResult FreeDescriptorSets(VkDeviceInfo info, IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var bDescriptorPool = (VkDescriptorPool)descriptorPool;
            Debug.Assert(bDescriptorPool != null); // MAYBE DUPLICATE TESTING 

            var descriptorSetCount = (uint)pDescriptorSets.Length;
            var internalHandles = new ulong[descriptorSetCount];
            for (var i = 0; i < descriptorSetCount; ++i)
            {
                var bDescriptorSet = (VkDescriptorSet)pDescriptorSets[i];
                Debug.Assert(bDescriptorSet != null);
                internalHandles[i] = bDescriptorSet.Handle;
            }

            return vkFreeDescriptorSets(info.Handle, bDescriptorPool.Handle, descriptorSetCount, internalHandles);
        }
    }
}
