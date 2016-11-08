using LightInject;
using Magnesium;
using OpenTK;
using System;

namespace TriangleDemo
{
    class Program
    {
        private static bool IsWindowClosed = false;

        public static void Main(string[] args)
        {
            try
            {

                using (var container = new ServiceContainer())
                using (var window = new NativeWindow())
                {
                    window.Title = "Vulkan Example - Basic indexed triangle";

                    window.Closed += Window_Closed;

                    container.RegisterInstance<INativeWindow>(window);

                    // Magnesium
                    container.Register<Magnesium.MgDriverContext>(new PerContainerLifetime());
                    container.Register<Magnesium.IMgPresentationSurface, Magnesium.PresentationSurfaces.OpenTK.VulkanPresentationSurface>(new PerContainerLifetime());

                    container.Register<Magnesium.IMgGraphicsConfiguration, Magnesium.MgDefaultGraphicsConfiguration>(new PerScopeLifetime());

                    container.Register<Magnesium.IMgImageTools, Magnesium.MgImageTools>(new PerContainerLifetime());

                    // Magnesium.VUlkan
                    container.Register<Magnesium.IMgEntrypoint, Magnesium.Vulkan.VkEntrypoint>(new PerContainerLifetime());

                    // SCOPE
                    container.Register<VulkanExample>(new PerScopeLifetime());
                    container.Register<Magnesium.IMgPresentationBarrierEntrypoint, Magnesium.MgPresentationBarrierEntrypoint>(new PerScopeLifetime());
                    container.Register<Magnesium.IMgGraphicsDevice, Magnesium.MgDefaultGraphicsDevice>(new PerScopeLifetime());
                    container.Register<Magnesium.IMgPresentationLayer, Magnesium.MgPresentationLayer>(new PerScopeLifetime());
                    container.Register<Magnesium.IMgSwapchainCollection, Magnesium.MgSwapchainCollection>(new PerScopeLifetime());


                    using (var scope = container.BeginScope())                    
                    using (var driver = container.GetInstance<MgDriverContext>())
                    {
                        driver.Initialize(
                            new MgApplicationInfo
                            {
                                ApplicationName = "Vulkan Example",
                                ApiVersion = Magnesium.MgApplicationInfo.GenerateApiVersion(1, 0, 17),
                                ApplicationVersion = 1,
                                EngineName = "Magnesium",
                                EngineVersion = 1,
                            },
                            MgInstanceExtensionOptions.ALL);

                        using (var secondLevel = container.BeginScope())
                        {
                            var example = container.GetInstance<VulkanExample>();

                            while (IsWindowClosed)
                            {
                                example.RenderLoop();
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            IsWindowClosed = true;
        }
    }
}