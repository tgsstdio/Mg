
namespace Magnesium.OpenGL
{
	public interface IGLRenderPass : IMgRenderPass
	{
		GLClearAttachmentInfo[] AttachmentFormats { get; }
	}

}

