using LightInject;
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
                    container.Register<Magnesium.MgDriverContext>(new PerContainerLifetime());
                    container.Register<Magnesium.IMgPresentationSurface, Magnesium.PresentationSurfaces.OpenTK.VulkanPresentationSurface>(new PerContainerLifetime());

                    container.Register<Magnesium.IMgGraphicsConfiguration, Magnesium.MgDefaultGraphicsConfiguration>(new PerContainerLifetime());
                    container.Register<Magnesium.IMgImageTools, Magnesium.MgImageTools>(new PerContainerLifetime());

                    // Magnesium.VUlkan
                    container.Register<Magnesium.IMgEntrypoint, Magnesium.Vulkan.VkEntrypoint>(new PerContainerLifetime());

                    // SCOPE

                    container.Register<Magnesium.IMgPresentationBarrierEntrypoint, Magnesium.MgPresentationBarrierEntrypoint>(new PerContainerLifetime());

                    using (var scope = container.BeginScope())
                    {
                        // GAME START
                        container.Register<Example>(new PerScopeLifetime());

                        // container.Register<IDemoApplication, OffscreenDemoApplication>(new PerScopeLifetime());
                        container.Register<IDemoApplication, TriangleDemoApplication>(new PerScopeLifetime());

                        // container.Register<IMgPlatformMemoryLayout, VkPlatformMemoryLayout>(new PerScopeLifetime());
                        container.Register<IMgPlatformMemoryLayout, VkDebugVertexPlatformMemoryLayout>(new PerScopeLifetime());

                        
                        container.Register<IMgOptimizedStoragePartitioner, MgOptimizedStoragePartitioner>(new PerScopeLifetime());
                        container.Register<IMgOptimizedStoragePartitionVerifier, MgOptimizedStoragePartitionVerifier>(new PerScopeLifetime());
                        container.Register<MgOptimizedStorageBuilder>(new PerScopeLifetime());

                        container.Register<OffscreenPipeline>(new PerScopeLifetime());
                        container.Register<IOffscreenPipelineMediaPath, VkOffscreenDemoShaderPath>(new PerScopeLifetime());

                        container.Register<ToScreenPipeline>(new PerScopeLifetime());
                        container.Register<IToScreenPipelineMediaPath, VkToScreenPipelinePath>(new PerScopeLifetime());

                        // GAME END


                        container.Register<Magnesium.IMgGraphicsDevice, Magnesium.MgDefaultGraphicsDevice>(new PerScopeLifetime());
                        container.Register<Magnesium.IMgGraphicsDeviceContext, Magnesium.MgDefaultGraphicsDeviceContext>(new PerScopeLifetime());
                        container.Register<Magnesium.IMgPresentationLayer, Magnesium.MgPresentationLayer>(new PerScopeLifetime());
                        container.Register<Magnesium.IMgSwapchainCollection, Magnesium.MgSwapchainCollection>(new PerScopeLifetime());
                        container.Register<MgGraphicsConfigurationManager>(new PerScopeLifetime());


                        using (var driver = container.GetInstance<Magnesium.MgDriverContext>())
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
                                Magnesium.MgInstanceExtensionOptions.ALL);
                            using (var graphicsConfiguration = container.GetInstance<Magnesium.IMgGraphicsConfiguration>())
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
