using System;

namespace Magnesium.OpenGL.Internals
{
	public class GLBindingPointOffsetInfo : IEquatable<GLBindingPointOffsetInfo>
	{
		public uint Binding { get; set; }
		public int First { get; set; }
		public int Last { get; set; }

		public bool Equals(GLBindingPointOffsetInfo other)
		{
			if (Binding != other.Binding)
				return false;

			if (First != other.First)
				return false;

			return Last == other.Last;
		}
	}
}
