namespace Magnesium.Metal
{
	class AmtCmdDescriptorSetParameter
	{
		public AmtCmdDescriptorSetParameter()
		{
		}

		public MgPipelineBindPoint Bindpoint { get; internal set; }
		public IMgDescriptorSet[] DescriptorSets { get; internal set; }
		public uint[] DynamicOffsets { get; internal set; }
		public uint FirstSet { get; internal set; }
		public IMgPipelineLayout Layout { get; internal set; }
	}
}