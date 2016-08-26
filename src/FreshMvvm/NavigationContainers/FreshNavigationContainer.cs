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

        internal Page CreateContainerPageSafe (Page page)
        {
            if (page is NavigationPage || page is MasterDetailPage || page is TabbedPage)
                return page;

            return CreateContainerPage(page);
        }

        protected virtual Page CreateContainerPage (Page page)
        {
            return new NavigationPage (page);
        }

		public virtual Task PushPage (Xamarin.Forms.Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            if (modal)
                return Navigation.PushModalAsync (CreateContainerPageSafe (page), animate);
            return Navigation.PushAsync (page, animate);
        }

		public virtual Task PopPage (bool modal = false, bool animate = true)
        {
            if (modal)
                return Navigation.PopModalAsync (animate);
            return Navigation.PopAsync (animate);
        }

        public Task PopToRoot (bool animate = true)
        {
            return Navigation.PopToRootAsync (animate); 
        }

        public string NavigationServiceName { get; private set; }

        public void NotifyChildrenPageWasPopped()
        {
            this.NotifyAllChildrenPopped();
        }

        public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
        {
            throw new Exception("This navigation container has no selected roots, just a single root");
        }
    }
}

