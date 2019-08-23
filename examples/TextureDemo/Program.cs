using OpenTK;
using System;
using DryIoc;
using Magnesium.Toolkit;

namespace TextureDemo
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {

                using (var container = new Container ())
                using (var window = new NativeWindow())
                {                   
                    window.Title = "Vulkan Example - Basic indexed triangle";
                    window.Visible = true;


                    container.RegisterInstance<INativeWindow>(window);

                    // Magnesium
                    container.Register<MgDriverContext>(Reuse.Singleton);
                    container.Register<IMgPresentationSurface, Magnesium.PresentationSurfaces.OpenTK.VulkanPresentationSurface>(Reuse.Singleton);

                    container.Register<IMgGraphicsConfiguration, MgDefaultGraphicsConfiguration>(Reuse.Singleton);
                    container.Register<IMgImageTools, MgImageTools>(Reuse.Singleton);

                    // Magnesium.VUlkan
                    container.Register<Magnesium.IMgEntrypoint, Magnesium.Vulkan.VkEntrypoint>(Reuse.Singleton);

                    // SCOPE

                    container.Register<IMgPresentationBarrierEntrypoint, MgPresentationBarrierEntrypoint>(Reuse.Singleton);

                    using (var scope = container.OpenScope())
                    {
                        container.Register<TextureExample>(Reuse.InResolutionScope);
                        container.Register<IMgGraphicsDevice, MgDefaultGraphicsDevice>(Reuse.InResolutionScope);
                        container.Register<IMgPresentationLayer, MgPresentationLayer>(Reuse.InResolutionScope);
                        container.Register<IMgSwapchainCollection, MgSwapchainCollection>(Reuse.InResolutionScope);
                        container.Register<MgGraphicsConfigurationManager>(Reuse.InResolutionScope);


                        using (var driver = container.Resolve<MgDriverContext>())
                        {
                            driver.Initialize(
                                new Magnesium.MgApplicationInfo
                                {
                                    ApplicationName = "Vulkan Example",
                                    ApiVersion = Magnesium.MgApplicationInfo.GenerateApiVersion(1, 0, 30),
                                    ApplicationVersion = 1,
                                    EngineName = "Magnesium",
                                    EngineVersion = 1,
                                },
                                MgInstanceExtensionOptions.ALL);
                            using (var graphicsConfiguration = container.Resolve<IMgGraphicsConfiguration>())
                            {
                                using (var secondLevel = container.OpenScope())
                                {
                                    using (var gameWindow = new GameWindow(window))
                                    using (var example = container.Resolve<TextureExample>())
                                    {                                        
                                        gameWindow.RenderFrame += (sender, e) =>
                                        {
                                            example.Render();
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
