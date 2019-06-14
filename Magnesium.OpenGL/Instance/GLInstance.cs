using System;

namespace Magnesium.OpenGL.Internals
{
	public class GLInstance : IMgInstance
	{
		public GLInstance (IGLQueue queue, IGLDeviceEntrypoint entrypoint)
		{
			mPhysicalDevices = new GLPhysicalDevice[1];
			mPhysicalDevices[0] = new GLPhysicalDevice(queue, entrypoint);
		}

		private GLPhysicalDevice[] mPhysicalDevices;

		#region IMgInstance implementation
		public void DestroyInstance (IMgAllocationCallbacks allocator)
		{
	
		}

		public MgResult EnumeratePhysicalDevices (out IMgPhysicalDevice[] physicalDevices)
		{
			physicalDevices = mPhysicalDevices;
			return MgResult.SUCCESS;
		}

		public PFN_vkVoidFunction GetInstanceProcAddr (string pName)
		{
			throw new NotImplementedException ();
		}

		public MgResult CreateDisplayPlaneSurfaceKHR (MgDisplaySurfaceCreateInfoKHR createInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			throw new NotImplementedException ();
		}

		public void DestroySurfaceKHR (IMgSurfaceKHR surface, IMgAllocationCallbacks allocator)
		{
			throw new NotImplementedException ();
		}

		public MgResult CreateWin32SurfaceKHR (MgWin32SurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			throw new NotImplementedException ();
		}

		public MgResult CreateDebugReportCallbackEXT (MgDebugReportCallbackCreateInfoEXT pCreateInfo, IMgAllocationCallbacks allocator, out IMgDebugReportCallbackEXT pCallback)
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

		public MgResult CreateAndroidSurfaceKHR(MgAndroidSurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			throw new NotImplementedException();
		}

        public MgResult CreateDebugUtilsMessengerEXT(MgDebugUtilsMessengerCreateInfoEXT createInfo, IMgAllocationCallbacks allocator, out IMgDebugUtilsMessengerEXT pSurface)
        {
            throw new NotImplementedException();
        }

        public MgResult EnumeratePhysicalDeviceGroups(out MgPhysicalDeviceGroupProperties[] physicalDeviceGroupProperties)
        {
            throw new NotImplementedException();
        }

        public void SubmitDebugUtilsMessageEXT(MgDebugUtilsMessageSeverityFlagBitsEXT messageSeverity, MgDebugUtilsMessageTypeFlagBitsEXT messageTypes, MgDebugUtilsMessengerCallbackDataEXT pCallbackData)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

}

