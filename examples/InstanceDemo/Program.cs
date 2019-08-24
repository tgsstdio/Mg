using System;
using Magnesium;
using Magnesium.Toolkit;
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
       

				using (var driver = new MgDriverContext(entrypoint))
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

                    using (var device = driver.CreateLogicalDevice(null, MgDeviceExtensionOptions.SWAPCHAIN_ONLY, MgQueueAllocation.One, MgQueueFlagBits.GRAPHICS_BIT | MgQueueFlagBits.COMPUTE_BIT))
					{                                                            
						if (device.Queues.Length > 0)
						{
							Console.WriteLine(nameof(device.Queues.Length) + " : " + device.Queues.Length);

                            using (var partition = device.Queues[0].CreatePartition(0))                            
                            {
                                IMgBuffer buffer;
                                var result = partition.Device.CreateBuffer(new MgBufferCreateInfo { SharingMode = MgSharingMode.EXCLUSIVE, Size = 1024, Usage = MgBufferUsageFlagBits.VERTEX_BUFFER_BIT }, null, out buffer);
                                buffer.DestroyBuffer(partition.Device, null);
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
	}
}
