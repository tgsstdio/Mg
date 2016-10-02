using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtEncodingInstruction
	{
		public uint Index { get; set; }
		public AmtEncoderCategory Category { get; internal set; }
 		public Action<AmtCommandRecording, uint> Operation { get; set;}
	}
}
