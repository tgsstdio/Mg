using Magnesium;

namespace Magnesium.OpenGL
{
	public interface IGLCmdBlendEntrypoint
	{
		void EnableLogicOp (bool logicOpEnable);
		void LogicOp (MgLogicOp logicOp);
	
		GLGraphicsPipelineBlendColorState Initialize(uint noOfAttachments);

		void EnableBlending (uint index, bool value);

		void SetColorMask (uint index, MgColorComponentFlagBits colorMask);

		void ApplyBlendSeparateFunction (
			uint index,
			MgBlendFactor colorSource,
			MgBlendFactor colorDest,
			MgBlendFactor alphaSource,
			MgBlendFactor alphaDest);
        void SetBlendConstants(MgColor4f blendConstants);
    }
}

