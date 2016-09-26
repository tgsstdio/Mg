namespace Magnesium.Metal
{
	public class AmtPipelineLayoutBufferBinding
	{
		public uint Binding { get; internal set; }
		public uint DescriptorCount { get; internal set; }
		public AmtPipelineLayoutBufferBindingCategory Category { get; internal set; }
	}
}