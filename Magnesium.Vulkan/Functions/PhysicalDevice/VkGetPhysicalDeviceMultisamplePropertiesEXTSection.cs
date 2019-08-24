using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDeviceMultisamplePropertiesEXTSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkGetPhysicalDeviceMultisamplePropertiesEXT(IntPtr physicalDevice, MgSampleCountFlagBits samples, ref VkMultisamplePropertiesEXT pMultisampleProperties);

        public static void GetPhysicalDeviceMultisamplePropertiesEXT(VkPhysicalDeviceInfo info, MgSampleCountFlagBits samples, MgMultisamplePropertiesEXT pMultisampleProperties)
        {
            var output = new VkMultisamplePropertiesEXT
            {
                sType = VkStructureType.StructureTypeMultisamplePropertiesExt,
                pNext = IntPtr.Zero, // TODO: extension
            };

            vkGetPhysicalDeviceMultisamplePropertiesEXT(info.Handle, samples, ref output);

            pMultisampleProperties = new MgMultisamplePropertiesEXT
            {
                MaxSampleLocationGridSize = output.maxSampleLocationGridSize,
            };
        }
    }
}
