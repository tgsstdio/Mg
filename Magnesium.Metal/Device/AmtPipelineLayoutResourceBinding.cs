namespace Magnesium.Metal
{
	internal class AmtPipelineResourceBinding
	{
		public uint Binding { get; internal set; }
		public uint DescriptorCount { get; internal set; }
		public MgDescriptorType DescriptorType { get; internal set; }
		public MgShaderStageFlagBits Stage { get; internal set; }
	}
}