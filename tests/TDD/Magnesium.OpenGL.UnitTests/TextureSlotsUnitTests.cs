using NUnit.Framework;
using Magnesium.OpenGL.Internals;

namespace Magnesium.OpenGL.UnitTests
{


    class MgTextureDescriptorSet
    {
        public GLTextureSlot[] Descriptors { get; set; }
    }

    [TestFixture]
    public class TextureSlotsUnitTests
    {
        [TestCase]
        public void ConstructorTest()
        {
            var entrypoint = new MockTextureCentralEntrypoint();
            var central = new GLTextureGallery(entrypoint);

            const int EXPECTED = 0;
            Assert.IsNotNull(central.AvailableSlots);
            Assert.AreEqual(EXPECTED, central.AvailableSlots.Length);
        }

        [TestCase]
        public void InitializeTest()
        {
            var entrypoint = new MockTextureCentralEntrypoint();
            var central = new GLTextureGallery(entrypoint);

            const int EXPECTED = 13;
            entrypoint.MaxSlots = EXPECTED;

            central.Initialize();

            AssertInitializedCentral(central, EXPECTED);
            AssertEmptyEntrypoint(entrypoint, EXPECTED);
        }

        private static void AssertInitializedCentral(GLTextureGallery central, int EXPECTED)
        {
            Assert.IsNotNull(central.AvailableSlots);
            Assert.AreEqual(EXPECTED, central.AvailableSlots.Length);

            for (var i = 0U; i < central.AvailableSlots.Length; i += 1)
            {
                var item = central.AvailableSlots[i];
                Assert.AreEqual(i, item.Binding);
                Assert.IsNull(item.Sampler);
                Assert.IsNull(item.View);
            }
        }

        private static void AssertEmptyEntrypoint(MockTextureCentralEntrypoint entrypoint, int noOfSlots)
        {
            for (var i = 0U; i < noOfSlots; i += 1)
            {
                {
                    bool found = entrypoint.Views.TryGetValue(i, out MockTextureCentralEntrypoint.ViewInfo entry);
                    Assert.IsTrue(found);
                    Assert.IsNotNull(entry);
                    Assert.AreEqual(MgImageViewType.TYPE_2D, entry.ViewType);
                    Assert.AreEqual(0, entry.TextureId);
                }

                {
                    bool found = entrypoint.Samplers.TryGetValue(i, out int sampler);
                    Assert.IsTrue(found);
                    Assert.AreEqual(0, sampler);
                }
            }
        }

        class MockGLSampler : IGLSampler
        {
            public int SamplerId { get; set; }

            public void DestroySampler(IMgDevice device, IMgAllocationCallbacks allocator)
            {
                throw new System.NotImplementedException();
            }
        }

        class MockGLImageView : IGLImageView
        {
            public int TextureId { get; set; }

            public MgImageViewType ViewTarget { get; set; }

            public bool IsNullImage => throw new System.NotImplementedException();

            public void DestroyImageView(IMgDevice device, IMgAllocationCallbacks allocator)
            {
                throw new System.NotImplementedException();
            }
        }

        [TestCase]
        public void EmptySetWithSampler()
        {
            var entrypoint = new MockTextureCentralEntrypoint();
            var central = new GLTextureGallery(entrypoint);

            const int MAX_SLOTS = 7;
            entrypoint.MaxSlots = MAX_SLOTS;

            central.Initialize();

            AssertInitializedCentral(central, MAX_SLOTS);
            AssertEmptyEntrypoint(entrypoint, MAX_SLOTS);

            const uint BINDING = 0U;
            const int EXPECTED_TEXTURE_ID = 23;
            var sampler = new MockGLSampler
            {
                SamplerId = EXPECTED_TEXTURE_ID,
            };

            var delta = new MgTextureDescriptorSet
            {
                Descriptors = new[]
                {
                    new GLTextureSlot
                    {
                        Binding = BINDING,
                        Sampler = sampler
                    }
                }
            };

            central.Bind(delta.Descriptors);

            AssertBindOk(entrypoint, delta);
        }

        [TestCase]
        public void EmptySetWithTexture()
        {
            var entrypoint = new MockTextureCentralEntrypoint();
            var central = new GLTextureGallery(entrypoint);

            const int MAX_SLOTS = 7;
            entrypoint.MaxSlots = MAX_SLOTS;

            central.Initialize();

            AssertInitializedCentral(central, MAX_SLOTS);
            AssertEmptyEntrypoint(entrypoint, MAX_SLOTS);

            const uint BINDING = 0U;
            const int EXPECTED_TEXTURE_ID = 23;
            var view = new MockGLImageView
            {
                TextureId = EXPECTED_TEXTURE_ID,
                ViewTarget = MgImageViewType.TYPE_2D,
            };

            var localSet = new MgTextureDescriptorSet
            {
                Descriptors = new[]
                {
                    new GLTextureSlot
                    {
                        Binding = BINDING,
                        View = view
                    }
                }
            };

            central.Bind(localSet.Descriptors);

            AssertBindOk(entrypoint, localSet);
        }

