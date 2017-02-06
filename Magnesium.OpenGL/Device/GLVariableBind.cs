namespace Magnesium.OpenGL
{
    internal struct GLVariableBind
	{
		public bool IsActive { get; set; }
		public uint Location { get; set; }
		public MgDescriptorType DescriptorType { get; set; }
	}
}

