using System;
using System.Diagnostics;
using Metal;

namespace Magnesium.Metal
{
	public class AmtGraphicsEncoder
	{
		private AmtGraphicsEncoderItemBag mItemBag;
		private IMTLDevice mDevice;
		public AmtGraphicsEncoder(AmtGraphicsEncoderItemBag bag, IMTLDevice device)
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

					if (attachment.ClearValueType == AmtRenderPassClearValueType.COLOR_INT)
					{
						var initialValue = clearValue.Color.Int32;

						r = Math.Max(Math.Min(initialValue.X, attachment.Divisor), -attachment.Divisor) / attachment.Divisor;
						g = Math.Max(Math.Min(initialValue.Y, attachment.Divisor), -attachment.Divisor) / attachment.Divisor;
						b = Math.Max(Math.Min(initialValue.Z, attachment.Divisor), -attachment.Divisor) / attachment.Divisor;
						a = Math.Max(Math.Min(initialValue.W, attachment.Divisor), -attachment.Divisor) / attachment.Divisor;

					}
					else if (attachment.ClearValueType == AmtRenderPassClearValueType.COLOR_INT)
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



			Debug.Assert(bPipeline.Attachments != null);
			for (var i = 0; i < bPipeline.Attachments.Length; ++i)
			{
				var attachment = bPipeline.Attachments[i];
				pipelineDescriptor.ColorAttachments[i].BlendingEnabled = attachment.IsBlendingEnabled;
				pipelineDescriptor.ColorAttachments[i].RgbBlendOperation = attachment.RgbBlendOperation;
				pipelineDescriptor.ColorAttachments[i].AlphaBlendOperation = attachment.AlphaBlendOperation;
				pipelineDescriptor.ColorAttachments[i].SourceRgbBlendFactor = attachment.SourceRgbBlendFactor;
				pipelineDescriptor.ColorAttachments[i].SourceAlphaBlendFactor = attachment.SourceAlphaBlendFactor;
				pipelineDescriptor.ColorAttachments[i].DestinationRgbBlendFactor = attachment.DestinationRgbBlendFactor;
				pipelineDescriptor.ColorAttachments[i].DestinationAlphaBlendFactor = attachment.DestinationAlphaBlendFactor;
				pipelineDescriptor.ColorAttachments[i].WriteMask = attachment.ColorWriteMask;
			}

			Foundation.NSError err;
			var pipelineState = mDevice.CreateRenderPipelineState(pipelineDescriptor, out err);
			// TODO : CHECK ERROR HERE


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

			var pipeDetail = new AmtPipelineEncoderState
			{
				PipelineState = pipelineState,
				DepthStencilState = depthStencil,

				CullMode = bPipeline.CullMode,

				Winding = bPipeline.Winding,
				FrontReference = mFrontReference,
				BackReference = mBackReference,
			};
			var nextIndex = mItemBag.PipelineStates.Push(pipeDetail);
			mItemBag.Instructions.Add(new AmtCommandEncoderInstruction
			{
				Category = AmtCommandEncoderCategory.Graphics,
				Index = (uint)nextIndex,
				Operation = CmdSetPipeline,
			});
		}



		private static void CmdSetPipeline(AmtCommandRecording recording, uint index)
		{
			var stage = recording.Graphics;
			var arg1 = stage.Encoder;

			var item = stage.Grid.Pipelines[index];
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
			mItemBag.Instructions.Add(new AmtCommandEncoderInstruction
			{
				Index = (uint) nextIndex,
				Operation = SetCmdBlendColors,
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
			mItemBag.Instructions.Add(new AmtCommandEncoderInstruction
			{
				Index = nextIndex,
				Operation = CmdSetViewport,

			});
		}

		static void CmdSetViewport(AmtCommandRecording recording, uint index)
		{
			var stage = recording.Graphics;
			var item = stage.Grid.Viewports[index];
			stage.Encoder.SetViewport(item);
		}

		public void SetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor)
		{
			var item = new AmtDepthBiasEncoderState
			{
				DepthBias = depthBiasConstantFactor,
				Clamp = depthBiasClamp,
				SlopeScale = depthBiasSlopeFactor
			};

			var nextIndex = mItemBag.DepthBias.Push(item);
			mItemBag.Instructions.Add(new AmtCommandEncoderInstruction
			{
				Index = nextIndex,
				Operation = SetCmdDepthBias,
			});
		}

		private static void SetCmdDepthBias(AmtCommandRecording recording, uint index)
		{
			var stage = recording.Graphics;
			var item = stage.Grid.DepthBias[index];
			stage.Encoder.SetDepthBias(item.DepthBias, item.SlopeScale, item.Clamp);
		}

		public void SetStencilReference(MgStencilFaceFlagBits faceMask, UInt32 reference)
		{
			if ((faceMask & MgStencilFaceFlagBits.FRONT_AND_BACK) == MgStencilFaceFlagBits.FRONT_AND_BACK)
			{
				var item = new AmtStencilReferenceEncoderState
				{
					Front = reference,
					Back = reference,
				};
				var nextIndex = mItemBag.StencilReferences.Push(item);
				mItemBag.Instructions.Add(new AmtCommandEncoderInstruction
				{
					Index = (uint)nextIndex,
					Operation = SetCmdStencilReference,
				});
				mBackReference = reference;
				mFrontReference = reference;
			}
			else if ((faceMask & MgStencilFaceFlagBits.BACK_BIT) == MgStencilFaceFlagBits.BACK_BIT)
			{
				var item = new AmtStencilReferenceEncoderState
				{
					Front = mFrontReference,
					Back = reference,
				};
				var nextIndex = mItemBag.StencilReferences.Push(item);
				mItemBag.Instructions.Add(new AmtCommandEncoderInstruction
				{
					Index = (uint)nextIndex,
					Operation = SetCmdStencilReference,
				});
				mBackReference = reference;

			}
			else if ((faceMask & MgStencilFaceFlagBits.FRONT_BIT) == MgStencilFaceFlagBits.FRONT_BIT)
			{
				var item = new AmtStencilReferenceEncoderState
				{
					Front = reference,
					Back = mBackReference,
				};
				var nextIndex = mItemBag.StencilReferences.Push(item);
				mItemBag.Instructions.Add(new AmtCommandEncoderInstruction
				{
					Index = (uint)nextIndex,
					Operation = SetCmdStencilReference,
				});
				mBackReference = reference;

			}
		}

		private static void SetCmdStencilReference(AmtCommandRecording recording, uint index)
		{
			var stage = recording.Graphics;
			var item = stage.Grid.StencilReferences[index];
			stage.Encoder.SetStencilFrontReferenceValue(item.Front, item.Back);
		}

		private static void SetCmdBlendColors(AmtCommandRecording recording, uint index)
		{
			var stage = recording.Graphics;
			var color = stage.Grid.BlendConstants[index];
			stage.Encoder.SetBlendColor(color.R, color.G, color.B, color.A);
		}
	}
}
