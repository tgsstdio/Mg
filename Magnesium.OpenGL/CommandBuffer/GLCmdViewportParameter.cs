using System;

namespace Magnesium.OpenGL
{
	public class GLCmdViewportParameter : IEquatable<GLCmdViewportParameter>, IGLCmdMergeableParameter<GLCmdViewportParameter>
	{
		public GLCmdViewportParameter (uint first, MgViewport[] viewports)
		{
			First = (int)first;
			Count = viewports.Length;

			Viewport = ConvertTo2DViewports(first, viewports);
			DepthRange = ConvertToDepthRanges(first, viewports);
		}

		private static GLCmdArraySlice<float> ConvertTo2DViewports(uint first, MgViewport[] viewports)
		{
			const int factor = 4;
			var count = viewports.Length;
			var values = new float[factor * count];

			Func<float[], uint, MgViewport, uint> copyFn = (dest, offset, vp) => {
				dest [0 + offset] = vp.X;
				dest [1 + offset] = vp.Y;
				dest [2 + offset] = vp.Width;
				dest [3 + offset] = vp.Height;
				return 4;
			};

			GLCmdArraySlice<float>.CopyValues<MgViewport>(values, 0, viewports, copyFn);

			return new GLCmdArraySlice<float> (values, factor, first, count);
		}

		private static GLCmdArraySlice<double> ConvertToDepthRanges(uint first, MgViewport[] depthRanges)
		{
			Func<double[], uint, MgViewport, uint> copyFn = (dest, offset, vp) => {
				dest [0 + offset] = vp.MinDepth;
				dest [1 + offset] = vp.MaxDepth;
				return 2;
			};

			const int factor = 2;
			var count = depthRanges.Length;
			var values  = new double[factor * count];

			GLCmdArraySlice<double>.CopyValues<MgViewport>(values, 0, depthRanges, copyFn);

			return new GLCmdArraySlice<double> (values, factor, first, count);
		}

		public int First { get; private set; }
		public int Count { get; private set; }
		public GLCmdArraySlice<float> Viewport { get; private set; }
		public GLCmdArraySlice<double> DepthRange { get; private set; }

		#region IMergeable implementation

		private GLCmdViewportParameter(GLCmdArraySlice<float> views, GLCmdArraySlice<double> depths)
		{
			First = views.First;
			Count = views.Count;
			Viewport = views;
			DepthRange = depths;
		}

		public GLCmdViewportParameter Merge (GLCmdViewportParameter delta)
		{
			var combinedViews = Viewport.Merge (delta.Viewport);
			var combinedDepths = DepthRange.Merge (delta.DepthRange);

			return new GLCmdViewportParameter (combinedViews, combinedDepths);
		}

		#endregion

		public bool Equals (GLCmdViewportParameter other)
		{
			bool stillMatches = (this.First == other.First && this.Count == other.Count);

			if (!stillMatches)
				return false;

			stillMatches = this.Viewport.Matches(other.Viewport,
				(a, b) => Math.Abs(a - b) > float.Epsilon);

			if (!stillMatches)
				return false;

			// EVEN IF VALUES ARE HELD AS DOUBLE, USE FLOAT.EPSILON
			return this.DepthRange.Matches(other.DepthRange,
				(a, b) => Math.Abs(a - b) > float.Epsilon);
			
		}
	}
}

