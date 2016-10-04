namespace Magnesium.Metal
{
	public interface IAmtBlitEncoder
	{
		void Clear();
		void CopyBuffer(IMgBuffer srcBuffer, IMgBuffer dstBuffer, MgBufferCopy[] pRegions);
		AmtBlitGrid AsGrid();
	}
}