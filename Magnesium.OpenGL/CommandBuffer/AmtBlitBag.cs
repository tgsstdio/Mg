﻿namespace Magnesium.OpenGL
{
    public class AmtBlitBag
    {
        public AmtEncoderItemCollection<AmtCopyBufferRecord> CopyBuffers { get; set; }

        public AmtBlitBag()
        {
            CopyBuffers = new AmtEncoderItemCollection<AmtCopyBufferRecord>();
        }

        public void Clear()
        {
            CopyBuffers.Clear();
        }
    }
}