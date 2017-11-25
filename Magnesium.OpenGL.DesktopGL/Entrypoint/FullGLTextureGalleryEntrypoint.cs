using OpenTK.Graphics.OpenGL4;
using System;

namespace Magnesium.OpenGL.DesktopGL
{
    public class FullGLTextureGalleryEntrypoint : IGLTextureGalleryEntrypoint
    {
        private const uint BASE_TEXTURE = (uint)TextureUnit.Texture0;

        public void BindSampler(uint binding, int sampler)
        {
            GL.BindSampler(binding, (uint) sampler);
        }

        public void BindView(uint binding, MgImageViewType viewType, int texture)
        {
            ChangeTextureSlot(binding);
            GL.BindTexture(GetTarget(viewType), texture);
        }

        public int GetMaximumNumberOfTextureUnits()
        {
            OpenTK.Graphics.OpenGL4.GL.GetInteger(OpenTK.Graphics.OpenGL4.GetPName.MaxCombinedTextureImageUnits, out int result);
            return result;
        }

        public void UnbindSampler(uint binding)
        {
            GL.BindSampler(binding, 0U);
        }

        private static TextureTarget GetTarget(MgImageViewType viewType)
        {
            switch(viewType)
            {
                case MgImageViewType.TYPE_1D:
                    return TextureTarget.Texture1D;
                case MgImageViewType.TYPE_1D_ARRAY:
                    return TextureTarget.Texture1DArray;
                case MgImageViewType.TYPE_2D:
                    return TextureTarget.Texture2D;
                case MgImageViewType.TYPE_2D_ARRAY:
                    return TextureTarget.Texture2DArray;
                case MgImageViewType.TYPE_3D:
                    return TextureTarget.Texture3D;
                case MgImageViewType.TYPE_CUBE:
                    return TextureTarget.TextureCubeMap;
                case MgImageViewType.TYPE_CUBE_ARRAY:
                    return TextureTarget.TextureCubeMapArray;
                default:
                    throw new NotSupportedException();
            }
        }

        public void UnbindView(uint binding, MgImageViewType viewType)
        {
            ChangeTextureSlot(binding);
            GL.BindTexture(GetTarget(viewType), 0);
        }

        private static void ChangeTextureSlot(uint binding)
        {
            TextureUnit slot = (TextureUnit)(BASE_TEXTURE + binding);
            GL.ActiveTexture(slot);
        }
    }
}
