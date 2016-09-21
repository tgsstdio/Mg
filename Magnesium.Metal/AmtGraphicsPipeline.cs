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

		static void InitialiseColorAttachments(AmtRenderPass renderPass, MTLRenderPipelineDescriptor dest)
		{

			nint colorAttachmentIndex = 0;
			foreach (var attachment in renderPass.ClearAttachments)
			{
				if (attachment.Destination == AmtRenderPassAttachmentDestination.COLOR)
				{
					dest.ColorAttachments[colorAttachmentIndex].PixelFormat = AmtFormatExtensions.GetPixelFormat(attachment.Format);
					++colorAttachmentIndex;
				}
				else if (attachment.Destination == AmtRenderPassAttachmentDestination.DEPTH_AND_STENCIL)
				{
					dest.DepthAttachmentPixelFormat = AmtFormatExtensions.GetPixelFormat(attachment.Format);
					dest.StencilAttachmentPixelFormat = AmtFormatExtensions.GetPixelFormat(attachment.Format);
				}
			}
		}






		public AmtVertexAttribute[] Attributes { get; private set; }




		private AmtVertexLayoutBinding[] Layouts;
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
