using FreshTinyIoC;

namespace FreshMvvm
{
    public class FreshIOC
    {
        static FreshIOC ()
        {
        }

        static IFreshIOC _freshIOCContainer;

        public static IFreshIOC Container { 
            get {
                if (_freshIOCContainer == null)
                    _freshIOCContainer = new FreshTinyIOCBuiltIn ();

                return _freshIOCContainer;
            } 
        }

        public static void OverrideContainer(IFreshIOC overrideContainer)
        {
            _freshIOCContainer = overrideContainer;
        }
    }
}

