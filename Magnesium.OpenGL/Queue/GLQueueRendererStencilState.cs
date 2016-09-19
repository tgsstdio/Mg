using System.Runtime.InteropServices;

namespace Magnesium.OpenGL
{
	[StructLayout(LayoutKind.Sequential)]
	public struct GLQueueRendererStencilState
	{
		public GLGraphicsPipelineFlagBits Flags { get; set; }
		public GLGraphicsPipelineStencilState Enums { get; set; }
		public GLGraphicsPipelineStencilMasks Front { get; set; }
		public GLGraphicsPipelineStencilMasks Back  { get; set; }
	}
}

