namespace Magnesium.OpenGL
{
	public class GLVertexBufferBinder
	{
		public GLVertexBufferBinder (GLVertexBufferBinding[] bindings, GLVertexInputAttribute[] attributes)
		{
			Bindings = bindings;
			Attributes = attributes;
		}

		public GLVertexBufferBinding[] Bindings { get; private set; }
		public GLVertexInputAttribute[] Attributes { get; private set; }
	}
}

