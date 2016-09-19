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
				uint enabledLayerCount;
				var ppEnabledLayerNames = VkInteropsUtility.CopyStringArrays(allocatedItems, createInfo.EnabledLayerNames, out enabledLayerCount);

				uint enabledExtensionCount;
				var ppEnabledExtensionNames  = VkInteropsUtility.CopyStringArrays(allocatedItems, createInfo.EnabledExtensionNames, out enabledExtensionCount);

				var pApplicationInfo = IntPtr.Zero;
				if (createInfo.ApplicationInfo != null)
				{
					var acInfo = new VkApplicationInfo
					{
						sType = VkStructureType.StructureTypeApplicationInfo,
						pNext = IntPtr.Zero,
						apiVersion = createInfo.ApplicationInfo.ApiVersion,
						applicationVersion = createInfo.ApplicationInfo.ApplicationVersion,
						engineVersion = createInfo.ApplicationInfo.EngineVersion,
						pApplicationName = CopySingleString(allocatedItems, createInfo.ApplicationInfo.ApplicationName),
						pEngineName = CopySingleString(allocatedItems, createInfo.ApplicationInfo.EngineName),
					};

					var destPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VkApplicationInfo)));
					allocatedItems.Add(destPtr);

					Marshal.StructureToPtr(acInfo, destPtr, false);

					pApplicationInfo = destPtr;
				}

				var ici = new VkInstanceCreateInfo
				{
					sType = VkStructureType.StructureTypeInstanceCreateInfo,
					pNext = IntPtr.Zero,
					enabledLayerCount = enabledLayerCount,
					ppEnabledLayerNames = ppEnabledLayerNames,
					enabledExtensionCount = enabledExtensionCount,
					ppEnabledExtensionNames = ppEnabledExtensionNames,
					pApplicationInfo = pApplicationInfo, 
				};

				var bAllocator = allocator as MgVkAllocationCallbacks;
				IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

				IntPtr handle = IntPtr.Zero;
				var result = Interops.vkCreateInstance(ref ici, allocatorPtr, ref handle);

				instance = new VkInstance(handle);

				return (Result)result;
			}
			finally
			{
				//foreach (var mem in allocatedItems)
				//{
				//	Marshal.FreeHGlobal(mem);
				//}
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
					LayerName = VkInteropsUtility.ByteArrayToTrimmedString(layerProperties[i].layerName),
					SpecVersion = layerProperties[i].specVersion,
					ImplementationVersion = layerProperties[i].implementationVersion,
					Description = VkInteropsUtility.ByteArrayToTrimmedString(layerProperties[i].description),
				};
			}
			return last;
		}

		public Result EnumerateInstanceExtensionProperties(string layerName, out MgExtensionProperties[] pProperties)
		{
			var pLayerName = IntPtr.Zero;
			try
			{
                if (!string.IsNullOrWhiteSpace(layerName))
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
						ExtensionName = VkInteropsUtility.ByteArrayToTrimmedString(extensionProperties[i].extensionName),
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
