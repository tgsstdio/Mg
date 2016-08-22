using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkQueryPipelineStatisticFlags : uint
	{
		QueryPipelineStatisticInputAssemblyVertices = 0x1,
		QueryPipelineStatisticInputAssemblyPrimitives = 0x2,
		QueryPipelineStatisticVertexShaderInvocations = 0x4,
		QueryPipelineStatisticGeometryShaderInvocations = 0x8,
		QueryPipelineStatisticGeometryShaderPrimitives = 0x10,
		QueryPipelineStatisticClippingInvocations = 0x20,
		QueryPipelineStatisticClippingPrimitives = 0x40,
		QueryPipelineStatisticFragmentShaderInvocations = 0x80,
		QueryPipelineStatisticTessellationControlShaderPatches = 0x100,
		QueryPipelineStatisticTessellationEvaluationShaderInvocations = 0x200,
		QueryPipelineStatisticComputeShaderInvocations = 0x400,
	}
}
