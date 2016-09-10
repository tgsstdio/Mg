using Magnesium;
using System;

namespace Magnesium.OpenGL
{
	public interface IGLCmdDrawEntrypoint
	{
		void DrawIndexed (MgPrimitiveTopology topology, MgIndexType indexType, uint first, uint count, uint instanceCount, int vertexOffset);

		void DrawArraysIndirect (MgPrimitiveTopology topology, IntPtr indirect, uint count, uint stride);

		void DrawIndexedIndirect (MgPrimitiveTopology topology, MgIndexType indexType, IntPtr indirect, uint count, uint stride);

		void DrawArrays (MgPrimitiveTopology topology, uint first, uint count, uint instanceCount, uint firstInstance);
	}

}

