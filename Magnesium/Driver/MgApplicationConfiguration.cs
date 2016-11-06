using System;

namespace Magnesium
{
    public class MgApplicationConfiguration : IDisposable
    {
        private readonly MgDriver mDriver;
        public MgDriver Driver
        {
            get
            {
                return mDriver;
            }
        }


        public MgApplicationConfiguration(MgDriver driver, MgApplicationConfigurationInfo createInfo)
        {
            mDriver = driver;
            var errorCode = mDriver.Initialize(
                new MgApplicationInfo
                {
                    ApplicationName = createInfo.ApplicationName,
                    EngineName = "Magnesium",
                    ApplicationVersion = createInfo.Version,
                    EngineVersion = 1,
                    ApiVersion = MgApplicationInfo.GenerateApiVersion(1, 0, 17),
                },
                  MgInstanceExtensionOptions.ALL
             );

            if (errorCode != Result.SUCCESS)
            {
                throw new InvalidOperationException("MgGraphicsConfiguration error : " + errorCode);
            }
        }

        ~MgApplicationConfiguration()
        {
            Dispose(false);
        }

        private bool mIsDisposed = false;
        protected void Dispose(bool v)
        {
            if (mIsDisposed)
                return;

            mDriver.Dispose();

            mIsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
