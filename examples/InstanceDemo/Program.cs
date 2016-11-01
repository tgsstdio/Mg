using System;
using System.Runtime.InteropServices;
using Magnesium;
using Magnesium.Vulkan;

namespace InstanceDemo
{
	class MainClass
	{
		public static void Main(string[] args)
		{
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
					MgInstanceExtensionOptions.ALL);

					using (var device = driver.CreateLogicalDevice(null, MgDeviceExtensionOptions.ALL))
					{                                                             

						if (device.Queues.Length > 0)
						{
							Console.WriteLine(nameof(device.Queues.Length) + " : " + device.Queues.Length);

                            using (var partition = device.Queues[0].CreatePartition(0,
                                new MgDescriptorPoolCreateInfo
                                {
                                    MaxSets = 1,
                                    PoolSizes = new MgDescriptorPoolSize[]
                                    {
                                        new MgDescriptorPoolSize
                                        {
                                            DescriptorCount = 1,
                                            Type = MgDescriptorType.COMBINED_IMAGE_SAMPLER,
                                        }
                                    },
                                })
                            )
                            {
                                IMgBuffer buffer;
                                var result = partition.Device.CreateBuffer(new MgBufferCreateInfo { SharingMode = MgSharingMode.EXCLUSIVE, Size = 1024, Usage = MgBufferUsageFlagBits.VERTEX_BUFFER_BIT }, callback, out buffer);
                                buffer.DestroyBuffer(partition.Device, callback);
                            }
						}
					}
                    Console.WriteLine("NO ERRORS!");
                }
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
                throw ex;
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
