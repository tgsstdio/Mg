namespace Magnesium.Metal
{
	public enum AmtBufferCategory
	{
		VertexBuffer,
		UniformBuffer,
		StorageBuffer,
	}

	public class AmtPipelineVertexBufferBinding
	{
		public uint Binding { get; internal set; }
		public uint DescriptorCount { get; internal set; }
		public AmtBufferCategory Category { get; internal set; }
	}
}