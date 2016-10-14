using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Foundation;
using Metal;

namespace Magnesium.Metal
{
	public class AmtGraphicsPipeline : IMgPipeline
	{
		public IMTLFunction VertexFunction { get; private set; }
		public IMTLFunction FragmentFunction { get; private set; }
		public AmtPipelineLayout Layout { get; private set; }

		public AmtGraphicsPipeline(IAmtMetalLibraryLoader generator, IMTLDevice device, MgGraphicsPipelineCreateInfo info)
		{
			var layout = (AmtPipelineLayout)info.Layout;
			if (layout == null)
			{
				throw new ArgumentException(nameof(info.Layout));
			}
			Layout = layout;

			if (info.VertexInputState == null)
			{
				throw new ArgumentNullException(nameof(info.VertexInputState));
			}

			if (info.InputAssemblyState == null)
			{
				throw new ArgumentNullException(nameof(info.InputAssemblyState));
			}

			if (info.RasterizationState == null)
			{
				throw new ArgumentNullException(nameof(info.RasterizationState));
			}

			// TODO : WHY DO I NEED RENDERPASS HERE
			if (info.RenderPass == null)
			{
				throw new ArgumentNullException(nameof(info.RenderPass));
			}

			RenderPass = (AmtRenderPass)info.RenderPass;

			InitializeShaderFunctions(generator, device, info.Stages);
			InitializeVertexDescriptor(info.VertexInputState);
			InitiailizeDepthStateDescriptor(info.DepthStencilState);
			InitializeRasterization(info.RasterizationState);
			InitializationInputAssembly(info.InputAssemblyState);
			InitializeColorBlending(info.ColorBlendState);
			InitializeDynamicStates(info.DynamicState);
			InitializeResources(info.VertexInputState);
			InitializeViewportAndScissor(info.ViewportState);
			InitializeMultisampleInfo(info.MultisampleState);
		}

		public bool AlphaToCoverageEnabled { get; private set; }
		public bool AlphaToOneEnabled { get; private set; }
		public nuint SampleCount { get; private set; }

		void InitializeMultisampleInfo(MgPipelineMultisampleStateCreateInfo multisampleState)
		{
			if (multisampleState != null)
			{
				AlphaToCoverageEnabled = multisampleState.AlphaToCoverageEnable;
				AlphaToOneEnabled = multisampleState.AlphaToOneEnable;
				SampleCount = AmtSampleCountFlagBitExtensions.TranslateSampleCount(
								multisampleState.RasterizationSamples);
			}
			else
			{
				// NO MULTISAMPLING
				AlphaToCoverageEnabled = false;
				AlphaToOneEnabled = false;
				SampleCount = 1;
			}
		}

		public MTLViewport? Viewport { get; private set;}
		public MTLScissorRect? ScissorRect { get; private set;}
		void InitializeViewportAndScissor(MgPipelineViewportStateCreateInfo viewportState)
		{
			// if dynamic state has not been supplied, then default viewport is mandatory

			bool isViewportUserSupplied = (DynamicStates & AmtGraphicsPipelineDynamicStateFlagBits.VIEWPORT) 
												== AmtGraphicsPipelineDynamicStateFlagBits.VIEWPORT;

			if (viewportState == null || viewportState.Viewports == null)
			{
				if (!isViewportUserSupplied)
				{
					throw new InvalidOperationException(nameof(viewportState.Viewports) + " must be supplied.");
				}
			}
			else
			{
				var vp = viewportState.Viewports[0];

				Viewport = new MTLViewport
				{
					Height = vp.Height,
					Width = vp.Width,
					OriginX = vp.X,
					OriginY = vp.Y,
					// HOPE THIS IS RIGHT
					ZNear = vp.MinDepth,
					ZFar = vp.MaxDepth,
				};
			}

			bool isScissorUserSupplied = (DynamicStates & AmtGraphicsPipelineDynamicStateFlagBits.SCISSOR)
												== AmtGraphicsPipelineDynamicStateFlagBits.SCISSOR;
			// if dynamic state has not been supplied, then default viewport is mandatory
			if (viewportState == null || viewportState.Scissors == null)
			{
				if (!isScissorUserSupplied)
				{
					throw new InvalidOperationException(nameof(viewportState.Scissors) + " must be supplied.");
				}
			}
			else
			{
				var scissor = viewportState.Scissors[0];

				ScissorRect = new MTLScissorRect
				{
					X = (nuint)scissor.Offset.X,
					Y = (nuint)scissor.Offset.Y,
					Width = (nuint)scissor.Extent.Width,
					Height = (nuint)scissor.Extent.Height
				};
			}
		}

