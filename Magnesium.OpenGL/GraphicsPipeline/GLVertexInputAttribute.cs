
namespace Magnesium.OpenGL
{
	public class GLVertexInputAttribute
	{
		public GLVertexAttribFunction Function { get; set; }
		public uint Binding { get; set; }
		public int Location { get; set; }
		public int Size { get; set; }
		public GLVertexAttributeType PointerType { get; set; }
		public bool IsNormalized { get; set; }
		public int Stride { get; set; }
		public int Offset {get;  set; }
		public int Divisor { get; set; }
	}
}

