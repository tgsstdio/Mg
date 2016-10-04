using System;
using System.Collections.Generic;
using System.Diagnostics;
using Metal;

namespace Magnesium.Metal
{
	public class AmtBlitEncoder : IAmtBlitEncoder
	{
		private readonly AmtBlitBag mBag;

		private readonly IAmtEncoderContextSorter mInstructions;


		public AmtBlitEncoder(AmtBlitBag bag, IAmtEncoderContextSorter instructions)
		{
			mBag = bag;
			mInstructions = instructions;
		}

		#region CopyBuffer methods 

		public void CopyBuffer(IMgBuffer srcBuffer, IMgBuffer dstBuffer, MgBufferCopy[] pRegions)
		{
			if (srcBuffer == null)
			{
				throw new ArgumentNullException(nameof(srcBuffer));
			}

			if (dstBuffer == null)
			{
				throw new ArgumentNullException(nameof(dstBuffer));
			}

			if (pRegions == null)
			{
				throw new ArgumentNullException(nameof(pRegions));
			}

			var bSrcBuffer = (AmtBuffer)srcBuffer;
			var bDstBuffer = (AmtBuffer)dstBuffer;

			var regions = new List<AmtBlitCopyBufferRegionRecord>();
			for (var i = 0; i < pRegions.Length; ++i)
			{
				if (pRegions[i].SrcOffset > nuint.MaxValue)
					throw new ArgumentOutOfRangeException(nameof(pRegions) + "[" + i  +  "].SrcOffset must be less than " + nuint.MaxValue);

				if (pRegions[i].DstOffset > nuint.MaxValue)
					throw new ArgumentOutOfRangeException(nameof(pRegions) + "[" + i + "].DstOffset must be less than " + nuint.MaxValue);

				if (pRegions[i].Size > nuint.MaxValue)
					throw new ArgumentOutOfRangeException(nameof(pRegions) + "[" + i + "].Size must be less than " + nuint.MaxValue);

				regions.Add(new AmtBlitCopyBufferRegionRecord
				{
					Size = (nuint) pRegions[i].Size,
					DestinationOffset = (nuint)pRegions[i].DstOffset,
					SourceOffset = (nuint) pRegions[i].SrcOffset,
				});
			}

			var item = new AmtBlitCopyBufferRecord
			{
				Src = bSrcBuffer.VertexBuffer,
				Dst = bDstBuffer.VertexBuffer,
				Regions = regions.ToArray(),
			};
			var nextIndex = mBag.CopyBuffers.Push(item);
			mInstructions.Add(
				new AmtEncodingInstruction
				{
					Category = AmtEncoderCategory.Blit,
					Index = nextIndex,
					Operation = CmdCopyBuffer,
				}
			);
		}

		private static void CmdCopyBuffer(AmtCommandRecording recording, uint index)
		{
			Debug.Assert(recording != null, nameof(recording) + " is null");
			var stage = recording.Blit;
			Debug.Assert(stage != null, nameof(stage) + " is null");
			Debug.Assert(stage.Encoder != null, nameof(stage.Encoder) + " is null");
			Debug.Assert(stage.Grid != null, nameof(stage.Grid) + " is null");
			AmtBlitCopyBufferRecord item = stage.Grid.CopyBuffers[index];
			Debug.Assert(item.Regions != null, nameof(item.Regions) + " is null");
			foreach (var region in item.Regions)
			{
				stage.Encoder.CopyFromBuffer(item.Src, region.SourceOffset, item.Dst, region.DestinationOffset, region.Size);
			}

		}

		#endregion

		public void Clear()
		{
			mBag.Clear();
		}

		public AmtBlitGrid AsGrid()
		{
			return new AmtBlitGrid
			{
				CopyBuffers = mBag.CopyBuffers.ToArray(),
			};
		}
	}
}
