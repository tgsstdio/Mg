using System;

namespace Magnesium.OpenGL
{
	public class GLDynamicOffsetInfo : IEquatable<GLDynamicOffsetInfo>
	{
		public GLBufferRangeTarget Target { get; set; }
		public uint DstIndex { get; set; }

		public bool Equals(GLDynamicOffsetInfo other)
		{
			if (Target != other.Target)
				return false;

			return DstIndex == other.DstIndex;
		}
	}
}