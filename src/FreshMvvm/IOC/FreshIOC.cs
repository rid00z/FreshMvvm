using FreshMvvm.IOC;

namespace FreshMvvm.IoC
{
    public class FreshIoC
    {
        static FreshIoC()
        {
        }

        static IFreshIoC _freshIoCContainer;

        public static IFreshIoC Container => _freshIoCContainer ?? (_freshIoCContainer = new FreshTinyIoCBuiltIn());

        public static void OverrideContainer(IFreshIoC overrideContainer)
        {
            _freshIoCContainer = overrideContainer;
        }
    }
}

