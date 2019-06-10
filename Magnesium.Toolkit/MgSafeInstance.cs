using System;
namespace Magnesium.Toolkit
{
	public class MgSafeInstance : IMgInstance
	{
		internal IMgInstance mImpl = null;
		internal MgSafeInstance(IMgInstance impl)
		{
			mImpl = impl;
		}

		public void DestroyInstance(IMgAllocationCallbacks allocator) {
			Validation.Instance.DestroyInstance.Validate(allocator);
			mImpl.DestroyInstance(allocator);
		}

		public MgResult EnumeratePhysicalDevices(out IMgPhysicalDevice[] physicalDevices) {
			return mImpl.EnumeratePhysicalDevices(out physicalDevices);
		}

		public PFN_vkVoidFunction GetInstanceProcAddr(string pName) {
			Validation.Instance.GetInstanceProcAddr.Validate(pName);
			return mImpl.GetInstanceProcAddr(pName);
		}

		public MgResult CreateDisplayPlaneSurfaceKHR(MgDisplaySurfaceCreateInfoKHR createInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface) {
			Validation.Instance.CreateDisplayPlaneSurfaceKHR.Validate(createInfo, allocator);
			return mImpl.CreateDisplayPlaneSurfaceKHR(createInfo, allocator, out pSurface);
		}

		public MgResult CreateAndroidSurfaceKHR(MgAndroidSurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface) {
			Validation.Instance.CreateAndroidSurfaceKHR.Validate(pCreateInfo, allocator);
			return mImpl.CreateAndroidSurfaceKHR(pCreateInfo, allocator, out pSurface);
		}

		public MgResult CreateWin32SurfaceKHR(MgWin32SurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface) {
			Validation.Instance.CreateWin32SurfaceKHR.Validate(pCreateInfo, allocator);
			return mImpl.CreateWin32SurfaceKHR(pCreateInfo, allocator, out pSurface);
		}

		public MgResult CreateDebugReportCallbackEXT(MgDebugReportCallbackCreateInfoEXT pCreateInfo, IMgAllocationCallbacks allocator, out IMgDebugReportCallbackEXT pCallback) {
			Validation.Instance.CreateDebugReportCallbackEXT.Validate(pCreateInfo, allocator);
			return mImpl.CreateDebugReportCallbackEXT(pCreateInfo, allocator, out pCallback);
		}

		public void DebugReportMessageEXT(MgDebugReportFlagBitsEXT flags, MgDebugReportObjectTypeEXT objectType, UInt64 @object, IntPtr location, Int32 messageCode, string pLayerPrefix, string pMessage) {
			Validation.Instance.DebugReportMessageEXT.Validate(flags, objectType, @object, location, messageCode, pLayerPrefix, pMessage);
			mImpl.DebugReportMessageEXT(flags, objectType, @object, location, messageCode, pLayerPrefix, pMessage);
		}

		public MgResult CreateDebugUtilsMessengerEXT(MgDebugUtilsMessengerCreateInfoEXT createInfo, IMgAllocationCallbacks allocator, out IMgDebugUtilsMessengerEXT pSurface) {
			Validation.Instance.CreateDebugUtilsMessengerEXT.Validate(createInfo, allocator);
			return mImpl.CreateDebugUtilsMessengerEXT(createInfo, allocator, out pSurface);
		}

		public MgResult EnumeratePhysicalDeviceGroups(out MgPhysicalDeviceGroupProperties[] physicalDeviceGroupProperties) {
			return mImpl.EnumeratePhysicalDeviceGroups(out physicalDeviceGroupProperties);
		}

		public void SubmitDebugUtilsMessageEXT(MgDebugUtilsMessageSeverityFlagBitsEXT messageSeverity, MgDebugUtilsMessageTypeFlagBitsEXT messageTypes, MgDebugUtilsMessengerCallbackDataEXT pCallbackData) {
			Validation.Instance.SubmitDebugUtilsMessageEXT.Validate(messageSeverity, messageTypes, pCallbackData);
			mImpl.SubmitDebugUtilsMessageEXT(messageSeverity, messageTypes, pCallbackData);
		}

	}
}
