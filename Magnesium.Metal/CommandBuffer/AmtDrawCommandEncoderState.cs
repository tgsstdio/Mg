namespace Magnesium.Metal
{
	public class AmtDrawCommandEncoderState
	{
		public AmtDrawEncoderState Draw { get; internal set; }
		public AmtDrawIndexedEncoderState DrawIndexed { get; internal set; }
		public AmtDrawIndexedIndirectEncoderState DrawIndexedIndirect { get; internal set; }
		public AmtDrawIndirectEncoderState DrawIndirect { get; internal set; }
	}
}