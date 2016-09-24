using Metal;

namespace Magnesium.Metal
{
	class AmtSampler : IMgSampler
	{
		IMTLDevice mDevice;
		MgSamplerCreateInfo pCreateInfo;

		public AmtSampler(IMTLDevice mDevice, MgSamplerCreateInfo pCreateInfo)
		{
			this.mDevice = mDevice;
			this.pCreateInfo = pCreateInfo;
		}
	}
}