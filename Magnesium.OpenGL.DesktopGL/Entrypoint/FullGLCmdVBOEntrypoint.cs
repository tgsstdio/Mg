using System;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLCmdVBOEntrypoint : IGLCmdVBOEntrypoint
	{
        private readonly IGLErrorHandler mErrHandler;
        public FullGLCmdVBOEntrypoint(IGLErrorHandler errHandler)
        {
            mErrHandler = errHandler;
        }

		#region ICmdVBOEntrypoint implementation

		public void BindIndexBuffer (uint vbo, uint bufferId)
		{
			GL.VertexArrayElementBuffer (vbo, bufferId);
            mErrHandler.LogGLError("BindIndexBuffer");
		}

		public void BindDoubleVertexAttribute (uint vbo, uint location, int size, GLVertexAttributeType pointerType, uint offset)
		{
			GL.Ext.EnableVertexArrayAttrib (vbo, location);
			GL.VertexArrayAttribLFormat (vbo, location, size, GetVertexAttribType(pointerType), offset);
            mErrHandler.LogGLError(nameof(BindDoubleVertexAttribute));

        }

		public void BindIntVertexAttribute (uint vbo, uint location, int size, GLVertexAttributeType pointerType, uint offset)
		{
			GL.Ext.EnableVertexArrayAttrib (vbo, location);
			GL.VertexArrayAttribIFormat (vbo, location, size, GetVertexAttribType(pointerType), offset);
            mErrHandler.LogGLError("BindIntVertexAttribute");
		}

		static VertexAttribType GetVertexAttribType (GLVertexAttributeType pointerType)
		{
			switch (pointerType)
			{
			case GLVertexAttributeType.Byte:
				return VertexAttribType.Byte;
			case GLVertexAttributeType.UnsignedByte:
				return VertexAttribType.UnsignedByte;

			case GLVertexAttributeType.Double:
				return VertexAttribType.Double;


			case GLVertexAttributeType.Float:
				return VertexAttribType.Float;
			case GLVertexAttributeType.HalfFloat:
				return VertexAttribType.HalfFloat;

			case GLVertexAttributeType.Int:
				return VertexAttribType.Int;
			case GLVertexAttributeType.UnsignedInt:
				return VertexAttribType.UnsignedInt;

			case GLVertexAttributeType.Int2101010Rev:
				return VertexAttribType.Int2101010Rev;
			case GLVertexAttributeType.UnsignedInt2101010Rev:
				return VertexAttribType.UnsignedInt2101010Rev;

			case GLVertexAttributeType.Short:
				return VertexAttribType.Short;
			case GLVertexAttributeType.UnsignedShort:
				return VertexAttribType.UnsignedShort;

			default:
				throw new NotSupportedException ();
			}
		}

		public void BindFloatVertexAttribute (uint vbo, uint location, int size, GLVertexAttributeType pointerType, bool isNormalized, uint offset)
		{
			GL.Ext.EnableVertexArrayAttrib (vbo, location);
            mErrHandler.LogGLError("BindFloatVertexAttribute.EnableVertexArrayAttrib");

            GL.VertexArrayAttribFormat (vbo, location, size, GetVertexAttribType(pointerType), isNormalized, offset);
            mErrHandler.LogGLError("BindFloatVertexAttribute.VertexArrayAttribFormat");
		}

		public void SetupVertexAttributeDivisor (uint vbo, uint location, uint divisor)
		{
			GL.VertexArrayBindingDivisor (vbo, location, divisor);
            mErrHandler.LogGLError("SetupVertexAttributeDivisor");
		}

		public uint GenerateVBO ()
		{
			var result = new uint[1];
			GL.CreateVertexArrays (1, result);
            mErrHandler.LogGLError("GenerateVBO");
			Debug.Assert (GL.IsVertexArray (result [0]));
			return result [0];
		}

		public void DeleteVBO (uint vbo)
		{
			var result = new uint[1];
			result [0] = vbo;

			//Debug.Assert (GL.IsVertexArray (vbo));
			// FIXME : FIGURE OUT WHY
			GL.DeleteVertexArray(vbo);
            mErrHandler.LogGLError("DeleteVBO");
        }

		public void AssociateBufferToLocation (uint vbo, uint location, uint bufferId, long offsets, uint stride)
		{
			if (stride >= (uint)int.MaxValue)
			{
				throw new ArgumentOutOfRangeException ("stride", "stride >= int.MaxValue");
			}

			GL.VertexArrayVertexBuffer (vbo, location, bufferId, new IntPtr (offsets), (int)stride);
            mErrHandler.LogGLError("AssociateBufferToLocation");
        }

		#endregion
	}
}

