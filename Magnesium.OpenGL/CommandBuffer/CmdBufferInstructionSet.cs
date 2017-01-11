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

		public uint[] FrontWriteMasks {
			get;
			set;
		}

		public uint[] BackWriteMasks {
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

		public uint[] FrontCompareMasks {
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

		public uint[] BackCompareMasks {
			get;
			set;
		}
	}
}

