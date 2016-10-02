namespace Magnesium.Metal
{
	public interface IAmtEncoderContextSorter
	{
		void Clear();
		void Add(AmtEncodingInstruction instruction);
		AmtCommandBufferRecord ToReplay();
	}
}