		public AmtRenderPass RenderPass { get; internal set;}

		public AmtPipelineLayoutBufferBinding[] VertexBufferBindings { get; private set; }

		void InitializeResources(MgPipelineVertexInputStateCreateInfo vertexInputState)
		{

			// Vertex data is made up of 
			// vertex buffers in vertex input state

			var slots = new SortedList<uint, AmtPipelineLayoutBufferBinding>();

			var bindingOffset = 0U;
			foreach (var definition in vertexInputState.VertexBindingDescriptions)
			{
				slots.Add(definition.Binding, new AmtPipelineLayoutBufferBinding
				{
					Binding = definition.Binding,
					DescriptorCount = 1,
					Category = AmtPipelineLayoutBufferBindingCategory.VertexBuffer,
				});
				++bindingOffset;
			}

			var combinedBuffers = new List<AmtPipelineLayoutBufferBinding>();
			combinedBuffers.AddRange(slots.Values);
			combinedBuffers.AddRange(Layout.VertexStage.VertexBuffers);

			VertexBufferBindings = combinedBuffers.ToArray();

			// then buffer, uniform, ssbo, ssbo dynamics sorted by binding


			// Next is samples and textures (with the same positional order no) sorted by binding 

			// Fragment 

			// Next is samples and textures (with the same positional order no) sorted by binding 
		}



		public AmtGraphicsPipelineDynamicStateFlagBits DynamicStates { get; private set;}
		void InitializeDynamicStates(MgPipelineDynamicStateCreateInfo dynamicState)
		{
			DynamicStates = 0U;

			if (dynamicState != null && dynamicState.DynamicStates != null)
			{
				foreach (var state in dynamicState.DynamicStates)
				{
					switch (state)
					{
						case MgDynamicState.BLEND_CONSTANTS:
							DynamicStates |= AmtGraphicsPipelineDynamicStateFlagBits.BLEND_CONSTANTS;
							break;
						case MgDynamicState.VIEWPORT:
							DynamicStates |=  AmtGraphicsPipelineDynamicStateFlagBits.VIEWPORT;
							break;
						case MgDynamicState.SCISSOR:
							DynamicStates |=  AmtGraphicsPipelineDynamicStateFlagBits.SCISSOR;
							break;
						case MgDynamicState.STENCIL_REFERENCE:
							DynamicStates |= AmtGraphicsPipelineDynamicStateFlagBits.STENCIL_REFERENCE;
							break;
						case MgDynamicState.STENCIL_WRITE_MASK:
							DynamicStates |= AmtGraphicsPipelineDynamicStateFlagBits.STENCIL_WRITE_MASK;
							break;
						case MgDynamicState.STENCIL_COMPARE_MASK:
							DynamicStates |= AmtGraphicsPipelineDynamicStateFlagBits.STENCIL_COMPARE_MASK;
							break;
						// NOT NEEDED
						case MgDynamicState.LINE_WIDTH:
							DynamicStates |= AmtGraphicsPipelineDynamicStateFlagBits.LINE_WIDTH;
							break;
						case MgDynamicState.DEPTH_BIAS:
							DynamicStates |= AmtGraphicsPipelineDynamicStateFlagBits.DEPTH_BIAS;
							break;
						// NOT NEEDED
						case MgDynamicState.DEPTH_BOUNDS:
							DynamicStates |= AmtGraphicsPipelineDynamicStateFlagBits.DEPTH_BOUNDS;
							break;
					}
				}
			}
		}

