using System;
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

		public AmtGraphicsPipeline(IMTLDevice device, MgGraphicsPipelineCreateInfo info)
		{
			var layout = (AmtPipelineLayout)info.Layout;
			if (layout == null)
			{
				throw new ArgumentException(nameof(info.Layout));
			}

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

			if (info.RenderPass == null)
			{
				throw new ArgumentNullException(nameof(info.RenderPass));
			}

			InitializeShaderFunctions(device, info);
			InitializeVertexDescriptor(info.VertexInputState);
			InitiailizeDepthStateDescriptor(info.DepthStencilState);
			InitializeRasterization(info.RasterizationState);
			InitializationInputAssembly(info.InputAssemblyState);
			//InitialiseColorAttachments(info);

			//Foundation.NSError error;
			//var pipelineState = device.CreateRenderPipelineState(PipelineStateDescriptor, out error);
			//if (pipelineState == null)
			//	Console.WriteLine("Failed to created pipeline state, error {0}", error);

			//var depthStateDesc = new MTLDepthStencilDescriptor
			//{
			//	DepthCompareFunction = MTLCompareFunction.Less,
			//	DepthWriteEnabled = true,

			//};

			//var depthState = device.CreateDepthStencilState(depthStateDesc);
		}

		public AmtBlendColorState ColorBlendEnums { get; private set; }
		public MgColor4f BlendConstants { get; set; }
		void PopulateColorBlend(MgPipelineColorBlendStateCreateInfo colorBlend)
		{
			ColorBlendEnums = new AmtBlendColorState();
			if (colorBlend != null)
			{
				BlendConstants = colorBlend.BlendConstants;

				if (colorBlend.Attachments != null)
				{
					var colorAttachments = new AmtBlendColorAttachmentState[colorBlend.Attachments.Length];
					for (uint i = 0; i < colorBlend.Attachments.Length; ++i)
					{
						var attachment = colorBlend.Attachments[i];

						colorAttachments[i] = new AmtBlendColorAttachmentState
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
					ColorBlendEnums.Attachments = colorAttachments;
				}
				else
				{
					ColorBlendEnums.Attachments = new AmtBlendColorAttachmentState[] { };
				}
			}
			else
			{
				BlendConstants = new MgColor4f(0f, 0f, 0f, 0f);
				ColorBlendEnums.Attachments = new AmtBlendColorAttachmentState[] { };
			}
		}

		private struct AmtColorWriteMaskKey
		{
			public MgColorComponentFlagBits CompareMask { get; set;}
			public MTLColorWriteMask WriteMask { get; set;}
		}


		static MTLColorWriteMask TranslateColorWriteMask(MgColorComponentFlagBits writeMask)
		{
			AmtColorWriteMaskKey[] masks = new []{
				new AmtColorWriteMaskKey{CompareMask = MgColorComponentFlagBits.R_BIT, WriteMask = MTLColorWriteMask.Red},
				new AmtColorWriteMaskKey{CompareMask = MgColorComponentFlagBits.G_BIT, WriteMask = MTLColorWriteMask.Green},
				new AmtColorWriteMaskKey{CompareMask = MgColorComponentFlagBits.B_BIT, WriteMask = MTLColorWriteMask.Blue},
				new AmtColorWriteMaskKey{CompareMask = MgColorComponentFlagBits.A_BIT, WriteMask = MTLColorWriteMask.Alpha},
			};

			MTLColorWriteMask output = MTLColorWriteMask.None;

			foreach (var key in masks)
			{
				if ((writeMask & key.CompareMask) == key.CompareMask)
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

		public float ConstantFactor { get; private set; }

		public MTLWinding Winding { get; private set; }

		public MTLTriangleFillMode FillMode { get; private set;}

		void InitializeRasterization(MgPipelineRasterizationStateCreateInfo rasterizationState)
		{
			CullMode = TranslateCullMode(rasterizationState.CullMode);
			Clamp = rasterizationState.DepthBiasClamp;
			SlopeScale = rasterizationState.DepthBiasSlopeFactor;
			ConstantFactor = rasterizationState.DepthBiasConstantFactor;
			Winding = TranslateWinding(rasterizationState.FrontFace);
			FillMode = TranslateFillMode(rasterizationState.PolygonMode);
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

		public AmtStencilInfo BackStencil { get; private set;}
		public bool DepthWriteEnabled { get; private set; }
		public MTLCompareFunction DepthCompareFunction { get; private set;}

		public AmtStencilInfo FrontStencil { get; private set; }

		void InitiailizeDepthStateDescriptor(MgPipelineDepthStencilStateCreateInfo depthStencil)
		{
			if (depthStencil != null)
			{
				DepthCompareFunction = GetCompareFunction(depthStencil.DepthCompareOp);
				DepthWriteEnabled = depthStencil.DepthWriteEnable;

				{
					var localState = depthStencil.Back;
					BackStencil = new AmtStencilInfo
					{
						WriteMask = localState.WriteMask,
						ReadMask = localState.CompareMask,
						StencilCompareFunction = GetCompareFunction(localState.CompareOp),
						DepthFailure = GetStencilOperation(localState.DepthFailOp),
						DepthStencilPass = GetStencilOperation(localState.PassOp),
						StencilFailure = GetStencilOperation(localState.FailOp),
						StencilReference = localState.Reference,
					};
				}

				{
					var localState = depthStencil.Front;
					BackStencil = new AmtStencilInfo
					{
						WriteMask = localState.WriteMask,
						ReadMask = localState.CompareMask,
						StencilCompareFunction = GetCompareFunction(localState.CompareOp),
						DepthFailure = GetStencilOperation(localState.DepthFailOp),
						DepthStencilPass = GetStencilOperation(localState.PassOp),
						StencilFailure = GetStencilOperation(localState.FailOp),
						StencilReference = localState.Reference,
					};
				}
			}
			else
			{
				// VULKAN : If there is no depth framebuffer attachment, it is as if the depth test always passes.
				DepthCompareFunction = MTLCompareFunction.Always;
				DepthWriteEnabled = true;

				// USE OPENGL DEFAULTS

				BackStencil = new AmtStencilInfo
				{
					WriteMask = ~0U,
					ReadMask = int.MaxValue,
					StencilCompareFunction = MTLCompareFunction.Always,
					DepthFailure = MTLStencilOperation.Keep,
					DepthStencilPass = MTLStencilOperation.Keep,
					StencilFailure = MTLStencilOperation.Keep,
					StencilReference = ~0U,
				};

				FrontStencil = new AmtStencilInfo
				{
					WriteMask = ~0U,
					ReadMask = int.MaxValue,
					StencilCompareFunction = MTLCompareFunction.Always,
					DepthFailure = MTLStencilOperation.Keep,
					DepthStencilPass = MTLStencilOperation.Keep,
					StencilFailure = MTLStencilOperation.Keep,
					StencilReference = ~0U,
				};
			}
		}

		MTLStencilOperation GetStencilOperation(MgStencilOp stencilOp)
		{
			switch (stencilOp)
			{
				default:
					throw new NotSupportedException();
				case MgStencilOp.DECREMENT_AND_CLAMP:
					return MTLStencilOperation.DecrementClamp;
				case MgStencilOp.DECREMENT_AND_WRAP:
					return MTLStencilOperation.DecrementWrap;
				case MgStencilOp.INCREMENT_AND_CLAMP:
					return MTLStencilOperation.IncrementClamp;
				case MgStencilOp.INCREMENT_AND_WRAP:
					return MTLStencilOperation.IncrementWrap;
				case MgStencilOp.INVERT:
					return MTLStencilOperation.Invert;
				case MgStencilOp.KEEP:
					return MTLStencilOperation.Keep;
				case MgStencilOp.REPLACE:
					return MTLStencilOperation.Replace;
				case MgStencilOp.ZERO:
					return MTLStencilOperation.Zero;
			}
		}

		MTLCompareFunction GetCompareFunction(MgCompareOp depthCompareOp)
		{
			switch (depthCompareOp)
			{
				default:
					throw new NotSupportedException();
				case MgCompareOp.LESS:
					return MTLCompareFunction.Less;
				case MgCompareOp.LESS_OR_EQUAL:
					return MTLCompareFunction.LessEqual;
				case MgCompareOp.GREATER:
					return MTLCompareFunction.Greater;
				case MgCompareOp.GREATER_OR_EQUAL:
					return MTLCompareFunction.GreaterEqual;
				case MgCompareOp.NEVER:
					return MTLCompareFunction.Never;
				case MgCompareOp.NOT_EQUAL:
					return MTLCompareFunction.NotEqual;
				case MgCompareOp.EQUAL:
					return MTLCompareFunction.Equal;

			}
		}



		public AmtVertexAttribute[] Attributes { get; private set; }
		public AmtVertexLayoutBinding[] Layouts { get; private set;}
		private void InitializeVertexDescriptor(MgPipelineVertexInputStateCreateInfo vertexInput)
		{
			{
				var noOfBindings = vertexInput.VertexBindingDescriptions.Length;
				Layouts = new AmtVertexLayoutBinding[noOfBindings];
				for (var i = 0; i < noOfBindings; ++i)
				{
					var binding = vertexInput.VertexBindingDescriptions[i];
					Layouts[i] = new AmtVertexLayoutBinding
					{
						Index = (nint)i,
						StepFunction = TranslateStepFunction(binding.InputRate),
						Stride = (nuint)binding.Stride,
					};
				}

			}

			{
				var noOfAttributes = vertexInput.VertexAttributeDescriptions.Length;
				Attributes = new AmtVertexAttribute[noOfAttributes];
				for (var i = 0; i < noOfAttributes; ++i)
				{
					var attribute = vertexInput.VertexAttributeDescriptions[i];

					Attributes[i] = new AmtVertexAttribute
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

		public void InitializeShaderFunctions(IMTLDevice device, MgGraphicsPipelineCreateInfo info)
		{
			IMTLFunction frag = null;
			IMTLFunction vert = null;
			foreach (var stage in info.Stages)
			{
				IMTLFunction shaderFunc = null;

				var module = (AmtShaderModule)stage.Module;
				Debug.Assert(module != null);
				if (module.Function != null)
				{
					shaderFunc = module.Function;
				}
				else
				{
					using (var ms = new MemoryStream())
					{
						module.Info.Code.CopyTo(ms, (int)module.Info.CodeSize.ToUInt32());

						// UPDATE SHADERMODULE wIth FUNCTION FOR REUSE
						using (NSData data = NSData.FromArray(ms.ToArray()))
						{
							NSError err;
							IMTLLibrary library = device.CreateLibrary(data, out err);
							if (library == null)
							{
								// TODO: better error handling
								throw new Exception(err.ToString());
							}
							module.Function = library.CreateFunction(stage.Name);
							shaderFunc = module.Function;
						}
					}
				}
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
			throw new NotImplementedException();
		}


	}
}
