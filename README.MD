# Magnesium

> C# Vulkan interface/polyfill for WINDOWS and MacOS

Quick Links

- [CHANGELOG](#changelog)
- [NUGET PACKAGES](#nuget-packages)
- [EXAMPLES](#examples)
- [BUILD INSTRUCTIONS](#build-instructions)
- [DESIGN DECISIONS](#design-decisions)
- [LICENSE](#license)
- [WHY?](#why?)
- [CROSS PLATFORM DEV GUIDE](#cross-platform-dev-guide)
- [PLATFORM SUMMARY](#platform-summary)
- [IMPLEMENTATIONS TODOs](#implementations-todos)

----------


## CHANGELOG

Magnesium and its implementations uses [Semantic Versioning v2.0.0](http://semver.org/) for versioning assemblies

### Version 6.0.0
 - Magnesium.OpenGL v6.0.0 - 8 Feb 2017
 - Magnesium.OpenGL.DesktopGL v6.0.0 - 8 Feb 2017
	- w.r.t Magnesium v5.0.4
	- Implemented descriptor sets functionality
	- Implemented GLFullFence of IMgFence interface
	- Implemented CopyBuffers into CommandBuffers

### Version 5.0.0
 - Removing descriptor pool from IMgQueueInfo.CreatePartition because MgDescriptorPools should be explicitly set based on usage
 - Added IMgGraphicsConfiguration/MgDefaultGraphicsConfiguration to process one IMgThreadPartition 
 - Modify IMgPresentationSurface.Initialize method signature
	 -  void Initialize(MgFormat format, uint width, uint height);
 - New assembly created Magnesium.PresentationSurfaces.OpenTK
 - (BUGFIX) Magnesium.Vulkan v5.0.2 - 9th Nov 2016
	- FIXED - CmdBindDescriptorSets to pass in null into dynamic offsets 
	- FIXED - CreateGraphicsPipelines to accept array of dynamic states 
 - Magnesium.Metal v5.0.0 - 9 Nov 2016
 	- Updated w.r.t Magnesium v5.0.4 	
 - Magnesium.Ktx v1.0.0 - 24 Dec 2016
	- Loads Khronos .ktx image files
		
### Version 4.0.0
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

### Version 3.0.0
 - Breaking changes from classes to interfaces due to Magnesium.Vulkan implementation 

### Version 2.0.0
 - Breaking changes to function

### Version 1.0.0

 - Now using Semantic Versioning v2.0.0 (http://semver.org/)


----------

## NUGET PACKAGES

| Interface / Implementation | Latest Version  | Description |
|-----------|----------|-----------|
| [Magnesium](https://www.nuget.org/packages/Magnesium/) | 5.1.1  | Core C# Vulkan interface with all applications should reference |
| [Magnesium.Vulkan](https://www.nuget.org/packages/Magnesium.Vulkan/) | 5.0.3 | C# interface to Vulkan version 1.0.17 |
| [Magnesium.Metal](https://www.nuget.org/packages/Magnesium.Metal/) | 5.0.0  |iOS polyfill for Magnesium for iOS devices using Apple's own Metal API |
| [Magnesium.OpenGL](https://www.nuget.org/packages/Magnesium.OpenGL/) | 6.1.0  | OpenGL enumation/polyfill to replicate most of Vulkan entities and features |
| [Magnesium.OpenGL.DesktopGL](https://www.nuget.org/packages/Magnesium.OpenGL.DesktopGL/) | 6.1.0  | OpenTK / OpenGL 4.5 implementation |
| [Magnesium.PresentationSurfaces.OpenTK](https://www.nuget.org/packages/Magnesium.PresentationSurfaces.OpenTK/) | 1.0.2  | Contains OpenTK implementation of IMgPresentationSurface for Vulkan & DesktopGL |
| [Magnesium.Ktx](https://www.nuget.org/packages/Magnesium.Ktx/) | 1.0.0 | Loads Khronos .ktx image files |

-----------

## BUILD INSTRUCTIONS 


 1. Change directory to the Build folder in the repository 
 2. Open ```Mg ALL BUILD.sln``` in Visual Studio or Xamarin Studio
 3. Build all projects attached to this solution. The assemblies can be found in the bin/ 
 	- Magnesium
	- Magnesium.OpenGL
	- Magnesium.Vulkan

-----------

## EXAMPLES 
	
All Magnesium demos can be found in the examples folder
 
 - InstanceDemo (Windows)
 	- Console only Vulkan interface test
 - MetalSample (MacOS)
 	- IOC library used - [SimpleInjector](https://simpleinjector.org/index.html)
 	- Spinning cube translation of Metal cube demo
 - KtxReader (Cross-Platform)
 	- Opens a .ktx for testing
 - TriangleDemo (Windows, OpenTK)
	- Shared application code running on Vulkan and OpenGL 
 	- IOC library used - [LightInject](http://www.lightinject.net/)
	- Simple Vulkan triangle demo based on [Sascha Willems](https://github.com/SaschaWillems)'s [triangle demo](https://github.com/SaschaWillems/Vulkan/blob/master/triangle/triangle.cpp)
 - TextureDemo (Windows, OpenTK)
  	- IOC library used - [DryIoc](https://bitbucket.org/dadhi/dryioc)
 	- Opens a .ktx image file and display it on a plane. Code based on [Sascha Willems](https://github.com/SaschaWillems)'s [texture demo](https://github.com/SaschaWillems/Vulkan/blob/master/texture/texture.cpp)

	
----------

## DESIGN DECISIONS

1. It is the responsibility of the programmer to clean up any unmanaged memory used by Mg objects themselves after use. 
Almost Mg interfaces require explicit calls to its DestoryXXX methods (e.g. void vkDestroyInstance ) to dispose any memory associated to the objects.

2. Magnesium does not SPECIFIC a shader language of choice, rather relying on the shader language available to the platform itself. 

3. To benefit more multi-threaded application use in Vulkan/Magnesium, it is recommended to restrict instances of command pools (i.e. IMgCommandPool) and descriptor pools (i.e. IMgDescriptorPool) to a single thread.
For example, in application that uses 2 thread, each thread would have its own descriptor pool(s) and command pool(s) and none of the instances should be shared between those threads.          

4. (TODO) You will not directly modify/set for any indirect draw call buffer data as the structs used for indirect draws.
Therefore a indirect interface class must be created act as a bridge to allow improve portability.   
	
	
----------

## LICENSE

Licensed under MIT license

----------

## WHY?

Scenario 1: 
You want to implement a cross-platform C#/.net 2D/3D application with  graphics API/library that uses modern high-performance features such as 

 - compute shaders
 - tessellation shaders
 - immutable storage
 - compiled shader bytecode
 - constant buffers/shader storage buffer objects
 - upfront memory allocation
 - general purpose memory
 - multi-indirect instanced draws

However, the choice of the graphics API is dependant on the platform itself.

1. DirectX 11/12: Available for Windows 10 and Xbox. No support for MacOS or Linux (double check?).
2. OpenGL and OpenGL ES: Available on all platforms.  Yet Mac devices are limited to OpenGL 4.1. 
3. [Vulkan](https://khronosgroup.com):  a next generation low level graphics library designed with respect to multi-threading devices. Now available on Windows, Android N devices and Linux. Unofficial MacOS driver support available via [MoltenVK]()
4. Metal: Apple's own graphics rendering library that has modern features such as compute shaders.

Vulkan is *almost* the correct choice for a modern graphics library because there is native driver support for Windows, Linux and Android N platforms. 

So Magnesium is primarily a C#/.net interface for Vulkan (for Windows). 

However, Vulkan itself is not available or arriving on a number of platforms (e.g Apple) and leaves a number of the devices  i.e. iPad, iPhone, pre Android-N devices and even HTML 5 browsers unsupported.

**Here** is where Magnesium comes to the rescue; it is an interface of Vulkan.  We can use dependency injection (i.e. BYO C# IOC container not MEF) to load a polyfill instead into your applications if native Vulkan support is not available.

| Interface | Platforms |
|-----------|-----------|
| Magnesium  | Core C# Vulkan interface with all applications should reference |
| Magnesium.Vulkan | C# interface to Vulkan version 1.0.17 |
| Magnesium.Metal | iOS polyfill for Magnesium for iOS devices using Apple's own Metal API |
| Magnesium.OpenGL | OpenGL enumation/polyfill to replicate most of Vulkan entities and features |
| Magnesium.OpenGL.DesktopGL | OpenTK / OpenGL 4.5 implementation |
| Magnesium.OpenGL.AndroidGL | OpenTK / Xamarin Android / OpenGL ES 2.0, 3.0, 3.1 implementations |

To reach a wider number of platforms, all implementations have compiled as portable core library profile 111. Maybe moving to .NET standard 2.0 when released.
To reduce recompilation, Magnesium libraries are compiled with minimal preprocessor defines

 - Most debug assemblies only use DEBUG

A exception is Magnesium.Metal which uses UNIFIED for both debug and release assemblies as Xamarin assemblies that are compiled with UNIFIED can be both for iOS and MacOS applications.

----------

## CROSS PLATFORM DEV GUIDE

### IMgShaderModule 

Magnesium does not SPECIFIC a shader language of choice, rather relying on the shader language available to the platform itself.
 
By passing a System.IO.Stream handle contain some data and its length in bytes into IMgDevice.CreateShaderModule, Magnesium and its implementation can cater to any shader file type.  

```C#
public class MgShaderModuleCreateInfo
{
	public Stream Code { get; set; }
	public UIntPtr CodeSize { get; set; }
}
```

| Platform | Description | File extension |
|----------|-------------|----------------|
| Magnesium.Vulkan | SPIRV bytecode code | |
| Magnesium.OpenGL | GLSL source code | .glsl, .frag, .vert |
| Magnesium.Metal  | Metal source code | .metal |

OPENGL: Since METAL does not allow "main" as a function entrypoint in MgPipelineStageCreateInfo.Name,  
OpenGL GLSL shaders should not include ```void main()``` function section in the source code. To work around GLSL inability to specify a shader name,
the Magnesium.OpenGL implementation will append the ```void main() { <shader_name>() }``` dynamically.   

```C#
public class MgPipelineStageCreateInfo
{
	public MgPipelineStageFlagBits Stage { get; set; }
	public string Name { get; set; }	
}

```

VULKAN: Since METAL does not allow *"main"* as a function entrypoint in MgPipelineStageCreateInfo.Name, please use another 
entrypoint other than *"main"*. See [SPIRVReplace](https://github.com/tgsstdio/SPIRVReplace) CLI to convert glsl to valid SPIRV bytecode on Windows. 

METAL: final Position.y should be flipped in the vertex shader

OPENGL: gl_Position.y should be flipped in the vertex shader 	

```GLSL
	gl_Position = vec4(in_position.x, -in_position.y, 0, 1);
```

### Compute

METAL: compute pipelines need to specify the dimensions of the workgroup therefore you must specify ThreadsPerWorkgroup when calling IMgDevice.CreateComputePipelines.

```C#
public class MgComputePipelineCreateInfo
{
	public MgPipelineCreateFlagBits Flags { get; set; }
	public MgPipelineShaderStageCreateInfo Stage { get; set; }
	public IMgPipelineLayout Layout { get; set; }
	// REQUIRED FOR METAL
	public MgVec3Ui ThreadsPerWorkgroup { get; set;}
	public IMgPipeline BasePipelineHandle { get; set; }
	public Int32 BasePipelineIndex { get; set; }
}
``` 


### Descriptor Sets

OPENGL: Only one of the descriptor sets can be bound.  
METAL: Only one of the descriptor sets can be bound. Internally, descriptor sets, vertex buffers, samplers and textures are combined together within the shaders.       

----------

## PLATFORM SUMMARY


| Platform | API     | Availability | Caveats |
|----------|---------|--------------|---------|
| Windows  | Vulkan  |  Yes         | Driver support | 
|          | OpenGL  |  Yes         | Driver support |
|          | DirectX |  Yes         |                |
| Android  | Vulkan  |  Yes*        | Only for Android N devices and above |
|          | OpenGL  |  Yes*        | Widespread OpenGL ES 2.0+ availability, Limited devices using OpenGL ES 3.0 or 3.1.               |
| MacOS	   | Vulkan  |  Yes*         | No for official Apple driver, potential emulation via MoltenVK |
|          | OpenGL  | Yes          | Limited to OpenGL 4.1 |
|          | Metal   | Yes          | Most iOS devices with El Capitan |
| Linux  | Vulkan | Yes | Driver support, Specific windowing API  |
|        | OpenGL | Yes | Driver support |
| HTML 5 | Vulkan | No  | No direct support for Chrome, Safari or Firefox browsers  | 
|        | OpenGL | Yes | Widespread OpenGL ES 2.0+ availability, Limited devices using OpenGL ES 3.0 or 3.1 |


----------

## IMPLEMENTATIONS TODOs

| Features | Magnesium.Vulkan | Magnesium.Metal | Magnesium.OpenGL.DesktopGL |
|----------|------------------|-----------------|----------------------------|
| Draw arrays  | Yes  |  Yes  | Yes  |
| Draw indexed  | Yes | Yes | Yes |
| Indirect draws  | Yes, partial | Yes, partial  | Yes, partial  |
| Indirect draw indexed |  Yes, partial   | Yes, partial  | Yes, partial  |
| Graphics Pipelines | Yes, Completed  | Yes, Completed | No |
| Secondary Command Buffers  | Yes, Completed  | No | No |
| Compute Pipelines | Yes, Completed  | Yes, Completed | No |
| Query Pools  | Yes, Completed | No | No |
| Pipeline caches  | Yes, Completed | No | No |
| CmdUpdateDescriptorSets (Writes)  | Yes, Completed | Yes, Completed | No |
| CmdUpdateDescriptorSets (Copies)  | Yes, Completed | No | No |
| Subpasses  | Yes, Completed | Yes, Partial | No |
| Framebuffers  | Yes, Completed | Yes, Partial | No |

> Written with [StackEdit](https://stackedit.io/) & Visual Studio Code