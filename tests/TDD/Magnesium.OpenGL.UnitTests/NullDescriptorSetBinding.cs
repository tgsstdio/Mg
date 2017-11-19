using Magnesium.OpenGL.Internals;
using NUnit.Framework;
using System.Collections.Generic;

namespace Magnesium.OpenGL.UnitTests
{
    [TestFixture]
    class NullDescriptorSetBinding
    {
        [Test]
        public void NullTest_01()
        {
            var shaderEntrypoint = new MockGLCmdShaderProgramEntrypoint();
            var gallery = new MockGLTextureGallery();
            var shaderCache = new GLNextCmdShaderProgramCache(shaderEntrypoint, gallery);

            Assert.DoesNotThrow(() => { shaderCache.BindDescriptorSets(null); });
        }

        [Test]
        public void Initialize_01()
        {
            var shaderEntrypoint = new MockGLCmdShaderProgramEntrypoint();
            var gallery = new MockGLTextureGallery();
            var shaderCache = new GLNextCmdShaderProgramCache(shaderEntrypoint, gallery);

            shaderCache.Initialize();
            Assert.DoesNotThrow(() => { shaderCache.BindDescriptorSets(null); });
        }

        [Test]
        public void NullTest_02()
        {
            var shaderEntrypoint = new MockGLCmdShaderProgramEntrypoint();
            var gallery = new MockGLTextureGallery();
            var shaderCache = new GLNextCmdShaderProgramCache(shaderEntrypoint, gallery);

            Assert.DoesNotThrow(() => {
                var param = new Internals.GLCmdDescriptorSetParameter
                {
                    Bindpoint = MgPipelineBindPoint.GRAPHICS,
                    Layout = null,
                    DescriptorSet = null,
                    DynamicOffsets = null,
                };
                shaderCache.BindDescriptorSets(param);
            });
            Assert.AreEqual(0, shaderEntrypoint.Count);
            Assert.IsNotNull(shaderEntrypoint.UniformBuffers);
            Assert.IsNotNull(shaderEntrypoint.UniformOffsets);
            Assert.IsNotNull(shaderEntrypoint.UniformSizes);
        }

        [Test]
        public void EmptyDescriptorSet_01()
        {
            var shaderEntrypoint = new MockGLCmdShaderProgramEntrypoint();
            var gallery = new MockGLTextureGallery();
            var shaderCache = new GLNextCmdShaderProgramCache(shaderEntrypoint, gallery);

            var pipelineLayout = new MockGLPipelineLayout();
            var ds = new MockGLDescriptorSet
            {
                IsValidDescriptorSet = false,
                Parent = null,
                Resources = null,
            };
            var dynamicOffsets = new uint[4];

            Assert.DoesNotThrow(() => {
                var param = new Internals.GLCmdDescriptorSetParameter
                {
                    Bindpoint = MgPipelineBindPoint.GRAPHICS,
                    Layout = pipelineLayout,
                    DescriptorSet = ds,
                    DynamicOffsets = dynamicOffsets,
                };
                shaderCache.BindDescriptorSets(param);
            });
            Assert.AreEqual(0, shaderEntrypoint.Count);
            Assert.IsNotNull(shaderEntrypoint.UniformBuffers);
            Assert.IsNotNull(shaderEntrypoint.UniformOffsets);
            Assert.IsNotNull(shaderEntrypoint.UniformSizes);
        }

