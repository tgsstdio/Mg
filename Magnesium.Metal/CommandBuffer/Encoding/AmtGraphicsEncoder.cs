using System;
using System.Collections.Generic;
using System.Diagnostics;
using Metal;

namespace Magnesium.Metal
{
	public class AmtGraphicsEncoder : IAmtGraphicsEncoder
	{
		private AmtGraphicsBag mBag;
		private IMTLDevice mDevice;

		private readonly IAmtEncoderContextSorter mInstructions;

		public AmtGraphicsEncoder(IAmtEncoderContextSorter instructions, IMTLDevice device, AmtGraphicsBag bag)
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

			const uint FIRST_SUBPASS = 0;
			var descriptor = InitializeDescriptor(FIRST_SUBPASS, bRenderPass, pRenderPassBegin.ClearValues);

			var nextIndex = mBag.RenderPasses.Push(descriptor);
			mInstructions.Add(new AmtEncodingInstruction
			{
				Category = AmtEncoderCategory.Graphics,
				Index = nextIndex,
				Operation = CmdSetBeginRenderPass,
			});

			if (mBoundVertexBuffer != null)
			{
				mInstructions.Add(mBoundVertexBuffer);
			}
		}

		static void CmdSetBeginRenderPass(AmtCommandRecording recording, uint index)
		{
			Debug.Assert(recording != null, nameof(recording) + " is null");
			var cmdBuf = recording.CommandBuffer;
			Debug.Assert(cmdBuf != null, nameof(recording.CommandBuffer) + " is null");
			var stage = recording.Graphics;
			Debug.Assert(stage != null, nameof(stage) + " is null");
			Debug.Assert(stage.Grid != null, nameof(stage.Grid) + " is null");
			var item = stage.Grid.RenderPasses[index];
			stage.Encoder = cmdBuf.CreateRenderCommandEncoder(item);
		}

		static MTLRenderPassDescriptor InitializeDescriptor(uint subpassIndex, AmtRenderPass renderPass, MgClearValue[] clearValues)
		{
			var dest = new MTLRenderPassDescriptor { };

			var subpass = renderPass.Subpasses[subpassIndex];

			foreach (var attachment in subpass.ColorAttachments)
			{
				var clearValue = clearValues[attachment.Index];

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

				dest.ColorAttachments[(nint)attachment.Index].ClearColor = new MTLClearColor(r, g, b, a);
			}

			if (subpass.DepthStencil != null)
			{
				var depthValue = clearValues[subpass.DepthStencil.Index];
				dest.DepthAttachment.ClearDepth = depthValue.DepthStencil.Depth;
				dest.StencilAttachment.ClearStencil = depthValue.DepthStencil.Stencil;
			}

			return dest;
		}

		#endregion

		#region BindPipeline methods

		private uint mFrontReference;
		private uint mBackReference;

