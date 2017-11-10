// This code was written for the OpenTK library and has been released
// to the Public Domain.
// It is provided "as is" without express or implied warranty of any kind.

using System;
using System.Diagnostics;

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using Magnesium;
using Examples.Shapes;
using System.Runtime.InteropServices;

namespace Examples.Tutorial
{
    public class SimpleFBO : GameWindow
    {
        public SimpleFBO()
            : base(800, 400)
        {
        }

        uint ColorTexture;
        uint DepthTexture;
        uint FBOHandle;

        const int TEXTURE_SIZE = 512;

        Examples.Shapes.DrawableShape Object;
        private IMgOffscreenDeviceAttachment mColorReplacement;
        private IMgOffscreenDeviceAttachment mDepthReplacement;
        private IMgEffectFramework mOffscreen;

        #region CreateOffscreenShader methods

        string vertexShaderSource = @"
#version 450

precision highp float;

uniform mat4 projection_matrix;
uniform mat4 modelview_matrix;

layout(location = 0) in vec3 in_position;
layout(location = 1) in vec3 in_normal;
layout(location = 2) in vec2 in_uv;

out vec3 normal;
out vec2 texCoords;

void main(void)
{
  //works only for orthogonal modelview
  texCoords = in_uv;
  normal = (modelview_matrix * vec4(in_normal, 0)).xyz;
  
  gl_Position = projection_matrix * modelview_matrix * vec4(in_position, 1);
}";

        string fragmentShaderSource = @"
#version 450

precision highp float;

const vec3 ambient = vec3(0.1, 0.1, 0.1);
const vec3 lightVecNormalized = normalize(vec3(0.5, 0.5, 2.0));
const vec3 lightColor = vec3(0.0, 1.0, 0.0);

in vec3 normal;
in vec2 texCoords;

out vec4 out_frag_color;

void main(void)
{
   float diffuse = clamp(dot(lightVecNormalized, normalize(normal)), 0.0, 1.0);
  out_frag_color = vec4(ambient + diffuse * lightColor, 1.0);
  //  out_frag_color = vec4(0, 0, 1, 1);
}";

        int mOffscreenShaderProgram;
        int mProjectionMatrixLocation;
        int mModelviewMatrixLocation;

        void CreateOffscreenShader()
        {
            var vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            var fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(vertexShaderHandle, vertexShaderSource);
            GL.ShaderSource(fragmentShaderHandle, fragmentShaderSource);

            GL.CompileShader(vertexShaderHandle);
            GL.CompileShader(fragmentShaderHandle);

            Console.WriteLine(GL.GetShaderInfoLog(vertexShaderHandle));
            Console.WriteLine(GL.GetShaderInfoLog(fragmentShaderHandle));

            // Create program
            mOffscreenShaderProgram = GL.CreateProgram();

            GL.AttachShader(mOffscreenShaderProgram, vertexShaderHandle);
            GL.AttachShader(mOffscreenShaderProgram, fragmentShaderHandle);

             GL.BindAttribLocation(mOffscreenShaderProgram, 0, "in_position");
            GL.BindAttribLocation(mOffscreenShaderProgram, 1, "in_normal");
            GL.BindAttribLocation(mOffscreenShaderProgram, 2, "in_uv");

            GL.LinkProgram(mOffscreenShaderProgram);
            Console.WriteLine(GL.GetProgramInfoLog(mOffscreenShaderProgram));

            GL.DeleteShader(vertexShaderHandle);
            GL.DeleteShader(fragmentShaderHandle);

            //GL.UseProgram(mShaderProgramHandle);

            GL.UseProgram(mOffscreenShaderProgram);
            // Set uniforms
            mProjectionMatrixLocation = GL.GetUniformLocation(mOffscreenShaderProgram, "projection_matrix");
            mModelviewMatrixLocation = GL.GetUniformLocation(mOffscreenShaderProgram, "modelview_matrix");

            OpenTK.Matrix4 projectionMatrix = OpenTK.Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, TEXTURE_SIZE / (float)TEXTURE_SIZE, 2.5f, 6f);
            Matrix4 modelviewMatrix = Matrix4.LookAt(0f, 0f, 4.5f, 0f, 0f, 0f, 0f, 1f, 0f);
           // var projectionMatrix = Matrix4.Identity;
           // var modelviewMatrix = Matrix4.Translation(0, 0, -2.5f);

