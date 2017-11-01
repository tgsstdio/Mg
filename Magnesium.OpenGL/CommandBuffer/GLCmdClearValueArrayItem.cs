using System;

namespace Magnesium.OpenGL.Internals
{
	public struct GLCmdClearValueArrayItem : IEquatable<GLCmdClearValueArrayItem>
	{
		public GLRenderPassClearAttachment Attachment { get; set; }
		public MgClearValue Value { get; set; }
        public MgColor4f Color { get; set; }

        #region IEquatable implementation
        public bool Equals (GLCmdClearValueArrayItem other)
		{
			if (!Attachment.Equals(other.Attachment))
			{
				return false;
			}

            if (!Color.Equals(other.Color))
            {
                return false;
            }

			switch (this.Attachment.AttachmentType)
			{
			case GLClearAttachmentType.COLOR_FLOAT:
				return this.Value.Color.Float32.Equals (other.Value.Color.Float32);
			case GLClearAttachmentType.COLOR_INT:
				return this.Value.Color.Int32.Equals (other.Value.Color.Int32);
			case GLClearAttachmentType.COLOR_UINT:
				return this.Value.Color.Uint32.Equals (other.Value.Color.Uint32);
			case GLClearAttachmentType.DEPTH_STENCIL:				
				return this.Value.DepthStencil.Equals (other.Value.DepthStencil);
			default:
				throw new NotSupportedException ();
			}


		}
		#endregion
	}
}

