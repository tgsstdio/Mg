namespace Magnesium.OpenGL
{
	public interface IGLCmdBufferRepository
	{
		IGLCmdBufferStore<MgColor4f> BlendConstants { get; }
		IGLCmdBufferStore<int> BackWriteMasks { get;	 }
		IGLCmdBufferStore<int> FrontWriteMasks { get;  }
		IGLCmdBufferStore<int> BackReferences { get; }
		IGLCmdBufferStore<int> FrontReferences { get; }
		IGLCmdBufferStore<int> FrontCompareMasks { get; 	}
		IGLCmdBufferStore<int> BackCompareMasks { get;  }
		IGLCmdBufferStore<GLCmdVertexBufferParameter> VertexBuffers { get; 	}
		IGLCmdBufferStore<GLCmdIndexBufferParameter> IndexBuffers { get;  }
		IGLCmdBufferStore<IGLGraphicsPipeline> GraphicsPipelines { get;  }
		IGLCmdBufferStore<GLCmdViewportParameter> Viewports { get;  }
		IGLCmdBufferStore<GLCmdDescriptorSetParameter> DescriptorSets { get; }
		IGLCmdBufferStore<GLCmdScissorParameter> Scissors { get; }
		IGLCmdBufferStore<float> LineWidths { get;  }
		IGLCmdBufferStore<GLCmdDepthBiasParameter> DepthBias { get;  }
		IGLCmdBufferStore<GLCmdDepthBoundsParameter> DepthBounds { get;  }	

		void SetStencilReference(MgStencilFaceFlagBits face, uint mask);
		void SetWriteMask(MgStencilFaceFlagBits face, uint mask);
		void SetCompareMask(MgStencilFaceFlagBits face, uint mask);

		bool MapRepositoryFields(ref GLCmdDrawCommand command);
		void Clear ();
		void PushGraphicsPipeline (IGLGraphicsPipeline glPipeline);
		void PushScissors (uint firstScissor, MgRect2D[] pScissors);
		void PushViewports (uint firstViewport, MgViewport[] pViewports);
		void PushDepthBias (float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor);
		void PushDepthBounds (float minDepthBounds, float maxDepthBounds);

		void PushLineWidth(float lineWidth);
		void PushBlendConstants(MgColor4f blendConstants);
	}
}

