namespace Magnesium.Metal
{
	class AmtCmdVertexBufferParameter
	{
		internal uint firstBinding;
		internal IMgBuffer[] pBuffers;
		internal ulong[] pOffsets;

		public AmtCmdVertexBufferParameter()
		{
		}
	}
}