using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Instance
{
	public class VkCreateDebugUtilsMessengerEXTSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention = CallingConvention.Winapi)]
        internal extern static MgResult vkCreateDebugUtilsMessengerEXT(IntPtr instance, ref VkDebugUtilsMessengerCreateInfoEXT pCreateInfo, IntPtr pAllocator, ref UInt64 pMessenger);

        public static MgResult CreateDebugUtilsMessengerEXT(VkInstanceInfo info, MgDebugUtilsMessengerCreateInfoEXT createInfo, IMgAllocationCallbacks allocator, out IMgDebugUtilsMessengerEXT pMessenger)
        {
            if (createInfo == null)
                throw new ArgumentNullException(nameof(createInfo));

            if (createInfo.PfnUserCallback == null)
                throw new ArgumentNullException(nameof(createInfo.PfnUserCallback));

            Debug.Assert(!info.IsDisposed);

            var allocatorHandle = VkInteropsUtility.GetAllocatorHandle(allocator);

            var messengerData = new VkDebugUtilsMessengerData(
                createInfo.PfnUserCallback,
                createInfo.PUserData);

            var bCreateInfo = new VkDebugUtilsMessengerCreateInfoEXT
            {
                sType = VkStructureType.StructureTypeDebugUtilsMessengerCreateInfoExt,
                pNext = IntPtr.Zero,
                flags = createInfo.Flags,
                messageSeverity = createInfo.MessageSeverity,
                messageType = createInfo.MessageType,
                pfnUserCallback = messengerData.UnwrapFn,
                pUserData = messengerData.Capsule,
            };

            ulong bHandle = 0UL;
            var result = vkCreateDebugUtilsMessengerEXT(info.Handle, ref bCreateInfo, allocatorHandle, ref bHandle);
            pMessenger = new VkDebugUtilsMessengerEXT(
                bHandle,
                messengerData
            );

            return result;
        }
    }
}
