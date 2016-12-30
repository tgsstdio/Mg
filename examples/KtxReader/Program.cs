using Magnesium;
using Magnesium.Ktx;
using SimpleInjector;
using System;

namespace KtxReader
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("HELLO WORLD");

            try
            {
                using (var container = new SimpleInjector.Container())
                {
                    // Magnesium
                    container.Register<Magnesium.MgDriverContext>(Lifestyle.Singleton);

                    // Magnesium.VUlkan
                    container.Register<Magnesium.IMgEntrypoint, Magnesium.Vulkan.VkEntrypoint>(Lifestyle.Singleton);
                    container.Register<Magnesium.IMgImageTools, Magnesium.MgImageTools>(Lifestyle.Singleton);

                    container.Register<IKTXTextureLoader, KTXTextureManager>(Lifestyle.Singleton);
                    container.Register<IMgTextureGenerator, MgStagingBufferOptimizer>(Lifestyle.Singleton);
                    container.Register<IMgGraphicsConfiguration, MgDefaultGraphicsConfiguration>(Lifestyle.Singleton);
                    container.Register<IMgPresentationSurface, MgNullSurface>(Lifestyle.Singleton);

                    using (var driver = container.GetInstance<Magnesium.MgDriverContext>())
                    {
                        driver.Initialize(
                            new MgApplicationInfo
                            {
                                ApplicationName = "Vulkan Example",
                                ApiVersion = MgApplicationInfo.GenerateApiVersion(1, 0, 17),
                                ApplicationVersion = 1,
                                EngineName = "Magnesium",
                                EngineVersion = 1,
                            },
                            MgInstanceExtensionOptions.ALL);


                        using (var scope = new SimpleInjector.Scope(container))
                        {
                            var configuration = container.GetInstance<IMgGraphicsConfiguration>();
                            configuration.Initialize(0, 0);

                            IKTXTextureLoader loader = container.GetInstance<IKTXTextureLoader>();
                            using (var fs = System.IO.File.OpenRead("1.ktx"))
                            {
                                var result = loader.Load(fs);
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
    }
}
