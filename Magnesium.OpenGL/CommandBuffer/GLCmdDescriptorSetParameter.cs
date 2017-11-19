namespace Magnesium.OpenGL.Internals
{
    public class GLCmdDescriptorSetParameter
	{
		public IGLFutureDescriptorSet DescriptorSet {
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

