using System;

namespace Magnesium
{
	public class MgDepthStencilBufferCreateInfo
	{
		public IMgCommandBuffer Command { get; set; }
		public UInt32 Width { get; set; }
		public UInt32 Height { get; set;}
		public MgFormat Format { get; set;}
		public MgSampleCountFlagBits Samples { get; set; }
	}

	public interface IMgDepthStencilBuffer : IDisposable
	{
		IMgImageView View {
			get;
		}

		void Setup();
		void Create(MgDepthStencilBufferCreateInfo createInfo);
	}
}

