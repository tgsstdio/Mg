using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Magnesium.Vulkan
{
	public class VkEntrypoint : IMgEntrypoint
	{
		#region IMgEntrypoint implementation

		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
		internal struct ApplicationCreateInfo
		{
			public UInt32 sType;
			public IntPtr pNext;
			[MarshalAs(UnmanagedType.LPStr)] public string pApplicationName;
			public UInt32 applicationVersion;
			[MarshalAs(UnmanagedType.LPStr)] public string pEngineName;
			public UInt32 engineVersion;
			public UInt32 apiVersion;
		}

		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
		internal class InstanceCreateInfo
		{
			public UInt32 sType;
			public IntPtr pNext;
			public UInt32 flags;
			public ApplicationCreateInfo pApplicationInfo;
			public UInt32 enabledLayerCount;
			public IntPtr ppEnabledLayerNames;
			public UInt32 enabledExtensionCount;
			public IntPtr ppEnabledExtensionNames;
		}

		[DllImport("vulkan-1", CallingConvention=CallingConvention.Winapi)]
		extern static Result vkCreateInstance([In] InstanceCreateInfo createInfo, IntPtr allocator, out IntPtr instance);

		public Result CreateInstance (MgInstanceCreateInfo createInfo, MgAllocationCallbacks allocator, out IMgInstance instance)
		{
			var ici = new InstanceCreateInfo {
				sType = 0,
				pNext = IntPtr.Zero,
			};

			ici.enabledLayerCount = (uint) createInfo.EnabledLayerNames.Length;
			ici.ppEnabledLayerNames = Marshal.AllocHGlobal (Marshal.SizeOf(typeof(IntPtr)) * createInfo.EnabledLayerNames.Length);

			ici.enabledExtensionCount = (uint) createInfo.EnabledExtensionNames.Length;
			ici.ppEnabledExtensionNames = Marshal.AllocHGlobal (Marshal.SizeOf(typeof(IntPtr)) * createInfo.EnabledExtensionNames.Length);



			var acInfo = new ApplicationCreateInfo {
				sType = 0,
				pNext = IntPtr.Zero,
			};

			MgApplicationInfo optionalAppInfo = createInfo.ApplicationInfo;
			if (optionalAppInfo != null)
			{
				acInfo.apiVersion = optionalAppInfo.ApiVersion;
				acInfo.applicationVersion = optionalAppInfo.ApplicationVersion;
				acInfo.engineVersion = optionalAppInfo.EngineVersion;
				acInfo.pApplicationName = optionalAppInfo.ApplicationName;
				acInfo.pEngineName = optionalAppInfo.EngineName;

				ici.pApplicationInfo = acInfo;
			}

			IntPtr handle;
			var result = vkCreateInstance (ici, IntPtr.Zero, out handle);
		
			instance = new VkInstance{ Index = 0, Handle = handle };

			return result;
		}

		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
		internal struct LayerProperties
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=256)] public string layerName;
			public UInt32 specVersion;
			public UInt32 implementationVersion;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=256)] public string description;
		}

		[DllImport("vulkan-1", CallingConvention=CallingConvention.Winapi)]
		extern static Result vkEnumerateInstanceLayerProperties(ref UInt32 pPropertyCount, [In, Out] LayerProperties[] pProperties);

		public Result EnumerateInstanceLayerProperties (out MgLayerProperties[] properties)
		{
			UInt32 pPropertyCount = 0;

			Result first = vkEnumerateInstanceLayerProperties (ref pPropertyCount, null);

			if (first != Result.SUCCESS)
			{
				properties = null;
				return first;
			}

			var layerProperties = new LayerProperties[pPropertyCount];
			Result result = vkEnumerateInstanceLayerProperties (ref pPropertyCount, layerProperties);

			var output = new List<MgLayerProperties> ();
			foreach (var prop in layerProperties)				
			{
				output.Add (
					new MgLayerProperties{
						LayerName = prop.layerName,
						SpecVersion = prop.specVersion,
						ImplementationVersion = prop.implementationVersion,
						Description = prop.description,
					}
				);
			}
			properties = output.ToArray ();
			return result;
		}

		public Result EnumerateInstanceExtensionProperties (string layerName, out MgExtensionProperties[] pProperties)
		{
			throw new NotImplementedException ();
		}

		#endregion


	}
}

