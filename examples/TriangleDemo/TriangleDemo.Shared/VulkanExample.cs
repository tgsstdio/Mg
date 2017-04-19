/*
* Translation into C# and Magnesium interface 2016
* Vulkan Example - Basic indexed triangle rendering by 2016 by Copyright (C) Sascha Willems - www.saschawillems.de
*
* This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
*/

using Magnesium;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TriangleDemo
{
    public class VulkanExample : IDisposable
    {
        // Vertex buffer and attributes
        class VertexBufferInfo
        {
            public IMgDeviceMemory memory;	// Handle to the device memory for this buffer
            public IMgBuffer buffer; // Handle to the Vulkan buffer object that the memory is bound to
            public MgPipelineVertexInputStateCreateInfo inputState;
            public MgVertexInputBindingDescription inputBinding;
            public MgVertexInputAttributeDescription[] inputAttributes;
        }

        VertexBufferInfo vertices = new VertexBufferInfo();

        struct IndicesInfo
        {
            public IMgDeviceMemory memory;
            public IMgBuffer buffer;
            public UInt32 count;
        }

        IndicesInfo indices = new IndicesInfo();

        private IMgGraphicsConfiguration mConfiguration;

        // Uniform block object
        struct UniformData
        {
            public IMgDeviceMemory memory;
            public IMgBuffer buffer;
            public MgDescriptorBufferInfo descriptor;
        }

        struct UniformBufferObject
        {
            public Matrix4 projectionMatrix;
            public Matrix4 modelMatrix;
            public Matrix4 viewMatrix;
        };

        UniformBufferObject uboVS;


        UniformData uniformDataVS = new UniformData();

        // The pipeline layout is used by a pipline to access the descriptor sets 
        // It defines interface (without binding any actual data) between the shader stages used by the pipeline and the shader resources
        // A pipeline layout can be shared among multiple pipelines as long as their interfaces match
        IMgPipelineLayout mPipelineLayout;

        // Pipelines (often called "pipeline state objects") are used to bake all states that affect a pipeline
        // While in OpenGL every state can be changed at (almost) any time, Vulkan requires to layout the graphics (and compute) pipeline states upfront
        // So for each combination of non-dynamic pipeline states you need a new pipeline (there are a few exceptions to this not discussed here)
        // Even though this adds a new dimension of planing ahead, it's a great opportunity for performance optimizations by the driver
        IMgPipeline mPipeline;

        // The descriptor set layout describes the shader binding layout (without actually referencing descriptor)
        // Like the pipeline layout it's pretty much a blueprint and can be used with different descriptor sets as long as their layout matches
        IMgDescriptorSetLayout mDescriptorSetLayout;

        // The descriptor set stores the resources bound to the binding points in a shader
        // It connects the binding points of the different shaders with the buffers and images used for those bindings
        IMgDescriptorSet mDescriptorSet;


        // Synchronization primitives
        // Synchronization is an important concept of Vulkan that OpenGL mostly hid away. Getting this right is crucial to using Vulkan.

        // Semaphores
        // Used to coordinate operations within the graphics queue and ensure correct command ordering
        IMgSemaphore mPresentCompleteSemaphore;
        IMgSemaphore mRenderCompleteSemaphore;

        // Fences
        // Used to check the completion of queue operations (e.g. command buffer execution)
        List<IMgFence> mWaitFences = new List<IMgFence>();

        private uint mWidth;
        private uint mHeight;
        private IMgGraphicsDevice mGraphicsDevice;
        private IMgDescriptorPool mDescriptorPool;

        private IMgCommandBuffer mPrePresentCmdBuffer;
        private IMgCommandBuffer mPostPresentCmdBuffer;
        private IMgPresentationLayer mPresentationLayer;

        public VulkanExample
        (
            IMgGraphicsConfiguration configuration,
            IMgSwapchainCollection swapchains,
            IMgGraphicsDevice graphicsDevice,
            IMgPresentationLayer presentationLayer,
            ITriangleDemoShaderPath shaderPath
        )
        {
            mConfiguration = configuration;
            mSwapchains = swapchains;
            mGraphicsDevice = graphicsDevice;
            mPresentationLayer = presentationLayer;
            mTrianglePath = shaderPath;

            mWidth = 1280U;
            mHeight = 720U;

            try
            {
                mConfiguration.Initialize(mWidth, mHeight);
                initSwapchain(mWidth, mHeight);
                prepare();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private IMgSwapchainCollection mSwapchains;
        private void initSwapchain(uint width, uint height)
        {
            Debug.Assert(mConfiguration.Partition != null);


            const int NO_OF_BUFFERS = 1;
            var buffers = new IMgCommandBuffer[NO_OF_BUFFERS];
            var pAllocateInfo = new MgCommandBufferAllocateInfo
            {
                CommandBufferCount = NO_OF_BUFFERS,
                CommandPool = mConfiguration.Partition.CommandPool,
                Level = MgCommandBufferLevel.PRIMARY,
            };

            mConfiguration.Device.AllocateCommandBuffers(pAllocateInfo, buffers);

            var createInfo = new MgGraphicsDeviceCreateInfo
            {
                Samples = MgSampleCountFlagBits.COUNT_1_BIT,
                Color = MgFormat.R8G8B8A8_UINT,
                DepthStencil = MgFormat.D24_UNORM_S8_UINT,
                Width = mWidth,
                Height = mHeight,
            };

            var setupCmdBuffer = buffers[0];

            var cmdBufInfo = new MgCommandBufferBeginInfo
            {

            };


            var err = setupCmdBuffer.BeginCommandBuffer(cmdBufInfo);
            Debug.Assert(err == Result.SUCCESS);

            mGraphicsDevice.Create(setupCmdBuffer, mSwapchains, createInfo);

            err = setupCmdBuffer.EndCommandBuffer();
            Debug.Assert(err == Result.SUCCESS);


            var submission = new[] {
                new MgSubmitInfo
                {
                    CommandBuffers = new IMgCommandBuffer[]
                    {
                        buffers[0],
                    },
                }
            };

            err = mConfiguration.Queue.QueueSubmit(submission, null);
            Debug.Assert(err == Result.SUCCESS);

            mConfiguration.Queue.QueueWaitIdle();

            mConfiguration.Device.FreeCommandBuffers(mConfiguration.Partition.CommandPool, buffers);
        }

        #region prepare

        private bool mPrepared = false;
        void prepare()
        {
            BeforePrepare();

            prepareSynchronizationPrimitives();
            prepareVertices();
            prepareUniformBuffers();
            setupDescriptorSetLayout();
            preparePipelines();
            setupDescriptorPool();
            setupDescriptorSet();
            buildCommandBuffers();
            mPrepared = true;
        }

        private void BeforePrepare()
        {
            createCommandBuffers();
        }

        // Create the Vulkan synchronization primitives used in this example
        void prepareSynchronizationPrimitives()
        {
            // Semaphores (Used for correct command ordering)
            var semaphoreCreateInfo = new MgSemaphoreCreateInfo { };

            Debug.Assert(mConfiguration.Device != null);

            // Semaphore used to ensures that image presentation is complete before starting to submit again
            var err = mConfiguration.Device.CreateSemaphore(semaphoreCreateInfo, null, out mPresentCompleteSemaphore);
            Debug.Assert(err == Result.SUCCESS);

            // Semaphore used to ensures that all commands submitted have been finished before submitting the image to the queue
            err = mConfiguration.Device.CreateSemaphore(semaphoreCreateInfo, null, out mRenderCompleteSemaphore);
            Debug.Assert(err == Result.SUCCESS);

            // Fences (Used to check draw command buffer completion)
            var fenceCreateInfo = new MgFenceCreateInfo {
                Flags = MgFenceCreateFlagBits.SIGNALED_BIT,
            };

            // Create in signaled state so we don't wait on first render of each command buffer
            var noOfCommandBuffers = drawCmdBuffers.Length; // TODO: drawCmdBuffers.Length;
            for (var i = 0; i < noOfCommandBuffers; ++i)
            {
                IMgFence fence;
                err = mConfiguration.Device.CreateFence(fenceCreateInfo, null, out fence);
                Debug.Assert(err == Result.SUCCESS);
                mWaitFences.Add(fence);
            }
        }

        struct TriangleVertex
        {
            public Vector3 position;
            public Vector3 color;
        };

        class StagingBuffer
        {
            public IMgDeviceMemory memory;
            public IMgBuffer buffer;
        };

        // Prepare vertex and index buffers for an indexed triangle
        // Also uploads them to device local memory using staging and initializes vertex input and attribute binding to match the vertex shader
        void prepareVertices()
        {
            // A note on memory management in Vulkan in general:
            //	This is a very complex topic and while it's fine for an example application to to small individual memory allocations that is not
            //	what should be done a real-world application, where you should allocate large chunkgs of memory at once isntead.

            // Setup vertices
            TriangleVertex[] vertexBuffer =
            {
                new TriangleVertex{
                    position =new Vector3(1.0f,  1.0f, 0.0f),
                    color = new Vector3( 1.0f, 0.0f, 0.0f )
                },

                new TriangleVertex{
                    position =new Vector3(-1.0f,  1.0f, 0.0f ),
                    color = new Vector3( 0.0f, 1.0f, 0.0f )
                },

                new TriangleVertex{
                    position =new Vector3( 0.0f, -1.0f, 0.0f ),
                    color = new Vector3( 0.0f, 0.0f, 1.0f )
                },
            };

            var structSize = Marshal.SizeOf(typeof(TriangleVertex));
            var vertexBufferSize = (ulong)(vertexBuffer.Length * structSize);

            // Setup indices
            UInt32[] indexBuffer = { 0, 1, 2 };
            indices.count = (uint)indexBuffer.Length;
            var indexBufferSize = indices.count * sizeof(UInt32);


            // Static data like vertex and index buffer should be stored on the device memory 
            // for optimal (and fastest) access by the GPU
            //
            // To achieve this we use so-called "staging buffers" :
            // - Create a buffer that's visible to the host (and can be mapped)
            // - Copy the data to this buffer
            // - Create another buffer that's local on the device (VRAM) with the same size
            // - Copy the data from the host to the device using a command buffer
            // - Delete the host visible (staging) buffer
            // - Use the device local buffers for rendering


            var stagingBuffers = new
            {
                vertices = new StagingBuffer(),
                indices = new StagingBuffer(),
            };

            // HOST_VISIBLE STAGING Vertex buffer
            {
                var vertexBufferInfo = new MgBufferCreateInfo
                {
                    Size = vertexBufferSize,

                    // Buffer is used as the copy source
                    Usage = MgBufferUsageFlagBits.TRANSFER_SRC_BIT,
                };

                // Create a host-visible buffer to copy the vertex data to (staging buffer)
                var err = mConfiguration.Device.CreateBuffer(vertexBufferInfo, null, out stagingBuffers.vertices.buffer);
                Debug.Assert(err == Result.SUCCESS);

                MgMemoryRequirements memReqs;
                mConfiguration.Device.GetBufferMemoryRequirements(stagingBuffers.vertices.buffer, out memReqs);

                // Request a host visible memory type that can be used to copy our data do
                // Also request it to be coherent, so that writes are visible to the GPU right after unmapping the buffer
                uint typeIndex;
                var isValid = mConfiguration.Partition.GetMemoryType(memReqs.MemoryTypeBits,
                    MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT,
                    out typeIndex);

                Debug.Assert(isValid);

                MgMemoryAllocateInfo memAlloc = new MgMemoryAllocateInfo
                {
                    AllocationSize = memReqs.Size,
                    MemoryTypeIndex = typeIndex,
                };

                err = mConfiguration.Device.AllocateMemory(memAlloc, null, out stagingBuffers.vertices.memory);
                Debug.Assert(err == Result.SUCCESS);

                // Map and copy
                IntPtr data;
                err = stagingBuffers.vertices.memory.MapMemory(mConfiguration.Device, 0, memAlloc.AllocationSize, 0, out data);
                Debug.Assert(err == Result.SUCCESS);

                var offset = 0;
                foreach (var vertex in vertexBuffer)
                {
                    IntPtr dest = IntPtr.Add(data, offset);
                    Marshal.StructureToPtr(vertex, dest, false);
                    offset += structSize;
                }

                stagingBuffers.vertices.memory.UnmapMemory(mConfiguration.Device);

                stagingBuffers.vertices.buffer.BindBufferMemory(mConfiguration.Device, stagingBuffers.vertices.memory, 0);
                Debug.Assert(err == Result.SUCCESS);
            }

            // DEVICE_LOCAL Vertex buffer
            {
                var vertexBufferInfo = new MgBufferCreateInfo
                {
                    Size = vertexBufferSize,
                    // Create a device local buffer to which the (host local) vertex data will be copied and which will be used for rendering
                    Usage = MgBufferUsageFlagBits.VERTEX_BUFFER_BIT | MgBufferUsageFlagBits.TRANSFER_DST_BIT,
                };

                var err = mConfiguration.Device.CreateBuffer(vertexBufferInfo, null, out vertices.buffer);
                Debug.Assert(err == Result.SUCCESS);

                MgMemoryRequirements memReqs;
                mConfiguration.Device.GetBufferMemoryRequirements(vertices.buffer, out memReqs);


                uint typeIndex;
                var isValid = mConfiguration.Partition.GetMemoryType(memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT, out typeIndex);
                Debug.Assert(isValid);

                var memAlloc = new MgMemoryAllocateInfo
                {
                    AllocationSize = memReqs.Size,
                    MemoryTypeIndex = typeIndex,
                };

                err = mConfiguration.Device.AllocateMemory(memAlloc, null, out vertices.memory);
                Debug.Assert(err == Result.SUCCESS);

                err = vertices.buffer.BindBufferMemory(mConfiguration.Device, vertices.memory, 0);
                Debug.Assert(err == Result.SUCCESS);
            }

            // HOST_VISIBLE Index buffer
            {
                var indexbufferInfo = new MgBufferCreateInfo
                {
                    Size = indexBufferSize,
                    Usage = MgBufferUsageFlagBits.TRANSFER_SRC_BIT,
                };

                // Copy index data to a buffer visible to the host (staging buffer)
                var err = mConfiguration.Device.CreateBuffer(indexbufferInfo, null, out stagingBuffers.indices.buffer);
                Debug.Assert(err == Result.SUCCESS);

                MgMemoryRequirements memReqs;
                mConfiguration.Device.GetBufferMemoryRequirements(stagingBuffers.indices.buffer, out memReqs);

                uint typeIndex;
                var isValid = mConfiguration.Partition.GetMemoryType(memReqs.MemoryTypeBits,
                    MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT,
                    out typeIndex);
                Debug.Assert(isValid);

                var memAlloc = new MgMemoryAllocateInfo
                {
                    AllocationSize = memReqs.Size,
                    MemoryTypeIndex = typeIndex,
                };

                err = mConfiguration.Device.AllocateMemory(memAlloc, null, out stagingBuffers.indices.memory);
                Debug.Assert(err == Result.SUCCESS);

                IntPtr data;
                err = stagingBuffers.indices.memory.MapMemory(mConfiguration.Device, 0, indexBufferSize, 0, out data);
                Debug.Assert(err == Result.SUCCESS);

                var uintBuffer = new byte[indexBufferSize];

                var bufferSize = (int)indexBufferSize;
                Buffer.BlockCopy(indexBuffer, 0, uintBuffer, 0, bufferSize);
                Marshal.Copy(uintBuffer, 0, data, bufferSize);

                stagingBuffers.indices.memory.UnmapMemory(mConfiguration.Device);

                err = stagingBuffers.indices.buffer.BindBufferMemory(mConfiguration.Device, stagingBuffers.indices.memory, 0);
                Debug.Assert(err == Result.SUCCESS);
            }

            // DEVICE_LOCAL index buffer
            {
                // Create destination buffer with device only visibility
                var indexbufferInfo = new MgBufferCreateInfo
                {
                    Size = indexBufferSize,
                    Usage = MgBufferUsageFlagBits.INDEX_BUFFER_BIT | MgBufferUsageFlagBits.TRANSFER_DST_BIT,
                };

                var err = mConfiguration.Device.CreateBuffer(indexbufferInfo, null, out indices.buffer);
                Debug.Assert(err == Result.SUCCESS);

                MgMemoryRequirements memReqs;
                mConfiguration.Device.GetBufferMemoryRequirements(indices.buffer, out memReqs);

                uint typeIndex;
                var isValid = mConfiguration.Partition.GetMemoryType(memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT, out typeIndex);
                Debug.Assert(isValid);

                var memAlloc = new MgMemoryAllocateInfo
                {
                    AllocationSize = memReqs.Size,
                    MemoryTypeIndex = typeIndex,
                };

                err = mConfiguration.Device.AllocateMemory(memAlloc, null, out indices.memory);
                Debug.Assert(err == Result.SUCCESS);

                err = indices.buffer.BindBufferMemory(mConfiguration.Device, indices.memory, 0);
                Debug.Assert(err == Result.SUCCESS);
            }

            {
                var cmdBufferBeginInfo = new MgCommandBufferBeginInfo
                {

                };

                // Buffer copies have to be submitted to a queue, so we need a command buffer for them
                // Note: Some devices offer a dedicated transfer queue (with only the transfer bit set) that may be faster when doing lots of copies
                IMgCommandBuffer copyCmd = getCommandBuffer(true);

                // Put buffer region copies into command buffer

                // Vertex buffer
                copyCmd.CmdCopyBuffer(
                    stagingBuffers.vertices.buffer,
                    vertices.buffer,
                    new[]
                    {
                        new MgBufferCopy
                        {
                            Size = vertexBufferSize,
                        }
                    }
                );

                // Index buffer
                copyCmd.CmdCopyBuffer(stagingBuffers.indices.buffer, indices.buffer,
                    new[]
                    {
                        new MgBufferCopy
                        {
                            Size = indexBufferSize,
                        }
                    });

                // Flushing the command buffer will also submit it to the queue and uses a fence to ensure that all commands have been executed before returning
                flushCommandBuffer(copyCmd);

                // Destroy staging buffers
                // Note: Staging buffer must not be deleted before the copies have been submitted and executed
                stagingBuffers.vertices.buffer.DestroyBuffer(mConfiguration.Device, null);
                stagingBuffers.vertices.memory.FreeMemory(mConfiguration.Device, null);
                stagingBuffers.indices.buffer.DestroyBuffer(mConfiguration.Device, null);
                stagingBuffers.indices.memory.FreeMemory(mConfiguration.Device, null);
            }

            // Vertex input binding
            const uint VERTEX_BUFFER_BIND_ID = 0;

            vertices.inputBinding = new MgVertexInputBindingDescription
            {
                Binding = VERTEX_BUFFER_BIND_ID,
                Stride = (uint) structSize,
                InputRate = MgVertexInputRate.VERTEX,
            };

            // Inpute attribute binding describe shader attribute locations and memory layouts
            // These match the following shader layout (see triangle.vert):
            //	layout (location = 0) in vec3 inPos;
            //	layout (location = 1) in vec3 inColor;

            var vertexSize = (uint) Marshal.SizeOf(typeof(Vector3));

            vertices.inputAttributes = new MgVertexInputAttributeDescription[]
            {
                new MgVertexInputAttributeDescription
                {
                    // Attribute location 0: Position
                    Binding = VERTEX_BUFFER_BIND_ID,
                    Location = 0,
                    Format =  MgFormat.R32G32B32_SFLOAT,
                    Offset = 0,
                },                             
                new MgVertexInputAttributeDescription
                {
                    // Attribute location 1: Color
                    Binding = VERTEX_BUFFER_BIND_ID,
                    Location = 1,
                    Format = MgFormat.R32G32B32_SFLOAT,
                    Offset = vertexSize,
                }
            };

            // Assign to the vertex input state used for pipeline creation
            vertices.inputState = new MgPipelineVertexInputStateCreateInfo
            {
                VertexBindingDescriptions = new MgVertexInputBindingDescription[]
                {
                    vertices.inputBinding,
                },
                VertexAttributeDescriptions = vertices.inputAttributes,
            };                
        }

        // Get a new command buffer from the command pool
        // If begin is true, the command buffer is also started so we can start adding commands
        IMgCommandBuffer getCommandBuffer(bool begin)
        {
            var buffers = new IMgCommandBuffer[1];

            var cmdBufAllocateInfo = new MgCommandBufferAllocateInfo
            {
                CommandPool = mConfiguration.Partition.CommandPool,
                Level =  MgCommandBufferLevel.PRIMARY,
                CommandBufferCount = 1,
            };

            var err = mConfiguration.Device.AllocateCommandBuffers(cmdBufAllocateInfo, buffers);
            Debug.Assert(err == Result.SUCCESS);

            var cmdBuf = buffers[0];

            // If requested, also start the new command buffer
            if (begin)
            {
                var cmdBufInfo = new MgCommandBufferBeginInfo
                {

                };

                err = cmdBuf.BeginCommandBuffer(cmdBufInfo);
                Debug.Assert(err == Result.SUCCESS);
            }

            return cmdBuf;
        }


        // End the command buffer and submit it to the queue
        // Uses a fence to ensure command buffer has finished executing before deleting it
        void flushCommandBuffer(IMgCommandBuffer commandBuffer)
        {
            Debug.Assert(commandBuffer != null);

            var err = commandBuffer.EndCommandBuffer();
            Debug.Assert(err == Result.SUCCESS);

            var submitInfos = new MgSubmitInfo[] 
            {
                new MgSubmitInfo
                {
                    CommandBuffers = new []
                    {
                        commandBuffer
                    }
                }
            };

            // Create fence to ensure that the command buffer has finished executing
            var fenceCreateInfo = new MgFenceCreateInfo
            {

            };

            IMgFence fence;
            err = mConfiguration.Device.CreateFence(fenceCreateInfo, null, out fence);
            Debug.Assert(err == Result.SUCCESS);

            // Submit to the queue
            err = mConfiguration.Queue.QueueSubmit(submitInfos, fence);
            Debug.Assert(err == Result.SUCCESS);

            // Mg.OpenGL
            err = mConfiguration.Queue.QueueWaitIdle();
            Debug.Assert(err == Result.SUCCESS);

            // Wait for the fence to signal that command buffer has finished executing
            err = mConfiguration.Device.WaitForFences(new[] { fence }, true, ulong.MaxValue);
            Debug.Assert(err == Result.SUCCESS);

            fence.DestroyFence(mConfiguration.Device, null);
            mConfiguration.Device.FreeCommandBuffers(mConfiguration.Partition.CommandPool, new[] { commandBuffer } );
        }

        void prepareUniformBuffers()
        {

            var structSize = (uint)Marshal.SizeOf(typeof(UniformBufferObject));

            // Vertex shader uniform buffer block
            MgBufferCreateInfo bufferInfo = new MgBufferCreateInfo
            {
                Size = structSize,
                // This buffer will be used as a uniform buffer
                Usage = MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT,
            };

            // Create a new buffer
            var err = mConfiguration.Device.CreateBuffer(bufferInfo, null, out uniformDataVS.buffer);
            Debug.Assert(err == Result.SUCCESS);

            // Prepare and initialize a uniform buffer block containing shader uniforms
            // Single uniforms like in OpenGL are no longer present in Vulkan. All Shader uniforms are passed via uniform buffer blocks
            MgMemoryRequirements memReqs;

            // Get memory requirements including size, alignment and memory type 
            mConfiguration.Device.GetBufferMemoryRequirements(uniformDataVS.buffer, out memReqs);


            // Get the memory type index that supports host visibile memory access
            // Most implementations offer multiple memory types and selecting the correct one to allocate memory from is crucial
            // We also want the buffer to be host coherent so we don't have to flush (or sync after every update.
            // Note: This may affect performance so you might not want to do this in a real world application that updates buffers on a regular base
            uint typeIndex;
            var isValid = mConfiguration.Partition.GetMemoryType(memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT, out typeIndex);
            Debug.Assert(isValid);

            MgMemoryAllocateInfo allocInfo = new MgMemoryAllocateInfo
            {
                AllocationSize = memReqs.Size,
                MemoryTypeIndex = typeIndex,
            };

            // Allocate memory for the uniform buffer
            err = mConfiguration.Device.AllocateMemory(allocInfo, null, out uniformDataVS.memory);
            Debug.Assert(err == Result.SUCCESS);

            // Bind memory to buffer
            err = uniformDataVS.buffer.BindBufferMemory(mConfiguration.Device, uniformDataVS.memory, 0);
            Debug.Assert(err == Result.SUCCESS);

            // Store information in the uniform's descriptor that is used by the descriptor set
            uniformDataVS.descriptor = new MgDescriptorBufferInfo
            {
                Buffer = uniformDataVS.buffer,
                Offset = 0,
                Range = structSize,
            };

            updateUniformBuffers();
        }


        void setupDescriptorSetLayout()
        {
            // Setup layout of descriptors used in this example
            // Basically connects the different shader stages to descriptors for binding uniform buffers, image samplers, etc.
            // So every shader binding should map to one descriptor set layout binding
            var descriptorLayout = new MgDescriptorSetLayoutCreateInfo
            {
                Bindings = new[]
                {
                    // Binding 0: Uniform buffer (Vertex shader)
                    new MgDescriptorSetLayoutBinding
                    {
                        DescriptorCount = 1,
                        StageFlags = MgShaderStageFlagBits.VERTEX_BIT,
                        ImmutableSamplers = null,
                        DescriptorType = MgDescriptorType.UNIFORM_BUFFER,
                        Binding = 0,                         
                    }
                },
            };

            var err = mConfiguration.Device.CreateDescriptorSetLayout(descriptorLayout, null, out mDescriptorSetLayout);
            Debug.Assert(err == Result.SUCCESS);

            // Create the pipeline layout that is used to generate the rendering pipelines that are based on this descriptor set layout
            // In a more complex scenario you would have different pipeline layouts for different descriptor set layouts that could be reused
            var pPipelineLayoutCreateInfo = new MgPipelineLayoutCreateInfo
            {
                 SetLayouts = new IMgDescriptorSetLayout[]
                 {
                     mDescriptorSetLayout,
                 }
            };

            err = mConfiguration.Device.CreatePipelineLayout(pPipelineLayoutCreateInfo, null, out mPipelineLayout);
            Debug.Assert(err == Result.SUCCESS);
        }

        void preparePipelines()
        {
            // System.IO.File.OpenRead("shaders/triangle.vert.spv")
            using (var vertFs = mTrianglePath.OpenVertexShader())
            // System.IO.File.OpenRead("shaders/triangle.frag.spv")
            using (var fragFs = mTrianglePath.OpenFragmentShader())
            {
                // Load shaders
                // Vulkan loads it's shaders from an immediate binary representation called SPIR-V
                // Shaders are compiled offline from e.g. GLSL using the reference glslang compiler

                IMgShaderModule vsModule;
                {
                    var vsCreateInfo = new MgShaderModuleCreateInfo
                    {
                        Code = vertFs,
                        CodeSize = new UIntPtr((ulong)vertFs.Length),
                    };
                    //  shaderStages[0] = loadShader(getAssetPath() + "shaders/triangle.vert.spv", VK_SHADER_STAGE_VERTEX_BIT);
                    mConfiguration.Device.CreateShaderModule(vsCreateInfo, null, out vsModule);
                }

                IMgShaderModule fsModule;
                {
                    var fsCreateInfo = new MgShaderModuleCreateInfo
                    {
                        Code = fragFs,
                        CodeSize = new UIntPtr((ulong)fragFs.Length),
                    };
                    // shaderStages[1] = loadShader(getAssetPath() + "shaders/triangle.frag.spv", VK_SHADER_STAGE_FRAGMENT_BIT);
                    mConfiguration.Device.CreateShaderModule(fsCreateInfo, null, out fsModule);
                }

                // Create the graphics pipeline used in this example
                // Vulkan uses the concept of rendering pipelines to encapsulate fixed states, replacing OpenGL's complex state machine
                // A pipeline is then stored and hashed on the GPU making pipeline changes very fast
                // Note: There are still a few dynamic states that are not directly part of the pipeline (but the info that they are used is)

                var pipelineCreateInfo = new MgGraphicsPipelineCreateInfo
                {

                    Stages = new MgPipelineShaderStageCreateInfo[]
                    {
                        new MgPipelineShaderStageCreateInfo
                        {
                            Stage = MgShaderStageFlagBits.VERTEX_BIT,
                            Module = vsModule,
                            Name = "vertFunc",
                        },
                        new MgPipelineShaderStageCreateInfo
                        {
                            Stage = MgShaderStageFlagBits.FRAGMENT_BIT,
                            Module = fsModule,
                            Name = "fragFunc",
                        },
                    },

                    VertexInputState = vertices.inputState,

                    // Construct the differnent states making up the pipeline
                    InputAssemblyState = new MgPipelineInputAssemblyStateCreateInfo
                    {
                        // Input assembly state describes how primitives are assembled
                        // This pipeline will assemble vertex data as a triangle lists (though we only use one triangle)
                        Topology = MgPrimitiveTopology.TRIANGLE_LIST,
                    },

                    // Rasterization state
                    RasterizationState = new MgPipelineRasterizationStateCreateInfo
                    {
                        PolygonMode = MgPolygonMode.FILL,
                        CullMode = MgCullModeFlagBits.NONE,
                        FrontFace = MgFrontFace.COUNTER_CLOCKWISE,
                        DepthClampEnable = false,
                        RasterizerDiscardEnable = false,
                        DepthBiasEnable = false,
                        LineWidth = 1.0f,
                    },


                    // Color blend state describes how blend factors are calculated (if used)
                    // We need one blend attachment state per color attachment (even if blending is not used
                    ColorBlendState = new MgPipelineColorBlendStateCreateInfo
                    {
                        Attachments = new MgPipelineColorBlendAttachmentState[]
                        {
                        new MgPipelineColorBlendAttachmentState
                        {
                            ColorWriteMask =  MgColorComponentFlagBits.R_BIT | MgColorComponentFlagBits.G_BIT | MgColorComponentFlagBits.B_BIT | MgColorComponentFlagBits.A_BIT,
                            BlendEnable = false,
                        }
                        },
                    },

                    // Multi sampling state
                    // This example does not make use fo multi sampling (for anti-aliasing), the state must still be set and passed to the pipeline
                    MultisampleState = new MgPipelineMultisampleStateCreateInfo
                    {
                        RasterizationSamples = MgSampleCountFlagBits.COUNT_1_BIT,
                        SampleMask = null,
                    },


                    // The layout used for this pipeline (can be shared among multiple pipelines using the same layout)
                    Layout = mPipelineLayout,

                    // Renderpass this pipeline is attached to
                    RenderPass = mGraphicsDevice.Renderpass,

                    // Viewport state sets the number of viewports and scissor used in this pipeline
                    // Note: This is actually overriden by the dynamic states (see below)
                    ViewportState = null,

                    // Depth and stencil state containing depth and stencil compare and test operations
                    // We only use depth tests and want depth tests and writes to be enabled and compare with less or equal
                    DepthStencilState = new MgPipelineDepthStencilStateCreateInfo
                    {
                        DepthTestEnable = true,
                        DepthWriteEnable = true,
                        DepthCompareOp = MgCompareOp.LESS_OR_EQUAL,
                        DepthBoundsTestEnable = false,
                        Back = new MgStencilOpState
                        {
                            FailOp = MgStencilOp.KEEP,
                            PassOp = MgStencilOp.KEEP,
                            CompareOp = MgCompareOp.ALWAYS,
                        },
                        StencilTestEnable = false,
                        Front = new MgStencilOpState
                        {
                            FailOp = MgStencilOp.KEEP,
                            PassOp = MgStencilOp.KEEP,
                            CompareOp = MgCompareOp.ALWAYS,
                        },
                    },

                    // Enable dynamic states
                    // Most states are baked into the pipeline, but there are still a few dynamic states that can be changed within a command buffer
                    // To be able to change these we need do specify which dynamic states will be changed using this pipeline. Their actual states are set later on in the command buffer.
                    // For this example we will set the viewport and scissor using dynamic states

                    DynamicState = new MgPipelineDynamicStateCreateInfo
                    {
                        DynamicStates = new[]
                        {
                            MgDynamicState.VIEWPORT,
                            MgDynamicState.SCISSOR,
                        }
                    },
                };

                IMgPipeline[] pipelines;
                // Create rendering pipeline using the specified states
                var err = mConfiguration.Device.CreateGraphicsPipelines(null, new[] { pipelineCreateInfo }, null, out pipelines);
                Debug.Assert(err == Result.SUCCESS);

                vsModule.DestroyShaderModule(mConfiguration.Device, null);
                fsModule.DestroyShaderModule(mConfiguration.Device, null);

                mPipeline = pipelines[0];
            }

        }

        void setupDescriptorPool()
        {
            // Create the global descriptor pool
            // All descriptors used in this example are allocated from this pool
            var descriptorPoolInfo = new MgDescriptorPoolCreateInfo
            {
                // We need to tell the API the number of max. requested descriptors per type
                PoolSizes = new MgDescriptorPoolSize[]
                {
                    new MgDescriptorPoolSize
                    {
                        // This example only uses one descriptor type (uniform buffer) and only requests one descriptor of this type
                        Type = MgDescriptorType.UNIFORM_BUFFER,
                        DescriptorCount = 1,
                    },
                    // For additional types you need to add new entries in the type count list
                    // E.g. for two combined image samplers :
                    // typeCounts[1].type = VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER;
                    // typeCounts[1].descriptorCount = 2;
                },
                // Set the max. number of descriptor sets that can be requested from this pool (requesting beyond this limit will result in an error)
                MaxSets = 1,
            };
            var err = mConfiguration.Device.CreateDescriptorPool(descriptorPoolInfo, null, out mDescriptorPool);
            Debug.Assert(err == Result.SUCCESS);
        }

        void setupDescriptorSet()
        {
            // Allocate a new descriptor set from the global descriptor pool
            var allocInfo = new MgDescriptorSetAllocateInfo
            {
                DescriptorPool = mDescriptorPool,
                DescriptorSetCount = 1,
                SetLayouts = new[] { mDescriptorSetLayout },
            };

            IMgDescriptorSet[] dSets;
            var err = mConfiguration.Device.AllocateDescriptorSets(allocInfo, out dSets);
            mDescriptorSet = dSets[0];

            Debug.Assert(err == Result.SUCCESS);

            // Update the descriptor set determining the shader binding points
            // For every binding point used in a shader there needs to be one
            // descriptor set matching that binding point
            mConfiguration.Device.UpdateDescriptorSets(
                new []
                {
                    // Binding 0 : Uniform buffer
                    new MgWriteDescriptorSet
                    {
                        DstSet = mDescriptorSet,
                        DescriptorCount = 1,
                        DescriptorType =  MgDescriptorType.UNIFORM_BUFFER,
                        BufferInfo = new MgDescriptorBufferInfo[]
                        {
                            uniformDataVS.descriptor,
                        },
                        // Binds this uniform buffer to binding point 0
                        DstBinding = 0,
                    },
                }, null);
        }

        IMgCommandBuffer[] drawCmdBuffers;

        void createCommandBuffers()
        {
            // Create one command buffer per frame buffer
            // in the swap chain
            // Command buffers store a reference to the
            // frame buffer inside their render pass info
            // so for static usage withouth having to rebuild
            // them each frame, we use one per frame buffer
            drawCmdBuffers = new IMgCommandBuffer[mGraphicsDevice.Framebuffers.Length];

            {
                var cmdBufAllocateInfo = new MgCommandBufferAllocateInfo
                {
                    CommandBufferCount = (uint)mGraphicsDevice.Framebuffers.Length,
                    CommandPool = mConfiguration.Partition.CommandPool,
                    Level = MgCommandBufferLevel.PRIMARY,
                };

                var err = mConfiguration.Device.AllocateCommandBuffers(cmdBufAllocateInfo, drawCmdBuffers);
                Debug.Assert(err == Result.SUCCESS);
            }

            // Command buffers for submitting present barriers
            {
                var cmdBufAllocateInfo = new MgCommandBufferAllocateInfo
                {
                    CommandBufferCount = 2,
                    CommandPool = mConfiguration.Partition.CommandPool,
                    Level = MgCommandBufferLevel.PRIMARY,
                };
  
                var presentBuffers = new IMgCommandBuffer[2];
                var err = mConfiguration.Device.AllocateCommandBuffers(cmdBufAllocateInfo, presentBuffers);
                Debug.Assert(err == Result.SUCCESS);

                // Pre present
                mPrePresentCmdBuffer = presentBuffers[0];

                // Post present
                mPostPresentCmdBuffer = presentBuffers[1];
            }
        }

        // Build separate command buffers for every framebuffer image
        // Unlike in OpenGL all rendering commands are recorded once into command buffers that are then resubmitted to the queue
        // This allows to generate work upfront and from multiple threads, one of the biggest advantages of Vulkan
        void buildCommandBuffers()
        {
            var renderPassBeginInfo = new MgRenderPassBeginInfo {
                RenderPass = mGraphicsDevice.Renderpass,
                RenderArea = new MgRect2D
                {
                    Offset = new MgOffset2D {  X = 0, Y = 0 },
                    Extent = new MgExtent2D { Width = mWidth, Height = mHeight },
                },
                // Set clear values for all framebuffer attachments with loadOp set to clear
                // We use two attachments (color and depth) that are cleared at the start of the subpass and as such we need to set clear values for both
                ClearValues = new MgClearValue[]
                {
                    MgClearValue.FromColorAndFormat(mSwapchains.Format, new MgColor4f(0f, 0f, 0f, 0f)),                    
                    new MgClearValue { DepthStencil = new MgClearDepthStencilValue( 1.0f, 0) },
                },
            };
            
            for (var i = 0; i < drawCmdBuffers.Length; ++i)
            {
                // Set target frame buffer
                renderPassBeginInfo.Framebuffer = mGraphicsDevice.Framebuffers[i];

                var cmdBuf = drawCmdBuffers[i];

                var cmdBufInfo = new MgCommandBufferBeginInfo { };
                var err = cmdBuf.BeginCommandBuffer(cmdBufInfo);
                Debug.Assert(err == Result.SUCCESS);

                // Start the first sub pass specified in our default render pass setup by the base class
                // This will clear the color and depth attachment
                cmdBuf.CmdBeginRenderPass(renderPassBeginInfo, MgSubpassContents.INLINE);

                // Update dynamic viewport state

                cmdBuf.CmdSetViewport(0, 
                    new[] {
                        new MgViewport {
                            Height = (float) mHeight,
                            Width = (float) mWidth,
                            MinDepth = 0.0f,
                            MaxDepth = 1.0f,
                        }
                    }
                );

                // Update dynamic scissor state
                cmdBuf.CmdSetScissor(0,
                    new[] {
                        new MgRect2D {
                            Extent = new MgExtent2D { Width = mWidth, Height = mHeight },
                            Offset = new MgOffset2D { X = 0, Y = 0 },
                        }
                    }
                );

                // Bind descriptor sets describing shader binding points
                cmdBuf.CmdBindDescriptorSets( MgPipelineBindPoint.GRAPHICS, mPipelineLayout, 0, 1, new[] { mDescriptorSet }, null);

                // Bind the rendering pipeline
                // The pipeline (state object) contains all states of the rendering pipeline, binding it will set all the states specified at pipeline creation time
                cmdBuf.CmdBindPipeline(MgPipelineBindPoint.GRAPHICS, mPipeline);

                // Bind triangle vertex buffer (contains position and colors)
                cmdBuf.CmdBindVertexBuffers(0, new[] { vertices.buffer }, new [] { 0UL });

                // Bind triangle index buffer
                cmdBuf.CmdBindIndexBuffer(indices.buffer, 0, MgIndexType.UINT32);

                // Draw indexed triangle
                cmdBuf.CmdDrawIndexed(indices.count, 1, 0, 0, 1);

                cmdBuf.CmdEndRenderPass();

                // Ending the render pass will add an implicit barrier transitioning the frame buffer color attachment to 
                // VK_IMAGE_LAYOUT_PRESENT_SRC_KHR for presenting it to the windowing system

                err = cmdBuf.EndCommandBuffer();
                Debug.Assert(err == Result.SUCCESS);
            }
        }

        #endregion

        /// <summary>
        /// Convert degrees to radians
        /// </summary>
        /// <param name="degrees">An angle in degrees</param>
        /// <returns>The angle expressed in radians</returns>
        public static float DegreesToRadians(float degrees)
        {
            const double degToRad = System.Math.PI / 180.0;
            return (float) (degrees * degToRad);
        }

        void updateUniformBuffers()
        {
            // Update matrices
            uboVS.projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                DegreesToRadians(60.0f), 
                (mWidth / mHeight), 
                1.0f,
                256.0f);

            const float ZOOM = -2.5f;

            uboVS.viewMatrix = Matrix4.CreateTranslation(0, 0, ZOOM);

            // TODO : track down rotation
            uboVS.modelMatrix = Matrix4.Identity;
            //uboVS.modelMatrix = glm::rotate(uboVS.modelMatrix, glm::radians(rotation.x), glm::vec3(1.0f, 0.0f, 0.0f));
            //uboVS.modelMatrix = glm::rotate(uboVS.modelMatrix, glm::radians(rotation.y), glm::vec3(0.0f, 1.0f, 0.0f));
            //uboVS.modelMatrix = glm::rotate(uboVS.modelMatrix, glm::radians(rotation.z), glm::vec3(0.0f, 0.0f, 1.0f));


            var structSize = (ulong) Marshal.SizeOf(typeof(UniformBufferObject));

            // Map uniform buffer and update it
            IntPtr pData;

            var err = uniformDataVS.memory.MapMemory(mConfiguration.Device,  0, structSize, 0, out pData);

            Marshal.StructureToPtr(uboVS, pData, false);
            // Unmap after data has been copied
            // Note: Since we requested a host coherent memory type for the uniform buffer, the write is instantly visible to the GPU
            uniformDataVS.memory.UnmapMemory(mConfiguration.Device);
        }

        public void RenderLoop()
        {
            render();
        }

        void render()
        {
            if (!mPrepared)
                return;
            draw();
        }

        void draw()
        {
            // Get next image in the swap chain (back/front buffer)
            var currentBufferIndex = mPresentationLayer.BeginDraw(mPostPresentCmdBuffer, mPresentCompleteSemaphore);

            // Use a fence to wait until the command buffer has finished execution before using it again
            var fence = mWaitFences[(int) currentBufferIndex];
            var err = mConfiguration.Device.WaitForFences(new[] { fence } , true, ulong.MaxValue);
            Debug.Assert(err == Result.SUCCESS);

            err = mConfiguration.Device.ResetFences(new[] { fence });

            // Pipeline stage at which the queue submission will wait (via pWaitSemaphores)
            var submitInfos = new MgSubmitInfo[]
            {
                // The submit info structure specifices a command buffer queue submission batch
                new MgSubmitInfo
                {
                    WaitSemaphores = new []
                    {
                        // One wait semaphore
                        new MgSubmitInfoWaitSemaphoreInfo
                        {
                             // Pointer to the list of pipeline stages that the semaphore waits will occur at
                            WaitDstStageMask =  MgPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                            // Semaphore(s) to wait upon before the submitted command buffer starts executing
                            WaitSemaphore = mPresentCompleteSemaphore,
                        }
                    },
                     // One command buffer
                    CommandBuffers = new []
                    {
                        // Command buffers(s) to execute in this batch (submission)
                        drawCmdBuffers[currentBufferIndex]
                    },
                    // One signal semaphore
                    SignalSemaphores = new []
                    {
                        // Semaphore(s) to be signaled when command buffers have completed
                        mRenderCompleteSemaphore
                    },                    
                }
            };                                        

            // Submit to the graphics queue passing a wait fence
            err = mConfiguration.Queue.QueueSubmit(submitInfos, fence);
            Debug.Assert(err == Result.SUCCESS);

            // Present the current buffer to the swap chain
            // Pass the semaphore signaled by the command buffer submission from the submit info as the wait semaphore for swap chain presentation
            // This ensures that the image is not presented to the windowing system until all commands have been submitted

            mPresentationLayer.EndDraw(new[] { currentBufferIndex }, mPrePresentCmdBuffer, new[] { mRenderCompleteSemaphore });
        }

        void viewChanged()
        {
            // This function is called by the base example class each time the view is changed by user input
            updateUniformBuffers();
        }

        #region IDisposable Support
        private bool mIsDisposed = false; // To detect redundant calls
        private ITriangleDemoShaderPath mTrianglePath;

        protected virtual void Dispose(bool disposing)
        {
            if (mIsDisposed)
            {
                return;
            }

            ReleaseUnmanagedResources();

            if (disposing)
            {  
                ReleaseManagedResources();
            }

            mIsDisposed = true;            
        }

        private void ReleaseManagedResources()
        {
           
        }

        private void ReleaseUnmanagedResources()
        {
            var device = mConfiguration.Device;
            if (device != null)
            {

                // Clean up used Vulkan resources 
                // Note: Inherited destructor cleans up resources stored in base class
                if (mPipeline != null)
                    mPipeline.DestroyPipeline(device, null);

                if (mPipelineLayout != null)
                    mPipelineLayout.DestroyPipelineLayout(device, null);

                if (mDescriptorSetLayout != null)
                    mDescriptorSetLayout.DestroyDescriptorSetLayout(device, null);

                if (vertices.buffer != null)
                    vertices.buffer.DestroyBuffer(device, null);

                if (vertices.memory != null)
                    vertices.memory.FreeMemory(device, null);

                if (indices.buffer != null)
                    indices.buffer.DestroyBuffer(device, null);

                if (indices.memory != null)
                    indices.memory.FreeMemory(device, null);

                if (uniformDataVS.buffer != null)
                    uniformDataVS.buffer.DestroyBuffer(device, null);

                if (uniformDataVS.memory != null)
                    uniformDataVS.memory.FreeMemory(device, null);

                if (mPresentCompleteSemaphore != null)
                    mPresentCompleteSemaphore.DestroySemaphore(device, null);


                if (mRenderCompleteSemaphore != null)
                    mRenderCompleteSemaphore.DestroySemaphore(device, null);

                foreach (var fence in mWaitFences)
                {
                    fence.DestroyFence(device, null);
                }

                if (mDescriptorPool != null)
                    mDescriptorPool.DestroyDescriptorPool(device, null);

                if (drawCmdBuffers != null)
                    mConfiguration.Device.FreeCommandBuffers(mConfiguration.Partition.CommandPool, drawCmdBuffers);

                if (mPostPresentCmdBuffer != null)
                    mConfiguration.Device.FreeCommandBuffers(mConfiguration.Partition.CommandPool, new[] { mPostPresentCmdBuffer });


                if (mPrePresentCmdBuffer != null)
                    mConfiguration.Device.FreeCommandBuffers(mConfiguration.Partition.CommandPool, new[] { mPrePresentCmdBuffer });

                if (mGraphicsDevice != null)
                    mGraphicsDevice.Dispose();
            }
        }

        ~VulkanExample()
        {
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}