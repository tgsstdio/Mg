namespace Magnesium.OpenGL
{
	internal struct GLUniformBinding
	{
        public uint DescriptorCount { get; internal set; }
        public MgDescriptorType DescriptorType {
			get;
			set;
		}

		public uint Binding { get; set; }
        public MgShaderStageFlagBits StageFlags { get; internal set; }
    }
}