            GL.UniformMatrix4(mProjectionMatrixLocation, false, ref projectionMatrix);
            GL.UniformMatrix4(mModelviewMatrixLocation, false, ref modelviewMatrix);

            GL.UseProgram(0);
        }

        #endregion

        #region GenerateVAO methods 

        private int mVAOHandle;
        private int mPositionVertexBuffer;
        private PrimitiveType mPrimitiveType;
        private int mNoOfIndices;
        private void GenerateVAO(DrawableShape item)
        {
            item.GetArraysforVBO(out PrimitiveType priType, out VertexT2fN3fV3f[] vertices, out uint[] indices);
            mPrimitiveType = priType;

            mNoOfIndices = indices.Length;

            mPositionVertexBuffer = GenerateVertexBuffer(vertices, indices);

            var dataStride = Marshal.SizeOf(typeof(VertexT2fN3fV3f));
            mVAOHandle = GenerateVertexArray(indices, mPositionVertexBuffer, dataStride);
            // GL.DeleteVertexArrays(1, result);
        }

        private static int GenerateVertexArray(uint[] indices, int buffer, int dataStride)
        {
            int[] result = new int[1];
            GL.CreateVertexArrays(1, result);

            Debug.Assert(GL.IsVertexArray(result[0]));

            var vaoHandle = result[0];

            // ALWAYS OFFSET OF ZERO
            GL.VertexArrayElementBuffer(vaoHandle, buffer);

            var indexSize = (sizeof(uint) * indices.Length);
            var positionOffset = new IntPtr(indexSize);
            const int BINDING_INDEX = 0;
            GL.VertexArrayVertexBuffer(vaoHandle, BINDING_INDEX, buffer, positionOffset, dataStride);

            const int POSITION_ATTRIB = 0;
            const int NORMAL_ATTRIB = 1;
            const int UV_ATTRIB = 2;

            GL.EnableVertexArrayAttrib(vaoHandle, POSITION_ATTRIB);
            GL.EnableVertexArrayAttrib(vaoHandle, NORMAL_ATTRIB);
            GL.EnableVertexArrayAttrib(vaoHandle, UV_ATTRIB);

            GL.VertexArrayAttribBinding(vaoHandle, POSITION_ATTRIB, BINDING_INDEX);
            var positionAttribOffset = sizeof(float) * 5;
            GL.VertexArrayAttribFormat(vaoHandle, POSITION_ATTRIB, 3, VertexAttribType.Float, false, positionAttribOffset);

            GL.VertexArrayAttribBinding(vaoHandle, NORMAL_ATTRIB, BINDING_INDEX);
            var normalAttribOffset = sizeof(float) * 2;
            GL.VertexArrayAttribFormat(vaoHandle, NORMAL_ATTRIB, 3, VertexAttribType.Float, false, normalAttribOffset);

            GL.VertexArrayAttribBinding(vaoHandle, UV_ATTRIB, BINDING_INDEX);
            var uvAttribOffset = 0;
            GL.VertexArrayAttribFormat(vaoHandle, UV_ATTRIB, 2, VertexAttribType.Float, false, uvAttribOffset);

            return vaoHandle;
        }

        private static int GenerateVertexBuffer<TData>(TData[] vertices, uint[] indices)
        {
            var buffers = new int[1];
            // ARB_direct_state_access
            // Allows buffer objects to be initialised without binding them
            GL.CreateBuffers(1, buffers);

            var positionVboHandle = buffers[0];

            var stride = Marshal.SizeOf(typeof(TData));
            var indexSize = (sizeof(uint) * indices.Length);
            var positionOffset = indexSize;
            var positionSize = (stride * vertices.Length);
            var totalLength = indexSize + positionSize;

            AllocateDeviceMemory(positionVboHandle, totalLength);

            TransferVertexData(vertices, indices, positionVboHandle, indexSize, positionOffset, positionSize);

            //GL.DeleteBuffers(1, buffers);

            return positionVboHandle;
        }

