namespace Magnesium.OpenGL.Internals
{
	public interface IGLCmdEncoderContextSorter
	{
		void Clear();
		void Add(GLCmdEncodingInstruction instruction);
		GLCmdCommandBufferRecord ToReplay();
	}
}