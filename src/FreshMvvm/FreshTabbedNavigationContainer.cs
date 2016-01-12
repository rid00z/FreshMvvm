using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreshMvvm
{
    public class FreshTabbedNavigationContainer : TabbedPage, IFreshNavigationService
    {
        List<Page> _tabs = new List<Page>();
        public IEnumerable<Page> TabbedPages { get { return _tabs; } }

        public FreshTabbedNavigationContainer () : this(Constants.DefaultNavigationServiceName)
        {				
            
        }

        public FreshTabbedNavigationContainer(string navigationServiceName)
        {
            NavigationServiceName = navigationServiceName;
            RegisterNavigation ();
        }

        protected void RegisterNavigation ()
        {
            FreshIOC.Container.Register<IFreshNavigationService> (this, NavigationServiceName);
        }

        public virtual Page AddTab<T> (string title, string icon, object data = null) where T : FreshBasePageModel
        {
            var page = FreshPageModelResolver.ResolvePageModel<T> (data);
            page.GetModel ().CurrentNavigationServiceName = NavigationServiceName;
            _tabs.Add (page);
            var navigationContainer = CreateContainerPage (page);
            navigationContainer.Title = title;
            if (!string.IsNullOrWhiteSpace(icon))
                navigationContainer.Icon = icon;
            Children.Add (navigationContainer);
            return navigationContainer;
        }

        protected virtual Page CreateContainerPage (Page page)
        {
            if (page is NavigationPage || page is MasterDetailPage || page is TabbedPage)
                return page;
            
            return new NavigationPage (page);
        }

		public async System.Threading.Tasks.Task PushPage (Xamarin.Forms.Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            if (modal)
                await this.CurrentPage.Navigation.PushModalAsync (CreateContainerPage (page));
            else
                await this.CurrentPage.Navigation.PushAsync (page);
        }

		public async System.Threading.Tasks.Task PopPage (bool modal = false, bool animate = true)
        {
            if (modal)
                await this.CurrentPage.Navigation.PopModalAsync (animate);
            else
                await this.CurrentPage.Navigation.PopAsync (animate);
        }

        public async Task PopToRoot (bool animate = true)
        {
            await this.CurrentPage.Navigation.PopToRootAsync (animate);
        }

        public string NavigationServiceName { get; private set; }
    }
}

