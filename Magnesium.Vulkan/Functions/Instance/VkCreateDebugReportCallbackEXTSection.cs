using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Instance
{
	public class VkCreateDebugReportCallbackEXTSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention = CallingConvention.Winapi)]
        internal extern static MgResult vkCreateDebugReportCallbackEXT(IntPtr instance, ref VkDebugReportCallbackCreateInfoEXT pCreateInfo, IntPtr pAllocator, ref UInt64 pCallback);

        void Wrap(MgDebugUtilsMessengerCallbackDataEXT callbackData)
        {
            var msgIdName = (callbackData.MessageIdName != null)
                ? VkInteropsUtility.NativeUtf8FromString(callbackData.MessageIdName)
                : IntPtr.Zero;

            var msg = (callbackData.Message != null)
                ? VkInteropsUtility.NativeUtf8FromString(callbackData.Message)
                : IntPtr.Zero;

            var lbl = new VkDebugUtilsLabelEXT { };
        }

        public static MgResult CreateDebugReportCallbackEXT(VkInstanceInfo info, MgDebugReportCallbackCreateInfoEXT pCreateInfo, IMgAllocationCallbacks allocator, out IMgDebugReportCallbackEXT pCallback)
        {
            var allocatorHandle = VkInteropsUtility.GetAllocatorHandle(allocator);

            var createInfo = new VkDebugReportCallbackCreateInfoEXT
            {
                sType = VkStructureType.StructureTypeDebugReportCallbackCreateInfoExt,
                pNext = IntPtr.Zero,
                flags = (VkDebugReportFlagsExt)pCreateInfo.Flags,
                // TODO : figure out translation
                pfnCallback = IntPtr.Zero,
                pUserData = pCreateInfo.UserData,
            };

            var handle = 0UL;
            var result = vkCreateDebugReportCallbackEXT(info.Handle, ref createInfo, allocatorHandle, ref handle);
            // TODO : figure out translation

            throw new NotImplementedException();
            //pCallback = new VkDebugReportCallbackEXT(handle, );

            return result;
        }
    }
}
