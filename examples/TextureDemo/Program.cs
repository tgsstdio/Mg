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
                    window.Title = "Vulkan Example - Basic indexed triangle";
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
                    container.Register<Magnesium.IMgGraphicsDevice, Magnesium.MgDefaultGraphicsDevice>(Reuse.InCurrentScope);
                    container.Register<TextureExample>(Reuse.InCurrentScope);
                    container.Register<Magnesium.IMgPresentationBarrierEntrypoint, Magnesium.MgPresentationBarrierEntrypoint>(Reuse.InCurrentScope);

                    container.Register<Magnesium.IMgPresentationLayer, Magnesium.MgPresentationLayer>(Reuse.InCurrentScope);
                    container.Register<Magnesium.IMgSwapchainCollection, Magnesium.MgSwapchainCollection>(Reuse.InCurrentScope);


                    using (var scope = container.OpenScope())
                    {
                        using (var driver = container.Resolve<Magnesium.MgDriverContext>())
                        {
                            driver.Initialize(
                                new Magnesium.MgApplicationInfo
                                {
                                    ApplicationName = "Vulkan Example",
                                    ApiVersion = Magnesium.MgApplicationInfo.GenerateApiVersion(1, 0, 17),
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
