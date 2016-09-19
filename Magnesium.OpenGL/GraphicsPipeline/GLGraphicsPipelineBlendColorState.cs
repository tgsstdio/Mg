
namespace Magnesium.OpenGL
{
	public class GLGraphicsPipelineBlendColorState
	{
		public bool LogicOpEnable { get; set; }
		public MgLogicOp LogicOp { get; set; }
		public GLGraphicsPipelineBlendColorAttachmentState[] Attachments { get; set; }
	}
}

