using System;


namespace Magnesium.OpenGL
{
	public struct GLClearAttachmentInfo : IEquatable<GLClearAttachmentInfo>
	{
		public MgFormat Format { get; set; }
		public MgAttachmentLoadOp LoadOp { get; set; }
		public MgAttachmentLoadOp StencilLoadOp { get; set; }
		public GLClearAttachmentType AttachmentType { get; set; }
		public float Divisor { get; set; }


		#region IEquatable implementation
		public bool Equals (GLClearAttachmentInfo other)
		{
			if (Format != other.Format)
				return false;

			if (LoadOp != other.LoadOp)
				return false;

			if (StencilLoadOp != other.StencilLoadOp)
				return false;

			if (AttachmentType != other.AttachmentType)
				return false;

			return Math.Abs (Divisor - other.Divisor) <= float.Epsilon;
		}
		#endregion
	}
}