		private AmtGraphicsPipeline mCurrentPipeline;
		public void BindPipeline(IMgPipeline pipeline)
		{
			Debug.Assert(pipeline != null);

			var bPipeline = (AmtGraphicsPipeline)pipeline;
			mCurrentPipeline = bPipeline;

			// FIXME : SHOULD BE DELAYED PIPELINE CREATION TO EVERY DRAW CALLS TO CORRECTLY SIMULATE DYNAMIC STATES
			var pipelineDescriptor = new MTLRenderPipelineDescriptor
			{
				VertexFunction = bPipeline.VertexFunction,
				FragmentFunction = bPipeline.FragmentFunction,
				VertexDescriptor = bPipeline.GetVertexDescriptor(),
				AlphaToCoverageEnabled = bPipeline.AlphaToCoverageEnabled,
				RasterizationEnabled = !bPipeline.RasterizationDiscardEnabled,
				AlphaToOneEnabled = bPipeline.AlphaToOneEnabled

			};

			var currentSubpass = 0;
			if (mCurrentRenderPass != null)
			{
				mCurrentRenderPass.Subpasses[currentSubpass].InitializeFormat(pipelineDescriptor);
			}
			else
			{
				mCurrentPipeline.RenderPass.Subpasses[currentSubpass].InitializeFormat(pipelineDescriptor);
			}

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

			SetupDepthStencil();

			float depthBiasConstantFactor = mCurrentPipeline.DepthBiasConstantFactor;
			float depthBiasSlopeFactor = mCurrentPipeline.SlopeScale;
			float depthBiasClamp = mCurrentPipeline.Clamp;
			if (
				(mCurrentPipeline.DynamicStates & AmtGraphicsPipelineDynamicStateFlagBits.DEPTH_BIAS)
					== AmtGraphicsPipelineDynamicStateFlagBits.DEPTH_BIAS
				)
			{
				depthBiasConstantFactor = mDepthBiasConstantFactor;
				depthBiasSlopeFactor = mDepthBiasSlopeFactor;
				depthBiasClamp = mDepthBiasClamp;
			}

			var blendColors = mCurrentPipeline.BlendConstants;
			if (
				(mCurrentPipeline.DynamicStates & AmtGraphicsPipelineDynamicStateFlagBits.BLEND_CONSTANTS)
					== AmtGraphicsPipelineDynamicStateFlagBits.STENCIL_REFERENCE
				)
			{
				blendColors = mBlendConstants;
			}

			var viewport = mViewport;
			if (mCurrentPipeline.Viewport.HasValue)
			{
				viewport = mCurrentPipeline.Viewport.Value;
			}

			var scissorRect = mScissor;
			if (mCurrentPipeline.ScissorRect.HasValue)
			{
				scissorRect = mCurrentPipeline.ScissorRect.Value;
			}

			var pipeDetail = new AmtPipelineStateRecord
			{
				PipelineState = pipelineState,
				Viewport = viewport,
				CullMode = bPipeline.CullMode,
				BlendConstants = blendColors,
				Scissor = scissorRect,
				Winding = bPipeline.Winding,
				DepthBiasClamp = depthBiasClamp,
				DepthBiasSlopeScale = depthBiasSlopeFactor,
				DepthBiasConstantFactor = depthBiasConstantFactor,
			};
			var nextIndex = mBag.PipelineStates.Push(pipeDetail);

			mInstructions.Add(new AmtEncodingInstruction
			{
				Category = AmtEncoderCategory.Graphics,
				Index = (uint)nextIndex,
				Operation = CmdSetPipeline,
			});

		}

		private static void CmdSetPipeline(AmtCommandRecording recording, uint index)
		{
			Debug.Assert(recording != null, nameof(recording) + " is null");
			var stage = recording.Graphics;
			Debug.Assert(stage != null, nameof(stage) + " is null");
			Debug.Assert(stage.Encoder != null, nameof(stage.Encoder) + " is null");
			Debug.Assert(stage.Grid != null, nameof(stage.Grid) + " is null");
			var arg1 = stage.Encoder;

			var item = stage.Grid.Pipelines[index];
			arg1.SetRenderPipelineState(item.PipelineState);
			//arg1.SetDepthStencilState(item.DepthStencilState);
			arg1.SetViewport(item.Viewport);
			arg1.SetCullMode(item.CullMode);
			arg1.SetScissorRect(item.Scissor);
			arg1.SetTriangleFillMode(item.FillMode);
			arg1.SetFrontFacingWinding(item.Winding);
			var color = item.BlendConstants;
			arg1.SetBlendColor(color.R, color.G, color.B, color.A);
			arg1.SetDepthBias(item.DepthBiasConstantFactor, item.DepthBiasSlopeScale, item.DepthBiasClamp);

		}


		uint mFrontCompare;
		uint mBackCompare;

		uint mFrontWrite;
		uint mBackWrite;

