using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Instance
{
	public class VkCreateDebugReportCallbackEXTSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention = CallingConvention.Winapi)]
        internal extern static MgResult vkCreateDebugReportCallbackEXT(IntPtr instance, ref VkDebugReportCallbackCreateInfoEXT pCreateInfo, IntPtr pAllocator, ref UInt64 pCallback);

        public static MgResult CreateDebugReportCallbackEXT(VkInstanceInfo info, MgDebugReportCallbackCreateInfoEXT pCreateInfo, IMgAllocationCallbacks allocator, out IMgDebugReportCallbackEXT pCallback)
        {
            throw new NotImplementedException();

            Debug.Assert(!info.IsDisposed);

            var allocatorHandle = VkInteropsUtility.GetAllocatorHandle(allocator);

            var createInfo = new VkDebugReportCallbackCreateInfoEXT
            {
                sType = VkStructureType.StructureTypeDebugReportCallbackCreateInfoExt,
                pNext = IntPtr.Zero,
                flags = (VkDebugReportFlagsExt)pCreateInfo.Flags,
                // TODO : figure out translation
                pfnCallback = null,
                pUserData = pCreateInfo.UserData,
            };

            var callback = 0UL;
            var result = vkCreateDebugReportCallbackEXT(info.Handle, ref createInfo, allocatorHandle, ref callback);
            // TODO : figure out translation
            pCallback = new VkDebugReportCallbackEXT(callback);

            return result;
        }
    }
}
