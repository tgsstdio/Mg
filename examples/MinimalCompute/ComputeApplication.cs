using Magnesium;
using Magnesium.Vulkan;
using System;
using System.IO;
using System.Runtime.InteropServices;

// Translating minimalcompute into Magnesium
// https://github.com/Erkaman/vulkan_minimal_compute/blob/master/src/main.cpp

namespace MinimalCompute
{
    internal class ComputeApplication
    {
        public ComputeApplication()
        {
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Pixel
        {
            public float r;
            public float g;
            public float b;
            public float a;
        }

        IMgDescriptorSetLayout mDescSetLayout;
        private IMgBuffer mBuffer;
        private IMgDeviceMemory mDeviceMemory;
        private IMgDescriptorPool mDescriptorPool;
        private IMgDescriptorSet mDescriptorSet;
        private IMgPipelineLayout mPipelineLayout;
        private IMgPipeline mComputePipeline;
        private IMgCommandBuffer mCB;
        private IMgCommandPool mCommandPool;
        const int WORKGROUP_SIZE = 32;
        const int WIDTH = 3200; // Size of rendered mandelbrot set.
        const int HEIGHT = 2400; // Size of renderered mandelbrot set.

        void Running()
        {
            // Buffer size of the storage buffer that will contain the rendered mandelbrot set.
            var bufferSize = (ulong) (Marshal.SizeOf(typeof(Pixel)) * WIDTH * HEIGHT);

            // Initialize vulkan:
            //createInstance();
            //findPhysicalDevice();
            //createDevice();
            IMgThreadPartition partition = null;
            uint queueFamilyIndex = 0;
            CreateBuffer(partition, bufferSize);
            CreateDescriptorSetLayout(partition.Device);
            CreateDescriptorSet(partition.Device, bufferSize);
            CreateComputePipeline(partition.Device);
            CreateCommandBuffer(partition.Device, queueFamilyIndex);

            // Finally, run the recorded command buffer.
            RunCommandBuffer(partition.Device, partition.Queue);

            // The former command rendered a mandelbrot set to a buffer.
            // Save that buffer as a png on disk.
            SaveRenderedImage(partition.Device, bufferSize);

            // Clean up all vulkan resources.
            Cleanup(partition.Device);
        }

        private void SaveRenderedImage(IMgDevice device, ulong bufferSize)
        {
            // Map the buffer memory, so that we can read from it on the CPU.
            mDeviceMemory.MapMemory(device, 0, bufferSize, 0, out IntPtr mappedMemory);

            // Get the color data from the buffer, and cast it to bytes.
            // We save the data to a vector.
            const int byteLength = WIDTH * HEIGHT * 4;

            var src = new byte[byteLength];
            var stride = Marshal.SizeOf(typeof(Pixel));
            var srcOffset = 0;
            for (int i = 0; i < WIDTH * HEIGHT; i += 1)
            {
                IntPtr localPos = IntPtr.Add(mappedMemory, srcOffset);

                var p = (Pixel) Marshal.PtrToStructure(localPos, typeof(Pixel));

                var dstOffset = i * 4;
                src[dstOffset + 0] = (byte)(255.0f * p.r);
                src[dstOffset + 1] = (byte)(255.0f * p.g);
                src[dstOffset + 2] = (byte)(255.0f * p.b);
                src[dstOffset + 3] = (byte)(255.0f * p.a);
                srcOffset += stride;
            }

            // Done reading, so unmap.
            mDeviceMemory.UnmapMemory(device);

            var pinHandle = GCHandle.Alloc(src, GCHandleType.Pinned); //Pin the image data
            var scan0 = pinHandle.AddrOfPinnedObject();

            using (var b = new System.Drawing.Bitmap
                (
                    WIDTH,
                    HEIGHT,
                    4 * WIDTH,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb,
                    scan0
                )
            )
            {
                b.Save("mandelbrot.png", System.Drawing.Imaging.ImageFormat.Png);

                pinHandle.Free();
            }
        }

        static void VK_CHECK_RESULT(Result err) 																				
        {																																										
            if (err != Result.SUCCESS)																				
            {
                throw new Exception(err.ToString());
            }																									
        }

        void CreateBuffer(IMgThreadPartition partition, ulong bufferSize)
        {
            /*
            We will now create a buffer. We will render the mandelbrot set into this buffer
            in a computer shade later. 
            */

            var device = partition.Device;

            VK_CHECK_RESULT(
                device.CreateBuffer(new MgBufferCreateInfo
                {
                    Size = bufferSize, // buffer size in bytes. 
                    Usage = MgBufferUsageFlagBits.STORAGE_BUFFER_BIT,  // buffer is used as a storage buffer.
                    SharingMode = MgSharingMode.EXCLUSIVE, // buffer is exclusive to a single queue family at a time. 
                }
                , null
                , out IMgBuffer buffer)
            ); // create buffer.

            /*
            But the buffer doesn't allocate memory for itself, so we must do that manually.

            First, we find the memory requirements for the buffer.
            */
            device.GetBufferMemoryRequirements(buffer, out MgMemoryRequirements memoryRequirements);

            var isValid = partition.GetMemoryType(
                memoryRequirements.MemoryTypeBits,
                MgMemoryPropertyFlagBits.HOST_COHERENT_BIT | MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT,
                out uint typeIndex            
                );

            /*
            Now use obtained memory requirements info to allocate the memory for the buffer.
            */
            VK_CHECK_RESULT(
                device.AllocateMemory(
                    new MgMemoryAllocateInfo
                    {
                        AllocationSize = memoryRequirements.Size,

                        // specify required memory.
                        /*
                        There are several types of memory that can be allocated, and we must choose a memory type that:

                        1) Satisfies the memory requirements(memoryRequirements.memoryTypeBits). 
                        2) Satifies our own usage requirements. We want to be able to read the buffer memory from the GPU to the CPU
                            with vkMapMemory, so we set VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT. 
                        Also, by setting VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, memory written by the device(GPU) will be easily 
                        visible to the host(CPU), without having to call any extra flushing commands. So mainly for convenience, we set
                        this flag.
                        */
                        MemoryTypeIndex = typeIndex,
                    },
                    null,
                    out IMgDeviceMemory bufferMemory)
               ); // allocate memory on device.

            // Now associate that allocated memory with the buffer. With that, the buffer is backed by actual memory. 
            VK_CHECK_RESULT(
                buffer.BindBufferMemory(device, bufferMemory, 0)
                );

            mBuffer = buffer;
            mDeviceMemory = bufferMemory;
        }

        void CreateDescriptorSetLayout(IMgDevice device)
        {
            /*
            Here we specify a descriptor set layout. This allows us to bind our descriptors to 
            resources in the shader. 

            */

            /*
            Here we specify a binding of type VK_DESCRIPTOR_TYPE_STORAGE_BUFFER to the binding point
            0. This binds to 

              layout(std140, binding = 0) buffer buf

            in the compute shader.
            */

            // Create the descriptor set layout. 
            VK_CHECK_RESULT(
                device.CreateDescriptorSetLayout(
                    new MgDescriptorSetLayoutCreateInfo
                    {
                        Bindings = new MgDescriptorSetLayoutBinding[]
                        {
                            // only a single binding in this descriptor set layout. 
                            new MgDescriptorSetLayoutBinding
                            {
                                Binding = 0, // binding = 0
                                DescriptorType = MgDescriptorType.STORAGE_BUFFER,
                                DescriptorCount = 1,
                                StageFlags = MgShaderStageFlagBits.COMPUTE_BIT,
                            }
                        },                        
                    },
                    null,
                    out IMgDescriptorSetLayout dSetLayout
                )
            );

            mDescSetLayout = dSetLayout;
        }

        void CreateDescriptorSet(IMgDevice device, ulong bufferSize)
        {
            /*
            So we will allocate a descriptor set here.
            But we need to first create a descriptor pool to do that. 
            */
            // create descriptor pool.
            VK_CHECK_RESULT(
                device.CreateDescriptorPool(
                    new MgDescriptorPoolCreateInfo
                    {
                        MaxSets = 1, // we only need to allocate one descriptor set from the pool.
                        PoolSizes = new []
                        {
                            /*
                            Our descriptor pool can only allocate a single storage buffer.
                            */
                            new MgDescriptorPoolSize
                            {
                                Type = MgDescriptorType.STORAGE_BUFFER,
                                DescriptorCount = 1,
                            }
                        }
                    },  
                    null, 
                    out IMgDescriptorPool descriptorPool)
            );

            /*
            With the pool allocated, we can now allocate the descriptor set. 
            */

            // allocate descriptor set.
            VK_CHECK_RESULT(
                device.AllocateDescriptorSets(
                    new MgDescriptorSetAllocateInfo
                    {
                        DescriptorSetCount = 1, // allocate a single descriptor set.
                        DescriptorPool = descriptorPool, // pool to allocate from.
                        SetLayouts = new []
                        {
                            mDescSetLayout,
                        }
                    },
                out IMgDescriptorSet[] descriptorSets)
            );

            /*
            Next, we need to connect our actual storage buffer with the descrptor. 
            We use vkUpdateDescriptorSets() to update the descriptor set.
            */

            // Specify the buffer to bind to the descriptor.


            var writeDescriptorSet =  new MgWriteDescriptorSet {
                DstSet = descriptorSets[0],  // write to this descriptor set.
                DstBinding = 0, // write to the first, and only binding.
                DescriptorCount = 1,  // update a single descriptor.
                DescriptorType = MgDescriptorType.STORAGE_BUFFER,
                BufferInfo = new []
                {
                    new MgDescriptorBufferInfo
                    {
                        Buffer = mBuffer,
                        Offset = 0,
                        Range = bufferSize,
                    }
                },
            };

            // perform the update of the descriptor set.
            device.UpdateDescriptorSets(new[] { writeDescriptorSet } , null);

            mDescriptorPool = descriptorPool;
            mDescriptorSet = descriptorSets[0];
        }

        void CreateComputePipeline(IMgDevice device)
        {
            VK_CHECK_RESULT(
                device.CreatePipelineLayout(
                new MgPipelineLayoutCreateInfo
                {
                    SetLayouts = new[]
                    {
                        mDescSetLayout,
                    }
                },
                null,
                out IMgPipelineLayout pLayout)
            );


           using (var fs = File.Open("shaders/green.comp.spv", FileMode.Open))
           {
                VK_CHECK_RESULT(
                    device.CreateShaderModule(
                        new MgShaderModuleCreateInfo
                        {
                            Code = fs,
                            CodeSize = new UIntPtr((ulong)fs.Length),
                        }
                        , null
                        , out IMgShaderModule sModule
                    )
                );

                VK_CHECK_RESULT(
                    device.CreateComputePipelines(
                        null,
                        new[] {
                            new MgComputePipelineCreateInfo
                            {
                                Stage = new MgPipelineShaderStageCreateInfo
                                {
                                    Name = "csMain",
                                    Module = sModule,
                                    Stage = MgShaderStageFlagBits.COMPUTE_BIT,
                                },
                                Flags = 0,
                                Layout = pLayout,
                                ThreadsPerWorkgroup = new MgVec3Ui { X = 32U, Y = 32U, Z = 1U },
                            } },
                        null,
                        out IMgPipeline[] pipelines)
                );

                sModule.DestroyShaderModule(device, null);

                mPipelineLayout = pLayout;
                mComputePipeline = pipelines[0];
            }
        }

        void CreateCommandBuffer(IMgDevice device, uint queueFamilyIndex)
        {
            /*
            We are getting closer to the end. In order to send commands to the device(GPU),
            we must first record commands into a command buffer.
            To allocate a command buffer, we must first create a command pool. So let us do that.
            */
            // the queue family of this command pool. All command buffers allocated from this command pool,
            // must be submitted to queues of this family ONLY. 
            VK_CHECK_RESULT(
                device.CreateCommandPool(
                    new MgCommandPoolCreateInfo
                    {
                        QueueFamilyIndex = queueFamilyIndex,
                    }, 
                    null,
                    out IMgCommandPool commandPool
                )
            );

            /*
            Now allocate a command buffer from the command pool. 
            */
            IMgCommandBuffer[] commandBuffers = new IMgCommandBuffer[1];

            VK_CHECK_RESULT(
                device.AllocateCommandBuffers(
                    new MgCommandBufferAllocateInfo
                    {
                        CommandPool = commandPool,
                        // specify the command pool to allocate from. 
                        // if the command buffer is primary, it can be directly submitted to queues. 
                        // A secondary buffer has to be called from some primary command buffer, and cannot be directly 
                        // submitted to a queue. To keep things simple, we use a primary command buffer. 
                        CommandBufferCount = 1,  // allocate a single command buffer. 
                        Level = MgCommandBufferLevel.PRIMARY,
                    },
                    commandBuffers
                )
            ); // allocate command buffer.

            /*
            Now we shall start recording commands into the newly allocated command buffer. 
            */
            var cb = commandBuffers[0];

            VK_CHECK_RESULT(
                cb.BeginCommandBuffer(
                    new MgCommandBufferBeginInfo
                    {
                        // the buffer is only submitted and used once in this application.
                        Flags = MgCommandBufferUsageFlagBits.ONE_TIME_SUBMIT_BIT,
                    } 
                )
            ); // start recording commands.

            /*
            We need to bind a pipeline, AND a descriptor set before we dispatch.

            The validation layer will NOT give warnings if you forget these, so be very careful not to forget them.
            */
            cb.CmdBindPipeline(MgPipelineBindPoint.COMPUTE, mComputePipeline);
            cb.CmdBindDescriptorSets(MgPipelineBindPoint.COMPUTE, mPipelineLayout, 0, 1, new[] { mDescriptorSet }, null);

            /*
            Calling vkCmdDispatch basically starts the compute pipeline, and executes the compute shader.
            The number of workgroups is specified in the arguments.
            If you are already familiar with compute shaders from OpenGL, this should be nothing new to you.
            */
            var groupSize = (float) WORKGROUP_SIZE;

            cb.CmdDispatch(
                (uint) Math.Ceiling(WIDTH / groupSize),
                (uint) Math.Ceiling(HEIGHT / groupSize),
                1);

            VK_CHECK_RESULT(
                cb.EndCommandBuffer()
            ); // end recording commands.

            mCB = cb;
            mCommandPool = commandPool;
        }

        void RunCommandBuffer(IMgDevice device, IMgQueue queue)
        {
            /*
            Now we shall finally submit the recorded command buffer to a queue.
            */

            /*
              We create a fence.
            */
            VK_CHECK_RESULT(
                device.CreateFence(
                    new MgFenceCreateInfo
                    {

                    },
                    null,
                    out IMgFence fence));

            /*
            We submit the command buffer on the queue, at the same time giving a fence.
            */
            VK_CHECK_RESULT(
                queue.QueueSubmit(
                    new[]
                    {
                        new MgSubmitInfo
                        {
                            // submit a single command buffer
                            CommandBuffers = new[]
                            {
                                // the command buffer to submit.
                                mCB
                            }
                        }
                    },
                    fence
                )
            );
            /*
            The command will not have finished executing until the fence is signalled.
            So we wait here.
            We will directly after this read our buffer from the GPU,
            and we will not be sure that the command has finished executing unless we wait for the fence.
            Hence, we use a fence here.
            */
            VK_CHECK_RESULT(
                device.WaitForFences(
                    new[] { fence },
                    true,
                    100000000000)
                );

            fence.DestroyFence(device, null);
        }


        void Cleanup(IMgDevice device)
        {
            /*
            Clean up all Vulkan Resources. 
            */
            mDeviceMemory.FreeMemory(device, null);
            mBuffer.DestroyBuffer(device, null);
            mDescriptorPool.DestroyDescriptorPool(device, null);
            mDescSetLayout.DestroyDescriptorSetLayout(device, null);
            mPipelineLayout.DestroyPipelineLayout(device, null);
            mComputePipeline.DestroyPipeline(device, null);
            mCommandPool.DestroyCommandPool(device, null);
        }


        internal void Run()
        {
            try
            {
                var entrypoint = new VkEntrypoint();


                using (var driver = new MgDriverContext(entrypoint))
                {
                    driver.Initialize(new MgApplicationInfo
                    {
                        ApiVersion = MgApplicationInfo.GenerateApiVersion(1, 0, 17),
                        ApplicationName = "MinimalCompute",
                        ApplicationVersion = 1,
                        EngineName = "Magnesium.Vulkan",
                        EngineVersion = 1,
                    },
                    MgInstanceExtensionOptions.ALL);

                    using (var logicalDevice = driver.CreateLogicalDevice(null, MgDeviceExtensionOptions.SWAPCHAIN_ONLY, MgQueueAllocation.One, MgQueueFlagBits.GRAPHICS_BIT | MgQueueFlagBits.COMPUTE_BIT))
                    {                       
                        if (logicalDevice.Queues.Length > 0)
                        {
                            Console.WriteLine(nameof(logicalDevice.Queues.Length) + " : " + logicalDevice.Queues.Length);

                            var queueFamily = logicalDevice.Queues[0];
                            using (var partition = queueFamily.CreatePartition(0))
                            { 
                                var device = partition.Device;

                                // Buffer size of the storage buffer that will contain the rendered mandelbrot set.
                                var bufferSize = (ulong)(Marshal.SizeOf(typeof(Pixel)) * WIDTH * HEIGHT);

                                // Initialize vulkan:
                                //createInstance();
                                //findPhysicalDevice();
                                //createDevice();
                                CreateBuffer(partition, bufferSize);
                                CreateDescriptorSetLayout(device);
                                CreateDescriptorSet(device, bufferSize);
                                CreateComputePipeline(device);
                                CreateCommandBuffer(device, queueFamily.QueueFamilyIndex);

                                // Finally, run the recorded command buffer.
                                RunCommandBuffer(device, partition.Queue);

                                // The former command rendered a mandelbrot set to a buffer.
                                // Save that buffer as a png on disk.
                                SaveRenderedImage(device, bufferSize);

                                // Clean up all vulkan resources.
                                Cleanup(device);
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