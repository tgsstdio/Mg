using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Magnesium;
using OpenTK;

namespace MetalSample
{
	public class SpinningCube : IDisposable
	{
		readonly IMgGraphicsConfiguration mConfiguration;
		readonly IMgSwapchainCollection mSwapchains;
		readonly IMgGraphicsDevice mGraphicsDevice;
		readonly IMgPresentationLayer mPresentationLayer;

		uint mWidth;
		uint mHeight;

		// controller
		int constantDataBufferIndex = 0;

		// uniforms
		Matrix4 projectionMatrix;
		Matrix4 viewMatrix;
		float rotation;

		public SpinningCube(
			IMgGraphicsConfiguration configuration,
			IMgSwapchainCollection swapchains,
			IMgGraphicsDevice graphicsDevice,
			IMgPresentationLayer presentationLayer
		)
		{
			mConfiguration = configuration;
			mSwapchains = swapchains;
			mGraphicsDevice = graphicsDevice;
			mPresentationLayer = presentationLayer;

			mWidth = 640;
			mHeight = 480;

			mConfiguration.Initialize(mWidth, mHeight);
			SetupGraphicsDevice();

			InitializeMesh();
			InitializeUniforms();
			InitializeRenderCommandBuffers();
			InitializeGraphicsPipeline();
		}

		void SetupGraphicsDevice()
		{
			try
			{
				var setupCommands = new IMgCommandBuffer[1];
				var pAllocateInfo = new MgCommandBufferAllocateInfo
				{
					CommandPool = mConfiguration.Partition.CommandPool,
					CommandBufferCount = 1,
					Level = MgCommandBufferLevel.PRIMARY,
				};

				var err = mConfiguration.Device.AllocateCommandBuffers(pAllocateInfo, setupCommands);
				Debug.Assert(err == Result.SUCCESS);

				var dsCreateInfo = new MgGraphicsDeviceCreateInfo
				{
					//Color = MgFormat.B8G8R8A8_UNORM,
                    Color = MgColorFormatOption.AUTO_DETECT,
					// DepthStencil = MgFormat.D32_SFLOAT_S8_UINT,
                    DepthStencil = MgDepthFormatOption.AUTO_DETECT,

					Samples = MgSampleCountFlagBits.COUNT_1_BIT,
					Width = mWidth,
					Height = mHeight,
				};

				var cmdBuf = setupCommands[0];
				var cmdBufInfo = new MgCommandBufferBeginInfo {  };
				cmdBuf.BeginCommandBuffer(cmdBufInfo);
				mGraphicsDevice.Create(cmdBuf, mSwapchains, dsCreateInfo);
				cmdBuf.EndCommandBuffer();

				var pSubmits = new MgSubmitInfo[]
				{
					new MgSubmitInfo
					{
						CommandBuffers = new IMgCommandBuffer[]
						{
							cmdBuf,
						},
					}
				};

				mConfiguration.Partition.Queue.QueueSubmit(pSubmits, null);
				mConfiguration.Partition.Queue.QueueWaitIdle();

				mConfiguration.Partition.Device.FreeCommandBuffers(
					mConfiguration.Partition.CommandPool, setupCommands);

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw ex;
			}
		}

		Vector3[] positionVboData = new Vector3[]{
			new Vector3(-1.0f, 1.0f,  -1.0f),
			new Vector3(-0.333333f, 0.333333f, -0.333333f), // normal
			new Vector3( 1.0f, 1.0f,  -1.0f),
			new Vector3(0.333333f, 0.333333f, -0.333333f), // normal
			new Vector3( 1.0f, -1.0f,  -1.0f),
			new Vector3(0.333333f, -0.333333f, -0.333333f), // normal
			new Vector3(-1.0f, -1.0f,  -1.0f),
			new Vector3(-0.333333f, -0.333333f, -0.333333f), // normal
			new Vector3(-1.0f, 1.0f, 1.0f),
			new Vector3(-0.333333f, 0.333333f, 0.333333f), // normal
			new Vector3( 1.0f, 1.0f, 1.0f),
			new Vector3(0.333333f, 0.333333f, 0.333333f), // normal
			new Vector3( 1.0f, -1.0f, 1.0f),
			new Vector3(0.333333f, -0.333333f, 0.333333f), // normal
			new Vector3(-1.0f, -1.0f, 1.0f),
			new Vector3(-0.333333f, -0.333333f, 0.333333f), // normal
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
                0, 5, 1, 4, 5, 0,
                // right face
                1, 5, 6, 6, 2, 1, };

