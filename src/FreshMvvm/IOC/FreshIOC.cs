using FreshMvvm;

namespace FreshMvvm.Ioc
{
    public class FreshIoc
    {
        static IFreshIoc _freshIOCContainer;

        public static IFreshIoc Container { 
            get {
                if (_freshIOCContainer == null)
                    _freshIOCContainer = new FreshTinyIocBuiltIn ();

                return _freshIOCContainer;
            } 
        }

        public static void OverrideContainer(IFreshIoc overrideContainer)
        {
            _freshIOCContainer = overrideContainer;
        }
    }
}

