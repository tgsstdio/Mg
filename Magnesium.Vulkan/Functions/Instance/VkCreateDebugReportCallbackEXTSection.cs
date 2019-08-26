using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Instance
{
	public class VkCreateDebugReportCallbackEXTSection
	{
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

        private delegate MgResult vkCreateDebugReportCallbackEXT(IntPtr instance, ref VkDebugReportCallbackCreateInfoEXT pCreateInfo, IntPtr pAllocator, ref UInt64 pCallback);
        public static MgResult CreateDebugReportCallbackEXT(VkInstanceInfo info, MgDebugReportCallbackCreateInfoEXT pCreateInfo, IMgAllocationCallbacks allocator, out IMgDebugReportCallbackEXT pCallback)
        {
            Debug.Assert(!info.IsDisposed);

            var allocatorHandle = VkInteropsUtility.GetAllocatorHandle(allocator);
            var funcPtr = VkGetInstanceProcAddrSection.GetInstanceProcAddr(info, "vkCreateDebugReportCallbackEXT");

            if (funcPtr == IntPtr.Zero)
            {
                pCallback = null;
                return MgResult.ERROR_FEATURE_NOT_PRESENT;
            }

            var createFunc = Marshal.GetDelegateForFunctionPointer(funcPtr, typeof(vkCreateDebugReportCallbackEXT)) as vkCreateDebugReportCallbackEXT;

            var bCallback = Marshal.GetFunctionPointerForDelegate(pCreateInfo.PfnCallback);

            var createInfo = new VkDebugReportCallbackCreateInfoEXT
            {
                sType = VkStructureType.StructureTypeDebugReportCallbackCreateInfoExt,
                pNext = IntPtr.Zero,
                flags = (VkDebugReportFlagsExt)pCreateInfo.Flags,
                pfnCallback = bCallback,
                pUserData = pCreateInfo.UserData,
            };

            var debugHandle = 0UL;
            var result = createFunc(info.Handle, ref createInfo, allocatorHandle, ref debugHandle);
            // TODO : figure out translation
            pCallback = new VkDebugReportCallbackEXT(debugHandle, pCreateInfo.PfnCallback);

            return result;

        }
    }
}
