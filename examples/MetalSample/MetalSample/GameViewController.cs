using System;
using System.Diagnostics;

using AppKit;
using Magnesium;
using Magnesium.Metal;
using Metal;
using MetalKit;
using SimpleInjector;

namespace MetalSample
{
	public partial class GameViewController : NSViewController, IMTKViewDelegate
	{
		// view
		MTKView mApplicationView;

		// meshes
		private Container mContainer;

		IMgGraphicsConfiguration mGraphicsConfiguration = null;

		SpinningCube mApplication;

		MgDriverContext mDriver;

		private bool InitialiseMg()
		{
			try
			{
				// SINGLETONS HERE
				var localDevice = MTLDevice.SystemDefault;

				if (localDevice == null)
				{
					Console.WriteLine("Metal is not supported on this device");
					return false;
				}

				// APPLICATION HERE
				mContainer.Register<SpinningCube>(Lifestyle.Singleton);

				RegisterMagnesiumSingletons(localDevice);

				RegisterMagnesiumMappings();

				// RESOLVE HERE 

				mDriver = mContainer.GetInstance<MgDriverContext>();

				var err = mDriver.Initialize(
					new Magnesium.MgApplicationInfo
					{
						ApplicationName = "MetalSample",
						EngineName = "Magnesium",
						ApplicationVersion = 1,
						EngineVersion = 1,
						ApiVersion = Magnesium.MgApplicationInfo.GenerateApiVersion(1, 0, 17),
					},
				  	Magnesium.MgInstanceExtensionOptions.ALL
			 	);

				if (err != Result.SUCCESS)
				{
					throw new InvalidOperationException("Metal is not supported on this device : " + err);
				}

				mGraphicsConfiguration = mContainer.GetInstance<IMgGraphicsConfiguration>();

				mApplication = mContainer.GetInstance<SpinningCube>();

				// BIND TO VIEW
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

			mContainer.Register<Magnesium.IMgPresentationSurface,
				Magnesium.Metal.AmtPresentationSurface>(Lifestyle.Singleton);
			mContainer.Register<Magnesium.Metal.IAmtMetalLibraryLoader,
				Magnesium.Metal.AmtMetalTextSourceLibraryLoader>(Lifestyle.Singleton);
			mContainer.Register<Magnesium.IMgPresentationBarrierEntrypoint,
				Magnesium.Metal.AmtPresentationBarrierEntrypoint>(Lifestyle.Singleton);
			mContainer.Register<Magnesium.IMgPresentationLayer,
				Magnesium.MgPresentationLayer>(Lifestyle.Singleton);
            mContainer.Register<Magnesium.IMgGraphicsDeviceContext,
                Magnesium.Metal.AmtGraphicsDeviceContext>(Lifestyle.Singleton);

            mContainer.Register<Magnesium.Metal.IAmtDeviceEntrypoint,
                AmtDefaultDeviceEntrypoint>(Lifestyle.Singleton);
            mContainer.Register<Magnesium.Metal.IAmtFenceEntrypoint,
                AmtFenceEntrypoint>(Lifestyle.Singleton);
            mContainer.Register<Magnesium.Metal.IAmtSemaphoreEntrypoint,
                AmtSemaphoreEntrypoint>(Lifestyle.Singleton);

            mContainer.Register<
                Magnesium.Metal.IAmtPhysicalDeviceFormatLookupEntrypoint,
                Magnesium.Metal.AmtDefaultPhysicalDeviceFormatLookupEntrypoint>
                (Lifestyle.Singleton);

            mContainer.Register<Magnesium.IMgGraphicsDevice,
                Magnesium.MgDefaultGraphicsDevice>(Lifestyle.Singleton);

            //mContainer.Register<Magnesium.IMgGraphicsDevice,
                //Magnesium.Metal.AmtGraphicsDevice>(Lifestyle.Singleton);
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
		}

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

			if (InitialiseMg())
			{
				Debug.Assert(mApplication != null);

				mApplication.Reshape((uint) View.Bounds.Size.Width , (uint) View.Bounds.Size.Height);
				mApplication.Update();
			}
			else {
				//Console.WriteLine("Metal is not supported on this device");
				View = new NSView(View.Frame);
			}
		}

		public void DrawableSizeWillChange(MTKView view, CoreGraphics.CGSize size)
		{
			if (mApplication != null)
			{
				mApplication.Reshape((uint) size.Width, (uint) size.Height);
			}
		}

		public void Draw(MTKView view)
		{
			if (mApplication != null)
			{
				mApplication.Render();
			}
		}

	}
}