		void SetupDepthStencil()
		{
			var frontCompareMask = mCurrentPipeline.FrontStencil.ReadMask;
			var backCompareMask = mCurrentPipeline.BackStencil.ReadMask;


			// ONLY if pipeline ATTACHED and dynamic state has been set
			if (
				(mCurrentPipeline.DynamicStates & AmtGraphicsPipelineDynamicStateFlagBits.STENCIL_COMPARE_MASK)
					== AmtGraphicsPipelineDynamicStateFlagBits.STENCIL_COMPARE_MASK
				)
			{
				frontCompareMask = mFrontCompare;
				backCompareMask = mBackCompare;
			}

			var frontWriteMask = mCurrentPipeline.FrontStencil.WriteMask;
			var backWriteMask = mCurrentPipeline.BackStencil.WriteMask;

			// ONLY if pipeline ATTACHED and dynamic state has been set
			if (
				(mCurrentPipeline.DynamicStates & AmtGraphicsPipelineDynamicStateFlagBits.STENCIL_WRITE_MASK)
					== AmtGraphicsPipelineDynamicStateFlagBits.STENCIL_WRITE_MASK
				)
			{
				frontCompareMask = mFrontWrite;
				backCompareMask = mBackWrite;
			}

			var dsDescriptor = new MTLDepthStencilDescriptor
			{ 
				// TODO : add this back in later
				//DepthCompareFunction = mCurrentPipeline.DepthCompareFunction,
				DepthWriteEnabled = mCurrentPipeline.DepthWriteEnabled,
				BackFaceStencil = mCurrentPipeline.BackStencil.GetDescriptor(),
				FrontFaceStencil = mCurrentPipeline.FrontStencil.GetDescriptor(),
			};
			dsDescriptor.FrontFaceStencil.ReadMask = frontCompareMask;
			dsDescriptor.FrontFaceStencil.WriteMask = frontWriteMask;
			dsDescriptor.BackFaceStencil.ReadMask = backCompareMask;
			dsDescriptor.BackFaceStencil.WriteMask = backWriteMask;

			var depthStencil = mDevice.CreateDepthStencilState(dsDescriptor);

			var frontReference = mCurrentPipeline.FrontStencilReference;
			var backReference = mCurrentPipeline.BackStencilReference;

			// ONLY if pipeline ATTACHED and dynamic state has been set
			if (
				(mCurrentPipeline.DynamicStates & AmtGraphicsPipelineDynamicStateFlagBits.STENCIL_REFERENCE)
					== AmtGraphicsPipelineDynamicStateFlagBits.STENCIL_REFERENCE
			   )
			{
				frontReference = mFrontReference;
				backReference = mBackReference;
			}

			var item = new AmtDepthStencilStateRecord
			{
				DepthStencilState = depthStencil,
				FrontReference = frontReference,
				BackReference = backReference,
			};

			var nextIndex = mBag.DepthStencilStates.Push(item);
			mInstructions.Add(new AmtEncodingInstruction
			{
				Category = AmtEncoderCategory.Graphics,
				Index = nextIndex,
				Operation = CmdSetDepthStencilState,
			});
		}

		private static void CmdSetDepthStencilState(AmtCommandRecording recording, uint index)
		{
			Debug.Assert(recording != null, nameof(recording) + " is null");
			var stage = recording.Graphics;
			Debug.Assert(stage != null, nameof(stage) + " is null");
			Debug.Assert(stage.Encoder != null, nameof(stage.Encoder) + " is null");
			Debug.Assert(stage.Grid != null, nameof(stage.Grid) + " is null");
			var item = stage.Grid.DepthStencilStates[index];
			stage.Encoder.SetDepthStencilState(item.DepthStencilState);
			stage.Encoder.SetStencilFrontReferenceValue(item.FrontReference, item.BackReference);
		}

		#endregion

		#region SetBlendConstants methods

		private MgColor4f mBlendConstants;
		public void SetBlendConstants(MgColor4f color)
		{

			mBlendConstants = color;
			// ONLY if 
			// no pipeline has been set
			// OR pipeline ATTACHED and dynamic state has been set
			if
			(
				(mCurrentPipeline == null)
				||
				(mCurrentPipeline != null
					&&
					(
						(mCurrentPipeline.DynamicStates & AmtGraphicsPipelineDynamicStateFlagBits.VIEWPORT)
							== AmtGraphicsPipelineDynamicStateFlagBits.VIEWPORT
					)
				)
			)
			{

				var nextIndex = mBag.BlendConstants.Push(color);

				mInstructions.Add(new AmtEncodingInstruction
				{
					Category = AmtEncoderCategory.Graphics,
					Index = (uint)nextIndex,
					Operation = SetCmdBlendColors,
				});
			}

		}

		private static void SetCmdBlendColors(AmtCommandRecording recording, uint index)
		{
			Debug.Assert(recording != null, nameof(recording) + " is null");
			var stage = recording.Graphics;
			Debug.Assert(stage != null, nameof(stage) + " is null");
			Debug.Assert(stage.Encoder != null, nameof(stage.Encoder) + " is null");
			Debug.Assert(stage.Grid != null, nameof(stage.Grid) + " is null");
			var color = stage.Grid.BlendConstants[index];
			stage.Encoder.SetBlendColor(color.R, color.G, color.B, color.A);
		}

		#endregion

		#region SetViewports methods

		private MTLViewport mViewport;
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

			mViewport = item;

