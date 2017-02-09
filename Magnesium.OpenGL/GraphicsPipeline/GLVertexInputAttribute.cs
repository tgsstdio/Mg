
namespace Magnesium.OpenGL.Internals
{
	public class GLVertexInputAttribute
	{
		public GLVertexAttribFunction Function { get; set; }
		public uint Binding { get; set; }
		public uint Location { get; set; }
		public int Size { get; set; }
		public GLVertexAttributeType PointerType { get; set; }
		public bool IsNormalized { get; set; }
		public int Stride { get; set; }
		public uint Offset {get;  set; }
		public uint Divisor { get; set; }
	}
}

