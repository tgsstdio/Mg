namespace Magnesium.OpenGL.Internals
{
	public interface IGLGraphicsPipeline : IMgPipeline
    {
		int ProgramID {
			get;
		}

        GLInternalCache InternalCache { get; }
        IGLPipelineLayout Layout { get; }

		GLGraphicsPipelineDynamicStateFlagBits DynamicsStates { get; }
		GLGraphicsPipelineBlendColorState ColorBlendEnums { get; }

		GLCmdViewportParameter Viewports { get; }
		GLCmdScissorParameter Scissors { get; }

		GLGraphicsPipelineFlagBits Flags { get; }

		MgPolygonMode PolygonMode { get; }

		GLGraphicsPipelineStencilMasks Front { get; }
		GLGraphicsPipelineStencilMasks Back { get; }

		MgPrimitiveTopology Topology { get; }

		GLGraphicsPipelineDepthState DepthState {
			get;
		}

		GLVertexBufferBinder VertexInput { get; }

		MgColor4f BlendConstants {
			get;
		}

		float MinDepthBounds {
			get;
		}

		float MaxDepthBounds {
			get;
		}

		float DepthBiasSlopeFactor {
			get;
		}

		float DepthBiasClamp {
			get;
		}

		float DepthBiasConstantFactor {
			get;
		}

		float LineWidth {
			get;
		}

		GLGraphicsPipelineStencilState StencilState {
			get;
		}		
	}
}

