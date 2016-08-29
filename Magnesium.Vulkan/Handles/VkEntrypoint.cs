using Magnesium;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

namespace Magnesium.Vulkan
{
	public class VkEntrypoint : IMgEntrypoint
	{
		public Result CreateInstance(MgInstanceCreateInfo createInfo, IMgAllocationCallbacks allocator, out IMgInstance instance)
		{
			var allocatedItems = new List<IntPtr>();
			try
			{
				var ici = new VkInstanceCreateInfo
				{
					sType = VkStructureType.StructureTypeInstanceCreateInfo,
					pNext = IntPtr.Zero,
				};

				ici.enabledLayerCount = createInfo.EnabledLayerNames != null ? (uint)createInfo.EnabledExtensionNames.Length : 0;
				ici.ppEnabledLayerNames = CopyStringArrays(allocatedItems, createInfo.EnabledLayerNames);

				ici.enabledExtensionCount = createInfo.EnabledExtensionNames != null ? (uint)createInfo.EnabledExtensionNames.Length : 0;
				ici.ppEnabledExtensionNames  = CopyStringArrays(allocatedItems, createInfo.EnabledExtensionNames);

				if (createInfo.ApplicationInfo != null)
				{
					var acInfo = new VkApplicationInfo();
					acInfo.sType = VkStructureType.StructureTypeApplicationInfo;
					acInfo.pNext = IntPtr.Zero;
					acInfo.apiVersion = createInfo.ApplicationInfo.ApiVersion;
					acInfo.applicationVersion = createInfo.ApplicationInfo.ApplicationVersion;
					acInfo.engineVersion = createInfo.ApplicationInfo.EngineVersion;
					acInfo.pApplicationName = CopySingleString(allocatedItems, createInfo.ApplicationInfo.ApplicationName);
					acInfo.pEngineName = CopySingleString(allocatedItems, createInfo.ApplicationInfo.EngineName);

					var acInfoSize = Marshal.SizeOf(acInfo);
					var destPtr = Marshal.AllocHGlobal(acInfoSize);

					Marshal.StructureToPtr(acInfo, destPtr, false);

					ici.pApplicationInfo = destPtr;
				}
				else
				{
					ici.pApplicationInfo = IntPtr.Zero;
				}

				var bAllocator = allocator as MgVkAllocationCallbacks;
				IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

				IntPtr handle = IntPtr.Zero;
				var result = Interops.vkCreateInstance(ici, allocatorPtr, ref handle);

				instance = new VkInstance(handle);

				return (Result)result;
			}
			finally
			{
				foreach (var mem in allocatedItems)
				{
					Marshal.FreeHGlobal(mem);
				}
			}
		}

		static IntPtr CopySingleString(List<IntPtr> allocatedItems, string name)
		{
			if (!string.IsNullOrWhiteSpace(name))
			{
				var dest = VkInteropsUtility.NativeUtf8FromString(name);
				allocatedItems.Add(dest);
				return dest;
			}
			else
			{
				return IntPtr.Zero;
			}
		}

		static IntPtr CopyStringArrays(List<IntPtr> allocatedItems, string[] array)
		{
			var POINTER_SIZE = Marshal.SizeOf(typeof(IntPtr));

			int noOfElements = array.Length;

			//  EnabledLayerNames
			if (noOfElements > 0)
			{
				var dest = Marshal.AllocHGlobal(POINTER_SIZE * noOfElements);
				allocatedItems.Add(dest);

				var names = new IntPtr[noOfElements];
				for (int i = 0; i < noOfElements; ++i)
				{
					names[i] = VkInteropsUtility.NativeUtf8FromString(array[i]);
					allocatedItems.Add(names[i]);
				}

				Marshal.Copy(names, 0, dest, noOfElements);

				return dest;
			}
			else
			{
				return IntPtr.Zero;
			}
		}

		public Result EnumerateInstanceLayerProperties(out MgLayerProperties[] properties)
		{
			UInt32 pPropertyCount = 0;

			var first = Interops.vkEnumerateInstanceLayerProperties(ref pPropertyCount, null);

			if (first != Result.SUCCESS)
			{
				properties = null;
				return first;
			}

			var layerProperties = new VkLayerProperties[pPropertyCount];
			var last = Interops.vkEnumerateInstanceLayerProperties(ref pPropertyCount, layerProperties);

			properties = new MgLayerProperties[pPropertyCount];
			for (uint i = 0; i < pPropertyCount; ++i)
			{
				properties[i] = new MgLayerProperties
				{
					LayerName =
						Encoding.UTF8.GetString(
						layerProperties[i].layerName,
						0,
						layerProperties[i].layerName.Length),
					SpecVersion = layerProperties[i].specVersion,
					ImplementationVersion = layerProperties[i].implementationVersion,
					Description = 
						Encoding.UTF8.GetString(
						layerProperties[i].description,
						0,
						layerProperties[i].description.Length),
				};
			}
			return last;
		}

		public Result EnumerateInstanceExtensionProperties(string layerName, out MgExtensionProperties[] pProperties)
		{
			var pLayerName = IntPtr.Zero;
			try
			{
				pLayerName = VkInteropsUtility.NativeUtf8FromString(layerName);

				UInt32 pPropertyCount = 0;
				var first = Interops.vkEnumerateInstanceExtensionProperties(pLayerName, ref pPropertyCount, null);

				if (first != Result.SUCCESS)
				{
					pProperties = null;
					return first;
				}

				var extensionProperties = new VkExtensionProperties[pPropertyCount];
				var last = Interops.vkEnumerateInstanceExtensionProperties(pLayerName, ref pPropertyCount, extensionProperties);

				pProperties = new MgExtensionProperties[pPropertyCount];
				for (uint i = 0; i < pPropertyCount; ++i)
				{
					pProperties[i] = new MgExtensionProperties
					{
						ExtensionName = Encoding.UTF8.GetString(
							extensionProperties[i].extensionName,
							0,
							extensionProperties[i].extensionName.Length),
						SpecVersion = extensionProperties[i].specVersion,
					};
				}
				return last;
			}
			finally
			{
				if (pLayerName != IntPtr.Zero)
				{

				}
			}
		}

		public IMgAllocationCallbacks CreateAllocationCallbacks()
		{
			return new MgVkAllocationCallbacks();
		}
	}
}
