namespace Magnesium.Metal
{
	public interface IAmtBlitEncoder
	{
		void Clear();
		void CopyBuffer(IMgBuffer srcBuffer, IMgBuffer dstBuffer, MgBufferCopy[] pRegions);
		void CmdCopyImage(IMgImage srcImage, MgImageLayout srcImageLayout, IMgImage dstImage, MgImageLayout dstImageLayout, MgImageCopy[] pRegions);
		void CopyBufferToImage(IMgBuffer srcBuffer, IMgImage dstImage, MgImageLayout dstImageLayout, MgBufferImageCopy[] pRegions);
		AmtBlitGrid AsGrid();
	}
}