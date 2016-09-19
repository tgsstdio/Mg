using System;

namespace Magnesium.OpenGL
{
	public class GLCmdDescriptorSetParameter
	{
		public uint FirstSet {
			get;
			set;
		}

		public IMgDescriptorSet[] DescriptorSets {
			get;
			set;
		}

		public IMgPipelineLayout Layout {
			get;
			set;
		}

		public uint[] DynamicOffsets { get; set; }
		public MgPipelineBindPoint Bindpoint { get; set; }
	}
}

