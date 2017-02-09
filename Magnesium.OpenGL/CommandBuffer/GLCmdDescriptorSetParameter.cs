namespace Magnesium.OpenGL.Internals
{
    public class GLCmdDescriptorSetParameter
	{
		public IGLDescriptorSet DescriptorSet {
			get;
			set;
		}

		public IGLPipelineLayout Layout {
			get;
			set;
		}

		public uint[] DynamicOffsets { get; set; }

		public MgPipelineBindPoint Bindpoint { get; set; }
	}
}

