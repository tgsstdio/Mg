using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using AppKit;
using Foundation;
using Magnesium;
using Magnesium.Metal;
using Metal;
using MetalKit;
using ModelIO;
using OpenTK;
using SimpleInjector;

namespace MetalSample
{
	public partial class GameViewController : NSViewController, IMTKViewDelegate
	{
		[StructLayout(LayoutKind.Sequential)]
		struct Uniforms
		{
			public Matrix4 ModelviewProjectionMatrix;
			public Matrix4 NormalMatrix;
		}

		// The max number of command buffers in flight
		const int MaxInflightBuffers = 3;

		// Max API memory buffer size.
		const int MaxBytesPerFrame = 1024 * 1024;

		// view
		MTKView mApplicationView;

		// controller
		//Semaphore inflightSemaphore;
		int constantDataBufferIndex;

		// renderer
		//IMTLDevice device;
		//IMTLCommandQueue commandQueue;
		//IMTLRenderPipelineState pipelineState;
		//IMTLDepthStencilState depthState;

		// uniforms
		Matrix4 projectionMatrix;
		Matrix4 viewMatrix;
		float rotation;

		// meshes

		private Container mContainer;

		MgGraphicsConfiguration mGraphicsConfiguration;

		IMgPresentationLayer mPresentationLayer;

		//private Magnesium.MgDriver mDriver;

		//private Magnesium.IMgLogicalDevice mLogicalDevice;

