using System;
using System.Collections.Generic;
using Metal;

namespace Magnesium.Metal
{
	public class AmtComputeEncoder : IAmtComputeEncoder
	{
		private List<AmtCommandEncoderInstruction> mInstructions;
		private AmtComputeEncoderItemBag mBag;

		IMTLDevice mDevice;

		public AmtComputeEncoder(List<AmtCommandEncoderInstruction> instructions, IMTLDevice device, AmtComputeEncoderItemBag bag)
		{
			mInstructions = instructions;
			mDevice = device;
			mBag = bag;
		}

		private AmtComputePipeline mCurrentPipeline; 
		public void BindPipeline(IMgPipeline pipeline)
		{
			if (pipeline == null)
				throw new ArgumentNullException(nameof(pipeline));
			mCurrentPipeline = (AmtComputePipeline) pipeline;

			var nextIndex = mBag.Pipelines.Push(mCurrentPipeline);
			mInstructions.Add(new AmtCommandEncoderInstruction
			{
				Category = AmtCommandEncoderCategory.Compute,
				Index = nextIndex,
				Operation = CmdBindPipeline,
			});
		}


		private static void CmdBindPipeline(AmtCommandRecording recording, uint index)
		{
			var stage = recording.Compute;
			var item = stage.Grid.Pipelines[index];
			stage.Encoder.SetComputePipelineState(item.Compute);
		}

		public void Clear()
		{
			mBag.Clear();
		}

		public void Dispatch(uint x, uint y, uint z)
		{
			var threadsPerGroupSize = mCurrentPipeline != null ? mCurrentPipeline.ThreadsPerGroupSize : new MTLSize();

			var item = new AmtDispatchEncoderState
			{
				GroupSize = new MTLSize((nint)x, (nint)y, (nint)z),
				ThreadsPerGroupSize = threadsPerGroupSize,
			};

			var nextIndex = mBag.Dispatch.Push(item);

			mInstructions.Add(new AmtCommandEncoderInstruction
			{
				Category = AmtCommandEncoderCategory.Compute,
				Index = nextIndex,
				Operation = CmdDispatch,
			});
		}

		public void CmdDispatch(AmtCommandRecording recording, uint index)
		{
			var stage = recording.Compute;
			var item = stage.Grid.Dispatch[index];
			stage.Encoder.DispatchThreadgroups(item.GroupSize, item.ThreadsPerGroupSize);
		}


		public void DispatchIndirect(IMgBuffer buffer, ulong offset)
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer) + " is null");

			var bBuffer = (AmtBuffer)buffer;
			var indirect = bBuffer.VertexBuffer;

			var threadsPerGroupSize = mCurrentPipeline != null ? mCurrentPipeline.ThreadsPerGroupSize : new MTLSize();

			var valueItem = new AmtDispatchIndirectEncoderState
			{
				IndirectBuffer = indirect,
				Offset = (nuint)offset,
				ThreadsPerGroupSize = threadsPerGroupSize,
			};

			var nextIndex = mBag.DispatchIndirect.Push(valueItem);


			mInstructions.Add(new AmtCommandEncoderInstruction
			{
				Category = AmtCommandEncoderCategory.Compute,
				Index = nextIndex,
				Operation = DispatchIndirect,
			});


		}

		private static void DispatchIndirect(AmtCommandRecording recording, uint index)
		{
			var stage = recording.Compute;
			var item = stage.Grid.DispatchIndirect[index];
			stage.Encoder.DispatchThreadgroups(item.IndirectBuffer, item.Offset, item.ThreadsPerGroupSize);
		}
	}
}
