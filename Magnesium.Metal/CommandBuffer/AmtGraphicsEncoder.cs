using System;
using System.Collections.Generic;
using System.Diagnostics;
using Metal;

namespace Magnesium.Metal
{
	public class AmtGraphicsEncoder : IAmtGraphicsEncoder
	{
		private AmtGraphicsEncoderItemBag mBag;
		private IMTLDevice mDevice;

		List<AmtCommandEncoderInstruction> mInstructions;

		public AmtGraphicsEncoder(List<AmtCommandEncoderInstruction> instructions, AmtGraphicsEncoderItemBag bag, IMTLDevice device)
		{
			mInstructions = instructions;
			mBag = bag;
			mDevice = device;
		}

		#region NextSubpass methods

		public void NextSubpass(MgSubpassContents contents)
		{
			
		}

		#endregion

		#region BeginRenderPass methods

		private AmtRenderPass mCurrentRenderPass;
		public void BeginRenderPass(MgRenderPassBeginInfo pRenderPassBegin, MgSubpassContents contents)
		{
			if (pRenderPassBegin != null)
				throw new ArgumentNullException(nameof(pRenderPassBegin));

			var bRenderPass = (AmtRenderPass)pRenderPassBegin.RenderPass;
			Debug.Assert(bRenderPass != null, nameof(pRenderPassBegin.RenderPass) + " is null");
			mCurrentRenderPass = bRenderPass;

			var bFramebuffer = (AmtFramebuffer)pRenderPassBegin.Framebuffer;
			Debug.Assert(bFramebuffer != null, nameof(pRenderPassBegin.Framebuffer) + " is null");

			var descriptor = new MTLRenderPassDescriptor { };
			InitializeClearValues(bRenderPass, pRenderPassBegin.ClearValues, descriptor);

			var nextIndex = mBag.RenderPasses.Push(descriptor);
			mInstructions.Add(new AmtCommandEncoderInstruction
			{
				Category = AmtCommandEncoderCategory.Graphics,
				Index = nextIndex,
				Operation = CmdSetBeginRenderPass,
			});
		}

		static void CmdSetBeginRenderPass(AmtCommandRecording recording, uint index)
		{
			var cmdBuf = recording.CommandBuffer;
			Debug.Assert(cmdBuf != null, nameof(recording.CommandBuffer) + " is null");
			var stage = recording.Graphics;
			var item = stage.Grid.RenderPasses[index];
			stage.Encoder = cmdBuf.CreateRenderCommandEncoder(item);
		}

		static void InitializeClearValues(AmtRenderPass renderPass, MgClearValue[] clearValues, MTLRenderPassDescriptor dest)
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

		#endregion

		#region BindPipeline methods

		private uint mFrontReference;
		private uint mBackReference;

		public void BindPipeline(IMgPipeline pipeline)
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
			var nextIndex = mBag.PipelineStates.Push(pipeDetail);

			mInstructions.Add(new AmtCommandEncoderInstruction
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

		#endregion

		#region SetBlendConstants methods

		public void SetBlendConstants(MgColor4f color)
		{
			var nextIndex = mBag.BlendConstants.Push(color);

			mInstructions.Add(new AmtCommandEncoderInstruction
			{
				Index = (uint) nextIndex,
				Operation = SetCmdBlendColors,
			});
		}

		private static void SetCmdBlendColors(AmtCommandRecording recording, uint index)
		{
			var stage = recording.Graphics;
			var color = stage.Grid.BlendConstants[index];
			stage.Encoder.SetBlendColor(color.R, color.G, color.B, color.A);
		}

		#endregion

		#region SetViewports methods

		public void SetViewports(uint firstViewport, MgViewport[] viewports)
		{
			if (firstViewport != 0)
				throw new ArgumentOutOfRangeException(firstViewport + " != 0");

			var vp = viewports[0];

			var item = new MTLViewport
			{
				Height = vp.Height,
				Width = vp.Width,
				OriginX = vp.X,
				OriginY = vp.Y,
				// HOPE THIS IS RIGHT
				ZNear = vp.MinDepth,
				ZFar = vp.MaxDepth,
			};
			var nextIndex = mBag.Viewports.Push(item);

			mInstructions.Add(new AmtCommandEncoderInstruction
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

		#endregion

		#region SetDepthBias methods

		public void SetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor)
		{
			var item = new AmtDepthBiasEncoderState
			{
				DepthBias = depthBiasConstantFactor,
				Clamp = depthBiasClamp,
				SlopeScale = depthBiasSlopeFactor
			};

			var nextIndex = mBag.DepthBias.Push(item);

			mInstructions.Add(new AmtCommandEncoderInstruction
			{
				Category = AmtCommandEncoderCategory.Graphics,
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

		#endregion

		public void SetStencilReference(MgStencilFaceFlagBits faceMask, UInt32 reference)
		{
			if ((faceMask & MgStencilFaceFlagBits.FRONT_AND_BACK) == MgStencilFaceFlagBits.FRONT_AND_BACK)
			{
				var item = new AmtStencilReferenceEncoderState
				{
					Front = reference,
					Back = reference,
				};
				var nextIndex = mBag.StencilReferences.Push(item);

				mInstructions.Add(new AmtCommandEncoderInstruction
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
				var nextIndex = mBag.StencilReferences.Push(item);
				mInstructions.Add(new AmtCommandEncoderInstruction
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
				var nextIndex = mBag.StencilReferences.Push(item);
				mInstructions.Add(new AmtCommandEncoderInstruction
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



		public void Clear()
		{
			mBag.Clear();
			mCurrentRenderPass = null;
		}

		public void SetScissor(uint firstScissor, MgRect2D[] pScissors)
		{
			throw new NotImplementedException();
		}

		public void SetStencilCompareMask(MgStencilFaceFlagBits faceMask, uint compareMask)
		{
			throw new NotImplementedException();
		}

		public void SetStencilWriteMask(MgStencilFaceFlagBits faceMask, uint writeMask)
		{
			throw new NotImplementedException();
		}

		#region EndRenderPass methods

		public void EndRenderPass()
		{
			mInstructions.Add(new AmtCommandEncoderInstruction
			{
				Category = AmtCommandEncoderCategory.Graphics,
				Index = 0,
				Operation = CmdEndRenderPass,
			});
		             
		}

		private static void CmdEndRenderPass(AmtCommandRecording recording, uint index)
		{
			var stage = recording.Graphics;
			Debug.Assert(stage.Encoder != null, nameof(stage.Encoder) + " is null");
			stage.Encoder.EndEncoding();
			stage.Encoder = null;
		}

		#endregion


	}
}
