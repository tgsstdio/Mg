# Magnesium

Four words to describe Magnesium:

> C# Vulkan interface/polyfill

Quick Links

- [CHANGELOG](#CHANGELOG)
- [LICENSE](#LICENSE)
- [WHY?](#WHY?)
- [PLATFORM SUMMARY](#PLATFORM-SUMMARY)


----------


## CHANGELOG

Magnesium and its implementations uses [Semantic Versioning v2.0.0](http://semver.org/) for versioning assemblies

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
		- Metal iOS API

### Version 3.0.0
 - Breaking changes from classes to interfaces due to Magnesium.Vulkan implementation 

### Version 2.0.0
 - Breaking changes to function

### Version 1.0.0

 - Now using Semantic Versioning v2.0.0 (http://semver.org/)

----------

##LICENSE

Licensed under MIT license

----------

##WHY?

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


##PLATFORM SUMMARY


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

> Written with [StackEdit](https://stackedit.io/)