using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	public class VkInstance : IMgInstance
	{
		internal IntPtr Handle { get; private set; }
		internal VkInstance(IntPtr handle)
		{
			Handle = handle;
		}

		/// <summary>
		/// Allocator is optional
		/// </summary>
		/// <param name="allocator"></param>
		/// <returns></returns>
		static IntPtr GetAllocatorHandle(IMgAllocationCallbacks allocator)
		{
			var bAllocator = (MgVkAllocationCallbacks)allocator;
			return bAllocator != null ? bAllocator.Handle : IntPtr.Zero;
		}

		private bool mIsDisposed = false;
		public void DestroyInstance(IMgAllocationCallbacks allocator)
		{
			if (!mIsDisposed)
				return;

			var allocatorHandle = GetAllocatorHandle(allocator);	

			Interops.vkDestroyInstance(Handle, allocatorHandle);

			Handle = IntPtr.Zero;
			mIsDisposed = true;
		}

		public MgResult EnumeratePhysicalDevices(out IMgPhysicalDevice[] physicalDevices)
		{
			Debug.Assert(!mIsDisposed);

			var pPropertyCount = 0U;

			var first = Interops.vkEnumeratePhysicalDevices(Handle, ref pPropertyCount, null);

			if (first != MgResult.SUCCESS)
			{
				physicalDevices = null;
				return first;
			}

			var devices = new IntPtr[pPropertyCount];
			var last = Interops.vkEnumeratePhysicalDevices(Handle, ref pPropertyCount, devices);

			physicalDevices = new VkPhysicalDevice[pPropertyCount];
			for (uint i = 0; i < pPropertyCount; ++i)
			{
				physicalDevices[i] = new VkPhysicalDevice(devices[i]);
			}
			return last;
		}

		public PFN_vkVoidFunction GetInstanceProcAddr(string pName)
		{
			Debug.Assert(!mIsDisposed);

			return Interops.vkGetInstanceProcAddr(Handle, pName);
		}

		public MgResult CreateDisplayPlaneSurfaceKHR(MgDisplaySurfaceCreateInfoKHR createInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			if (createInfo == null)
				throw new ArgumentNullException(nameof(createInfo));

			Debug.Assert(!mIsDisposed);

			var bDisplayMode = (VkDisplayModeKHR)createInfo.DisplayMode;
			Debug.Assert(bDisplayMode != null);

			var pCreateInfo = new VkDisplaySurfaceCreateInfoKHR
			{
				sType = VkStructureType.StructureTypeDisplaySurfaceCreateInfoKhr,
				pNext = IntPtr.Zero,
				flags = createInfo.Flags,
				displayMode = bDisplayMode.Handle,
				planeIndex = createInfo.PlaneIndex,
				planeStackIndex = createInfo.PlaneStackIndex,
				transform = (VkSurfaceTransformFlagsKhr)createInfo.Transform,
				globalAlpha = createInfo.GlobalAlpha,
				alphaMode = (VkDisplayPlaneAlphaFlagsKhr)createInfo.AlphaMode,
				imageExtent = createInfo.ImageExtent,
			};

			// MIGHT NEED GetInstanceProcAddr INSTEAD

			var allocatorHandle = GetAllocatorHandle(allocator);

			UInt64 handle = 0UL;
			var result = Interops.vkCreateDisplayPlaneSurfaceKHR(Handle, ref pCreateInfo, allocatorHandle, ref handle);
			pSurface = new VkSurfaceKHR(handle);

			return result;
		}

		public MgResult CreateAndroidSurfaceKHR(MgAndroidSurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var allocatorHandle = GetAllocatorHandle(allocator);

			// TODO : MIGHT NEED GetInstanceProcAddr INSTEAD
			var createInfo = new VkAndroidSurfaceCreateInfoKHR
			{
				sType = VkStructureType.StructureTypeAndroidSurfaceCreateInfoKhr,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
				window = pCreateInfo.Window,
			};

			var surfaceHandle = 0UL;
			var result = Interops.vkCreateAndroidSurfaceKHR(Handle, ref createInfo, allocatorHandle, ref surfaceHandle);
			pSurface = new VkSurfaceKHR(surfaceHandle);

			return result;
		}

		public MgResult CreateWin32SurfaceKHR(MgWin32SurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var allocatorHandle = GetAllocatorHandle(allocator);

			var createInfo = new VkWin32SurfaceCreateInfoKHR
			{
				sType = VkStructureType.StructureTypeWin32SurfaceCreateInfoKhr,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
				hinstance = pCreateInfo.Hinstance,
				hwnd = pCreateInfo.Hwnd,
			};

			// TODO : MIGHT NEED GetInstanceProcAddr INSTEAD

			var surfaceHandle = 0UL;
			var result = Interops.vkCreateWin32SurfaceKHR(Handle, ref createInfo, allocatorHandle, ref surfaceHandle);
			pSurface = new VkSurfaceKHR(surfaceHandle);

			return result;
		}

		public MgResult CreateDebugReportCallbackEXT(MgDebugReportCallbackCreateInfoEXT pCreateInfo, IMgAllocationCallbacks allocator, out IMgDebugReportCallbackEXT pCallback)
		{
			throw new NotImplementedException();

			Debug.Assert(!mIsDisposed);

			var allocatorHandle = GetAllocatorHandle(allocator);

			var createInfo = new VkDebugReportCallbackCreateInfoEXT
			{
				sType = VkStructureType.StructureTypeDebugReportCallbackCreateInfoExt,
				pNext = IntPtr.Zero,
				flags = (VkDebugReportFlagsExt)pCreateInfo.Flags,
				// TODO : figure out translation
				pfnCallback = null,
				pUserData = pCreateInfo.UserData,
			};

			var callback = 0UL;
			var result = Interops.vkCreateDebugReportCallbackEXT(Handle, ref createInfo, allocatorHandle, ref callback);
			// TODO : figure out translation
			pCallback = new VkDebugReportCallbackEXT(callback);

			return result;
		}

		public void DebugReportMessageEXT(MgDebugReportFlagBitsEXT flags, MgDebugReportObjectTypeEXT objectType, UInt64 @object, IntPtr location, Int32 messageCode, string pLayerPrefix, string pMessage)
		{
			Debug.Assert(!mIsDisposed);

			throw new NotImplementedException();
		}

        delegate VkBool32 PFN_vkDebugUtilsMessengerCallbackEXT(
            MgDebugUtilsMessageSeverityFlagBitsEXT messageSeverity,
            MgDebugUtilsMessageTypeFlagBitsEXT messageType,
            IntPtr pCallbackData, // const VkDebugUtilsMessengerCallbackDataEXT*
            IntPtr pUserData);

        [StructLayout(LayoutKind.Sequential)]
        struct VkUnwrapMessengerCallbackCapsule
        {
            public IntPtr PfnCallback { get; set; }
            public IntPtr UserData { get; set; }
        }

        static MgDebugUtilsLabelEXT TransformIntoLabelInfo(ref VkDebugUtilsLabelEXT src)
        {
            return new MgDebugUtilsLabelEXT
            {
                LabelName = VkInteropsUtility.StringFromNativeUtf8(src.pLabelName),
                Color = src.color,
            };
        }

        static MgDebugUtilsObjectNameInfoEXT TransformIntoObjectInfo(ref VkDebugUtilsObjectNameInfoEXT src)
        {
            return new MgDebugUtilsObjectNameInfoEXT
            {
                ObjectHandle = src.objectHandle,
                ObjectType = src.objectType,
                ObjectName = VkInteropsUtility.StringFromNativeUtf8(src.pObjectName),
            };
        }

        public static VkBool32 UnwrapCallback(
            MgDebugUtilsMessageSeverityFlagBitsEXT messageSeverity,
            MgDebugUtilsMessageTypeFlagBitsEXT messageType,
            IntPtr pCallbackData, // const VkDebugUtilsMessengerCallbackDataEXT*
            IntPtr pUserData
        )
        {
            var callbackData =
                (VkDebugUtilsMessengerCallbackDataEXT)Marshal.PtrToStructure(
                    pCallbackData,
                    typeof(VkDebugUtilsMessengerCallbackDataEXT)
                );
            var userDataCapsule = (VkUnwrapMessengerCallbackCapsule)Marshal.PtrToStructure(pUserData, typeof(VkUnwrapMessengerCallbackCapsule));

            var actualCallback = (MgDebugUtilsMessengerCallbackEXT)Marshal.GetDelegateForFunctionPointer(
                userDataCapsule.PfnCallback,
                typeof(MgDebugUtilsMessengerCallbackEXT));

            var queueLabelCount = callbackData.queueLabelCount;
            var queueLabels = VkInteropsUtility.TransformIntoStructArray<VkDebugUtilsLabelEXT, MgDebugUtilsLabelEXT>(
                callbackData.pQueueLabels,
                queueLabelCount,
                TransformIntoLabelInfo);
            var cmdBufLabels = VkInteropsUtility.TransformIntoStructArray<VkDebugUtilsLabelEXT, MgDebugUtilsLabelEXT>(
                callbackData.pCmdBufLabels, 
                callbackData.cmdBufLabelCount,
                TransformIntoLabelInfo);
            var objects = VkInteropsUtility.TransformIntoStructArray<VkDebugUtilsObjectNameInfoEXT, MgDebugUtilsObjectNameInfoEXT>(
                callbackData.pObjects,
                callbackData.objectCount,
                TransformIntoObjectInfo
            );                

            var srcCallbackData = new MgDebugUtilsMessengerCallbackDataEXT
            {
                Flags = callbackData.flags,
                Message = VkInteropsUtility.StringFromNativeUtf8(callbackData.pMessage),
                MessageIdNumber = callbackData.messageIdNumber,
                MessageIdName = VkInteropsUtility.StringFromNativeUtf8(callbackData.pMessageIdName),
                QueueLabels = queueLabels,
                CmdBufLabels = cmdBufLabels,
                Objects = objects,
            };

            var result = actualCallback(messageSeverity, messageType, srcCallbackData, userDataCapsule.UserData);

            return VkBool32.ConvertTo(result);
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

        class VkDebugUtilsMessengerEXT : IMgDebugUtilsMessengerEXT
        {
            public IntPtr Handle { get; private set; }
            public IntPtr UserData { get; private set; }
            public IntPtr Capsule { get; private set; }
            public MgDebugUtilsMessengerCallbackEXT Callback { get; private set; }
            PFN_vkDebugUtilsMessengerCallbackEXT Wrapped { get; set; }

            public VkDebugUtilsMessengerEXT(
                IntPtr handle,
                MgDebugUtilsMessengerCallbackEXT callback,
                PFN_vkDebugUtilsMessengerCallbackEXT wrapped,
                IntPtr userData,
                IntPtr capsule
            )
            {
                Handle = handle;
                UserData = userData;
                Capsule = IntPtr.Zero;
                Callback = callback;
                Wrapped = wrapped;
            }

            public void DestroyDebugUtilsMessengerEXT(IMgInstance instance, IMgAllocationCallbacks allocator)
            {
                Wrapped = null;

                Interops.DestroyDebugUtilsMessengerEXT();
            }
        }

        public MgResult CreateDebugUtilsMessengerEXT(MgDebugUtilsMessengerCreateInfoEXT createInfo, IMgAllocationCallbacks allocator, out IMgDebugUtilsMessengerEXT pSurface)
        {
            if (createInfo == null)
                throw new ArgumentNullException(nameof(createInfo));

            if (createInfo.PfnUserCallback == null)
                throw new ArgumentNullException(nameof(createInfo.PfnUserCallback));

            Debug.Assert(!mIsDisposed);

            var allocatorHandle = GetAllocatorHandle(allocator);

            PFN_vkDebugUtilsMessengerCallbackEXT del = VkInstance.UnwrapCallback;
            var bActual = Marshal.GetFunctionPointerForDelegate(createInfo.PfnUserCallback);
            var bWrapped = Marshal.GetFunctionPointerForDelegate(del);

            var stride = Marshal.SizeOf(typeof(VkUnwrapMessengerCallbackCapsule));
            var bCapsule = Marshal.AllocHGlobal(stride);

            var capsule = new VkUnwrapMessengerCallbackCapsule
            {
                PfnCallback = bActual,
                UserData = createInfo.PUserData,
            };

            Marshal.StructureToPtr(capsule, bCapsule, false);

            var bCreateInfo = new VkDebugUtilsMessengerCreateInfoEXT
            {
                 sType = VkStructureType.StructureTypeDebugUtilsMessengerCreateInfoExt,
                 pNext = IntPtr.Zero,
                 flags = createInfo.Flags,
                 messageSeverity = createInfo.MessageSeverity,
                 messageType = createInfo.MessageType,
                 pfnUserCallback = bWrapped,
                 pUserData = bCapsule,
            };

            var result = Interops.vkCreateDebugUtilsMessengerEXT(
            pSurface = new VkDebugUtilsMessengerEXT();

            return result;
        }

        public MgResult EnumeratePhysicalDeviceGroups(out MgPhysicalDeviceGroupProperties[] pPhysicalDeviceGroupProperties)
        {
            Debug.Assert(!mIsDisposed);

            var count = 0U;

            var first = Interops.vkEnumeratePhysicalDeviceGroups(Handle, ref count, null);

            var srcGroups = new VkPhysicalDeviceGroupProperties[count];
            var final = Interops.vkEnumeratePhysicalDeviceGroups(Handle, ref count, srcGroups);

            var dstGroups = new MgPhysicalDeviceGroupProperties[count];
            for (var i = 0; i < count; ++i)
            {
                var deviceCount = srcGroups[i].physicalDeviceCount;

                var devices = new IMgPhysicalDevice[deviceCount];

                for(var j = 0; j < deviceCount; j += 1)
                {
                    devices[j] = new VkPhysicalDevice(srcGroups[i].physicalDevices[j]);
                }

                dstGroups[i] = new MgPhysicalDeviceGroupProperties
                {
                    PhysicalDevices = devices,
                    SubsetAllocation = VkBool32.ConvertFrom(srcGroups[i].subsetAllocation),
                };
            }

            pPhysicalDeviceGroupProperties = dstGroups;

            return final;
        }

        public void SubmitDebugUtilsMessageEXT(MgDebugUtilsMessageSeverityFlagBitsEXT messageSeverity, MgDebugUtilsMessageTypeFlagBitsEXT messageTypes, MgDebugUtilsMessengerCallbackDataEXT pCallbackData)
        {
            throw new NotImplementedException();
        }
    }
}
