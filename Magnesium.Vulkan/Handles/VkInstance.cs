using Magnesium.Vulkan.Functions.Instance;
using System;

namespace Magnesium.Vulkan
{
    public partial class VkInstance : IMgInstance
	{
		internal VkInstanceInfo Info { get; private set; }
		internal VkInstance(IntPtr handle)
		{
			Info = new VkInstanceInfo(handle);
		}

		public void DestroyInstance(IMgAllocationCallbacks allocator)
        {
            VkDestroyInstanceSection.DestroyInstance(Info, allocator);
        }

        public MgResult EnumeratePhysicalDevices(out IMgPhysicalDevice[] physicalDevices)
        {
            return VkEnumeratePhysicalDevicesSection.EnumeratePhysicalDevices(Info, out physicalDevices);
        }

        public PFN_vkVoidFunction GetInstanceProcAddr(string pName)
        {
            return VkGetInstanceProcAddrSection.GetInstanceProcAddr(Info, pName);
        }

        public MgResult CreateDisplayPlaneSurfaceKHR(MgDisplaySurfaceCreateInfoKHR createInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
        {
            return VkCreateDisplayPlaneSurfaceKHRSection.CreateDisplayPlaneSurfaceKHR(Info, createInfo, allocator, out pSurface);
        }

        public MgResult CreateAndroidSurfaceKHR(MgAndroidSurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
        {
            return VkCreateAndroidSurfaceKHRSection.CreateAndroidSurfaceKHR(Info, pCreateInfo, allocator, out pSurface);
        }

        public MgResult CreateWin32SurfaceKHR(MgWin32SurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
        {
            return VkCreateWin32SurfaceKHRSection.CreateWin32SurfaceKHR(Info, pCreateInfo, allocator, out pSurface);
        }

        public MgResult CreateDebugReportCallbackEXT(MgDebugReportCallbackCreateInfoEXT pCreateInfo, IMgAllocationCallbacks allocator, out IMgDebugReportCallbackEXT pCallback)
        {
            return VkCreateDebugReportCallbackEXTSection.CreateDebugReportCallbackEXT(Info, pCreateInfo, allocator, out pCallback);
        }

        public void DebugReportMessageEXT(MgDebugReportFlagBitsEXT flags, MgDebugReportObjectTypeEXT objectType, UInt64 @object, IntPtr location, Int32 messageCode, string pLayerPrefix, string pMessage)
        {
            VkDebugReportMessageEXTSection.DebugReportMessageEXT(Info, flags, objectType, @object, location, messageCode, pLayerPrefix, pMessage);
        }

        public void Wrap(MgDebugUtilsMessengerCallbackDataEXT callbackData)
        {
            var msgIdName = (callbackData.MessageIdName != null)
                ? VkInteropsUtility.NativeUtf8FromString(callbackData.MessageIdName)
                : IntPtr.Zero;

            var msg = (callbackData.Message != null)
                ? VkInteropsUtility.NativeUtf8FromString(callbackData.Message)
                : IntPtr.Zero;

            var lbl = new VkDebugUtilsLabelEXT { };
        }

        public MgResult CreateDebugUtilsMessengerEXT(
            MgDebugUtilsMessengerCreateInfoEXT createInfo,
            IMgAllocationCallbacks allocator,
            out IMgDebugUtilsMessengerEXT pMessenger
        )
        {
            return VkCreateDebugUtilsMessengerEXTSection.CreateDebugUtilsMessengerEXT(Info, createInfo, allocator, out pMessenger);
        }

        public MgResult EnumeratePhysicalDeviceGroups(
            out MgPhysicalDeviceGroupProperties[] pPhysicalDeviceGroupProperties
        )
        {
            return VkEnumeratePhysicalDeviceGroupsSection.EnumeratePhysicalDeviceGroups(Info, out pPhysicalDeviceGroupProperties);
        }

        public void SubmitDebugUtilsMessageEXT(MgDebugUtilsMessageSeverityFlagBitsEXT messageSeverity, MgDebugUtilsMessageTypeFlagBitsEXT messageTypes, MgDebugUtilsMessengerCallbackDataEXT pCallbackData)
        {
            VkSubmitDebugUtilsMessageEXTSection.SubmitDebugUtilsMessageEXT(Info, messageSeverity, messageTypes, pCallbackData);
        }
    }
}
