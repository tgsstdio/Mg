namespace Magnesium
{
    // Device
    public class MgDescriptorImageInfo
	{
		public IMgSampler Sampler { get; set; }
		public IMgImageView ImageView { get; set; }
		public MgImageLayout ImageLayout { get; set; }
	}
}

