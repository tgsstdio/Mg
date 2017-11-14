# CHANGELOG

Magnesium and its implementations uses [Semantic Versioning v2.0.0](http://semver.org/) for versioning assemblies

## Version 7.0.0
 - Magnesium v7.0.0
 	- Refactored GetMemoryType to MgPhysicalDeviceMemoryProperties and MgGraphicsConfiguration
	- Changed MgGraphicsDeviceCreateInfo to either auto-detect (default) or override color format of the graphics device
	- Changed MgGraphicsDeviceCreateInfo to either auto-detect (default), override, or disable (use none) depth/stencil format of the graphics device
	- Removed unused variable QueueFamilyIndexCount from MgBufferCreateInfo
	- Removed descriptorCount from IMgCommandBuffer.CmdBindDescriptorSets because it was redundant
	- Changed interface of GetInstanceProcAddr & GetDeviceProcAddr to return IntPtr
	- Removed PFN_vkVoidFunction from Mg library
	- Updated CreateDebugReportCallbackEXT w.r.t to GetInstanceProcAddr
	- Updated GetInstanceProcAddr & GetDeviceProcAddr to accept proc names as null-terminated UTF-8 strings
	- Using one graphics device implementation for both Magnesium.Vulkan and Mg.OpenGL.DesktopGL 
	- Removed OpenTKGraphicsDevice and its references from examples and tests 
	- Removed bindless textures implementation 

## Version 5.2.9
 - Magnesium v5.2.9 - 10 Oct 2017 10 Oct 2017
SHOULD BE LAST COMPATIBLE VERSION BEFORE INTERFACE CHANGE i.e. Mg 7.0.0
	- New enum value MgDeviceExtensionOption.SWAPCHAIN_ONLY
	- MgDefaultGraphicsConfiguration uses MgDeviceExtensionOption.SWAPCHAIN_ONLY as its default option
	- MgDefaultGraphicsDevice will ignore user specified DepthFormat and use highest available version
 - Magnesium.OpenGL v6.1.4
	- Updated w.r.t Magnesium v5.2.9
 - Magnesium.OpenGL.DesktopGL v6.1.5
	- Updated w.r.t Magnesium v5.2.9
	- GL002 - Fixed LineStrip issue (missing enum in switch)
 - Magnesium.Vulkan v5.0.7
	- Updated w.r.t Magnesium v5.2.9
 - Magnesium.PresentationSurfaces.OpenTK v1.0.4
	- Updated w.r.t Magnesium v5.2.9
 - Magnesium.Ktx v1.0.4
	- Updated w.r.t Magnesium v5.2.9	
	
## Version 5.2.8-beta
 - Magnesium v5.2.8 - 2 Sep 2017
	- New enum value MgDeviceExtensionOption.SWAPCHAIN_ONLY
	- MgDefaultGraphicsConfiguration uses MgDeviceExtensionOption.SWAPCHAIN_ONLY as its default option
	- MgDefaultGraphicsDevice will ignore user specified DepthFormat and use highest available version
 - Magnesium.OpenGL v6.1.4 beta
 	- Updated w.r.t Magnesium v5.2.7
 - Magnesium.OpenGL.DesktopGL v6.1.4 beta
 	- Updated w.r.t Magnesium v5.2.7
 - Magnesium.Vulkan v5.0.7 beta
	- Updated w.r.t Magnesium v5.2.7 
 - Magnesium.PresentationSurfaces.OpenTK v1.0.4 beta
	- Updated w.r.t Magnesium v5.2.7 
 - Magnesium.Ktx v1.0.4 beta
	- Updated w.r.t Magnesium v5.2.7

## Version 6.0.0
 - Magnesium.OpenGL v6.0.0 - 8 Feb 2017
 - Magnesium.OpenGL.DesktopGL v6.0.1 - 12 Feb 2017
	- Fixed descriptor sets issue with pull request #9 
 - Magnesium.OpenGL.DesktopGL v6.0.0 - 8 Feb 2017
	- w.r.t Magnesium v5.0.4
	- Implemented descriptor sets functionality
	- Implemented GLFullFence of IMgFence interface
	- Implemented CopyBuffers into CommandBuffers

## Version 5.0.0
 - Magnesium v5.0.0
 	- Removing descriptor pool from IMgQueueInfo.CreatePartition because MgDescriptorPools should be explicitly set based on usage
 	- Added IMgGraphicsConfiguration/MgDefaultGraphicsConfiguration to process one IMgThreadPartition 
	- Modify IMgPresentationSurface.Initialize method signature
		-  void Initialize(MgFormat format, uint width, uint height);
	- New assembly created Magnesium.PresentationSurfaces.OpenTK
	- (BUGFIX) Magnesium.Vulkan v5.0.2 - 9th Nov 2016
		- FIXED - CmdBindDescriptorSets to pass in null into dynamic offsets 
		- FIXED - CreateGraphicsPipelines to accept array of dynamic states 
 - Magnesium.Vulkan v5.0.2 - 9 Nov 2016
	- FIXED - CmdBindDescriptorSets to pass in null into dynamic offsets 
	- FIXED - CreateGraphicsPipelines to accept array of dynamic states 

- Magnesium.Metal v5.0.0 - 9 Nov 2016
	- Updated w.r.t Magnesium v5.0.4 	
- Magnesium.Ktx v1.0.0 - 24 Dec 2016
	- Loads Khronos .ktx image files
		
## Version 4.0.0
 - Moving all implementations under one roof/repository
	- Magnesium 
		- interface only
	- Magnesium.Vulkan 
		- Vulkan implementation from existing repository
	- Magnesium.OpenGL 
		- partial implementation created for MonoGame
	- Magnesium.OpenGL.AndroidGL 
	      - implementation created for MonoGame + OpenTK
	- Magnesium.OpenGL.DesktopGL 
		- implementation created for MonoGame + OpenTK
	- Magnesium.Metal 
		- implementation using Metal iOS API for MacOS and iOS

## Version 3.0.0
 - Breaking changes from classes to interfaces due to Magnesium.Vulkan implementation 

## Version 2.0.0
 - Breaking changes to function

## Version 1.0.0

 - Now using Semantic Versioning v2.0.0 (http://semver.org/)




