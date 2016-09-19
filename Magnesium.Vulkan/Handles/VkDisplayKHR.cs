namespace Magnesium.Vulkan
{
	public class VkDisplayKHR : IMgDisplayKHR
	{
		public ulong Handle { get; private set; }

		public VkDisplayKHR(ulong display)
		{
			this.Handle = display;
		}
	}
}