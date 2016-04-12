using System;

namespace Magnesium
{
    [Flags] 
	public enum MgPipelineStageFlagBits : UInt32
	{
		// Before subsequent commands are processed
		TOP_OF_PIPE_BIT = 1 << 0,
		// Draw/DispatchIndirect command fetch
		DRAW_INDIRECT_BIT = 1 << 1,
		// Vertex/index fetch
		VERTEX_INPUT_BIT = 1 << 2,
		// Vertex shading
		VERTEX_SHADER_BIT = 1 << 3,
		// Tessellation control shading
		TESSELLATION_CONTROL_SHADER_BIT = 1 << 4,
		// Tessellation evaluation shading
		TESSELLATION_EVALUATION_SHADER_BIT = 1 << 5,
		// Geometry shading
		GEOMETRY_SHADER_BIT = 1 << 6,
		// Fragment shading
		FRAGMENT_SHADER_BIT = 1 << 7,
		// Early fragment (depth and stencil) tests
		EARLY_FRAGMENT_TESTS_BIT = 1 << 8,
		// Late fragment (depth and stencil) tests
		LATE_FRAGMENT_TESTS_BIT = 1 << 9,
		// Color attachment writes
		COLOR_ATTACHMENT_OUTPUT_BIT = 1 << 10,
		// Compute shading
		COMPUTE_SHADER_BIT = 1 << 11,
		// Transfer/copy operations
		TRANSFER_BIT = 1 << 12,
		// After previous commands have completed
		BOTTOM_OF_PIPE_BIT = 1 << 13,
		// Indicates host (CPU) is a source/sink of the dependency
		HOST_BIT = 1 << 14,
		// All stages of the graphics pipeline
		ALL_GRAPHICS_BIT = 1 << 15,
		// All stages supported on the queue
		ALL_COMMANDS_BIT = 1 << 16,
	};
}

