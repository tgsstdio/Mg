namespace Magnesium.OpenGL
{
	public struct GLUniformBinding
	{
        public uint DescriptorCount { get; internal set; }
        public MgDescriptorType DescriptorType {
			get;
			set;
		}

		public int Location { get; set; }
        public MgShaderStageFlagBits StageFlags { get; internal set; }
    }
}

