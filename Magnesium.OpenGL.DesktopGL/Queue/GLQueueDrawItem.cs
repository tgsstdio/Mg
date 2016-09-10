using System.Runtime.InteropServices;

namespace Magnesium.OpenGL.DesktopGL
{
	[StructLayout(LayoutKind.Sequential)]
	public struct GLQueueDrawItem
	{
		public byte PassIndex { get; set; }
		public int ProgramIndex { get; set; }
		public MgPrimitiveTopology Topology { get; set;}
		public MgPolygonMode Mode { get; set; }			

		public GLGraphicsPipelineFlagBits Flags {get;set;}
		public GLQueueRendererRasterizerState RasterizerValues {get;set;}
		public GLGraphicsPipelineStencilState StencilValues {get;set;}
		public GLGraphicsPipelineBlendColorAttachmentState BlendValues { get; set;}
		public GLGraphicsPipelineDepthState DepthValues {get;set;}

//
//		public DrawState State { get; set; }
//		public byte TargetIndex {get;set;}
		public byte DescriptorSet { get; set;}
//		public ushort BufferMask { get; set; }
//		public ushort ShaderOptions { get; set; }
//
//		public uint ResourceIndex { get; set; }

		public int VBO { get; set; }
		public ushort DrawItem {get; set; }
//		public uint MarkerIndex { get; set; }




	}
}

