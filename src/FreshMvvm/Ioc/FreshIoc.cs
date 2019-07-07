using FreshMvvm;

namespace FreshMvvm.Ioc
{
    public class FreshIoc
    {
        static IFreshIoc _freshIocContainer;

        public static IFreshIoc Container { 
            get {
                if (_freshIocContainer == null)
                    _freshIocContainer = new FreshTinyIocBuiltIn ();

                return _freshIocContainer;
            } 
        }

        public static void OverrideContainer(IFreshIoc overrideContainer)
        {
            _freshIocContainer = overrideContainer;
        }
    }
}

