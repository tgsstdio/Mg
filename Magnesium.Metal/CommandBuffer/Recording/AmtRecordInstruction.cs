using System;
namespace Magnesium.Metal
{
	public struct AmtRecordInstruction
	{
		public uint Index { get; set; }
		public Action<AmtCommandRecording, uint> Operation { get; set; }

		public void Perform(AmtCommandRecording recording)
		{
			Operation(recording, Index);
		}
	}
}
