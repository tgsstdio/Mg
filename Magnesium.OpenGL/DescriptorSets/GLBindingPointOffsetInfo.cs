using System;

namespace Magnesium.OpenGL
{
	internal class GLBindingPointOffsetInfo : IEquatable<GLBindingPointOffsetInfo>
	{
		public uint Binding { get; set; }
		public uint First { get; set; }
		public uint Last { get; set; }

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
