using System;

namespace Magnesium.OpenGL.Internals
{
	public class GLInstance : IMgInstance
	{
		public GLInstance (IGLQueue queue, IGLDeviceEntrypoint entrypoint, IGLPhysicalDeviceFormatLookupEntrypoint formatLookup)
		{
			mPhysicalDevices = new GLPhysicalDevice[1];
			mPhysicalDevices[0] = new GLPhysicalDevice(queue, entrypoint, formatLookup);
		}

		private GLPhysicalDevice[] mPhysicalDevices;

		#region IMgInstance implementation
		public void DestroyInstance (IMgAllocationCallbacks allocator)
		{
	
		}

		public Result EnumeratePhysicalDevices (out IMgPhysicalDevice[] physicalDevices)
		{
			physicalDevices = mPhysicalDevices;
			return Result.SUCCESS;
		}

		public PFN_vkVoidFunction GetInstanceProcAddr (string pName)
		{
			throw new NotImplementedException ();
		}

		public Result CreateDisplayPlaneSurfaceKHR (MgDisplaySurfaceCreateInfoKHR createInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			throw new NotImplementedException ();
		}

		public void DestroySurfaceKHR (IMgSurfaceKHR surface, IMgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}

		public Result CreateWin32SurfaceKHR (MgWin32SurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			throw new NotImplementedException ();
		}

		public Result CreateDebugReportCallbackEXT (MgDebugReportCallbackCreateInfoEXT pCreateInfo, IMgAllocationCallbacks allocator, out IMgDebugReportCallbackEXT pCallback)
		{
			throw new NotImplementedException ();
		}

		public void DestroyDebugReportCallbackEXT (IMgDebugReportCallbackEXT callback, IMgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}

		public void DebugReportMessageEXT (MgDebugReportFlagBitsEXT flags, MgDebugReportObjectTypeEXT objectType, ulong @object, IntPtr location, int messageCode, string pLayerPrefix, string pMessage)
		{
			throw new NotImplementedException ();
		}

		public Result CreateAndroidSurfaceKHR(MgAndroidSurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			throw new NotImplementedException();
		}
		#endregion
	}

}

