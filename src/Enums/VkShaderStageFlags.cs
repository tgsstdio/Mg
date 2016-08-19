using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkShaderStageFlags : uint
	{
		ShaderStageVertex = 0x1,
		ShaderStageTessellationControl = 0x2,
		ShaderStageTessellationEvaluation = 0x4,
		ShaderStageGeometry = 0x8,
		ShaderStageFragment = 0x10,
		ShaderStageCompute = 0x20,
		ShaderStageAllGraphics = 0x0000001F,
		ShaderStageAll = 0x7FFFFFFF,
	}
}
