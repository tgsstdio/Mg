using System;

namespace Magnesium.OpenGL
{
	public class GLCmdClearValuesParameter : IEquatable<GLCmdClearValuesParameter>
	{
		public GLClearValueArrayItem[] Attachments { get; set; }

		#region IEquatable implementation
		public bool Equals (GLCmdClearValuesParameter other)
		{
			if (Attachments == null && other.Attachments != null)
			{
				return false;
			}

			if (Attachments != null && other.Attachments == null)
			{
				return false;
			}

			if (Attachments.Length != other.Attachments.Length)
			{
				return false;
			}

			var noOfAttachments = Attachments.Length;
			for (var i = 0; i < noOfAttachments; ++i)
			{
				if (!Attachments [i].Equals (other.Attachments [i]))
					return false;
			}

			return true;
		}
		#endregion
	}
}