		public AmtBlendColorAttachmentRecord[] Attachments { get; private set; }
		public MgColor4f BlendConstants { get; set; }
		void InitializeColorBlending(MgPipelineColorBlendStateCreateInfo colorBlend)
		{
			if (colorBlend != null)
			{
				BlendConstants = colorBlend.BlendConstants;

				if (colorBlend.Attachments != null)
				{
					int arrayLength = colorBlend.Attachments.Length;
					var colorAttachments = new AmtBlendColorAttachmentRecord[arrayLength];
					for (uint i = 0; i < arrayLength; ++i)
					{
						var attachment = colorBlend.Attachments[i];

						colorAttachments[i] = new AmtBlendColorAttachmentRecord
						{
							IsBlendingEnabled = attachment.BlendEnable,
							SourceRgbBlendFactor = TranslateBlendFactor(attachment.SrcColorBlendFactor),
							DestinationRgbBlendFactor = TranslateBlendFactor(attachment.DstColorBlendFactor),
							RgbBlendOperation = TranslateBlendOperation(attachment.ColorBlendOp),
							SourceAlphaBlendFactor = TranslateBlendFactor(attachment.SrcAlphaBlendFactor),
							DestinationAlphaBlendFactor = TranslateBlendFactor(attachment.DstAlphaBlendFactor),
							AlphaBlendOperation = TranslateBlendOperation(attachment.AlphaBlendOp),
							ColorWriteMask = TranslateColorWriteMask(attachment.ColorWriteMask),
						};
					}
					Attachments = colorAttachments;
				}
				else
				{
					Attachments = new AmtBlendColorAttachmentRecord[] { };
				}
			}
			else
			{
				BlendConstants = new MgColor4f(0f, 0f, 0f, 0f);
				Attachments = new AmtBlendColorAttachmentRecord[] { };
			}
		}

		private struct AmtColorWriteMaskKey
		{
			public MgColorComponentFlagBits Compare { get; set;}
			public MTLColorWriteMask WriteMask { get; set;}
		}


		static MTLColorWriteMask TranslateColorWriteMask(MgColorComponentFlagBits writeMask)
		{
			AmtColorWriteMaskKey[] masks = new []{
				new AmtColorWriteMaskKey{Compare = MgColorComponentFlagBits.R_BIT, WriteMask = MTLColorWriteMask.Red},
				new AmtColorWriteMaskKey{Compare = MgColorComponentFlagBits.G_BIT, WriteMask = MTLColorWriteMask.Green},
				new AmtColorWriteMaskKey{Compare = MgColorComponentFlagBits.B_BIT, WriteMask = MTLColorWriteMask.Blue},
				new AmtColorWriteMaskKey{Compare = MgColorComponentFlagBits.A_BIT, WriteMask = MTLColorWriteMask.Alpha},
			};

			MTLColorWriteMask output = MTLColorWriteMask.None;

			foreach (var key in masks)
			{
				if ((writeMask & key.Compare) == key.Compare)
				{
					output |= key.WriteMask;
				}
			}


          	return output;
		}

