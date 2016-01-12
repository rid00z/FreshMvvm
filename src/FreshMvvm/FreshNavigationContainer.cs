using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FreshMvvm
{
    public class FreshNavigationContainer : Xamarin.Forms.NavigationPage, IFreshNavigationService
    {
        public FreshNavigationContainer (Page page) 
            : this (page, Constants.DefaultNavigationServiceName)
        {            
        }

        public FreshNavigationContainer (Page page, string navigationPageName) 
            : base (page)
        {
            var pageModel = page.GetModel ();
            pageModel.CurrentNavigationServiceName = navigationPageName;
            NavigationServiceName = navigationPageName;
            RegisterNavigation ();
        }

        protected void RegisterNavigation ()
        {
            FreshIOC.Container.Register<IFreshNavigationService> (this, NavigationServiceName);
        }

        protected virtual Page CreateContainerPage (Page page)
        {
            if (page is NavigationPage || page is MasterDetailPage || page is TabbedPage)
                return page;
            
            return new NavigationPage (page);
        }

		public async virtual Task PushPage (Xamarin.Forms.Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            if (modal)
				await Navigation.PushModalAsync (CreateContainerPage (page), animate);
            else
				await Navigation.PushAsync (page, animate);
        }

		public async virtual Task PopPage (bool modal = false, bool animate = true)
        {
            if (modal)
				await Navigation.PopModalAsync (animate);
            else
				await Navigation.PopAsync (animate);
        }

        public async Task PopToRoot (bool animate = true)
        {
            await Navigation.PopToRootAsync (animate); 
        }

        public string NavigationServiceName { get; private set; }
    }
}

