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

        public IntPtr GetInstanceProcAddr(string pName)
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

        public void DebugReportMessageEXT(MgDebugReportFlagBitsEXT flags, MgDebugReportObjectTypeEXT objectType, ulong @object, IntPtr location, int messageCode, string pLayerPrefix, string pMessage)
        {
            // TODO: need to recheck issue
            throw new NotImplementedException();
        }
    }
}
