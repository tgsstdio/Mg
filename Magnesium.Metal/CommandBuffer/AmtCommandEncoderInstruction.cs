using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtCommandEncoderInstruction
	{
		public uint Index { get; set; }
		public AmtCommandEncoderCategory Category { get; internal set; }
 		public Action<AmtCommandRecording, uint> Operation { get; set;}
	}
}
