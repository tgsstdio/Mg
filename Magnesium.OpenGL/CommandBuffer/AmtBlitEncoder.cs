using System;

namespace Magnesium.OpenGL
{
    public class AmtBlitEncoder : IAmtBlitEncoder
    {
        public AmtBlitEncoder()
        {
        }

        public AmtBlitGrid AsGrid()
        {
            return new AmtBlitGrid
            {

            };
        }

        public void Clear()
        {
 
        }

        public void CmdCopyImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageCopy[] pRegions)
        {
            throw new NotImplementedException();
        }

        public void CopyBuffer(IMgBuffer srcBuffer, IMgBuffer dstBuffer, MgBufferCopy[] pRegions)
        {
            throw new NotImplementedException();
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