        [Test]
        public void Pass_01()
        {
            var shaderEntrypoint = new MockGLCmdShaderProgramEntrypoint();
            var gallery = new MockGLTextureGallery();
            var shaderCache = new GLNextCmdShaderProgramCache(shaderEntrypoint, gallery);

            var pipelineLayout = new MockGLPipelineLayout
            {
                Ranges = new Dictionary<int, GLBindingPointOffsetInfo>(),
                NoOfBindingPoints = 4,
                NoOfExpectedDynamicOffsets = 0,
                NoOfStorageBuffers = 1,
                OffsetDestinations = new GLDynamicOffsetInfo[] { }
                
            };
            var parentPool = new MockGLDescriptorPool
            {
                UniformBuffers = new MockGLDescriptorPoolResource<GLBufferDescriptor>
                {
                    Items = new GLBufferDescriptor[]
                    {
                       // NEED FOUR 
                       new GLBufferDescriptor
                       {
                           
                       },
                       new GLBufferDescriptor
                       {

                       },
                       new GLBufferDescriptor
                       {

                       },
                       new GLBufferDescriptor
                       {

                       },
                    }
                },
                StorageBuffers = new MockGLDescriptorPoolResource<GLBufferDescriptor>
                {
                    Items = new GLBufferDescriptor[]
                    {
                       // NEED FOUR 
                       new GLBufferDescriptor
                       {

                       },
                    }
                }
            };

            var ds = new MockGLDescriptorSet
            {
                Parent = parentPool,
                IsValidDescriptorSet = true,
                Resources = new GLDescriptorPoolResourceInfo[]
                {
                    new GLDescriptorPoolResourceInfo
                    {
                        Binding = 0,
                        DescriptorCount = 4,
                        GroupType = GLDescriptorBindingGroup.UniformBuffer,
                        Ticket = new GLPoolResourceTicket
                        {
                            First = 0,
                            Last = 3,
                            Count = 4,
                        }
                    },
                    new GLDescriptorPoolResourceInfo
                    {
                        Binding = 1,
                        DescriptorCount = 1,
                        GroupType = GLDescriptorBindingGroup.StorageBuffer,
                        Ticket = new GLPoolResourceTicket
                        {
                            First = 0,
                            Last = 0,
                            Count = 1,
                        }
                    },
                }
            };
            var dynamicOffsets = new uint[4];

            const int FIRST_BINDING = 0;

            var blockEntries = new GLUniformBlockEntry[]
            {
                new GLUniformBlockEntry
                {
                    BlockName = "UBO",
                    ActiveIndex = 0,
                    FirstBinding = 0,
                    Stride = 2,
                    Token = new GLUniformBlockInfo
                    {
                        BindingIndex = 0,
                        Prefix = "UBO",
                        X = 0,
                        Y = 0,
                        Z = 0,
                    },
                },
                new GLUniformBlockEntry
                {
                    BlockName = "UBO",
                    ActiveIndex = 1,
                    FirstBinding = 0,
                    Stride = 2,
                    Token = new GLUniformBlockInfo
                    {
                        BindingIndex = 1,
                        Prefix = "UBO",
                        X = 1,
                        Y = 0,
                        Z = 0,
                    },
                },
                new GLUniformBlockEntry
                {
                    BlockName = "UBO",
                    ActiveIndex = 2,
                    FirstBinding = 0,
                    Stride = 2,
                    Token = new GLUniformBlockInfo
                    {
                        BindingIndex = 2,
                        Prefix = "UBO",
                        X = 2,
                        Y = 0,
                        Z = 0,
                    },
                },
                new GLUniformBlockEntry
                {
                    BlockName = "UBO",
                    ActiveIndex = 3,
                    FirstBinding = 0,
                    Stride = 2,
                    Token = new GLUniformBlockInfo
                    {
                        BindingIndex = 3,
                        Prefix = "UBO",
                        X = 3,
                        Y = 0,
                        Z = 0,
                    },
                },
            };

            pipelineLayout.Ranges[FIRST_BINDING] = new GLBindingPointOffsetInfo
            {
                Binding = FIRST_BINDING,
                First = 0,
                Last = 3,
            };

            var arrayMapper = new Internals.GLInternalCacheArrayMapper(pipelineLayout, blockEntries);
            var internalCache = new Internals.GLInternalCache(pipelineLayout, blockEntries, arrayMapper);

            const int PROGRAM_ID = 5;
            shaderCache.SetProgramID(MgPipelineBindPoint.GRAPHICS, PROGRAM_ID, internalCache, pipelineLayout);
            Assert.AreEqual(PROGRAM_ID, shaderEntrypoint.ProgramID);
            Assert.AreEqual(blockEntries.Length, shaderEntrypoint.NoOfSetUniformCalls);

            var param = new Internals.GLCmdDescriptorSetParameter
            {
                Bindpoint = MgPipelineBindPoint.GRAPHICS,
                Layout = pipelineLayout,
                DescriptorSet = ds,
                DynamicOffsets = dynamicOffsets,
            };
            shaderCache.BindDescriptorSets(param);
            Assert.AreEqual(blockEntries.Length, shaderEntrypoint.Count);
            Assert.IsNotNull(shaderEntrypoint.UniformBuffers);
            Assert.AreEqual(blockEntries.Length, shaderEntrypoint.UniformBuffers.Length);
            Assert.IsNotNull(shaderEntrypoint.UniformOffsets);
            Assert.AreEqual(blockEntries.Length, shaderEntrypoint.UniformOffsets.Length);
            Assert.IsNotNull(shaderEntrypoint.UniformSizes);
            Assert.AreEqual(blockEntries.Length, shaderEntrypoint.UniformSizes.Length);
        }

        private class MockGLTextureGallery : IGLShaderTextureDescriptorCache
        {
            public void Bind(IGLFutureDescriptorSet ds, GLDescriptorPoolResourceInfo resource)
            {
           
            }

            public void Initialize()
            {
      
            }
        }
    }
}
