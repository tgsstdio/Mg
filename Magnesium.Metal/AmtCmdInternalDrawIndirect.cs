namespace Magnesium.Metal
{
	public class AmtCmdInternalDrawIndirect
	{
		internal IMgBuffer buffer;
		internal uint drawCount;
		internal ulong offset;
		internal uint stride;

		public AmtCmdInternalDrawIndirect()
		{
		}
	}
}