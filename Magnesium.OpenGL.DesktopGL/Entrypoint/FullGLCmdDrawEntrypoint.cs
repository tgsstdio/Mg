using System;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLCmdDrawEntrypoint : IGLCmdDrawEntrypoint
	{
		#region ICmdDrawCapabilities implementation

		PrimitiveType GetPrimitiveType (MgPrimitiveTopology topology)
		{
			switch (topology)
			{
			case MgPrimitiveTopology.LINE_LIST:
				return PrimitiveType.Lines;
			case MgPrimitiveTopology.POINT_LIST:
				return PrimitiveType.Points;
			case MgPrimitiveTopology.TRIANGLE_LIST:
				return PrimitiveType.Triangles;
			case MgPrimitiveTopology.LINE_STRIP:
				return PrimitiveType.LineStrip;
			case MgPrimitiveTopology.TRIANGLE_FAN:
				return PrimitiveType.TriangleFan;
			case MgPrimitiveTopology.LINE_LIST_WITH_ADJACENCY:
				return PrimitiveType.LinesAdjacency;
			case MgPrimitiveTopology.LINE_STRIP_WITH_ADJACENCY:
				return PrimitiveType.LineStripAdjacency;
			case MgPrimitiveTopology.TRIANGLE_LIST_WITH_ADJACENCY:
				return PrimitiveType.TrianglesAdjacency;
			case MgPrimitiveTopology.TRIANGLE_STRIP_WITH_ADJACENCY:
				return PrimitiveType.TriangleStripAdjacency;
			case MgPrimitiveTopology.PATCH_LIST:
				return PrimitiveType.Patches;
			default:
				throw new NotSupportedException ();
			}
		}

		public void DrawArrays (MgPrimitiveTopology topology, uint first, uint count, uint instanceCount, uint firstInstance)
		{
			//void glDrawArraysInstancedBaseInstance(GLenum mode​, GLint first​, GLsizei count​, GLsizei primcount​, GLuint baseinstance​);
			//mDrawCommands.Add (mIncompleteDrawCommand);
			// first => firstVertex
			// count => vertexCount
			// primcount => instanceCount Specifies the number of instances of the indexed geometry that should be drawn.
			// baseinstance => firstInstance Specifies the base instance for use in fetching instanced vertex attributes.

			if (first >= (uint) int.MaxValue)
			{
				throw new ArgumentOutOfRangeException ("first","first > int.MaxValue");
			}

			if (count >= (uint) int.MaxValue)
			{
				throw new ArgumentOutOfRangeException ("count","count > int.MaxValue");
			}

			if (instanceCount >= (uint) int.MaxValue)
			{
				throw new ArgumentOutOfRangeException ("instanceCount", "instanceCount > int.MaxValue");
			}

			GL.DrawArraysInstancedBaseInstance (GetPrimitiveType (topology), (int)first, (int)count, (int)instanceCount, firstInstance);
		}

		static All GetIndexBufferType (MgIndexType indexType)
		{
			switch (indexType)
			{
			case MgIndexType.UINT16:
				return All.UnsignedShort;
			case MgIndexType.UINT32:
				return All.UnsignedInt;		
			default:
				throw new NotSupportedException ();
			}
		}

		public void DrawIndexedIndirect (MgPrimitiveTopology topology, MgIndexType indexType, IntPtr indirect, uint count, uint stride)
		{
			//			typedef struct VkDrawIndexedIndirectCommand {
			//				uint32_t    indexCount;
			//				uint32_t    instanceCount;
			//				uint32_t    firstIndex;
			//				int32_t     vertexOffset;
			//				uint32_t    firstInstance;
			//			} VkDrawIndexedIndirectCommand;
			// void glMultiDrawElementsIndirect(GLenum mode​, GLenum type​, const void *indirect​, GLsizei drawcount​, GLsizei stride​);
			// indirect  => buffer + offset (IntPtr)
			// drawcount => drawcount
			// stride => stride
			//			glDrawElementsInstancedBaseVertexBaseInstance(mode,
			//				cmd->count,
			//				type,
			//				cmd->firstIndex * size-of-type,
			//				cmd->instanceCount,
			//				cmd->baseVertex,
			//				cmd->baseInstance);
			//			typedef  struct {
			//				uint  count;
			//				uint  instanceCount;
			//				uint  firstIndex;
			//				uint  baseVertex; // TODO: negetive index
			//				uint  baseInstance;
			//			} DrawElementsIndirectCommand;
			//mDrawCommands.Add (mIncompleteDrawCommand);

			if (count >= (uint)int.MaxValue)
			{
				throw new ArgumentOutOfRangeException ("count", "count >= int.MaxValue");
			}

			if (stride >= (uint)int.MaxValue)
			{
				throw new ArgumentOutOfRangeException ("stride", "stride >= int.MaxValue");
			}

			GL.MultiDrawElementsIndirect ((All)GetPrimitiveType (topology), GetIndexBufferType (indexType), indirect, (int)count, (int)stride);
		}

		public void DrawArraysIndirect (MgPrimitiveTopology topology, IntPtr indirect, uint count, uint stride)
		{
			// ARB_multi_draw_indirect
			//			typedef struct VkDrawIndirectCommand {
			//				uint32_t    vertexCount;
			//				uint32_t    instanceCount; 
			//				uint32_t    firstVertex; 
			//				uint32_t    firstInstance;
			//			} VkDrawIndirectCommand;
			// glMultiDrawArraysIndirect 
			//void glMultiDrawArraysIndirect(GLenum mode​, const void *indirect​, GLsizei drawcount​, GLsizei stride​);
			// indirect => buffer + offset IntPtr
			// drawCount => drawCount
			// stride => stride
			//			typedef  struct {
			//				uint  count;
			//				uint  instanceCount;
			//				uint  first;
			//				uint  baseInstance;
			//			} DrawArraysIndirectCommand;
			//mDrawCommands.Add (mIncompleteDrawCommand);

			if (count >= (uint)int.MaxValue)
			{
				throw new ArgumentOutOfRangeException ("count", "count >= int.MaxValue");
			}

			if (stride >= (uint)int.MaxValue)
			{
				throw new ArgumentOutOfRangeException ("stride", "stride >= int.MaxValue");
			}

			GL.MultiDrawArraysIndirect (GetPrimitiveType (topology), indirect, (int)count, (int)stride);
		}

		static DrawElementsType GetElementType (MgIndexType indexType)
		{
			switch (indexType)
			{
			case MgIndexType.UINT16:
				return DrawElementsType.UnsignedShort;
			case MgIndexType.UINT32:
				return DrawElementsType.UnsignedInt;
			default:
				throw new NotSupportedException ();
			}
		}

		static IntPtr GetByteOffset (uint first, MgIndexType indexType)
		{
			int elementSize = (indexType == MgIndexType.UINT32) ? 4 : 2;
			return new IntPtr(first * elementSize);
		}

		public void DrawIndexed (MgPrimitiveTopology topology, MgIndexType indexType, uint first, uint count, uint instanceCount, int vertexOffset)
		{
			// void glDrawElementsInstancedBaseVertex(GLenum mode​, GLsizei count​, GLenum type​, GLvoid *indices​, GLsizei primcount​, GLint basevertex​);
			// count => indexCount Specifies the number of elements to be rendered. (divide by elements)
			// indices => firstIndex Specifies a byte offset (cast to a pointer type) (multiple by data size)
			// primcount => instanceCount Specifies the number of instances of the indexed geometry that should be drawn.
			// basevertex => vertexOffset Specifies a constant that should be added to each element of indices​ when chosing elements from the enabled vertex arrays.
			// TODO : need to handle negetive offset
			//mDrawCommands.Add (mIncompleteDrawCommand);

			if (count >= (uint)int.MaxValue)
			{
				throw new ArgumentOutOfRangeException ("count", "count >= int.MaxValue");
			}

			if (instanceCount >= (uint)int.MaxValue)
			{
				throw new ArgumentOutOfRangeException ("instanceCount", "instanceCount >= int.MaxValue");
			}

			GL.DrawElementsInstancedBaseVertex (GetPrimitiveType (topology), (int)count, GetElementType (indexType), GetByteOffset(first, indexType), (int)instanceCount, vertexOffset);
		}
		#endregion
	}
}

