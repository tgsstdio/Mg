using LightInject;
using Magnesium;
using Magnesium.Toolkit;
using OpenTK;
using System;

namespace TriangleDemo.Vulkan
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
                    window.Title = "Vulkan Example - Basic indexed triangle";
                    window.Visible = true;

                    container.RegisterInstance<INativeWindow>(window);

                    // Magnesium                    
                    container.Register<Magnesium.Toolkit.MgDriverContext>(new PerContainerLifetime());
                    container.Register<Magnesium.Toolkit.IMgPresentationSurface, Magnesium.PresentationSurfaces.OpenTK.VulkanPresentationSurface>(new PerContainerLifetime());

                    container.Register<Magnesium.Toolkit.IMgGraphicsConfiguration, Magnesium.Toolkit.MgDefaultGraphicsConfiguration>(new PerContainerLifetime());
                    container.Register<Magnesium.Toolkit.IMgImageTools, Magnesium.Toolkit.MgImageTools>(new PerContainerLifetime());

                    // Magnesium.VUlkan
                    container.Register<TriangleDemo.ITriangleDemoShaderPath, SPIRVTriangleShaderPath>(new PerContainerLifetime());
                    container.Register<Magnesium.IMgEntrypoint, Magnesium.Vulkan.VkEntrypoint>(new PerContainerLifetime());

                    // SCOPE
                    container.Register<Magnesium.Toolkit.IMgGraphicsDevice, MgDefaultGraphicsDevice>(new PerScopeLifetime());
                    container.Register<Magnesium.Toolkit.IMgGraphicsDeviceContext, MgDefaultGraphicsDeviceContext>(new PerScopeLifetime());

                    container.Register<VulkanExample>(new PerScopeLifetime());
                    container.Register<Magnesium.Toolkit.IMgPresentationBarrierEntrypoint, Magnesium.Toolkit.MgPresentationBarrierEntrypoint>(new PerScopeLifetime());

                    container.Register<Magnesium.Toolkit.IMgPresentationLayer, Magnesium.Toolkit.MgPresentationLayer>(new PerScopeLifetime());
                    container.Register<Magnesium.Toolkit.IMgSwapchainCollection, Magnesium.Toolkit.MgSwapchainCollection>(new PerScopeLifetime());

                    //var displayInfo = new GLTriangleDemoDisplayInfo
                    //{
                    //    Color = MgFormat.R8G8B8A8_UINT,
                    //    Depth = MgFormat.D24_UNORM_S8_UINT
                    //};

                    //container.RegisterInstance<TriangleDemo.ITriangleDemoDisplayInfo>(displayInfo);

                    using (var scope = container.BeginScope())
                    {
                        using (var driver = container.GetInstance<Magnesium.Toolkit.MgDriverContext>())
                        {
                            driver.Initialize(
                                new MgApplicationInfo
                                {
                                    ApplicationName = "Vulkan Example",
                                    ApiVersion = Magnesium.MgApplicationInfo.GenerateApiVersion(1, 1, 114),
                                    ApplicationVersion = 1,
                                    EngineName = "Magnesium",
                                    EngineVersion = 1,
                                },
                                Magnesium.Toolkit.MgInstanceExtensionOptions.ALL);
                            using (var graphicsConfiguration = container.GetInstance<Magnesium.Toolkit.IMgGraphicsConfiguration>())
                            {
                                using (var secondLevel = container.BeginScope())
                                {
                                    using (var gameWindow = new GameWindow(window))
                                    using (var example = container.GetInstance<VulkanExample>())
                                    {
                                        gameWindow.RenderFrame += (sender, e) =>
                                        {
                                            example.RenderLoop();
                                        };

                                        gameWindow.Run(60, 60);
                                    }
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