        private static void TransferVertexData<TData>(TData[] vertices, uint[] indices, int positionVboHandle, int indexSize, int positionOffset, int positionSize)
        {
            BufferAccessMask rangeFlags = BufferAccessMask.MapWriteBit | BufferAccessMask.MapPersistentBit | BufferAccessMask.MapCoherentBit;
            IntPtr indexDest = GL.MapNamedBufferRange(positionVboHandle, IntPtr.Zero, indexSize, rangeFlags);
            // Copy the struct to unmanaged memory.	
            CopyUint32s(indexDest, indices, 0, indexSize, 0, indices.Length);
            GL.Ext.UnmapNamedBuffer(positionVboHandle);

            IntPtr vertexDest = GL.MapNamedBufferRange(positionVboHandle, new IntPtr(positionOffset), positionSize, rangeFlags);
            CopyValueData<TData>(vertexDest, vertices, 0, positionSize, 0, vertices.Length);
            GL.Ext.UnmapNamedBuffer(positionVboHandle);
        }

        private static void AllocateDeviceMemory(int positionVboHandle, int totalLength)
        {
            BufferStorageFlags flags = BufferStorageFlags.MapWriteBit | BufferStorageFlags.MapPersistentBit | BufferStorageFlags.MapCoherentBit;
            GL.NamedBufferStorage(positionVboHandle, totalLength, IntPtr.Zero, flags);
        }

        static void CopyUint32s(IntPtr dest, uint[] data, int destOffset, int sizeInBytes, int startIndex, int elementCount)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            const int stride = sizeof(uint);
            if (sizeInBytes < (stride * elementCount))
            {
                throw new ArgumentOutOfRangeException("sizeInBytes");
            }

            var localData = new byte[sizeInBytes];

            var srcOffset = startIndex * stride;
            var totalBytesToCopy = (int)sizeInBytes;
            Buffer.BlockCopy(data, srcOffset, localData, 0, totalBytesToCopy);

