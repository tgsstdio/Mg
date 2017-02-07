namespace Magnesium.OpenGL.Internals
{
	public interface IAmtEncoderContextSorter
	{
		void Clear();
		void Add(AmtEncodingInstruction instruction);
		AmtCommandBufferRecord ToReplay();
	}
}