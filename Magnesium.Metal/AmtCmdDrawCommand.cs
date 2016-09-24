namespace Magnesium.Metal
{
	public class AmtCmdDrawCommand
	{
		public AmtCmdInternalDrawIndexed DrawIndexed { get; internal set; }
		public AmtCmdInternalDrawIndexedIndirect DrawIndexedIndirect { get; internal set; }
		public AmtCmdInternalDrawIndirect DrawIndirect { get; internal set; }
	}
}