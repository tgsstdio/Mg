namespace Magnesium.OpenGL
{
	public interface IGLCmdVBOEntrypoint
	{
		void BindIndexBuffer (int vbo, int bufferId);

		void BindDoubleVertexAttribute(int vbo, int location, int size, GLVertexAttributeType pointerType, int offset);
		void BindIntVertexAttribute   (int vbo, int location, int size, GLVertexAttributeType pointerType, int offset);
		void BindFloatVertexAttribute (int vbo, int location, int size, GLVertexAttributeType pointerType, bool isNormalized, int offset);
		void SetupVertexAttributeDivisor (int vbo, int location, int divisor);

		int GenerateVBO ();

		void DeleteVBO(int vbo);

		void AssociateBufferToLocation (int vbo, int location, int bufferId, long offsets, uint stride);
	}
}

