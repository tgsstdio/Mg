using System;
using System.Diagnostics;

using AppKit;
using Magnesium;
using Magnesium.Metal;
using Metal;
using MetalKit;
using SimpleInjector;

namespace TriangleDemo.MetalMac
{
	public partial class GameViewController : NSViewController, IMTKViewDelegate
	{
		// view
		MTKView mApplicationView;

		// meshes
		private Container mContainer;

		IMgGraphicsConfiguration mGraphicsConfiguration = null;

		VulkanExample mApplication;

		MgDriverContext mDriver;

		public GameViewController(IntPtr handle) : base(handle)
		{
		}

		~GameViewController()
		{
			if (mApplication != null)
			{
				mApplication.Dispose();
			}

			if (mGraphicsConfiguration != null)
			{
				mGraphicsConfiguration.Dispose();
			}

			if (mDriver != null)
			{
				mDriver.Dispose();
			}

			if (mContainer != null)
				mContainer.Dispose();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			mContainer = new Container();
			RegisterApplication();
			if (InitialiseMg(out mApplication))
			{
				Debug.Assert(mApplication != null);
				//mApplication.RenderLoop();
			}
			else {
				//Console.WriteLine("Metal is not supported on this device");
				View = new NSView(View.Frame);
			}
		}

		void RegisterApplication()
		{
			// APPLICATION HERE
			mContainer.Register<VulkanExample>(Lifestyle.Singleton);
			mContainer.Register<ITriangleDemoShaderPath, MetalTriangleDemoShaderPath>(Lifestyle.Singleton);

		}

		#region InitialiseMg methods

		private bool InitialiseMg<TApplication>(out TApplication app)
			where TApplication : class
		{
			try
			{
				// SINGLETONS HERE
				var localDevice = MTLDevice.SystemDefault;

				if (localDevice == null)
				{
					app = null;
					Console.WriteLine("Metal is not supported on this device");
					return false;
				}

				RegisterMagnesiumSingletons(localDevice);

				RegisterMagnesiumMappings();

				// RESOLVE HERE 

				mDriver = mContainer.GetInstance<MgDriverContext>();

				var err = mDriver.Initialize(
					new Magnesium.MgApplicationInfo
					{
						ApplicationName = "TriangleDemo",
						EngineName = "Magnesium",
						ApplicationVersion = 1,
						EngineVersion = 1,
						ApiVersion = Magnesium.MgApplicationInfo.GenerateApiVersion(1, 0, 17),
					},
				  	Magnesium.MgInstanceExtensionOptions.ALL
			 	);

				mGraphicsConfiguration = mContainer.GetInstance<IMgGraphicsConfiguration>();

				app = mContainer.GetInstance<TApplication>();

				// BIND TO VIEW AFTER APPLICATION INITIALISED
				mApplicationView.Delegate = this;
				mApplicationView.Device = localDevice;



				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw ex;
			}
		}

		void RegisterMagnesiumMappings()
		{
			Debug.Assert(mContainer != null);

			mContainer.Register<Magnesium.MgDriverContext>(Lifestyle.Singleton);
			mContainer.Register<Magnesium.IMgGraphicsConfiguration,
				Magnesium.MgDefaultGraphicsConfiguration>(Lifestyle.Singleton);


			mContainer.Register<Magnesium.IMgEntrypoint,
				Magnesium.Metal.AmtEntrypoint>(Lifestyle.Singleton);
			mContainer.Register<Magnesium.IMgSwapchainCollection,
				Magnesium.Metal.AmtSwapchainCollection>(Lifestyle.Singleton);
			mContainer.Register<Magnesium.IMgGraphicsDevice,
				Magnesium.Metal.AmtGraphicsDevice>(Lifestyle.Singleton);
			mContainer.Register<Magnesium.IMgPresentationSurface,
				Magnesium.Metal.AmtPresentationSurface>(Lifestyle.Singleton);
			mContainer.Register<Magnesium.Metal.IAmtMetalLibraryLoader,
				Magnesium.Metal.AmtMetalTextSourceLibraryLoader>(Lifestyle.Singleton);
			mContainer.Register<Magnesium.IMgPresentationBarrierEntrypoint,
				Magnesium.Metal.AmtPresentationBarrierEntrypoint>(Lifestyle.Singleton);
			mContainer.Register<Magnesium.IMgPresentationLayer,
				Magnesium.MgPresentationLayer>(Lifestyle.Singleton);

			mContainer.Register <Magnesium.Metal.IAmtSemaphoreEntrypoint, 
				Magnesium.Metal.AmtSemaphoreEntrypoint>(Lifestyle.Singleton);
			mContainer.Register<Magnesium.Metal.IAmtFenceEntrypoint,
				Magnesium.Metal.AmtFenceEntrypoint>(Lifestyle.Singleton);
			mContainer.Register<Magnesium.Metal.IAmtDeviceEntrypoint,
				Magnesium.Metal.AmtDefaultDeviceEntrypoint>(Lifestyle.Singleton);

		}

		void RegisterMagnesiumSingletons(IMTLDevice localDevice)
		{
			Debug.Assert(mContainer != null);

			// METAL SPECIFIC
			var deviceQuery = new AmtDeviceQuery { NoOfCommandBufferSlots = 5 };
			mContainer.RegisterSingleton<Magnesium.Metal.IAmtDeviceQuery>(deviceQuery);
			mContainer.RegisterSingleton<IMTLDevice>(localDevice);

			mApplicationView = (MTKView)View;
			mContainer.RegisterSingleton<MTKView>(mApplicationView);

			var query = new MacTriangleDemoDisplayInfo
			{
				Color = MgFormat.B8G8R8A8_UNORM,
				Depth = MgFormat.D32_SFLOAT_S8_UINT,
			};
			mContainer.RegisterSingleton<ITriangleDemoDisplayInfo>(query);
		}

		#endregion

		public void DrawableSizeWillChange(MTKView view, CoreGraphics.CGSize size)
		{
			if (mApplication != null)
			{
				//mApplication.Reshape((uint)size.Width, (uint)size.Height);
			}
		}

		public void Draw(MTKView view)
		{
			try
			{
				if (mApplication != null)
				{
					mApplication.RenderLoop();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
