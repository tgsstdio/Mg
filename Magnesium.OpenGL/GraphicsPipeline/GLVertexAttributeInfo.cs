namespace Magnesium.OpenGL
{
	public struct GLVertexAttributeInfo
	{
		public GLVertexAttributeType PointerType { get; set; }
		public int Size {get; set;}
		public bool IsNormalized { get; set; }
		public GLVertexAttribFunction Function { get; set; }
	}
}