		MTLBlendFactor TranslateBlendFactor(MgBlendFactor factor)
		{
			switch (factor)
			{
				default:
					throw new NotSupportedException();
				case MgBlendFactor.ONE:
					return MTLBlendFactor.One;
				case MgBlendFactor.ZERO:
					return MTLBlendFactor.Zero;
				case MgBlendFactor.SRC_COLOR:
					return MTLBlendFactor.SourceColor;
				case MgBlendFactor.SRC_ALPHA:
					return MTLBlendFactor.SourceAlpha;
				case MgBlendFactor.DST_COLOR:
					return MTLBlendFactor.DestinationColor;
				case MgBlendFactor.DST_ALPHA:
					return MTLBlendFactor.DestinationAlpha;
				case MgBlendFactor.ONE_MINUS_SRC_COLOR:
					return MTLBlendFactor.OneMinusSourceColor;
				case MgBlendFactor.ONE_MINUS_SRC_ALPHA:
					return MTLBlendFactor.OneMinusSourceAlpha;
				case MgBlendFactor.ONE_MINUS_DST_COLOR:
					return MTLBlendFactor.OneMinusDestinationColor;
				case MgBlendFactor.ONE_MINUS_DST_ALPHA:
					return MTLBlendFactor.OneMinusDestinationAlpha;
				case MgBlendFactor.SRC_ALPHA_SATURATE:
					return MTLBlendFactor.SourceAlphaSaturated;
				case MgBlendFactor.CONSTANT_COLOR:
					return MTLBlendFactor.BlendColor;
				case MgBlendFactor.ONE_MINUS_CONSTANT_COLOR:
					return MTLBlendFactor.OneMinusBlendColor;
				case MgBlendFactor.CONSTANT_ALPHA:
					return MTLBlendFactor.BlendAlpha;
				case MgBlendFactor.ONE_MINUS_CONSTANT_ALPHA:
					return MTLBlendFactor.OneMinusBlendAlpha;
			}
		}

		MTLBlendOperation TranslateBlendOperation(MgBlendOp blendOp)
		{
			switch (blendOp)
			{
				default:
					throw new NotSupportedException();
				case MgBlendOp.ADD:
					return MTLBlendOperation.Add;
				case MgBlendOp.MAX:
					return MTLBlendOperation.Max;
				case MgBlendOp.MIN:
					return MTLBlendOperation.Min;
				case MgBlendOp.REVERSE_SUBTRACT:
					return MTLBlendOperation.ReverseSubtract;
				case MgBlendOp.SUBTRACT:
					return MTLBlendOperation.Subtract;
			}
		}

		public MTLPrimitiveType Topology { get; private set;}
		void InitializationInputAssembly(MgPipelineInputAssemblyStateCreateInfo inputAssemblyState)
		{
			Topology = TranslatePrimitiveType(inputAssemblyState.Topology);
		}

		private static MTLPrimitiveType TranslatePrimitiveType(MgPrimitiveTopology topology)
		{
			switch(topology)
			{
				default:
					throw new NotSupportedException();
				case MgPrimitiveTopology.TRIANGLE_LIST:
					return MTLPrimitiveType.Triangle;
				case MgPrimitiveTopology.POINT_LIST:
					return MTLPrimitiveType.Point;
				case MgPrimitiveTopology.TRIANGLE_STRIP:
					return MTLPrimitiveType.TriangleStrip;
				case MgPrimitiveTopology.LINE_LIST:
					return MTLPrimitiveType.Line;
				case MgPrimitiveTopology.LINE_STRIP:
					return MTLPrimitiveType.LineStrip;
			}
		}

		public MTLCullMode CullMode { get; private set; }

		public float Clamp { get; private set; }

		public float SlopeScale { get; private set; }

		public float DepthBiasConstantFactor { get; private set; }

		public MTLWinding Winding { get; private set; }

		public MTLTriangleFillMode FillMode { get; private set;}

		public bool RasterizationDiscardEnabled { get; private set; }

		void InitializeRasterization(MgPipelineRasterizationStateCreateInfo rasterizationState)
		{
			CullMode = TranslateCullMode(rasterizationState.CullMode);
			Clamp = rasterizationState.DepthBiasClamp;
			SlopeScale = rasterizationState.DepthBiasSlopeFactor;
			DepthBiasConstantFactor = rasterizationState.DepthBiasConstantFactor;
			Winding = TranslateWinding(rasterizationState.FrontFace);
			FillMode = TranslateFillMode(rasterizationState.PolygonMode);
			RasterizationDiscardEnabled = rasterizationState.RasterizerDiscardEnable;
		}

