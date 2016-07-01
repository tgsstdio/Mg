namespace Magnesium
{
	public class MgTextureInfo
	{
		public MgImageLayout ImageLayout { get; set;}
		public IMgImage Image { get; set;}
		public IMgDeviceMemory DeviceMemory { get; set;}
		public IMgCommandBuffer Command { get; set; }
	}
}

