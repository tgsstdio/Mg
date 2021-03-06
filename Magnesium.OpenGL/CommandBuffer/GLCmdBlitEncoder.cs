﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Magnesium.OpenGL.Internals
{
    public class GLCmdBlitEncoder : IGLCmdBlitEncoder
    {
        private readonly GLCmdBlitBag mBag;
        private readonly IGLCmdEncoderContextSorter mInstructions;

        public GLCmdBlitEncoder(IGLCmdEncoderContextSorter instructions, GLCmdBlitBag bag)
        {
            mBag = bag;
            mInstructions = instructions;
        }

        public GLCmdBlitGrid AsGrid()
        {
            return new GLCmdBlitGrid
            {
                CopyBuffers = mBag.CopyBuffers.ToArray(),
            };
        }

        public void Clear()
        {
            mBag.Clear();
        }

        public void CmdCopyImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageCopy[] pRegions)
        {
            throw new NotImplementedException();
        }

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

            var bSrcBuffer = (IGLBuffer)srcBuffer;
            Debug.Assert(bSrcBuffer.IsBufferType);

            var bDstBuffer = (IGLBuffer)dstBuffer;
            Debug.Assert(bDstBuffer.IsBufferType);

            var copyParams = new List<GLCmdCopyBufferRegionRecord>();
            for (var i = 0; i < pRegions.Length; i += 1)
            {
                var region = pRegions[i];
                if (region.SrcOffset > (ulong) long.MaxValue)
                {
                    throw new InvalidOperationException(nameof(pRegions) + "[" +  i + "].SrcOffset is greater than long.MaxValue");
                }

                if (region.DstOffset > (ulong)long.MaxValue)
                {
                    throw new InvalidOperationException(nameof(pRegions) + "[" + i + "].DstOffset is greater than long.MaxValue");
                }

                if (region.Size > (ulong) int.MaxValue)
                {
                    throw new InvalidOperationException(nameof(pRegions) + "[" + i + "].Size is greater than int.MaxValue");
                }

                var bufferParam = new GLCmdCopyBufferRegionRecord
                {
                    ReadOffset = new IntPtr((long) region.SrcOffset),
                    WriteOffset = new IntPtr((long) region.DstOffset),
                    Size = (int)region.Size,
                };
                copyParams.Add(bufferParam);
            }

            var item = new GLCmdCopyBufferRecord
            {
                Source = bSrcBuffer.BufferId,
                Destination = bDstBuffer.BufferId,
                Regions = copyParams.ToArray(),
            };

            var nextIndex = mBag.CopyBuffers.Push(item);

            var instruction = new GLCmdEncodingInstruction
            {
                Category = GLCmdEncoderCategory.Blit,
                Index = nextIndex,
                Operation = CmdCopyBuffer,
            };

            mInstructions.Add(instruction);
        }

        private void CmdCopyBuffer(GLCmdCommandRecording arg1, uint arg2)
        {
            var encoder = arg1.Blit;
            Debug.Assert(encoder != null);
            var grid = encoder.Grid;
            Debug.Assert(grid != null);
            var entrypoint = encoder.Entrypoint;
            Debug.Assert(entrypoint != null);
            Debug.Assert(grid.CopyBuffers != null);
            var item = grid.CopyBuffers[arg2];

            if (item.Regions != null)
            {
                foreach (var region in item.Regions)
                {
                    entrypoint.CopyBuffer(item.Source, item.Destination, region.ReadOffset, region.WriteOffset, region.Size);
                }
            }
        }

        public void CopyBufferToImage(IMgBuffer srcBuffer, IMgImage dstImage, MgImageLayout dstImageLayout, MgBufferImageCopy[] pRegions)
        {
            throw new NotImplementedException();
        }

        public void CopyImageToBuffer(IMgImage srcImage, MgImageLayout srcImageLayout, IMgBuffer dstBuffer, MgBufferImageCopy[] pRegions)
        {
            throw new NotImplementedException();
        }

        internal void EndEncoding()
        {
            throw new NotImplementedException();
        }
    }
}
