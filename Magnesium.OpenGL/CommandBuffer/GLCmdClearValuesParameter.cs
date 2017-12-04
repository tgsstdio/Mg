using System;

namespace Magnesium.OpenGL.Internals
{
	public class GLCmdClearValuesParameter : IEquatable<GLCmdClearValuesParameter>
	{
		public GLCmdClearValueArrayItem[] Attachments { get; set; }
        public GLQueueClearBufferMask Bitmask { get; internal set; }

        #region IEquatable implementation
        public bool Equals (GLCmdClearValuesParameter other)
		{
            if (Bitmask != other.Bitmask)
            {
                return false;
            }

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