		MTLTriangleFillMode TranslateFillMode(MgPolygonMode polygonMode)
		{
			switch (polygonMode)
			{
				default:
					// METAL : POINTS MODE RENDERING FOR POLYGON NOT SUPPORT, MAYBE CHANGE TOPOLOGY TO POINTS
					throw new NotSupportedException();
				case MgPolygonMode.FILL:
					return MTLTriangleFillMode.Fill;
				case MgPolygonMode.LINE:
					return MTLTriangleFillMode.Lines;
					                  
			}
		}

		static MTLWinding TranslateWinding(MgFrontFace winding)
		{
			switch (winding)
			{
				default:
					throw new NotSupportedException();
				case MgFrontFace.CLOCKWISE:
					return MTLWinding.Clockwise;
				case MgFrontFace.COUNTER_CLOCKWISE:
					return MTLWinding.CounterClockwise;
			}
		}

		static MTLCullMode TranslateCullMode(MgCullModeFlagBits cullMode)
		{
			switch (cullMode)
			{
				default:
					// METAL : FRONT_AND_BACK
					throw new NotSupportedException();
				case MgCullModeFlagBits.NONE:
					return MTLCullMode.None;
				case MgCullModeFlagBits.FRONT_BIT:
					return MTLCullMode.Front;
				case MgCullModeFlagBits.BACK_BIT:
					return MTLCullMode.Back;
			}
		}

		public AmtGraphicsPipelineStencilInfo BackStencil { get; private set;}
		public bool DepthWriteEnabled { get; private set; }
		public MgCompareOp DepthCompareFunction { get; private set;}

		public AmtGraphicsPipelineStencilInfo FrontStencil { get; private set; }
		public uint FrontStencilReference { get; private set;}
		public uint BackStencilReference { get; private set;}

		void InitiailizeDepthStateDescriptor(MgPipelineDepthStencilStateCreateInfo depthStencil)
		{
			if (depthStencil != null)
			{
				DepthCompareFunction = depthStencil.DepthCompareOp;
				DepthWriteEnabled = depthStencil.DepthWriteEnable;

				{
					var localState = depthStencil.Back;
					BackStencil = new AmtGraphicsPipelineStencilInfo
					{
						WriteMask = localState.WriteMask,
						ReadMask = localState.CompareMask,
						StencilCompareFunction = localState.CompareOp,
						DepthFailure = localState.DepthFailOp,
						DepthStencilPass = localState.PassOp,
						StencilFailure = localState.FailOp,
					};
					BackStencilReference = localState.Reference;
				}


				{
					var localState = depthStencil.Front;
					BackStencil = new AmtGraphicsPipelineStencilInfo
					{
						WriteMask = localState.WriteMask,
						ReadMask = localState.CompareMask,
						StencilCompareFunction = localState.CompareOp,
						DepthFailure = localState.DepthFailOp,
						DepthStencilPass = localState.PassOp,
						StencilFailure = localState.FailOp,
					};
					FrontStencilReference = localState.Reference;
				}
			}
			else
			{
				// VULKAN : If there is no depth framebuffer attachment, it is as if the depth test always passes.
				DepthCompareFunction = MgCompareOp.ALWAYS;
				DepthWriteEnabled = true;

				// USE OPENGL DEFAULTS

				BackStencil = new AmtGraphicsPipelineStencilInfo
				{
					WriteMask = ~0U,
					ReadMask = int.MaxValue,
					StencilCompareFunction = MgCompareOp.ALWAYS,
					DepthFailure = MgStencilOp.KEEP,
					DepthStencilPass = MgStencilOp.KEEP,
					StencilFailure = MgStencilOp.KEEP,
				};
				BackStencilReference = ~0U;

				FrontStencil = new AmtGraphicsPipelineStencilInfo
				{
					WriteMask = ~0U,
					ReadMask = int.MaxValue,
					StencilCompareFunction = MgCompareOp.ALWAYS,
					DepthFailure = MgStencilOp.KEEP,
					DepthStencilPass = MgStencilOp.KEEP,
					StencilFailure = MgStencilOp.KEEP,
				};
				FrontStencilReference = ~0U;
			}
		}

