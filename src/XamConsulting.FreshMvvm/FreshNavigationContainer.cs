using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FreshMvvm
{
    public class FreshNavigationContainer : Xamarin.Forms.NavigationPage, IFreshNavigationService
    {
        public FreshNavigationContainer (Page page) : base (page)
        {
            RegisterNavigation ();
        }

        protected void RegisterNavigation ()
        {
            FreshIOC.Container.Register<IFreshNavigationService> (this);
        }

        protected virtual Page CreateContainerPage (Page page)
        {
            return new NavigationPage (page);
        }

        public async virtual Task PushPage (Xamarin.Forms.Page page, FreshBasePageModel model, bool modal = false)
        {
            if (modal)
                await Navigation.PushModalAsync (CreateContainerPage (page));
            else
                await Navigation.PushAsync (page);
        }

        public async virtual Task PopPage (bool modal = false)
        {
            if (modal)
                await Navigation.PopModalAsync ();
            else
                await Navigation.PopAsync ();
        }
    }
}

