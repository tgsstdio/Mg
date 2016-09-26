namespace Magnesium.Metal
{
	public class AmtVertexBufferEncoderState
	{
		internal uint firstBinding;
		internal IMgBuffer[] pBuffers;
		internal ulong[] pOffsets;

		public AmtVertexBufferEncoderState()
		{
		}
	}
}