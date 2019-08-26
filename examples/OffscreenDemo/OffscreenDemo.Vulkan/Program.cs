using LightInject;
using Magnesium.Toolkit;
using Magnesium.Utilities;
using OpenTK;
using System;

namespace OffscreenDemo
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {

                using (var container = new ServiceContainer())
                using (var window = new NativeWindow())
                {
                    window.Title = "Offscreen Demo ";
                    window.Visible = true;


                    container.RegisterInstance<INativeWindow>(window);

                    // Magnesium
                    container.Register<MgDriverContext>(new PerContainerLifetime());
                    container.Register<IMgPresentationSurface, Magnesium.PresentationSurfaces.OpenTK.VulkanPresentationSurface>(new PerContainerLifetime());

                    container.Register<IMgGraphicsConfiguration, MgDefaultGraphicsConfiguration>(new PerContainerLifetime());
                    container.Register<IMgImageTools, MgImageTools>(new PerContainerLifetime());

                    // Magnesium.VUlkan
                    container.Register<Magnesium.IMgEntrypoint, Magnesium.Vulkan.VkEntrypoint>(new PerContainerLifetime());

                    // SCOPE

                    container.Register<IMgPresentationBarrierEntrypoint, MgPresentationBarrierEntrypoint>(new PerContainerLifetime());

                    using (var scope = container.BeginScope())
                    {
                        // GAME START
                        container.Register<Example>(new PerScopeLifetime());

                        container.Register<IDemoApplication, OffscreenDemoApplication>(new PerScopeLifetime());
                        //container.Register<IDemoApplication, TriangleDemoApplication>(new PerScopeLifetime());

                        container.Register<IMgPlatformMemoryLayout, VkPlatformMemoryLayout>(new PerScopeLifetime());
                        //container.Register<IMgPlatformMemoryLayout, VkDebugVertexPlatformMemoryLayout>(new PerScopeLifetime());
                        container.Register<MgOffscreenDeviceFactory>(new PerScopeLifetime());
                        container.Register<IMgOffscreenDeviceEntrypoint, MgDefaultOffscreenDeviceEntrypoint>(new PerScopeLifetime());

                        container.Register<IMgOptimizedStoragePartitioner, MgOptimizedStoragePartitioner>(new PerScopeLifetime());
                        container.Register<IMgOptimizedStoragePartitionVerifier, MgOptimizedStoragePartitionVerifier>(new PerScopeLifetime());
                        container.Register<MgOptimizedStorageBuilder>(new PerScopeLifetime());

                        container.Register<OffscreenPipeline>(new PerScopeLifetime());
                        container.Register<IOffscreenPipelineMediaPath, VkOffscreenDemoShaderPath>(new PerScopeLifetime());

                        container.Register<ToScreenPipeline>(new PerScopeLifetime());
                        container.Register<IToScreenPipelineMediaPath, VkToScreenPipelinePath>(new PerScopeLifetime());

                        // GAME END

                        container.Register<IMgGraphicsDevice, MgDefaultGraphicsDevice>(new PerScopeLifetime());
                        container.Register<IMgGraphicsDeviceContext, MgDefaultGraphicsDeviceContext>(new PerScopeLifetime());
                        container.Register<IMgPresentationLayer, MgPresentationLayer>(new PerScopeLifetime());
                        container.Register<IMgSwapchainCollection, MgSwapchainCollection>(new PerScopeLifetime());
                        container.Register<MgGraphicsConfigurationManager>(new PerScopeLifetime());


                        using (var driver = container.GetInstance<MgDriverContext>())
                        {
                            driver.Initialize(
                                new Magnesium.MgApplicationInfo
                                {
                                    ApplicationName = "OffscreenDemo",
                                    ApiVersion = Magnesium.MgApplicationInfo.GenerateApiVersion(1, 0, 30),
                                    ApplicationVersion = 1,
                                    EngineName = "Magnesium",
                                    EngineVersion = 1,
                                },
                                MgInstanceExtensionOptions.ALL);
                            using (var graphicsConfiguration = container.GetInstance<IMgGraphicsConfiguration>())
                            using (var secondLevel = scope.BeginScope())
                            using (var gameWindow = new GameWindow(window))
                            using (var example = container.GetInstance<Example>())
                            {
                                try
                                {
                                    example.Initialize();
                                    gameWindow.RenderFrame += (sender, e) =>
                                    {
                                        example.Render();
                                    };

                                    gameWindow.Run(60, 60);
                                }
                                finally
                                {
                                    example.Dispose();
                                }
                            }                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
