using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Magnesium.OpenGL.Internals
{
    public class GLCmdBlitEncoder : IGLCmdBlitEncoder
    {
        private readonly GLCmdBlitBag mBag;
        private readonly IGLCmdEncoderContextSorter mInstructions;
        private readonly IGLImageFormatEntrypoint mImageFormat;

        public GLCmdBlitEncoder(IGLCmdEncoderContextSorter instructions, GLCmdBlitBag bag, IGLImageFormatEntrypoint imageFormat)
        {
            mBag = bag;
            mInstructions = instructions;
            mImageFormat = imageFormat;
        }

        public GLCmdBlitGrid AsGrid()
        {
            return new GLCmdBlitGrid
            {
                CopyBuffers = mBag.CopyBuffers.ToArray(),
                LoadImageOps = mBag.LoadImageOps.ToArray(),
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
                throw new ArgumentNullException("srcBuffer");
            }

            if (dstBuffer == null)
            {
                throw new ArgumentNullException("dstBuffer");
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
                    throw new InvalidOperationException("pRegions[" +  i + "].SrcOffset is greater than long.MaxValue");
                }

                if (region.DstOffset > (ulong)long.MaxValue)
                {
                    throw new InvalidOperationException("pRegions[" + i + "].DstOffset is greater than long.MaxValue");
                }

                if (region.Size > (ulong) int.MaxValue)
                {
                    throw new InvalidOperationException("pRegions[" + i + "].Size is greater than int.MaxValue");
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

        public void LoadImageData(MgImageMemoryBarrier[] pImageMemoryBarriers)
        {
            var images = new List<GLCmdTexImageData>();

            foreach (var imgBarrier in pImageMemoryBarriers)
            {
                if (imgBarrier != null)
                {
                    var image = (IGLImage) imgBarrier.Image;
                    if (image != null && imgBarrier.OldLayout == MgImageLayout.PREINITIALIZED && imgBarrier.NewLayout == MgImageLayout.SHADER_READ_ONLY_OPTIMAL)
                    {
                        var internalFormat = mImageFormat.GetGLFormat(image.Format, true);
                        //					PixelInternalFormat glInternalFormat;
                        //					PixelFormat glFormat;
                        //					PixelType glType;
                        //					image.Format.GetGLFormat (true, out glInternalFormat, out glFormat, out glType);

                        var subResourceRange = imgBarrier.SubresourceRange;
                        int layerEnd = (int)(subResourceRange.BaseArrayLayer + subResourceRange.LayerCount);
                        int levelEnd = (int)(subResourceRange.BaseMipLevel + subResourceRange.LevelCount);
                        for (int i = (int)subResourceRange.BaseArrayLayer; i < layerEnd; ++i)
                        {
                            var arrayDetail = image.ArrayLayers[i];
                            for (int j = (int)subResourceRange.BaseMipLevel; j < levelEnd; ++j)
                            {
                                var levelDetail = arrayDetail.Levels[j];
                                var copyCmd = new GLCmdTexImageData
                                {
                                    Target = image.ImageType,
                                    Level = j,
                                    Slice = i,
                                    Width = levelDetail.Width,
                                    Height = levelDetail.Height,
                                    Depth = levelDetail.Depth,
                                    Format = image.Format,
                                    PixelFormat = internalFormat.GLFormat,
                                    InternalFormat = internalFormat.InternalFormat,
                                    PixelType = internalFormat.GLType,
                                    TextureId = image.OriginalTextureId,
                                };
                                if (levelDetail.SubresourceLayout.Size > (ulong)int.MaxValue)
                                {
                                    throw new InvalidOperationException(string.Format("array[{0}].Levels[{1}].SubresourceLayout.Size > int.MaxValue", i, j));
                                }
                                copyCmd.Size = (int)levelDetail.SubresourceLayout.Size;
                                if (levelDetail.SubresourceLayout.Offset > (ulong)int.MaxValue)
                                {
                                    throw new InvalidOperationException(string.Format("array[{0}].Levels[{1}].SubresourceLayout.Offset > int.MaxValue", i, j));
                                }
                                copyCmd.Data = IntPtr.Add(image.Handle, (int)levelDetail.SubresourceLayout.Offset);
                                images.Add(copyCmd);
                            }
                        }
                    }
                }
            }

            if (images.Count > 0)
            {
                var item = new GLCmdImageInstructionSet
                {
                    Images = images.ToArray()
                };

                var nextIndex = mBag.LoadImageOps.Push(item);

                var instruction = new GLCmdEncodingInstruction
                {
                    Category = GLCmdEncoderCategory.Blit,
                    Index = nextIndex,
                    Operation = CmdLoadImageData,
                };

                mInstructions.Add(instruction);
            }
        }

        private void CmdLoadImageData(GLCmdCommandRecording arg1, uint arg2)
        {
            var encoder = arg1.Blit;
            Debug.Assert(encoder != null);
            var grid = encoder.Grid;
            Debug.Assert(grid != null);
            var entrypoint = encoder.Entrypoint;
            Debug.Assert(entrypoint != null);
            var items = grid.LoadImageOps;
            Debug.Assert(items != null);
            var item = items[arg2];

            entrypoint.PerformOperation(item);
        }
    }    
}
