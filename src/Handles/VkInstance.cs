using Magnesium;
using System;
namespace Magnesium.Vulkan
{
	public class VkInstance : IMgInstance
	{
		internal IntPtr Handle = IntPtr.Zero;
		internal VkInstance(IntPtr handle)
		{
			Handle = handle;
		}

		public void DestroyInstance(IMgAllocationCallbacks allocator)
		{
			throw new NotImplementedException();
		}

		public Result EnumeratePhysicalDevices(out IMgPhysicalDevice[] physicalDevices)
		{
			throw new NotImplementedException();
		}

		public PFN_vkVoidFunction GetInstanceProcAddr(string pName)
		{
			throw new NotImplementedException();
		}

		public Result CreateDisplayPlaneSurfaceKHR(MgDisplaySurfaceCreateInfoKHR createInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			throw new NotImplementedException();
		}

		public Result CreateAndroidSurfaceKHR(MgAndroidSurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			throw new NotImplementedException();
		}

		public Result CreateWin32SurfaceKHR(MgWin32SurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			throw new NotImplementedException();
		}

		public Result CreateDebugReportCallbackEXT(MgDebugReportCallbackCreateInfoEXT pCreateInfo, IMgAllocationCallbacks allocator, out MgDebugReportCallbackEXT pCallback)
		{
			throw new NotImplementedException();
		}

		public void DestroyDebugReportCallbackEXT(MgDebugReportCallbackEXT callback, IMgAllocationCallbacks allocator)
		{
			throw new NotImplementedException();
		}

		public void DebugReportMessageEXT(MgDebugReportFlagBitsEXT flags, MgDebugReportObjectTypeEXT objectType, UInt64 @object, IntPtr location, Int32 messageCode, string pLayerPrefix, string pMessage)
		{
			throw new NotImplementedException();
		}

	}
}
