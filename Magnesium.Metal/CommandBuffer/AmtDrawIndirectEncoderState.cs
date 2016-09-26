namespace Magnesium.Metal
{
	public class AmtDrawIndirectEncoderState
	{
		internal IMgBuffer buffer;
		internal uint drawCount;
		internal ulong offset;
		internal uint stride;
	}
}