using System;

namespace Magnesium
{
    [Flags] 
	public enum MgPipelineStageFlagBits : UInt32
	{
		/// <summary>
		/// Before subsequent commands are processed
		/// </summary>
		TOP_OF_PIPE_BIT = 1 << 0,
		/// <summary>
		/// Draw/DispatchIndirect command fetch
		/// </summary>
		DRAW_INDIRECT_BIT = 1 << 1,
		/// <summary>
		/// Vertex/index fetch
		/// </summary>
		VERTEX_INPUT_BIT = 1 << 2,
		/// <summary>
		/// Vertex shading
		/// </summary>
		VERTEX_SHADER_BIT = 1 << 3,
		/// <summary>
		/// Tessellation control shading
		/// </summary>
		TESSELLATION_CONTROL_SHADER_BIT = 1 << 4,
		/// <summary>
		/// Tessellation evaluation shading
		/// </summary>
		TESSELLATION_EVALUATION_SHADER_BIT = 1 << 5,
		/// <summary>
		/// Geometry shading
		/// </summary>
		GEOMETRY_SHADER_BIT = 1 << 6,
		/// <summary>
		/// Fragment shading
		/// </summary>
		FRAGMENT_SHADER_BIT = 1 << 7,
		/// <summary>
		/// Early fragment (depth and stencil) tests
		/// </summary>
		EARLY_FRAGMENT_TESTS_BIT = 1 << 8,
		/// <summary>
		/// Late fragment (depth and stencil) tests
		/// </summary>
		LATE_FRAGMENT_TESTS_BIT = 1 << 9,
		/// <summary>
		/// Color attachment writes
		/// </summary>
		COLOR_ATTACHMENT_OUTPUT_BIT = 1 << 10,
		/// <summary>
		/// Compute shading
		/// </summary>
		COMPUTE_SHADER_BIT = 1 << 11,
		/// <summary>
		/// Transfer/copy operations
		/// </summary>
		TRANSFER_BIT = 1 << 12,
		/// <summary>
		/// After previous commands have completed
		/// </summary>
		BOTTOM_OF_PIPE_BIT = 1 << 13,
		/// <summary>
		/// Indicates host (CPU) is a source/sink of the dependency
		/// </summary>
		HOST_BIT = 1 << 14,
		/// <summary>
		/// All stages of the graphics pipeline
		/// </summary>
		ALL_GRAPHICS_BIT = 1 << 15,
		/// <summary>
		/// All stages supported on the queue
		/// </summary>
		ALL_COMMANDS_BIT = 1 << 16,
	};
}

