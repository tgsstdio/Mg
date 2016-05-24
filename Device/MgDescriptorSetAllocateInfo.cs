namespace Magnesium
{
    // Device
    public class MgDescriptorSetAllocateInfo
	{
		public IMgDescriptorPool DescriptorPool { get; set; }
		public IMgDescriptorSetLayout[] SetLayouts { get; set; }
	}
}

