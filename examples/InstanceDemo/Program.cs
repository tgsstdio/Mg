using System;
using System.Runtime.InteropServices;
using Magnesium;
using Magnesium.Vulkan;

namespace InstanceDemo
{
	class MainClass
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct LayerProperties
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string layerName;
			public UInt32 specVersion;
			public UInt32 implementationVersion;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string description;
		}

		[DllImport("vulkan-1", CallingConvention = CallingConvention.Winapi)]
		extern static unsafe UInt32 vkEnumerateInstanceLayerProperties(ref UInt32 pPropertyCount, [In, Out] LayerProperties[] pProperties);


		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			try
			{
				var entrypoint = new VkEntrypoint();


                IMgAllocationCallbacks callback = entrypoint.CreateAllocationCallbacks();
                callback.PfnInternalAllocation = DebugInternalAllocation;
                callback.PfnAllocation = DebugAllocateFunction;
                callback.PfnReallocation = DebugReallocationFunction;
                callback.PfnInternalFree = DebugInternalFree;
                callback.PfnFree = null;

				using (var driver = new MgDriver(entrypoint))
				{                    
					driver.Initialize(new MgApplicationInfo
					{
						ApiVersion = MgApplicationInfo.GenerateApiVersion(1,0,17),
						ApplicationName = "InstanceDemo",
						ApplicationVersion = 1,
						EngineName = "Magnesium.Vulkan",
						EngineVersion = 1,
					},
                    MgEnableExtensionsOption.ALL);

                    using (var device = driver.CreateLogicalDevice(null, MgEnableExtensionsOption.ALL))
                    {                                                             

                        if (device.Queues.Length > 0)
                        {
                            Console.WriteLine(nameof(device.Queues.Length) + " : " + device.Queues.Length);

                            using (var partition = device.Queues[0].CreatePartition())
                            {
                                IMgBuffer buffer;
                                var result = partition.Device.CreateBuffer(new MgBufferCreateInfo { SharingMode = MgSharingMode.EXCLUSIVE, Size = 1024, Usage = MgBufferUsageFlagBits.VERTEX_BUFFER_BIT }, callback, out buffer);
                                buffer.DestroyBuffer(partition.Device, null);
                            }
                        }
                    }                  
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

        private static void DebugInternalFree(IntPtr pUserData, IntPtr size, uint allocationType, uint allocationScope)
        {
            Console.WriteLine(nameof(DebugInternalFree));
        }

        private static void DebugReallocationFunction(IntPtr pUserData, IntPtr pOriginal, IntPtr size, IntPtr alignment, uint allocationScope)
        {
            Console.WriteLine(nameof(DebugReallocationFunction));
        }

        private static void DebugInternalAllocation(IntPtr pUserData, IntPtr size, uint allocationType, uint allocationScope)
        {
            Console.WriteLine("DebugInternalAllocation");
        }

        private static void DebugAllocateFunction(IntPtr pUserData, IntPtr size, IntPtr alignment, uint allocationScope)
        {
            Console.WriteLine(nameof(size) + " : " + size);
        }
    }
}
