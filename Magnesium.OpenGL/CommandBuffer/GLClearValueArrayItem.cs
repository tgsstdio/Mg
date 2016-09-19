using System;

namespace Magnesium.OpenGL
{
	public struct GLClearValueArrayItem : IEquatable<GLClearValueArrayItem>
	{
		public GLClearAttachmentInfo Attachment { get; set; }
		public MgClearValue Value { get; set; }

		 
		#region IEquatable implementation
		public bool Equals (GLClearValueArrayItem other)
		{
			if (!Attachment.Equals(other.Attachment))
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

