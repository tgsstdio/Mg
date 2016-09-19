using System;

namespace Magnesium.OpenGL
{
	/// <summary>
	/// GL cmd float4 array slice.
	/// </summary>
	public class GLCmdArraySlice<TFloat>
	{
		public GLCmdArraySlice (TFloat[] values, int factor, uint first, int count)
		{
			this.Factor = factor;
			this.Values = values;
			this.First = (int)first;
			this.Count = count;
		}

		public int Factor { get; private set; }	

		public TFloat[] Values {
			get;
			private set;
		}

		public int Count {
			get;
			private set;
		}

		public int First {
			get;
			private set;
		}

		public GLCmdArraySlice<TFloat> Merge(GLCmdArraySlice<TFloat> delta)
		{
			return MergeData (this.Factor, this, delta);
		}

		public static void CopyValues<TData>(TFloat[] dest, uint offset, TData[] src, Func<TFloat[], uint, TData, uint> copyFn)				
		{
			uint localOffset = offset;
			for (uint i = 0; i < src.Length; ++i)
			{					
				localOffset += copyFn (dest, localOffset, src [i]);
			}
		}

		public static int GetAdjustedLength (int basisLength, int deltaFirst, int deltaLength)
		{
			int basisMax = (basisLength - 1);
			int deltaMax = (deltaFirst + deltaLength - 1);

			return Math.Max (basisMax, deltaMax) + 1;
		}

		public static GLCmdArraySlice<TFloat> MergeData(int factor, GLCmdArraySlice<TFloat> basis, GLCmdArraySlice<TFloat> change)
		{
			var length = GetAdjustedLength(basis.Count, change.First, change.Count);
			var dest = new TFloat[factor * length];

			var combined = new GLCmdArraySlice<TFloat>(dest, basis.Factor, 0 , length);

			Array.Copy (basis.Values, dest, basis.Values.Length);
			var offset = (change.First * factor);
			var arrayCount = change.Count * factor;
			Array.Copy (change.Values, 0, dest, offset, arrayCount);

			return combined;
		}

		public bool Matches (GLCmdArraySlice<TFloat> other, Func<TFloat, TFloat, bool> isDifferent)
		{
			bool quickCheck = this.First == other.First
				&& this.Count == other.Count;

			if (!quickCheck)
				return false;

			for (uint i = 0; i < Values.Length; ++i)
			{
				if (isDifferent(this.Values [i], other.Values [i]))
					return false;
			}

			return true;
		}
	}

}