		//IMgThreadPartition mDefaultPartition;
		private bool InitialiseMg()
		{
			try
			{
				var localDevice = MTLDevice.SystemDefault;

				if (localDevice == null)
				{
					Console.WriteLine("Metal is not supported on this device");
					return false;
				}


				// METAL SPECIFIC
				mContainer.RegisterSingleton<IMTLDevice>(localDevice);

				mApplicationView = (MTKView)View;

				mContainer.RegisterSingleton<MTKView>(mApplicationView);


				// MAGNESIUM 
				mContainer.Register<Magnesium.MgDriver>(Lifestyle.Singleton);

				var deviceQuery = new AmtDeviceQuery { NoOfCommandBufferSlots = 5 };
				mContainer.RegisterSingleton<Magnesium.Metal.IAmtDeviceQuery>(deviceQuery);
				mContainer.Register<Magnesium.Metal.IAmtMetalLibraryLoader,
						Magnesium.Metal.AmtMetalTextSourceLibraryLoader>(Lifestyle.Singleton);

				mContainer.Register<Magnesium.IMgEntrypoint, Magnesium.Metal.AmtEntrypoint>(
					Lifestyle.Singleton);
				mContainer.Register<Magnesium.IMgPresentationSurface, Magnesium.Metal.AmtPresentationSurface>(
					Lifestyle.Singleton);
				mContainer.Register<Magnesium.IMgSwapchainCollection, Magnesium.Metal.AmtSwapchainCollection>(
					Lifestyle.Singleton);
				mContainer.Register<Magnesium.IMgPresentationBarrierEntrypoint,
					Magnesium.Metal.AmtPresentationBarrierEntrypoint>(Lifestyle.Singleton);

				mContainer.Register<MgGraphicsConfiguration>(Lifestyle.Singleton);

				mGraphicsConfiguration = mContainer.GetInstance<MgGraphicsConfiguration>();

				SetupMagnesium();

				mApplicationView.Delegate = this;
				mApplicationView.Device = localDevice;

				InitializeMesh();
				InitializeUniforms();
				InitializeRenderCommandBuffers();
				InitializeGraphicsPipeline();

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public GameViewController(IntPtr handle) : base(handle)
		{
			
		}

		IMgCommandBuffer mPrePresentBarrierCmd;
		IMgCommandBuffer mPostPresentBarrierCmd;
		IMgCommandBuffer[] mPresentingCmdBuffers;

		~GameViewController()
		{
			if (mRenderCmdBuffers != null)
			{
				mGraphicsConfiguration.Device.FreeCommandBuffers(
					mGraphicsConfiguration.Partition.CommandPool, mRenderCmdBuffers);
			}

			if (mPresentingCmdBuffers != null)
			{
				mGraphicsConfiguration.Device.FreeCommandBuffers(
					mGraphicsConfiguration.Partition.CommandPool, mPresentingCmdBuffers);
			}

			if (mSwapchainCollection != null)
				mSwapchainCollection.Dispose();

			if (mGraphicsDevice != null)
				mGraphicsDevice.Dispose();
			
			if (mContainer != null)
				mContainer.Dispose();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			constantDataBufferIndex = 0;
			//inflightSemaphore = new Semaphore(MaxInflightBuffers, MaxInflightBuffers);

			mContainer = new Container();

			//SetupMagnesium();
			if (InitialiseMg())
			{
				//
				Reshape();
			}
			else {
				//Console.WriteLine("Metal is not supported on this device");
				View = new NSView(View.Frame);
			}
		}

		IMgGraphicsDevice mGraphicsDevice;

		IMgSwapchainCollection mSwapchainCollection;

		void SetupMagnesium()
		{
			try
			{
				mGraphicsDevice = new AmtGraphicsDevice(mApplicationView, mGraphicsConfiguration.LogicalDevice);
				mSwapchainCollection = mContainer.GetInstance<Magnesium.IMgSwapchainCollection>();
							
				var setupCommands = new Magnesium.IMgCommandBuffer[1];
				var pAllocateInfo = new Magnesium.MgCommandBufferAllocateInfo
				{
					CommandPool = mGraphicsConfiguration.Partition.CommandPool,
					CommandBufferCount = 1,
					Level = Magnesium.MgCommandBufferLevel.PRIMARY,
				};

				var err = mGraphicsConfiguration.Device.AllocateCommandBuffers(pAllocateInfo, setupCommands);
				Debug.Assert(err == Magnesium.Result.SUCCESS);

				var dsCreateInfo = new Magnesium.MgGraphicsDeviceCreateInfo
				{
					Color = Magnesium.MgFormat.B8G8R8A8_UNORM,
					DepthStencil = Magnesium.MgFormat.D32_SFLOAT_S8_UINT,
					Samples = Magnesium.MgSampleCountFlagBits.COUNT_1_BIT,
					Width = 640,
					Height = 480,
				};

				mGraphicsDevice.Create(setupCommands[0], mSwapchainCollection, dsCreateInfo);

				var pSubmits = new MgSubmitInfo[]
				{
					new MgSubmitInfo
					{
						CommandBuffers = new IMgCommandBuffer[]
						{
							setupCommands[0],
						},
					}
				};
	
				mGraphicsConfiguration.Partition.Queue.QueueSubmit(pSubmits, null);
				mGraphicsConfiguration.Partition.Queue.QueueWaitIdle();

				mGraphicsConfiguration.Partition.Device.FreeCommandBuffers(
					mGraphicsConfiguration.Partition.CommandPool, setupCommands);

				var barriers = mContainer.GetInstance<IMgPresentationBarrierEntrypoint>();

				mPresentationLayer = new MgPresentationLayer(mGraphicsConfiguration.Partition,
															 mSwapchainCollection,
															 barriers);

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public void DrawableSizeWillChange(MTKView view, CoreGraphics.CGSize size)
		{
			Reshape();
		}

		public void Draw(MTKView view)
		{
			Render();
		}

		Vector3[] positionVboData = new Vector3[]{
			new Vector3(-1.0f, -1.0f,  1.0f),
			new Vector3(-0.333333f, -0.333333f, 0.333333f), // normal
			new Vector3( 1.0f, -1.0f,  1.0f),
			new Vector3(0.333333f, -0.333333f, 0.333333f), // normal
			new Vector3( 1.0f,  1.0f,  1.0f),
			new Vector3(0.333333f, 0.333333f, 0.333333f), // normal
			new Vector3(-1.0f,  1.0f,  1.0f),
			new Vector3(-0.333333f, 0.333333f, 0.333333f), // normal
			new Vector3(-1.0f, -1.0f, -1.0f),
			new Vector3(-0.333333f, -0.333333f, -0.333333f), // normal
			new Vector3( 1.0f, -1.0f, -1.0f),
			new Vector3(0.333333f, -0.333333f, -0.333333f), // normal
			new Vector3( 1.0f,  1.0f, -1.0f),
			new Vector3(0.333333f, 0.333333f, -0.333333f), // normal
			new Vector3(-1.0f,  1.0f, -1.0f),
			new Vector3(-0.333333f, 0.333333f, -0.333333f), // normal
		};

		uint[] indicesVboData = new uint[]{
             // front face
                0, 1, 2, 2, 3, 0,
                // top face
                3, 2, 6, 6, 7, 3,
                // back face
                7, 6, 5, 5, 4, 7,
                // left face
                4, 0, 3, 3, 7, 4,
                // bottom face
                0, 1, 5, 5, 4, 0,
                // right face
                1, 5, 6, 6, 2, 1, };

		class BufferInfo
		{
			public IMgBuffer Buffer { get; set;}
			public IMgDeviceMemory DeviceMemory { get; set;}
			public ulong Offset { get; set;}
			public ulong Length { get; set;}
		}

		private BufferInfo mVertices; 
		private BufferInfo mIndices;
		private BufferInfo mUniforms;

		void InitializeMesh()
		{
			// Generate meshes
			// Mg : Buffer

			var indicesInBytes = (ulong)(sizeof(uint) * indicesVboData.Length);
			var vertexStride = Marshal.SizeOf(typeof(Vector3));
			var verticesInBytes = (ulong)(vertexStride * positionVboData.Length);
			var bufferSize = indicesInBytes + verticesInBytes;

			var bufferCreateInfo = new MgBufferCreateInfo
			{
				Usage = MgBufferUsageFlagBits.INDEX_BUFFER_BIT | MgBufferUsageFlagBits.VERTEX_BUFFER_BIT,
				Size = bufferSize,
			};

			IMgBuffer buffer;

			var device = mGraphicsConfiguration.Device;

			var result = device.CreateBuffer(bufferCreateInfo, null, out buffer);
			Debug.Assert(result == Result.SUCCESS);

			MgMemoryRequirements memReqs;
			device.GetBufferMemoryRequirements(buffer, out memReqs);

			const MgMemoryPropertyFlagBits memoryPropertyFlags = MgMemoryPropertyFlagBits.HOST_COHERENT_BIT;

			uint memoryTypeIndex;
			mGraphicsConfiguration.Partition.GetMemoryType(
				memReqs.MemoryTypeBits, memoryPropertyFlags, out memoryTypeIndex);

			var memAlloc = new MgMemoryAllocateInfo
			{
				MemoryTypeIndex = memoryTypeIndex,
				AllocationSize = memReqs.Size,
			};

			IMgDeviceMemory deviceMemory;
			result = device.AllocateMemory(memAlloc, null, out deviceMemory);
			Debug.Assert(result == Result.SUCCESS);

			buffer.BindBufferMemory(device, deviceMemory, 0);

			// COPY INDEX DATA
			IntPtr dest;
			result = deviceMemory.MapMemory(device, 0, bufferSize, 0, out dest);
			Debug.Assert(result == Result.SUCCESS);

			var tempIndices = new byte[indicesInBytes];
			Buffer.BlockCopy(indicesVboData, 0, tempIndices, 0, (int)indicesInBytes);

			Marshal.Copy(tempIndices, 0, dest, (int)indicesInBytes);

			// COPY VERTEX DATA

			var vertexOffset = indicesInBytes;

			// Copy the struct to unmanaged memory.	
			int offset = (int)vertexOffset;
			for (int i = 0; i < positionVboData.Length; ++i)
			{
				IntPtr localDest = IntPtr.Add(dest, offset);
				Marshal.StructureToPtr(positionVboData[i], localDest, false);
				offset += vertexStride;
			}

			deviceMemory.UnmapMemory(device);

			mIndices = new BufferInfo
			{
				Buffer = buffer,
				DeviceMemory = deviceMemory,
				Offset = 0,
				Length = indicesInBytes,
			};

			mVertices = new BufferInfo
			{
				Buffer = buffer,
				DeviceMemory = deviceMemory,
				Offset = indicesInBytes,
				Length = verticesInBytes,
			};

			//MDLMesh mdl = MDLMesh.CreateBox(new Vector3(2f, 2f, 2f), new Vector3i(1, 1, 1), MDLGeometryType.Triangles, false, new MTKMeshBufferAllocator(device));

			//NSError error;
			//boxMesh = new MTKMesh(mdl, device, out error);
		}

		IMgDescriptorSetLayout mSetLayout;

		// Allocate one region of memory for the uniform buffer
		private void InitializeUniforms()
		{
			MgBufferCreateInfo pCreateInfo = new MgBufferCreateInfo
			{
				Usage = MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT,
				Size = MaxBytesPerFrame,
			};
			IMgBuffer buffer;
			var err = mGraphicsConfiguration.Device.CreateBuffer(pCreateInfo, null, out buffer);
			Debug.Assert(err == Result.SUCCESS);
			//dynamicConstantBuffer = device.CreateBuffer(MaxBytesPerFrame, (MTLResourceOptions)0);
			//dynamicConstantBuffer.Label = "UniformBuffer";

			MgMemoryRequirements uniformsMemReqs;
			mGraphicsConfiguration.Device.GetBufferMemoryRequirements(buffer, out uniformsMemReqs);

			const MgMemoryPropertyFlagBits uniformPropertyFlags = MgMemoryPropertyFlagBits.HOST_COHERENT_BIT;

			uint uniformMemoryTypeIndex;
			mGraphicsConfiguration.Partition.GetMemoryType(
				uniformsMemReqs.MemoryTypeBits, uniformPropertyFlags, out uniformMemoryTypeIndex);

			var uniformMemAlloc = new MgMemoryAllocateInfo
			{
				MemoryTypeIndex = uniformMemoryTypeIndex,
				AllocationSize = uniformsMemReqs.Size,
			};

			IMgDeviceMemory deviceMemory;
			var result = mGraphicsConfiguration.Device.AllocateMemory(uniformMemAlloc, null, out deviceMemory);
			Debug.Assert(result == Result.SUCCESS);

			buffer.BindBufferMemory(mGraphicsConfiguration.Device, deviceMemory, 0);

			mUniforms = new BufferInfo
			{
				Buffer = buffer,
				DeviceMemory = deviceMemory,
				Offset = 0,
				Length = MaxBytesPerFrame,
			};

			IMgDescriptorSetLayout pSetLayout;
			var dslCreateInfo = new MgDescriptorSetLayoutCreateInfo
			{
				Bindings = new MgDescriptorSetLayoutBinding[]
				{
					new MgDescriptorSetLayoutBinding
					{
						Binding = 0,
						DescriptorCount = 1,
						DescriptorType = MgDescriptorType.UNIFORM_BUFFER_DYNAMIC,
						StageFlags = MgShaderStageFlagBits.VERTEX_BIT,
					},
				},
			};
			err = mGraphicsConfiguration.Device.CreateDescriptorSetLayout(dslCreateInfo, null, out pSetLayout);

			IMgDescriptorSet[] dSets = new IMgDescriptorSet[] { };
			MgDescriptorSetAllocateInfo pAllocateInfo = new MgDescriptorSetAllocateInfo
			{
				DescriptorPool = mGraphicsConfiguration.Partition.DescriptorPool,
				DescriptorSetCount = 1,
				SetLayouts = new IMgDescriptorSetLayout[]
				{
					pSetLayout,
				},
			};
			mGraphicsConfiguration.Device.AllocateDescriptorSets(pAllocateInfo, out dSets);
			mUniformDescriptorSet = dSets[0];

			MgWriteDescriptorSet[] writes = new MgWriteDescriptorSet[]
			{
				new MgWriteDescriptorSet
				{
					DescriptorCount = 1,
					DescriptorType = MgDescriptorType.UNIFORM_BUFFER_DYNAMIC,
					DstSet = mUniformDescriptorSet,
					BufferInfo = new MgDescriptorBufferInfo[]
					{
						new MgDescriptorBufferInfo
						{
							Buffer = mUniforms.Buffer,
							Offset = mUniforms.Offset,
							Range = mUniforms.Length,
						},
					},
					DstBinding = 0,
				}
			};
			mGraphicsConfiguration.Device.UpdateDescriptorSets(writes, null);
		

			mSetLayout = pSetLayout;
		}

		private void InitializeRenderCommandBuffers()
		{
			const uint MAX_NO_OF_PRESENT_BUFFERS = 2;
			mPresentingCmdBuffers = new Magnesium.IMgCommandBuffer[MAX_NO_OF_PRESENT_BUFFERS];
			var pAllocateInfo = new Magnesium.MgCommandBufferAllocateInfo
			{
				CommandPool = mGraphicsConfiguration.Partition.CommandPool,
				CommandBufferCount = MAX_NO_OF_PRESENT_BUFFERS,
				Level = Magnesium.MgCommandBufferLevel.PRIMARY,
			};

			var err = mGraphicsConfiguration.Device.AllocateCommandBuffers(pAllocateInfo, mPresentingCmdBuffers);
			Debug.Assert(err == Result.SUCCESS);
			mPrePresentBarrierCmd = mPresentingCmdBuffers[0];
			mPostPresentBarrierCmd = mPresentingCmdBuffers[1];
		}

		IMgPipelineLayout mPipelineLayout;

		void InitializeGraphicsPipeline()
		{
			using (var shaderSrc = System.IO.File.OpenRead("Fragment.metal"))
			using (var fragMemory = new System.IO.MemoryStream())
			using (var vertMemory = new System.IO.MemoryStream())
			{
				shaderSrc.CopyTo(fragMemory);
				// Magnesium uses like open files i.e. open the stream and let it process the data
				fragMemory.Seek(0, System.IO.SeekOrigin.Begin);

				// Load the fragment program into the library
				IMgShaderModule fragmentProgram = null;

				//IMTLFunction fragmentProgram = defaultLibrary.CreateFunction("lighting_fragment");
				MgShaderModuleCreateInfo fragCreateInfo = new MgShaderModuleCreateInfo
				{
					Code = fragMemory,
					CodeSize = new UIntPtr((ulong)fragMemory.Length),
				};
				var err = mGraphicsConfiguration.Device.CreateShaderModule(fragCreateInfo, null, out fragmentProgram);
				Debug.Assert(err == Result.SUCCESS);

				// REWIND AFTER USE
				shaderSrc.Seek(0, System.IO.SeekOrigin.Begin);
				shaderSrc.CopyTo(vertMemory);

				vertMemory.Seek(0, System.IO.SeekOrigin.Begin);

				// Load the vertex program into the library
				//IMTLFunction vertexProgram = defaultLibrary.CreateFunction("lighting_vertex");
				MgShaderModuleCreateInfo vertCreateInfo = new MgShaderModuleCreateInfo
				{
					Code = vertMemory,
					CodeSize = new UIntPtr((ulong) vertMemory.Length),
				};

				IMgShaderModule vertexProgram = null;
				err = mGraphicsConfiguration.Device.CreateShaderModule(vertCreateInfo, null, out vertexProgram);
				Debug.Assert(err == Result.SUCCESS);

				IMgPipelineLayout pipelineLayout;
				MgPipelineLayoutCreateInfo plCreateInfo = new MgPipelineLayoutCreateInfo
				{
					SetLayouts = new IMgDescriptorSetLayout[]
					{
						mSetLayout,
					},
				};
				err = mGraphicsConfiguration.Device.CreatePipelineLayout(plCreateInfo, null, out pipelineLayout);
				mPipelineLayout = pipelineLayout;

				var vertexStride = Marshal.SizeOf<Vector3>();


				IMgPipeline[] pipelines;
				var pCreateInfos = new MgGraphicsPipelineCreateInfo[]
				{
					new MgGraphicsPipelineCreateInfo
					{
						Stages = new MgPipelineShaderStageCreateInfo[]
						{
							new MgPipelineShaderStageCreateInfo
							{
								Module = fragmentProgram,
								Stage = MgShaderStageFlagBits.FRAGMENT_BIT,
								Name = "lighting_fragment",
							},
							new MgPipelineShaderStageCreateInfo
							{
								Module = vertexProgram,
								Stage = MgShaderStageFlagBits.VERTEX_BIT,
								Name = "lighting_vertex",
							},
						},
						InputAssemblyState = new MgPipelineInputAssemblyStateCreateInfo
						{
							Topology = MgPrimitiveTopology.TRIANGLE_LIST,
						},
						DepthStencilState = new MgPipelineDepthStencilStateCreateInfo
						{
							DepthCompareOp = MgCompareOp.LESS,
							DepthWriteEnable = true,
						},
						MultisampleState = new MgPipelineMultisampleStateCreateInfo
						{
							// TODO : METALSample uses 4 bits, have to investigated multisampling in 
							// Vulkan ( => Magnesium) for correct code
							RasterizationSamples = MgSampleCountFlagBits.COUNT_1_BIT,
						},
						RasterizationState = new MgPipelineRasterizationStateCreateInfo
						{
							FrontFace = MgFrontFace.COUNTER_CLOCKWISE,
							PolygonMode = MgPolygonMode.FILL,
						},
						VertexInputState = new MgPipelineVertexInputStateCreateInfo
						{
							VertexBindingDescriptions = new MgVertexInputBindingDescription[]
							{
								new MgVertexInputBindingDescription
								{
									Binding = 0,
									InputRate = MgVertexInputRate.VERTEX,
									Stride = (uint) (2 * vertexStride),
								},
							},
							VertexAttributeDescriptions = new MgVertexInputAttributeDescription[]
							{
								new MgVertexInputAttributeDescription
								{
									Binding = 0,
									Location = 0,
									Format = MgFormat.R32G32B32_SFLOAT,
									Offset = 0,
								},
								new MgVertexInputAttributeDescription
								{
									Binding = 0,
									Location = 1,
									Format = MgFormat.R32G32B32_SFLOAT,
									Offset = (uint) vertexStride,
								},
							},
								
						},
						ViewportState = new MgPipelineViewportStateCreateInfo
						{
							Viewports = new MgViewport[]
							{
								mGraphicsDevice.CurrentViewport,
							},
							Scissors = new MgRect2D[]
							{
								mGraphicsDevice.Scissor,
							}
						},
						//DynamicState = new MgPipelineDynamicStateCreateInfo
						//{
						//	DynamicStates = new MgDynamicState[]
						//	{
						//		MgDynamicState.VIEWPORT,
						//		MgDynamicState.SCISSOR,
						//	}
						//},
						RenderPass = mGraphicsDevice.Renderpass,
						Layout = pipelineLayout,
					},
				};
				err = mGraphicsConfiguration.Device.CreateGraphicsPipelines(
					null, pCreateInfos, null, out pipelines);
				Debug.Assert(err == Result.SUCCESS);

				fragmentProgram.DestroyShaderModule(mGraphicsConfiguration.Device, null);
				vertexProgram.DestroyShaderModule(mGraphicsConfiguration.Device, null);

				mPipelineState = pipelines[0];
            }

			GenerateRenderingCommandBuffers();
		}

		private IMgPipeline mPipelineState;

		IMgDescriptorSet mUniformDescriptorSet;

		void Render()
		{
			//inflightSemaphore.WaitOne();
			try
			{
				uint layerNo = mPresentationLayer.BeginDraw(mPostPresentBarrierCmd, null);

				Update();

				mGraphicsConfiguration.Queue.QueueSubmit(
					new[]
					{
					new MgSubmitInfo
					{
						CommandBuffers = new []
						{
							mRenderCmdBuffers[layerNo],
						}
					}
					},
					 null);

				mGraphicsConfiguration.Queue.QueueWaitIdle();

				//// The render assumes it can now increment the buffer index and that the previous index won't be touched
				///  until we cycle back around to the same index
				constantDataBufferIndex = (constantDataBufferIndex + 1) % MaxInflightBuffers;

				//// Finalize rendering here & push the command buffer to the GPU
				//commandBuffer.Commit();

				mPresentationLayer.EndDraw(new[] { layerNo }, mPrePresentBarrierCmd, null);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}


		IMgCommandBuffer[] mRenderCmdBuffers;
		void GenerateRenderingCommandBuffers()
		{
			var noOfFramebuffers = (uint) mGraphicsDevice.Framebuffers.Length;
			var uniformStride = Marshal.SizeOf(typeof(Uniforms));

			mRenderCmdBuffers = new Magnesium.IMgCommandBuffer[noOfFramebuffers];
			var pAllocateInfo = new Magnesium.MgCommandBufferAllocateInfo
			{
				CommandPool = mGraphicsConfiguration.Partition.CommandPool,
				CommandBufferCount = noOfFramebuffers,
				Level = Magnesium.MgCommandBufferLevel.PRIMARY,
			};

			var err = mGraphicsConfiguration.Device.AllocateCommandBuffers(pAllocateInfo, mRenderCmdBuffers);
			Debug.Assert(err == Result.SUCCESS);

			for (var i = 0; i < noOfFramebuffers; ++i)
			{
				var cmdBuf = mRenderCmdBuffers[i];
				var fb = mGraphicsDevice.Framebuffers[i];

				MgCommandBufferBeginInfo pBeginInfo = new MgCommandBufferBeginInfo
				{

				};
				// Create a new command buffer for each renderpass to the current drawable
				cmdBuf.BeginCommandBuffer(pBeginInfo);

				var pRenderPassBegin = new MgRenderPassBeginInfo
				{
					RenderPass = mGraphicsDevice.Renderpass,
					Framebuffer = fb,
					RenderArea = mGraphicsDevice.Scissor,
					ClearValues = new MgClearValue[]
					{
					MgClearValue.FromColorAndFormat(mSwapchainCollection.Format, new MgColor4f(0,0,0,0)),
					new MgClearValue{ DepthStencil = new MgClearDepthStencilValue{ Depth = 1f} },
					},
				};
				cmdBuf.CmdBeginRenderPass(pRenderPassBegin, MgSubpassContents.INLINE);

				cmdBuf.CmdBindPipeline(MgPipelineBindPoint.GRAPHICS, mPipelineState);
				cmdBuf.CmdBindVertexBuffers(
					0,
					new IMgBuffer[]
					{
					mVertices.Buffer,
					},
					new[]
					{
					mVertices.Offset,
					}
				);
				cmdBuf.CmdBindDescriptorSets(
					MgPipelineBindPoint.GRAPHICS,
					mPipelineLayout,
					0,
					1,
					new[]
					{
				mUniformDescriptorSet,
					},
					new[]
					{
					(uint) (constantDataBufferIndex * uniformStride),
					}
				);
				cmdBuf.CmdBindIndexBuffer(
					mIndices.Buffer,
					mIndices.Offset,
					MgIndexType.UINT32);

				cmdBuf.CmdDrawIndexed((uint)indicesVboData.Length, 1, 0, 0, 0);

				cmdBuf.CmdEndRenderPass();

				cmdBuf.EndCommandBuffer();
			}
		}

		void Update()
		{
			var baseModel = Matrix4.Mult(CreateMatrixFromTranslation(0f, 0f, 5f), CreateMatrixFromRotation(rotation, 0f, 1f, 0f));
			var baseMv = Matrix4.Mult(viewMatrix, baseModel);
			var modelViewMatrix = Matrix4.Mult(baseMv, CreateMatrixFromRotation(rotation, 1f, 1f, 1f));

			var uniforms = new Uniforms();
			uniforms.NormalMatrix = Matrix4.Invert(Matrix4.Transpose(modelViewMatrix));
			uniforms.ModelviewProjectionMatrix = Matrix4.Transpose(Matrix4.Mult(projectionMatrix, modelViewMatrix));

			int rawsize = Marshal.SizeOf<Uniforms>();
			IntPtr ptr;
			mUniforms.DeviceMemory.MapMemory(mGraphicsConfiguration.Device,
			                                (ulong) (rawsize * constantDataBufferIndex),
                 							(ulong) rawsize, 0, 
                							out ptr);
			Marshal.StructureToPtr(uniforms, ptr, false);
			mUniforms.DeviceMemory.UnmapMemory(mGraphicsConfiguration.Device);
			rotation += .01f;
		}

		void Reshape()
		{
			// When reshape is called, update the view and projection matricies since this means the view orientation or size changed
			var aspect = (float)(View.Bounds.Size.Width / View.Bounds.Size.Height);
			projectionMatrix = CreateMatrixFromPerspective(65f * ((float)Math.PI / 180f), aspect, .1f, 100f);

			viewMatrix = Matrix4.Identity;
		}

		public static Matrix4 CreateMatrixFromRotation(float radians, float x, float y, float z)
		{
			Vector3 v = Vector3.Normalize(new Vector3(x, y, z));
			var cos = (float)Math.Cos(radians);
			var sin = (float)Math.Sin(radians);
			float cosp = 1f - cos;

			var m = new Matrix4
			{
				Row0 = new Vector4(cos + cosp * v.X * v.X, cosp * v.X * v.Y - v.Z * sin, cosp * v.X * v.Z + v.Y * sin, 0f),
				Row1 = new Vector4(cosp * v.X * v.Y + v.Z * sin, cos + cosp * v.Y * v.Y, cosp * v.Y * v.Z - v.X * sin, 0f),
				Row2 = new Vector4(cosp * v.X * v.Z - v.Y * sin, cosp * v.Y * v.Z + v.X * sin, cos + cosp * v.Z * v.Z, 0f),
				Row3 = new Vector4(0f, 0f, 0f, 1f)
			};

			return m;
		}

		public static Matrix4 CreateMatrixFromTranslation(float x, float y, float z)
		{
			var m = Matrix4.Identity;
			m.Row0.W = x;
			m.Row1.W = y;
			m.Row2.W = z;
			m.Row3.W = 1f;
			return m;
		}

		public static Matrix4 CreateMatrixFromPerspective(float fovY, float aspect, float nearZ, float farZ)
		{
			float yscale = 1f / (float)Math.Tan(fovY * .5f);
			float xscale = yscale / aspect;
			float q = farZ / (farZ - nearZ);

			var m = new Matrix4
			{
				Row0 = new Vector4(xscale, 0f, 0f, 0f),
				Row1 = new Vector4(0f, yscale, 0f, 0f),
				Row2 = new Vector4(0f, 0f, q, q * -nearZ),
				Row3 = new Vector4(0f, 0f, 1f, 0f)
			};

			return m;
		}
	}
}

