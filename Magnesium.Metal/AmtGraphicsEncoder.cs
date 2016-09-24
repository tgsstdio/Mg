using System;
using System.Diagnostics;
using Metal;

namespace Magnesium.Metal
{
	public class AmtGraphicsEncoder
	{
		delegate void SetEncoderCommand(IMTLRenderCommandEncoder encoder, AmtEncoderItemGrid grid, uint index);

		private AmtEncoderItemBag mItemBag;
		private IMTLDevice mDevice;
		public AmtGraphicsEncoder(AmtEncoderItemBag bag, IMTLDevice device)
		{
			mItemBag = bag;
			mDevice = device;
		}

		private AmtRenderPass mRenderPass;

		public void SetRenderpass(IMgRenderPass renderpass, MgClearValue[] clearValues)
		{
			Debug.Assert(renderpass != null);
			var bRenderPass = (AmtRenderPass)renderpass;

			var descriptor = new MTLRenderPassDescriptor { };
			SetClearValues(bRenderPass, clearValues, descriptor);


		}

		static void SetClearValues(AmtRenderPass renderPass, MgClearValue[] clearValues, MTLRenderPassDescriptor dest)
		{
			for (var i = 0; i < clearValues.Length; ++i)
			{
				var attachment = renderPass.ClearAttachments[i];
				var clearValue = clearValues[i];
				if (attachment.Destination == AmtRenderPassAttachmentDestination.COLOR)
				{
					double r, g, b, a;

					if (attachment.ClearValueType == AmtRenderPass.AmtClearValueType.COLOR_INT)
					{
						var initialValue = clearValue.Color.Int32;

						r = Math.Max(Math.Min(initialValue.X, attachment.Divisor), -attachment.Divisor) / attachment.Divisor;
						g = Math.Max(Math.Min(initialValue.Y, attachment.Divisor), -attachment.Divisor) / attachment.Divisor;
						b = Math.Max(Math.Min(initialValue.Z, attachment.Divisor), -attachment.Divisor) / attachment.Divisor;
						a = Math.Max(Math.Min(initialValue.W, attachment.Divisor), -attachment.Divisor) / attachment.Divisor;

					}
					else if (attachment.ClearValueType == AmtRenderPass.AmtClearValueType.COLOR_INT)
					{
						var initialValue = clearValue.Color.Uint32;

						r = Math.Min(initialValue.X, attachment.Divisor) / attachment.Divisor;
						g = Math.Min(initialValue.Y, attachment.Divisor) / attachment.Divisor;
						b = Math.Min(initialValue.Z, attachment.Divisor) / attachment.Divisor;
						a = Math.Min(initialValue.W, attachment.Divisor) / attachment.Divisor;
					}
					else
					{
						var initialValue = clearValue.Color.Float32;

						r = initialValue.R;
						g = initialValue.G;
						b = initialValue.B;
						a = initialValue.A;
					}

					dest.ColorAttachments[(nint)attachment.Index].ClearColor = new MTLClearColor(r,g,b,a);

				}
				else if (attachment.Destination == AmtRenderPassAttachmentDestination.DEPTH_AND_STENCIL)
				{
					dest.DepthAttachment.ClearDepth = clearValue.DepthStencil.Depth;
					dest.StencilAttachment.ClearStencil = clearValue.DepthStencil.Stencil;
				}
			}
		}

		private uint mFrontReference;
		private uint mBackReference;
		public void SetPipeline(IMgPipeline pipeline)
		{
			Debug.Assert(pipeline != null);


			var bPipeline = (AmtGraphicsPipeline)pipeline;

			var pipelineDescriptor = new MTLRenderPipelineDescriptor
			{
				VertexFunction = bPipeline.VertexFunction,
				FragmentFunction = bPipeline.FragmentFunction,
				VertexDescriptor = bPipeline.GetVertexDescriptor(),
				 
			};

			Debug.Assert(bPipeline.ColorBlendEnums != null);
			Debug.Assert(bPipeline.ColorBlendEnums.Attachments != null);
			foreach (var attachment in bPipeline.ColorBlendEnums.Attachments)
			{

			}

			Foundation.NSError err;
			var pipelineState = mDevice.CreateRenderPipelineState(pipelineDescriptor, out err);



			var dsDescriptor = new MTLDepthStencilDescriptor
			{
				DepthCompareFunction = bPipeline.DepthCompareFunction,
				DepthWriteEnabled = bPipeline.DepthWriteEnabled,
				BackFaceStencil = bPipeline.BackStencil.GetDescriptor(),
				FrontFaceStencil = bPipeline.FrontStencil.GetDescriptor(),
			};

			var depthStencil = mDevice.CreateDepthStencilState(dsDescriptor);

			//mFrontReference = bPipeline.FrontStencil.StencilReference;
			//mBackReference = bPipeline.BackStencil.StencilReference;

			var pipeDetail = new AmtCmdGraphicsPipelineDetail
			{
				PipelineState = pipelineState,
				DepthStencilState = depthStencil,

				CullMode = bPipeline.CullMode,

				Winding = bPipeline.Winding,
				FrontReference = mFrontReference,
				BackReference = mBackReference,
			};

		}



