using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkPipelineStageFlags : uint
	{
		PipelineStageTopOfPipe = 0x1,
		PipelineStageDrawIndirect = 0x2,
		PipelineStageVertexInput = 0x4,
		PipelineStageVertexShader = 0x8,
		PipelineStageTessellationControlShader = 0x10,
		PipelineStageTessellationEvaluationShader = 0x20,
		PipelineStageGeometryShader = 0x40,
		PipelineStageFragmentShader = 0x80,
		PipelineStageEarlyFragmentTests = 0x100,
		PipelineStageLateFragmentTests = 0x200,
		PipelineStageColorAttachmentOutput = 0x400,
		PipelineStageComputeShader = 0x800,
		PipelineStageTransfer = 0x1000,
		PipelineStageBottomOfPipe = 0x2000,
		PipelineStageHost = 0x4000,
		PipelineStageAllGraphics = 0x8000,
		PipelineStageAllCommands = 0x10000,
	}
}
