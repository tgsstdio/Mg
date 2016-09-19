namespace Magnesium.OpenGL
{
	public class CmdBufferInstructionSet
	{
		public GLCmdClearValuesParameter[] ClearValues {
			get;
			set;
		}

		public GLGraphicsPipelineBlendColorState[] ColorBlends {
			get;
			set;
		}

		public GLCmdDepthBoundsParameter[] DepthBounds {
			get;
			set;
		}

		public GLCmdDepthBiasParameter[] DepthBias {
			get;
			set;
		}

		public GLCmdDescriptorSetParameter[] DescriptorSets {
			get;
			set;
		}

		public MgColor4f[] BlendConstants {
			get;
			set;
		}

		public GLCmdScissorParameter[] Scissors {
			get;
			set;
		}

		public float[] LineWidths {
			get;
			set;
		}

		public int[] FrontWriteMasks {
			get;
			set;
		}

		public int[] BackWriteMasks {
			get;
			set;
		}

		public int[] FrontReferences {
			get;
			set;
		}

		public int[] BackReferences {
			get;
			set;
		}

		public int[] FrontCompareMasks {
			get;
			set;
		}

		public GLCmdBufferDrawItem[] DrawItems;
		public GLCmdBufferPipelineItem[] Pipelines;
		public GLCmdVertexBufferObject[] VBOs;

		public GLCmdViewportParameter[] Viewports {
			get;
			set;
		}

		public int[] BackCompareMasks {
			get;
			set;
		}
	}
}