		private static void CmdSetPipeline(IMTLRenderCommandEncoder arg1, AmtEncoderItemGrid arg2, uint arg3)
		{
			var item = arg2.PipelineDetails[arg3];
			arg1.SetRenderPipelineState(item.PipelineState);
			arg1.SetDepthStencilState(item.DepthStencilState);
			arg1.SetViewport(item.Viewport);
			arg1.SetCullMode(item.CullMode);
			arg1.SetScissorRect(item.Scissor);
			arg1.SetTriangleFillMode(item.FillMode);
			arg1.SetFrontFacingWinding(item.Winding);
			arg1.SetStencilFrontReferenceValue(item.FrontReference, item.BackReference);



		}

		public void SetBlendConstants(MgColor4f color)
		{
			var nextIndex =mItemBag.BlendConstants.Push(color);
			mItemBag.Instructions.Add(new AmtCmdInstruction
			{
				Index = (uint) nextIndex,
				Setter = SetCmdBlendColors,
			});
		}

		public void SetViewport(MgViewport viewport)
		{
			var item = new MTLViewport
			{
				Height = viewport.Height,
				Width = viewport.Width,
				OriginX = viewport.X,
				OriginY = viewport.Y,
				// HOPE THIS IS RIGHT
				ZNear = viewport.MinDepth,
				ZFar = viewport.MaxDepth,
			};
			var nextIndex = mItemBag.Viewports.Push(item);
			mItemBag.Instructions.Add(new AmtCmdInstruction
			{
				Index = nextIndex,
				Setter = CmdSetViewport,

			});
		}

		static void CmdSetViewport(IMTLRenderCommandEncoder arg1, AmtEncoderItemGrid arg2, uint arg3)
		{
			var item = arg2.Viewports[arg3];
			arg1.SetViewport(item);
		}

		public void SetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor)
		{
			var item = new AmtDepthBiasParameter
			{
				DepthBias = depthBiasConstantFactor,
				Clamp = depthBiasClamp,
				SlopeScale = depthBiasSlopeFactor
			};

			var nextIndex = mItemBag.DepthBias.Push(item);
			mItemBag.Instructions.Add(new AmtCmdInstruction
			{
				Index = nextIndex,
				Setter = SetCmdDepthBias,
			});
		}

		private static void SetCmdDepthBias(IMTLRenderCommandEncoder encoder, AmtEncoderItemGrid grid, uint index)
		{
			var item = grid.DepthBias[index];
			encoder.SetDepthBias(item.DepthBias, item.SlopeScale, item.Clamp);
		}

		public void SetStencilReference(MgStencilFaceFlagBits faceMask, UInt32 reference)
		{
			if ((faceMask & MgStencilFaceFlagBits.FRONT_AND_BACK) == MgStencilFaceFlagBits.FRONT_AND_BACK)
			{
				var item = new AmtStencilReferenceMask
				{
					Front = reference,
					Back = reference,
				};
				var nextIndex = mItemBag.StencilReferences.Push(item);
				mItemBag.Instructions.Add(new AmtCmdInstruction
				{
					Index = (uint)nextIndex,
					Setter = SetCmdStencilReference,
				});
				mBackReference = reference;
				mFrontReference = reference;
			}
			else if ((faceMask & MgStencilFaceFlagBits.BACK_BIT) == MgStencilFaceFlagBits.BACK_BIT)
			{
				var item = new AmtStencilReferenceMask
				{
					Front = mFrontReference,
					Back = reference,
				};
				var nextIndex = mItemBag.StencilReferences.Push(item);
				mItemBag.Instructions.Add(new AmtCmdInstruction
				{
					Index = (uint)nextIndex,
					Setter = SetCmdStencilReference,
				});
				mBackReference = reference;

			}
			else if ((faceMask & MgStencilFaceFlagBits.FRONT_BIT) == MgStencilFaceFlagBits.FRONT_BIT)
			{
				var item = new AmtStencilReferenceMask
				{
					Front = reference,
					Back = mBackReference,
				};
				var nextIndex = mItemBag.StencilReferences.Push(item);
				mItemBag.Instructions.Add(new AmtCmdInstruction
				{
					Index = (uint)nextIndex,
					Setter = SetCmdStencilReference,
				});
				mBackReference = reference;

			}
		}

		private static void SetCmdStencilReference(IMTLRenderCommandEncoder encoder, AmtEncoderItemGrid grid, uint index)
		{
			var item = grid.StencilReferences[index];
			encoder.SetStencilFrontReferenceValue(item.Front, item.Back);
		}

		private static void SetCmdBlendColors(IMTLRenderCommandEncoder encoder, AmtEncoderItemGrid grid, uint index)
		{
			var color = grid.BlendConstants[index];
			encoder.SetBlendColor(color.R, color.G, color.B, color.A);
		}
	}
}
