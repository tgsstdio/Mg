namespace Magnesium.OpenGL
{
	public interface IGLCmdVBOEntrypoint
	{
		void BindIndexBuffer (uint vbo, uint bufferId);

		void BindDoubleVertexAttribute(uint vbo, uint location, int size, GLVertexAttributeType pointerType, uint offset);
		void BindIntVertexAttribute   (uint vbo, uint location, int size, GLVertexAttributeType pointerType, uint offset);
		void BindFloatVertexAttribute (uint vbo, uint location, int size, GLVertexAttributeType pointerType, bool isNormalized, uint offset);
		void SetupVertexAttributeDivisor (uint vbo, uint location, uint divisor);

		uint GenerateVBO ();

		void DeleteVBO(uint vbo);

		void AssociateBufferToLocation (uint vbo, uint location, uint bufferId, long offsets, uint stride);
	}
}