			// ONLY if 
			// no pipeline has been set
			// OR pipeline ATTACHED and dynamic state has been set
			if
			(
				(mCurrentPipeline == null)
				||
				(mCurrentPipeline != null
					&&
					(
						(mCurrentPipeline.DynamicStates & AmtGraphicsPipelineDynamicStateFlagBits.VIEWPORT)
							== AmtGraphicsPipelineDynamicStateFlagBits.VIEWPORT
					)
				)
			)
			{

				var nextIndex = mBag.Viewports.Push(item);

				mInstructions.Add(new AmtEncodingInstruction
				{
					Category = AmtEncoderCategory.Graphics,
					Index = nextIndex,
					Operation = CmdSetViewport,

				});
			}

		}

		static void CmdSetViewport(AmtCommandRecording recording, uint index)
		{
			Debug.Assert(recording != null, nameof(recording) + " is null");
			var stage = recording.Graphics;
			Debug.Assert(stage != null, nameof(stage) + " is null");
			Debug.Assert(stage.Encoder != null, nameof(stage.Encoder) + " is null");
			Debug.Assert(stage.Grid != null, nameof(stage.Grid) + " is null");
			var item = stage.Grid.Viewports[index];
			stage.Encoder.SetViewport(item);
		}

		#endregion

		#region SetDepthBias methods

		float mDepthBiasConstantFactor;

		float mDepthBiasClamp;

		float mDepthBiasSlopeFactor;

		public void SetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor)
		{
			mDepthBiasConstantFactor = depthBiasConstantFactor;
			mDepthBiasClamp = depthBiasClamp;
			mDepthBiasSlopeFactor = depthBiasSlopeFactor;

			// ONLY if 
			// no pipeline has been set
			// OR pipeline ATTACHED and dynamic state has been set
			if
			(
				(mCurrentPipeline == null)
				||
				(mCurrentPipeline != null
					&&
					(
						(mCurrentPipeline.DynamicStates & AmtGraphicsPipelineDynamicStateFlagBits.DEPTH_BIAS)
							== AmtGraphicsPipelineDynamicStateFlagBits.DEPTH_BIAS
					)
				)
			)
			{
				var item = new AmtDepthBiasRecord
				{
					DepthBias = depthBiasConstantFactor,
					Clamp = depthBiasClamp,
					SlopeScale = depthBiasSlopeFactor
				};

				var nextIndex = mBag.DepthBias.Push(item);

				mInstructions.Add(new AmtEncodingInstruction
				{
					Category = AmtEncoderCategory.Graphics,
					Index = nextIndex,
					Operation = SetCmdDepthBias,
				});
			}
		}

		private static void SetCmdDepthBias(AmtCommandRecording recording, uint index)
		{
			Debug.Assert(recording != null, nameof(recording) + " is null");
			var stage = recording.Graphics;
			Debug.Assert(stage != null, nameof(stage) + " is null");
			Debug.Assert(stage.Encoder != null, nameof(stage.Encoder) + " is null");
			Debug.Assert(stage.Grid != null, nameof(stage.Grid) + " is null");
			var item = stage.Grid.DepthBias[index];
			stage.Encoder.SetDepthBias(item.DepthBias, item.SlopeScale, item.Clamp);
		}

		#endregion

		#region SetStencilReference methods

		public void SetStencilReference(MgStencilFaceFlagBits faceMask, UInt32 reference)
		{
			if ((faceMask & MgStencilFaceFlagBits.FRONT_AND_BACK) == MgStencilFaceFlagBits.FRONT_AND_BACK)
			{

				mBackReference = reference;
				mFrontReference = reference;
			}
			else if ((faceMask & MgStencilFaceFlagBits.BACK_BIT) == MgStencilFaceFlagBits.BACK_BIT)
			{
				mBackReference = reference;
			}
			else if ((faceMask & MgStencilFaceFlagBits.FRONT_BIT) == MgStencilFaceFlagBits.FRONT_BIT)
			{
				mBackReference = reference;
			}

			// ONLY if 
			// no pipeline has been set
			// OR pipeline ATTACHED and dynamic state has been set
			if
			(
				(mCurrentPipeline == null)
				||
				(mCurrentPipeline != null
					&&
					(
						(mCurrentPipeline.DynamicStates & AmtGraphicsPipelineDynamicStateFlagBits.STENCIL_REFERENCE)
							== AmtGraphicsPipelineDynamicStateFlagBits.STENCIL_REFERENCE
					)
				)
			)
			{
				var item = new AmtStencilReferenceRecord
				{
					Front = mFrontReference,
					Back = mBackReference,
				};
				var nextIndex = mBag.StencilReferences.Push(item);

				mInstructions.Add(new AmtEncodingInstruction
				{
					Category = AmtEncoderCategory.Graphics,
					Index = nextIndex,
					Operation = CmdSetStencilReference,
				});
			}
		}

		private static void CmdSetStencilReference(AmtCommandRecording recording, uint index)
		{
			Debug.Assert(recording != null, nameof(recording) + " is null");
			var stage = recording.Graphics;
			Debug.Assert(stage != null, nameof(stage) + " is null");
			Debug.Assert(stage.Encoder != null, nameof(stage.Encoder) + " is null");
			Debug.Assert(stage.Grid != null, nameof(stage.Grid) + " is null");
			var item = stage.Grid.StencilReferences[index];
			stage.Encoder.SetStencilFrontReferenceValue(item.Front, item.Back);
		}

		#endregion

		public void Clear()
		{
			mBag.Clear();
			mCurrentPipeline = null;
			mCurrentRenderPass = null;
			mIndexBuffer = null;
			mFrontCompare = ~0U;
			mBackCompare = ~0U;
			mFrontWrite = ~0U;
			mBackWrite = ~0U;
			mFrontReference = 0U;
			mBackReference = 0U;
			mDepthBiasClamp = 0f;
			mDepthBiasSlopeFactor = 0f;
			mDepthBiasConstantFactor = 0f;
			mBoundVertexBuffer = null;
		}

		#region SetScissor methods

		public void SetScissor(uint firstScissor, MgRect2D[] pScissors)
		{
			if (firstScissor != 0)
				throw new ArgumentOutOfRangeException(nameof(firstScissor) + " != 0");

			var scissor = pScissors[0];

			if (scissor.Offset.X < 0)
				throw new ArgumentOutOfRangeException(nameof(pScissors) + "[0].X must be >= 0");

			if (scissor.Offset.Y < 0)
				throw new ArgumentOutOfRangeException(nameof(pScissors) + "[0].Y must be >= 0");

			var item = new MTLScissorRect
			{
				X = (nuint)scissor.Offset.X,
				Y = (nuint)scissor.Offset.Y,
				Width = (nuint)scissor.Extent.Width,
				Height = (nuint)scissor.Extent.Height
			};

			mScissor = item;

			// ONLY if 
			// no pipeline has been set
			// OR pipeline ATTACHED and dynamic state has been set
			if
			(
				(mCurrentPipeline == null)
				||
				(mCurrentPipeline != null
					&&
					(
						(mCurrentPipeline.DynamicStates & AmtGraphicsPipelineDynamicStateFlagBits.SCISSOR)
							== AmtGraphicsPipelineDynamicStateFlagBits.SCISSOR
					)
				)
			)
			{

				var nextIndex = mBag.Scissors.Push(item);
				mInstructions.Add(new AmtEncodingInstruction
				{
					Category = AmtEncoderCategory.Graphics,
					Index = nextIndex,
					Operation = CmdSetScissor,
				});
			}

		}

		private MTLScissorRect mScissor;

		private static void CmdSetScissor(AmtCommandRecording recording, uint index)
		{
			Debug.Assert(recording != null, nameof(recording) + " is null");
			var stage = recording.Graphics;
			Debug.Assert(stage != null, nameof(stage) + " is null");
			Debug.Assert(stage.Encoder != null, nameof(stage.Encoder) + " is null");
			Debug.Assert(stage.Grid != null, nameof(stage.Grid) + " is null");
			var item = stage.Grid.Scissors[index];
			stage.Encoder.SetScissorRect(item);
		}

		#endregion

		#region SetStencilCompareMask methods

		public void SetStencilCompareMask(MgStencilFaceFlagBits faceMask, uint compareMask)
		{
			if ((faceMask & MgStencilFaceFlagBits.FRONT_AND_BACK) == MgStencilFaceFlagBits.FRONT_AND_BACK)
			{
				mFrontCompare = compareMask;
				mBackCompare = compareMask;
			}
			else if ((faceMask & MgStencilFaceFlagBits.BACK_BIT) == MgStencilFaceFlagBits.BACK_BIT)
			{
				mBackCompare = compareMask;
			}
			else if ((faceMask & MgStencilFaceFlagBits.FRONT_BIT) == MgStencilFaceFlagBits.FRONT_BIT)
			{
				mFrontCompare = compareMask;
			}

			// ONLY if 
			//  pipeline ATTACHED and dynamic state has been set
			if
			(
				(mCurrentPipeline != null
					&&
					(
						(mCurrentPipeline.DynamicStates & AmtGraphicsPipelineDynamicStateFlagBits.STENCIL_COMPARE_MASK)
						== AmtGraphicsPipelineDynamicStateFlagBits.STENCIL_COMPARE_MASK
					)
				)
			)
			{
				SetupDepthStencil();
			}

		}

		#endregion

		#region SetStencilWriteMask methods

		public void SetStencilWriteMask(MgStencilFaceFlagBits faceMask, uint writeMask)
		{
			if ((faceMask & MgStencilFaceFlagBits.FRONT_AND_BACK) == MgStencilFaceFlagBits.FRONT_AND_BACK)
			{
				mFrontWrite = writeMask;
				mBackWrite = writeMask;
			}
			else if ((faceMask & MgStencilFaceFlagBits.BACK_BIT) == MgStencilFaceFlagBits.BACK_BIT)
			{
				mBackWrite = writeMask;
			}
			else if ((faceMask & MgStencilFaceFlagBits.FRONT_BIT) == MgStencilFaceFlagBits.FRONT_BIT)
			{
				mFrontWrite = writeMask;
			}

			// ONLY if 
			//  pipeline ATTACHED and dynamic state has been set
			if
			(
				(mCurrentPipeline != null
					&&
					(
						(mCurrentPipeline.DynamicStates & AmtGraphicsPipelineDynamicStateFlagBits.STENCIL_WRITE_MASK)
							== AmtGraphicsPipelineDynamicStateFlagBits.STENCIL_WRITE_MASK
					)
				)
			)
			{
				SetupDepthStencil();
			}
		}

		#endregion

		#region EndRenderPass methods

		public void EndRenderPass()
		{
			mInstructions.Add(new AmtEncodingInstruction
			{
				Category = AmtEncoderCategory.Graphics,
				Index = 0,
				Operation = CmdEndRenderPass,
			});

		}

		private static void CmdEndRenderPass(AmtCommandRecording recording, uint index)
		{
			Debug.Assert(recording != null, nameof(recording) + " is null");
			var stage = recording.Graphics;
			Debug.Assert(stage != null, nameof(stage) + " is null");
			Debug.Assert(stage.Encoder != null, nameof(stage.Encoder) + " is null");
			Debug.Assert(stage.Grid != null, nameof(stage.Grid) + " is null");
			stage.Encoder.EndEncoding();
			stage.Encoder = null;
		}

		#endregion

		#region DrawIndexed and DrawIndexedIndirect methods

		public void DrawIndexed(uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance)
		{
			if (mCurrentRenderPass == null)
				return;

			if (mCurrentPipeline == null)
				return;

			if (mIndexBuffer == null)
				return;

			var item = new AmtDrawIndexedRecord
			{
				PrimitiveType = mCurrentPipeline.Topology,
				IndexCount = indexCount,
				IndexType = mIndexType,
				IndexBuffer = mIndexBuffer.VertexBuffer,
				BufferOffset = mBufferOffset,
				InstanceCount = instanceCount,
				VertexOffset = vertexOffset,
				FirstInstance = firstInstance,
			};

			var nextIndex = mBag.DrawIndexeds.Push(item);

			mInstructions.Add(new AmtEncodingInstruction
			{
				Category = AmtEncoderCategory.Graphics,
				Index = nextIndex,
				Operation = CmdDrawIndexed,
			});
		}

		static void CmdDrawIndexed(AmtCommandRecording recording, uint index)
		{
			Debug.Assert(recording != null, nameof(recording) + " is null");
			var stage = recording.Graphics;
			Debug.Assert(stage != null, nameof(stage) + " is null");
			Debug.Assert(stage.Encoder != null, nameof(stage.Encoder) + " is null");
			Debug.Assert(stage.Grid != null, nameof(stage.Grid) + " is null");
			var item = stage.Grid.DrawIndexed[index];

			stage.Encoder.DrawIndexedPrimitives(
				item.PrimitiveType,
				item.IndexCount,
				item.IndexType,
				item.IndexBuffer,
				item.BufferOffset,
				item.InstanceCount,
				item.VertexOffset,
				item.FirstInstance);
		}

		private AmtBuffer mIndexBuffer;
		private nuint mBufferOffset;
		private MTLIndexType mIndexType;
		public void BindIndexBuffer(IMgBuffer buffer, ulong offset, MgIndexType indexType)
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));

			var bBuffer = (AmtBuffer)buffer;

			if (offset > nuint.MaxValue)
				throw new ArgumentOutOfRangeException(nameof(offset) + " must be <= nuint.MaxValue");

			mIndexType = TranslateIndexType(indexType);

			mIndexBuffer = bBuffer;

			mBufferOffset = (nuint)offset;

		}

		static MTLIndexType TranslateIndexType(MgIndexType indexType)
		{
			switch (indexType)
			{
				default:
					throw new NotSupportedException();
				case MgIndexType.UINT16:
					return MTLIndexType.UInt16;
				case MgIndexType.UINT32:
					return MTLIndexType.UInt32;
			}
		}

		public void DrawIndexedIndirect(IMgBuffer buffer, ulong offset, uint drawCount, uint stride)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException(nameof(buffer));
			}

			if (offset > nuint.MaxValue)
			{
				throw new ArgumentOutOfRangeException(nameof(offset) + " must be <= nuint.MaxValue");
			}

			if (stride % 4 != 0)
			{
				throw new ArgumentException("stride must be multiple of 4 bytes");
			}

			if (mCurrentPipeline == null)
				return;

			if (mIndexBuffer == null)
				return;

			var bIndirectBuffer = (AmtBuffer)buffer;

			var item = new AmtDrawIndexedIndirectRecord
			{
				DrawCount = drawCount,
				Stride = (nuint)stride,
				PrimitiveType = mCurrentPipeline.Topology,
				IndexType = mIndexType,
				IndexBuffer = mIndexBuffer.VertexBuffer,
				IndexBufferOffset = mBufferOffset,
				IndirectBuffer = bIndirectBuffer.VertexBuffer,
				IndirectBufferOffset = (nuint)offset,
			};

			var nextIndex = mBag.DrawIndexedIndirects.Push(item);

			mInstructions.Add(new AmtEncodingInstruction
			{
				Category = AmtEncoderCategory.Graphics,
				Index = nextIndex,
				Operation = CmdDrawIndexedIndirect,
			});

		}

		static void CmdDrawIndexedIndirect(AmtCommandRecording recording, uint index)
		{
			Debug.Assert(recording != null, nameof(recording) + " is null");
			var stage = recording.Graphics;
			Debug.Assert(stage != null, nameof(stage) + " is null");
			Debug.Assert(stage.Encoder != null, nameof(stage.Encoder) + " is null");
			Debug.Assert(stage.Grid != null, nameof(stage.Grid) + " is null");
			var item = stage.Grid.DrawIndexedIndirects[index];

			var drawCount = item.DrawCount;
			var stride = item.Stride;

			nuint offset = item.IndirectBufferOffset;
			for (var i = 0; i < drawCount; ++i)
			{
				stage.Encoder.DrawIndexedPrimitives(
					primitiveType: item.PrimitiveType,
					indexType: item.IndexType,
					indexBuffer: item.IndexBuffer,
					indexBufferOffset: item.IndexBufferOffset,
					indirectBuffer: item.IndirectBuffer,
					indirectBufferOffset: offset
				);
				offset += stride;
			}
		}

		#endregion

		#region CmdDraw methods

		public void Draw(uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance)
		{
			if (mCurrentPipeline == null)
				return;

			var item = new AmtDrawRecord
			{
				PrimitiveType = mCurrentPipeline.Topology,
				VertexCount = vertexCount,
				InstanceCount = instanceCount,
				FirstVertex = firstVertex,
				FirstInstance = firstInstance,
			};

			var nextIndex = mBag.Draws.Push(item);
			mInstructions.Add(new AmtEncodingInstruction
			{
				Category = AmtEncoderCategory.Graphics,
				Index = nextIndex,
				Operation = CmdDraw,
			});

		}

		private static void CmdDraw(AmtCommandRecording recording, uint index)
		{
			Debug.Assert(recording != null, nameof(recording) + " is null");
			var stage = recording.Graphics;
			Debug.Assert(stage != null, nameof(stage) + " is null");
			Debug.Assert(stage.Encoder != null, nameof(stage.Encoder) + " is null");
			Debug.Assert(stage.Grid != null, nameof(stage.Grid) + " is null");
			var item = stage.Grid.Draws[index];
			stage.Encoder.DrawPrimitives(item.PrimitiveType, item.FirstVertex, item.VertexCount, item.InstanceCount, item.FirstInstance);
		}

		#endregion

		#region DrawIndirect methods

		public void DrawIndirect(IMgBuffer buffer, ulong offset, uint drawCount, uint stride)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException(nameof(buffer));
			}

			if (offset > nuint.MaxValue)
			{
				throw new ArgumentOutOfRangeException(nameof(offset) + " must be <= nuint.MaxValue");
			}

			if (stride % 4 != 0)
			{
				throw new ArgumentException("stride must be multiple of 4 bytes");
			}

			if (mCurrentPipeline == null)
				return;

			var bIndirectBuffer = (AmtBuffer)buffer;

			var item = new AmtDrawIndirectRecord
			{
				PrimitiveType = mCurrentPipeline.Topology,
				DrawCount = drawCount,
				IndirectBuffer = bIndirectBuffer.VertexBuffer,
				IndirectBufferOffset = (nuint)offset,
				Stride = (nuint)stride,
			};

			var nextIndex = mBag.DrawIndirects.Push(item);
			mInstructions.Add(new AmtEncodingInstruction
			{
				Category = AmtEncoderCategory.Graphics,
				Index = nextIndex,
				Operation = CmdDrawIndirect,
			});

		}

		private static void CmdDrawIndirect(AmtCommandRecording recording, uint index)
		{
			Debug.Assert(recording != null, nameof(recording) + " is null");
			var stage = recording.Graphics;
			Debug.Assert(stage != null, nameof(stage) + " is null");
			Debug.Assert(stage.Encoder != null, nameof(stage.Encoder) + " is null");
			Debug.Assert(stage.Grid != null, nameof(stage.Grid) + " is null");
			var item = stage.Grid.DrawIndirects[index];

			var drawCount = item.DrawCount;
			var stride = item.Stride;

			nuint offset = item.IndirectBufferOffset;
			for (var i = 0; i < drawCount; ++i)
			{
				stage.Encoder.DrawPrimitives(
					item.PrimitiveType,
					item.IndirectBuffer,
					offset);
				offset += stride;
			}
		}

		#endregion

		#region BindVertexBuffers methods

		AmtEncodingInstruction mBoundVertexBuffer;

		public void BindVertexBuffers(uint firstBinding, IMgBuffer[] pBuffers, ulong[] pOffsets)
		{
			if (pBuffers == null)
				throw new ArgumentNullException(nameof(pBuffers));

			var count = pBuffers.Length;

			var buffers = new AmtVertexBufferBindingRecord[count];
			for (var i = 0; i < count; ++i)
			{
				var offset = (nuint)pOffsets[i];
				var bBuffer = (AmtBuffer)pBuffers[i];

				buffers[i] = new AmtVertexBufferBindingRecord
				{
					VertexBuffer = bBuffer.VertexBuffer,
					VertexOffset = offset,
				};
			}

			var item = new AmtVertexBufferRecord
			{
				FirstBinding = firstBinding,
				Bindings = buffers,
			};

			var nextIndex = mBag.VertexBuffers.Push(item);

			mBoundVertexBuffer = new AmtEncodingInstruction
			{
				Category = AmtEncoderCategory.Graphics,
				Index = nextIndex,
				Operation = CmdBindVertexBuffers,
			};

			if (mCurrentRenderPass != null)
			{
				mInstructions.Add(mBoundVertexBuffer);
			}
		}

		private static void CmdBindVertexBuffers(AmtCommandRecording recording, uint index)
		{
			var stage = recording.Graphics;
			Debug.Assert(stage.Encoder != null, nameof(stage.Encoder) + " is null");
			Debug.Assert(stage.Grid != null, nameof(stage.Grid) + " is null");
			var item = stage.Grid.VertexBuffers[index];

			var arrayIndex = item.FirstBinding;
			var count = item.Bindings.Length;
			for (var i = 0; i < count; ++i)
			{
				var binding = item.Bindings[i];
				stage.Encoder.SetVertexBuffer(binding.VertexBuffer, binding.VertexOffset, arrayIndex);
				++arrayIndex;
			}
		}

		#endregion

		public AmtGraphicsGrid AsGrid()
		{
			return new AmtGraphicsGrid
			{
				BlendConstants = mBag.BlendConstants.ToArray(),
				DepthBias = mBag.DepthBias.ToArray(),
				DepthStencilStates = mBag.DepthStencilStates.ToArray(),
				DrawIndexed = mBag.DrawIndexeds.ToArray(),
				DrawIndexedIndirects = mBag.DrawIndexedIndirects.ToArray(),
				DrawIndirects = mBag.DrawIndirects.ToArray(),
				Draws = mBag.Draws.ToArray(),
				Pipelines = mBag.PipelineStates.ToArray(),
				RenderPasses = mBag.RenderPasses.ToArray(),
				Scissors = mBag.Scissors.ToArray(),
				StencilReferences = mBag.StencilReferences.ToArray(),
				VertexBuffers = mBag.VertexBuffers.ToArray(),
			};
		}
	}
}