        [TestCase]
        public void UnbindTexture()
        {
            var entrypoint = new MockTextureCentralEntrypoint();
            var central = new GLTextureGallery(entrypoint);

            const int MAX_SLOTS = 7;
            entrypoint.MaxSlots = MAX_SLOTS;

            central.Initialize();

            AssertInitializedCentral(central, MAX_SLOTS);
            AssertEmptyEntrypoint(entrypoint, MAX_SLOTS);

            const uint BINDING = 2U;
            const int EXPECTED_TEXTURE_ID = 23;
            const MgImageViewType EXPECTED_VIEW_TYPE = MgImageViewType.TYPE_2D;
            const int EXPECTED_SAMPLER_ID = 11;

            var view = new MockGLImageView
            {
                TextureId = EXPECTED_TEXTURE_ID,
                ViewTarget = EXPECTED_VIEW_TYPE,
            };

            var sampler = new MockGLSampler
            {
                SamplerId = EXPECTED_SAMPLER_ID,
            };

            var preload = new MgTextureDescriptorSet
            {
                Descriptors = new[]
                {
                    new GLTextureSlot
                    {
                        Binding = BINDING,
                        Sampler = sampler,
                        View = view,
                    }
                }
            };

            central.Bind(preload.Descriptors);
            AssertBindOk(entrypoint, preload);

            var delta = new MgTextureDescriptorSet
            {
                Descriptors = new[]
                {
                    new GLTextureSlot
                    {
                        Binding = BINDING,
                        Sampler = sampler,
                        View = null,
                    }
                }
            };

            central.Bind(delta.Descriptors);
            AssertBindOk(entrypoint, delta);
        }

        [TestCase]
        public void UnbindSampler()
        {
            var entrypoint = new MockTextureCentralEntrypoint();
            var central = new GLTextureGallery(entrypoint);

            const int MAX_SLOTS = 7;
            entrypoint.MaxSlots = MAX_SLOTS;

            central.Initialize();

            AssertInitializedCentral(central, MAX_SLOTS);
            AssertEmptyEntrypoint(entrypoint, MAX_SLOTS);

            const uint BINDING = 5U;
            const int EXPECTED_TEXTURE_ID = 23;
            const MgImageViewType EXPECTED_VIEW_TYPE = MgImageViewType.TYPE_2D;
            const int EXPECTED_SAMPLER_ID = 11;

            var view = new MockGLImageView
            {
                TextureId = EXPECTED_TEXTURE_ID,
                ViewTarget = EXPECTED_VIEW_TYPE,
            };

            var sampler = new MockGLSampler
            {
                SamplerId = EXPECTED_SAMPLER_ID,
            };

            var preload = new MgTextureDescriptorSet
            {
                Descriptors = new[]
                {
                    new GLTextureSlot
                    {
                        Binding = BINDING,
                        Sampler = sampler,
                        View = view,
                    }
                }
            };

            central.Bind(preload.Descriptors);
            AssertBindOk(entrypoint, preload);

            var delta = new MgTextureDescriptorSet
            {
                Descriptors = new[]
                {
                    new GLTextureSlot
                    {
                        Binding = BINDING,
                        Sampler = null,
                        View = view,
                    }
                }
            };

            central.Bind(delta.Descriptors);
            AssertBindOk(entrypoint, delta);
        }

        [TestCase]
        public void UnbindAll()
        {
            var entrypoint = new MockTextureCentralEntrypoint();
            var central = new GLTextureGallery(entrypoint);

            const int MAX_SLOTS = 7;
            entrypoint.MaxSlots = MAX_SLOTS;

            central.Initialize();

            AssertInitializedCentral(central, MAX_SLOTS);
            AssertEmptyEntrypoint(entrypoint, MAX_SLOTS);

            const uint BINDING = 1U;
            const int EXPECTED_TEXTURE_ID = 23;
            const MgImageViewType EXPECTED_VIEW_TYPE = MgImageViewType.TYPE_2D;
            const int EXPECTED_SAMPLER_ID = 11;

            var view = new MockGLImageView
            {
                TextureId = EXPECTED_TEXTURE_ID,
                ViewTarget = EXPECTED_VIEW_TYPE,
            };

            var sampler = new MockGLSampler
            {
                SamplerId = EXPECTED_SAMPLER_ID,
            };

            var preload = new MgTextureDescriptorSet
            {
                Descriptors = new[]
                {
                    new GLTextureSlot
                    {
                        Binding = BINDING,
                        Sampler = sampler,
                        View = view,
                    }
                }
            };

            central.Bind(preload.Descriptors);
            AssertBindOk(entrypoint, preload);

            var delta = new MgTextureDescriptorSet
            {
                Descriptors = new[]
                {
                    new GLTextureSlot
                    {
                        Binding = BINDING,
                        Sampler = null,
                        View = null,
                    }
                }
            };

            central.Bind(delta.Descriptors);
            AssertBindOk(entrypoint, delta);
        }

        private static void AssertBindOk(MockTextureCentralEntrypoint entrypoint, MgTextureDescriptorSet source)
        {
            foreach (var descriptor in source.Descriptors)
            {
                var binding = descriptor.Binding;
                var samplerId = descriptor.Sampler != null ? descriptor.Sampler.SamplerId : 0;
                // MAKE SURE SAMPLER IS UNAFFECTED
                {
                    bool found = entrypoint.Samplers.TryGetValue(binding, out int actual);
                    Assert.IsTrue(found);
                    Assert.AreEqual(samplerId, actual);
                }

                var textureId = descriptor.View != null ? descriptor.View.TextureId : 0;
                {
                    bool found = entrypoint.Views.TryGetValue(binding, out MockTextureCentralEntrypoint.ViewInfo entry);
                    Assert.IsTrue(found);
                    Assert.IsNotNull(entry);

                    Assert.AreEqual(textureId, entry.TextureId);
                    if (descriptor.View != null)
                    {
                        Assert.AreEqual(descriptor.View.ViewTarget, entry.ViewType);
                    }
                    else
                    {
                        Assert.AreEqual(MgImageViewType.TYPE_2D, entry.ViewType);
                    }
                }
            }
        }
    }
}

