namespace Magnesium.OpenGL
{
	public class GLVertexBufferBinding
	{
		public uint Binding { get; private set; }
		public MgVertexInputRate InputRate { get; private set; }
		public uint Stride { get; private set; }

		public GLVertexBufferBinding (uint binding, MgVertexInputRate inputRate, uint stride)
		{
			Binding = binding;
			InputRate = inputRate;
			Stride = stride;
		}
	}
}

