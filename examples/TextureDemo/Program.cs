using OpenTK;
using System;
using DryIoc;

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
                    window.Title = "TextureDemo - KTX Texture demo";
                    window.Visible = true;


                    container.RegisterInstance<INativeWindow>(window);

                    // Magnesium
                    container.Register<Magnesium.MgDriverContext>(Reuse.Singleton);
                    container.Register<Magnesium.IMgPresentationSurface, Magnesium.PresentationSurfaces.OpenTK.VulkanPresentationSurface>(Reuse.Singleton);

                    container.Register<Magnesium.IMgGraphicsConfiguration, Magnesium.MgDefaultGraphicsConfiguration>(Reuse.Singleton);
                    container.Register<Magnesium.IMgImageTools, Magnesium.MgImageTools>(Reuse.Singleton);

                    // Magnesium.VUlkan
                    container.Register<Magnesium.IMgEntrypoint, Magnesium.Vulkan.VkEntrypoint>(Reuse.Singleton);

                    // SCOPE

                    container.Register<Magnesium.IMgPresentationBarrierEntrypoint, Magnesium.MgPresentationBarrierEntrypoint>(Reuse.Singleton);

                    using (var scope = container.OpenScope())
                    {
                        container.Register<TextureExample>(Reuse.InResolutionScope);
                        container.Register<Magnesium.IMgGraphicsDevice, Magnesium.MgDefaultGraphicsDevice>(Reuse.InResolutionScope);
                        container.Register<Magnesium.IMgGraphicsDeviceContext, Magnesium.MgDefaultGraphicsDeviceContext>(Reuse.InResolutionScope);
                        container.Register<Magnesium.IMgPresentationLayer, Magnesium.MgPresentationLayer>(Reuse.InResolutionScope);
                        container.Register<Magnesium.IMgSwapchainCollection, Magnesium.MgSwapchainCollection>(Reuse.InResolutionScope);
                        container.Register<MgGraphicsConfigurationManager>(Reuse.InResolutionScope);


                        using (var driver = container.Resolve<Magnesium.MgDriverContext>())
                        {
                            driver.Initialize(
                                new Magnesium.MgApplicationInfo
                                {
                                    ApplicationName = "TextureDemo",
                                    ApiVersion = Magnesium.MgApplicationInfo.GenerateApiVersion(1, 0, 30),
                                    ApplicationVersion = 1,
                                    EngineName = "Magnesium",
                                    EngineVersion = 1,
                                },
                                Magnesium.MgInstanceExtensionOptions.ALL);
                            using (var graphicsConfiguration = container.Resolve<Magnesium.IMgGraphicsConfiguration>())
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
