// 

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
            : base(720, 480)
        {
        }

        const int TEXTURE_SIZE = 256;

        Examples.Shapes.DrawableShape Object;
        private IMgOffscreenDeviceAttachment mColorAttachment;
        private IMgOffscreenDeviceAttachment mDepthAttachment;
        private IMgOffscreenDeviceAttachment mNormalAttachment;
        private Magnesium.OpenGL.IGLCmdShaderProgramEntrypoint mShaderCache;
        private IMgEffectFramework mOffscreen;

        #region CreateOffscreenShader methods

        int mOffscreenShaderProgram;
       // int mProjectionMatrixLocation;
       // int mModelviewMatrixLocation;

        static int CreateOffscreenShader()
        {
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

layout(location = 0) out vec4 out_frag_color_0;
layout(location = 1) out vec4 out_frag_color_1;

void main(void)
{
    vec3 unitNormal = normalize(normal);
    float diffuse = clamp(dot(lightVecNormalized, unitNormal), 0.0, 1.0);
    out_frag_color_0 = vec4(ambient + diffuse * lightColor, 1.0);
    out_frag_color_1 = vec4(unitNormal, 1.0);
}";
            var vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            var fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(vertexShaderHandle, vertexShaderSource);
            GL.ShaderSource(fragmentShaderHandle, fragmentShaderSource);

            GL.CompileShader(vertexShaderHandle);
            GL.CompileShader(fragmentShaderHandle);

            Console.WriteLine(GL.GetShaderInfoLog(vertexShaderHandle));
            Console.WriteLine(GL.GetShaderInfoLog(fragmentShaderHandle));

            // Create program
            var programId = GL.CreateProgram();

            GL.AttachShader(programId, vertexShaderHandle);
            GL.AttachShader(programId, fragmentShaderHandle);

             GL.BindAttribLocation(programId, 0, "in_position");
            GL.BindAttribLocation(programId, 1, "in_normal");
            GL.BindAttribLocation(programId, 2, "in_uv");

            GL.LinkProgram(programId);
            Console.WriteLine(GL.GetProgramInfoLog(programId));

            GL.DeleteShader(vertexShaderHandle);
            GL.DeleteShader(fragmentShaderHandle);

            //GL.UseProgram(mShaderProgramHandle);

            GL.UseProgram(programId);
            // Set uniforms
            var projectionMatrixLocation = GL.GetUniformLocation(programId, "projection_matrix");
            var modelviewMatrixLocation = GL.GetUniformLocation(programId, "modelview_matrix");

            const float Aspect = TEXTURE_SIZE / (float)TEXTURE_SIZE;
            OpenTK.Matrix4 projectionMatrix = OpenTK.Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Aspect, 2.5f, 6f);
            Matrix4 modelviewMatrix = Matrix4.LookAt(0f, 0f, 4.5f, 0f, 0f, 0f, 0f, 1f, 0f);
           // var projectionMatrix = Matrix4.Identity;
           // var modelviewMatrix = Matrix4.Translation(0, 0, -2.5f);

            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);
            GL.UniformMatrix4(modelviewMatrixLocation, false, ref modelviewMatrix);

            GL.UseProgram(0);
            return programId;
        }

        #endregion

        #region GenerateVAO methods 

        private uint mVAOHandle;
        private uint mPositionVertexBuffer;
        private PrimitiveType mPrimitiveType;
        private int mNoOfIndices;
        private void GenerateVAO(DrawableShape item)
        {
            item.GetArraysforVBO(out PrimitiveType priType, out VertexT2fN3fV3f[] vertices, out uint[] indices);
            mPrimitiveType = priType;

            mNoOfIndices = indices.Length;

            mPositionVertexBuffer = GenerateVertexBuffer(vertices, indices);

            var dataStride = Marshal.SizeOf(typeof(VertexT2fN3fV3f));

            const int BINDING_INDEX = 0;

            const int POSITION_ATTRIB = 0;
            const int NORMAL_ATTRIB = 1;
            const int UV_ATTRIB = 2;

            var vertexInput = new MgPipelineVertexInputStateCreateInfo
            {
                VertexBindingDescriptions = new[]
    {
                    new MgVertexInputBindingDescription
                    {
                        Binding = BINDING_INDEX,
                        InputRate = MgVertexInputRate.VERTEX,
                        Stride = (uint) dataStride,
                    }
                },
                VertexAttributeDescriptions = new[]
    {
                    new MgVertexInputAttributeDescription
                    {
                        Binding = BINDING_INDEX,
                        Location = POSITION_ATTRIB,
                        Format= MgFormat.R32G32B32_SFLOAT,
                        Offset =  (uint) Marshal.OffsetOf(typeof(VertexT2fN3fV3f), "Position"),
                    },
                    new MgVertexInputAttributeDescription
                    {
                        Binding = BINDING_INDEX,
                        Location = NORMAL_ATTRIB,
                        Format= MgFormat.R32G32B32_SFLOAT,
                        Offset = (uint) Marshal.OffsetOf(typeof(VertexT2fN3fV3f), "Normal"),
                    },
                    new MgVertexInputAttributeDescription
                    {
                        Binding = BINDING_INDEX,
                        Location = UV_ATTRIB,
                        Format= MgFormat.R32G32_SFLOAT,
                        Offset = (uint) Marshal.OffsetOf(typeof(VertexT2fN3fV3f), "TexCoord"),
                    },
                }
            };

            var indexSize = (sizeof(uint) * indices.Length);
            var positionOffset = new IntPtr(indexSize);

            mVAOHandle = GenerateVertexArray(mPositionVertexBuffer, positionOffset, vertexInput);
            GL.VertexArrayElementBuffer(mVAOHandle, mPositionVertexBuffer);
            GL.BindVertexArray(0);
        }

        private static uint GenerateVertexArray(uint buffer, IntPtr vertexOffset, MgPipelineVertexInputStateCreateInfo vertexInput)
        {
            uint[] result = new uint[1];
            GL.CreateVertexArrays(1, result);

            Debug.Assert(GL.IsVertexArray(result[0]));

            var vaoHandle = result[0];

            //// ALWAYS OFFSET OF ZERO
            //if (hasIndices)
            //{
            //    GL.VertexArrayElementBuffer(vaoHandle, buffer);
            //}

            foreach (var desc in vertexInput.VertexBindingDescriptions)
            {
                GL.VertexArrayVertexBuffer(vaoHandle, desc.Binding, buffer, vertexOffset, (int) desc.Stride);
            }

            foreach(var desc in vertexInput.VertexAttributeDescriptions)
            {
                GL.EnableVertexArrayAttrib(vaoHandle, desc.Location);
                GL.VertexArrayAttribBinding(vaoHandle, desc.Location, desc.Binding);
                ExtractFormatValues(desc.Format, out int noOfComponents, out VertexAttribType attribType, out bool isNormalized);
                GL.VertexArrayAttribFormat(vaoHandle, desc.Location, noOfComponents, attribType, isNormalized, desc.Offset);
            }


            //GL.EnableVertexArrayAttrib(vaoHandle, POSITION_ATTRIB);
            //GL.EnableVertexArrayAttrib(vaoHandle, NORMAL_ATTRIB);
            //GL.EnableVertexArrayAttrib(vaoHandle, UV_ATTRIB);

            //GL.VertexArrayAttribBinding(vaoHandle, POSITION_ATTRIB, BINDING_INDEX);

            //GL.VertexArrayAttribFormat(vaoHandle, POSITION_ATTRIB, 3, VertexAttribType.Float, false, positionAttribOffset);

            //GL.VertexArrayAttribBinding(vaoHandle, NORMAL_ATTRIB, BINDING_INDEX);
     
            //GL.VertexArrayAttribFormat(vaoHandle, NORMAL_ATTRIB, 3, VertexAttribType.Float, false, normalAttribOffset);

            //GL.VertexArrayAttribBinding(vaoHandle, UV_ATTRIB, BINDING_INDEX);

            //GL.VertexArrayAttribFormat(vaoHandle, UV_ATTRIB, 2, VertexAttribType.Float, false, uvAttribOffset);

            return vaoHandle;
        }

        private static bool ExtractFormatValues(MgFormat format, out int noOfComponents, out VertexAttribType attribType, out bool isNormalized)
        {
            switch(format)
            {
                case MgFormat.R16G16_UNORM:
                    noOfComponents = 2;
                    attribType = VertexAttribType.HalfFloat;
                    isNormalized = true;
                    return true;
                case MgFormat.R32G32_SFLOAT:
                    noOfComponents = 2;
                    attribType = VertexAttribType.Float;
                    isNormalized = false;
                    return true;
                case MgFormat.R32G32B32_SFLOAT:
                    noOfComponents = 3;
                    attribType = VertexAttribType.Float;
                    isNormalized = false;
                    return true;
                default:
                    throw new NotSupportedException();
            }
        }

        private static uint GenerateVertexBuffer<TData>(TData[] vertices, uint[] indices)
        {
            var buffers = new uint[1];
            // ARB_direct_state_access
            // Allows buffer objects to be initialised without binding them
            GL.CreateBuffers(1, buffers);

            var positionVboHandle = buffers[0];

            var stride = Marshal.SizeOf(typeof(TData));

            var indexCount = indices != null ? indices.Length : 0;
            var indexSize = (sizeof(uint) * indexCount);
            var positionOffset = indexSize;
            var positionSize = (stride * vertices.Length);
            var totalLength = indexSize + positionSize;

            AllocateDeviceMemory(positionVboHandle, totalLength);

            TransferVertexData(vertices, indices, positionVboHandle, indexSize, positionOffset, positionSize);

            //GL.DeleteBuffers(1, buffers);

            return positionVboHandle;
        }

        private static void TransferVertexData<TData>(TData[] vertices, uint[] indices, uint positionVboHandle, int indexSize, int positionOffset, int positionSize)
        {
            BufferAccessMask rangeFlags = BufferAccessMask.MapWriteBit | BufferAccessMask.MapPersistentBit | BufferAccessMask.MapCoherentBit;

            if (indices != null)
            {
                IntPtr indexDest = GL.MapNamedBufferRange(positionVboHandle, IntPtr.Zero, indexSize, rangeFlags);
                // Copy the struct to unmanaged memory.	
                CopyUint32s(indexDest, indices, 0, indexSize, 0, indices.Length);
                GL.Ext.UnmapNamedBuffer(positionVboHandle);
            }

            IntPtr vertexDest = GL.MapNamedBufferRange(positionVboHandle, new IntPtr(positionOffset), positionSize, rangeFlags);
            CopyValueData<TData>(vertexDest, vertices, 0, positionSize, 0, vertices.Length);
            GL.Ext.UnmapNamedBuffer(positionVboHandle);
        }

        private static void AllocateDeviceMemory(uint positionVboHandle, int totalLength)
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

            mOffscreenShaderProgram = CreateOffscreenShader();
            mToScreenShaderProgram = CreateToScreenShader();

            Object = new Shapes.TorusKnot(256, 32, 0.2, 7, 8, 1, true);

            GenerateVAO(Object);
            GenerateQuad();

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

            Magnesium.OpenGL.IGLDescriptorSetEntrypoint dSetEntrypoint = new Magnesium.OpenGL.GLFutureDescriptorSetEntrypoint();
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
                dSetEntrypoint,
                null,
                selector
               );
            var configuration = new MockGraphicsConfiguration(deviceEntrypoint);
            var entrypoint = new Magnesium.OpenGL.GLOffscreenDeviceEntrypoint();
            var factory = new MgOffscreenDeviceFactory(configuration, entrypoint);

            mColorAttachment = factory.CreateColorAttachment(MgFormat.R8G8B8A8_UNORM, TEXTURE_SIZE, TEXTURE_SIZE);

            mDepthAttachment = factory.CreateDepthStencilAttachment(MgFormat.D16_UNORM, TEXTURE_SIZE, TEXTURE_SIZE);

            mNormalAttachment = factory.CreateColorAttachment(MgFormat.R8G8B8A8_UNORM, TEXTURE_SIZE, TEXTURE_SIZE);

            mShaderCache = new Magnesium.OpenGL.DesktopGL.FullGLCmdShaderProgramEntrypoint(errHandler, selector);

            var createInfo = new MgOffscreenDeviceCreateInfo
            {
                Width = TEXTURE_SIZE,
                Height = TEXTURE_SIZE,
                MinDepth = 0f,
                MaxDepth = 1f,
                ColorAttachments = new[]
                {
                    new MgOffscreenColorAttachmentInfo
                    {
                        Format = MgFormat.R8G8B8A8_UNORM,
                        LoadOp = MgAttachmentLoadOp.CLEAR,
                        StoreOp = MgAttachmentStoreOp.STORE,
                        View = mColorAttachment.View,
                    },
                    new MgOffscreenColorAttachmentInfo
                    {
                        Format = MgFormat.R8G8B8A8_UNORM,
                        LoadOp = MgAttachmentLoadOp.CLEAR,
                        StoreOp = MgAttachmentStoreOp.STORE,
                        View = mNormalAttachment.View,
                    },
                },
                DepthStencilAttachment = new MgOffscreenDepthStencilAttachmentInfo
                {
                    Format = MgFormat.D16_UNORM,
                    LoadOp = MgAttachmentLoadOp.CLEAR,
                    StoreOp = MgAttachmentStoreOp.STORE,
                    StencilLoadOp = MgAttachmentLoadOp.CLEAR,
                    StencilStoreOp = MgAttachmentStoreOp.STORE,
                    View = mDepthAttachment.View,
                    Layout = MgImageLayout.GENERAL,
                }
            };
            // Create a FBO and attach the textures
            mOffscreen = factory.CreateOffscreenDevice(createInfo);
            var fb = mOffscreen.Framebuffers[0] as Magnesium.OpenGL.GLNextFramebuffer;
            var fboHandle = fb.Subpasses[0].Framebuffer;

            mShaderCache.BindFramebuffer(fboHandle);

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

                GL.ClearBuffer(ClearBuffer.Color, 1, new float[] { 0f, 0.7f, 0.7f, 0f });

                DrawVAO();

            }
            GL.PopAttrib();

            mShaderCache.BindFramebuffer(0);

            GL.ClearColor(.1f, .2f, .3f, 0f);
            GL.Color3(1f, 1f, 1f);

            GL.Enable(EnableCap.Texture2D); // enable Texture Mapping
            GL.BindTexture(TextureTarget.Texture2D, 0); // bind default texture

            Magnesium.OpenGL.IGLTextureGalleryEntrypoint texGallery = new Magnesium.OpenGL.DesktopGL.FullGLTextureGalleryEntrypoint();
            mTextureCache = new Magnesium.OpenGL.GLSolarShaderTextureDescriptorCache(texGallery);
            mTextureCache.Initialize();

            var poolCreateInfo = new MgDescriptorPoolCreateInfo
            {
                MaxSets = 3,
                PoolSizes = new[]
                {
                    new MgDescriptorPoolSize
                    {
                        DescriptorCount = 3,
                        Type = MgDescriptorType.COMBINED_IMAGE_SAMPLER,
                    }
                }
            };

            var descriptorPool = new Magnesium.OpenGL.GLSolarDescriptorPool(poolCreateInfo);

            var device = new Magnesium.OpenGL.Internals.GLDevice(null, deviceEntrypoint);

            var lCreateInfo = new MgDescriptorSetLayoutCreateInfo
            {
                Bindings = new []
                {
                    new MgDescriptorSetLayoutBinding
                    {
                        Binding = 0,
                        DescriptorCount = 1,
                        DescriptorType = MgDescriptorType.COMBINED_IMAGE_SAMPLER,                        
                    }
                }
            };
            device.CreateDescriptorSetLayout(lCreateInfo, null, out IMgDescriptorSetLayout pSetLayout);

            var pAllocateInfo = new MgDescriptorSetAllocateInfo
            {
                DescriptorPool = descriptorPool,
                DescriptorSetCount = 3,
                SetLayouts = new[] { pSetLayout, pSetLayout, pSetLayout },
            };
            var err = device.AllocateDescriptorSets(pAllocateInfo, out IMgDescriptorSet[] dSets);
            Debug.Assert(err == Result.SUCCESS);
            mTextureSets = dSets;

            MgWriteDescriptorSet[] pDescriptorWrites = new MgWriteDescriptorSet[]
            {
                new MgWriteDescriptorSet
                {
                    DstSet = mTextureSets[0],
                    DescriptorCount = 1,
                    DstBinding = 0,
                    DescriptorType = MgDescriptorType.COMBINED_IMAGE_SAMPLER,                    
                    ImageInfo = new []
                    {
                        new MgDescriptorImageInfo
                        {
                            ImageLayout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
                            ImageView = mColorAttachment.View,
                        }
                    }
                },
                new MgWriteDescriptorSet
                {
                    DstSet = mTextureSets[1],
                    DescriptorCount = 1,
                    DstBinding = 0,
                    DescriptorType = MgDescriptorType.COMBINED_IMAGE_SAMPLER,
                    ImageInfo = new []
                    {
                        new MgDescriptorImageInfo
                        {
                            ImageLayout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
                            ImageView = mDepthAttachment.View,
                        }
                    }
                },
                new MgWriteDescriptorSet
                {
                    DstSet = mTextureSets[2],
                    DescriptorCount = 1,
                    DstBinding = 0,
                    DescriptorType = MgDescriptorType.COMBINED_IMAGE_SAMPLER,
                    ImageInfo = new []
                    {
                        new MgDescriptorImageInfo
                        {
                            ImageLayout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
                            ImageView = mNormalAttachment.View,
                        }
                    }
                },
            };
            device.UpdateDescriptorSets(pDescriptorWrites, null);
        }
        Magnesium.OpenGL.IGLShaderTextureDescriptorCache mTextureCache;
        IMgDescriptorSet[] mTextureSets;

        private void DrawVAO()
        {
            mShaderCache.BindProgram(mOffscreenShaderProgram);
            mShaderCache.BindVAO(mVAOHandle);

            GL.DrawElementsInstancedBaseVertex(PrimitiveType.TriangleStrip, mNoOfIndices, DrawElementsType.UnsignedInt, IntPtr.Zero, 1, 0);

            mShaderCache.BindVAO(0);
            mShaderCache.BindProgram(0);
        }

        protected override void OnUnload(EventArgs e)
        {
            Object.Dispose();

            mOffscreen.Dispose();

            // Clean up what we allocated before exiting
            mColorAttachment.Dispose();

            mDepthAttachment.Dispose();

            mNormalAttachment.Dispose();

            if (mOffscreenShaderProgram != 0)
            {
                GL.DeleteProgram(mOffscreenShaderProgram);
            }

            if (mToScreenShaderProgram != 0)
            {
                GL.DeleteProgram(mToScreenShaderProgram);
            }

            if (mVAOHandle != 0)
            {
                GL.DeleteVertexArrays(1, new[] { mVAOHandle });
            }

            if (mPositionVertexBuffer != 0)
            {
                GL.DeleteBuffers(1, new[] { mPositionVertexBuffer });
            }

            if (mQuadBuffer != 0)
            {
                GL.DeleteBuffers(1, new[] { mQuadBuffer });
            }
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);

            double aspect_ratio = Width / (double)Height;

            //OpenTK.Matrix4 perspective = OpenTK.Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)aspect_ratio, 1, 64);
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadMatrix(ref perspective);

            //Matrix4 lookat = Matrix4.LookAt(0, 0, 3, 0, 0, 0, 0, 1, 0);
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadMatrix(ref lookat);

            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var keyboard = OpenTK.Input.Keyboard.GetState();
            if (keyboard[Key.Escape])
                this.Exit();
        }

        #region CreateToScreenShader methods

        int mToScreenShaderProgram;

        static int CreateToScreenShader()
        {
            const string VERT_SOURCE = @"
#version 450

precision highp float;

uniform mat4 projection_matrix;
uniform mat4 modelview_matrix;
uniform vec3 offset;

layout(location = 0) in vec2 in_position;
layout(location = 1) in vec2 in_uv;

out vec2 texCoords;

void main(void)
{
  texCoords = in_uv;
  
  vec4 adjusted = vec4(in_position, 0, 1) + vec4(offset, 1);

  gl_Position = projection_matrix * modelview_matrix * adjusted;
}";

            const string FRAG_SOURCE = @"
#version 450

#extension GL_ARB_shading_language_420pack : require

precision highp float;

layout (binding = 0) uniform sampler2D diffuseTex;

in vec2 texCoords;

out vec4 out_frag_color;

void main(void)
{
  vec4 diffuse = texture2D(diffuseTex, texCoords);
  out_frag_color = diffuse;
}";

            var vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            var fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(vertexShaderHandle, VERT_SOURCE);
            GL.ShaderSource(fragmentShaderHandle, FRAG_SOURCE);

            GL.CompileShader(vertexShaderHandle);
            GL.CompileShader(fragmentShaderHandle);

            Console.WriteLine(GL.GetShaderInfoLog(vertexShaderHandle));
            Console.WriteLine(GL.GetShaderInfoLog(fragmentShaderHandle));

            // Create program
            var programId = GL.CreateProgram();

            GL.AttachShader(programId, vertexShaderHandle);
            GL.AttachShader(programId, fragmentShaderHandle);

            GL.BindAttribLocation(programId, 0, "in_position");
            GL.BindAttribLocation(programId, 1, "in_uv");

            GL.LinkProgram(programId);
            Console.WriteLine(GL.GetProgramInfoLog(programId));

            GL.DeleteShader(vertexShaderHandle);
            GL.DeleteShader(fragmentShaderHandle);

            //GL.UseProgram(mShaderProgramHandle);

            GL.UseProgram(programId);
            // Set uniforms
            var projectionMatrixLocation = GL.GetUniformLocation(programId, "projection_matrix");
            var modelviewMatrixLocation = GL.GetUniformLocation(programId, "modelview_matrix");

            OpenTK.Matrix4 projectionMatrix = OpenTK.Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, TEXTURE_SIZE / (float)TEXTURE_SIZE, 2.5f, 6f);
            Matrix4 modelviewMatrix = Matrix4.LookAt(0f, 0f, 4.5f, 0f, 0f, 0f, 0f, 1f, 0f);
            // var projectionMatrix = Matrix4.Identity;
            // var modelviewMatrix = Matrix4.Translation(0, 0, -2.5f);

            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);
            GL.UniformMatrix4(modelviewMatrixLocation, false, ref modelviewMatrix);

            GL.UseProgram(0);
            return programId;
        }

        #endregion

        #region GenerateQuad methods 

        [StructLayout(LayoutKind.Sequential)]
        public struct QuadVertexData
        {
            public Vector2h TexCoord;
            public Vector2 Position;
        }

        private uint mQuadBuffer;
        private uint mQuadVAO;
        public void GenerateQuad()
        {
            //GL.TexCoord2(0f, 1f);
            //GL.Vertex2(-1.0f, 1.0f);

            //GL.TexCoord2(0.0f, 0.0f);
            //GL.Vertex2(-1.0f, -1.0f);

            //GL.TexCoord2(1.0f, 0.0f);
            //GL.Vertex2(1.0f, -1.0f);

            //GL.TexCoord2(1.0f, 1.0f);
            //GL.Vertex2(1.0f, 1.0f);

            var vertexData = new []
            {
                new QuadVertexData
                {
                    TexCoord = new Vector2h(0f,1f),
                    Position = new Vector2(-1f,1f),
                },
                new QuadVertexData
                {
                    TexCoord = new Vector2h(0f,0f),
                    Position = new Vector2(-1f,-1f),
                },
                new QuadVertexData
                {
                    TexCoord = new Vector2h(1f,0f),
                    Position = new Vector2(1f,-1f),
                },
                new QuadVertexData
                {
                    TexCoord = new Vector2h(1f,1f),
                    Position = new Vector2(1f, 1f),
                },
            };

            var indices = new uint[] { 0, 1 , 2, 0, 2, 3 };

            mQuadBuffer = GenerateVertexBuffer< QuadVertexData>(vertexData, indices);
            var dataStride = Marshal.SizeOf(typeof(QuadVertexData));

            var indexSize = (sizeof(uint) * indices.Length);
            var positionSize = (dataStride * vertexData.Length);
            TransferVertexData<QuadVertexData>(vertexData, indices, mQuadBuffer, indexSize, indexSize, positionSize);

            const int BINDING_INDEX = 0;

            const int POSITION_ATTRIB = 0;
            const int UV_ATTRIB = 1;


            var vertexInput = new MgPipelineVertexInputStateCreateInfo
            {
                VertexBindingDescriptions = new[]
                {
                    new MgVertexInputBindingDescription
                    {
                        Binding = BINDING_INDEX,
                        InputRate = MgVertexInputRate.VERTEX,
                        Stride = (uint) dataStride,
                    }
                },
                VertexAttributeDescriptions = new[]
                {
                    new MgVertexInputAttributeDescription
                    {
                        Binding = BINDING_INDEX,
                        Location = POSITION_ATTRIB,
                        Format= MgFormat.R32G32_SFLOAT,
                        Offset = (uint) Marshal.OffsetOf(typeof(QuadVertexData), "Position"),
                    },
                    new MgVertexInputAttributeDescription
                    {
                        Binding = BINDING_INDEX,
                        Location = UV_ATTRIB,
                        Format= MgFormat.R16G16_UNORM,
                        Offset = (uint) Marshal.OffsetOf(typeof(QuadVertexData), "TexCoord"),
                    },
                }
            };


            var positionOffset = new IntPtr(indexSize);

            mQuadVAO = GenerateVertexArray(mQuadBuffer, positionOffset, vertexInput);
            GL.VertexArrayElementBuffer(mQuadVAO, mQuadBuffer);
            GL.BindVertexArray(0);
        }

        #endregion

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            mShaderCache.BindProgram(0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var colorMapDs = mTextureSets[0] as Magnesium.OpenGL.IGLFutureDescriptorSet;

            mTextureCache.Bind(
                colorMapDs,
                colorMapDs.Resources[0]);

            mShaderCache.BindVAO(mQuadVAO);
            mShaderCache.BindProgram(mToScreenShaderProgram);

            var projectionMatrixLocation = GL.GetUniformLocation(mToScreenShaderProgram, "projection_matrix");
            var modelviewMatrixLocation = GL.GetUniformLocation(mToScreenShaderProgram, "modelview_matrix");
            var offsetLocation = GL.GetUniformLocation(mToScreenShaderProgram, "offset");

            var projectionMatrix = OpenTK.Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float) this.Width / (float)this.Height, 0.1f, 1000f); ;

            var modelviewMatrix = Matrix4.LookAt(0f, 0f, 2.8f, 0f, 0f, 0f, 0f, 1f, 0f);
                
            GL.Uniform3(offsetLocation, -2.2f, 0f, 0f);
            GL.UniformMatrix4(modelviewMatrixLocation, false, ref modelviewMatrix);
            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);

            GL.DrawElementsInstancedBaseInstance(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero, 1, 0);

            var depthMapDs = mTextureSets[1] as Magnesium.OpenGL.IGLFutureDescriptorSet;

            mTextureCache.Bind(
                depthMapDs,
                depthMapDs.Resources[0]);

            GL.Uniform3(offsetLocation, 2.2f, 0f, 0f);
            GL.DrawElementsInstancedBaseInstance(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero, 1, 0);

            var normalDs = mTextureSets[2] as Magnesium.OpenGL.IGLFutureDescriptorSet;

            mTextureCache.Bind(
                normalDs,
                normalDs.Resources[0]);

            GL.Uniform3(offsetLocation, 0f, 0f, 0f);
            GL.DrawElementsInstancedBaseInstance(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero, 1, 0);

            mShaderCache.BindProgram(0);

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