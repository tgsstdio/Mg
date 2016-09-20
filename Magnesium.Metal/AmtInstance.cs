using System;
namespace Magnesium.Metal
{
	public class AmtInstance : IMgInstance
	{
		private AmtPhysicalDevice mPhysicalDevice;
		public AmtInstance(AmtPhysicalDevice device)
		{
			mPhysicalDevice = device;
		}

		public Result CreateAndroidSurfaceKHR(MgAndroidSurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			throw new NotImplementedException();
		}

		public Result CreateDebugReportCallbackEXT(MgDebugReportCallbackCreateInfoEXT pCreateInfo, IMgAllocationCallbacks allocator, out IMgDebugReportCallbackEXT pCallback)
		{
			throw new NotImplementedException();
		}

		public Result CreateDisplayPlaneSurfaceKHR(MgDisplaySurfaceCreateInfoKHR createInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			throw new NotImplementedException();
		}

		public Result CreateWin32SurfaceKHR(MgWin32SurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			throw new NotImplementedException();
		}

		public void DebugReportMessageEXT(MgDebugReportFlagBitsEXT flags, MgDebugReportObjectTypeEXT objectType, ulong @object, IntPtr location, int messageCode, string pLayerPrefix, string pMessage)
		{
			throw new NotImplementedException();
		}

		public void DestroyInstance(IMgAllocationCallbacks allocator)
		{
			throw new NotImplementedException();
		}

		public Result EnumeratePhysicalDevices(out IMgPhysicalDevice[] physicalDevices)
		{
			physicalDevices = new[] { mPhysicalDevice };
			return Result.SUCCESS;
		}

		public PFN_vkVoidFunction GetInstanceProcAddr(string pName)
		{
			throw new NotImplementedException();
		}
	}
}
