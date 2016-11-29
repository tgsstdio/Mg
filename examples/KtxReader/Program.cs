using Magnesium.Ktx;
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
                    container.Register<IKTXTextureLoader, KTXTextureManager>();

                    IKTXTextureLoader loader = container.GetInstance<IKTXTextureLoader>();
                    using (var fs = System.IO.File.OpenRead("1.ktx"))
                    {
                        var result = loader.Load(fs);
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
