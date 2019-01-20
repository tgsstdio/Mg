using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
    public partial class VkInstance : IMgInstance
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
            if (createInfo == null)
                throw new ArgumentNullException(nameof(createInfo));

            if (createInfo.PfnUserCallback == null)
                throw new ArgumentNullException(nameof(createInfo.PfnUserCallback));

            Debug.Assert(!mIsDisposed);

            var allocatorHandle = GetAllocatorHandle(allocator);

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
            var result = Interops.vkCreateDebugUtilsMessengerEXT(this.Handle, ref bCreateInfo, allocatorHandle, ref bHandle);
            pMessenger = new VkDebugUtilsMessengerEXT(
                bHandle,
                messengerData
            );

            return result;
        }

        public MgResult EnumeratePhysicalDeviceGroups(
            out MgPhysicalDeviceGroupProperties[] pPhysicalDeviceGroupProperties
        )
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
            Debug.Assert(!mIsDisposed);

            var queueLabelCount =
                pCallbackData.QueueLabels != null
                ? (UInt32)pCallbackData.QueueLabels.Length
                : 0U;

            var pQueueLabels = IntPtr.Zero;

            var cmdBufLabelCount =
                pCallbackData.CmdBufLabels != null
                ? (UInt32)pCallbackData.CmdBufLabels.Length
                : 0U;

            var pCmdBufLabels = IntPtr.Zero;

            var objectCount =
                pCallbackData.Objects != null
                ? (UInt32)pCallbackData.Objects.Length
                : 0U;

            var pObjects = IntPtr.Zero;

            var pinned = new List<IntPtr>();
            try
            {
                if (queueLabelCount > 0)
                {
                    pQueueLabels = VkInteropsUtility.AllocateHGlobalArray(pCallbackData.QueueLabels,
                        (f) =>
                        {
                            var lbl = VkInteropsUtility.NativeUtf8FromString(f.LabelName);
                            pinned.Add(lbl);

                            return new VkDebugUtilsLabelEXT
                            {
                                sType = VkStructureType.StructureTypeDebugUtilsLabelExt,
                                pNext = IntPtr.Zero,
                                pLabelName = lbl,
                                color = f.Color,
                            };
                        });
                    pinned.Add(pQueueLabels);
                }

                if (cmdBufLabelCount > 0)
                {
                    pCmdBufLabels = VkInteropsUtility.AllocateHGlobalArray(pCallbackData.CmdBufLabels,
                        (f) =>
                        {
                            var lbl = VkInteropsUtility.NativeUtf8FromString(f.LabelName);
                            pinned.Add(lbl);

                            return new VkDebugUtilsLabelEXT
                            {
                                sType = VkStructureType.StructureTypeDebugUtilsLabelExt,
                                pNext = IntPtr.Zero,
                                pLabelName = lbl,
                                color = f.Color,
                            };
                        });
                    pinned.Add(pCmdBufLabels);
                }

                if (objectCount > 0)
                {
                    pObjects = VkInteropsUtility.AllocateHGlobalArray(pCallbackData.Objects,
                        (f) =>
                            {
                                var pObjectName = IntPtr.Zero;
                                if (!string.IsNullOrEmpty(f.ObjectName))
                                {
                                    pObjectName = VkInteropsUtility.NativeUtf8FromString(f.ObjectName);
                                    pinned.Add(pObjectName);
                                }

                                return new VkDebugUtilsObjectNameInfoEXT
                                {
                                    sType = VkStructureType.StructureTypeDebugMarkerObjectNameInfoExt,
                                    pNext = IntPtr.Zero,
                                    objectHandle = f.ObjectHandle,
                                    pObjectName = pObjectName,
                                    objectType = f.ObjectType,
                                };
                            }
                        );

                    pinned.Add(pObjects);
                }


                var pMessageIdName = IntPtr.Zero;

                if (!string.IsNullOrEmpty(pCallbackData.MessageIdName))
                {
                    pMessageIdName = VkInteropsUtility.NativeUtf8FromString(pCallbackData.MessageIdName);
                    pinned.Add(pMessageIdName);
                }

                var bCallbackData = new VkDebugUtilsMessengerCallbackDataEXT
                {
                    sType = VkStructureType.StructureTypeDebugUtilsMessengerCallbackDataExt,
                    pNext = IntPtr.Zero,
                    flags = pCallbackData.Flags,
                    pMessage = VkInteropsUtility.NativeUtf8FromString(pCallbackData.Message),
                    messageIdNumber = pCallbackData.MessageIdNumber,
                    pMessageIdName = pMessageIdName,
                    queueLabelCount = queueLabelCount,
                    pQueueLabels = pQueueLabels,
                    cmdBufLabelCount = cmdBufLabelCount,
                    pCmdBufLabels = pCmdBufLabels,
                    objectCount = objectCount,
                    pObjects = pObjects,                    
                };

                Interops.vkSubmitDebugUtilsMessageEXT(this.Handle, messageSeverity, messageTypes, bCallbackData);
            }
            finally
            {
                foreach (var pin in pinned)
                {
                    Marshal.FreeHGlobal(pin);
                }

            }
        }
    }
}
