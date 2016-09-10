
namespace Magnesium.OpenGL
{
	public struct GLCmdDrawCommand
	{
		public GLCmdInternalDrawIndexedIndirect DrawIndexedIndirect {
			get;
			set;
		}

		public GLCmdInternalDraw Draw {
			get;
			set;
		}

		public GLCmdInternalDrawIndirect DrawIndirect {
			get;
			set;
		}

		public GLCmdInternalDrawIndexed DrawIndexed {
			get;
			set;
		}

		public int? BlendConstants  { get; set;	}
		public int? IndexBuffer  { get; set; }
		public int? VertexBuffer  { get; set; }
		public int? Pipeline  { get; set; }
		public int? Scissors  { get; set; }
		public int? Viewports  { get; set; }
		public int? DescriptorSet { get; set; }
		public int? DepthBounds { get; set;	}
		public int? DepthBias  { get; set; }
		public int? FrontReference  { get; set;	}
		public int? BackReference { get; set;	}
		public int? FrontWriteMask  { get; set;	}
		public int? BackWriteMask  { get; set;	}
		public int? FrontCompareMask  { get; set; }
		public int? BackCompareMask { get; set;	}
		public int? LineWidth  { get; set;	}
	}
}

