using System;

namespace Magnesium.Metal
{
	public class AmtBlitBag
	{
		internal AmtEncoderItemCollection<AmtBlitCopyBufferRecord> CopyBuffers { get; private set; }

		public AmtBlitBag()
		{
			CopyBuffers = new AmtEncoderItemCollection<AmtBlitCopyBufferRecord>();
		}

		internal void Clear()
		{
			CopyBuffers.Clear();
		}
	}
}