namespace Magnesium.OpenGL
{
	public class GLDescriptorPoolResourceInfo
	{
		public GLDescriptorBindingGroup GroupType { get; set; }
		public uint Binding { get; set; }
		public uint DescriptorCount { get; set; }
		public GLPoolResourceTicket Ticket { get; set; }
	}
}
