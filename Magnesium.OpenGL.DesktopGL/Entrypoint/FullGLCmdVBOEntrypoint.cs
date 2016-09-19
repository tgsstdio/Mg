using System;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLCmdVBOEntrypoint : IGLCmdVBOEntrypoint
	{
		#region ICmdVBOEntrypoint implementation

		public void BindIndexBuffer (int vbo, int bufferId)
		{
			GL.VertexArrayElementBuffer (vbo, bufferId);

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("BindIndexBuffer : " + error);
				}
			}
		}

		public void BindDoubleVertexAttribute (int vbo, int location, int size, GLVertexAttributeType pointerType, int offset)
		{
			GL.Ext.EnableVertexArrayAttrib (vbo, location);
			GL.VertexArrayAttribLFormat (vbo, location, size, (All)GetVertexAttribType(pointerType), offset);
			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("BindDoubleVertexAttribute : " + error);
				}
			}
		}

		public void BindIntVertexAttribute (int vbo, int location, int size, GLVertexAttributeType pointerType, int offset)
		{
			GL.Ext.EnableVertexArrayAttrib (vbo, location);
			GL.VertexArrayAttribIFormat (vbo, location, size, GetVertexAttribType(pointerType), offset);

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("BindIntVertexAttribute : " + error);
				}
			}
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

		public void BindFloatVertexAttribute (int vbo, int location, int size, GLVertexAttributeType pointerType, bool isNormalized, int offset)
		{
			GL.Ext.EnableVertexArrayAttrib (vbo, location);

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("EnableVertexArrayAttrib : " + error);
				}
			}

			GL.VertexArrayAttribFormat (vbo, location, size, GetVertexAttribType(pointerType), isNormalized, offset);

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("BindFloatVertexAttribute : " + error);
				}
			}
		}

		public void SetupVertexAttributeDivisor (int vbo, int location, int divisor)
		{
			GL.VertexArrayBindingDivisor (vbo, location, divisor);

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("SetupVertexAttributeDivisor : " + error);
				}
			}
		}

		public int GenerateVBO ()
		{
			var result = new int[1];
			GL.CreateVertexArrays (1, result);

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("GenerateVBO : " + error);
				}
			}
			Debug.Assert (GL.IsVertexArray (result [0]));

			return result [0];
		}

		public void DeleteVBO (int vbo)
		{
			int[] result = new int[1];
			result [0] = vbo;

			Debug.Assert (GL.IsVertexArray (vbo));

			// FIXME : FIGURE OUT WHY
			GL.DeleteVertexArray(result [0]);

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("DeleteVBO : " + error);
				}
			}
		}

		public void AssociateBufferToLocation (int vbo, int location, int bufferId, long offsets, uint stride)
		{
			if (stride >= (uint)int.MaxValue)
			{
				throw new ArgumentOutOfRangeException ("stride", "stride >= int.MaxValue");
			}

			GL.VertexArrayVertexBuffer (vbo, location, bufferId, new IntPtr (offsets), (int)stride);

			{
				var error = GL.GetError ();
				if (error != ErrorCode.NoError)
				{
					Debug.WriteLine ("AssociateBufferToLocation : " + error);
				}
			}
		}

		#endregion
	}
}

