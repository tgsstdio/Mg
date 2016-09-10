using System;

namespace Magnesium.OpenGL
{
	public struct GLVariableBind
	{
		public bool IsActive { get; set; }
		public int Location { get; set; }
		public MgDescriptorType DescriptorType { get; set; }
	}
}