		public AmtGraphicsPipelineVertexAttribute[] Attributes { get; private set; }
		public AmtGraphicsPipelineVertexLayoutBinding[] Layouts { get; private set;}
		private void InitializeVertexDescriptor(MgPipelineVertexInputStateCreateInfo vertexInput)
		{
			{
				var noOfBindings = vertexInput.VertexBindingDescriptions.Length;
				Layouts = new AmtGraphicsPipelineVertexLayoutBinding[noOfBindings];
				for (var i = 0; i < noOfBindings; ++i)
				{
					var binding = vertexInput.VertexBindingDescriptions[i];
					Layouts[i] = new AmtGraphicsPipelineVertexLayoutBinding
					{
						Index = (nint)i,
						StepFunction = TranslateStepFunction(binding.InputRate),
						Stride = (nuint)binding.Stride,
					};
				}

			}

			{
				var noOfAttributes = vertexInput.VertexAttributeDescriptions.Length;
				Attributes = new AmtGraphicsPipelineVertexAttribute[noOfAttributes];
				for (var i = 0; i < noOfAttributes; ++i)
				{
					var attribute = vertexInput.VertexAttributeDescriptions[i];

					Attributes[i] = new AmtGraphicsPipelineVertexAttribute
					{
						Index = (nint)i,
						Offset = attribute.Offset,
						BufferIndex = (nuint)attribute.Binding,
						Format = AmtFormatExtensions.GetVertexFormat(attribute.Format),
					};

				}
			}
		}

		public MTLVertexDescriptor GetVertexDescriptor()
		{
			var output = new MTLVertexDescriptor
			{

			};

			for (var i = 0; i < Layouts.Length; ++i)
			{
				nint index = Layouts[i].Index;
				output.Layouts[index].StepFunction = Layouts[i].StepFunction;
				output.Layouts[index].StepRate = 1;
				output.Layouts[index].Stride = Layouts[i].Stride;
			}

			for (var i = 0; i<Attributes.Length; ++i)
			{
				nint index = Attributes[i].Index;
				output.Attributes[index].Offset = Attributes[i].Offset;
				output.Attributes[index].BufferIndex = Attributes[i].BufferIndex;
				output.Attributes[index].Format = Attributes[i].Format;
			}

			return output;
		}

		MTLVertexStepFunction TranslateStepFunction(MgVertexInputRate inputRate)
		{
			switch (inputRate)
			{
				case MgVertexInputRate.INSTANCE:
					return MTLVertexStepFunction.PerInstance;
				case MgVertexInputRate.VERTEX:
					return MTLVertexStepFunction.PerVertex;
				default:
					throw new NotSupportedException();
			}
		}

		private void InitializeShaderFunctions(IAmtMetalLibraryLoader generator, IMTLDevice device, MgPipelineShaderStageCreateInfo[] stages)
		{
			IMTLFunction frag = null;
			IMTLFunction vert = null;
			foreach (var stage in stages)
			{
				IMTLFunction shaderFunc = null;

				var module = (AmtShaderModule)stage.Module;
				Debug.Assert(module != null);
				// LIBRARY : can't store function unless name matches
				if (module.Library != null)
				{
					shaderFunc = module.Library.CreateFunction(stage.Name);
				}
				else
				{
					using (var ms = new MemoryStream())
					{
						module.Info.Code.CopyTo(ms, (int)module.Info.CodeSize.ToUInt32());
						ms.Seek(0, SeekOrigin.Begin);
						module.Library = generator.LoadLibrary(device, ms);
					}
				}
				shaderFunc = module.Library.CreateFunction(stage.Name);
				Debug.Assert(shaderFunc != null);
				if (stage.Stage == MgShaderStageFlagBits.VERTEX_BIT)
				{
					vert = shaderFunc;
				}
				else if (stage.Stage == MgShaderStageFlagBits.FRAGMENT_BIT)
				{
					frag = shaderFunc;
				}

			}

			VertexFunction = vert;
			FragmentFunction = frag;
		}

		public void DestroyPipeline(IMgDevice device, IMgAllocationCallbacks allocator)
		{
	
		}
	}
}
