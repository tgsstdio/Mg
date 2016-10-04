namespace Magnesium.Metal
{
	public interface IAmtBlitEncoder
	{
		void Clear();
		void CopyBuffer(IMgBuffer srcBuffer, IMgBuffer dstBuffer, MgBufferCopy[] pRegions);

		void CopyBufferToImage(IMgBuffer srcBuffer, IMgImage dstImage, MgImageLayout dstImageLayout, MgBufferImageCopy[] pRegions);

		AmtBlitGrid AsGrid();
}
}