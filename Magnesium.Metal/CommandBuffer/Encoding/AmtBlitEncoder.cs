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

		#region CopyImageToBuffer methods

		public void CopyImageToBuffer(IMgImage srcImage, MgImageLayout srcImageLayout, IMgBuffer dstBuffer, MgBufferImageCopy[] pRegions)
		{
			if (srcImage == null)
			{
				throw new ArgumentNullException(nameof(srcImage));
			}

			if (dstBuffer == null)
			{
				throw new ArgumentNullException(nameof(dstBuffer));
			}

			if (pRegions == null)
			{
				throw new ArgumentNullException(nameof(pRegions));
			}

			var bSrc = (AmtImage)srcImage;
			var bDst = (AmtBuffer)dstBuffer;

			var regions = new List<AmtBlitCopyBufferToImageRegionRecord>();
			for (var i = 0; i < pRegions.Length; ++i)
			{
				var currentRegion = pRegions[i];

				if (currentRegion.BufferOffset > nuint.MaxValue)
				{
					throw new ArgumentOutOfRangeException(
						"pRegions[" + i + "].BufferOffset must be less than " + nuint.MaxValue);
				}

				if (currentRegion.BufferRowLength > nuint.MaxValue)
				{
					throw new ArgumentOutOfRangeException(
						"pRegions[" + i + "].DstOffset must be less than " + nuint.MaxValue);
				}

				var extent = currentRegion.ImageExtent;

				if (extent.Width > nint.MaxValue)
				{
					throw new ArgumentOutOfRangeException(
						"pRegions[" + i + "].ImageExtent.Width  must be <=" + nint.MaxValue);
				}

				if (extent.Height > nint.MaxValue)
				{
					throw new ArgumentOutOfRangeException(
						"pRegions[" + i + "].ImageExtent.Height must be <= " + nint.MaxValue);
				}

				if (extent.Depth > nint.MaxValue)
				{
					throw new ArgumentOutOfRangeException(
						"pRegions[" + i + "].ImageExtent.Depth must be <= " + nint.MaxValue);
				}

				var sourceImageSize = currentRegion.BufferRowLength * currentRegion.BufferImageHeight;

				if (sourceImageSize > nuint.MaxValue)
				{
					throw new ArgumentOutOfRangeException(
						"sourceImageSize (pRegions[" + i
							+ "].BufferRowLength * BufferImageHeight) must be <= " + nuint.MaxValue);
				}

				regions.Add(new AmtBlitCopyBufferToImageRegionRecord
				{
					BufferOffset = (nuint)currentRegion.BufferOffset,
					BufferBytesPerRow = (nuint)currentRegion.BufferRowLength,
					BufferImageAllocationSize = (nuint)sourceImageSize,
					ImageSize = new MTLSize
					{
						Width = (nint)extent.Width,
						Height = (nint)extent.Height,
						Depth = (nint)extent.Depth,
					},
					BaseArrayLayer = currentRegion.ImageSubresource.BaseArrayLayer,
					ImageMipLevel = currentRegion.ImageSubresource.MipLevel,
					ImageLayerCount = currentRegion.ImageSubresource.LayerCount,
					ImageOffset = new MTLOrigin
					{
						X = currentRegion.ImageOffset.X,
						Y = currentRegion.ImageOffset.Y,
						Z = currentRegion.ImageOffset.Z,
					},
				});
			}

			var item = new AmtBlitCopyBufferToImageRecord
			{
				Image = bSrc.OriginalTexture,
				Buffer = bDst.VertexBuffer,
				Regions = regions.ToArray(),
			};

			var nextIndex = mBag.CopyBufferToImages.Push(item);
			mInstructions.Add(new AmtEncodingInstruction
			{
				Category = AmtEncoderCategory.Blit,
				Index = nextIndex,
				Operation = CmdCopyImageToBuffer,
			});
		}

		private static void CmdCopyImageToBuffer(AmtCommandRecording recording, uint index)
		{
			Debug.Assert(recording != null, nameof(recording) + " is null");
			var stage = recording.Blit;
			Debug.Assert(stage != null, nameof(stage) + " is null");
			Debug.Assert(stage.Encoder != null, nameof(stage.Encoder) + " is null");
			Debug.Assert(stage.Grid != null, nameof(stage.Grid) + " is null");
			AmtBlitCopyBufferToImageRecord item = stage.Grid.CopyBufferToImages[index];
			Debug.Assert(item.Regions != null, nameof(item.Regions) + " is null");
			foreach (var region in item.Regions)
			{
				nuint slice = region.BaseArrayLayer;
				for (var i = 0; i < region.ImageLayerCount; ++i)
				{
					stage.Encoder.CopyFromTexture(
						item.Image,
						slice,
						region.ImageMipLevel,
						region.ImageOffset,
						region.ImageSize,
						item.Buffer,
						region.BufferOffset,
						region.BufferBytesPerRow,
						region.BufferImageAllocationSize
					);
					++slice;
				}
			}
		}

		#endregion

		#region CopyBufferToImage methods

		public void CopyBufferToImage(IMgBuffer srcBuffer,
		                              IMgImage dstImage, MgImageLayout dstImageLayout, MgBufferImageCopy[] pRegions)
		{
			if (srcBuffer == null)
			{
				throw new ArgumentNullException(nameof(srcBuffer));
			}

			if (dstImage == null)
			{
				throw new ArgumentNullException(nameof(dstImage));
			}

			if (pRegions == null)
			{
				throw new ArgumentNullException(nameof(pRegions));
			}

			var bSrcBuffer = (AmtBuffer)srcBuffer;
			var bDstImage = (AmtImage)dstImage;

			var regions = new List<AmtBlitCopyBufferToImageRegionRecord>();
			for (var i = 0; i < pRegions.Length; ++i)
			{
				var currentRegion = pRegions[i];

				if (currentRegion.BufferOffset > nuint.MaxValue)
				{
					throw new ArgumentOutOfRangeException(
						"pRegions[" + i + "].BufferOffset must be less than " + nuint.MaxValue);
				}

				if (currentRegion.BufferRowLength > nuint.MaxValue)
				{
					throw new ArgumentOutOfRangeException(
						"pRegions[" + i + "].DstOffset must be less than " + nuint.MaxValue);
				}

				var extent = currentRegion.ImageExtent;

				if (extent.Width > nint.MaxValue)
				{
					throw new ArgumentOutOfRangeException(
						"pRegions[" + i + "].ImageExtent.Width  must be <=" + nint.MaxValue);
				}

				if (extent.Height > nint.MaxValue)
				{
					throw new ArgumentOutOfRangeException(
						"pRegions[" + i + "].ImageExtent.Height must be <= " + nint.MaxValue);
				}

				if (extent.Depth > nint.MaxValue)
				{
					throw new ArgumentOutOfRangeException(
						"pRegions[" + i + "].ImageExtent.Depth must be <= " + nint.MaxValue);
				}

				var sourceImageSize = currentRegion.BufferRowLength * currentRegion.BufferImageHeight;

				if (sourceImageSize > nuint.MaxValue)
				{
					throw new ArgumentOutOfRangeException(
						"sourceImageSize (pRegions[" + i
							+ "].BufferRowLength * BufferImageHeight) must be <= " + nuint.MaxValue);
				}

				regions.Add(new AmtBlitCopyBufferToImageRegionRecord
				{
					SourceOffset = (nuint)currentRegion.BufferOffset,
					BufferBytesPerRow = (nuint)currentRegion.BufferRowLength,
					BufferImageAllocationSize = (nuint)sourceImageSize,
					ImageSize = new MTLSize
					{
						Width = (nint) extent.Width,
						Height = (nint) extent.Height,
						Depth = (nint)extent.Depth,
					},
					BaseArrayLayer = currentRegion.ImageSubresource.BaseArrayLayer,
					ImageMipLevel = currentRegion.ImageSubresource.MipLevel,
					ImageLayerCount = currentRegion.ImageSubresource.LayerCount,
					ImageOffset = new MTLOrigin
					{
						X = currentRegion.ImageOffset.X,
						Y = currentRegion.ImageOffset.Y,
						Z = currentRegion.ImageOffset.Z,
					},
				});
			}

			var item = new AmtBlitCopyBufferToImageRecord
			{
				Buffer = bSrcBuffer.VertexBuffer,
				Image = bDstImage.OriginalTexture,
				Regions = regions.ToArray(),
			};

			var nextIndex = mBag.CopyBufferToImages.Push(item);
			mInstructions.Add(new AmtEncodingInstruction
			{
				Category = AmtEncoderCategory.Blit,
				Index = nextIndex,
				Operation = CmdCopyBufferToImage,
			});
		}

		private static void CmdCopyBufferToImage(AmtCommandRecording recording, uint index)
		{
			Debug.Assert(recording != null, nameof(recording) + " is null");
			var stage = recording.Blit;
			Debug.Assert(stage != null, nameof(stage) + " is null");
			Debug.Assert(stage.Encoder != null, nameof(stage.Encoder) + " is null");
			Debug.Assert(stage.Grid != null, nameof(stage.Grid) + " is null");
			AmtBlitCopyBufferToImageRecord item = stage.Grid.CopyBufferToImages[index];
			Debug.Assert(item.Regions != null, nameof(item.Regions) + " is null");
			foreach (var region in item.Regions)
			{
				nuint slice = region.BaseArrayLayer;
				for (var i = 0; i < region.ImageLayerCount; ++i)
				{
					stage.Encoder.CopyFromBuffer(
						item.Buffer,
						region.SourceOffset,
						region.BufferBytesPerRow,
						region.BufferImageAllocationSize,
						region.ImageSize,
						item.Image,
						slice,
						region.ImageMipLevel,
						region.ImageOffset);
					++slice;
				}
			}
		}

		#endregion

		#region CmdCopyImage methods

		public void CmdCopyImage(IMgImage srcImage, MgImageLayout srcImageLayout,
		                         IMgImage dstImage, MgImageLayout dstImageLayout, MgImageCopy[] pRegions)
		{
			if (srcImage == null)
			{
				throw new ArgumentNullException(nameof(srcImage));
			}

			if (dstImage == null)
			{
				throw new ArgumentNullException(nameof(dstImage));
			}

			if (pRegions == null)
			{
				throw new ArgumentNullException(nameof(pRegions));
			}

			var bSrcImage = (AmtImage)srcImage;
			var bDstImage = (AmtImage)dstImage;

			var regions = new List<AmtBlitCopyImageRegionRecord>();
			for (var i = 0; i < pRegions.Length; ++i)
			{
				var currentRegion = pRegions[i];

				var extent = currentRegion.Extent;

				if (extent.Width > nint.MaxValue)
				{
					throw new ArgumentOutOfRangeException(
						"pRegions[" + i + "].Extent.Width  must be <=" + nint.MaxValue);
				}

				if (extent.Height > nint.MaxValue)
				{
					throw new ArgumentOutOfRangeException(
						"pRegions[" + i + "].Extent.Height must be <= " + nint.MaxValue);
				}

				if (extent.Depth > nint.MaxValue)
				{
					throw new ArgumentOutOfRangeException(
						"pRegions[" + i + "].Extent.Depth must be <= " + nint.MaxValue);
				}

				regions.Add(new AmtBlitCopyImageRegionRecord
				{
					SourceSlice = currentRegion.SrcSubresource.BaseArrayLayer,
					SourceMipLevel = currentRegion.SrcSubresource.MipLevel,
					SourceOrigin = new MTLOrigin
					{
						X = currentRegion.SrcOffset.X,
						Y = currentRegion.SrcOffset.Y,
						Z = currentRegion.SrcOffset.Z
					},
					SourceLayerCount = currentRegion.SrcSubresource.LayerCount,
					SourceSize = new MTLSize
					{
						Width = (nint) currentRegion.Extent.Width,
						Height = (nint) currentRegion.Extent.Height,
						Depth = (nint) currentRegion.Extent.Depth,
					},
					DestinationSlice = currentRegion.DstSubresource.BaseArrayLayer,
					DestinationMipLevel = currentRegion.DstSubresource.MipLevel,
					DestinationLayerCount = currentRegion.DstSubresource.LayerCount,
					DestinationOrigin = new MTLOrigin
					{
						X = currentRegion.DstOffset.X,
						Y = currentRegion.DstOffset.Y,
						Z = currentRegion.DstOffset.Z
					},
				});
			}

			var item = new AmtBlitCopyImageRecord
			{
				Source = bSrcImage.OriginalTexture,
				Destination = bDstImage.OriginalTexture,
				Regions = regions.ToArray(),
			};

			var nextIndex = mBag.CopyImages.Push(item);
			mInstructions.Add(new AmtEncodingInstruction
			{
				Category = AmtEncoderCategory.Blit,
				Index = nextIndex,
				Operation = CmdCopyImage,
			});
		}

		private static void CmdCopyImage(AmtCommandRecording recording, uint index)
		{
			Debug.Assert(recording != null, nameof(recording) + " is null");
			var stage = recording.Blit;
			Debug.Assert(stage != null, nameof(stage) + " is null");
			Debug.Assert(stage.Encoder != null, nameof(stage.Encoder) + " is null");
			Debug.Assert(stage.Grid != null, nameof(stage.Grid) + " is null");
			AmtBlitCopyImageRecord item = stage.Grid.CopyImages[index];
			Debug.Assert(item.Regions != null, nameof(item.Regions) + " is null");

			foreach (var region in item.Regions)
			{
				nuint srcSlice = region.SourceSlice;
				nuint dstSlice = region.DestinationSlice;
				for (var i = 0; i < region.SourceLayerCount; ++i)
				{
					stage.Encoder.CopyFromTexture(
						item.Source,
						srcSlice,
						region.SourceMipLevel,
						region.SourceOrigin,
						region.SourceSize,
						item.Destination,
						dstSlice,
						region.DestinationMipLevel,
						region.DestinationOrigin
					);
					++srcSlice;
				}

				srcSlice = region.SourceSlice;
				for (var i = 0; i < region.DestinationLayerCount; ++i)
				{
					stage.Encoder.CopyFromTexture(
						item.Source,
						srcSlice,
						region.SourceMipLevel,
						region.SourceOrigin,
						region.SourceSize,
						item.Destination,
						dstSlice,
						region.DestinationMipLevel,
						region.DestinationOrigin
					);
					++dstSlice;
				}
			}
		}

		#endregion
	}
}