            IntPtr itemDest = IntPtr.Add(dest, destOffset);
            Marshal.Copy(localData, 0, itemDest, totalBytesToCopy);
        }

        static void CopyValueData<TData>(IntPtr dest, TData[] data, int destOffset, int sizeInBytes, int startIndex, int elementCount)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            int stride = Marshal.SizeOf(typeof(TData));
            if (sizeInBytes < (stride * elementCount))
            {
                throw new ArgumentOutOfRangeException("sizeInBytes");
            }

            // Map and copy
            var itemOffset = destOffset;
            for (var i = 0; i < elementCount; i += 1)
            {
                var index = i + startIndex;
                IntPtr itemDest = IntPtr.Add(dest, itemOffset);
                Marshal.StructureToPtr(data[index], itemDest, false);
                itemOffset += stride;
            }
        }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!GL.GetString(StringName.Extensions).Contains("GL_EXT_framebuffer_object"))
            {
                throw new NotSupportedException(
                     "GL_EXT_framebuffer_object extension is required. Please update your drivers.");
            }

            CreateOffscreenShader();
            Object = new Shapes.TorusKnot(256, 32, 0.2, 7,8, 1, true);

            GenerateVAO(Object);

            GL.Enable(EnableCap.DepthTest);
            GL.ClearDepth(1.0);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Enable(EnableCap.CullFace);

            var imgFormat = new Magnesium.OpenGL.DesktopGL.DesktopGLImageFormatEntrypoint();
            var errHandler = new Magnesium.OpenGL.DesktopGL.FullGLErrorHandler();
            var extensions = new Magnesium.OpenGL.DesktopGL.FullGLExtensionLookup();
            extensions.Initialize();
            var capabilities = new Magnesium.OpenGL.DesktopGL.FullGLFramebufferSupport(extensions);
            var selector = new Magnesium.OpenGL.DesktopGL.FullGLFramebufferHelperSelector(capabilities, errHandler);
            selector.Initialize();

            var deviceEntrypoint = new Magnesium.OpenGL.DefaultGLDeviceEntrypoint(
                null,
                new Magnesium.OpenGL.DesktopGL.FullGLSamplerEntrypoint(errHandler),
                new Magnesium.OpenGL.DesktopGL.FullGLDeviceImageEntrypoint(errHandler),
                new Magnesium.OpenGL.DesktopGL.FullGLDeviceImageViewEntrypoint(errHandler, imgFormat),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                selector
               );
            var configuration = new MockGraphicsConfiguration(deviceEntrypoint);
            var entrypoint = new Magnesium.OpenGL.GLOffscreenDeviceEntrypoint();
            var factory = new MgOffscreenDeviceFactory(configuration, entrypoint);

            mColorReplacement = factory.CreateColorAttachment(MgFormat.R8G8B8A8_UNORM, TEXTURE_SIZE, TEXTURE_SIZE);
            var internalView = mColorReplacement.View as Magnesium.OpenGL.Internals.IGLImageView;
            ColorTexture = (uint) internalView.TextureId;

            // Create Color Tex
       //     GL.GenTextures(1, out ColorTexture);
            GL.BindTexture(TextureTarget.Texture2D, ColorTexture);
            //   GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, TextureSize, TextureSize, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
            // GL.Ext.GenerateMipmap( GenerateMipmapTarget.Texture2D );

            mDepthReplacement = factory.CreateDepthStencilAttachment(MgFormat.D16_UNORM, TEXTURE_SIZE, TEXTURE_SIZE);
            var depthView = mDepthReplacement.View as Magnesium.OpenGL.Internals.IGLImageView;
            DepthTexture = (uint)depthView.TextureId;

            // Create Depth Tex
           // GL.GenTextures(1, out DepthTexture);
            GL.BindTexture(TextureTarget.Texture2D, DepthTexture);
            // GL.TexImage2D(TextureTarget.Texture2D, 0, (PixelInternalFormat)All.DepthComponent32, TextureSize, TextureSize, 0, PixelFormat.DepthComponent, PixelType.UnsignedInt, IntPtr.Zero);
            //GL.TexImage2D(TextureTarget.Texture2D, 0, (PixelInternalFormat)All.DepthComponent16, TextureSize, TextureSize, 0, PixelFormat.DepthComponent, PixelType.UnsignedShort, IntPtr.Zero);
            // things go horribly wrong if DepthComponent's Bitcount does not match the main Framebuffer's Depth
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
            // GL.Ext.GenerateMipmap( GenerateMipmapTarget.Texture2D );

            var createInfo = new MgOffscreenDeviceCreateInfo
            {
                Width = TEXTURE_SIZE,
                Height = TEXTURE_SIZE,
                MinDepth = 0f,
                MaxDepth = 1f,
                ColorAttachments = new []
                {
                    new MgOffscreenColorAttachmentInfo
                    {
                        Format = MgFormat.R8G8B8A8_UNORM,
                        LoadOp = MgAttachmentLoadOp.CLEAR,
                        StoreOp = MgAttachmentStoreOp.STORE,
                        View = internalView,
                    }
                },
                DepthStencilAttachment = new MgOffscreenDepthStencilAttachmentInfo
                {
                    Format = MgFormat.D16_UNORM,
                    LoadOp = MgAttachmentLoadOp.CLEAR,
                    StoreOp = MgAttachmentStoreOp.STORE,
                    StencilLoadOp = MgAttachmentLoadOp.CLEAR,
                    StencilStoreOp = MgAttachmentStoreOp.STORE,
                    View = depthView,
                    Layout = MgImageLayout.GENERAL,
                }
            };
            // Create a FBO and attach the textures
            mOffscreen = factory.CreateOffscreenDevice(createInfo);
            var fb = mOffscreen.Framebuffers[0] as Magnesium.OpenGL.GLNextFramebuffer;
            FBOHandle = (uint) fb.Subpasses[0].Framebuffer;

           // GL.GenFramebuffers(1, out FBOHandle);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBOHandle);
            //GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, ColorTexture, 0);
            //GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, DepthTexture, 0);

            //GL.Ext.GenFramebuffers(1, out FBOHandle);
            //GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, FBOHandle);
            // GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.Texture2D, ColorTexture, 0);
            // GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, TextureTarget.Texture2D, DepthTexture, 0);

            #region Test for Error

            switch (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer))
            //switch (GL.Ext.CheckFramebufferStatus(FramebufferTarget.FramebufferExt))
            {
                case FramebufferErrorCode.FramebufferCompleteExt:
                    {
                        Console.WriteLine("FBO: The framebuffer is complete and valid for rendering.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteAttachmentExt:
                    {
                        Console.WriteLine("FBO: One or more attachment points are not framebuffer attachment complete. This could mean there’s no texture attached or the format isn’t renderable. For color textures this means the base format must be RGB or RGBA and for depth textures it must be a DEPTH_COMPONENT format. Other causes of this error are that the width or height is zero or the z-offset is out of range in case of render to volume.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteMissingAttachmentExt:
                    {
                        Console.WriteLine("FBO: There are no attachments.");
                        break;
                    }
                /* case  FramebufferErrorCode.GL_FRAMEBUFFER_INCOMPLETE_DUPLICATE_ATTACHMENT_EXT: 
                     {
                         Console.WriteLine("FBO: An object has been attached to more than one attachment point.");
                         break;
                     }*/
                case FramebufferErrorCode.FramebufferIncompleteDimensionsExt:
                    {
                        Console.WriteLine("FBO: Attachments are of different size. All attachments must have the same width and height.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteFormatsExt:
                    {
                        Console.WriteLine("FBO: The color attachments have different format. All color attachments must have the same format.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteDrawBufferExt:
                    {
                        Console.WriteLine("FBO: An attachment point referenced by GL.DrawBuffers() doesn’t have an attachment.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteReadBufferExt:
                    {
                        Console.WriteLine("FBO: The attachment point referenced by GL.ReadBuffers() doesn’t have an attachment.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferUnsupportedExt:
                    {
                        Console.WriteLine("FBO: This particular FBO configuration is not supported by the implementation.");
                        break;
                    }
                default:
                    {
                        Console.WriteLine("FBO: Status unknown. (yes, this is really bad.)");
                        break;
                    }
            }

            // using FBO might have changed states, e.g. the FBO might not support stereoscopic views or double buffering
            int[] queryinfo = new int[6];
            GL.GetInteger(GetPName.MaxColorAttachmentsExt, out queryinfo[0]);
            GL.GetInteger(GetPName.AuxBuffers, out queryinfo[1]);
            GL.GetInteger(GetPName.MaxDrawBuffers, out queryinfo[2]);
            GL.GetInteger(GetPName.Stereo, out queryinfo[3]);
            GL.GetInteger(GetPName.Samples, out queryinfo[4]);
            GL.GetInteger(GetPName.Doublebuffer, out queryinfo[5]);
            Console.WriteLine("max. ColorBuffers: " + queryinfo[0] + " max. AuxBuffers: " + queryinfo[1] + " max. DrawBuffers: " + queryinfo[2] +
                               "\nStereo: " + queryinfo[3] + " Samples: " + queryinfo[4] + " DoubleBuffer: " + queryinfo[5]);

            Console.WriteLine("Last GL Error: " + GL.GetError());

            #endregion Test for Error

            GL.PushAttrib(AttribMask.ViewportBit);
            {
                var vp = mOffscreen.Viewport;
                // GL.Viewport(vp.Offset.X, vp.Offset.Y, (int) vp.Extent.Width, (int) vp.Extent.Height);
                GL.ViewportIndexed(0, vp.X, vp.Y, vp.Width, vp.Height);
                //GL.Viewport(0, 0, TEXTURE_SIZE, TEXTURE_SIZE);
                //  GL.UseProgram(mShaderProgramHandle);
                // clear the screen in red, to make it very obvious what the clear affected. only the FBO, not the real framebuffer
                //GL.ClearColor(1f, 0f, 1f, 0f);
                GL.ClearBuffer(ClearBuffer.Color, 0, new float[] { 0.7f, 0f, 0f, 0f });
                GL.ClearBuffer(ClearBufferCombined.DepthStencil, 0, 1f, 0);
                // GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

               // DrawImmediateMode();

                DrawVAO();

            }
            GL.PopAttrib();
            //GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0); // disable rendering into the FBO
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            GL.ClearColor(.1f, .2f, .3f, 0f);
            GL.Color3(1f, 1f, 1f);

            GL.Enable(EnableCap.Texture2D); // enable Texture Mapping
            GL.BindTexture(TextureTarget.Texture2D, 0); // bind default texture
        }

        private void DrawVAO()
        {
            GL.UseProgram(mOffscreenShaderProgram);

            GL.BindVertexArray(mVAOHandle);
          //  GL.DrawArrays(PrimitiveType.Triangles, 0, mNoOfIndices);
            GL.DrawElementsInstancedBaseVertex(PrimitiveType.TriangleStrip, mNoOfIndices, DrawElementsType.UnsignedInt, IntPtr.Zero, 1, 0);
            GL.UseProgram(0);
            GL.BindVertexArray(0);
        }

        private void DrawImmediateMode()
        {
            OpenTK.Matrix4 perspective = OpenTK.Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, TEXTURE_SIZE / (float)TEXTURE_SIZE, 2.5f, 6f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);

            Matrix4 lookat = Matrix4.LookAt(0f, 0f, 4.5f, 0f, 0f, 0f, 0f, 1f, 0f);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);

            // draw some complex object into the FBO's textures
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Color3(0f, 1f, 0f);
            Object.Draw();

            GL.Disable(EnableCap.ColorMaterial);
            GL.Disable(EnableCap.Light0);
            GL.Disable(EnableCap.Lighting);
        }

        protected override void OnUnload(EventArgs e)
        {
            Object.Dispose();

            mOffscreen.Dispose();

            // Clean up what we allocated before exiting
            mColorReplacement.Dispose();

            //if (ColorTexture != 0)
            //    GL.DeleteTextures(1, ref ColorTexture);

            mDepthReplacement.Dispose();
            //if (DepthTexture != 0)
            //    GL.DeleteTextures(1, ref DepthTexture);

            if (FBOHandle != 0)
            {
               // GL.Ext.DeleteFramebuffers(1, ref FBOHandle);
                GL.DeleteFramebuffers(1, ref FBOHandle);
            }

            if (mOffscreenShaderProgram != 0)
            {
                GL.DeleteProgram(mOffscreenShaderProgram);
            }

            if (mVAOHandle != 0)
            {
                GL.DeleteVertexArrays(1, new[] { mVAOHandle });
            }

            if (mPositionVertexBuffer != 0)
            {
                GL.DeleteBuffers(1, new[] { mPositionVertexBuffer });
            }
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);

            double aspect_ratio = Width / (double)Height;

            OpenTK.Matrix4 perspective = OpenTK.Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);

            Matrix4 lookat = Matrix4.LookAt(0, 0, 3, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);

            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var keyboard = OpenTK.Input.Keyboard.GetState();
            if (keyboard[Key.Escape])
                this.Exit();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.UseProgram(0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.PushMatrix();
            {
                // Draw the Color Texture
                GL.Translate(-1.1f, 0f, 0f);
                GL.BindTexture(TextureTarget.Texture2D, ColorTexture);
                GL.Begin(PrimitiveType.Quads);
                {
                    GL.TexCoord2(0f, 1f);
                    GL.Vertex2(-1.0f, 1.0f);
                    GL.TexCoord2(0.0f, 0.0f);
                    GL.Vertex2(-1.0f, -1.0f);
                    GL.TexCoord2(1.0f, 0.0f);
                    GL.Vertex2(1.0f, -1.0f);
                    GL.TexCoord2(1.0f, 1.0f);
                    GL.Vertex2(1.0f, 1.0f);
                }
                GL.End();

                // Draw the Depth Texture
                GL.Translate(+2.2f, 0f, 0f);
                GL.BindTexture(TextureTarget.Texture2D, DepthTexture);
                GL.Begin(PrimitiveType.Quads);
                {
                    GL.TexCoord2(0f, 1f);
                    GL.Vertex2(-1.0f, 1.0f);
                    GL.TexCoord2(0.0f, 0.0f);
                    GL.Vertex2(-1.0f, -1.0f);
                    GL.TexCoord2(1.0f, 0.0f);
                    GL.Vertex2(1.0f, -1.0f);
                    GL.TexCoord2(1.0f, 1.0f);
                    GL.Vertex2(1.0f, 1.0f);
                }
                GL.End();
            }
            GL.PopMatrix();

            this.SwapBuffers();
        }

        #region public static void Main()

        /// <summary>
        /// Entry point of this example.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (SimpleFBO example = new SimpleFBO())
            {               
                example.Run(30.0, 0.0);
            }
        }

        #endregion
    }
}