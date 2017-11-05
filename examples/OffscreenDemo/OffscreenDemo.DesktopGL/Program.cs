using LightInject;
using Magnesium;
using Magnesium.Utilities;
using OpenTK;
using System;

namespace OffscreenDemo.DesktopGL
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var container = new ServiceContainer())
                using (var window = new NativeWindow())
                {
                    window.Title = "OffscreenDemo";
                    window.Visible = true;

                    container.RegisterInstance<INativeWindow>(window);

                    container.Register<Example>(new PerScopeLifetime());
                    container.Register<MgGraphicsConfigurationManager>(new PerScopeLifetime());

                    container.Register<IDemoApplication,
                        OffscreenDemoApplication>(new PerScopeLifetime());
                    //container.Register<IDemoApplication,
                    //    TriangleDemoApplication>(new PerScopeLifetime());
                    container.Register<OffscreenPipeline>(new PerScopeLifetime());
                    container.Register<ToScreenPipeline>(new PerScopeLifetime());

                    //container.Register<IMgPlatformMemoryLayout,
                    //   FullGLPlatformMemoryLayout>(new PerScopeLifetime());
                    container.Register<IMgPlatformMemoryLayout, 
                        VkDebugVertexPlatformMemoryLayout >(new PerScopeLifetime());

                    container.Register<IMgOptimizedStoragePartitioner,
                        MgOptimizedStoragePartitioner>(new PerScopeLifetime());
                    container.Register<IMgOptimizedStoragePartitionVerifier,
                        MgOptimizedStoragePartitionVerifier>(new PerScopeLifetime());
                    container.Register<MgOptimizedStorageBuilder>(new PerScopeLifetime());

                    container.Register<Magnesium.MgOffscreenDeviceFactory>(new PerScopeLifetime());
                    container.Register<Magnesium.IMgOffscreenDeviceEntrypoint, 
                        Magnesium.OpenGL.GLOffscreenDeviceEntrypoint>(new PerScopeLifetime());

                    // APP PLATFORM IMPLEMENTATION 
                    container.Register<IOffscreenPipelineMediaPath,
                        GLOffscreenDemoShaderPath>(new PerScopeLifetime());
                    container.Register<IToScreenPipelineMediaPath,
                        GLToScreenPipelinePath>(new PerScopeLifetime());

                    ApplyMagnesiumRegistrations(container);
                    ApplyDesktopGLRegistrations(container);

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
                        using (var graphicsConfiguration = container.GetInstance<IMgGraphicsConfiguration>())
                        using (var secondLevel = container.BeginScope())                            
                        using (var gameWindow = new GameWindow(window))
                        using (var example = secondLevel.GetInstance<Example>())
                        {
                            example.Initialize();
                            gameWindow.RenderFrame += (sender, e) =>
                            {
                                example.Render();
                            };

                            gameWindow.Run(10, 10);
                        }                                                
                    }                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void ApplyMagnesiumRegistrations(ServiceContainer container)
        {
            container.Register<Magnesium.MgDriverContext>(new PerContainerLifetime());
            container.Register<Magnesium.IMgGraphicsConfiguration,
                Magnesium.MgDefaultGraphicsConfiguration>(new PerContainerLifetime());
            container.Register<Magnesium.IMgImageTools,
                Magnesium.MgImageTools>(new PerContainerLifetime());

            container.Register<Magnesium.IMgGraphicsDevice,
                Magnesium.MgDefaultGraphicsDevice>(new PerScopeLifetime());
            container.Register<Magnesium.IMgPresentationLayer,
                Magnesium.MgPresentationLayer>(new PerScopeLifetime());

            container.Register<Magnesium.IMgPresentationBarrierEntrypoint,
                Magnesium.MgPresentationBarrierEntrypoint>(new PerScopeLifetime());
        }

        static void ApplyDesktopGLRegistrations(ServiceContainer container)
        {
            container.Register<Magnesium.IMgTextureGenerator,
                Magnesium.MgLinearImageOptimizer>(new PerContainerLifetime());

            // Magnesium.OpenGL
            container.Register<Magnesium.IMgEntrypoint,
                Magnesium.OpenGL.GLEntrypoint>(new PerContainerLifetime());
            // for depth format
            container.Register<Magnesium.OpenGL.IGLPhysicalDeviceFormatLookupEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLPhysicalDeviceFormatLookupEntrypoint>(new PerContainerLifetime());

            // Magnesium.OpenGL INTERNALS
            container.Register<Magnesium.OpenGL.IGLGraphicsPipelineCompiler,
                Magnesium.OpenGL.GLSLGraphicsPipelineCompilier>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLQueue,
                Magnesium.OpenGL.GLCmdQueue>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLRenderer,
                Magnesium.OpenGL.GLCmdRenderer>(new PerContainerLifetime());

            container.Register<Magnesium.OpenGL.IGLCmdStateRenderer,
                Magnesium.OpenGL.GLCmdStateRenderer>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLDeviceEntrypoint,
                Magnesium.OpenGL.DefaultGLDeviceEntrypoint>(new PerContainerLifetime());

            container.Register<Magnesium.OpenGL.IGLCmdBlendEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLCmdBlendEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLCmdStencilEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLCmdStencilEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLCmdRasterizationEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLCmdRasterizationEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLErrorHandler,
                Magnesium.OpenGL.DesktopGL.FullGLErrorHandler>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLCmdDepthEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLCmdDepthEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLNextCmdShaderProgramCache,
                Magnesium.OpenGL.GLNextCmdShaderProgramCache>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLCmdScissorsEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLCmdScissorsEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLCmdDrawEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLCmdDrawEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLCmdClearEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLCmdClearEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLSemaphoreEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLSemaphoreEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLCmdImageEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLCmdImageEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLCmdVBOEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLCmdVBOEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLSamplerEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLSamplerEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLDeviceImageEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLDeviceImageEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLDeviceImageViewEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLDeviceImageViewEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLImageFormatEntrypoint,
                Magnesium.OpenGL.DesktopGL.DesktopGLImageFormatEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLImageDescriptorEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLImageDescriptorEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLShaderModuleEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLShaderModuleEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLDescriptorPoolEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLDescriptorPoolEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLBufferEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLBufferEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLDeviceMemoryEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLDeviceMemoryEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLGraphicsPipelineEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLGraphicsPipelineEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLFramebufferHelperSelector,
                Magnesium.OpenGL.DesktopGL.FullGLFramebufferHelperSelector>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLFramebufferSupport,
                Magnesium.OpenGL.DesktopGL.FullGLFramebufferSupport>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLExtensionLookup,
                Magnesium.OpenGL.DesktopGL.FullGLExtensionLookup>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLFenceEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLFullFenceEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLBlitOperationEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLBlitOperationEntrypoint>(new PerContainerLifetime());

            // DESCRIPTOR SET
            container.Register<Magnesium.OpenGL.IGLUniformBlockEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLUniformBlockEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLUniformBlockNameParser,
                Magnesium.OpenGL.DefaultGLUniformBlockNameParser>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLCmdShaderProgramEntrypoint,
                Magnesium.OpenGL.DesktopGL.FullGLCmdShaderProgramEntrypoint>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.IGLDescriptorSetEntrypoint,
                Magnesium.OpenGL.DefaultGLDescriptorSetEntrypoint>(new PerContainerLifetime());


            // Magnesium.OpenGL.DesktopGL INTERNALS
            container.Register<Magnesium.OpenGL.DesktopGL.IOpenTKSwapchainKHR, 
                Magnesium.OpenGL.DesktopGL.GLSwapchainKHR>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.DesktopGL.IGLBackbufferContext, 
                Magnesium.OpenGL.DesktopGL.OpenTKBackbufferContext>(new PerContainerLifetime());
            container.Register<Magnesium.OpenGL.DesktopGL.IMgGraphicsDeviceLogger, 
                Magnesium.OpenGL.DesktopGL.NullMgGraphicsDeviceLogger>(new PerContainerLifetime());

            // PER SCOPE 
            container.Register<Magnesium.IMgGraphicsDeviceContext,
                Magnesium.OpenGL.DesktopGL.DesktopGLGraphicsContext>(new PerScopeLifetime());
            container.Register<Magnesium.IMgSwapchainCollection,
                Magnesium.OpenGL.DesktopGL.OpenTKSwapchainCollection>(new PerScopeLifetime());

            container.Register<Magnesium.IMgPresentationSurface,
                Magnesium.PresentationSurfaces.OpenTK.DesktopGLPresentationSurface>(new PerContainerLifetime());
        }
    }
}
