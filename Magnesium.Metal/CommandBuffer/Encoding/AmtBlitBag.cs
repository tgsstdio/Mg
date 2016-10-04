using System;

namespace Magnesium.Metal
{
	public class AmtBlitBag
	{
		internal AmtEncoderItemCollection<AmtBlitCopyBufferRecord> CopyBuffers { get; private set; }
		internal AmtEncoderItemCollection<AmtBlitCopyBufferToImageRecord> CopyBufferToImages { get; private set;}

		public AmtBlitBag()
		{
			CopyBuffers = new AmtEncoderItemCollection<AmtBlitCopyBufferRecord>();
			CopyBufferToImages = new AmtEncoderItemCollection<AmtBlitCopyBufferToImageRecord>();
		}

		internal void Clear()
		{
			CopyBuffers.Clear();
			CopyBufferToImages.Clear();
		}
	}
}