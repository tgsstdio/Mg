using Foundation;
using Metal;

namespace Magnesium.Metal
{
	public class AmtCmdBindDescriptorSetsStageRecord
	{
		public AmtCmdBindDescriptorSetBufferRecord[] Buffers { get; set; }
		public NSRange TexturesRange { get; set; }
		public IMTLTexture[] Textures { get; set; }
		public IMTLSamplerState[] Samplers { get; set; }
		public NSRange SamplersRange { get; internal set; }
	}
}