		class BufferInfo
		{
			public IMgBuffer Buffer { get; set; }
			public IMgDeviceMemory DeviceMemory { get; set; }
			public ulong Offset { get; set; }
			public ulong Length { get; set; }
		}

		private BufferInfo mVertices;
		private BufferInfo mIndices;
		private BufferInfo mUniforms;

		IMgBuffer mBuffer;
		IMgDeviceMemory mDeviceMemory;

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

			var device = mConfiguration.Device;

			var result = device.CreateBuffer(bufferCreateInfo, null, out mBuffer);
			Debug.Assert(result == Result.SUCCESS);

			MgMemoryRequirements memReqs;
			device.GetBufferMemoryRequirements(mBuffer, out memReqs);

			const MgMemoryPropertyFlagBits memoryPropertyFlags = MgMemoryPropertyFlagBits.HOST_COHERENT_BIT;

			uint memoryTypeIndex;
            mConfiguration.MemoryProperties.GetMemoryType(
				memReqs.MemoryTypeBits, memoryPropertyFlags, out memoryTypeIndex);

			var memAlloc = new MgMemoryAllocateInfo
			{
				MemoryTypeIndex = memoryTypeIndex,
				AllocationSize = memReqs.Size,
			};

			result = device.AllocateMemory(memAlloc, null, out mDeviceMemory);
			Debug.Assert(result == Result.SUCCESS);

			mBuffer.BindBufferMemory(device, mDeviceMemory, 0);

			// COPY INDEX DATA
			IntPtr dest;
			result = mDeviceMemory.MapMemory(device, 0, bufferSize, 0, out dest);
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

			mDeviceMemory.UnmapMemory(device);

			mIndices = new BufferInfo
			{
				Buffer = mBuffer,
				DeviceMemory = mDeviceMemory,
				Offset = 0,
				Length = indicesInBytes,
			};

			mVertices = new BufferInfo
			{
				Buffer = mBuffer,
				DeviceMemory = mDeviceMemory,
				Offset = indicesInBytes,
				Length = verticesInBytes,
			};
		}

		IMgDescriptorSetLayout mSetLayout;
		IMgDescriptorSet mUniformDescriptorSet;

		// The max number of command buffers in flight
		const int MaxInflightBuffers = 3;

		// Max API memory buffer size.
		const int MaxBytesPerFrame = 1024 * 1024;

		IMgDescriptorPool mDescriptorPool;

		// Allocate one region of memory for the uniform buffer
		private void InitializeUniforms()
		{
			MgBufferCreateInfo pCreateInfo = new MgBufferCreateInfo
			{
				Usage = MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT,
				Size = MaxBytesPerFrame,
			};
			IMgBuffer buffer;
			var err = mConfiguration.Device.CreateBuffer(pCreateInfo, null, out buffer);
			Debug.Assert(err == Result.SUCCESS);
			//dynamicConstantBuffer = device.CreateBuffer(MaxBytesPerFrame, (MTLResourceOptions)0);
			//dynamicConstantBuffer.Label = "UniformBuffer";

			MgMemoryRequirements uniformsMemReqs;
			mConfiguration.Device.GetBufferMemoryRequirements(buffer, out uniformsMemReqs);

			const MgMemoryPropertyFlagBits uniformPropertyFlags = MgMemoryPropertyFlagBits.HOST_COHERENT_BIT;

			uint uniformMemoryTypeIndex;
            mConfiguration.MemoryProperties.GetMemoryType(
				uniformsMemReqs.MemoryTypeBits, uniformPropertyFlags, out uniformMemoryTypeIndex);

			var uniformMemAlloc = new MgMemoryAllocateInfo
			{
				MemoryTypeIndex = uniformMemoryTypeIndex,
				AllocationSize = uniformsMemReqs.Size,
			};

			IMgDeviceMemory deviceMemory;
			var result = mConfiguration.Device.AllocateMemory(uniformMemAlloc, null, out deviceMemory);
			Debug.Assert(result == Result.SUCCESS);

			buffer.BindBufferMemory(mConfiguration.Device, deviceMemory, 0);

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
			err = mConfiguration.Device.CreateDescriptorSetLayout(dslCreateInfo, null, out pSetLayout);

			var poolCreateInfo = new Magnesium.MgDescriptorPoolCreateInfo
			{
				MaxSets = 1,
				PoolSizes = new MgDescriptorPoolSize[] {
									new MgDescriptorPoolSize
									{
										DescriptorCount = 1,
										Type =  MgDescriptorType.COMBINED_IMAGE_SAMPLER,
									},
							},
			};
			err = mConfiguration.Device.CreateDescriptorPool(poolCreateInfo, null, out mDescriptorPool);

			IMgDescriptorSet[] dSets;
			MgDescriptorSetAllocateInfo pAllocateInfo = new MgDescriptorSetAllocateInfo
			{
				DescriptorPool = mDescriptorPool,
				DescriptorSetCount = 1,
				SetLayouts = new IMgDescriptorSetLayout[]
				{
					pSetLayout,
				},
			};
			mConfiguration.Device.AllocateDescriptorSets(pAllocateInfo, out dSets);
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
			mConfiguration.Device.UpdateDescriptorSets(writes, null);


			mSetLayout = pSetLayout;
		}


