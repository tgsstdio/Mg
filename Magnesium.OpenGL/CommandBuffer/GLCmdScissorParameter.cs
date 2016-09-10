using System;

namespace Magnesium.OpenGL
{
	public class GLCmdScissorParameter : IEquatable<GLCmdScissorParameter>, IGLCmdMergeableParameter<GLCmdScissorParameter>
	{
		public GLCmdScissorParameter (uint first, MgRect2D[] scissors)
		{
			const int factor = 4;
			var count = scissors.Length;
			var values = new int[factor * count];

			Func<int[], uint, MgRect2D, uint> copyFn = (dest, offset, sc) => {
				dest [0 + offset] = sc.Offset.X;
				dest [1 + offset] = sc.Offset.Y;
				dest [2 + offset] = (int)sc.Extent.Width;
				dest [3 + offset] = (int)sc.Extent.Height;
				return 4;
			};

			GLCmdArraySlice<int>.CopyValues<MgRect2D>(values, 0, scissors, copyFn);

			Parameters = new GLCmdArraySlice<int> (values, factor, first, count);
		}

		private GLCmdScissorParameter(GLCmdArraySlice<int> scissors)
		{
			Parameters = scissors;
		}

		public GLCmdArraySlice<int> Parameters { get; private set; }

		#region IMergeable implementation

		public GLCmdScissorParameter Merge (GLCmdScissorParameter delta)
		{
			var combined = this.Parameters.Merge (delta.Parameters);
			return new GLCmdScissorParameter (combined);
		}

		#endregion

		#region IEquatable implementation

		public bool Equals (GLCmdScissorParameter other)
		{
			return this.Parameters.Matches(other.Parameters,
				(a,b) => a != b);
		}

		#endregion
	}
}