		IMgCommandBuffer mPrePresentBarrierCmd;
		IMgCommandBuffer mPostPresentBarrierCmd;
		IMgCommandBuffer[] mPresentingCmdBuffers;

		private void InitializeRenderCommandBuffers()
		{
			const uint MAX_NO_OF_PRESENT_BUFFERS = 2;
			mPresentingCmdBuffers = new Magnesium.IMgCommandBuffer[MAX_NO_OF_PRESENT_BUFFERS];
			var pAllocateInfo = new Magnesium.MgCommandBufferAllocateInfo
			{
				CommandPool = mConfiguration.Partition.CommandPool,
				CommandBufferCount = MAX_NO_OF_PRESENT_BUFFERS,
				Level = Magnesium.MgCommandBufferLevel.PRIMARY,
			};

			var err = mConfiguration.Device.AllocateCommandBuffers(pAllocateInfo, mPresentingCmdBuffers);
			Debug.Assert(err == Result.SUCCESS);
			mPrePresentBarrierCmd = mPresentingCmdBuffers[0];
			mPostPresentBarrierCmd = mPresentingCmdBuffers[1];
		}

		IMgPipelineLayout mPipelineLayout;
		private IMgPipeline mPipelineState;

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
				var err = mConfiguration.Device.CreateShaderModule(fragCreateInfo, null, out fragmentProgram);
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
					CodeSize = new UIntPtr((ulong)vertMemory.Length),
				};

				IMgShaderModule vertexProgram = null;
				err = mConfiguration.Device.CreateShaderModule(vertCreateInfo, null, out vertexProgram);
				Debug.Assert(err == Result.SUCCESS);

				IMgPipelineLayout pipelineLayout;
				MgPipelineLayoutCreateInfo plCreateInfo = new MgPipelineLayoutCreateInfo
				{
					SetLayouts = new IMgDescriptorSetLayout[]
					{
						mSetLayout,
					},
				};
				err = mConfiguration.Device.CreatePipelineLayout(plCreateInfo, null, out pipelineLayout);
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
							DepthTestEnable = true,
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
							CullMode = MgCullModeFlagBits.BACK_BIT,
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
				err = mConfiguration.Device.CreateGraphicsPipelines(
					null, pCreateInfos, null, out pipelines);
				Debug.Assert(err == Result.SUCCESS);

				fragmentProgram.DestroyShaderModule(mConfiguration.Device, null);
				vertexProgram.DestroyShaderModule(mConfiguration.Device, null);

				mPipelineState = pipelines[0];
			}

			GenerateRenderingCommandBuffers();
		}

		[StructLayout(LayoutKind.Sequential)]
		struct Uniforms
		{
			public Matrix4 ModelviewProjectionMatrix;
			public Matrix4 NormalMatrix;
		}

		IMgCommandBuffer[] mRenderCmdBuffers;
		void GenerateRenderingCommandBuffers()
		{
			var noOfFramebuffers = (uint)mGraphicsDevice.Framebuffers.Length;
			var uniformStride = Marshal.SizeOf(typeof(Uniforms));

			mRenderCmdBuffers = new Magnesium.IMgCommandBuffer[noOfFramebuffers];
			var pAllocateInfo = new Magnesium.MgCommandBufferAllocateInfo
			{
				CommandPool = mConfiguration.Partition.CommandPool,
				CommandBufferCount = noOfFramebuffers,
				Level = Magnesium.MgCommandBufferLevel.PRIMARY,
			};

			var err = mConfiguration.Device.AllocateCommandBuffers(pAllocateInfo, mRenderCmdBuffers);
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
						MgClearValue.FromColorAndFormat(mSwapchains.Format, new MgColor4f(0.5f,0.5f,0.5f,0.5f)),
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
				//cmdBuf.CmdDrawIndexed(15, 1, 0, 0, 0);

				cmdBuf.CmdEndRenderPass();

				cmdBuf.EndCommandBuffer();
			}
		}

		public void Update()
		{
			var baseModel = Matrix4.Mult(CreateMatrixFromTranslation(0f, 0f, 5f), CreateMatrixFromRotation(rotation, 0f, 1f, 0f));
			var baseMv = Matrix4.Mult(viewMatrix, baseModel);
			var modelViewMatrix = Matrix4.Mult(baseMv, CreateMatrixFromRotation(rotation, 1f, 1f, 1f));

			var uniforms = new Uniforms();
			uniforms.NormalMatrix = Matrix4.Invert(Matrix4.Transpose(modelViewMatrix));
			uniforms.ModelviewProjectionMatrix = Matrix4.Transpose(Matrix4.Mult(projectionMatrix, modelViewMatrix));

			int rawsize = Marshal.SizeOf<Uniforms>();
			IntPtr ptr;
			mUniforms.DeviceMemory.MapMemory(mConfiguration.Device,
											(ulong)(rawsize * constantDataBufferIndex),
											 (ulong)rawsize, 0,
											out ptr);
			Marshal.StructureToPtr(uniforms, ptr, false);
			mUniforms.DeviceMemory.UnmapMemory(mConfiguration.Device);
			rotation += .01f;
		}

		public void Reshape(uint width, uint height)
		{
			mWidth = width;
			mHeight = height;

			// When reshape is called, update the view and projection matricies since this means the view orientation or size changed
			var aspect = (float) mWidth / (float) mHeight ;
			projectionMatrix = CreateMatrixFromPerspective(65f * ((float)Math.PI / 180f), aspect, .1f, 100f);

			viewMatrix = Matrix4.Identity;
		}

		public void Render()
		{
			//inflightSemaphore.WaitOne();
			try
			{
				uint layerNo = mPresentationLayer.BeginDraw(mPostPresentBarrierCmd, null);

				Update();

				mConfiguration.Queue.QueueSubmit(
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

				mConfiguration.Queue.QueueWaitIdle();

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
				throw ex;
			}
		}

		private static Matrix4 CreateMatrixFromRotation(float radians, float x, float y, float z)
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

		private static Matrix4 CreateMatrixFromTranslation(float x, float y, float z)
		{
			var m = Matrix4.Identity;
			m.Row0.W = x;
			m.Row1.W = y;
			m.Row2.W = z;
			m.Row3.W = 1f;
			return m;
		}

		private static Matrix4 CreateMatrixFromPerspective(float fovY, float aspect, float nearZ, float farZ)
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

		#region IDisposable Support
		private bool mIsDisposed = false; // To detect redundant calls
		protected virtual void Dispose(bool disposing)
		{
			if (mIsDisposed)
			{
				return;
			}

			if (disposing)
			{
				ReleaseManagedResources();
			}
			ReleaseUnmanagedResources();

			mIsDisposed = true;

		}

		void ReleaseManagedResources()
		{
			
		}

		void ReleaseUnmanagedResources()
		{
			var device = mConfiguration.Device;

			if (device != null)
			{
				if (mSetLayout != null)
					mSetLayout.DestroyDescriptorSetLayout(device, null);

				if (mDescriptorPool != null)
				{
					if (mUniformDescriptorSet != null)
						device.FreeDescriptorSets(mDescriptorPool, new[] { mUniformDescriptorSet });

					mDescriptorPool.DestroyDescriptorPool(device, null);
				}

				if (mPresentingCmdBuffers != null)
				{
					device.FreeCommandBuffers(mConfiguration.Partition.CommandPool, mPresentingCmdBuffers);
				}

				if (mRenderCmdBuffers != null)
				{
					device.FreeCommandBuffers(mConfiguration.Partition.CommandPool, mRenderCmdBuffers);
				}

				if (mDeviceMemory != null)
				{
					mDeviceMemory.FreeMemory(device, null);
				}

				if (mBuffer != null)
				{
					mBuffer.DestroyBuffer(device, null);
				}

				if (mPipelineState != null)
				{
					mPipelineState.DestroyPipeline(device, null);
				}

				if (mPipelineLayout != null)
				{
					mPipelineLayout.DestroyPipelineLayout(device, null);
				}
			}

			if (mSwapchains != null)
				mSwapchains.Dispose();

			if (mGraphicsDevice != null)
				mGraphicsDevice.Dispose();
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~SpinningCube() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			//GC.SuppressFinalize(this);
		}
		#endregion
	}
}
